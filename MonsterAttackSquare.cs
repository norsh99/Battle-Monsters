using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterAttackSquare : MonoBehaviour
{

    [SerializeField] private Image selector;
    [SerializeField] private List<Image> cells;
    [SerializeField] private List<TextMeshProUGUI> textCells;
    [SerializeField] private List<Color> attackColors;
    [SerializeField] private List<SpriteRenderer> upSprite;
    [SerializeField] private List<SpriteRenderer> downSprite;
    [SerializeField] private List<SpriteRenderer> leftSprite;
    [SerializeField] private List<SpriteRenderer> rightSprite;
    private List<List<SpriteRenderer>> allAttackDirections;

    private AttackCluster attackCluster;
    private Unit unit;


    public void AssignValues(Unit unit)
    {
        if (allAttackDirections == null)
        {
            allAttackDirections = new List<List<SpriteRenderer>>() { upSprite, rightSprite, downSprite, leftSprite };

        }
        this.unit = unit;
        //this.attackCluster = unit.attackCluster; OLD
        selector.gameObject.SetActive(false);

        for (int i = 0; i < cells.Count; i++)
        {
            AttackNode node = attackCluster.attackNodes[i];

            textCells[i].text = node.damage.ToString();


            if (node.attackColor == AttackColor.White)
            {
                cells[i].color = attackColors[(int)ColorSelect.white];
            }
            else if(node.attackColor == AttackColor.Gold)
            {
                cells[i].color = attackColors[(int)ColorSelect.gold];
            }
            else if (node.attackColor == AttackColor.Purple)
            {
                cells[i].color = attackColors[(int)ColorSelect.purple];
            }
            else if (node.attackColor == AttackColor.Red)
            {
                cells[i].color = attackColors[(int)ColorSelect.red];
            }
        }

        //Attack Direction
        //SetAttackDirection(unit.attackDirection, unit.attackDirectionPower); OLD
    }


    public void SetSelector(int cellPosition)
    {
        selector.gameObject.SetActive(true);

        selector.transform.position = cells[cellPosition - 1].transform.position;
    }
   



    //ATTACK DIRECTION

    public void TurnAllAttackDirectionOff()
    {
        for (int i = 0; i < allAttackDirections.Count; i++)
        {
            for (int m = 0; m < allAttackDirections[i].Count; m++)
            {
                allAttackDirections[i][m].gameObject.SetActive(false);
            }
        }
    }

    private void ActivateAttackRow(List<SpriteRenderer> theList, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            theList[i].gameObject.SetActive(true);
        }
    }

    public void SetAttackDirection(AttackDirection attackDirection, int attackAmount)
    {
        TurnAllAttackDirectionOff();
        switch (attackDirection)
        {
            case AttackDirection.Up:
                ActivateAttackRow(upSprite, attackAmount);
                break;
            case AttackDirection.Down:
                ActivateAttackRow(downSprite, attackAmount);
                break;
            case AttackDirection.Left:
                ActivateAttackRow(leftSprite, attackAmount);
                break;
            case AttackDirection.Right:
                ActivateAttackRow(rightSprite, attackAmount);
                break;
            default:
                break;
        }


    }














    private enum ColorSelect { white = 0, gold = 1, purple = 2, red = 3 }
}
