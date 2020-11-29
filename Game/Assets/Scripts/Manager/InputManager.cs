using System;
using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour, IManager {
    private string fire1 = "Fire1", fire2 = "Fire2";
    private bool leftActive = false, rightActive = false;
    private GameObject player;
    public GameObject Player { 
        get{
            if(player == null) {
                player = (GameObject)GameManger.resourceLoader.LoadResourceObject(
                    ResourcePaths.PlayerPrefabPath);
            }
            return player;
        } set { 
            player = value; 
        } 
    }

    public bool Initialize() {
        Debug.Log($"Initializing {GetType().Name}.");
        return true;
    }
    
    // global input check routine 
    public IEnumerator Routine(){
        Debug.Log("Running Input Routine");
        while(true){
            if(Input.GetButtonDown(fire1) && !leftActive){
                Debug.Log("Hit");
                leftActive = true;
            }
            if(Input.GetButtonUp(fire1) && leftActive){
                Debug.Log("Hit");
                leftActive = false;
            }
            if(Input.GetButtonDown(fire2) && !rightActive){
                Debug.Log("Hit");
                rightActive = true;
            } 
            if(Input.GetButtonUp(fire2) && rightActive){
                Debug.Log("Hit");
                rightActive = false;
            }
            yield return null;
        }
    }
}