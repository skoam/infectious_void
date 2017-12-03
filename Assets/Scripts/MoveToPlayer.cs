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
    private float attackCheckDelay;

    public float minimumDistance;
    public float maximumDistance;

    public float minimumAttackDistance;

    public float[] attackLengths;
    public int currentAttack = 0;

    public float afterAttackFreeze = 0.5f;
    public float afterAttackPause = 0.5f;

    public bool attackRandomly;
    public int attackStrength = 1;

    [Range(0, 100)]
    public float randomAttackChance;

    private SpriteRenderer sprite;
    private Animator animator;
    private Collider2D hitbox;

    private float randomDirectionInterval = 1;
    private float randomDirectionTimer = 0;

    private int randomDirection = 1;

    void Awake () {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start () {
        hitbox = GameObject.Instantiate(ManagesGame.instance.hitBoxTemplate, ManagesGame.instance.hitBoxRoot).GetComponent<Collider2D>();
    }

    protected override void SendAnimatorData () {
    }

    protected override void ComputeVelocity () {
        if (hitbox == null) {
            return;
        }

        if (slashDuration != 0 && !ManagesPlayer.instance.settings.canAttackWhileMoving) {
            return;
        }

        if (walkDelay > 0) {
            return;
        }

        Vector2 move = Vector2.zero;
        currentAttack = 0;

        randomDirectionTimer += Time.deltaTime;

        if (randomDirectionTimer > randomDirectionInterval) {
            randomDirection = Random.Range(-1, 2);
            
            if (randomDirection != 1) {
                if (Random.Range(0, 100) > 10) {
                    randomDirection = 1;
                }
            }

            randomDirectionTimer = 0;
        }

        float distance = Vector3.Distance(this.transform.position, ManagesPlayer.instance.getPlayer().transform.position);
        if (distance  > minimumDistance && distance < maximumDistance) {
            move.x = ManagesPlayer.instance.getPlayer().transform.position.x < this.transform.position.x ? -maxSpeed * randomDirection : maxSpeed * randomDirection;
        }

        if (randomDirection == 0 || distance > maximumDistance) {
            if (ManagesPlayer.instance.getPlayer().transform.position.x < this.transform.position.x) {
                sprite.flipX = true;
            } else {
                sprite.flipX = false;
            }
        } else {
            bool flipSprite = (sprite.flipX ? (move.x > 0.01f) : (move.x < -0.01f));

            if (flipSprite) {
                sprite.flipX = !sprite.flipX;
            }
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
        if (hitbox == null) {
            return;
        }

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

        if (slashDuration > 0 && attackCheckDelay == 0) {
            int count = hitbox.Cast(Vector2.zero, attackContactFilter, attackHitBuffer);

            for (int i = 0; i < count; i++) {
                if (attackHitBuffer[i].transform.gameObject.layer == 14 || attackHitBuffer[i].transform.gameObject.layer == 9) {
                    if (attackHitBuffer[i].transform.gameObject != this.transform.gameObject) {
                        InteractableObject interaction = attackHitBuffer[i].transform.gameObject.GetComponent<InteractableObject>();
                        
                        if (interaction != null) {
                            interaction.hit();
                        }

                        if (attackHitBuffer[i].transform.gameObject.layer == 9) {
                            ManagesPlayer.instance.receiveDamage(attackStrength, sprite.flipX ? -0.5f : 0.5f);
                        }
                    }
                }
            }
        }

        if (attackCheckDelay > 0) {
            attackCheckDelay -= Time.deltaTime;
        } else {
            attackCheckDelay = 0;
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

        if (!ManagesPlayer.instance.isAlive()) {
            return;
        }

        if (GetComponent<InteractableObject>() != null && GetComponent<InteractableObject>().wasActivated()) {
            return;
        }

        float distance = Vector3.Distance(this.transform.position, ManagesPlayer.instance.getPlayer().transform.position);
        if (distance > minimumAttackDistance) {
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

            attackCheckDelay = slashDuration / 4;
            
            if (currentAttack + 1 < attackLengths.Length) {
                currentAttack++;
            } else {
                currentAttack = 0;
            }
        }
    }
}
