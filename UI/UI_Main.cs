using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Main : MonoBehaviour
{
    [SerializeField] private GameMaster gm;

    [SerializeField] private List<Color> teamColors;
    [SerializeField] private TextMeshProUGUI turnText;

    //UI PLAYER
    [SerializeField] private List<TextMeshProUGUI> playerWaitText;


    //BUTTONS
    [SerializeField] private GameObject endTurnButton;
    [SerializeField] private GameObject infoButton;

    //INFO
    private bool infoButtonIsTurnedOn;


    private void Start()
    {
        ChangeTeamText(1);
        TurnOnInfoButton(false);
    }

    public void UpdateWaitText()
    {
        for (int i = 0; i < playerWaitText.Count; i++)
        {
            playerWaitText[i].text = gm.unitTotalWait[i].ToString();
        }
    }

    public void ChangeTeamText(int playerTurn)
    {
        if (playerTurn == 1)
        {
            turnText.color = teamColors[0];
            turnText.text = "Player 1";
        }
        else
        {
            turnText.color = teamColors[1];
            turnText.text = "Player 2";

        }
    }

    public void EnableEndTurnButton(bool endTurn)
    {
        endTurnButton.SetActive(endTurn);
    }

    public void TurnOnInfoButton(bool turnOn)
    {
        infoButton.SetActive(turnOn);
        if (!turnOn)
        {
            InfoButton(false);
        }
        
    }



    //BUTTON
    public void EndTurnButton()
    {
        gm.actionLog.AddToKeyLog(ActionKey.EndTurn, 0);
        gm.EndTurn(true);
    }

    public void InfoButton(bool turnOn)
    {
        if (turnOn && !infoButtonIsTurnedOn)
        {
            gm.infoCardTopSection.EnableInfoCard(gm.currentSelection, true);
            infoButtonIsTurnedOn = true;
        }
        else
        {
            gm.infoCardTopSection.EnableInfoCard(null, false);
            infoButtonIsTurnedOn = false;
        }
    }
}
