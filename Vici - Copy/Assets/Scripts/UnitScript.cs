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
    public float actDist = 0.8f;  // Float (or int if necessary) that determines the radius a unit can act on for attacking and other actions
    public int player = 0;
    public int team = 0; //unit team, team 0 is neutral team
    public Color teamColor;
    private GameObject teamManager;
    public GameObject Target;    // Target for attacking
    public GameObject PrevTarget; //Saved target if unit approaches nearby enemy unit first
    private Collider2D[] nearestObject = new Collider2D[1];

    CircleCollider2D aoe = new CircleCollider2D();
    string State = "Idle";


    // Start is called before the first frame update
    void Start()
    {
        teamManager = GameObject.Find("TeamManager");
        TeamScript tInfo = teamManager.GetComponent<TeamScript>();
        setTeam(tInfo.getPlayerTeam(player));
        setColor(tInfo.getPlayerColor(player));

        aoe = GetComponent<CircleCollider2D>();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<UnitScript>() != null)
            if(collision.gameObject.GetComponent<UnitScript>().team != team && PrevTarget == null)
            {
                PrevTarget = Target;
                Target = collision.gameObject;
            }

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

    private void updatePos()
    {
        //Magical rotation code that I spent 6 hours on
        float offset = -90f;
        float rotationSpeed = 2.5f;
        Vector3 direction = Target.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        //One line to actually move
        transform.Translate(Vector3.up * Speed * Time.fixedDeltaTime, Space.Self);

        //Collider
        //CheckForEnemies();
    }

    /*
    private void CheckForEnemies()
    {
        
        //Physics2D.OverlapCircleNonAlloc(transform.position, actDist, nearestObject);
    }
    */

    //WIP attack function
    private void Action()
    {
        if (Target.GetComponent<UnitScript>() != null)
        {
            Attack(Target);
            if(PrevTarget != null)
            {
                Target = PrevTarget;
                PrevTarget = null;
            }
            else
            {
                Target = null;
            }
                
        }
        else if (Target.GetComponent<TerritoryScript>() != null)
        {
            Target.GetComponent<TerritoryScript>().adjustHealth(team);
            Destroy(this.gameObject);
        }
    }

    private void Attack(GameObject currTarget)
    {

    }

    //WIP sacrifice function
    private void Sacrifice()
    {
        //needs to call the territory's add health function then destroy self
        Target.GetComponent<TerritoryScript>().adjustHealth(team);
        Destroy(this.gameObject);
    }

    //sets current target
    public void setTarget(GameObject t)
    {
        Target = t;
    }

    //returns current target
    public GameObject getTarget()
    {
        return Target;
    }

    //sets color
    public void setColor(Color c)
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        teamColor = c;
        sprite.color = teamColor;
    }

    //sets color
    public Color getColor()
    {
        return teamColor;
    }

    //sets team, neutral is considered team 0
    public void setTeam(int t)
    {
        team = t;
    }

    //returns team
    public int getTeam()
    {
        return team;
    }
}
