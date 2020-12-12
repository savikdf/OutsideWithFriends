using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuCodeBehind
{
    int MenuIndex { get; set; }
    GameObject GameObject { get; set; }
    List<MenuTransitioner> TransferButtons { get; set; }

    void Initialize();
}
