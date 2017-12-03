using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSortingLayer : MonoBehaviour {

    public string SortingLayer = "Default";
    public int OrderInLayer = 0;

    void Awake () {
        gameObject.GetComponent<MeshRenderer>().sortingLayerName = SortingLayer;
        gameObject.GetComponent<MeshRenderer>().sortingOrder = OrderInLayer;
    }

}
