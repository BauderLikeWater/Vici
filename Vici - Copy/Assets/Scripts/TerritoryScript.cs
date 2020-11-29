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
    public GameObject Target = null;
    public GameObject RandomTarget;
    private CircleCollider2D physCollider;


    // Start is called before the first frame update
    void Start()
    {
        teamManager = GameObject.Find("TeamManager");
        TeamScript tInfo = teamManager.GetComponent<TeamScript>();
        setTeam(tInfo.getPlayerTeam(player));
        setColor(tInfo.getPlayerColor(player));

        physCollider = this.GetComponent<CircleCollider2D>();

        updateDiameter(health);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            generateUnit();
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

    public void adjustHealth(int unitTeam, int unitPlayer)
    {
        if(unitTeam == team)
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

        //new generation rate algorithm
        //to increase genration rate, reduce the denominator
        //a denominator of 132 will produce a unit at max health every 1.5 seconds
        genRate = (float)health / 66f;
    }

    private Vector3 randomPosition()
    {
        float theta = 360 * Random.Range(0.0f, 1.0f);
        float innerRadius = diameter / 2f;
        float outerRadus = innerRadius + 1f;

        float dist = Mathf.Sqrt(Random.Range(0.0f, 1.0f) * (Mathf.Pow(innerRadius, 2.0f) - Mathf.Pow(outerRadus, 2f)) + Mathf.Pow(outerRadus, 2f));

        float x = (dist * Mathf.Cos(theta)) + this.transform.position.x;
        float y = (dist * Mathf.Sin(theta)) + this.transform.position.y;


        /*
        float x = Random.insideUnitCircle.x;
        x = x + this.transform.position.x + (x * diameter);

        /*
        if (x >= 0)
            x = 1.1f * x + this.transform.position.x + (x * diameter);
        else if (x < 0)
            x = (diameter / 2f) * x + this.transform.position.x - (diameter / 2f);
        */
        /*
        float y = Random.insideUnitCircle.y;
        y = y + this.transform.position.y + (y * diameter);
        */
        /*
        if (y >= 0)
            y = (diameter / 2f) * y + this.transform.position.y + (diameter / 2f);
        else if (y < 0)
            y = (diameter / 2f) * y + this.transform.position.y - (diameter / 2f);

        /*
        float x = Random.insideUnitCircle.x;
        x =(diameter / 2f) * x + this.transform.position.x;

        if (x > this.transform.position.x)
            x += diameter;
        else if (x < this.transform.position.x)
            x -= diameter;

        float y = Random.insideUnitCircle.y;
        y =(diameter / 2f) * y + this.transform.position.y;

        if (y > this.transform.position.y)
            y += diameter;
        else if (y < this.transform.position.y)
            y -= diameter;
        */

        return new Vector3(x, y, 0f);
    }

    private void convert(int unitPlayer)
    {
        TeamScript tInfo = teamManager.GetComponent<TeamScript>();
        setTeam(tInfo.getPlayerTeam(unitPlayer));
        setPlayer(unitPlayer);
        setColor(tInfo.getPlayerColor(unitPlayer));
    }
    
    private void generateUnit()
    {
        //print("Generate");
        GameObject newUnit = Instantiate(Unit, transform.position, Quaternion.Euler(0f, 0f, 180f));
        UnitScript nUscr = newUnit.GetComponent<UnitScript>();
        nUscr.player = player;
        nUscr.team = team;
        nUscr.enabled = true;

        if (Target != null)
            nUscr.setTarget(Target);
        else
        {
            nUscr.setTarget(Instantiate(RandomTarget, randomPosition(), Quaternion.Euler(0f, 0f, 0f)));
        }
    }

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

    public void setPlayer(int unitPlayer)
    {
        player = unitPlayer;
    }

    public int getPlayer()
    {
        return player;
    }
}
