using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTransitioner : MonoBehaviour
{
    public string buttonName { get; set; }
    public int toIndex;
    [HideInInspector]
    public int fromIndex;

    void Awake()
    {
        buttonName = gameObject.name;
    }
}
