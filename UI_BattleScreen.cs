using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Threading.Tasks;

public class UI_BattleScreen : MonoBehaviour
{
    public GameMaster gm;

    [SerializeField] private List<GameObject> cards;
    [SerializeField] private List<GameObject> iceOverlay;

    [SerializeField] private List<PieChart> spinners;

    [SerializeField] private Transform modelHolder1;
    [SerializeField] private Transform modelHolder2;
    [SerializeField] private Transform spinSelector;


    //BATTLE UI
    [SerializeField] private Image blackOverlayImage;
    [SerializeField] private List<GameObject> decoSlides;

    private GameObject model1;
    private GameObject model2;



    //ATTACK NUMBERS/INFO
    private List<int> unitSpinNum;
    public List<Unit> allUnits;
    public Unit attackUnit;

    private List<int> damageTotals;
    public StageOfBattle stageOfBattle;
    public List<bool> canUnitSpin;



    //TEXT FIELDS
    public List<TextMeshProUGUI> damageText;
    public List<TextMeshProUGUI> titleText;
    public List<TextMeshProUGUI> multiplierText;


    //ABILITIES
    private Ability attackCardAbility;
    public List<Ability> endBattleAbilities;
    public List<Ability> allBattleAbilities;



    //FINAL WINNER?LOSER
    public Unit unitThatWon;
    public Unit unitThatLost;
    public int unitIndexThatWon;




    [Button("Activate Battle")]
    public void LoadBattleScreen(Unit unit1, Unit unit2, bool isUnit1Attacker, int unitSpinNum1, int unitSpinNum2, Ability attackCardAbility)
    {
        Debug.Log("Unit Spin1: " + unitSpinNum1 + ", Unit Spin2: " + unitSpinNum2);

        //ASSIGN
        unitSpinNum = new List<int>() { 0, 0 };
        allUnits = new List<Unit>() { unit1, unit2 };
        endBattleAbilities = new List<Ability>();
        allBattleAbilities = new List<Ability>();
        AddAbilitesToList(unit1, 0);
        AddAbilitesToList(unit2, 1);
        canUnitSpin = new List<bool>() { true, true };



        unitThatWon = null;
        unitThatLost = null;
        this.unitSpinNum[0] = unitSpinNum1;
        this.unitSpinNum[1] = unitSpinNum2;
        spinners[0].LoadIn(unit1.GetMonster(), unit1.attackMoveList);
        spinners[1].LoadIn(unit2.GetMonster(), unit2.attackMoveList);
        this.attackCardAbility = attackCardAbility;
        damageTotals = new List<int>() { 0, 0 };
        unitIndexThatWon = 0;
        stageOfBattle = StageOfBattle.BeforeFirstSpin;
        iceOverlay[0].SetActive(false);
        iceOverlay[1].SetActive(false);


        if (isUnit1Attacker)
        {
            attackUnit = unit1;
        }
        else
        {
            attackUnit = unit2;
        }



        //RESET
        gameObject.SetActive(true);
        blackOverlayImage.DOFade(0f, 0f);
        decoSlides[0].transform.localPosition = new Vector3(-7f, decoSlides[0].transform.localPosition.y, decoSlides[0].transform.localPosition.z);
        decoSlides[1].transform.localPosition = new Vector3(7f, decoSlides[1].transform.localPosition.y, decoSlides[1].transform.localPosition.z);
        spinSelector.localScale = new Vector3(0, 0, 0);
        spinners[0].gameObject.SetActive(false);
        spinners[1].gameObject.SetActive(false);
        //decoSlides[0].gameObject.SetActive(false);
        //decoSlides[1].gameObject.SetActive(false);
        decoSlides[0].transform.DOLocalMoveX(1100, .0f);
        decoSlides[1].transform.DOLocalMoveX(-1100, .0f);
        damageText[0].text = "";
        damageText[1].text = "";
        titleText[0].text = "";
        titleText[1].text = "";
        multiplierText[0].text = "";
        multiplierText[1].text = "";

        if (model1 != null)
        {
            Destroy(model1);
        }
        if (model2 != null)
        {
            Destroy(model2);
        }


        //START ANIMATION
        cards[0].GetComponent<CardAnimate>().ActivateCard(() => AnimateInSpinners());
        cards[1].GetComponent<CardAnimate>().ActivateCard();

        cards[0].GetComponent<Image>().sprite = unit1.GetMonster().cardSprite;
        cards[1].GetComponent<Image>().sprite = unit2.GetMonster().cardSprite;


        model1 = Instantiate(unit1.GetMonster().monsterModel, modelHolder1);
        model1.transform.localScale = new Vector3(130f, 130f, 130f);
        model1.transform.localRotation = modelHolder1.localRotation;
        model1.transform.localEulerAngles = new Vector3(0, 180, 0);


        model2 = Instantiate(unit2.GetMonster().monsterModel, modelHolder2);
        model2.transform.localScale = new Vector3(130f, 130f, 130f);
        model2.transform.localRotation = modelHolder2.localRotation;
        model2.transform.localEulerAngles = new Vector3(0, 180, 0);
    }


