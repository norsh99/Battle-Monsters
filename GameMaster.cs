using CM_TaskSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.PlayerLoop;
using UnityEditor;
using DG.Tweening;
using System.Threading;

public class GameMaster : MonoBehaviour
{

    //MAP
    [ShowInInspector]
    [Title("MAP")]
    public Map map;

    public List<Unit> team1Units;
    public List<Unit> team2Units;
    public List<Unit> allUnits;





    //PLAYERS
    [ShowInInspector]
    [Title("PLAYERS")]
    public List<Player> allPlayers;
    [ReadOnly] public Player currentPlayersTurn;
    [ReadOnly] public Player localPlayerTurn;
    [SerializeField] private List<Material> teamMaterialColors;




    //GAME STATUS
    [ShowInInspector]
    [Title("GAME STATUS")]
    [ReadOnly] public bool localGame = true;
    [OdinSerialize, ReadOnly] public GameState gameState = GameState.NoSelect;
    private bool canClickGameBoard = true;

    [ReadOnly] public List<int> unitTotalWait;



    //UNIT SELECTION
    [ShowInInspector]
    [Title("UNIT SELECTION")]
    [ReadOnly] public bool canMove = false;
    [HideInInspector] public Unit currentSelection;
    [SerializeField] private List<Color> selectionNodeColors;
    [HideInInspector] public FindAllNodesInRange nodesInRange;




    //UI 
    [ShowInInspector]
    [Title("UI")]
    public UI_Main UIMain;
    public UI_BattleScreen battleScreen;

    [SerializeField] private UI_EndGamePopupManager endGamePopUpManager;
    public UI_DescriptionBoxPopUp descriptionBoxPopUp;
    public UI_InfoCardTopSection infoCardTopSection;
    [SerializeField] private CanvasGroup uiCirclesCanvasGroup;
    [SerializeField] private GameObject raidBossHealthBar;


    //TASK SYSTEM
    ActionTree actionTree = new ActionTree();
    public ActionLog actionLog;







    //ABLITIES
    public List<Ability> activeAbilites;
    [HideInInspector] public Action liveAbilityActionReturn;

    //AI
    public RaidBossAI raidBoss;


    //TESTING TASK
    public bool isTaskComplete;


    //PLAYBACK MODE
    private bool playbackModeOn = false;

    private void Start()
    {

        LoadIn();

        actionLog = new ActionLog(this, playbackModeOn, "T1S0M7");
        actionLog.nextAction = true;

        if (!actionLog.playbackModeOn)
        {
            actionLog.AddToKeyLog(ActionKey.TeamSelectStartGame, currentPlayersTurn.index);
        }

    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space Key Down");
            actionTree.PlayNextTask();
        }

