using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    /* Future note to self:
     * You can make Targets dynamic types to allow for Vector3s instead of invisible targets,
     * but that requires adding a check in FixedUpdate that says:
     * 
     * if(Target type is Vector3 AND Unit Position is within .1f of all Vector axis)
     *      Target = null;
     *      
     * Then you have to add handlers in the movement script that get the Target's Vector3 if it's
     * a GameObject instead of naturally being a Vector3, instead of getting that info in-line.
     */

    public float Health  = 2f;  // int that describes amount of time a unit can be "attacked" before "dying"
    public float Speed = 1f;   // Float that controls speed of the unit per frame
    public float actDist = 0.8f;  // Float (or int if necessary) that determines the radius a unit can act on for attacking and other actions
    public int player = 0;
    public int team = 0; //unit team, team 0 is neutral team
    public Color teamColor;
    private GameObject teamManager;
    public dynamic Target;    // Target for attacking
    public dynamic PrevTarget; //Saved target if unit approaches nearby enemy unit first
    private Collider2D[] nearestObject = new Collider2D[1];
    private bool isTargVector = false;

    public GameObject AIController;

    CircleCollider2D aoe = new CircleCollider2D();


    // Start is called before the first frame update
    public void Start()
    {
        teamManager = GameObject.Find("TeamManager");
        TeamScript tInfo = teamManager.GetComponent<TeamScript>();
        setTeam(tInfo.getPlayerTeam(player));
        setColor(tInfo.getPlayerColor(player));

        aoe = GetComponent<CircleCollider2D>();

        //Checks to see is Target is a Vector3
        isTargVector = typeof(UnityEngine.Vector3).IsInstanceOfType(Target);

        AIController.GetComponent<AIScript>().AddUnit(this.gameObject);
    }

    //updates independently of framerate
    private void FixedUpdate()
    {
        if (Target != null)
            updatePos();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isTargVector)
        {
            if (collision.gameObject.GetComponent<UnitScript>() != null)
            {
                if (collision.gameObject == Target && collision.gameObject.GetComponent<UnitScript>().team != team)
                {
                    Action();
                }
                else if (collision.gameObject.GetComponent<UnitScript>().team != team && PrevTarget == null)
                {
                    PrevTarget = Target;
                    Target = collision.gameObject;
                    Action();
                }
            }
            else if (collision.gameObject.GetComponent<TerritoryScript>() != null && collision.gameObject == Target)
            {
                Action();
            }
        }
        /*
         * No longer necessary now that Invisible Targets are actually just Vector3s
         * 
        else if (collision.gameObject.CompareTag("Invisible Target") && collision.gameObject == Target)
        {
            Destroy(Target);
            Target = null;
            if (PrevTarget != null)
            {
                Target = PrevTarget;
                PrevTarget = null;
            }
        }
        */
    }

    /* 
     * This is here for future implementation of moving units if they occupy the same space as others when they complete their movement
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(Target == null)
        {
            if (timeStationary < 0.1f && collision.gameObject.GetComponent<UnitScript>() != null && collision.gameObject.GetComponent<UnitScript>().team == team)
            {
                Target = randomPosition(collision.collider.bounds.size.x);
            }
        }
    }
    */

    private void OnDestroy()
    {
        if (!isTargVector && Target != null && Target.CompareTag("Invisible Target"))
            Destroy(Target);
        int rip = AIController.GetComponent<AIScript>().units.IndexOf(this.gameObject);
        AIController.GetComponent<AIScript>().units.RemoveAt(rip);
    }

    private void updatePos()
    {
        //Magical rotation code that I spent 6 hours on
        float offset = -90f;
        float rotationSpeed = 2.5f;
        Vector3 direction;

        isTargVector = typeof(UnityEngine.Vector3).IsInstanceOfType(Target);

        if (isTargVector)
            direction = Target - transform.position;
        else
            direction = Target.transform.position - transform.position;

        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        //One line to actually move
        transform.Translate(Vector3.up * Speed * Time.fixedDeltaTime, Space.Self);

        //Checks if Target is a Vector3
        if(isTargVector)
        {
            if(Mathf.Abs(Vector3.Distance(transform.position, Target)) < 0.1f)
            {
                Target = PrevTarget;
                PrevTarget = null;
            }
        }

        //Collider
        //CheckForEnemies();
    }

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
            /*
             * THIS MEANS YOU CANNOT TARGET AN ENEMY TERRITORY AT FULL HEALTH
             * 
             * Possible fixes include changing territory health from unitscript rather than territoryscript.
             */

            if (Target.GetComponent<TerritoryScript>().health < Target.GetComponent<TerritoryScript>().healthCap || Target.GetComponent<TerritoryScript>().team != team)
            {
                //ActionTerritory(Target);
                Target.GetComponent<TerritoryScript>().adjustHealth(team, player);
                Destroy(this.gameObject);
            }
            else
                Target = null;
        }
    }

    private void Attack(GameObject currTarget)
    {
        Destroy(currTarget);
        Target = null;
    }

    private void ActionTerritory(GameObject currTarget)
    {
        TerritoryScript tScript = currTarget.GetComponent<TerritoryScript>();
        if (tScript.team == team && tScript.health <= tScript.healthCap)
            tScript.health++;
        else if (tScript.team != team)
            tScript.health--;

        if(tScript.health <= 0 && tScript.team != 0)
        {
            tScript.convert(0);
            tScript.health = 20;
        }
        else if(tScript.health <= 0 && tScript.team == 0)
        {
            tScript.convert(player);
            tScript.health = 15;
        }
    }

    //variation of randomPosition from Territory script. Will be called when two units occupy the same space to send a new position for the incoming unit
    /*
     * Will be implemented in future update
    private Vector3 randomPosition(float diameter)
    {
        float theta = 360 * UnityEngine.Random.Range(0.0f, 1.0f);
        float innerRadius = (diameter / 2f) + 0.1f;
        float outerRadus = innerRadius + diameter + 0.4f;

        float dist = Mathf.Sqrt(UnityEngine.Random.Range(0.0f, 1.0f) * (Mathf.Pow(innerRadius, 2.0f) - Mathf.Pow(outerRadus, 2f)) + Mathf.Pow(outerRadus, 2f));

        float x = (dist * Mathf.Cos(theta)) + this.transform.position.x;
        float y = (dist * Mathf.Sin(theta)) + this.transform.position.y;

        return new Vector3(x, y, 0f);
    }
    */

    //sets current target
    public void setTarget(dynamic t)
    {
        Target = t;
        //Checks to see is Target is a Vector3
        isTargVector = typeof(UnityEngine.Vector3).IsInstanceOfType(Target);
    }

    //returns current target
    public dynamic getTarget()
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
