using UnityEngine;
using System;
using System.Collections;

public class CopyLocation : MonoBehaviour {
    public Transform target;
    public bool X;
    public float offsetX;
    public float minimumX;
    public float maximumX;
    public bool smoothX;
    public bool Y;
    public float offsetY;
    public float minimumY;
    public float maximumY;
    public bool smoothY;
    public bool Z;
    public float offsetZ;
    public float minimumZ;
    public float maximumZ;
    public bool smoothZ;

    public float smoothingTime = 0.5f;
    
    public bool accelerateByDistance;
    public float accelerationFactor = 0.01f;
    public float minimumDistance = 2;
    
    public bool nonFixed;

    private Vector3 velocity;
    private float timeReduction;

    public bool forceFix;
    private bool forceSmooth;

    public float xShakeOffset;

    private void Start() {
        // QualitySettings.vSyncCount = 0;  // VSync must be disabled
        // Application.targetFrameRate = 30;
    } 

    void FixedUpdate () {
        if (!nonFixed) {
            UpdateStep();
        }
    }

    void Update () {
        if (nonFixed) {
            UpdateStep();
        }
    }

    void UpdateStep () {
        timeReduction = 0;
        forceFix = false;
        forceSmooth = false;

        if (Mathf.Abs(xShakeOffset) > 0.1f) {
            if (xShakeOffset < 0) {
                xShakeOffset += 2 * Time.deltaTime;
            } else {
                xShakeOffset -= 2 * Time.deltaTime;
            }
        } else {
            xShakeOffset = 0;
        }

        Vector3 myPos = this.transform.position;
        Vector3 newPos = new Vector3(target.position.x + offsetX + xShakeOffset, target.position.y + offsetY, target.position.z + offsetZ);

        newPos.y = 0;

        newPos.x = Mathf.Clamp(newPos.x, minimumX, maximumX);
        newPos.y = Mathf.Clamp(newPos.y, minimumY, maximumY);
        newPos.z = Mathf.Clamp(newPos.z, minimumZ, maximumZ);
        
        if (accelerateByDistance && Vector3.Distance(transform.position, newPos) > minimumDistance) {
            timeReduction = Vector3.Distance(transform.position, newPos) / 10;
            timeReduction *= accelerationFactor;
        }

        if (Mathf.Abs(xShakeOffset) > 0) {
            timeReduction = smoothingTime - 0.1f;
        }

        timeReduction = Mathf.Clamp(timeReduction, 0, smoothingTime - 0.1f);

        float calculatedSmoothingTime = (float)System.Math.Round(smoothingTime - timeReduction, 2);
        
        if (!forceFix) {
            this.transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, calculatedSmoothingTime, Mathf.Infinity, Time.fixedDeltaTime);
        }

        if (X) {
            if (!smoothX || forceFix && !forceSmooth) {
                transform.position = new Vector3(newPos.x, transform.position.y, transform.position.z);
            }
        }

        if (Y) {
            if (!smoothY || forceFix && !forceSmooth) {
                transform.position = new Vector3(transform.position.x, newPos.y, transform.position.z);
            }
        }

        if (Z) {
            if (!smoothZ || forceFix && !forceSmooth) {
                transform.position = new Vector3(transform.position.x, transform.position.y, newPos.z);
            }
        }

    }
}