        actionLog.PlayNextAction();

    }

    private void AICall(bool endTurnChange)
    {
        if (!map.isDuelMap)
            return;

        if (endTurnChange)
        {
            raidBoss.raidState = RaidState.None;
        }

        if (currentPlayersTurn.index == 2)
        {
            raidBoss.Activate();
        }
    }



    //LOAD IN
    private async void LoadIn()
    {
        if (map.isDuelMap)
        {
            LoadInRaid();
            raidBossHealthBar.SetActive(true);
        }
        else
        {
            LoadInNormalGame();
            raidBossHealthBar.SetActive(false);

        }

        canMove = true;
        unitTotalWait = new List<int>() { 3, 3 };
        UIMain.UpdateWaitText();
        map.selectionCircles.TurnOffCircles();
        ResetAllEnergyBoostToFalse();
        endGamePopUpManager.ResetEndGameScreen();


        //VERY LAST ACTION
        await StartGameSetEveryoneToZeroEnergy();
        //Debug.Log("Done with Load In");

        if (map.isDuelMap)
        {
            raidBoss.LoadIn(team2Units[0]);
            await Task.Delay(300);
            AICall(true);
        }
    }

    private void LoadInNormalGame()
    {
        LoadInWhoGoesFirst();
        LoadInMonsters(1, team1Units, map.team1StartNodes, allPlayers[0], teamMaterialColors[0]);
        LoadInMonsters(2, team2Units, map.team2StartNodes, allPlayers[1], teamMaterialColors[1]);
        
        
        map.SetWhichPlayerOwnsTheMiddle(null);
        InstallAbilitiesOnUnitsOnLoadIn(team1Units);
        InstallAbilitiesOnUnitsOnLoadIn(team2Units);
    }


    //LOAD IN RAID
    private void LoadInRaid()
    {
        localGame = false;

        LoadInWhoGoesFirst();
        LoadInMonsters(1, team1Units, map.team1StartNodes, allPlayers[0], teamMaterialColors[0]);
        LoadInMonsters(2, team2Units, map.monsterSpawn, allPlayers[1], teamMaterialColors[1]);

        //InstallAbilitiesOnUnitsOnLoadIn(team1Units);
        //InstallAbilitiesOnUnitsOnLoadIn(team2Units);
        
    }



    private void LoadInMonsters(int ownerIndex, List<Unit> theList, List<Node> nodes, Player player, Material teamColor)
    {
        player.index = ownerIndex;
        for (int i = 0; i < theList.Count; i++)
        {
            if (i < player.monsterDeck.Count)
            {
                theList[i].LoadIn(player.monsterDeck[i], player, teamColor);
                theList[i].transform.position = nodes[i].transform.position;
                nodes[i].AssignUnit(theList[i]);
            }
            else
            {
                theList[i].LoadIn(null, null, teamColor);
            }



            theList[i].ownerPlayer = player;
            theList[i].ownerIndex = player.index;
        }
    }


    private void LoadInWhoGoesFirst()
    {
        int randNum = RandumNum(0, 1);
        currentPlayersTurn = allPlayers[randNum];
        UIMain.ChangeTeamText(randNum + 1);
        localPlayerTurn = allPlayers[0];
    }

    public void OverrideSetTeamAtStart(int playerNum)
    {
        currentPlayersTurn = allPlayers[playerNum];
        UIMain.ChangeTeamText(currentPlayersTurn.index);
        if (localGame)
        {
            localPlayerTurn = allPlayers[playerNum];
        }
    }

    //END TURN
    public async void EndTurn(bool deSelect = false)
    {
        //EnergyForEntireTeamOnBoard(currentPlayersTurn.index); Obsolete
        Debug.Log("End Turn");

        if (deSelect && currentSelection != null)
        {
            DeselectUnit(currentSelection);
        }


        if (currentPlayersTurn.index == 1)
        {
            currentPlayersTurn = allPlayers[1];
            UIMain.ChangeTeamText(currentPlayersTurn.index);
            if (localGame)
            {
                localPlayerTurn = allPlayers[1];
            }
        }
        else
        {
            currentPlayersTurn = allPlayers[0];
            UIMain.ChangeTeamText(currentPlayersTurn.index);
            if (localGame)
            {
                localPlayerTurn = allPlayers[0];
            }
        }
        
        
        UIMain.EnableEndTurnButton(false);
        descriptionBoxPopUp.CloseDescriptionPopUp();
        MoveAlongWaitTimers(allUnits);
        ResetAllEnergyBoostToFalse();

        await EndTurnAddEnergyToAll();

        AICall(true);
        canMove = true;
    }

    private void MoveAlongWaitTimers(List<Unit> theList)
    {
        foreach (Unit unit in theList)
        {
            unit.UpdateWaitTimer();
        }
    }
    


    //NODE SELECTION -------------------------------------------------------------------------------------------------------------------
    public async void NodeSelection(Node node, bool isLocalPlayerClicking = false)
    {
        if (node == null)
        {
            Debug.Log("Node is Null!");
        }

        Unit unitSelected = node.currentUnit;
        if (!canClickGameBoard)
            return;

        if (localPlayerTurn != currentPlayersTurn && currentPlayersTurn.playerControlled == PlayerControlled.PlayerControlled)
            return;


        if (gameState == GameState.NoSelect)
        {
            if (unitSelected == null)
            {
                return;
            }

            if (unitSelected != currentSelection)
            {
                unitSelected.SelectOutline(true);
                currentSelection = unitSelected;
                nodesInRange = new FindAllNodesInRange(node, unitSelected.GetSpeed(), unitSelected.canFly, unitSelected.isBenched);
                _ = unitSelected.energyDisc.ScaleUp(true);
                //Testing for Effects and wait
                if (!unitSelected.DoesUnitHaveThisStatus(Status.Frozen) && unitSelected.waitTimer == 0)
                {
                    ColorMultipleNodes(nodesInRange.GetClickableNodes(), true);
                } 
                //End test for Effects
                if (unitSelected.ownerPlayer == currentPlayersTurn)
                {
                    gameState = GameState.SelectUnitPlayerTurn;
                    //UIMain.TurnOnInfoButton(true); //Commented out for the test
                    infoCardTopSection.EnableInfoCard(unitSelected, true); //Test

                }
                else
                {
                    gameState = GameState.SelectUnitNotPlayerTurn;
                    infoCardTopSection.EnableInfoCard(unitSelected, true);
                }

            }
        }

        else if (gameState == GameState.SelectUnitNotPlayerTurn)
        {
            if (unitSelected != null)
            {
                DeselectUnit(currentSelection);
                infoCardTopSection.EnableInfoCard(null, false);

            }
        }


        else if (gameState == GameState.SelectUnitPlayerTurn)
        {

            //CLICKING ALREADY SELECTED NODE TO DESLECT
            if (unitSelected == currentSelection && unitSelected != null)
            {
                DeselectUnit(unitSelected);
            }
            //CLICKING NEIGHBORING ALLY TO REMOVE STATUS
            else if (currentSelection.ownerPlayer == node?.currentUnit?.ownerPlayer && node.currentUnit.DoesUnitHaveRemoveableStatus())
            {
                node.currentUnit.RemoveAllAbilitesWithAttribute();
                Debug.Log("Removing Stats by clicking on neighbor");
                EndTurn(true);
            }
            //CLICKING OPPONENT NODE TO ATTACK
            else if (currentSelection.ownerPlayer != node?.currentUnit?.ownerPlayer && node.IsUnitAdjacentToThisNode(currentSelection) && node.currentUnit != null) //TODO: ERROR HAS POPPED UP HERE BEFORE, I wonder if it was becasue I had red as an attack before being implimented
            {
                if (!currentSelection.DoesUnitHaveThisStatus(Status.Frozen))
                {
                    await unitSelected.JumpMoveWhenClicked();
                    AttackAction(currentSelection, unitSelected);
                }
            }
            //CLICKING A SPOT YOU CANT MOVE TO OR INTERACT WITH
            else if(!nodesInRange.IsNodeAMoveableSpot(node))
            {
                DeselectUnit(currentSelection);
            }
            //CLICKING EMPTY NODE TO  MOVE
            else if (canMove && currentSelection.ownerPlayer == currentPlayersTurn && node.currentUnit == null )
            {
                if (currentPlayersTurn == localPlayerTurn || !isLocalPlayerClicking) //currentPlayersTurn == localPlayerTurn OLD CODE HERE
                {
                    if (!currentSelection.DoesUnitHaveThisStatus(Status.Frozen) && currentSelection.waitTimer == 0) //Checking for status like frozen
                    {
                        canMove = false;
                        infoCardTopSection.EnableInfoCard(currentSelection, false);
                        _= currentSelection.energyDisc.ScaleUp(false);
                        await MoveUnitToNode(currentSelection, node, nodesInRange);
                        infoCardTopSection.EnableInfoCard(currentSelection, true);
                        CheckIfEnemiesAreWithinRangeAndIfNeighborAllyHasStatusToRemove();
                    }
                }
            }
            
            
        }
        else if(gameState == GameState.AbilityMove)
        {
            if (currentPlayersTurn == localPlayerTurn)
            {
                await MoveUnitToNode(currentSelection, node, nodesInRange);
                isTaskComplete = true;
            }
            else
            {
                Debug.Log("Not the correct players turn.");
            }
        }
    }

    public List<Node> NoDuplicateNodes(List<Node> theList)
    {
        List<Node> newList = new List<Node>();

        foreach (Node node in theList)
        {
            if (!newList.Contains(node))
            {
                newList.Add(node);
            }
        }
        return newList;
    }

    public void ColorMultipleNodes(List<Node> allNodes, bool isSelected)
    {
        Color assignColor = selectionNodeColors[1];
        if (currentSelection?.ownerIndex != currentPlayersTurn.index)
        {
            assignColor = selectionNodeColors[0];
        }

        if (!isSelected)
        {
            //assignColor = selectionNodeColors[0];
            map.selectionCircles.TurnOffCircles();

        }
        else
        {
            map.selectionCircles.TurnOnSelectedCircles(NoDuplicateNodes(allNodes), assignColor, map.isDuelMap);

        }
        //for (int i = 0; i < allNodes.Count; i++)
        //{
        //    allNodes[i].ColorNode(assignColor);
        //}
    }
    public void ColorNodesTemp(List<Node> allNodes, Color assignColor)
    {
        for (int i = 0; i < allNodes.Count; i++)
        {
            allNodes[i].ColorNode(assignColor);
        }
    }

    public void DeselectUnit(Unit unit)
    {
        unit.SelectOutline(false);
        _ = unit.energyDisc.ScaleUp(false);
        currentSelection = null;
        ColorMultipleNodes(nodesInRange.GetClickableNodes(), false);
        nodesInRange = null;
        gameState = GameState.NoSelect;
        UIMain.TurnOnInfoButton(false);
    }


    public bool IsNodeTheBench(Node node)
    {

        for (int i = 0; i < map.team1StartNodes.Count; i++)
        {
            if (node == map.team1StartNodes[i])
            {
                return true;
            }
        }
        for (int i = 0; i < map.team2StartNodes.Count; i++)
        {
            if (node == map.team2StartNodes[i])
            {
                return true;
            }
        }

        return false;
    }










    //UNIT MOVEMENT
    private async Task MoveUnitToNode(Unit unit, Node node, FindAllNodesInRange availableNodesToMoveTo)
    {
        if (!availableNodesToMoveTo.GetClickableNodes().Contains(node))
        {
            return;
        }

        actionLog.AddToKeyLog(ActionKey.Select, unit.currentNode.id);
        actionLog.AddToKeyLog(ActionKey.Move, node.id);


        canClickGameBoard = false;
        unit.currentNode.currentUnit = null;
        TurnOffInfoCircle(true);
        ColorMultipleNodes(nodesInRange.GetClickableNodes(), false);
        await unit.MoveUnitv2(availableNodesToMoveTo.GetPath(unit.currentNode, node));

        await AfterMove(unit);
    }

    public async Task AfterMove(Unit unit)
    {
        canClickGameBoard = true;


        //DeselectUnit(unit);

        await CheckForSurrounds(unit);


        map.IsPlayerInMiddle();
        DidSomeoneWin();

        actionLog.nextAction = true;
        TurnOffInfoCircle(false);
        map.SetWhichPlayerOwnsTheMiddle(map.playerThatOwnsTheMiddle);

        if (unit.isBenched)
        {
            unit.isBenched = false;
        }
    }


    private void CheckIfEnemiesAreWithinRangeAndIfNeighborAllyHasStatusToRemove()
    {

        if (currentSelection == null)
            EndTurn();

        FindEnemys findEnemys = new FindEnemys(currentSelection, currentSelection.GetAttackRange());

        int enemyCount = findEnemys.GetAllEnemiesCount();
        if (enemyCount > 0)
        {
            UIMain.EnableEndTurnButton(true);
            _ = currentSelection.energyDisc.ScaleUp(true);

            if (map.isDuelMap)
            {
                AICall(false);
            }
        }
        else if(DoesNeighborAllyHaveStatusToRemove(currentSelection))
        {
            UIMain.EnableEndTurnButton(true);
            _ = currentSelection.energyDisc.ScaleUp(true);

            if (map.isDuelMap)
            {
                AICall(false);
            }
        }
        else
        {
            EndTurn(true);
        }
    }

    private bool DoesNeighborAllyHaveStatusToRemove(Unit unit)
    {
        foreach (Node node in unit.currentNode.neighbors)
        {
            if (node.currentUnit != null)
            {
                if (node.currentUnit.ownerIndex == unit.ownerIndex)
                {
                    if (node.currentUnit.DoesUnitHaveRemoveableStatus())
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }


    public async Task CheckForSurrounds(Unit unitToCheck)
    {
        List<Unit> allUnitsWithSurrounds = new List<Unit>();

        if (IsSurroundedByEnemies(unitToCheck))
        {
            allUnitsWithSurrounds.Add(unitToCheck);
        }
        List<Node> neighbors = unitToCheck.currentNode.GetNeighbors();
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i].currentUnit != null)
            {
                if (IsSurroundedByEnemies(neighbors[i].currentUnit))
                {
                    allUnitsWithSurrounds.Add(neighbors[i].currentUnit);
                }
            }
        }
        for (int i = 0; i < allUnitsWithSurrounds.Count; i++)
        {
            Node newNode = allUnitsWithSurrounds[i].currentNode;
            await NodeSurroundSendToBench(newNode);
        }
        //Debug.Log("IN CHECK FOR SURROUNDS");
    }

    public bool IsSurroundedByEnemies(Unit unit)
    {
        FindEnemys findEnemys = new FindEnemys(unit, 1);
        if (findEnemys.GetAllEnemiesCount() == unit.currentNode.GetNeighbors().Count)
        {
            return true;
        }
        return false;
    }

    public List<Unit> GetAdjacentUnits(Unit unit)
    {
        FindEnemys findEnemys = new FindEnemys(unit, 1);

        return findEnemys.GetAdjacentUnits();
    }

    private async Task NodeSurroundSendToBench(Node nodeSurround)
    {
        TurnOffInfoCircle(true);
        await nodeSurround.currentUnit.SendToBench(map.GetTeamBenchNodes(nodeSurround.currentUnit.ownerPlayer));
        
    }

    public void TurnOffInfoCircle(bool turnOff)
    {
        if (turnOff)
        {
            uiCirclesCanvasGroup.DOFade(0, .1f);
        }
        else
        {
            uiCirclesCanvasGroup.DOFade(1, .1f);
        }
    }



    //ENERGY

    public async Task StartGameSetEveryoneToZeroEnergy()
    {
        for (int i = 0; i < allUnits.Count; i++)
        {
            allUnits[i].energyDisc.transform.DOScale(0,0);

            if (i == allUnits.Count -1)
            {
                await allUnits[i].UpdateEnergy(-allUnits[i].maxEnergy);
            }
            else
            {
            _ = allUnits[i].UpdateEnergy(-allUnits[i].maxEnergy);
            }

        }
    }

    public async Task EndTurnAddEnergyToAll()
    {
        await Task.Delay(100);
        for (int i = 0; i < allUnits.Count; i++)
        {

            if(!allUnits[i].isBenched)
            {
                _ = allUnits[i].UpdateEnergy(1);
            }

        }
        await Task.Delay(1000);
    }

    public void ResetAllEnergyBoostToFalse()
    {
        for (int i = 0; i < allUnits.Count; i++)
        {
            allUnits[i].activateEnergyBoost = false;
        }
    }





    //CHECK FOR END OF GAME
    private void DidSomeoneWin()
    {
        if (map.isDuelMap)
            return;

        int splitAuroNodesInHalf = (map.auraNodes.Count - 1) / 2; //To examine the other half of the aura nodes
        for (int i = 0; i < splitAuroNodesInHalf; i++)
        {
            if (map.auraNodes[i].currentUnit != null)
            {
                if (map.auraNodes[i].currentUnit.ownerPlayer.index == 2 && map.playerThatOwnsTheMiddle?.index == 2)
                {
                    EndOfGameUIAnimation(false, "Player 2 Winner");
                    Debug.Log("Player 2 Has Won!");

                    return;
                }
            }
            if (map.auraNodes[i + splitAuroNodesInHalf].currentUnit != null)
            {
                if (map.auraNodes[i + splitAuroNodesInHalf].currentUnit.ownerPlayer.index == 1 && map.playerThatOwnsTheMiddle?.index == 1)
                {
                    Debug.Log("Player 1 Has Won!");

                    EndOfGameUIAnimation(false, "Player 1 Winner");
                    return;
                }
            }
        }

        
    }
    public void EndOfGameUIAnimation(bool player1Won, string textOverride = "")
    {
        TurnOffInfoCircle(true);
        if (textOverride != "")
        {
            endGamePopUpManager.SetWinnerOrLoserScreenPopUp(true);
            endGamePopUpManager.ChangeText(textOverride);

        }
        else if (player1Won)
        {
            endGamePopUpManager.SetWinnerOrLoserScreenPopUp(true);
        }
        else
        {
            endGamePopUpManager.SetWinnerOrLoserScreenPopUp(false);
        }
    }





    //ATTACKING

    public void AttackAction(Unit attackUnit, Unit defendUnit)
    {

        int attackSpin = RandumNum(0, 95); //I believe 96 was giving me an error so I'm lowing it to 95, this makes sense since there is supposed to be 96 slices with 0 inclusive, 0-95 is 96 slices.
        int defendSpin = RandumNum(0, 95);

        AttackMenuLanch(attackUnit, defendUnit, attackSpin, defendSpin);
    }
    public void AttackMenuLanch(Unit attackUnit, Unit defendUnit, int spinNumAttacker, int spinNumDefender)
    {
        Unit closeUnit = attackUnit;
        Unit farUnit = defendUnit;
        bool isCloseUnitAttacking = true;
        int closeSpin = spinNumAttacker;
        int farSpin = spinNumDefender;

        if (attackUnit.ownerPlayer.index == 2)
        {
            closeUnit = defendUnit;
            farUnit = attackUnit;
            closeSpin = spinNumDefender;
            farSpin = spinNumAttacker;

            isCloseUnitAttacking = false;

        }

        battleScreen.LoadBattleScreen(closeUnit, farUnit, isCloseUnitAttacking, closeSpin, farSpin, null);
    }


    public async void AfterAttackAnimationMenuActions()
    {
        DeselectUnit(currentSelection);
        battleScreen.attackUnit.waitTimer = 2;

        if (battleScreen.unitThatWon != null)
        {
            
        }

        if (battleScreen.unitThatLost != null)
        {
            if (!map.isDuelMap || battleScreen.unitThatLost.ownerPlayer.playerControlled == PlayerControlled.PlayerControlled)
            {
                int waitTimer = unitTotalWait[battleScreen.unitThatLost.ownerPlayer.index - 1];
                UpdateWaitTimerPlayer(battleScreen.unitThatLost.ownerPlayer.index - 1);
                await battleScreen.unitThatLost.SendToBench(map.GetTeamBenchNodes(battleScreen.unitThatLost.ownerPlayer), waitTimer);

                if (!map.isDuelMap)
                    map.SetWhichPlayerOwnsTheMiddle(map.playerThatOwnsTheMiddle);
            }
            else //IS A DUEL && Raid Monster
            {
                await raidBoss.Damaged(-battleScreen.GetTotalDamageByUnit(battleScreen.unitThatWon.ownerIndex));

                if (raidBoss.health <= 0)
                {
                    Debug.Log("Raid Boss has fainted, you lose.");
                    return;
                }
            }
            
        }
        //TIE
        else if(battleScreen.unitThatWon == null && battleScreen.unitThatLost == null)
        {
            if (battleScreen.allUnits[0].DoesUnitHaveRemoveableStatus())
            {
                battleScreen.allUnits[0].RemoveAllAbilitesWithAttribute();
            }
            if (battleScreen.allUnits[1].DoesUnitHaveRemoveableStatus())
            {
                battleScreen.allUnits[1].RemoveAllAbilitesWithAttribute();
            }
        }

        await AfterBattleAction();
        Debug.Log("After Battle End Turn about to be called");
        EndTurn(true);
    }
    private async Task AfterBattleAction()
    {
        await Task.Delay(500);
        if (battleScreen.endBattleAbilities.Count > 0)
        {
            
            foreach (Ability ability in battleScreen.endBattleAbilities)
            {
                isTaskComplete = false;
                await ability.OnLeaveBattle();
            }
        }
        Debug.Log("After Attacking Abilities Complete");
    }

    private void UpdateWaitTimerPlayer(int index)
    {
        unitTotalWait[index] += 1;
        UIMain.UpdateWaitText();
    }


    //ABILITIES
    private void InstallAbilitiesOnUnitsOnLoadIn(List<Unit> teamUnits)
    {
        foreach (Unit unit in teamUnits)
        {
            foreach (Ability ability in unit.allAbilites)
            {
                ability.InstallOnAllUnitsInGame();
            }
        }
    }

    public List<Ability> AbilityCreate(List<Ability> theList)
    {
        List<Ability> newList = new List<Ability>();

        for (int i = 0; i < theList.Count; i++)
        {
            Ability newAbility = Instantiate(theList[i]);
            newList.Add(newAbility);
        }
        return newList;
    }

    //public void InstantiateNewAbilitiesInAttackMove(List<AttackMove> theList)
    //{
    //    for (int i = 0; i < theList.Count; i++)
    //    {
    //        if (theList[i].hasAbility)
    //        {
    //            Ability newAbility = Instantiate(theList[i].ability);
    //            theList[i].ability = newAbility;
    //        }
    //    }
    //}







    






    //OTHER
    private int RandumNum(int startNum, int endNum)
    {
        return UnityEngine.Random.Range(startNum, endNum + 1);
    }




}



[System.Serializable]
public enum GameState { NoSelect, SelectUnitPlayerTurn, SelectUnitNotPlayerTurn, AbilityMove }
public enum AttackDirection { Up, Down, Left, Right }
public enum AttackColor { None, White, Gold, Purple, Red, Blue, Green, Pink }
public enum Spirit { None, Normal, Dark, Fairy }
public enum PlayerControlled { PlayerControlled, AIControlled, RaidMonster }
public enum CreatureType { None, Poison }