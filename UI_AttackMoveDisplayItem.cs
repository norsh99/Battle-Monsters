using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_AttackMoveDisplayItem : MonoBehaviour
{


    [SerializeField] private List<Image> backgrounds;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private List<Color> colorList;

    [SerializeField] private GameObject descriptionGO;


    public void LoadInfo(AttackMove attackMove)
    {
        titleText.text = attackMove.title;
        damageText.text = attackMove.damage.ToString();

        if (attackMove.ability != null)
        {
            descriptionText.text = attackMove.ability.description;
            descriptionGO.SetActive(true);
        }
        else
        {
            descriptionGO.SetActive(false);
        }

        foreach (Image image in backgrounds)
        {
            image.color = GetColor(attackMove.attackColor);
        }
    }


    private Color GetColor(AttackColor ac)
    {
        switch (ac)
        {
            case AttackColor.White:
                return colorList[0];
            case AttackColor.Gold:
                return colorList[1];
            case AttackColor.Purple:
                return colorList[2];
            case AttackColor.Red:
                return colorList[3];
            case AttackColor.Blue:
                return colorList[4];
            case AttackColor.Green:
                return colorList[5];
            case AttackColor.Pink:
                return colorList[6];
            default:
                return Color.gray;
        }
    }

}
