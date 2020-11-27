using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryScript : MonoBehaviour
{
    public int health = 10;
    public int healthCap = 100;
    public int team = 0;
    float genCounter = 0f;
    public float genRate = 10f;
    public GameObject Target;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            generateUnit();
    }

    private void FixedUpdate() {

        if (genCounter >= genRate) { 
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

        //new generation rate algorithm
        //to increase genration rate, reduce the denominator
        //a denominator of 132 will produce a unit at max health every 1.5 seconds
        genRate = (float)health/132f;
    }

    //do units generate in or out of the territory
    
    void generateUnit()
    {
        //print("Generate");
        GameObject newUnit = Instantiate(Resources.Load("Unit")) as GameObject;
        newUnit.GetComponent<UnitScript>().setTeam(team);
        newUnit.GetComponent<UnitScript>().setTarget(Target);
        newUnit.transform.Translate(this.transform.position.x, this.transform.position.y, 0);
    }
}
