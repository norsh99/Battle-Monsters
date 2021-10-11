using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackMove
{
    public int wedgeSize;
    public string title;
    public int damage;
    [GUIColor("GetColor")]
    public AttackColor attackColor;
    [OnValueChanged("ResetToNull")]
    public bool hasAbility;
    [ShowIf("hasAbility", true)]
    public Ability ability;

    public AttackMove(AttackMove attackMove)
    {
        wedgeSize = attackMove.wedgeSize;
        title = attackMove.title;
        damage = attackMove.damage;
        attackColor = attackMove.attackColor;
        hasAbility = attackMove.hasAbility;
        ability = attackMove.ability;
    }


    public bool DoesMoveLoserToBench()
    {
        bool moveToBench = true;
        if (attackColor == AttackColor.Purple || attackColor == AttackColor.Blue || attackColor == AttackColor.Red)
        {
            moveToBench = false;
        }

        return moveToBench;
    }

    private void ResetToNull()
    {
        if (!hasAbility)
        {
            ability = null;
        }
    }

    private Color GetColor()
    {
        switch (attackColor)
        {
            case AttackColor.White:
                return new Color(.99f, 0.99f, 0.99f, 1f);
            case AttackColor.Gold:
                return new Color(1f, 0.764f, 0.239f, 1f);
            case AttackColor.Purple:
                return new Color(0.595f, 0.296f, 1f, 1f);
            case AttackColor.Red:
                return new Color(1f, 0.277f, 0.277f, 1f);
            case AttackColor.Blue:
                return new Color(0.11f, 0.878f, 0.996f, 1f);
            case AttackColor.Green:
                return new Color(0.408f, 0.996f, 0f, 1f);
            case AttackColor.Pink:
                return new Color(1f, 0.504f, 0.822f, 1f);
            default:
                return Color.gray;
        }
    }
}
