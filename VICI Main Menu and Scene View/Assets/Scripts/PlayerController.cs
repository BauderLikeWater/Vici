using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * Issues with dynamic type?
 * https://forum.unity.com/threads/missing-compiler-required-member-microsoft-csharp-runtimebinder-csharpargumentinfo-create.563839/
 * ^Explains how to set up project to support dynamic types, but once setup we shouldn't have to do anything special when building game.
 */


public class PlayerController : MonoBehaviour
{
    #if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
    private readonly KeyCode control = KeyCode.LeftCommand;
    #else
    private readonly KeyCode control = KeyCode.LeftControl;
    #endif

    private Vector3 beginMouseSelec = new Vector3(0, 0, 0);
    private Vector3 beginMouseSelecRec = new Vector3(0, 0, 0);
    private Vector3 endMouseSelec = new Vector3(0, 0, 0);
    private Vector3 endMouseSelecRec = new Vector3(0, 0, 0);
    public Texture myTexture;
    public Camera mainCamera;

    private bool bselectionDraw = false;
    private bool hasSelected = false;
    private bool hasUnit = false;

    //Collider2D SelectionCollider = new Collider2D();
    List<GameObject> SelectionList = new List<GameObject>();
    public int player = 0;
    public int team = 1;
    public Color teamColor = new Color(1f, 1f, 1f, 1f);
    public Texture2D selectBox;
    public GUIStyle boxStyle;
    public GameObject RandomTarget;
    public GameObject pauseMenu;


    // Start is called before the first frame update
    void Start()
    {
        //mainCamera = Camera.Find("Main Camera");
    }

    private void Update()
    {
        if (!pauseMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            pauseMenu.SetActive(true);
        else if (pauseMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            pauseMenu.SetActive(false);

        if (Input.GetMouseButtonDown(1))
            Deselection();

        if (!pauseMenu.activeSelf)
            playerClick();
    }

    private void OnGUI()
    {
        if (bselectionDraw)
            GUI.Box(new Rect(beginMouseSelecRec.x, -(beginMouseSelecRec.y - Screen.height),
                (endMouseSelecRec.x - beginMouseSelecRec.x),
                -(endMouseSelecRec.y - beginMouseSelecRec.y)),
                new GUIContent(selectBox), boxStyle);
    }

    private void playerClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            beginMouseSelec = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            beginMouseSelecRec = Input.mousePosition;
            endMouseSelec = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            endMouseSelecRec = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            endMouseSelec = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            endMouseSelecRec = Input.mousePosition;
            if(Mathf.Abs(beginMouseSelec.x - endMouseSelec.x) > .3f || Mathf.Abs(beginMouseSelec.y - endMouseSelec.y) > .3f)
                bselectionDraw = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Selection(bselectionDraw, Input.GetKey(control));
            bselectionDraw = false;
        }
    }

    private void Selection(bool highlighted, bool controlClicked)
    {

        if (beginMouseSelec == endMouseSelec)
            endMouseSelec += new Vector3(.1f, .1f, .1f);

        Collider2D[] selected = Physics2D.OverlapAreaAll(beginMouseSelec, endMouseSelec);

        int currentCount = SelectionList.Count;
        bool justSelectedNew = selected.Length > 0;
        bool givenTarget = false;

        //List<GameObject> TempList = new List<GameObject>();

        //user was not selecting units or additional selectable objects, and did not highlight
        if (!highlighted && hasSelected && !controlClicked)
        {
            giveTarget(selected);
            givenTarget = true;
        }
        if (!controlClicked && hasSelected && justSelectedNew)
        {
            Deselection();
        }
        //if nothing has already been selected, or user control clicked to add to selection
        if (justSelectedNew && !givenTarget && (!hasSelected || controlClicked))
        {
            foreach (Collider2D objs in selected)
            {
                if (objs.gameObject.GetComponent<UnitScript>() != null)
                {
                    if (objs.gameObject.GetComponent<UnitScript>().player == player)
                    {
                        SelectionList.Add(objs.gameObject);
                        hasUnit = true;
                        objs.gameObject.GetComponent<SpriteOutline>().enabled = true;
                    }
                }
            }

            //if no units were found in selection, then look for and add territories
            if (!hasUnit)
            {
                foreach (Collider2D objs in selected)
                {
                    if (objs.gameObject.GetComponent<TerritoryScript>() != null)
                    {
                        if (objs.gameObject.GetComponent<TerritoryScript>().player == player)
                        {
                            SelectionList.Add(objs.gameObject);
                            objs.gameObject.GetComponent<SpriteOutline>().enabled = true;
                        }
                    }
                }
            }

            hasSelected = true;
        }
        

    }

    private void giveTarget(Collider2D[] targeted)
    {
        dynamic targ;

        if (targeted.Length > 0)
        {
            Collider2D t = targeted[0];
            targ = t.gameObject;
        }
        else
        {
            //if switching targeting to vector system, then targ should equal beginMouseSelec
            //targ = Instantiate(RandomTarget, beginMouseSelec, Quaternion.Euler(0f, 0f, 0f));
            targ = new Vector3(beginMouseSelec.x, beginMouseSelec.y, 0f);
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

    private void Deselection()
    {
        if (SelectionList.Count > 0)
        {
            for (int i = 0; i < SelectionList.Count; i++)
            {
                if (SelectionList[i] != null)
                    SelectionList[i].GetComponent<SpriteOutline>().enabled = false;
            }
        }

        SelectionList.Clear();
        hasSelected = false;
        hasUnit = false;
    }
}
