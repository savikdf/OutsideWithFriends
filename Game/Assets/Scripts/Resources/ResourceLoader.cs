using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader : MonoBehaviour, IResourceLoader
{
    public UnityEngine.Object LoadResourceObject(string filePath)
    {
        try
        {
            UnityEngine.Object obj = Resources.Load(filePath) as UnityEngine.Object;
            return obj;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load resource at path: {filePath} {Environment.NewLine} Error Message: {ex.Message}");
        }
        return null;
    }
}
