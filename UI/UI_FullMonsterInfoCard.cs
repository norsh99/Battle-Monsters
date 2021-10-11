using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_FullMonsterInfoCard : MonoBehaviour
{

    [SerializeField] private UI_AttackMoveList attackList;
    [SerializeField] private PieChart monsterPieChart;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI abilityText;
    [SerializeField] private TextMeshProUGUI abilityDescriptionText;

    [SerializeField] private TextMeshProUGUI costOfMonText;


    [SerializeField] private GameObject canFlyImage;
    [SerializeField] private GameObject modelSpawnTransform;

    [SerializeField] private Image cardBackground;

    private GameObject model;
    private bool isViewModelOn;

    public void LoadInfo(Unit unit)
    {
        if (unit == null)
        {
            Debug.Log("Unit = NULL");
            return; 
        }
        nameText.text = unit.GetMonster().monsterName;
        speedText.text = unit.GetSpeed().ToString();
        typeText.text = unit.GetMonster().spirit + ", " + unit.GetMonster().type.ToString();
        costOfMonText.text = unit.GetMonster().totalCost.ToString();

        if (unit.allAbilites.Count > 0)
        {
            abilityText.text = unit.allAbilites[0].title;
            abilityDescriptionText.text = unit.allAbilites[0].description;
        }
        else
        {
            abilityText.text = "";
            abilityDescriptionText.text = "";
        }
        CanFly(unit);
        cardBackground.sprite = unit.GetMonster().cardSprite;
        monsterPieChart.LoadIn(unit.GetMonster(), unit.attackMoveList);

        attackList.LoadInfo(unit.attackMoveList);

        isViewModelOn = false;
        SwapBetweenViewModes();
        LoadInModel(unit);
    }





    private void LoadInModel(Unit unit)
    {
        if (model != null)
        {
            Destroy(model);
        }

        model = Instantiate(unit.GetMonster().monsterModel, modelSpawnTransform.transform);
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


    //BUTTONS
    public void SwapBetweenViewModes()
    {
        if (isViewModelOn)
        {
            isViewModelOn = false;
            modelSpawnTransform.SetActive(false);
            monsterPieChart.gameObject.SetActive(true);
        }
        else
        {
            isViewModelOn = true;
            modelSpawnTransform.SetActive(true);
            monsterPieChart.gameObject.SetActive(false);


        }
    }
}
