using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTagController : MonoBehaviour {

    public float reach = 3.0f;
    public GameObject left, right;
    private Vector3 leftOgPos, rightOgPos;
    // Start is called before the first frame update
    void Start() {
    }

    void Initialize(){
        leftOgPos = left.transform.position;
        rightOgPos = right.transform.position;
    }

    // Update is called once per frame
    void Update() {

        if(Input.GetButton("Fire1")){
            Vector3 leftPos = left.transform.position;
            left.transform.position = new Vector3(
                leftPos.x, 
                leftPos.y, 
                leftPos.z
            ) + (transform.forward * reach);
        }
        if(Input.GetButton("Fire2")){
            Vector3 rightPos = left.transform.position;
            left.transform.position = new Vector3(
                rightPos.x, 
                rightPos.y, 
                rightPos.z
            ) + (transform.forward * reach);
        }

        // reset position
        left.transform.position = leftOgPos;
        right.transform.position = rightOgPos;
    }
}
