using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuManager : MonoBehaviour, IManager
{
    public static MenuManager instance;
    public List<IMenuCodeBehind> menuCodeBehinds = new List<IMenuCodeBehind>();

    public void Awake()
    {
        instance = this;
        menuCodeBehinds = FindObjectsOfType<MonoBehaviour>().OfType<IMenuCodeBehind>().ToList();
        if (menuCodeBehinds.GroupBy(m => m.MenuIndex).Any(g => g.Count() > 1)) {
            Debug.LogError("Menu's with the same index exist. Fix or transition errors will occur");
        }
        Initialize();
    }

    public void Start() {
        TransitionMenus(0);
    }

    public void TransitionMenus(int toIndex)
    {
        menuCodeBehinds.ForEach(m =>
        {
            m.GameObject.SetActive(m.MenuIndex == toIndex);
        });
    }

    public bool Initialize()
    {
        menuCodeBehinds.ForEach(m => m.Initialize());
        return true;
    }

    public IEnumerator Routine()
    {
        yield return null;
    }
}
