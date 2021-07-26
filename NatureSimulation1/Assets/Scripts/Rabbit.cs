using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    private int foodLayer;
    private Collider target;
    
    
    float sight = 5f;
    void Start()
    {
        foodLayer = LayerMask.NameToLayer("food");
    }

    // Update is called once per frame
    void Update()
    {
        Vision();
    }

    void FixedUpdate()
    {
        if(target != null) GetComponent<AIDestinationSetter>().target = target.transform;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position , sight);
    }

    void Vision()
    {
        if (target != null) target.GetComponent<MeshRenderer>().material.color = Color.green;
            
            
        float distance = Mathf.Infinity;
        var colliders = Physics.OverlapSphere(transform.position, sight, 1 << foodLayer);
        
        
        foreach (var collider in colliders)
        {
            if (Vector3.Distance(collider.transform.position , transform.position) < distance)
            {
                target = collider;
                distance = Vector3.Distance(collider.transform.position, transform.position);
            }
           
        }

        if (target != null)
        {
            target.GetComponent<MeshRenderer>().material.color = Color.red;
            
        }
        else
        {
            Debug.Log("theres no targets in the area");
        }
        
    }
}
