using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Player")]
public class Player : ScriptableObject
{

    public string name;
    public List<Monster> monsterDeck;

    [EnumToggleButtons]
    public PlayerControlled playerControlled;


    //IN GAME VARS
    [HideInInspector]
    public int index;
    public Player(string name, int index, PlayerControlled playerControlled)
    {
        this.name = name;
        this.index = index;
        this.playerControlled = playerControlled;
    }





}

