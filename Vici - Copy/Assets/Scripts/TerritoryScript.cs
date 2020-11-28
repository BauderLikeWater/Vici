using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryScript : MonoBehaviour
{
    public GameObject Unit;
    public float health = 10;
    public float healthCap = 100;
    private float diameter = 0;
    public int player = 0;
    public int team = 0;
    private float genCounter = 0f;
    private float genRate = 10f;
    public GameObject Target = null;
    public GameObject RandomTarget;
    private CircleCollider2D physCollider;


    // Start is called before the first frame update
    void Start()
    {
        updateDiameter(health);
        physCollider = GetComponent<CircleCollider2D>();
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

    public void adjustHealth(int unitTeam)
    {

        //if health is Zero it makes the initial health of the captured health 10 and sets the team
        if (health == 0)
        {
            health += 9;
            team = unitTeam;
        }
        else if (unitTeam == team)
        {
            health++;
        }
        else
        {

            health--;
            //if it was a negative change, and health is now 0
            //the territory is now nuetral
            if (health == 0)
                team = 0;
        }

        updateDiameter(health);

        //new generation rate algorithm
        //to increase genration rate, reduce the denominator
        //a denominator of 132 will produce a unit at max health every 1.5 seconds
        genRate = (float)health/132f;
    }

    private void updateDiameter(float h)
    {
        diameter = 1.5f + (h / 28f);
        physCollider.radius = diameter / 2f;
    }

    private Vector3 randomPosition()
    {
        float x = Random.insideUnitCircle.x;
        x += this.transform.position.x;

        if (x > this.transform.position.x)
            x += (diameter / 2f);
        else if (x < this.transform.position.x)
            x -= (diameter / 2f);

        float y = Random.insideUnitCircle.y;
        y += this.transform.position.y;

        if (y > this.transform.position.y)
            y += (diameter / 2f);
        else if (y < this.transform.position.y)
            y -= (diameter / 2f);

        print("" + x + ", " + y);

        return new Vector3(x, y, 0f);
    }

    private void convert(int player)
    {

    }
    
    private void generateUnit()
    {
        //print("Generate");
        GameObject newUnit = Instantiate(Unit, transform.position, Quaternion.Euler(0f, 0f, 180f));
        UnitScript nUscr = newUnit.GetComponent<UnitScript>();
        nUscr.player = player;
        nUscr.enabled = true;
        if (Target != null)
            nUscr.setTarget(Target);
        else
        {
            nUscr.setTarget(Instantiate(RandomTarget, randomPosition(), Quaternion.Euler(0f, 0f, 0f)));
        }
    }
}
