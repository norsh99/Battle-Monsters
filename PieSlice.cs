using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class PieSlice : MonoBehaviour
{
    public Color colorPick;
    public AttackMove attackMove;
    public string title;
    public int num;
    public float value;
    public Image backgroundImage;
    public Transform textTransform;
    public Transform numTransform;
    public RectTransform backgroundTransform;
    public TextProOnACircle textProOnACircle;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI numText;



    private float transformStandard = 1000f;
    private float arcStandard = -100f;


    public void SetTitleArc(float newNum)
    {
        textProOnACircle.m_arcDegrees = newNum;
    }

    private void SetTransformWidth(float newNum)
    {
        backgroundTransform.sizeDelta = new Vector2(newNum, 1000f);
    }


    public void UpdateText(float scale, string titleText, string numText)
    {
        this.titleText.text = titleText;
        this.numText.text = numText;

        if (scale < .05f)
        {
            float newRatio = scale / .35f;
            SetTitleArc(newRatio * arcStandard);
            SetTransformWidth(newRatio * transformStandard);
            this.titleText.text = "";
            this.numText.text = "";
        }
        else if (scale < .35f)
        {
            float newRatio = scale / .35f;
            SetTitleArc(newRatio * arcStandard);
            SetTransformWidth(newRatio * transformStandard);
        }
        else
        {
            SetTitleArc(arcStandard);
            SetTransformWidth(transformStandard);
        }
    }

}
