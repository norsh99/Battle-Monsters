using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Attack of Opportunity")]
public class A_AttackOfOpportunity : Ability
{
    [InfoBox(" Level 1: +5dmg, \n Level 2: +10dmg, \n Level 3: +15dmg, \n Level 4: +20dmg", InfoMessageType.None)]
    [PropertyRange(1, 4)]
    public int level;

    public async override Task OnAfterFirstSpin()
    {
        int bonusDamage = 0;

        if (GetGameMaster().battleScreen.GetOtherUnit(GetUnitOwner()).waitTimer > 0)
        {
            switch (level)
            {

                case 1:
                    bonusDamage = 5;
                    break;
                case 2:
                    bonusDamage = 10;
                    break;
                case 3:
                    bonusDamage = 15;
                    break;
                case 4:
                    bonusDamage = 20;
                    break;

                default:
                    break;
            }
        }
        await GetGameMaster().battleScreen.UpdateDamage(GetUnitOwner().currentBattleIndex, bonusDamage);
    }
}
