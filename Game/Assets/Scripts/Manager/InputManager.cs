using System;
using System.Collections;
using UnityEngine;
 
public class InputManager : MonoBehaviour, IManager, IInputManager {
    private GameObject player;
    public GameObject Player { 
        get{
            if(player == null)
            {
                player = (GameObject)GameManger.resourceLoader.LoadResourceObject(ResourcePaths.PlayerPrefabPath);
            }
            return player;
        }
        set { 
            player = value; 
        } 
    }
    private InputMap map;
    private string fire1 = "Fire1", fire2 = "Fire2";
    public bool Initialize() {
        Debug.Log($"Initializing {GetType().Name}.");
        return true;
    }
    public void SetInput(InputMap map){
        this.map = map;
    }
    public InputMap GetInput(){
        return this.map;
    }
    // global input check routine 
    public IEnumerator Routine(){
        Debug.Log("Running Input Routine");
        yield return null;
        
        while(true){
            InputMap newMap = new InputMap();
        
            if(Input.GetButtonDown(fire1) && !map.fire1Lock){
                newMap.fire1 = state.DOWN;
                map.fire1Lock = true;
            } else if(Input.GetButtonUp(fire1)){
                newMap.fire1 = state.UP;
                map.fire1Lock = true;
            } else {
                newMap.fire1 = state.NONE;
                map.fire1Lock = false;
            }
            if(Input.GetButtonDown(fire2) && !map.fire2Lock){
                newMap.fire2 = state.DOWN;
                map.fire2Lock = true;
            } else if(Input.GetButtonUp(fire2)){
                newMap.fire2 = state.UP;
                map.fire2Lock = true;
            } else {
                newMap.fire2 = state.NONE;
                newMap.fire2Lock = false;
            }

            SetInput(newMap);
            yield return null;
        }
    }
}

