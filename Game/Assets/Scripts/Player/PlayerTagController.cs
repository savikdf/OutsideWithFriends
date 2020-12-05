using System.Collections;
using UnityEngine;
using Utils;

[RequireComponent(typeof(PlayerAudioController))]
public class PlayerTagController : MonoBehaviour {
    private PlayerAudioController audioController;
    public float reach = 3.0f;
    public GameObject left, right;
    private IInputManager input;
    private double leftTimeStamp, leftTimeDifference;
    private double rightTimeStamp, rightTimeDifference;
    public double timeInterval = 0.05;
    private bool leftTrigger, rightTrigger;
    void Awake(){
        audioController = GetComponent<PlayerAudioController>();
        StartCoroutine(HandleInput());
        input = GameManger.gameManger.InputManager;
    }
    IEnumerator HandleInput(){
        while(true){
            if (input != null){

            // left hand
            HandleHand(
                left, 
                input.GetInput().fire1, 
                ref leftTrigger, 
                ref leftTimeStamp, 
                ref leftTimeDifference);

            // right hand
            HandleHand(
                right, 
                input.GetInput().fire2, 
                ref rightTrigger, 
                ref rightTimeStamp, 
                ref rightTimeDifference);  
            }
     
            yield return null;
        }
    }

    void HandleHand(GameObject hand, state fire, ref bool trigger, ref double timeStamp, ref double timeDifference){
        Vector3 extend = hand.transform.position;
        if(input != null){
            //Debug.Log("NOT NULL.");
            if(fire == state.DOWN && !trigger){
                audioController.PlayPlayerClip(PlayerAudioClips.Fire);
                timeStamp = Time.time;
                trigger = true;
                extend += hand.transform.forward * reach;
            }
            if(trigger){
                timeDifference = Time.time - timeStamp;
                if(timeDifference > timeInterval){
                    trigger = false;
                        extend -= hand.transform.forward * reach;
                }
            }
            hand.transform.position = extend;
        }
    }

    public void HandleCollision(Collision collision, Vector3 direction, string who){
        if(collision.gameObject.GetComponent<Rigidbody>() != null){
            if (who == "left" && leftTrigger) {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(
                        direction * 100, ForceMode.Impulse);
            } else if (who == "right" && rightTrigger) {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(
                        direction * 100, ForceMode.Impulse);
            }

        }
    }
}
