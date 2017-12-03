using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectPowerUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Collected!");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
