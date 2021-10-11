using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Damage Buff To Ally If On Board")]
public class A_DamageBuffToAllyIfOnBoard : Ability
{
    public int buffAmount;


    //public override void OnEnterBattle(Unit activatingUnit, bool isAttacking)
    //{
    //    //if (isAttacking && attack.attackerBonusDamage != -1)
    //    //{
    //    //    attack.attackerBonusDamage += buffAmount;

    //    //}
    //    //else if(attack.defenderBonusDamage != -1)
    //    //{
    //    //    attack.defenderBonusDamage += buffAmount;
    //    //}
    //}
}
