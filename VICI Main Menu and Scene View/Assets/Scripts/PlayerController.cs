using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    Vector3 beginMouseSelec = new Vector3(0, 0, 0);
    Vector3 beginMouseSelecRec = new Vector3(0, 0, 0);
    Vector3 endMouseSelec = new Vector3(0, 0, 0);
    Vector3 endMouseSelecRec = new Vector3(0, 0, 0);
    public Texture myTexture;
    public Camera mainCamera;
    bool bselectionDraw = false;
    //Collider2D SelectionCollider = new Collider2D();
    List<GameObject> SelectionList = new List<GameObject>();
    public int player = 0;
    public int team = 1;
    public Color teamColor = new Color(1f, 1f, 1f, 1f);
    public Texture selectBox;
    public GUIStyle boxStyle;

    // Start is called before the first frame update
    void Start()
    {
        //mainCamera = Camera.Find("Main Camera");
    }

    private void playerClick()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            beginMouseSelec = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            beginMouseSelecRec = Input.mousePosition;
            endMouseSelec = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            endMouseSelecRec = Input.mousePosition;
            bselectionDraw = true;
        }

        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift))
        {
            endMouseSelec = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            endMouseSelecRec = Input.mousePosition;


        }

        if ((Input.GetMouseButtonUp(0) && Input.GetKey(KeyCode.LeftShift)) || (Input.GetMouseButton(0) && Input.GetKeyUp(KeyCode.LeftShift)))
        {
            bselectionDraw = false;
            if (SelectionList.Count < 1)
                Selection();
        }
        else if (Input.GetMouseButtonUp(0) && !Input.GetKey(KeyCode.LeftShift))
        {

            if (SelectionList.Count >= 1)
                giveTarget();
            else
                Deselection();
        }
    }

    private void giveTarget()
    {
        foreach (GameObject stuff in SelectionList)
        {
            if (stuff.gameObject.GetComponent<UnitScript>() != null)
            {
                stuff.gameObject.GetComponent<UnitScript>().Target = null;
            }
            else
            {

            }
        }
    }

    private void OnGUI()
    {
        if (bselectionDraw)
            GUI.Box(new Rect(beginMouseSelecRec.x, -(beginMouseSelecRec.y - Screen.height),
                (endMouseSelecRec.x - beginMouseSelecRec.x),
                -(endMouseSelecRec.y - beginMouseSelecRec.y)),
                new GUIContent(selectBox), boxStyle);
    }


    private void Selection()
    {
        bool foundUnit = false;


        if (beginMouseSelec == endMouseSelec)
        {
            endMouseSelec = endMouseSelec + new Vector3(.01f, .01f, .01f);
        }

        Collider2D[] selected = Physics2D.OverlapAreaAll(beginMouseSelec, endMouseSelec);
        if (selected.Length > 0)
        {
            foreach (Collider2D stuff in selected)
            {
                if (stuff.gameObject.GetComponent<UnitScript>() != null)
                {
                    SelectionList.Add(stuff.gameObject);
                    foundUnit = true;

                }
            }

            if (!foundUnit)
            {
                foreach (Collider2D stuff in selected)
                {
                    if (stuff.gameObject.GetComponent<TerritoryScript>() != null)
                    {
                        SelectionList.Add(stuff.gameObject);
                    }
                }
            }
        }
    }



    private void Deselection()
    {

        if (SelectionList.Count > 0)
            SelectionList.Clear();
    }


    private void Update()
    {
        playerClick();
        //print(Input.GetMouseButtonDown(0));
        //print(Input.mousePosition);
        //print(Event.current.mousePosition);
    }
}
