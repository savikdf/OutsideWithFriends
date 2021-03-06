﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuCodeBehind : MonoBehaviour, IMenuCodeBehind
{
    public int menuIndex;
    public bool isMultiplayerMenu = false;
    public int MenuIndex { get { return menuIndex; } set { menuIndex = value; } }
    public GameObject GameObject { get { return this.gameObject; } set {  } }

    private MenuManager menuManager = null;

    public List<MenuTransitioner> TransferButtons { get; set; }


    internal List<MenuTransitioner> transferButtons;

    public void Initialize()
    {
        menuManager = FindObjectOfType<MenuManager>().GetComponent<MenuManager>();
        transferButtons = GetComponentsInChildren<MenuTransitioner>().ToList();
        transferButtons.ForEach(b =>
        {
            b.GetComponent<Button>().onClick.AddListener(delegate { TransferButtonClicked(b); });
            b.fromIndex = menuIndex;            
        });
        RectTransform rec = GetComponent<RectTransform>();
        rec.anchoredPosition3D = Vector3.zero;
        MenuIndex = menuIndex;
    }

    public virtual void TransferButtonClicked(MenuTransitioner t) {
        if(t != null)
        {
            if (t.buttonTransitionType == TransitionType.Menu) {
                //Debug.Log($"{t.buttonName} transitioning to menu index: {t.toIndex}");
                menuManager.TransitionMenus(t.toIndex);
            }
            else if (t.buttonTransitionType == TransitionType.Level)
            {
                //Debug.Log($"{t.buttonName} transitioning to scene index: {t.toIndex}");
                menuManager.TransitionToLevel(t.toIndex);
            }
        }
    }
}
