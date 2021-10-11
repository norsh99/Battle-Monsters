using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_DescriptionBoxPopUp : MonoBehaviour
{
    [SerializeField] private GameMaster gm;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject cancelButton;

    private Ability ability;



    public void LoadInDescriptionPopUp(Ability ability, string title, string description, bool useCancalButton)
    {
        this.ability = ability;

        gameObject.SetActive(true);
        cancelButton.SetActive(useCancalButton);
        titleText.text = title;
        descriptionText.text = description;

        gm.UIMain.EnableEndTurnButton(false);
    }

    public void CloseDescriptionPopUp()
    {
        gameObject.SetActive(false);
    }

    //BUTTONS
    //public void RemoveButton()
    //{
    //    CloseDescriptionPopUp();
    //    ability.RemoveAbility();
    //    gm.EndTurn();
    //}

    public void CancelButton()
    {
        CloseDescriptionPopUp();

        if (gm.currentSelection != null)
        {
            gm.DeselectUnit(gm.currentSelection);
        }

        gm.isTaskComplete = true;
    }
}
