using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    public bool selected = false;
    public Color selectColor = Color.blue;

    /*
    private LineRenderer line;
    public int segments = 30;
    public float xradius = 1;
    public float yradius = 1;
    */


    // Start is called before the first frame update
    void Start()
    {
        /*
        line = gameObject.GetComponent<LineRenderer>();

        setRadius(base.transform.localScale.x * 1.2f, base.transform.localScale.y * 1.2f);
        */




    }

    // Update is called once per frame
    void Update()
    {
        /*
        line.enabled = true;
        if(selected)
        {

        }
        else
        {
            //line.enabled = false;
        }
        */





    }

    /*
    private void CreatePoints()
    {
        float x;
        float y;

        float angle = 20f;

        for(int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            line.SetPosition(i, new Vector3(x + base.transform.position.x, y + base.transform.position.y, 0));

            angle += (360f / segments);
        }
    }

    public void setRadius(float xr, float yr)
    {
        xradius = xr;
        yradius = yr;
        CreatePoints();
    }
    */

    public void setSelected(bool select)
    {
        selected = select;
    }

    public bool isSelected()
    {
        return selected;
    }
}
