using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitScript : MonoBehaviour
{

    public float Health  = 2f;  // int that describes amount of time a unit can be "attacked" before "dying"
    public float Speed = 1f;   // Float that controls speed of the unit per frame
    public float rotationSpeed = 180f;
    //float ActDist = .1f;  // Float (or int if necessary) that determines the radius a unit can act on for attacking and other actions
    public int team = 0; //unit team, team 0 is neutral team
    Vector3 Destin;   // Destination for travel
    public GameObject Target;    // Target for attacking
    public GameObject PrevTarget; //Saved target if unit approaches nearby enemy unit first
    string State = "Idle";
    

    // Start is called before the first frame update
    void Start()
    {
        team = GameObject.Find("Player Controller").GetComponent<PlayerController>().getTeam();
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = GameObject.Find("Player Controller").GetComponent<PlayerController>().getColor();
    }

    public void setTarget(GameObject t)
    {
        Target = t;
    }

    public GameObject getTarget()
    {
        return Target;
    }

    //sets a new target, and an appropriate state for the target
    //I intend for this to be called when the player clicks on an object while 
    //this unit is selected

    /*
    public void setNewTargetState(GameObject newObject, string newState)
    {
        Target = newObject;

        if (Target.name.Contains("Territory") )
        {
            if (Target.GetComponent<TerritoryScript>().team == this.team && Target.GetComponent<TerritoryScript>().health < Target.GetComponent<TerritoryScript>().healthCap)
                State = "Sacrificing";
            else
                State = "Attacking";
        }
        else if (Target.name.Contains("Unit"))
        {
            if (Target.GetComponent<UnitScript>().team == this.team)
                State = "Attacking";
        }
        //friendly units and other objects will be ignored, so no if case for them
    }
    */

    //sets team. neutral is considered team 0
    public void setTeam(int t)
    {
        team = t;
    }

    public int getTeam()
    {
        return team;
    }
    
    private void updatePos()
    {
        //Magical rotation code that I spent 6 hours on
        float rotationSpeed = 2.5f;
        float offset = -90f;
        Vector3 direction = Target.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        //One line to actually move
        transform.Translate(Vector3.up * Speed * Time.fixedDeltaTime, Space.Self);
    }

    private void CheckForTarget()
    {

    }
    

    /*
     * Old updatePos()
    private void updatePos() {

        //saves the math for testing positions of the x and y for multiple uses
        float differenceX = transform.position.x - Target.transform.position.x;
        bool testX = (Mathf.Abs(differenceX) >= ActDist);

        float differenceY = transform.position.y - Target.transform.position.y;
        bool testY = (Mathf.Abs(differenceY) >= ActDist);

        bool clearX = false;
        bool clearY = false;


        //Looks at target, it currently borks the sprite
        //transform.LookAt(Target.transform.localPosition);

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
    */

    //WIP attack function
    private void Attack()
    {
        if (Target.GetComponent<UnitScript>() != null)
        {
            Destroy(Target);
            Target = null;
        }
        else if(Target.GetComponent<TerritoryScript>() != null)
        {
            Target.GetComponent<TerritoryScript>().adjustHealth(team);
            Destroy(this.gameObject);
        }
    }

    //WIP sacrifice function
    private void Sacrifice()
    {
        //needs to call the territory's add health function then destroy self
        Target.GetComponent<TerritoryScript>().adjustHealth(team);
        Destroy(this.gameObject);
    }

    //Keybinds for testing
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

    //updates independently of framerate
    private void FixedUpdate()
    {
        if (Target != null)
            updatePos();
    }
}
