using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public Vector3 boostVec = new Vector3(0,50f,0);
    public bool useLocalRotationAsUp = false;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            FirstPersonController fpc = other.GetComponent<FirstPersonController>();
            if (fpc != null)
            {
                fpc.ApplyBoost(boostVec);
                Debug.Log("Applying boost on " + other.gameObject.name);
            }
        }        
    }
}
