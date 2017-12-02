using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

    private float currentSpeed;
    public float minSpeed = 0.1f;
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    
    private float slashDuration;
    private bool slash;

    private float walkDelay;

    void Awake () {
    }

    protected override void SendAnimatorData () {
    }

    protected override void ComputeVelocity () {
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

        if (walkDelay > 0) {
            walkDelay -= Time.deltaTime;
        } else {
            walkDelay = 0;
        }

        if (ManagesPlayer.instance.transformed()) {
            if (slashDuration == 0 && Input.GetAxis("Attack") > 0) {
                walkDelay = 0;

                int currentSlash = ManagesPlayer.instance.settings.currentSlash;

                ManagesPlayer.instance.getAnimator().SetBool("slash", true);
                ManagesPlayer.instance.getAnimator().SetInteger("slashType",
                    currentSlash);
                
                slashDuration = 0;
                slashDuration += ManagesPlayer.instance.settings.slashDuration[currentSlash];
                
                if (currentSlash + 1 < ManagesPlayer.instance.settings.slashDuration.Length) {
                    ManagesPlayer.instance.settings.currentSlash++;
                } else {
                    ManagesPlayer.instance.settings.currentSlash = 0;
                }
            }
        }
    }
}
