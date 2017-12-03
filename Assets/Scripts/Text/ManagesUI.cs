using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagesUI : MonoBehaviour {
    public static ManagesUI instance;

    void Awake () {
        if (instance != null) {
            Destroy(this.transform.gameObject);
            return;
        }

        instance = this;
    }

    public GameObject UICanvas;
    public GameObject UITextTemplate;

    public void addUIText (string text, Vector3 position, float interactableDistance = 0) {
        GameObject UIText = GameObject.Instantiate(UITextTemplate, position, Quaternion.identity);
        TextMesh textMesh = UIText.GetComponent<TextMesh>();
        textMesh.text = text;
        
        if (interactableDistance != 0) {
            FadeInTextMesh fader = UIText.GetComponent<FadeInTextMesh>();
            fader.distance = interactableDistance;
            fader.fadeOutOnDistance = true;
            fader.removeOnDistance = true;
        }
    }

    public void removeUIText () {

    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
