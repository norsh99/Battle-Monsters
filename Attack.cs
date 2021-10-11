//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Attack
//{

//    public int attackLocation;
//    public Unit attackUnit { get; }
//    public Unit defendUnit { get; }

//    public Unit winnerUnit;
//    public Unit loserUnit;
//    private LoserStatus loserStatus;

//    //BONUS DAMAGE
//    public int attackerMultiplyBonusDamage = 0;
//    public int defenderMultiplyBonusDamage = 0;
//    public int attackerBonusDamage = 0;
//    public int defenderBonusDamage = 0;


//    public bool isAttackDone;


//    //AFTER BATTLE ABILITY EXECUTE
//    public Ability afterBattleAbilityToExecute;


//    //For initializing an attack
//    public Attack(Unit attackUnit, Unit defendUnit)
//    {
//        isAttackDone = false;

//        this.attackUnit = attackUnit;
//        this.defendUnit = defendUnit;
//        attackLocation = DetermineAttackLocation(attackUnit, defendUnit);
//        ActivateBeforeBattleAbilities(attackUnit, true);
//        ActivateBeforeBattleAbilities(defendUnit, false);

//        DetermineWhoWon(attackUnit.attackCluster.attackNodes[attackLocation-1], defendUnit.attackCluster.attackNodes[attackLocation-1]);
//    }

//    //To continue an attack for multiplayer or playback system when attackLocation is already determined
//    public Attack(int attackLocation, Unit attackUnit, Unit defendUnit)
//    {
//        isAttackDone = false;

//        this.attackUnit = attackUnit;
//        this.defendUnit = defendUnit;
//        this.attackLocation = attackLocation;
//        ActivateBeforeBattleAbilities(attackUnit, true);
//        ActivateBeforeBattleAbilities(defendUnit, false);
//        DetermineWhoWon(attackUnit.attackCluster.attackNodes[attackLocation-1], defendUnit.attackCluster.attackNodes[attackLocation-1]);
//    }

//    //DETERMINE LOCATION
//    private int DetermineAttackLocation(Unit attackUnit, Unit defendUnit)
//    {
//        List<Unit> unitList = new List<Unit>() { attackUnit, defendUnit };
//        int leftAxisRandNum = RandumNum(1, 10);
//        int upAxisRandNum = RandumNum(1, 10);

//        int leftAxis = 0; //1,2,3
//        int upAxis = 0;  //1,2,3


//        foreach (Unit unit in unitList)
//        {
//            switch (unit.attackDirection)
//            {
//                case AttackDirection.Up:
//                    upAxisRandNum -= unit.attackDirectionPower;
//                    break;
//                case AttackDirection.Down:
//                    upAxisRandNum += unit.attackDirectionPower;
//                    break;
//                case AttackDirection.Left:
//                    leftAxisRandNum -= unit.attackDirectionPower;
//                    break;
//                case AttackDirection.Right:
//                    leftAxisRandNum += unit.attackDirectionPower;
//                    break;
//                default:
//                    break;
//            }
//        }
//        //Left to Right
//        if (leftAxisRandNum < 5)
//        {
//            leftAxis = 1;
//        }
//        else if(leftAxisRandNum > 6)
//        {
//            leftAxis = 3;
//        }
//        else
//        {
//            leftAxis = 2;
//        }

//        //Up to Down
//        if (upAxisRandNum < 5)
//        {
//            upAxis = 1;
//        }
//        else if (upAxisRandNum > 6)
//        {
//            upAxis = 3;
//        }
//        else
//        {
//            upAxis = 2;
//        }

//        return GetAttackGridLocation(leftAxis, upAxis);
//    }


//    private int GetAttackGridLocation(int leftNum, int upNum)
//    {
//        if (leftNum == 1 && upNum == 1)
//        {
//            return 1;
//        }
//        else if(leftNum == 2 && upNum == 1)
//        {
//            return 2;
//        }
//        else if (leftNum == 3 && upNum == 1)
//        {
//            return 3;
//        }
//        else if (leftNum == 1 && upNum == 2)
//        {
//            return 4;
//        }
//        else if (leftNum == 2 && upNum == 2)
//        {
//            return 5;
//        }
//        else if (leftNum == 3 && upNum == 2)
//        {
//            return 6;
//        }
//        else if (leftNum == 1 && upNum == 3)
//        {
//            return 7;
//        }
//        else if (leftNum == 2 && upNum == 3)
//        {
//            return 8;
//        }
//        else
//        {
//            return 9;
//        }
//    }



//    //DETERMINE WINNER
//    private void DetermineWhoWon(AttackNode attackerNode, AttackNode defenderNode)
//    {

