using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {
    public bool requireTransformed = true;
    public bool locked;
    public float interactionDistance = 2.0f;

    public bool ignoreTextTuple;
    public TextMesh[] interactionTextTuple;

    public bool hitBoxTriggered;
    public bool automaticTriggerOnDistance;

    private bool forceActivation;
    private bool activated;

    public Animator[] activateAnimator;
    public Animator[] deactivateAnimator;
    public Collider2D[] activateCollider;
    public Collider2D[] deactivateCollider;
    public ParticleSystem[] emitParticles;
    public int[] emissionAmounts;
    public ParticleSystem[] activateParticleSystems;
    public ParticleSystem[] deactivateParticleSystems;
    public GameObject[] activateGameObject;
    public GameObject[] deactivateGameObject;
    public SpriteRenderer[] activateRenderer;
    public SpriteRenderer[] deactivateRenderer;
    public InteractableObject[] permanentlyDisableInteractableObject;

    public bool teleportPlayer;
    public Transform teleportPosition;

    public bool transformPlayer;
    public bool transformationValue;

    public int increaseIllness = 0;
    public int decreaseIllness = 0;
    public int decreaseHealth = 0;
    public int increaseHealth = 0;

    public bool isSaveGame;

    public bool doNotRestore;

    private Vector3 originalPosition;

    public AudioClip activationSound;
    public float soundLevel;

	// Use this for initialization
	void Start () {
		originalPosition = this.transform.position;
	}

    public bool wasActivated () {
        return activated;
    }

    public void hit () {
        if (hitBoxTriggered) {
            forceActivation = true;
        }
    }

    public void restore () {
        if (doNotRestore) {
            return;
        }

        foreach (Animator animator in activateAnimator) {
            animator.SetBool("activated", false);
        }

        foreach (Animator animator in deactivateAnimator) {
            animator.SetBool("activated", false);
        }

        foreach (Collider2D collider in activateCollider) {
            collider.enabled = false;
        }

        foreach (Collider2D collider in deactivateCollider) {
            collider.enabled = true;
        }

        for (int i = 0; i < activateParticleSystems.Length; i++) {
            particleEmission(activateParticleSystems[i], false);
        }
        
        for (int i = 0; i < deactivateParticleSystems.Length; i++) {
            particleEmission(deactivateParticleSystems[i], true);
        }
        
        for (int i = 0; i < activateGameObject.Length; i++) {
            activateGameObject[i].SetActive(false);
        }

        for (int i = 0; i < deactivateGameObject.Length; i++) {
            deactivateGameObject[i].SetActive(true);
        }

        for (int i = 0; i < activateRenderer.Length; i++) {
            activateRenderer[i].enabled = false;
        }

        for (int i = 0; i < deactivateRenderer.Length; i++) {
            deactivateRenderer[i].enabled = true;
        }

        if (transformPlayer) {
            ManagesPlayer.instance.values.transformed = !transformationValue;
        }

        this.transform.position = originalPosition;

        forceActivation = false;
        activated = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!ManagesGame.instance.updatesAllowed()) {
            return;
        }

        if (requireTransformed && !ManagesPlayer.instance.values.transformed) {
            return;
        }
        
        if (!activated && Vector3.Distance(ManagesPlayer.instance.getPlayer().transform.position, this.transform.position) < interactionDistance) {
            if (automaticTriggerOnDistance) {
                forceActivation = true;
            }

            if (!ignoreTextTuple) {
                foreach (TextMesh interactionText in interactionTextTuple) {
                    if (interactionText.color.a < 1) {
                        interactionText.color = interactionText.color + new Color(0, 0, 0, 2 * Time.deltaTime);
                    }
                }
            }
        } else {
            if (!ignoreTextTuple) {
                foreach (TextMesh interactionText in interactionTextTuple) {
                    if (interactionText.color.a > 0) {
                        interactionText.color = interactionText.color - new Color(0, 0, 0, 2 * Time.deltaTime);
                    }
                }
            }
            
            if (!forceActivation) {
                return;
            }
        }

        if (locked) {
            return;
        }

        if (!activated && (forceActivation || (!hitBoxTriggered && Input.GetAxis("Interact") > 0))) {
            foreach (Animator animator in activateAnimator) {
                animator.SetBool("activated", true);
            }

            foreach (Animator animator in deactivateAnimator) {
                animator.SetBool("activated", false);
            }

            foreach (Collider2D collider in activateCollider) {
                collider.enabled = true;
            }

            foreach (Collider2D collider in deactivateCollider) {
                collider.enabled = false;
            }

            for (int i = 0; i < activateParticleSystems.Length; i++) {
                particleEmission(activateParticleSystems[i], true);
            }
            
            for (int i = 0; i < deactivateParticleSystems.Length; i++) {
                particleEmission(deactivateParticleSystems[i], false);
            }

            for (int i = 0; i < emitParticles.Length; i++) {
                emitParticles[i].Emit(emissionAmounts[i]);
            }

            for (int i = 0; i < activateGameObject.Length; i++) {
                activateGameObject[i].SetActive(true);
            }

            for (int i = 0; i < deactivateGameObject.Length; i++) {
                deactivateGameObject[i].SetActive(false);
            }

            for (int i = 0; i < activateRenderer.Length; i++) {
                activateRenderer[i].enabled = true;
            }

            for (int i = 0; i < deactivateRenderer.Length; i++) {
                deactivateRenderer[i].enabled = false;
            }

            for (int i = 0; i < permanentlyDisableInteractableObject.Length; i++) {
                permanentlyDisableInteractableObject[i].doNotRestore = true;
            }

            if (transformPlayer) {
                ManagesPlayer.instance.values.transformed = transformationValue;
            }

            if (teleportPlayer) {
                ManagesPlayer.instance.container.transform.position = teleportPosition.position;
            }
            
            ManagesPlayer.instance.values.illness += increaseIllness;
            if (ManagesPlayer.instance.values.illness > ManagesPlayer.instance.values.maxIllness) {
                ManagesPlayer.instance.values.illness = ManagesPlayer.instance.values.maxIllness;
            }

            ManagesPlayer.instance.values.illness -= decreaseIllness;
            if (ManagesPlayer.instance.values.illness < 0) {
                ManagesPlayer.instance.values.illness = 0;
            }
            
            ManagesPlayer.instance.values.health -= decreaseHealth;
            if (ManagesPlayer.instance.values.health < 0) {
                ManagesPlayer.instance.values.health = 0;
            }

            ManagesPlayer.instance.values.health += increaseHealth;
            if (ManagesPlayer.instance.values.health > ManagesPlayer.instance.values.maxHealth) {
                ManagesPlayer.instance.values.health = ManagesPlayer.instance.values.maxHealth;
            }

            if (isSaveGame) {
                ManagesGame.instance.lastSaveGame = this.transform;
            }
            
            if (activationSound != null) {
                ManagesGame.instance.playSound(activationSound, soundLevel);
            }

            activated = true;
        }
	}

    public void particleEmission(ParticleSystem PS, bool newVal) {
        ParticleSystem.EmissionModule em = PS.emission;
        em.enabled = newVal;
    }
}
