using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreenTextAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshPro mainText;
    public float animationTime;
    public float startFontSize;
    public float endFontSize;

    public void ChangeText(string changeText)
    {
        mainText.text = changeText;
    }

    private void OnEnable()
    {
        //RESET
        mainText.DOFade(0, 0);
        mainText.DOFontSize(startFontSize, 0);


        //ANIMATE
        mainText.DOFade(1, animationTime);
        mainText.DOFontSize(endFontSize, animationTime);
    }
}
