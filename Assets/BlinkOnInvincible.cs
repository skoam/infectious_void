using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkOnInvincible : MonoBehaviour {

    private SpriteRenderer spriteRenderer;

    private bool fadeIn;

    public float speed;

    public float minimumAlpha = 0;
    public float maximumAlpha = 1;

    public bool defaultIsMinimum;

    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

	void Update () {
        if (ManagesPlayer.instance.isInvincible()) {
            if (!fadeIn) {
                if (spriteRenderer.color.a > minimumAlpha) {
                    spriteRenderer.color -= new Color(0, 0, 0, speed * Time.deltaTime);
                } else {
                    fadeIn = true;
                }
            } else {
                if (spriteRenderer.color.a < maximumAlpha) {
                    spriteRenderer.color += new Color(0, 0, 0, speed * Time.deltaTime);
                } else {
                    fadeIn = false;
                }
            }
        } else {
            if (defaultIsMinimum) {
                if (spriteRenderer.color.a > minimumAlpha) {
                    spriteRenderer.color -= new Color(0, 0, 0, speed * Time.deltaTime);
                }
            } else {
                if (spriteRenderer.color.a < maximumAlpha) {
                    spriteRenderer.color += new Color(0, 0, 0, speed * Time.deltaTime);
                }
            }
        }
	}
}
