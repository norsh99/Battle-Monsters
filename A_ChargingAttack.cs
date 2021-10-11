using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Charging Attack")]
public class A_ChargingAttack : Ability
{

    public int attackBuff;

    [EnumToggleButtons]
    public WhichColors whichColors;


    public override async Task OnAfterFirstSpin()
    {
        GameMaster gm = GetGameMaster();
        if (gm.canMove == false && GetUnitOwner().ownerPlayer == gm.currentPlayersTurn)
        {
            switch (whichColors)
            {
                case WhichColors.White:
                    if (gm.battleScreen.GetAttackMove(GetUnitOwner().currentBattleIndex).attackColor == AttackColor.White)
                    {
                        await gm.battleScreen.UpdateDamage(GetUnitOwner().currentBattleIndex, attackBuff);
                    }
                    break;
                case WhichColors.Gold:
                    if (gm.battleScreen.GetAttackMove(GetUnitOwner().currentBattleIndex).attackColor == AttackColor.Gold)
                    {
                        await gm.battleScreen.UpdateDamage(GetUnitOwner().currentBattleIndex, attackBuff);
                    }
                    break;
                case WhichColors.Purple:
                    if (gm.battleScreen.GetAttackMove(GetUnitOwner().currentBattleIndex).attackColor == AttackColor.Purple)
                    {
                        await gm.battleScreen.UpdateDamage(GetUnitOwner().currentBattleIndex, attackBuff);
                    }
                    break;
                case WhichColors.WhiteGold:
                    if (gm.battleScreen.GetAttackMove(GetUnitOwner().currentBattleIndex).attackColor == AttackColor.White || gm.battleScreen.GetAttackMove(GetUnitOwner().currentBattleIndex).attackColor == AttackColor.Gold)
                    {
                        await gm.battleScreen.UpdateDamage(GetUnitOwner().currentBattleIndex, attackBuff);
                    }
                    break;
                case WhichColors.All:
                    await gm.battleScreen.UpdateDamage(GetUnitOwner().currentBattleIndex, attackBuff);
                    break;
                default:
                    break;
            }
        }
    }

    public enum WhichColors { White, Gold, Purple, WhiteGold, All }
}

