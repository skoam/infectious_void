using UnityEngine;
using System.Collections;

public class rotatePowerUp : MonoBehaviour {
    private Vector3 movement;

    public float movementAmount = 0.2f;
    public float movementSpeed = 2.0f;
    public float rotationAmount = 1.0f;

    public bool randomizeMovementAmount;

    float currentDirection = 1;

    float currentVelocity = 0.0f;

    void Start () {
        if (randomizeMovementAmount) {
            movementAmount = Random.Range(0, movementAmount);
        }
    }

    private float updateTime = 0.01f;
    private float timeOver = 0;
    void Update () {
        timeOver += 1 * Time.deltaTime;

        if (timeOver >= updateTime) {
            UpdateStep();
            timeOver = 0;
        }
    }

    void UpdateStep () {
        currentVelocity += currentDirection * 0.1f * Time.deltaTime;
        currentVelocity = Mathf.Clamp (currentVelocity, -movementAmount, movementAmount);

        if (Mathf.Abs (currentVelocity) == movementAmount) {
            currentDirection *= -1;
        }

        movement = new Vector3(0.0f, currentVelocity, 0.0f) * Time.deltaTime * movementSpeed;
        movement.y = Mathf.Clamp (movement.y, -movementAmount, movementAmount);

        this.transform.position = this.transform.position + movement;
        this.transform.RotateAround (this.transform.position, this.transform.position, rotationAmount);
    }
}