//        int attackColorResult = DidAttackerWinColor(attackerNode, defenderNode);
//        int damageResult = 0; //Default is tie


//        //TIE = 0
//        if (attackColorResult == 0)
//        {
//            //WHITE vs WHITE
//            if (attackerNode.attackColor == AttackColor.White && defenderNode.attackColor == AttackColor.White)
//            {
//                damageResult = DidAttackerWinDamageBattle(
//                    attackUnit.GetAttackDamage(attackerNode) + attackerBonusDamage,
//                    defendUnit.GetAttackDamage(defenderNode) + defenderBonusDamage);
//            }
//            //GOLD vs GOLD
//            else if (attackerNode.attackColor == AttackColor.Gold && defenderNode.attackColor == AttackColor.Gold)
//            {
//                damageResult = DidAttackerWinDamageBattle(
//                    attackUnit.GetAttackDamage(attackerNode) + attackerBonusDamage,
//                    defendUnit.GetAttackDamage(defenderNode) + defenderBonusDamage);
//            }
//            //GOLD vs WHITE
//            else if (attackerNode.attackColor == AttackColor.Gold && defenderNode.attackColor == AttackColor.White)
//            {
//                defenderMultiplyBonusDamage = 2;
//                damageResult = DidAttackerWinDamageBattle(
//                    attackUnit.GetAttackDamage(attackerNode) + attackerBonusDamage,
//                    (defendUnit.GetAttackDamage(defenderNode) * 2) + defenderBonusDamage);
//            }
//            //WHITE vs GOLD
//            else if (attackerNode.attackColor == AttackColor.White && defenderNode.attackColor == AttackColor.Gold)
//            {
//                attackerMultiplyBonusDamage = 2;
//                damageResult = DidAttackerWinDamageBattle(
//                    (attackUnit.GetAttackDamage(attackerNode) * 2) + attackerBonusDamage,
//                    defendUnit.GetAttackDamage(defenderNode) + defenderBonusDamage);
//            }
//            //PURPLE vs PURPLE
//            else if (attackerNode.attackColor == AttackColor.Purple)
//            {
//                damageResult = DidAttackerWinDamageBattle(
//                    attackUnit.GetAttackDamage(attackerNode) + attackerBonusDamage,
//                    defendUnit.GetAttackDamage(defenderNode) + defenderBonusDamage);

//                if (damageResult == 1)
//                {
//                    //EnableAbility(attackUnit, defendUnit);
//                    EnablePurpleAbility(attackUnit);
//                    winnerUnit = attackUnit;
//                    loserUnit = defendUnit;
//                }
//                else if (damageResult == 2)
//                {
//                    //EnableAbility(defendUnit, attackUnit);
//                    EnablePurpleAbility(defendUnit);

//                    winnerUnit = defendUnit;
//                    loserUnit = attackUnit;
//                }
//                else
//                {
//                    winnerUnit = null;
//                    loserUnit = null;
//                }
//                return;
                
//            }
//            //Nothing found
//            else 
//            {
//                Debug.Log("No attack matching found when there is a tie.");
//            }


//            //DETERMINE FINAL WINNER NON COLOR ---------------------------------
//            if (damageResult == 1) //ATTACKER WON
//            {
//                Debug.Log("Attacker, " + attackUnit.ownerPlayer.name + " has won.");

//                winnerUnit = attackUnit;
//                loserUnit = defendUnit;
//                loserStatus = LoserStatus.SendToBench;

//            }
//            else if(damageResult == 2) //ATTACKER LOST
//            {
//                Debug.Log("Defender, " + defendUnit.ownerPlayer.name + " has won.");

//                winnerUnit = defendUnit;
//                loserUnit = attackUnit;
//                loserStatus = LoserStatus.SendToBench;

//            }
//            else //TIE
//            {
//                Debug.Log("It was a tie.");

//                winnerUnit = null;
//                loserUnit = null;
//                loserStatus = LoserStatus.None;

//            }


//        }
//        //DETERMINE COLOR WINNER
//        //ATTACKER WON = 1
//        else if(attackColorResult == 1)
//        {
//            Debug.Log("Attacker, " + attackUnit.ownerPlayer.name + " has won.");
//            winnerUnit = attackUnit;
//            loserUnit = defendUnit;
//            if (attackerNode.attackColor == AttackColor.Gold)
//            {
//                loserStatus = LoserStatus.SendToBench;
//            }
//        }
//        //ATTACKER LOST = 2
//        else
//        {
//            Debug.Log("Defender, " + defendUnit.ownerPlayer.name + " has won.");
//            winnerUnit = defendUnit;
//            loserUnit = attackUnit;
//            if (defenderNode.attackColor == AttackColor.Gold)
//            {
//                loserStatus = LoserStatus.SendToBench;
//            }
//        }
//        AttackingIsComplete();
//    }

