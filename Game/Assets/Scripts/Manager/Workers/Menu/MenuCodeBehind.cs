using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuCodeBehind : MonoBehaviour, IMenuCodeBehind
{
    public int menuIndex;
    public int MenuIndex { get { return menuIndex; } set { MenuIndex = value; } }
    public List<MenuTransitioner> TransferButtons { get; set; }

    internal List<MenuTransitioner> transferButtons;

    public virtual void Initialize()
    {
        transferButtons = GetComponentsInChildren<MenuTransitioner>().ToList();
        MenuIndex = menuIndex;
    }

    public virtual void TransferButtonClicked(Button button) {
        MenuTransitioner transitioner = button.GetComponent<MenuTransitioner>();
        if(transitioner != null)
        {

        }
    }
}
