using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Give Status Effect")]
public class A_GiveStatusEffect : Ability
{
    public Ability giveStatusType; 
    public async override Task OnWedgeAbilitesAfterFinalSpin(Unit winnerUnit, Unit loserUnit)
    {
        Ability newStatus = Instantiate(giveStatusType);
        newStatus.status = Status.Frozen;

        GetGameMaster().battleScreen.GetOtherUnit(winnerUnit).allAbilites.Add(newStatus);
        newStatus.Innitialize(GetGameMaster().battleScreen.GetOtherUnit(winnerUnit), GetGameMaster());
        GetGameMaster().battleScreen.endBattleAbilities.Add(newStatus);
    }




    
}


