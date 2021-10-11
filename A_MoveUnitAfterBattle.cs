using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Threading;

[CreateAssetMenu(menuName = "Abilities/Move Unit After Battle")]
public class A_MoveUnitAfterBattle : Ability
{

    [HideInInspector] public bool enableAction;
    [HideInInspector] public bool isMoveOver;

    public override void Innitialize(Unit ownerUnit, GameMaster gm)
    {
        base.Innitialize(ownerUnit, gm);
        enableAction = false;
        isMoveOver = false;
    }


    public async override Task OnWedgeAbilitesAfterFinalSpin(Unit winnerUnit, Unit loserUnit)
    {
        if (GetGameMaster() == null)
        {
            Debug.Log("No Game Master Reference...");
        }

        GetGameMaster().battleScreen.endBattleAbilities.Add(this);
    }



    public async override Task OnLeaveBattle()
    {
        GetGameMaster().nodesInRange = new FindAllNodesInRange(GetUnitOwner().currentNode, 0, false, false);
        GetGameMaster().ColorMultipleNodes(GetGameMaster().nodesInRange.GetClickableNodes(), true);
        GetGameMaster().gameState = GameState.AbilityMove;
        GetGameMaster().currentSelection = GetUnitOwner();
        GetGameMaster().descriptionBoxPopUp.LoadInDescriptionPopUp(this, title, description, true);
        GetUnitOwner().SelectOutline(true);

        Debug.Log("Waiting...");


        while (!GetGameMaster().isTaskComplete)
        {
            await Task.Delay(1000);
        }
    }

    public override bool RemoveAbility()
    {
        GetGameMaster().DeselectUnit(GetGameMaster().currentSelection);
        return true;
    }
}
