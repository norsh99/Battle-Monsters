using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Frozen")]
public class A_Frozen : Ability
{

    public async override Task OnBeforeFirstSpin()
    {
        GetGameMaster().battleScreen.UnitIsFrozen(GetUnitOwner().currentBattleIndex);
        await Task.Delay(500);
    }


    public async override Task OnLeaveBattle()
    {
        GetUnitOwner().statue.EnableFrozen();
        //Debug.Log("FROZEN");
    }


    public override bool RemoveAbility()
    {
        GetUnitOwner().statue.TurnOffAllEffects();
        return true;
    }
}
