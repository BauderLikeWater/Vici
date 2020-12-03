using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

        print(hasSelected);
        //print(Input.GetMouseButtonDown(0));
        //print(Input.mousePosition);
        //print(Event.current.mousePosition);
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
        /* 
         * Note to self: You can check the whole array of selected before adding to see if you need to deselect and select new units.
         * This is inefficient though because you're running down the whole array twice. Trying to see if the new list is bigger than
         * the old one once you've added more units without holding control and then removing the old ones with .RemoveRange(,) poses a 
         * problem if a unit is destroyed. Do the list indices change, or is that index just null? Is this better behavior than the 
         * current one? This is worth future consideration.
         * 
         * Extra note: I think destroyed units leave a null value in the index. This shouldn't be too hard to work with if that's the case.
         */

        if (beginMouseSelec == endMouseSelec)
            endMouseSelec += new Vector3(.1f, .1f, .1f);

        Collider2D[] selected = Physics2D.OverlapAreaAll(beginMouseSelec, endMouseSelec);

        //if nothing has already been selected, or user control clicked to add to selection
        if (selected.Length > 0 && (!hasSelected || controlClicked))
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

            if (SelectionList.Count > 0)
                hasSelected = true;
        }
        //user was not selecting units or additional selectable objects, and did not highlight
        else if(!highlighted)
        {
            giveTarget(selected);
        }

    }

    private void giveTarget(Collider2D[] targeted)
    {
        GameObject targ;

        if (targeted.Length > 0)
        {
            Collider2D t = targeted[0];
            targ = t.gameObject;
        }
        else
        {
            //if switching targeting to vector system, then targ should equal beginMouseSelec
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
