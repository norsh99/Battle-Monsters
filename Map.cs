using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [ShowInInspector]
    [Title("BOARD NODES")]
    public List<Node> team1StartNodes;
    public List<Node> team2StartNodes;
    public List<Node> allNodes;

    //AURAS AND MIDDLE INTERACTION
    [ShowInInspector]
    [Title("AURAS")]
    public List<GameObject> allAuras;
    public List<Node> auraNodes;
    public Player playerThatOwnsTheMiddle;
    public MiddleRingCaptureZone middleRingCaptureZone;

    //SELECTION CIRCLES
    public SelectionCircles selectionCircles;



    //SPAWN SPOTS
    public List<Node> monsterSpawn;

    //EXIT NODE
    public GameObject exitNodeHighlight;


    public bool isDuelMap;

    //AURA NODEs AND MIDDLE CHECKS
    public void SetWhichPlayerOwnsTheMiddle(Player newPlayerControllingTheMiddle)
    {
        if (isDuelMap)
            return;


        playerThatOwnsTheMiddle = newPlayerControllingTheMiddle;
        int splitAuroNodesInHalf = (auraNodes.Count - 1) / 2; //To examine the other half of the aura nodes
        for (int i = 0; i < allAuras.Count; i++)
        {
            allAuras[i].SetActive(false);
        }


        //No team owns the middle
        if (newPlayerControllingTheMiddle == null)
        {
            middleRingCaptureZone.AssignNewColor(3);
        }
        //Player 1 owns middle
        else if (newPlayerControllingTheMiddle.index == 1)
        {
            middleRingCaptureZone.AssignNewColor(1);
            for (int i = splitAuroNodesInHalf; i < auraNodes.Count - 1; i++)
            {
                if (auraNodes[i].currentUnit == null)
                {
                    allAuras[i].SetActive(true);
                }
            }
        }
        //Player 2 owns middle
        else if (newPlayerControllingTheMiddle.index == 2)
        {
            middleRingCaptureZone.AssignNewColor(2);

            for (int i = 0; i < splitAuroNodesInHalf; i++)
            {
                if (auraNodes[i].currentUnit == null)
                {
                    allAuras[i].SetActive(true);
                }
            }
        }
    }

    public void IsPlayerInMiddle()
    {
        if (isDuelMap)
            return;

        Unit unitInMiddle = auraNodes[auraNodes.Count - 1].currentUnit;

        //Check if someone is on the middle node
        if (unitInMiddle != null)
        {
            SetWhichPlayerOwnsTheMiddle(unitInMiddle.ownerPlayer);
        }
        //No one is in the middle node
        else
        {

        }
    }






    //OTHER
    public Node GetNodeFromIndex(int num)
    {
        return allNodes[num];
    }
    public List<Node> GetTeamBenchNodes(Player player)
    {
        if (player.index == 1)
        {
            return team1StartNodes;
        }
        else
        {
            return team2StartNodes;
        }
    }


}
