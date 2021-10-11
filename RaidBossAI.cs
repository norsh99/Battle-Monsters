using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using UnityEngine;

public class RaidBossAI : MonoBehaviour
{
    [SerializeField] private GameMaster gm;
    [SerializeField] private RaidBossHealthBar healthBar;

    public Unit raidBossUnit;



    //STATE
    public RaidState raidState;
    public bool agitated;
    public Node exitNode;

    //STATS
    public int health;
    public int maxHealth;


    public void LoadIn(Unit raidBossUnit)
    {
        this.raidBossUnit = raidBossUnit;
        raidState = RaidState.None;
        health = 100;
        maxHealth = 100;
    }


    public async Task Damaged(int amount)
    {
        //LOWER HEALTH
        health += amount;

        if (health <= 0)
        {
            health = 0;
            Debug.Log("Raid boss has fainted, you lose.");
            await raidBossUnit.SkyUnitMove();
            DidPlayerWin(false, "You lose!");
            return;
        }

        await healthBar.UpdateHealth(health, maxHealth);

        Debug.Log("Raid Boss Health: " + health);


        if (!agitated && RandumNum(0, 1) == 0)
        {
            SetAgitated(true);
            await Task.Delay(1000);
        }
    }


    public async void Activate()
    {
        await Task.Delay(300);

        switch (raidState)
        {
            case RaidState.None:
                Debug.Log("None -");
                RaidState_None();
                break;
            case RaidState.RandMove:
                Debug.Log("RandMove -");
                RaidState_RandMove();
                break;
            case RaidState.AgitatedMove:
                Debug.Log("AgitatedMove -");
                RaidState_AgitatedMove();
                break;
            case RaidState.HasMoved:
                Debug.Log("HasMoved -");
                RaidState_HasMoved();
                break;
            case RaidState.Attacking:
                Debug.Log("Attacking -");
                RaidState_Attacking();
                break;
            case RaidState.EndTurn:
                Debug.Log("EndTurn -");
                RaidState_EndTurn();
                break;
            default:
                Debug.Log("Default in the switch statement... ERROR!");
                break;
        }
    }

    public void RaidState_None()
    {

        if (agitated)
        {
            Agitated();
        }
        else
        {
            NonAgitated();
        }
    }

    public void Agitated()
    {
        List<Unit> adjacentUnits = gm.GetAdjacentUnits(raidBossUnit);

        CheckAgitatedNode();

        if (adjacentUnits.Count > 0 && adjacentUnits.Count < 4) //SOMEONE IS NEAR, CHANCE AT MOVING OR ATTACKING --------
        {
            if (RandumNum(0,1) == 1)
            {
                //Attack
                raidState = RaidState.Attacking;
                Activate();
            }
            else
            {
                //Move
                raidState = RaidState.AgitatedMove;
                Activate();
            }
        }
        else if (adjacentUnits.Count == 0) //NO ONE NEAR, MOVE TO EXIT 
        {
            raidState = RaidState.AgitatedMove;
            Activate();
        }
        else //COMPLETELY SURROUNDED, Must attack
        {
            raidState = RaidState.Attacking;
            Activate();
        }


    }

    public void NonAgitated()
    {
        List<Unit> adjacentUnits = gm.GetAdjacentUnits(raidBossUnit);

        if (adjacentUnits.Count > 0 && adjacentUnits.Count < 4) //SOMEONE IS NEAR, CHANCE AT MOVING OR ATTACKING --------
        {
            int randNum = RandumNum(0, 1);
            if (randNum == 0)
            {
                //ATTACK
                raidState = RaidState.Attacking;
                Activate();
            }
            else
            {
                //MOVE
                raidState = RaidState.RandMove;
                Activate();
            }
        }
        else if(adjacentUnits.Count == 0) //NO ONE NEAR, MUST MOVE RANDOMLY --------------------------------------------
        {
            //MOVE
            raidState = RaidState.RandMove;
            Activate();
        }
        else //COMPLETELY SURROUNDED -----------------------------------------------------------------------------------
        {
            //ATTACK
            raidState = RaidState.Attacking;
            Activate();
        }

    }

    public async void RaidState_RandMove()
    {
        if (gm.canMove) //HAVEN'T MOVED YET, GO AND MOVE
        {
            Debug.Log("Can move, randomly");
            gm.NodeSelection(raidBossUnit.currentNode);
            
            await Task.Delay(300);
            Node nodeSelected = FindRandumNodeToMoveTo();
            await Task.Delay(300);


            gm.NodeSelection(nodeSelected);

            raidState = RaidState.HasMoved;
        }
    }

