using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

    private float currentSpeed;
    public float minSpeed = 0.1f;
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    // Use this for initialization
    void Awake () {
        // spriteRenderer = GetComponent<SpriteRenderer> (); 
        // animator = GetComponent<Animator> ();
    }

    protected override void SendAnimatorData () {
        // animator.SetFloat("velocity_y", velocity.y);
    }

    protected override void ComputeVelocity () {
        Vector2 move = Vector2.zero;

        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f) {
            move.x = Input.GetAxis ("Horizontal");
        }

        /*if (Input.GetButtonDown ("Jump") && grounded) {
            velocity.y = jumpTakeOffSpeed;
        } else if (Input.GetButtonUp ("Jump")) {
            if (velocity.y > 0) {
                velocity.y = velocity.y * 0.5f;
            }
        }*/

        bool flipSprite = (ManagesPlayer.instance.getSprite().flipX ? (move.x > 0.01f) : (move.x < -0.01f));

        if (flipSprite) {
            ManagesPlayer.instance.getSprite().flipX = !ManagesPlayer.instance.getSprite().flipX;
        }

        ManagesPlayer.instance.getAnimator().SetBool ("grounded", grounded);
        ManagesPlayer.instance.getAnimator().SetFloat ("velocity_x", Mathf.Abs (velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }
}
