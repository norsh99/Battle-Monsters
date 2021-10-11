using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_InfoCardTopSection : MonoBehaviour
{
    [SerializeField] private GameMaster gm;
    //[SerializeField] private MonsterAttackSquare monsterAttackSquare;
    [SerializeField] private PieChart monsterPieChart;
    [SerializeField] private GameObject activateEnergyBoostButton;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI energyText;

    [SerializeField] private TextMeshProUGUI spiritTitle;
    public TextMeshProUGUI abilityDescription;

    //[SerializeField] private TextMeshProUGUI abilityDescription;

    [SerializeField] private TextMeshProUGUI costOfMonText;


    [SerializeField] private GameObject fullInfoCard;
    [SerializeField] private GameObject canFlyImage;

    [SerializeField] private List<GameObject> pages;


    //UNIT
    private Unit currentUnit;

    //STATUS
    [SerializeField] private List<UI_StatusNode> listOfNodes;
    [SerializeField] private GameObject abilityTextNode;



    public void EnableInfoCard(Unit selectedUnit, bool enable)
    {
        //currentUnit = selectedUnit;
        if (selectedUnit != null)
        {
            monsterPieChart.LoadIn(selectedUnit.GetMonster(), selectedUnit.attackMoveList);
            FillInInfo(selectedUnit);
        }
        gameObject.SetActive(enable);
        TurnOnPage1(true);

    }

    private void TurnOnPage1(bool turnON)
    {
        pages[0].SetActive(turnON);
        pages[1].SetActive(!turnON);
    }

    private void FillInInfo(Unit unit)
    {
        nameText.text = unit.GetMonster().monsterName;
        speedText.text = (unit.GetSpeed() + 1).ToString();
        costOfMonText.text = unit.GetMonster().totalCost.ToString();
        spiritTitle.text = unit.GetMonster().spirit.ToString();
        energyText.text = unit.currentEnergy + "/" + unit.maxEnergy;

        if (unit.currentEnergy == unit.maxEnergy && gm.canMove && unit.ownerIndex == gm.currentPlayersTurn.index)
        {
            activateEnergyBoostButton.SetActive(true);
        }
        else
        {
            activateEnergyBoostButton.SetActive(false);

        }


        CanFly(unit);

        currentUnit = unit;

        ResetStatusNodesOff();
        ActivateStatusNode(unit);
    }



    private void CanFly(Unit unit)
    {
        if (unit.canFly)
        {
            canFlyImage.SetActive(true);
        }
        else
        {
            canFlyImage.SetActive(false);
        }
    }

    //STATUS NODES
    private void ResetStatusNodesOff()
    {
        for (int i = 0; i < listOfNodes.Count; i++)
        {
            listOfNodes[i].DisableNode();
        }
    }

    private void ActivateStatusNode(Unit unit)
    {
        for (int i = 0; i < unit.allAbilites.Count; i++)
        {
            if (i < listOfNodes.Count)
            {
                if (!listOfNodes[i].isActive)
                {
                    listOfNodes[i].LoadInfo(unit.allAbilites[i]);
                }
            }
        }
    }


    private Color GetColor(AttackColor ac)
    {
        switch (ac)
        {
            case AttackColor.White:
                return new Color(.99f, 0.99f, 0.99f, 1f);
            case AttackColor.Gold:
                return new Color(1f, 0.764f, 0.239f, 1f);
            case AttackColor.Purple:
                return new Color(0.595f, 0.296f, 1f, 1f);
            case AttackColor.Red:
                return new Color(1f, 0.277f, 0.277f, 1f);
            default:
                return Color.gray;
        }
    }


    //BUTTONS
    public void ExitButton()
    {
        gm.DeselectUnit(gm.currentSelection);
        gameObject.SetActive(false);

    }

    public void CloseAbilityDescription()
    {
        TurnOnPage1(true);
    }

    public void InfoButton()
    {
        gm.DeselectUnit(gm.currentSelection);
        gameObject.SetActive(false);
        fullInfoCard.SetActive(true);
        fullInfoCard.GetComponentInChildren<UI_FullMonsterInfoCard>().LoadInfo(currentUnit);

    }

    public void AbilityPress()
    {
        TurnOnPage1(false);
    }







    //BUTTONS
    public void ActivateEnergyBoostButton()
    {
        currentUnit.activateEnergyBoost = true;
        activateEnergyBoostButton.SetActive(false);
        currentUnit.statue.EnableEnergyEffect(true);
        _ = currentUnit.UpdateEnergy(-currentUnit.maxEnergy);
    }
}
