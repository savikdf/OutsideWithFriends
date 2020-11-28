using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceLoader
{
    UnityEngine.Object LoadResourceObject(string filePath);
}