//    private int DidAttackerWinColor(AttackNode attackerNode, AttackNode defenderNode)
//    {
//        //Return 0 if tie, 1 if attacker won, and 2 if attacker lost
//        if (attackerNode.attackColor == AttackColor.White)
//        {
//            if (defenderNode.attackColor == AttackColor.White)
//            {
//                return 0;
//            }
//            else if (defenderNode.attackColor == AttackColor.Purple)
//            {
//                //EnableAbility(defendUnit, attackUnit);
//                EnablePurpleAbility(defendUnit);

//                return 2;
//            }
//            else if (defenderNode.attackColor == AttackColor.Gold)
//            {
//                return 0;
//            }
//        }
//        else if (attackerNode.attackColor == AttackColor.Purple)
//        {
//            if (defenderNode.attackColor == AttackColor.White)
//            {
//                //EnableAbility(attackUnit, defendUnit);
//                EnablePurpleAbility(attackUnit);
//                return 1;
//            }
//            else if (defenderNode.attackColor == AttackColor.Purple)
//            {
//                return 0;
//            }
//            else if (defenderNode.attackColor == AttackColor.Gold)
//            {
//                return 2;
//            }
//        }
//        else if (attackerNode.attackColor == AttackColor.Gold)
//        {
//            if (defenderNode.attackColor == AttackColor.White)
//            {
//                return 0;
//            }
//            else if (defenderNode.attackColor == AttackColor.Purple)
//            {
//                return 1;
//            }
//            else if (defenderNode.attackColor == AttackColor.Gold)
//            {
//                return 0;
//            }
//        }
//        return 0;
//    }


//    private int DidAttackerWinDamageBattle(int attackerDamage, int defenderDamage)
//    {
//        //ATTACK WON
//        if (attackerDamage > defenderDamage)
//        {
//            return 1;
//        }
//        //ATTACKER LOST
//        else if(attackerDamage < defenderDamage)
//        {
//            return 2;
//        }
//        //TIE
//        else
//        {
//            return 0;
//        }
//    }





//    //LAUNCH ABILITIES

//    private void EnablePurpleAbility(Unit unit)
//    {
//        //afterBattleAbilityToExecute = attackNode.triggerAbility; OLD
//        foreach (AttackMove attackMove in unit.attackMoveList)
//        {
//            if (attackMove.attackColor == AttackColor.Purple)
//            {
//                if (attackMove.ability != null)
//                {
//                    afterBattleAbilityToExecute = attackMove.ability;
//                }
//            }
//        }
        
//    }

//    private void ActivateBeforeBattleAbilities(Unit unit, bool isAttacking)
//    {
//        AttackColor attackColor = unit.attackCluster.GetAttackNode(attackLocation).attackColor;
//        foreach (AttackMove attackMove in unit.attackMoveList)
//        {
//            if (attackMove.attackColor == attackColor)
//            {
//                if (attackMove.ability != null)
//                {
//                    attackMove.ability.OnEnterBattle(this, unit, isAttacking);
//                }
//            }
//        }

//        foreach (Ability ability in unit.allAbilites)
//        {
//            ability.OnEnterBattle(this, unit, isAttacking);
//        }
//        foreach (Ability ability in unit.tempAbilites)
//        {
//            ability.OnEnterBattle(this, unit, isAttacking);
//        }

//        if (isAttacking)
//        {
//            if (attackerBonusDamage == -1)
//            {
//                attackerBonusDamage = 0;
//            }
//        }
//        else
//        {
//            if (defenderBonusDamage == -1)
//            {
//                defenderBonusDamage = 0;
//            }
//        }
//    }





//    //OTHER
//    private int RandumNum(int startNum, int endNum)
//    {
//        return UnityEngine.Random.Range(startNum, endNum + 1);
//    }

//    private void AttackingIsComplete()
//    {
//        isAttackDone = true;
//    }

//    //SET
//    public void SetLoserStatus(LoserStatus loserStatus) { this.loserStatus = loserStatus; }

//    //GETS
//    public Unit GetWinner()
//    {
//        return winnerUnit;
//    }

//    public Unit GetLoser()
//    {
//        return loserUnit;
//    }
//    public int GetAttackLocation()
//    {
//        return attackLocation;
//    }

//    public LoserStatus GetLoserStatus() { return loserStatus; }
//}


//public enum LoserStatus { None, SendToBench, StayInPlace }
