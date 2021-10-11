using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindEnemys
{

    Node startNode;
    Unit unit;
    List<Node> allEnemys;

    public FindEnemys(Unit unit, int distance)
    {
        this.unit = unit;
        startNode = unit.currentNode;

        allEnemys = new List<Node>();
        SearchForEnemys(startNode, distance);
    }

    private void SearchForEnemys(Node startNode, int distance)
    {
        List<Node> openNodes = new List<Node>();
        List<Node> prefNodes = new List<Node>();
        List<Node> searchedNodes = new List<Node>();

        openNodes.Add(startNode);
        for (int i = 0; i < distance; i++)
        {
            for (int m = 0; m < openNodes.Count; m++)
            {
                Node currentNode = openNodes[m];
                if (!searchedNodes.Contains(currentNode))
                {
                    searchedNodes.Add(currentNode);

                    foreach (Node neighbor in currentNode.GetNeighbors())
                    {
                        if (neighbor.currentUnit != null && neighbor.currentUnit.ownerPlayer != unit.ownerPlayer)
                        {
                            allEnemys.Add(neighbor);
                        }
                        else if(!searchedNodes.Contains(neighbor))
                        {
                            prefNodes.Add(neighbor);
                        }
                    }
                }
            }
            openNodes.Clear();
            openNodes = new List<Node>(prefNodes);
            prefNodes.Clear();
        }
    }

    public List<Unit> GetAdjacentUnits()
    {
        List<Unit> newList = new List<Unit>();

        if (allEnemys.Count > 0)
        {
            foreach (Node node in allEnemys)
            {
                newList.Add(node.currentUnit);
            }
        }
        
        return newList;
    }

    public int GetAllEnemiesCount() { return allEnemys.Count; }

    public List<Node> AllEnemyNodes() { return allEnemys; }
}
