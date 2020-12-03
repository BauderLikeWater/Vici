using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControllerCopy : MonoBehaviour
{
    private Vector3 beginMouseSelec = new Vector3(0, 0, 0);
    private Vector3 beginMouseSelecRec = new Vector3(0, 0, 0);
    private Vector3 endMouseSelec = new Vector3(0, 0, 0);
    private Vector3 endMouseSelecRec = new Vector3(0, 0, 0);
    public Texture myTexture;
    public Camera mainCamera;
    private bool bselectionDraw = false;
    private bool hasSelected = false;
    //Collider2D SelectionCollider = new Collider2D();
    List<GameObject> SelectionList = new List<GameObject>();
    public int player = 0;
    public int team = 1;
    public Color teamColor = new Color(1f, 1f, 1f, 1f);
    public Texture2D selectBox;
    public GUIStyle boxStyle;
    public GameObject RandomTarget;

    // Start is called before the first frame update
    void Start()
    {
        //mainCamera = Camera.Find("Main Camera");
    }

    private void playerClick()
    {
        if (!hasSelected && Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            beginMouseSelec = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            beginMouseSelecRec = Input.mousePosition;
            endMouseSelec = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            endMouseSelecRec = Input.mousePosition;
            bselectionDraw = true;
        }

        else if (!hasSelected && Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift))
        {
            endMouseSelec = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            endMouseSelecRec = Input.mousePosition;
        }
        else if (!hasSelected && Input.GetMouseButtonDown(0))
        {
            beginMouseSelec = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            endMouseSelec = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Selection();
        }

        else if ((Input.GetMouseButtonUp(0) && Input.GetKey(KeyCode.LeftShift)) || (Input.GetMouseButton(0) && Input.GetKeyUp(KeyCode.LeftShift)))
        {
            bselectionDraw = false;
            if (SelectionList.Count < 1)
                Selection();
        }

        if(hasSelected && Input.GetMouseButtonDown(0))
        {
            beginMouseSelec = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            endMouseSelec = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            giveTarget();
        }

        if (Input.GetMouseButtonDown(1))
        {
            //print("no");
            Deselection();
        }

        /*
        else if (Input.GetMouseButtonUp(0) && !Input.GetKey(KeyCode.LeftShift))
        {

            if (SelectionList.Count >= 1)
                giveTarget();
            else
                Deselection();
        }
        */
    }

    private void giveTarget()
    {
        if (true)
        {
            endMouseSelec = endMouseSelec + new Vector3(.1f, .1f, .1f);
        }

        Collider2D[] targeted = Physics2D.OverlapAreaAll(beginMouseSelec, endMouseSelec);
        GameObject targ;

        if (targeted.Length > 0)
        {
            Collider2D t = targeted[0];
            targ = t.gameObject;
        }
        else
        {
            targ = Instantiate(RandomTarget, beginMouseSelec, Quaternion.Euler(0f, 0f, 0f));
        }


        for (int i = 0; i < SelectionList.Count; i++)
        {
            if(SelectionList[i] != null)
            {
                GameObject stuff = SelectionList[i];
                if (stuff.gameObject.GetComponent<UnitScript>() != null)
                {
                    stuff.gameObject.GetComponent<UnitScript>().setTarget(targ);
                }
                else if(stuff.gameObject.GetComponent<TerritoryScript>() != null)
                {
                    stuff.gameObject.GetComponent<TerritoryScript>().setTarget(targ);
                }
            }
        }

        Deselection();
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
            hasSelected = true;
            foreach (Collider2D stuff in selected)
            {
                if (stuff.gameObject.GetComponent<UnitScript>() != null)
                {
                    if (stuff.gameObject.GetComponent<UnitScript>().player == player)
                    {
                        SelectionList.Add(stuff.gameObject);
                        foundUnit = true;
                        hasSelected = true;
                        stuff.gameObject.GetComponent<SpriteOutline>().enabled = true;
                    }
                }
            }

            if (!foundUnit)
            {
                foreach (Collider2D stuff in selected)
                {
                    if (stuff.gameObject.GetComponent<TerritoryScript>() != null)
                    {
                        if(stuff.gameObject.GetComponent<TerritoryScript>().player == player)
                        {
                            SelectionList.Add(stuff.gameObject);
                            stuff.gameObject.GetComponent<SpriteOutline>().enabled = true;
                            hasSelected = true;
                        }
                    }
                }
            }
        }
    }



    private void Deselection()
    {
        if (SelectionList.Count > 0)
        {
            for(int i = 0; i < SelectionList.Count; i++)
            {
                if(SelectionList[i] != null)
                    SelectionList[i].GetComponent<SpriteOutline>().enabled = false;
            }
            SelectionList.Clear();
        }
            
        hasSelected = false;
    }


    private void Update()
    {
        playerClick();
        //print(Input.GetMouseButtonDown(0));
        //print(Input.mousePosition);
        //print(Event.current.mousePosition);
    }
}
