using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableByHealthThreshold : MonoBehaviour {

    private Image image;

    public int threshold;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (ManagesPlayer.instance.values.health < threshold) {
            image.enabled = false;
        } else {
            image.enabled = true;
        }
	}
}