    public async void RaidState_AgitatedMove()
    {
        if (gm.canMove) //HAVEN'T MOVED YET, GO AND MOVE TO EXIT
        {
            gm.NodeSelection(raidBossUnit.currentNode);

            FindAllNodesInRange nodesInRange = gm.nodesInRange;
            List<Node> pathToExit = nodesInRange.FindPath(raidBossUnit.currentNode, exitNode);
            List<Node> noDuplicateNodes = gm.NoDuplicateNodes(nodesInRange.GetClickableNodes());
            List<Node> availableNodesToMoveTo = new List<Node>();

            foreach (Node node in pathToExit)
            {
                if (noDuplicateNodes.Contains(node))
                {
                    availableNodesToMoveTo.Add(node);
                }
            }

            await Task.Delay(300);

            gm.NodeSelection(availableNodesToMoveTo[availableNodesToMoveTo.Count - 1]);

            raidState = RaidState.HasMoved;
        }
    }

    public async void RaidState_HasMoved()
    {
        //MOVED ALREADY AND CAN ATTACK (if possible) OR END TURN
        List<Unit> adjacentUnits = gm.GetAdjacentUnits(raidBossUnit);

        await HasMonsterHasFoundExit();
        
        if (RandumNum(0, 1) == 0 && adjacentUnits.Count > 0)
        {
            //ATTACK
            raidState = RaidState.Attacking;
            Activate();
        }
        else
        {
            //END TURN
            raidState = RaidState.EndTurn;
            Activate();
        }
    }


    public async void RaidState_Attacking()
    {
        List<Unit> adjacentUnits = gm.GetAdjacentUnits(raidBossUnit);
        Node attackThisNode = adjacentUnits[RandumNum(0, adjacentUnits.Count - 1)].currentNode;

        if (gm.currentSelection == raidBossUnit)
        {
            gm.NodeSelection(attackThisNode);
        }
        else
        {
            gm.NodeSelection(raidBossUnit.currentNode);
            await Task.Delay(300);
            gm.NodeSelection(attackThisNode);
        }

        
        raidState = RaidState.EndTurn; //This probably won't matter since after attacking it will be the end of the turn anyways (thus getting reset to none by the next go around)
    }
    public async void RaidState_EndTurn()
    {
        await Task.Delay(300);
        gm.EndTurn(true);
    }


    public async Task HasMonsterHasFoundExit()
    {
        if (raidBossUnit.currentNode == exitNode && agitated)
        {
            Debug.Log("Raid Boss found the exit, end the game.");
            DidPlayerWin(false, "You lose!");
            await raidBossUnit.SkyUnitMove();
        }
    }



    //WIN OR LOSE
    private void DidPlayerWin(bool didPlayerWin, string message)
    {
        if (didPlayerWin)
        {
            //WIN
            gm.EndOfGameUIAnimation(true, message);
        }
        else
        {
            //LOSE
            gm.EndOfGameUIAnimation(false, message);
        }
    }





    //PRIVATE ACTIONS

    private Node FindRandumNodeToMoveTo()
    {
        if (gm.nodesInRange == null)
        {
            Debug.Log("Nodes in Range is null");
        }

        List<Node> canMoveNodes = gm.nodesInRange.GetClickableNodes();
        int randNum = RandumNum(1, canMoveNodes.Count);

        return canMoveNodes[randNum - 1];
    }

    private void CheckAgitatedNode()
    {
        if(exitNode == null || exitNode.currentUnit != null)
        {
            SetAgitated(true);
        }
    }

    private Node CreateNewAgitagedNode()
    {
        List<Node> allEdgeNodesWithoutUnitsOnThem = new List<Node>();

        foreach (Node node in gm.map.allNodes)
        {
            if (node.isSidePiece && node.currentUnit == null)
            {
                allEdgeNodesWithoutUnitsOnThem.Add(node);
            }
        }

        int randNum = RandumNum(1, allEdgeNodesWithoutUnitsOnThem.Count);

        return allEdgeNodesWithoutUnitsOnThem[randNum - 1];
    }

    private void SetAgitated(bool isAgitated)
    {
        if (isAgitated)
        {
            exitNode = CreateNewAgitagedNode();
            agitated = true;
            healthBar.SetAgitatedOutline(true);
            gm.map.exitNodeHighlight.SetActive(true);
            gm.map.exitNodeHighlight.transform.position = exitNode.transform.position;
            raidBossUnit.infoNode.PlayAgitatedAnimation(4000);
        }
        else
        {
            agitated = false;
            healthBar.SetAgitatedOutline(false);
            gm.map.exitNodeHighlight.SetActive(false);
        }
        
    }









    //OTHER
    private int RandumNum(int startNum, int endNum)
    {
        return UnityEngine.Random.Range(startNum, endNum + 1);
    }
}


public enum RaidState { None, RandMove, AgitatedMove, HasMoved, Attacking, EndTurn }