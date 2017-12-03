using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagesGame : MonoBehaviour {
    public static ManagesGame instance;

    void Awake () {
        if (instance != null) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    public Transform hitBoxRoot;
    public Transform hitBoxTemplate;

    public GameObject mainCamera;

    public BlinkOnceWhenHit onHitEffect;

    public Transform lastSaveGame;

	void Start () {
		
	}
	
	void Update () {
        if (!ManagesPlayer.instance.isAlive()) {
            if (Input.GetAxis("Interact") > 0) {
                ManagesPlayer.instance.respawn();

                InteractableObject[] interactableObjects = GameObject.FindObjectsOfType<InteractableObject>();

                for (int i = 0; i < interactableObjects.Length; i++) {
                    interactableObjects[i].restore();
                }
            }
        }
	}

    public bool updatesAllowed () {
        bool allowed = true;
        return allowed;
    }

    public bool playerInteractionAllowed () {
        bool allowed = true;

        if (!ManagesPlayer.instance.isAlive()) {
            allowed = false;
        }

        return allowed;
    }
}