    private void AnimateInSpinners()
    {
        blackOverlayImage.DOFade(.8f, .3f);
        spinners[0].GetComponent<CardAnimate>().ActivateCard();
        spinners[1].GetComponent<CardAnimate>().ActivateCard(() => Stage1());
        decoSlides[0].transform.DOLocalMoveX(0, .5f);
        decoSlides[1].transform.DOLocalMoveX(0, .5f);
        spinSelector.DOScale(1f, .5f);
    }

    private async Task SpinSpinner(PieChart pieChart, int unitSpinNum, bool apply180Offset) //Spinner 2 (Top) needs offset
    {
        float spinToNum = 360 * (unitSpinNum / 96f);
        float spinnerOffset = 0;
        if (apply180Offset)
        {
            spinnerOffset = 180;
        }
        await pieChart.transform.DOLocalRotate(new Vector3(0, 0, spinToNum + 1 + 720f + spinnerOffset), 1.5f, RotateMode.FastBeyond360).AsyncWaitForCompletion();
    }

    private void AddAbilitesToList(Unit unit, int currentBattleIndex)
    {
        unit.currentBattleIndex = currentBattleIndex;
        foreach (Ability ability in unit.allAbilites)
        {
            allBattleAbilities.Add(ability);
        }
    }

    public AttackMove GetAttackMove(int unitIndex)
    {
        return spinners[unitIndex].GetAttackMove(unitSpinNum[unitIndex]);
    }

    private async Task ExecuteAbilites(StageOfBattle stage)
    {
        for (int i = 0; i < allBattleAbilities.Count; i++)
        {
            if (stage == StageOfBattle.BeforeFirstSpin)
            {
                await allBattleAbilities[i].OnBeforeFirstSpin();
            }
            if (stage == StageOfBattle.AfterFirstSpin)
            {
                await allBattleAbilities[i].OnAfterFirstSpin();
            }
            if (stage == StageOfBattle.WedgeAbilitiesAfterFinalSpin)
            {
                await allBattleAbilities[i].OnWedgeAbilitesAfterFinalSpin(unitThatWon, unitThatLost);
            }
        }
    }


    private async void Stage1() //First spin and assign values
    {
        stageOfBattle = StageOfBattle.BeforeFirstSpin;
        await AddInEnergyBoosts(allUnits[0]);
        await AddInEnergyBoosts(allUnits[1]);
        await ExecuteAbilites(stageOfBattle);
        //FIRST SPIN
        if (canUnitSpin[0])
            _ = SpinSpinner(spinners[0], unitSpinNum[0], true);
        if (canUnitSpin[1])
            await SpinSpinner(spinners[1], unitSpinNum[1], false);

        //UPDATE TEXT FIRST SPIN & ASSIGN VALUES
        for (int i = 0; i < 2; i++)
        {
            titleText[i].text = spinners[i].GetAttackMove(unitSpinNum[i]).title;

            _ = UpdateDamage(i, spinners[i].GetAttackMove(unitSpinNum[i]).damage);
        }
        //multiplierText[0].text = spinners[0].GetAttackMove(unitSpinNum[0]).title; TODO: Add the multiplier value in. Bonus mulitiplier for type advantage and such.

        await Task.Delay(1000);
        stageOfBattle = StageOfBattle.AfterFirstSpin;
        await ExecuteAbilites(stageOfBattle);

        if (attackCardAbility != null)
        {
            //Activate attack card ability
        }
        else
        {
            Stage2();
        }
    }

    private async void Stage2() //Who Won Color
    {
        //stage = 2;


        int unitThatWonColorBattle = WhoWonColorBattle(spinners[0].GetAttackMove(unitSpinNum[0]), spinners[1].GetAttackMove(unitSpinNum[1]));
        AttackMove winningMove = null;

        switch (unitThatWonColorBattle)
        {
            default:
                break;
            case 0: //Tie
                Stage3();
                break;
            case 1: //Winner
            case 2: //Winner
                winningMove = GetAttackMove(unitThatWonColorBattle - 1);
                AssignWinner(unitThatWonColorBattle, winningMove);
                break;
            case 3: //Tie but abilites can launch
                AssignWinner(0, null);
                break;
        }

    }

   


