//using DG.Tweening;
//using Sirenix.OdinInspector;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using System;

//public class BattleManager : MonoBehaviour
//{

//    [SerializeField] private List<GameObject> cards;
//    [SerializeField] private Transform modelHolder1;
//    [SerializeField] private Transform modelHolder2;

//    private GameObject model1;
//    private GameObject model2;

//    //BATTLE UI
//    [SerializeField] private SpriteRenderer blackOverlayImage;
//    [SerializeField] private List<GameObject> decoSlides;
//    [SerializeField] private List<GameObject> damageCard;
//    [SerializeField] private List<TextMeshPro> damageText;
//    [SerializeField] private List<TextMeshPro> multiplierText;
//    [SerializeField] private GameObject criticalHitGO;



//    [SerializeField] private List<MonsterAttackSquare> monsterAttackSquares;
//    [SerializeField] private GameObject middleSquare;

//    private int attackCellLocation;
//    private List<Unit> units; //Player 1 should be listed in slot 1 reguardless of attack or defend.
//    private int attackerUnitIndex;
//    private Attack latestAttack;
//    private Action doneWithAnimationAction;
//    public bool criticalHit;


//    //CARD GOs
//    private GameObject cardGO1;
//    private GameObject cardGO2;



//    [Button("Activate Battle")]
//    public void ActivateBattle(Attack attack, Action doneWithAnimationAction)
//    {
//        this.doneWithAnimationAction = doneWithAnimationAction;
//        latestAttack = attack;

//        //RESET
//        gameObject.SetActive(true);
//        blackOverlayImage.DOFade(0f, 0f);
//        decoSlides[0].transform.localPosition = new Vector3(-7f, decoSlides[0].transform.localPosition.y, decoSlides[0].transform.localPosition.z);
//        decoSlides[1].transform.localPosition = new Vector3(7f, decoSlides[1].transform.localPosition.y, decoSlides[1].transform.localPosition.z);
//        middleSquare.transform.DOScale(0, 0);
//        damageCard[0].SetActive(false);
//        damageCard[1].SetActive(false);
//        cards[0].transform.localScale = new Vector3(.1f, .1f, .1f);
//        cards[1].transform.localScale = new Vector3(.1f, .1f, .1f);
//        multiplierText[0].text = "";
//        multiplierText[1].text = "";
//        criticalHitGO.SetActive(false);

//        if (cardGO1 != null)
//        {
//            Destroy(cardGO1);
//        }
//        if (cardGO2 != null)
//        {
//            Destroy(cardGO2);
//        }


//        if (units == null)
//        {
//            units = new List<Unit>();
//        }
//        else
//        {
//            units.Clear();
//        }


//        //START
//        if (attack.attackUnit.ownerPlayer.index == 1)
//        {
//            units.Add(attack.attackUnit);
//            units.Add(attack.defendUnit);
//            attackerUnitIndex = 0;
//        }
//        else
//        {
//            units.Add(attack.defendUnit);
//            units.Add(attack.attackUnit);
//            attackerUnitIndex = 1;
//        }

//        cards[0].GetComponent<CardAnimate>().ActivateCard(() => CardAnimateInComplete());
//        cards[1].GetComponent<CardAnimate>().ActivateCard();

//        cards[0].GetComponent<SpriteRenderer>().sprite = units[0].GetMonster().cardSprite;
//        cards[1].GetComponent<SpriteRenderer>().sprite = units[1].GetMonster().cardSprite;

        

//        if (model1 != null)
//        {
//            Destroy(model1);
//        }
//        if (model2 != null)
//        {
//            Destroy(model2);
//        }
//        cardGO1 = Instantiate(units[0].GetMonster().monsterModel, modelHolder1);
//        cardGO1.transform.eulerAngles = new Vector3(0, 180, 0);
//        cardGO2 = Instantiate(units[1].GetMonster().monsterModel, modelHolder2);
//        cardGO2.transform.eulerAngles = new Vector3(0, 180, 0);



//        //SET ATTACK SQUARES
//        monsterAttackSquares[0].AssignValues(units[0]);
//        monsterAttackSquares[1].AssignValues(units[1]);
//        attackCellLocation = attack.attackLocation;

//    }

//    private void CardAnimateInComplete()
//    {
//        FadeInBlackOverlay();
//    }


//    private void FadeInBlackOverlay()
//    {
//        blackOverlayImage.DOFade(.8f, .3f).OnComplete(SlideInDecoSlides);
//    }


//    private void SlideInDecoSlides()
//    {
//        decoSlides[0].transform.DOLocalMoveX(0, .3f);
//        decoSlides[1].transform.DOLocalMoveX(0, .3f);
//        middleSquare.transform.DOScale(.25f, .5f);
//        middleSquare.GetComponent<CenterSquareAnimate>().AnimateSquare(attackCellLocation, () => StartCoroutine(EnableDamageCardsAndWait()));
//    }

//    IEnumerator EnableDamageCardsAndWait()
//    {
//        //Establish Selector
//        monsterAttackSquares[0].SetSelector(attackCellLocation);
//        monsterAttackSquares[1].SetSelector(attackCellLocation);

