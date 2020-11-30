using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuManager : MonoBehaviour, IManager
{
    public List<IMenuCodeBehind> menuCodeBehinds = new List<IMenuCodeBehind>();

    public void Awake()
    {
        menuCodeBehinds = FindObjectsOfType<MonoBehaviour>().OfType<IMenuCodeBehind>().ToList();
        if (menuCodeBehinds.GroupBy(m => m.MenuIndex).Any(g => g.Count() > 1)) {
            Debug.LogError("Menu's with the same index exist. Fix or transition errors will occur");
        }
    }

    public static void TransitionMenus(int newIndex)
    {
            
    }

    public bool Initialize()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator Routine()
    {
        yield return null;
    }
}
