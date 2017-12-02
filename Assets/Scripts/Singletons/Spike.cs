using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {
    public float frequency;
    public float height;
    public float speedIn;
    public float speedOut;

    public float outDelay;

    public float startDelay;
    public bool randomStartDelay;

    private float timer;
    private float delay;

    private float offsetY;

    void Start () {
        offsetY = this.transform.position.y;

        if (randomStartDelay) {
            startDelay = Random.Range(0, startDelay);
        }

        delay = startDelay;
    }

    void Update () {
        if (delay > 0) {
            delay -= Time.deltaTime;
            return;
        } else {
            delay = 0;
        }

        timer += Time.deltaTime;

        if (timer > frequency) {
            if (this.transform.position.y - offsetY < 0) {
                this.transform.position += new Vector3(0, speedIn, 0) * speedOut * Time.deltaTime;
            } else {
                timer = 0;
                delay = outDelay;
            }
        } else {
            if (this.transform.position.y - offsetY > -height) {
                this.transform.position += new Vector3(0, -speedOut, 0) * Time.deltaTime;
            }
        }
    }
}
