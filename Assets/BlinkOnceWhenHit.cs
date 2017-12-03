using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkOnceWhenHit : MonoBehaviour {
    SpriteRenderer spriteRenderer;

    public float minimumAlpha = 0;
    public float maximumAlpha = 1;

    public float speed;

    public bool defaultIsMinimum;

    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public void blink () {
        Color color = spriteRenderer.color;

        if (defaultIsMinimum) {
            color.a = maximumAlpha;
        } else {
            color.a = minimumAlpha;
        }

        spriteRenderer.color = color; 
    }

    void Update () {
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
