using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class animateImageUI : MonoBehaviour {
  public Sprite[] images;

  public float timeBetweenSteps = 0.1f;
  public int startImage;

  private Image image;
  private int current;

  void Start () {
    image = this.GetComponent<Image>();
    image.sprite = images[startImage];
    current = startImage;
  }

  private float timeOver = 0;
  void FixedUpdate () {
    timeOver += 1 * Time.deltaTime;

    if (timeOver >= timeBetweenSteps) {
      UpdateStep();
      timeOver = 0;
    }
  }

  void UpdateStep () {
    if (current < images.Length - 1) {
      current++;
    } else {
      current = 0;
    }

    image.sprite = images[current];
  }
}