    private async void Stage3() //Who Won Damage Compare
    {
        //stage = 3;




        await CheckForMaxEnergyBonus();


        //Do ability and type advantage buffs/nerfs


        int winner = 0;
        AttackMove winningMove = null;




        WhoWonDamageBattle(damageTotals[0], damageTotals[1], out winningMove, out winner);

        //Add Wedge Abilities
        if (GetAttackMove(0).hasAbility)
        {
            allBattleAbilities.Add(GetAttackMove(0).ability);
        }

        if (GetAttackMove(1).hasAbility)
        {
            allBattleAbilities.Add(GetAttackMove(1).ability);
        }

        AssignWinner(winner, winningMove);
    }


    private async void Stage4() //End Stage
    {
        //stage = 4;


        //Wedge Abilities Assign
        stageOfBattle = StageOfBattle.WedgeAbilitiesAfterFinalSpin;
        await ExecuteAbilites(stageOfBattle);


        StartCoroutine(EndBattleAnimations());
    }


    private void AssignWinner(int winner, AttackMove winningMove)
    {
        unitIndexThatWon = winner;
        if (winner == 1)
        {
            unitThatWon = allUnits[0];
            if (winningMove != null)
            {
                if (winningMove.DoesMoveLoserToBench())
                {
                    unitThatLost = allUnits[1];
                }
            }
            
        }
        else if (winner == 2)
        {
            unitThatWon = allUnits[1];
            if (winningMove != null)
            {
                if (winningMove.DoesMoveLoserToBench())
                {
                    unitThatLost = allUnits[0];
                }
            }
        }
        else
        {
            //Tie
        }
        Stage4();
    }



    IEnumerator EndBattleAnimations()
    {
        yield return new WaitForSeconds(1f);

        //Reverse Spinners
        spinners[0].GetComponent<CardAnimate>().ReverseCardOut();
        spinners[1].GetComponent<CardAnimate>().ReverseCardOut();
        spinSelector.DOScale(0f, .3f);

        //Fade out black
        blackOverlayImage.DOFade(0f, .3f);

        //Animate Loser and Winner Cards
        if (unitIndexThatWon == 1)
        {
            cards[0].GetComponent<CardAnimate>().GoCenter();
            cards[1].GetComponent<CardAnimate>().ReduceScale();
        }
        else if(unitIndexThatWon == 2)
        {
            cards[1].GetComponent<CardAnimate>().GoCenter();
            cards[0].GetComponent<CardAnimate>().ReduceScale();
        }
        

        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
        gm.AfterAttackAnimationMenuActions(); //AFTER BATTLE GO HERE ------------------------------------------------------------------------------------------------------------------
    }


    public async Task UpdateDamage(int unitNum, int damage)
    {
        int oldNum = damageTotals[unitNum];
        damageTotals[unitNum] += damage;
        await damageText[unitNum].DOCounter(oldNum, damageTotals[unitNum], .5f).AsyncWaitForCompletion();
    }



    //BATTLE FUNCS
    private void WhoWonDamageBattle(int unit1, int unit2, out AttackMove winningMove, out int unitWinner)
    {
        //ATTACK WON
        if (unit1 > unit2)
        {
            unitWinner = 1;
            winningMove = spinners[0].GetAttackMove(unitSpinNum[0]);
        }
        //ATTACKER LOST
        else if (unit1 < unit2)
        {
            unitWinner = 2;
            winningMove = spinners[1].GetAttackMove(unitSpinNum[1]);

        }
        //TIE
        else
        {
            unitWinner = 0;
            winningMove = null;
        }
    }

    private int WhoWonColorBattle(AttackMove a1, AttackMove a2)
    {
        int unitThatWon = 0;
        switch (a1.attackColor)
        {
            case AttackColor.None:
                unitThatWon = 0;
                break;
            case AttackColor.White:
                unitThatWon = ColorCode(a2.attackColor, white);
                break;
            case AttackColor.Gold:
                unitThatWon = ColorCode(a2.attackColor, gold);
                break;
            case AttackColor.Purple:
                unitThatWon = ColorCode(a2.attackColor, purple);
                break;
            case AttackColor.Red:
                unitThatWon = ColorCode(a2.attackColor, red);
                break;
            case AttackColor.Blue:
                unitThatWon = ColorCode(a2.attackColor, blue);
                break;
            case AttackColor.Green:
                unitThatWon = ColorCode(a2.attackColor, green);
                break;
            case AttackColor.Pink:
                unitThatWon = ColorCode(a2.attackColor, pink);
                break;
            default:
                return 0;
        }

        if (unitThatWon == 1 || unitThatWon == 3)
        {
            if (a1.hasAbility)
            {
                allBattleAbilities.Add(a1.ability);
            }
        }
        if (unitThatWon == 2 || unitThatWon == 3)
        {
            if (a2.hasAbility)
            {
                allBattleAbilities.Add(a2.ability);
            }
        }
        return unitThatWon;
    }


