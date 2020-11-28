using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public Image sprintCircle;


    public void SetSprintCircle(float percentage) {
        sprintCircle.fillAmount = 0.5f * percentage;
    }



}
