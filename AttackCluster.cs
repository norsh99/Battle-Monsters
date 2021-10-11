using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackCluster
{
    [TableList(ShowIndexLabels = true)]
    public List<AttackNode> attackNodes;

    
    public AttackNode GetAttackNode(int index)
    {
        return attackNodes[index - 1];
    }
}
