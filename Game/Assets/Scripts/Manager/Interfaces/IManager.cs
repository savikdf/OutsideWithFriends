﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManager
{
    bool Initialize();
    
    IEnumerator Routine();
}
