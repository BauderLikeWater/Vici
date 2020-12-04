using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    public GameObject target;
    int team = 4;
    float counter =  0;

    public List<GameObject> units = new List<GameObject>();
    GameObject[] territories;

    // Start is called before the first frame update
    void Start()
    {
        territories = GameObject.FindGameObjectsWithTag("Territory");
        //print(territories.Length);
        FindTarget();
    }


    void GiveTarget() {
        //print(units.Count);
        for(int i = 0; i < units.Count; i++)
        {
            if (units[i] != null)
            {
                if (units[i].GetComponent<UnitScript>().getTarget() != null)
                    units[i].GetComponent<UnitScript>().PrevTarget = target;
                else
                    units[i].GetComponent<UnitScript>().setTarget(target);
            }
        }
    }

    void FindTarget() {
        foreach (GameObject terr in territories) {
            if (terr.GetComponent<TerritoryScript>().team == 0)
            {
                target = terr;
                return;
            }
        }

        foreach (GameObject terr in territories)
        {
            if (terr.GetComponent<TerritoryScript>().team != team)
            {
                target = terr;
                return;
            }
        }
    }

    public void AddUnit(GameObject newUnit) {
        units.Add(newUnit);
        //print("test");
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.fixedDeltaTime;
        if (counter >= 20.0f)
        {
            //print(target.GetComponent<TerritoryScript>().getTeam());
            //if (target.GetComponent<TerritoryScript>().team == 4)
            { 
                FindTarget();

                GiveTarget();
            }
            counter = 0f;
        }
    }
}
