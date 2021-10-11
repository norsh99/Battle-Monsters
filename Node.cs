using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public List<Node> neighbors;
    [ReadOnly] public Unit currentUnit;
    [SerializeField] private Image image;
    public int id;
    public bool isSidePiece;


    private GameMaster gm;

    //For A* Search
    [ReadOnly] public Node saerchPrevNode;


    public void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    public void SelectSpaceButton()
    {
        if (gm.localGame)
        {
            gm.NodeSelection(this, true);
        }
        else
        {
            if (gm.currentPlayersTurn == gm.localPlayerTurn)
            {
                gm.NodeSelection(this, true);
            }
        }
    }

    public void AssignUnit(Unit unit)
    {
        currentUnit = unit;
        unit.currentNode = this;
    }

    public List<Node> GetNeighbors()
    {
        return neighbors;
    }

    public void ColorNode(Color assignColor)
    {
        image.color = assignColor;
    }

    public bool IsUnitAdjacentToThisNode(Unit unit)
    {
        foreach (Node node in neighbors)
        {
            if (node.currentUnit != null)
            {
                if (node.currentUnit == unit)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
