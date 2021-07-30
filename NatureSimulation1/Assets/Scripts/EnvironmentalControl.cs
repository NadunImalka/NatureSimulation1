using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalControl : MonoBehaviour
{
    public GameObject[] allGameObjects;
    public List<GameObject> availableGrassBlocks = new List<GameObject>();

    private void Awake()
    {
        allGameObjects =  UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (var objects in allGameObjects)
        {
            if (objects.name == "grassBlock")
            {
                availableGrassBlocks.Add(objects);
            }
        }
    }
    
    
}
