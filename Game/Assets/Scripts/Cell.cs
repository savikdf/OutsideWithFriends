using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Cell : MonoBehaviour
{
    int cellSize { get; set; }

    TextMesh dangerLevelText { get; set; }
    BoxCollider boxCollider { get; set; }

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        dangerLevelText = GetComponentInChildren<TextMesh>();

        if(boxCollider == null || dangerLevelText == null)
        {
            throw new System.NullReferenceException("BoxCollider and TextMesh are needed for a cell to function");
        }
    }

    void RevealDanger(string count) {
        dangerLevelText.text = count;
    }


}
