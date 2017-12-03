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

	void Start () {
		
	}
	
	void Update () {
		
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
