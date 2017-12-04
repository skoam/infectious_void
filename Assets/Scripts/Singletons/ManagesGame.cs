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

    private bool dead;

    public AudioClip aaah;
    public AudioClip death;
    public AudioClip dissolve;
    public AudioClip slash_1;
    public AudioClip slash_2;
    public AudioClip slash_3;
    public AudioClip _switch;

    public AudioSource FXSource;

    private float respawnDelay;

	void Start () {
		
	}

    public void playSound (AudioClip sound, float level) {
        if (FXSource != null) {
            FXSource.PlayOneShot(sound, level);
        }
    }
	
	void Update () {
        if (respawnDelay > 0) {
            respawnDelay -= Time.deltaTime;
        } else {
            respawnDelay = 0;
        }

        if (!ManagesPlayer.instance.isAlive()) {
            if (!dead) {
                playSound(aaah, 0.6f);
                playSound(death, 0.3f);
                Input.ResetInputAxes();
                dead = true;
                respawnDelay = 1;
            }

            if (respawnDelay == 0 && Input.GetAxis("Interact") > 0) {
                dead = false;

                ManagesPlayer.instance.respawn();
                
                if (lastSaveGame != null) {
                    InteractableObject[] interactableObjects = GameObject.FindObjectsOfType<InteractableObject>();

                    for (int i = 0; i < interactableObjects.Length; i++) {
                        interactableObjects[i].restore();
                    }

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
