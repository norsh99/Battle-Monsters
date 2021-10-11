using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using DG.Tweening;

public class UI_InfoNode : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private Image speedBackground;
    [SerializeField] private TextMeshProUGUI waitText;
    [SerializeField] private GameObject waitGameObject;
    [SerializeField] private GameObject agitatedStateGO;



    public void UpdateSpeedColor(Color newColor)
    {
        speedBackground.color = newColor;
    }

    public void UpdateSpeedText(string speedAmount)
    {
        
        speedText.text = speedAmount;
    }

    public void UpdateWaitText(int waitAmount)
    {
        if (waitAmount == 0)
        {
            waitGameObject.SetActive(false);
        }
        else
        {
            waitGameObject.SetActive(true);
        }
        waitText.text = waitAmount.ToString();
    }


    public async void PlayAgitatedAnimation(int time)
    {
        agitatedStateGO.SetActive(true);
        agitatedStateGO.transform.DOScale(1, 0);
        await agitatedStateGO.transform.DOScale(1, .5f).AsyncWaitForCompletion();



        await Task.Delay(time);
        await agitatedStateGO.transform.DOScale(0, .5f).AsyncWaitForCompletion();
        agitatedStateGO.SetActive(false);
    }
}
