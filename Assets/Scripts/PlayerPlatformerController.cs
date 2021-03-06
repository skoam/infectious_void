﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

    private float currentSpeed;
    public float minSpeed = 0.1f;
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    
    RaycastHit2D[] attackHitBuffer = new RaycastHit2D[16];

    public ContactFilter2D attackContactFilter;

    private float slashDuration;
    private bool slash;

    private float walkDelay;
    private float attackCheckDelay;

    void Awake () {
    }

    protected override void SendAnimatorData () {
    }

    protected override void ComputeVelocity () {
        if (!ManagesGame.instance.playerInteractionAllowed()) {
            return;
        }

        if (slashDuration != 0 && !ManagesPlayer.instance.settings.canAttackWhileMoving) {
            return;
        }

        if (walkDelay > 0) {
            return;
        }

        Vector2 move = Vector2.zero;

        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f) {
            ManagesPlayer.instance.settings.currentSlash = 0;
            move.x = Input.GetAxis ("Horizontal");
        }

        bool flipSprite = (ManagesPlayer.instance.getSprite().flipX ? (move.x > 0.01f) : (move.x < -0.01f));

        if (flipSprite) {
            ManagesPlayer.instance.getSprite().flipX = !ManagesPlayer.instance.getSprite().flipX;
        }

        if (ManagesPlayer.instance.getSprite().flipX) {
            ManagesPlayer.instance.getHitBox().offset = new Vector2(-0.5f, 0);
        } else {
            ManagesPlayer.instance.getHitBox().offset = new Vector2(0.5f, 0);
        }

        ManagesPlayer.instance.getHitBox().transform.position = this.transform.position;

        ManagesPlayer.instance.getAnimator().SetBool ("grounded", grounded);
        ManagesPlayer.instance.getAnimator().SetFloat ("velocity_x", Mathf.Abs (velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }

    protected override void OnUpdate () {
        if (slashDuration > 0) {
            slashDuration -= Time.deltaTime;
        } else {
            if (slashDuration < 0) {
                ManagesPlayer.instance.getAnimator().SetBool("slash", false);
                walkDelay = ManagesPlayer.instance.settings.afterAttackFreeze;
            }

            slashDuration = 0;
        }

        if (slashDuration > 0 && attackCheckDelay == 0) {
            int count = ManagesPlayer.instance.getHitBox().Cast(Vector2.zero, attackContactFilter, attackHitBuffer);

            for (int i = 0; i < count; i++) {
                if (attackHitBuffer[i].transform.gameObject.layer == 14) {
                    InteractableObject interaction = attackHitBuffer[i].transform.gameObject.GetComponent<InteractableObject>();
                    
                    if (interaction != null) {
                        interaction.hit();
                    }
                }
            }
        }

        if (attackCheckDelay > 0) {
            attackCheckDelay -= Time.deltaTime;
        } else {
            attackCheckDelay = 0;
        }


        if (walkDelay > 0) {
            walkDelay -= Time.deltaTime;
        } else {
            walkDelay = 0;
        }

        if (ManagesPlayer.instance.transformed() && ManagesPlayer.instance.isAlive()) {
            if (slashDuration == 0 && Input.GetAxis("Attack") > 0) {

                ParticleSystem.ShapeModule shape = ManagesPlayer.instance.getIllness().shape;

                if (ManagesPlayer.instance.values.illness == 1) {
                    shape.randomPositionAmount = 1;
                } else if (ManagesPlayer.instance.values.illness == 2) {
                    shape.randomPositionAmount = 1.2f;
                } else if (ManagesPlayer.instance.values.illness == 3) {
                    shape.randomPositionAmount = 1.5f;
                } else if (ManagesPlayer.instance.values.illness == 4) {
                    shape.randomPositionAmount = 2;
                }

                walkDelay = 0;
                ManagesPlayer.instance.getHitBox().transform.position = this.transform.position;

                int currentSlash = ManagesPlayer.instance.settings.currentSlash;

                ManagesPlayer.instance.getAnimator().SetBool("slash", true);
                ManagesPlayer.instance.getAnimator().SetInteger("slashType",
                    currentSlash);

                 if (currentSlash == 0) {
                    ManagesGame.instance.playSound(ManagesGame.instance.slash_2, 0.4f);
                } else {
                    ManagesGame.instance.playSound(ManagesGame.instance.slash_3, 0.4f);
                }
               
                slashDuration = 0;
                slashDuration += ManagesPlayer.instance.settings.slashDuration[currentSlash];

                attackCheckDelay = slashDuration / 4;
                
                if (currentSlash + 1 < ManagesPlayer.instance.settings.slashDuration.Length) {
                    ManagesPlayer.instance.settings.currentSlash++;
                } else {
                    ManagesPlayer.instance.settings.currentSlash = 0;
                }
            }
        }
    }
}
