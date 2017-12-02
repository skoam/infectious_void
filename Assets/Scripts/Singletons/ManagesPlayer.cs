using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagesPlayer : MonoBehaviour {
    public static ManagesPlayer instance;

    public PlayerSettings settings;
    public PlayerValues values;

    private Animator playerAnimator;
    private SpriteRenderer playerSprite;
        
    public GameObject container;
    public GameObject representation;
    public GameObject humanRepresentation;

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

    public bool transformed () {
        return values.transformed;
    }
}
