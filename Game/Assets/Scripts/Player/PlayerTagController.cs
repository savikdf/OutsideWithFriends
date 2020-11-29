using System.Collections;
using UnityEngine;

public class PlayerTagController : MonoBehaviour {
    public float reach = 3.0f;
    public GameObject left, right;
    // Start is called before the first frame update
    private string fire1 = "Fire1", fire2 = "Fire2";
    private bool leftActive = false, rightActive = false;
    void Start() {
        StartCoroutine(HandleInput());
    }
    IEnumerator HandleInput(){
        while(true){
            if(Input.GetButtonDown(fire1) && !leftActive){
                left.transform.position += (left.transform.forward * reach);
                leftActive = true;
            }
            if(Input.GetButtonUp(fire1) && leftActive){
                left.transform.position -= (left.transform.forward * reach);
                leftActive = false;
            }
            if(Input.GetButtonDown(fire2) && !rightActive){
                right.transform.position += (right.transform.forward * reach);
                rightActive = true;
            } 
            if(Input.GetButtonUp(fire2) && rightActive){
                right.transform.position -= (right.transform.forward * reach);
                rightActive = false;
            }
            yield return null;
        }
    }
    public void HandleCollision(Collision collision, Vector3 direction){
        if(collision.gameObject.GetComponent<Rigidbody>() != null){
            collision.gameObject.GetComponent<Rigidbody>().AddForce(
                        direction * 25, ForceMode.Impulse);
        }
    }
}
