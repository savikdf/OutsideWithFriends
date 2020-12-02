using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum state {NONE, UP, DOWN}
public struct InputMap {
    public state fire1;
    public bool fire1Lock;
    public state fire2;
    public bool fire2Lock;
}

public interface IInputManager : IManager {
    GameObject Player { get; set; }
    void SetInput(InputMap map);
    InputMap GetInput();
}
