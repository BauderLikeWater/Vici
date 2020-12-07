using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    public int team = 0;
    public int territoryCount = 1;
    public int enemyTerritoryCount = 1;
    private float genCounter = 0.0f;

    GameObject[] territories;
    public List<GameObject> units = new List<GameObject>();

    GameObject UText;
    GameObject TText;

    // Start is called before the first frame update
    void Start()
    {
        UText = GameObject.Find("U Count");
        TText = GameObject.Find("T Count");
        if (UText != null)
            print(UText);

        territories = GameObject.FindGameObjectsWithTag("Territory");
        setUnitText();
        updateTerritories();
    }

    // Update is called once per frame
    void Update()
    {
        if (genCounter >= 2)
        {
            if (territoryCount == 0 && units.Count <= 5)
                SceneManager.LoadScene("DefeatScreen", LoadSceneMode.Single);
            else if(enemyTerritoryCount == 0)
                SceneManager.LoadScene("VictoryScreen", LoadSceneMode.Single);
            genCounter = 0;
        }

        genCounter += Time.fixedDeltaTime;
    }

    public void AddUnit(GameObject newUnit)
    {
        if(newUnit.GetComponent<UnitScript>().getTeam() == team)
            units.Add(newUnit);
        setUnitText();
    }

    public void RemoveUnit(GameObject deadUnit)
    {

    }

    public void updateTerritories()
    {
        territoryCount = 0;
        enemyTerritoryCount = 0;
        foreach(GameObject t in territories)
        {
            if (t.GetComponent<TerritoryScript>().getTeam() == team)
                territoryCount++;
            else
                enemyTerritoryCount++;
        }
        setTerritoryText();
    }

    public void setUnitText()
    {
        UText.GetComponent<TextMeshProUGUI>().text = units.Count.ToString();
    }

    public void setTerritoryText()
    {
        TText.GetComponent<TextMeshProUGUI>().text = territoryCount.ToString();
    }
}
