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
    public BoxCollider2D hitbox_attack;
    public GameObject illness;

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
	
	void Update () {
        if (values.transformed) {
            humanRepresentation.GetComponent<SpriteRenderer>().enabled = false;
            representation.GetComponent<SpriteRenderer>().enabled = true;
        } else {
            humanRepresentation.GetComponent<SpriteRenderer>().enabled = true;
            representation.GetComponent<SpriteRenderer>().enabled = false;
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

    public SpriteRenderer getSprite () {
        if (values.transformed) {
            return representation.GetComponent<SpriteRenderer>();
        } else {
            return humanRepresentation.GetComponent<SpriteRenderer>();
        }
    }

    public BoxCollider2D getHitBox () {
        return hitbox_attack;
    }

    public ParticleSystem getIllness () {
        return illness.GetComponent<ParticleSystem>();
    }

    public bool transformed () {
        return values.transformed;
    }
}
