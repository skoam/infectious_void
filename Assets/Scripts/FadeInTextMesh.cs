using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInTextMesh : MonoBehaviour {
    public float easeInSpeed = 1;
    public float maxAlpha = 1;

    private TextMesh textMesh;

    private bool done;

    public bool fadeOutOnDistance;
    public bool removeOnDistance;
    public float distance;
    
    void Awake () {
        textMesh = GetComponent<TextMesh>();
    }
	
	// Update is called once per frame
	void Update () {
        if (fadeOutOnDistance || removeOnDistance) {
            if (Vector3.Distance(ManagesPlayer.instance.getPlayer().transform.position, this.transform.position) > distance) {
                done = true;

                if (fadeOutOnDistance && textMesh.color.a > 0) {
                    textMesh.color = textMesh.color - new Color(0, 0, 0, easeInSpeed * Time.deltaTime);
                } else if (removeOnDistance) {
                    Destroy(this.transform.gameObject);
                }
            }
        }
        
        if (done) {
            return;
        }

        if (textMesh.color.a < 1) {
            textMesh.color = textMesh.color + new Color(0, 0, 0, easeInSpeed * Time.deltaTime);
        } else {
            done = true;
        }
    }
}
