using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuCodeBehind
{
    int MenuIndex { get; set; }
    void Initialize();
}
