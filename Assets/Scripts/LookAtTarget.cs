using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour {
    public Transform target;
    private Quaternion rotation;

    public bool ignoreY;
    public bool ignoreX;
    public bool ignoreZ;

    public bool lookAtCamera;

    public bool reverse;
    public string searchTarget;

    void Start() {
        if (lookAtCamera) {
            // target = 
        }

        if (searchTarget != "") {
            target = GameObject.FindGameObjectWithTag(searchTarget).transform;
        }
    }

    void LateUpdate () {
        if (reverse) {
            this.transform.LookAt(2 * this.transform.position - target.position);
        } else {
            this.transform.LookAt(target);
        }

        rotation = this.transform.rotation;

        if (ignoreY) {
            rotation.y = 0;
        }

        if (ignoreX) {
            rotation.x = 0;
        }

        if (ignoreZ) {
            rotation.z = 0;
        }

        this.transform.rotation = rotation;
    }
}
