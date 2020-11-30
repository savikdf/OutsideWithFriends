using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuCodeBehind : MonoBehaviour, IMenuCodeBehind
{
    public int menuIndex;
    public int MenuIndex { get { return menuIndex; } set { menuIndex = value; } }
    public GameObject GameObject { get { return this.gameObject; } set {  } }


    public List<MenuTransitioner> TransferButtons { get; set; }


    internal List<MenuTransitioner> transferButtons;

    public void Initialize()
    {
        transferButtons = GetComponentsInChildren<MenuTransitioner>().ToList();
        transferButtons.ForEach(b =>
        {
            b.GetComponent<Button>().onClick.AddListener(delegate { TransferButtonClicked(b); });
            b.fromIndex = menuIndex;
        });
        MenuIndex = menuIndex;
    }

    public virtual void TransferButtonClicked(MenuTransitioner t) {
        MenuTransitioner transitioner = t.GetComponent<MenuTransitioner>();
        if(transitioner != null)
        {
            Debug.Log($"{transitioner.buttonName} transitioning to menu index: {transitioner.toIndex}");
            MenuManager.instance.TransitionMenus(t.toIndex);
        }
    }

}
