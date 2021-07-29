using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    private readonly String hungerTaskName = "food";
    private readonly String thirstTaskName = "water";
    private readonly String freeRoamName = "freeRoam";

    private String task;
    private int foodLayer;
    private Transform target;
    private bool atTheTarget;
    private bool busy;

    private float hunger, thirst;
    
    
    float sight = 5f;
    void Start()
    {
        foodLayer = LayerMask.NameToLayer("food");
    }

   

    void FixedUpdate()
    {

        task = ChooseTask();
        target = Vision(task);
        if(target != null) GetComponent<AIDestinationSetter>().target = target.transform;

        hunger += 0.1f;
        thirst += 0.1f;
    }

    void EngageTask()
    {
        
    }
    String ChooseTask()
    {
        String chosenTask = freeRoamName;
        if (hunger > thirst && hunger > 10)
        {
            chosenTask = hungerTaskName;
        }
        else if (thirst > hunger && thirst > 10)
        {
            chosenTask = thirstTaskName;
        }

        return chosenTask;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position , sight);
    }
    Transform Vision(String taskLayer)
    {
        Collider targetCollider = target.GetComponent<Collider>();
        
        
        if (targetCollider != null) targetCollider.GetComponent<MeshRenderer>().material.color = Color.green;
        
        float distance = Mathf.Infinity;
        var colliders = Physics.OverlapSphere(transform.position, sight, 1 << LayerMask.NameToLayer(taskLayer));
        
        
        foreach (var collider in colliders)
        {
            if (Vector3.Distance(collider.transform.position , transform.position) < distance)
            {
                targetCollider = collider;
                distance = Vector3.Distance(collider.transform.position, transform.position);
            }
           
        }
        
        if (targetCollider != null)
        {
            targetCollider.GetComponent<MeshRenderer>().material.color = Color.red;
            atTheTarget = Vector3.Distance(transform.position, targetCollider.transform.position) < 1.5f;
        }
        else
        {
            Debug.Log("theres no targets in the area");
        }

        return targetCollider.transform;
    }
}
