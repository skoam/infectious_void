using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : PhysicsObject {
    private float currentSpeed;
    public float minSpeed = 0.1f;
    public float maxSpeed = 2;
    
    RaycastHit2D[] attackHitBuffer = new RaycastHit2D[16];

    public ContactFilter2D attackContactFilter;

    private float slashDuration;
    private bool slash;

    private float walkDelay;
    private float attackDelay;

    public float minimumDistance;
    public float maximumDistance;

    public float[] attackLengths;
    public int currentAttack = 0;

    public float afterAttackFreeze = 0.5f;
    public float afterAttackPause = 0.5f;

    public bool attackRandomly;

    [Range(0, 100)]
    public float randomAttackChance;

    private SpriteRenderer sprite;
    private Animator animator;
    public Collider2D hitbox;

    void Awake () {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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

        float distance = Vector3.Distance(this.transform.position, ManagesPlayer.instance.getPlayer().transform.position);
        if (distance  > minimumDistance && distance < maximumDistance) {
            move.x = ManagesPlayer.instance.getPlayer().transform.position.x < this.transform.position.x ? -maxSpeed : maxSpeed;
        }

        currentAttack = 0;

        bool flipSprite = (sprite.flipX ? (move.x > 0.01f) : (move.x < -0.01f));

        if (flipSprite) {
            sprite.flipX = !sprite.flipX;
        }

        if (sprite.flipX) {
            hitbox.offset = new Vector2(-0.5f, 0);
        } else {
            hitbox.offset = new Vector2(0.5f, 0);
        }

        hitbox.transform.position = this.transform.position;

        animator.SetBool ("grounded", grounded);
        animator.SetFloat ("velocity_x", Mathf.Abs (velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }

    protected override void OnUpdate () {
        if (slashDuration > 0) {
            slashDuration -= Time.deltaTime;
        } else {
            if (slashDuration < 0) {
                animator.SetBool("slash", false);
                walkDelay = afterAttackFreeze;
                attackDelay = afterAttackPause;
            }

            slashDuration = 0;
        }

        if (slashDuration > 0) {
            int count = hitbox.Cast(Vector2.zero, attackContactFilter, attackHitBuffer);

            for (int i = 0; i < count; i++) {
                if (attackHitBuffer[i].transform.gameObject.layer == 14 || attackHitBuffer[i].transform.gameObject.layer == 9) {
                    if (attackHitBuffer[i].transform.gameObject != this.transform.gameObject) {
                        InteractableObject interaction = attackHitBuffer[i].transform.gameObject.GetComponent<InteractableObject>();
                        
                        if (interaction != null) {
                            interaction.hit();
                        }
                    }
                }
            }
        }

        if (attackDelay > 0) {
            attackDelay -= Time.deltaTime;
        } else {
            attackDelay = 0;
        }

        if (walkDelay > 0) {
            walkDelay -= Time.deltaTime;
        } else {
            walkDelay = 0;
        }

        float distance = Vector3.Distance(this.transform.position, ManagesPlayer.instance.getPlayer().transform.position);
        if (distance < minimumDistance || distance > maximumDistance) {
            return;
        }

        float attack = Random.Range(0, 100);

        if (attackRandomly && attack < randomAttackChance && slashDuration == 0 && attackDelay == 0) {
            walkDelay = 0;

            hitbox.transform.position = this.transform.position;

            animator.SetBool("slash", true);
            animator.SetInteger("slashType",
                currentAttack);
            
            slashDuration = 0;
            slashDuration += attackLengths[currentAttack];
            
            if (currentAttack + 1 < attackLengths.Length) {
                currentAttack++;
            } else {
                currentAttack = 0;
            }
        }
    }
}
