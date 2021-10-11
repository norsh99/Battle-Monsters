using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindAllNodesInRange
{
    List<Node> finalAvailableNodes = new List<Node>();
    List<Node> allClickableNodes = new List<Node>();

    bool canFly = false;
    public Node finalNodeInPath = null;

    public FindAllNodesInRange(Node startNode, int distance, bool canFly, bool isBenched)
    {
        this.canFly = canFly;
        if (isBenched)
        {
            this.canFly = false;
        }

        List<Node> finalAvailableNodes = new List<Node>();

        FindNodesWithinDistance(startNode, distance);
    }

    private void FindNodesWithinDistance(Node startNode, int distance)
    {
        List<Node> openNodes = new List<Node>();
        List<Node> prefNodes = new List<Node>();

        List<Node> closedNodes = new List<Node>();

        openNodes.Add(startNode);
        for (int i = 0; i < distance + 1; i++)
        {
            for (int m = 0; m < openNodes.Count; m++)
            {
                Node currentNode = openNodes[m];
                //openHexes.RemoveAt(0);
                if (!closedNodes.Contains(currentNode))
                {
                    closedNodes.Add(currentNode);

                }

                foreach (Node neighbor in currentNode.GetNeighbors())
                {
                    if (!closedNodes.Contains(neighbor) || !prefNodes.Contains(neighbor) || !openNodes.Contains(neighbor))
                    {
                        if (neighbor.currentUnit == null)
                        {
                            prefNodes.Add(neighbor);
                            allClickableNodes.Add(neighbor); 
                        }
                        //FOR UNITS THAT CAN FLY
                        else if(canFly && i < distance)
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
        closedNodes.RemoveAt(0);

        finalAvailableNodes = closedNodes;
    }

    public List<Node> GetAllNodes()
    {
        return finalAvailableNodes;
    }

    public List<Node> GetClickableNodes()
    {
        return allClickableNodes;
    }

    public bool IsNodeAMoveableSpot(Node node)
    {
        return allClickableNodes.Contains(node);
    }

    public bool AreEnemysInSight(Player ownerPlayer)
    {
        Debug.Log("Amount: " + allClickableNodes.Count);

        for (int i = 0; i < allClickableNodes.Count; i++)
        {
            if (allClickableNodes[i].currentUnit != null)
            {
                Debug.Log("In 1");
                if (ownerPlayer != allClickableNodes[i].currentUnit.ownerPlayer)
                {
                    Debug.Log("In 2");
                    return true;
                }
            }
        }
        return false;
    }



    //PATHFINDING
    public List<Node> FindPath(Node startNode, Node targetNode)
    {
        List<Node> openHexes = new List<Node>();
        List<Node> closedHexes = new List<Node>();

        openHexes.Add(startNode);
        while (true)
        {
            Node currentHex = openHexes[0];
            openHexes.RemoveAt(0);
            closedHexes.Add(currentHex);
            if (currentHex == targetNode)
            {
                return TracePathAndReturnPathToList(currentHex, startNode);
            }

            foreach (Node neighbor in currentHex.GetNeighbors())
            {
                if (!closedHexes.Contains(neighbor))
                {
                    if (!openHexes.Contains(neighbor))
                    {
                        if (neighbor.currentUnit == null)
                        {
                            neighbor.saerchPrevNode = currentHex;
                            openHexes.Add(neighbor);
                        }
                        //FOR UNITS THAT CAN FLY
                        else if(canFly)
                        {
                            neighbor.saerchPrevNode = currentHex;
                            openHexes.Add(neighbor);
                        }
                    }
                }
            }
        }
    }
    private List<Node> TracePathAndReturnPathToList(Node endHex, Node startHex)
    {
        List<Node> finalHexPath = new List<Node>();
        Node currentHex = endHex;
        while (true)
        {
            finalHexPath.Add(currentHex);
            if (currentHex == startHex)
            {
                break;
            }
            currentHex = currentHex.saerchPrevNode;
        }
        finalNodeInPath = finalHexPath[0];
        finalHexPath.Reverse();
        return finalHexPath;
    }


    public List<Node> GetPath(Node startNode, Node targetNode)
    {
        List<Node> newPath = FindPath(startNode, targetNode);

        return newPath;
    }


}
