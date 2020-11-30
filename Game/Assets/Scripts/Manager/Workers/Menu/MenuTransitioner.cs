using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTransitioner : MonoBehaviour
{
    public string buttonName { get; set; }
    public int childMenuIndex;
    public int parentMenuIndex;

    void Awake()
    {
        buttonName = gameObject.name;
    }
}
