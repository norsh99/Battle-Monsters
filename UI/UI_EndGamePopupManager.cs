using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EndGamePopupManager : MonoBehaviour
{
    [SerializeField] private GameObject winnerGO;
    [SerializeField] private GameObject loserGO;



    public void ChangeText(string changeText)
    {
        winnerGO.GetComponent<EndScreenTextAnimation>().ChangeText(changeText);
    }


    public void ResetEndGameScreen()
    {
        winnerGO.SetActive(false);
        loserGO.SetActive(false);
    }

    public void SetWinnerOrLoserScreenPopUp(bool didWin)
    {
        if (didWin)
        {
            ChangeText("Winner");
            winnerGO.SetActive(true);
            loserGO.SetActive(false);
        }
        else
        {
            winnerGO.SetActive(false);
            loserGO.SetActive(true);
        }
    }
}
