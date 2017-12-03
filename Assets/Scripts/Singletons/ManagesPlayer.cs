using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagesPlayer : MonoBehaviour {
    public static ManagesPlayer instance;

    public PlayerSettings settings;
    public PlayerValues values;

    public GameObject container;
    public GameObject representation;
    public GameObject humanRepresentation;
    Collider2D hitbox;
    public GameObject illness;

    public float invincibilitySecondsOnHit = 2;
    private float invincible;

    private ParticleSystem.EmissionModule illnessEmission;

    void Awake () {
        if (instance != null) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

	void Start () {
		
	}

    public bool isAlive () {
        return values.health > 0 && values.illness < 4;
    }
	
	void Update () {
        if (invincible > 0) {
            invincible -= Time.deltaTime;
        } else {
            invincible = 0;
        }

        if (values.transformed) {
            humanRepresentation.GetComponent<SpriteRenderer>().enabled = false;
            representation.GetComponent<SpriteRenderer>().enabled = true;
        } else {
            humanRepresentation.GetComponent<SpriteRenderer>().enabled = true;
            representation.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (!isAlive()) {
            getAnimator().SetBool("dead", true);
        } else {
            getAnimator().SetBool("dead", false);
        }
	}

    public Animator getAnimator () {
        if (values.transformed) {
            return representation.GetComponent<Animator>();
        } else {
            return humanRepresentation.GetComponent<Animator>();
        }
    }
    
    public Rigidbody2D getBody () {
        return container.GetComponent<Rigidbody2D>();
    }

    public GameObject getPlayer () {
         if (values.transformed) {
            return representation;
        } else {
            return humanRepresentation;
        }
    }

    public SpriteRenderer getSprite () {
        if (values.transformed) {
            return representation.GetComponent<SpriteRenderer>();
        } else {
            return humanRepresentation.GetComponent<SpriteRenderer>();
        }
    }

    public Collider2D getHitBox () {
        if (hitbox != null) {
            return hitbox;
        } else {
            hitbox = GameObject.Instantiate(ManagesGame.instance.hitBoxTemplate, ManagesGame.instance.hitBoxRoot).GetComponent<Collider2D>();
            return hitbox;
        }
    }

    public ParticleSystem getIllness () {
        return illness.GetComponent<ParticleSystem>();
    }

    public bool transformed () {
        return values.transformed;
    }

    public void receiveDamage (int amount) {
        if (invincible == 0) {
            values.health -= amount;
            invincible = invincibilitySecondsOnHit;
        }
    }
}