//        //Reveal Damage Cards
//        damageCard[0].SetActive(true);
//        damageCard[1].SetActive(true);
//        //damageText[0].text = units[0].attackCluster.GetAttackNode(attackCellLocation).damage.ToString(); OLD
//        //damageText[1].text = units[1].attackCluster.GetAttackNode(attackCellLocation).damage.ToString(); OLD
//        damageText[0].text = units[0].GetAttackDamage(attackCellLocation).ToString();
//        damageText[1].text = units[1].GetAttackDamage(attackCellLocation).ToString();
//        yield return new WaitForSeconds(1f);

//        //Reveal Critical Hit (if applicable)
//        float timeExtend = 0;
//        if (attackCellLocation == 5)
//        {
//            timeExtend = 1;
//            criticalHitGO.SetActive(true);
//        }
//        yield return new WaitForSeconds(timeExtend);


//        int attackerCurrentDamage = units[attackerUnitIndex].attackCluster.GetAttackNode(attackCellLocation).damage;
//        int defenderIndex = 0;
//        if (attackerUnitIndex == 0)
//        {
//            defenderIndex = 1;
//        }
//        int defenderCurrentDamage = units[defenderIndex].attackCluster.GetAttackNode(attackCellLocation).damage;

//        //Damage Multiplier Animation
//        float extendTime = 0;

//        if (latestAttack.attackerMultiplyBonusDamage > 0)
//        {
//            extendTime = 1;
//            int newDamageNum = attackerCurrentDamage * latestAttack.attackerMultiplyBonusDamage;
//            damageText[attackerUnitIndex].DOCounter(attackerCurrentDamage, newDamageNum, .5f);
//            multiplierText[attackerUnitIndex].text = latestAttack.attackerMultiplyBonusDamage + "x";
//            attackerCurrentDamage = newDamageNum;

//        }
//        if (latestAttack.defenderMultiplyBonusDamage > 0)
//        {
//            extendTime = 1;
//            int newDamageNum = defenderCurrentDamage * latestAttack.defenderMultiplyBonusDamage;
//            damageText[defenderIndex].DOCounter(defenderCurrentDamage, newDamageNum, .5f);
//            multiplierText[defenderIndex].text = latestAttack.defenderMultiplyBonusDamage + "x";
//            defenderCurrentDamage = newDamageNum;
//        }
//        yield return new WaitForSeconds(1f + extendTime);


//        //Damage Addition Animation
//        float extendTime2 = 0;

//        if (latestAttack.attackerBonusDamage > 0)
//        {
//            extendTime2 = 1;
//            int newDamageNum = attackerCurrentDamage + latestAttack.attackerBonusDamage;
//            damageText[attackerUnitIndex].DOCounter(attackerCurrentDamage, newDamageNum, .5f);
//            multiplierText[attackerUnitIndex].text = "+" + latestAttack.attackerBonusDamage;
//            attackerCurrentDamage = newDamageNum;
//        }
//        if (latestAttack.defenderBonusDamage > 0)
//        {
//            extendTime2 = 1;
//            int newDamageNum = defenderCurrentDamage + latestAttack.defenderBonusDamage;
//            damageText[defenderIndex].DOCounter(defenderCurrentDamage, newDamageNum, .5f);
//            multiplierText[defenderIndex].text = "+" + latestAttack.defenderBonusDamage;
//            defenderCurrentDamage = newDamageNum;
//        }


//        yield return new WaitForSeconds(extendTime2);
//        StartCoroutine(DoneWithAttackAnimation());
//    }

//    IEnumerator DoneWithAttackAnimation()
//    {

//        //Reverse Deco Cards
//        decoSlides[0].GetComponent<CardAnimate>().ReverseCardOut();
//        decoSlides[1].GetComponent<CardAnimate>().ReverseCardOut();
//        middleSquare.transform.DOScale(0f, .5f);

//        //Fade out black
//        blackOverlayImage.DOFade(0f, .3f);

//        //Animate Loser and Winner Cards
//        GameObject winnerGO = GetCardBasedOnWinner(true);
//        GameObject loserGO = GetCardBasedOnWinner(false);

//        //TIE
//        if (winnerGO == null)
//        {

//        }
//        else
//        {
//            winnerGO.transform.DOLocalMove(new Vector3(0, 0, 5f), 1.5f); //Winner
//            winnerGO.transform.DOScale(winnerGO.transform.localScale.x * 1.5f, 1.5f); //Winner

//            loserGO.transform.localPosition = new Vector3(loserGO.transform.localPosition.x, loserGO.transform.localPosition.y, 6f);//Loser
//            loserGO.transform.DOLocalMove(new Vector3(0, 0, 6f), 1.5f); //Loser
//            loserGO.transform.DOScale(0, 1.5f); //Loser
//        }
        

//        yield return new WaitForSeconds(2f);
//        doneWithAnimationAction();
//        gameObject.SetActive(false);
//    }

//    private GameObject GetCardBasedOnWinner(bool getWinner)
//    {
//        GameObject winner = null;
//        GameObject loser = null;
//        //TIE
//        if (latestAttack.GetWinner() == null)
//        {

//        }
//        //WINNER
//        else if (latestAttack.GetWinner().ownerPlayer.index == 1)
//        {
//            winner = cards[0];
//            loser = cards[1];
//        }
//        //LOSER
//        else
//        {
//            winner = cards[1];
//            loser = cards[0];
//        }


//        if (getWinner)
//        {
//            return winner;
//        }
//        else
//        {
//            return loser;
//        }
//    }
//}
