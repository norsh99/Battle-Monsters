using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using System.Threading.Tasks;

public class Unit : MonoBehaviour
{
    public GameMaster gm;

    [HideInInspector] public Statue statue;
    [SerializeField] private Outlinable outline;

    [ReadOnly] public Node currentNode;
    [SerializeField] private GameObject defaultGO;
    public EnergyDisc energyDisc;


    //BASE MONSTER
    [SerializeField] private Monster monster;
    public UI_InfoNode infoNode;

    //UNIT STATS
    public Player ownerPlayer;
    public int ownerIndex;

    [SerializeField] private int speed;
    public bool canFly;
    public CreatureType creatureType;
    public float maxEnergy;
    public float currentEnergy;
    public bool activateEnergyBoost;



    //ATTACK STATS
    [SerializeField] private int attackRange;
    [ReadOnly] public List<AttackMove> attackMoveList;

    //ABILITIES
    public List<Ability> allAbilites;
    public List<Ability> tempAbilites;







    //IN GAME VARS
    public bool isBenched;
    public int waitTimer;
    public int currentBattleIndex;


    public void LoadIn(Monster newMonster, Player ownerPlayer, Material teamColor)
    {
        isBenched = true;
        gameObject.SetActive(true);
        infoNode.gameObject.SetActive(true);
        if (newMonster != null)
        { 
            defaultGO.SetActive(false);
            this.ownerPlayer = ownerPlayer;
            ownerIndex = ownerPlayer.index;
            InstallMonster(newMonster, teamColor);
        }
        else
        {
            gameObject.SetActive(false); //If no 'newMonster' then the gameobject should be off
            infoNode.gameObject.SetActive(false);
        }
    }

    private void InstallMonster(Monster newMonster, Material baseColor)
    {
        monster = newMonster;
        GameObject go = Instantiate(newMonster.monsterModel, this.transform);
        statue = go.GetComponentInChildren<Statue>();
        outline = statue.GetOutline();
        statue.AssignPlayerColor(baseColor);
        infoNode.UpdateSpeedColor(baseColor.color);
        

        if (ownerPlayer.index == 1)
        {
            statue.RotateModel(false);
        }

        //Traits and info
        SetSpeed(newMonster.speed);
        waitTimer = newMonster.startWaitTimer;
        canFly = newMonster.canFly;
        attackRange = newMonster.attackRange;
        creatureType = newMonster.type;
        maxEnergy = newMonster.maxEnergy;
        currentEnergy = maxEnergy;

        //Attack Moves
        attackMoveList = CreateNewAttackMoveList(newMonster.attackMoves);

        //Abilites
        allAbilites = gm.AbilityCreate(newMonster.abilities);
        tempAbilites = new List<Ability>();


        UpdateWaitTimer(false);
        InnitializeAbilites();
    }



    public void SelectOutline(bool isSelected)
    {
        outline.OutlineParameters.Enabled = isSelected;
    }




    //WAIT TIMER
    public void UpdateWaitTimer(bool progressTimer = true)
    {
        if (waitTimer > 0 && progressTimer)
        {
            waitTimer -= 1;
        }

        infoNode?.UpdateWaitText(waitTimer);
    }





    //ABILITES
    private void InnitializeAbilites()
    {
        for (int i = 0; i < allAbilites.Count; i++)
        {
            allAbilites[i].Innitialize(this, gm);
        }
    }


    private List<AttackMove> CreateNewAttackMoveList(List<AttackMove> theList)
    {
        List<AttackMove> newList = new List<AttackMove>();
        for (int i = 0; i < theList.Count; i++)
        {
            AttackMove newMove = new AttackMove(theList[i]);

            if (newMove.hasAbility)
            {
                Ability newAbility = Instantiate(newMove.ability);
                newMove.ability = newAbility;
                newMove.ability?.Innitialize(this, gm);
            }

            //theList[i].ability?.Innitialize(this, gm);

            newList.Add(newMove);
        }
        return newList;
    }

    public void RemoveAllAbilitesWithAttribute()
    {
        List<Ability> removeList = new List<Ability>();
        foreach (Ability ability in allAbilites)
        {
            if (ability.RemoveAbility())
            {
                removeList.Add(ability);
            }
        }

        for (int i = 0; i < removeList.Count; i++)
        {
            for (int m = 0; m < allAbilites.Count; m++)
            {
                if (removeList[i] == allAbilites[m])
                {
                    allAbilites.RemoveAt(m);
                }
            }
        }
    }

