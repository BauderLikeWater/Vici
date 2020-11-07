using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitScript : MonoBehaviour
{

    int Health  = 2;  // int that describes amount of time a unit can be "attacked" before "dying"
    int Speed = 2;   // Float that controls speed of the unit per frame
    float ActDist = .1f;  // Float (or int if necessary) that determines the radius a unit can act on for attacking and other actions
    Vector3 Destin;   // Destination for travel 
    public GameObject Target;    // Target for attacking
    string State = "Idle";

    // Start is called before the first frame update
    void Start()
    {
        if (Target == null) {
            Target = new GameObject();
            Target.transform.Translate(new Vector3(0, 0, 0));
        }

        
    }

    public void setNewTargetState(GameObject newObject, string newState)
    {
        Target = newObject;
        
        if(Target.GetType() == )
    }



    private void updatePos() {

        //saves the math for testing positions of the x and y for multiple uses
        float differenceX = transform.position.x - Target.transform.position.x;
        bool testX = (Mathf.Abs(differenceX) >= ActDist);

        float differenceY = transform.position.y - Target.transform.position.y;
        bool testY = (Mathf.Abs(differenceY) >= ActDist);

        bool clearX = false;
        bool clearY = false;


        //tests x
        if (testX && differenceX > 0)
        {

            transform.position = new Vector3(
                transform.position.x - Speed * Time.fixedDeltaTime,
                transform.position.y,
                transform.position.z
                );
        }
        else if (testX && differenceX < 0)
        {
            transform.position = new Vector3(
                transform.position.x + Speed * Time.fixedDeltaTime,
                transform.position.y,
                transform.position.z
                );
        }
        else
            clearX = true;

        //tests y
        if (testY && differenceY > 0)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - Speed  * Time.fixedDeltaTime,
                transform.position.z
                );
        }
        else if (testY && differenceY < 0)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + Speed * Time.fixedDeltaTime,
                transform.position.z
                );
        }
        else
            clearY = true;


        //calls the update of the units current action if inside act dist of Target
        if (clearX && clearY)
        {
            if (State == "Attacking")
                Attack();
            else if (State == "Sacrificing")
                Sacrifice();
            //move and idle don't need/have update functions
        }
    }

  
    private void Attack() {
        print("attack");
    }
    
    private void Sacrifice() {
        print("sacrifice");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            if (State == "Attacking")
                State = "Sacrificing";
            else if (State == "Sacrificing")
                State = "Moving";
            else if (State == "Moving")
                State = "Idle";
            else
                State = "Attacking";
    }

    private void FixedUpdate()
    {

        if (State != "Idle")
            updatePos();
    }
}
