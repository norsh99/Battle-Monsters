using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string title;

    [LabelWidth(100)]
    [TextArea]
    public string description;
    public Status status;
    [ColorPalette("Ability Colors")]
    public Color backgroundColor;
    private Unit unitOwner;
    private GameMaster gm;

    



    public virtual void Innitialize(Unit ownerUnit, GameMaster gm)
    {
        unitOwner = ownerUnit;
        this.gm = gm;
    }

    public virtual void InstallOnAllUnitsInGame() { } //Coded
    public virtual void OnEnterBoard() { }
    public virtual void OnLeaveBoard() { }

    public virtual async Task OnBeforeFirstSpin() { }
    public virtual async Task OnAfterFirstSpin() { }

    public virtual async Task OnWedgeAbilitesAfterFinalSpin(Unit winnerUnit, Unit loserUnit) { }



    public virtual async Task OnLeaveBattle() { }
    public virtual void OnEndMove() { }
    public virtual void OnSentToBench() { }

    public virtual bool RemoveAbility() { return false; }



    public void SetUnit(Unit unit) { this.unitOwner = unit; }
    public Unit GetUnitOwner()
    {
        return unitOwner;
    }

    public GameMaster GetGameMaster() { return gm; }


}

public enum Status { Ability, None, Frozen, Burnt, NegativeMP1 }