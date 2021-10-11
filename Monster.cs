using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Monster", menuName = "Monster")]
public class Monster : ScriptableObject
{
    //DESRIPTIONS
    [BoxGroup("Basic Info")]
    [LabelWidth(100)]
    public string monsterName;

    [BoxGroup("Basic Info")]
    [LabelWidth(100)]
    [TextArea]
    public string description;


    //COST
    [BoxGroup("COST")]
    [LabelWidth(100)]
    public int totalCost;





    //TRAITS
    [BoxGroup("Monster Traits")]
    [LabelWidth(100)]
    public Spirit spirit;

    [BoxGroup("Monster Traits")]
    [LabelWidth(100)]
    public CreatureType type;

    [BoxGroup("Monster Traits")]
    [LabelWidth(100)]
    public bool canFly;

    [BoxGroup("Monster Traits")]
    [LabelWidth(100)]
    [PropertyRange(1, 3)]
    public int speed = 2;

    [BoxGroup("Monster Traits")]
    [LabelWidth(100)]
    [PropertyRange(0, 8)]
    public int startWaitTimer = 0;

    [BoxGroup("Monster Traits")]
    [LabelWidth(100)]
    [PropertyRange(1, 2)]
    public int attackRange = 1;

    [BoxGroup("Monster Traits")]
    [LabelWidth(100)]
    [PropertyRange(1, 10)]
    public int maxEnergy = 10;




    //ABILITIES
    [BoxGroup("Monster Abilities")]
    public List<Ability> abilities;

    //ATTACK MOVES
    [BoxGroup("Attacks")]
    [LabelWidth(100)]
    [TableList(ShowIndexLabels = true)]
    public List<AttackMove> attackMoves;


    [BoxGroup("Attacks")]
    [LabelWidth(100)]
    [ReadOnly] public int totalWedges;


    //ASSETS
    [HorizontalGroup("Game Data", 75)]
    [PreviewField(75)]
    public GameObject monsterModel;

    [HorizontalGroup("Game Data", 75)]
    [PreviewField(75)]
    public Sprite cardSprite;


    [BoxGroup("Attacks")]
    [Button("Update Total Wedges")]
    private void UpdateTotalWedges()
    {
        totalWedges = 0;
        for (int i = 0; i < attackMoves.Count; i++)
        {
            totalWedges += attackMoves[i].wedgeSize;
        }
    }
}