    public bool DoesUnitHaveThisStatus(Status status)
    {
        bool doesHaveStatus = false;

        foreach (Ability ability in allAbilites)
        {
            if (ability.status == status)
            {
                doesHaveStatus = true;
            }
        }

        return doesHaveStatus;
    }

    public bool DoesUnitHaveRemoveableStatus()
    {
        bool doesHaveStatus = false;

        foreach (Ability ability in allAbilites)
        {
            if (ability.status == Status.Frozen || ability.status == Status.NegativeMP1)
            {
                doesHaveStatus = true;
            }
        }

        return doesHaveStatus;
    }



    //GETS and SETS
    public int GetSpeed() { return speed - 1; } //Look into why it needs to be minus 1
    public int GetAttackRange() { return attackRange; }
    public void SetSpeed(int speedNum) { speed = speedNum; infoNode.UpdateSpeedText(speed.ToString()); }

    public Monster GetMonster() { return monster; }


    public async Task MoveUnitv2(List<Node> pathNodes, float defaultJumpHeight = .5f, float movementTime = .35f)
    {
        List<Vector3> newPath = new List<Vector3>();
        float jumpHeight = defaultJumpHeight;
        for (int i = 0; i < pathNodes.Count; i++)
        {
            newPath.Add(pathNodes[i].transform.position);
        }

        for (int i = 1; i < newPath.Count; i++)
        {
            jumpHeight = defaultJumpHeight;
            if (canFly)
            {
                if (pathNodes[i].currentUnit != null)
                {
                    jumpHeight = .8f;
                    i += 1;
                }
            }


            transform.DOMoveX(newPath[i].x, movementTime).SetEase(Ease.InOutSine);
            transform.DOMoveZ(newPath[i].z, movementTime).SetEase(Ease.InOutSine);
            await transform.DOMoveY(jumpHeight, movementTime / 2f).SetEase(Ease.InOutSine).AsyncWaitForCompletion();
            await transform.DOMoveY(0f, movementTime / 2f).SetEase(Ease.InOutSine).AsyncWaitForCompletion();
        }
        pathNodes[pathNodes.Count-1].AssignUnit(this);
        Debug.Log("Move Complete");

        if(ownerPlayer.playerControlled == PlayerControlled.RaidMonster)
        {
            await gm.raidBoss.HasMonsterHasFoundExit();
        }

    }

    public async Task SkyUnitMove()
    {
        await transform.DOMoveY(30f, .8f).AsyncWaitForCompletion();
    }

    public async Task JumpMoveWhenClicked()
    {
        await transform.DOMoveY(.4f, .1f).AsyncWaitForCompletion();
        await transform.DOMoveY(0f, .1f).AsyncWaitForCompletion();
    }


    //ATTACK DAMAGE
    public int GetAttackDamage(int nodeNum)
    {

        //for (int i = 0; i < attackMoveList.Count; i++)
        //{
        //    if (attackColor == attackMoveList[i].attackColor)
        //    {
        //        return totalDamage + attackMoveList[i].damage;
        //    }
        //}

        //return totalDamage;
        return 0;
    }
    public int GetAttackDamage(AttackNode attackNodenode)
    {
        int totalDamage = attackNodenode.damage;
        AttackColor attackColor = attackNodenode.attackColor;

        for (int i = 0; i < attackMoveList.Count; i++)
        {
            if (attackColor == attackMoveList[i].attackColor)
            {
                return totalDamage + attackMoveList[i].damage;
            }
        }

        return totalDamage;
    }

    //SEND TO BENCH
    public async Task SendToBench(List<Node> benchNodes, int waitTimerOverride = 5)
    {
        RemoveAllAbilitesWithAttribute();
        List<Node> newList = new List<Node>();
        newList.Add(currentNode);

        for (int i = 0; i < benchNodes.Count; i++)
        {
            if (benchNodes[i].currentUnit == null)
            {
                currentNode.currentUnit = null;
                
                
                newList.Add(benchNodes[i]);
                await MoveUnitv2(newList, .8f);
                //moveUnit.MoveOneSpace(benchNodes[i], action);
                benchNodes[i].AssignUnit(this);
                break;
            }
        }
        waitTimer = waitTimerOverride;

        isBenched = true;
    }








    //ENERGY
    public async Task UpdateEnergy(float newAmount)
    {
        currentEnergy = currentEnergy + newAmount;

        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }

        await energyDisc.LoadIn(currentEnergy, maxEnergy);
        _ = energyDisc.ScaleUp(false);
    }
}
