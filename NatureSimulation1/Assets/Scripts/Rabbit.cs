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

    private int eatDrinkThreshold = 30;
    private String task;
    private int foodLayer;
    private Collider target;
    private bool atTheTarget;
    private bool busy;
    private bool eating, drinking;
    private float distance;

    private float hunger, thirst = 30;


    float sight = 5f;


    void FixedUpdate()
    {
        task = ChooseTask();
        target = Vision(task);
        //set target in A* destination Setter
        if (target != null) GetComponent<AIDestinationSetter>().target = target.transform;
        
        EngageTask();

        DebugLog();
        if (!busy)
        {
            hunger += 0.05f;
            thirst += 0.05f;
        }
    }

    void DebugLog()
    {
        Debug.Log(hunger + " " + thirst + task + busy + atTheTarget + distance);
    }
    void EngageTask()
    {
        if (atTheTarget)
        {
          
            if (task == hungerTaskName && hunger > 1)
            {
                Debug.Log("HUNGERBUSY");
                busy = true;
                drinking = false;
                eating = true;
            }
            else if (task == thirstTaskName && thirst > 1)
            {
                Debug.Log("THIRSTBUSY");
                busy = true;
                eating = false;
                drinking = true;
            }
            else
            {
                eating = false;
                drinking = false;
                busy = false;
            }
            
        }

        if (task == freeRoamName)
        {
            drinking = false;
            eating = false;
            busy = false;
        }

        if (busy)
        {
            if (task == hungerTaskName && target.gameObject.layer == LayerMask.NameToLayer(task) && eating)
            {
                hunger -= 0.05f;
            }
            else if(task == thirstTaskName && target.gameObject.layer == LayerMask.NameToLayer(task) && drinking)
            {
                thirst -= 0.05f;
            }
        }
    }
    String ChooseTask()
    {
        String chosenTask = task;
        if (!busy) chosenTask = freeRoamName;
        if (hunger > thirst && hunger > eatDrinkThreshold)
        {
            chosenTask = hungerTaskName;
        }
        else if (thirst > hunger && thirst > eatDrinkThreshold)
        {
            chosenTask = thirstTaskName;
        }

        return chosenTask;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position , 2.1f);
    }
    Collider Vision(String taskLayer)
    {
        Collider targetCollider = new Collider();
        if(target != null) targetCollider = target.GetComponent<Collider>();
        
        
        if (targetCollider != null) targetCollider.GetComponent<MeshRenderer>().material.color = Color.green;
        
        distance = Mathf.Infinity;
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
            atTheTarget = Vector3.Distance(transform.position, targetCollider.transform.position) < 2.1f;
        }
        else
        {
            Debug.Log("theres no targets in the area");
        }

        return targetCollider;
    }
}
