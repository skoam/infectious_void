using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnlyWhenPlayerDead : MonoBehaviour {

    public SpriteRenderer spriteRenderer;

    public float speedIn = 1;
    public float speedOut = 6;

    public float maximumAlpha;
    public float minimumAlpha;
    
    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	void Update () {
        if (!ManagesPlayer.instance.isAlive()) {
            if (spriteRenderer.color.a < maximumAlpha) {
                spriteRenderer.color += new Color(0, 0, 0, speedIn * Time.deltaTime);
            }
        } else {
            if (spriteRenderer.color.a > minimumAlpha) {
                spriteRenderer.color -= new Color(0, 0, 0, speedOut * Time.deltaTime);
            }
        }
	}
}