    //3: Win for both teams, both activate abilites
    //  0: Tie,  1: Win,  2: Lose    White,  Gold,  Purple,  Red,  Blue,  Green,  Pink   //Score Ranking  //Close to 0 is balanced
    public int[] white = new int[7] {   0,     0,      2,      1,     2,     0,     0 }; //      -3

    //  0: Tie,  1: Win,  2: Lose    White,  Gold,  Purple,  Red,  Blue,  Green,  Pink
    public int[] gold = new int[7]  {   0,     0,      1,      1,     2,     1,     2 }; //      -1

    //  0: Tie,  1: Win,  2: Lose    White,  Gold,  Purple,  Red,  Blue,  Green,  Pink
    public int[] purple = new int[7] {  1,     2,      0,      1,     2,     0,     1 }; //      -1

    //  0: Tie,  1: Win,  2: Lose    White,  Gold,  Purple,  Red,  Blue,  Green,  Pink
    public int[] red = new int[7]   {   2,     2,      2,      3,     2,     2,     2 }; //      -9

    //  0: Tie,  1: Win,  2: Lose    White,  Gold,  Purple,  Red,  Blue,  Green,  Pink
    public int[] blue = new int[7]  {   1,     1,      1,      1,     3,     1,     2 }; //      6

    //  0: Tie,  1: Win,  2: Lose    White,  Gold,  Purple,  Red,  Blue,  Green,  Pink
    public int[] green = new int[7] {   0,     2,      0,      1,     2,     0,     0 }; //      -3

    //  0: Tie,  1: Win,  2: Lose    White,  Gold,  Purple,  Red,  Blue,  Green,  Pink
    public int[] pink = new int[7]  {   0,     1,      2,      1,     1,     0,     0 }; //      -1




    public int ColorCode(AttackColor ac, int[] color)
    {
        switch (ac)
        {
            case AttackColor.None:
                return 0;
            case AttackColor.White:
                return color[0];
            case AttackColor.Gold:
                return color[1];
            case AttackColor.Purple:
                return color[2];
            case AttackColor.Red:
                return color[3];
            case AttackColor.Blue:
                return color[4];
            case AttackColor.Green:
                return color[5];
            case AttackColor.Pink:
                return color[6];
            default:
                return 0;
        }
    }





    private async Task CheckForMaxEnergyBonus()
    {
        //Unit 0, Normal, Dark
        if (allUnits[0].GetMonster().spirit == Spirit.Normal && allUnits[0].activateEnergyBoost)
        {
            multiplierText[0].text = "2x";
            await UpdateDamage(0, damageTotals[0]); //Adding to the totals again essentially doubles the damage
        }

        //Unit 1, Normal, Dark
        if (allUnits[1].GetMonster().spirit == Spirit.Normal && allUnits[1].activateEnergyBoost)
        {
            multiplierText[1].text = "2x";
            await UpdateDamage(1, damageTotals[1]); //Adding to the totals again essentially doubles the damage
        }
    }






    public Unit GetOtherUnit(Unit unit)
    {
        if (allUnits[0] == unit)
        {
            return allUnits[1];
        }
        else
        {
            return allUnits[0];
        }
    }

    public void UnitIsFrozen(int battleIndex)
    {
        canUnitSpin[battleIndex] = false;
        iceOverlay[battleIndex].SetActive(true);
        titleText[battleIndex].text = "Miss";
        spinners[battleIndex].MakeEntireWheelMiss();
    }




    public int GetTotalDamageByUnit(int unitNormalIndex)
    {
        int damageToReturn = 0;
        Unit unit = null;
        if (unitNormalIndex == allUnits[0].ownerPlayer.index)
        {
            unit = allUnits[0];
        }
        else
        {
            unit = allUnits[1];
        }

        damageToReturn = damageTotals[unit.currentBattleIndex];

        return damageToReturn;
    }

    private async Task AddInEnergyBoosts(Unit unit)
    {
        if (unit.activateEnergyBoost)
        {
            await spinners[unit.currentBattleIndex].EnergyBoostUpgrade(unit.GetMonster().spirit);
        }
    }





    public enum StageOfBattle { BeforeFirstSpin, AfterFirstSpin, WedgeAbilitiesAfterFinalSpin }

}