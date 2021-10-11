using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackNode
{

    [GUIColor("GetColor")]
    public AttackColor attackColor;
    public int damage;
    //[ShowIf("attackColor", AttackColor.Purple)]
    //public Ability triggerAbility;

    public AttackNode(AttackColor attackColor, int damage)
    {
        this.attackColor = attackColor;
        this.damage = damage;
        //this.triggerAbility = triggerAbility;
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
            default:
                return Color.gray;
        }
    }
}

