using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamScript : MonoBehaviour
{
    private int player0team = 0;
    private int player1team = 1;
    private int player2team = 2;
    private int player3team = 3;
    private int player4team = 4;
    private Color playerColor0 = new Color(0.7f, 0.7f, 0.7f, 1f);
    private Color playerColor1 = Color.red;
    private Color playerColor2 = Color.green;
    private Color playerColor3 = Color.yellow;
    private Color playerColor4 = Color.blue;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPlayerTeam(int player, int team)
    {
        switch (player)
        {
            case 0:
                player0team = team;
                break;
            case 1:
                player1team = team;
                break;
            case 2:
                player2team = team;
                break;
            case 3:
                player3team = team;
                break;
            case 4:
                player4team = team;
                break;
            default:
                break;
        }
    }

    public int getPlayerTeam(int player)
    {
        switch (player)
        {
            case 0:
                return player0team;
            case 1:
                return player1team;
            case 2:
                return player2team;
            case 3:
                return player3team;
            case 4:
                return player4team;
            default:
                return 0;
        }
    }

    public void setPlayerColor(int player, Color playerColor)
    {
        switch (player)
        {
            case 0:
                playerColor0 = playerColor;
                break;
            case 1:
                playerColor1 = playerColor;
                break;
            case 2:
                playerColor2 = playerColor;
                break;
            case 3:
                playerColor3 = playerColor;
                break;
            case 4:
                playerColor4 = playerColor;
                break;
            default:
                break;
        }
    }

    public Color getPlayerColor(int player)
    {
        switch (player)
        {
            case 0:
                return playerColor0;
            case 1:
                return playerColor1;
            case 2:
                return playerColor2;
            case 3:
                return playerColor3;
            case 4:
                return playerColor4;
            default:
                return new Color(0f, 0f, 0f);
        }
    }
}
