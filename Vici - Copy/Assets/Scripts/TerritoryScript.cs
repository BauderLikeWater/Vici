using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryScript : MonoBehaviour
{
    public int health = 0;
    public int healthCap = 100;
    public int team = 0;
    float genCounter = 0f;
    public float genRate = 10f;
    int unitCount = 0;


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

    public void adjustHealth(int unitTeam) {

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
        else{

            health--;
            //if it was a negative change, and health is now 0
            //the territory is now nuetral
            if (health == 0)
                team = 0;
        }

        //after change, check health for unit generation rate
        if (health <= 9)
            genRate = -1f;
        else if (health <= 50)
            genRate = 10f;
        else if (health <= 100)
            genRate = 5f;
        else if (health <= 125)
            genRate = 3f;
    }

    //do units generate in or out of the territory
    void generateUnit() {
        print("Generate");
        GameObject go = Instantiate(Resources.Load("Unit")) as GameObject;
        go.transform.Translate(this.transform.position.x, this.transform.position.y, 0);
    }
}
