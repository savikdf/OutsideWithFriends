using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerTagController : MonoBehaviour {
    public float reach = 3.0f;
    public GameObject left, right;
    // Start is called before the first frame update
    private string fire1 = "Fire1", fire2 = "Fire2";
    void Start() {
    }

    void Initialize(){
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetButtonDown(fire1)){
            left.transform.position += (left.transform.forward * reach);
        }
        if(Input.GetButtonUp(fire1)){
            left.transform.position -= (left.transform.forward * reach);
        }
        if(Input.GetButtonDown(fire2)){
            right.transform.position += (right.transform.forward * reach);
        } 
        if(Input.GetButtonUp(fire2)){
            right.transform.position -= (right.transform.forward * reach);
        }
    }
    public void HandleCollision(Collision collision, Vector3 direction){
        if(collision.gameObject.GetComponent<Rigidbody>() != null){
            collision.gameObject.GetComponent<Rigidbody>().AddForce(
                        direction * 25, ForceMode.Impulse);
        }
    }
}
