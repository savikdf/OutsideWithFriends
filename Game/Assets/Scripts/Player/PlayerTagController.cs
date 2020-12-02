using System.Collections;
using UnityEngine;

public class PlayerTagController : MonoBehaviour {
    public float reach = 3.0f;
    public GameObject left, right;
    private IInputManager input;
    void Start() {
        StartCoroutine(HandleInput());

    }
    void Awake(){
        input = GameManger.gameManger.InputManager;
    }
    IEnumerator HandleInput(){
        while(true){
            if(input != null){
                //Debug.Log("NOT NULL.");
                if(input.GetInput().fire1 == state.UP){
                    left.transform.position -= (left.transform.forward * reach);
                } else if (input.GetInput().fire1 == state.DOWN){
                    left.transform.position += (left.transform.forward * reach);;
                }
                if(input.GetInput().fire1 == state.NONE){
                }

                if(input.GetInput().fire2 == state.UP){
                    right.transform.position -= (right.transform.forward * reach);
                } else if (input.GetInput().fire2 == state.DOWN){
                    right.transform.position += (right.transform.forward * reach);;
                }
                if(input.GetInput().fire2 == state.NONE){
                }
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
