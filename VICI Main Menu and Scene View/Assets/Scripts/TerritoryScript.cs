using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryScript : MonoBehaviour
{
    public GameObject Unit;
    private GameObject teamManager;
    public float health = 10;
    public float healthCap = 100;
    private float diameter = 0;
    public int player = 0;
    public int team = 0;
    public Color teamColor;
    private float genCounter = 0f;
    private float genRate = 10f;
    public dynamic Target;
    public GameObject RandomTarget;
    private bool isTargVector = false;

    SpriteRenderer sprite;

    GameObject UIController;


    // Start is called before the first frame update
    void Start()
    {
        teamManager = GameObject.Find("TeamManager");
        TeamScript tInfo = teamManager.GetComponent<TeamScript>();
        setTeam(tInfo.getPlayerTeam(player));

        sprite = GetComponent<SpriteRenderer>();
        setColor(tInfo.getPlayerColor(player));

        isTargVector = typeof(UnityEngine.Vector3).IsInstanceOfType(Target);

        updateDiameter(health);

        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 359f));

        UIController = FindObjectOfType<UIController>().gameObject;
    }

    private void Update()
    {

        //if territory health is at its cap and doesn't need to be its own target anymore
        if (Target != null)
            if (!isTargVector && Target.GetComponent<TerritoryScript>() != null)
                if (Target.GetComponent<TerritoryScript>().team == team && Target.GetComponent<TerritoryScript>().health >= Target.GetComponent<TerritoryScript>().healthCap)
                    Target = null;
    }

    private void FixedUpdate()
    {
        if (genCounter >= genRate)
        {
            if(team != 0)
                generateUnit();
            genCounter = 0f;
        }

        genCounter += Time.fixedDeltaTime;
    }

    //move this to be handled by unit
    public void adjustHealth(int unitTeam, int unitPlayer)
    {
        if(unitTeam == team && health <= healthCap)
        {
            health++;
        }
        else if(unitTeam != team)
        {
            health--;
        }

        if(health <= 0 && team != 0)
        {
            convert(0);
            health = 20;
        }
        else if(health <= 0 && team == 0)
        {
            convert(unitPlayer);
            health = 15;
        }

        updateDiameter(health);
    }

    private void updateDiameter(float h)
    {
        diameter = 1.5f + (h / 28f);

        transform.localScale = new Vector3(diameter, diameter, 0);

        genRate = 120f / (float)health;

    }

    private Vector3 randomPosition()
    {
        float theta = 360 * Random.Range(0.0f, 1.0f);
        float innerRadius = (diameter / 2f) + 0.5f;
        float outerRadus = innerRadius + 1.5f;

        float dist = Mathf.Sqrt(Random.Range(0.0f, 1.0f) * (Mathf.Pow(innerRadius, 2.0f) - Mathf.Pow(outerRadus, 2f)) + Mathf.Pow(outerRadus, 2f));

        float x = (dist * Mathf.Cos(theta)) + this.transform.position.x;
        float y = (dist * Mathf.Sin(theta)) + this.transform.position.y;

        return new Vector3(x, y, 0f);
    }

    public void convert(int unitPlayer)
    {
        TeamScript tInfo = teamManager.GetComponent<TeamScript>();
        setTeam(tInfo.getPlayerTeam(unitPlayer));
        setPlayer(unitPlayer);
        setColor(tInfo.getPlayerColor(unitPlayer));
        Target = null;

        UIController.GetComponent<UIController>().updateTerritories();
    }
    
    private void generateUnit()
    {
        GameObject newUnit = Instantiate(Unit, transform.position, Quaternion.Euler(0f, 0f, 180f));
        UnitScript nUscr = newUnit.GetComponent<UnitScript>();
        nUscr.player = player;
        nUscr.team = team;
        nUscr.enabled = true;

        if (Target != null)
            nUscr.PrevTarget = Target;


        Vector3 t = randomPosition();
        nUscr.setTarget(t);
        
    }

    public void setTarget(dynamic t)
    {
        Target = t;
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

    public void setPlayer(int unitPlayer)
    {
        player = unitPlayer;
    }

    public int getPlayer()
    {
        return player;
    }
}
