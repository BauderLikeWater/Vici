using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOLD : MonoBehaviour
{
    Vector3 beginMouseSelec;
    Vector3 endMouseSelec;
    public Sprite mySprite;
    public Camera mainCamera;
    bool bselectionDraw = false;
    public int player = 0;
    public int team = 1;
    public Color teamColor = new Color(1f, 1f, 1f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /*
    private void playerClick() {
        if (Input.GetMouseButtonDown(0)){ 
            beginMouseSelec = mainCamera.ScreenToWorldPoint(Input.mousePosition); ;
            endMouseSelec = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0)) {
            endMouseSelec = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            //Graphics.DrawTexture(new Rect(beginMouseSelec.x, beginMouseSelec.y, (beginMouseSelec.x- endMouseSelec.x), (beginMouseSelec.y - endMouseSelec.y)), myTexture);
            
            
            print(endMouseSelec);
        }

        if (Input.GetMouseButtonUp(0)){
            //Select all units between beginMouseSelec and endMouseSelec

        }

        drawSprite();
    }
    */

    private void playerClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                print(hit.collider.gameObject.name);
                hit.collider.attachedRigidbody.AddForce(Vector2.up);
            }
        }

    }



    private void drawSprite() {
        if (!bselectionDraw) {
        
        }
    }

    public int getTeam()
    {
        return team;
    }

    public Color getColor()
    {
        return teamColor;
    }

    public void setColor(Color newColor)
    {
        teamColor = newColor;
    }

    private void Update()
    {
       //V playerClick();
        
    }
}
