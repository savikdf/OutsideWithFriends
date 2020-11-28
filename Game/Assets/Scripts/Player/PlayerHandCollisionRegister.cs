﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandCollisionRegister : MonoBehaviour {
    public PlayerTagController Controller;
    
    void PassCollisionEvent(Collision collision, Vector3 direction){
         Controller.HandleCollision(collision, this.transform.forward);
    }
    void OnCollisionStay(Collision collision) {
       PassCollisionEvent(collision, this.transform.forward);
    }
    void OnCollisionEnter(Collision collision) {
        PassCollisionEvent(collision, this.transform.forward);
    }
}