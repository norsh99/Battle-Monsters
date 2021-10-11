using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Sirenix.OdinInspector;

public class UI_StatusNode : MonoBehaviour
{
    [SerializeField] private UI_InfoCardTopSection infoTopSection;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image imageBackground;
    private Color defaultColor = new Color(49f, 37f, 33f, 255f);
    [ReadOnly] public bool isActive;

    private Ability ability;
    public void LoadInfo(Ability ability)
    {
        this.ability = ability;

        titleText.text = ability.title;
        isActive = true;
        gameObject.SetActive(true);
        imageBackground.color = ability.backgroundColor;
    }

    public void DisableNode()
    {
        gameObject.SetActive(false);
        isActive = false;
    }





    //BUTTON
    public void AbilityButtonPress()
    {
        infoTopSection.AbilityPress();
        infoTopSection.abilityDescription.text = ability.title + "\n \n" + ability.description;

    }
}
