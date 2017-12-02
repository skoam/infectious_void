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

	void Start () {
		
	}
	
	void Update () {
		
	}
}
