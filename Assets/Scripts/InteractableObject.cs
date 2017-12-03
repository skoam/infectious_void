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

    private bool activatedByHitBox;
    private bool activated;

    public Animator[] activateAnimator;
    public Animator[] deactivateAnimator;
    public Collider2D[] activateCollider;
    public Collider2D[] deactivateCollider;
    public ParticleSystem[] emitParticles;
    public int[] emissionAmounts;
    public GameObject[] activateGameObject;
    public GameObject[] deactivateGameObject;
    public SpriteRenderer[] activateRenderer;
    public SpriteRenderer[] deactivateRenderer;
    public bool transformPlayer;
    public bool transformationValue;

	// Use this for initialization
	void Start () {
		
	}

    public void hit () {
        if (hitBoxTriggered) {
            activatedByHitBox = true;
        }
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
            
            if (!activatedByHitBox) {
                return;
            }
        }

        if (locked) {
            return;
        }

        if (!activated && (activatedByHitBox || (!hitBoxTriggered && Input.GetAxis("Interact") > 0))) {
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

            if (transformPlayer) {
                ManagesPlayer.instance.values.transformed = transformationValue;
            }

            activated = true;
        }
	}
}
