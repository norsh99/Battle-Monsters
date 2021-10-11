using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Cancel Opponent Damage Buff If On Board")]
public class A_CancelOpponentDamageBuffIfOnBoard : Ability
{
    public override void InstallOnAllUnitsInGame()
    {

        foreach (Unit unit in GetGameMaster().allUnits)
        {
            if (GetUnitOwner().ownerPlayer.index != unit.ownerPlayer.index)
            {
                unit.tempAbilites.Add(this);
            }
        }
    }

    //public override void OnEnterBattle(Unit activatingUnit, bool isAttacking)
    //{
    //    //This is a negative feature so if a monster has this ability on him when in a battle negate the bonus. Only if an opponet with this ability is on the field.
    //    //if (activatingUnit != GetUnitOwner() &&  !GetUnitOwner().isBenched)
    //    //{
    //    //    if (isAttacking)
    //    //    {
    //    //        attack.attackerBonusDamage = -1;
    //    //    }
    //    //    else
    //    //    {
    //    //        attack.defenderBonusDamage = -1;

    //    //    }
    //    //}
    //}

}
