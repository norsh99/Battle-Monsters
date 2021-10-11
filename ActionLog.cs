using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class ActionLog 
{
    GameMaster gm;
    public bool nextAction;
    private string key;
    private int index;
    public bool playbackModeOn;
    private List<string> keyLog;


    public ActionLog(GameMaster gm, bool playbackModeOn, string key)
    {
        this.gm = gm;
        this.key = key;
        this.playbackModeOn = playbackModeOn;
        nextAction = false;
        index = 0;
        if (playbackModeOn)
        {
            keyLog = ParseStringKey(key);
        }
        else
        {
            keyLog = new List<string>();
        }
    }

    public void PrintResults()
    {
        for (int i = 0; i < keyLog.Count; i++)
        {
            Debug.Log(keyLog[i]);
        }
    }
    public void AddToKeyLog(ActionKey keyCode, int num)
    {
        if (!playbackModeOn)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(KeyTranslateToString(keyCode));
            sb.Append(num);
            keyLog.Add(sb.ToString());
            //MULTIPLAYER: This could make it really easy to add multiplayer, just send this code over the internet and it will play out for the other player.
        }
    }

    private int ParseIntFromKey(string key)
    {
        StringBuilder sb = new StringBuilder();

        foreach (char c in key)
        {
            if (char.IsNumber(c))
            {
                sb.Append(c);
                continue;
            }
        }
        return Int32.Parse(sb.ToString());
    }
    private string ParseStringFromKey(string key)
    {
        StringBuilder sb = new StringBuilder();

        foreach (char c in key)
        {
            if (char.IsLetter(c))
            {
                sb.Append(c);
                break;
            }
        }
        return sb.ToString();
    }


    private List<string> ParseStringKey(string key)
    {
        List<string> output = new List<string>();

        StringBuilder sb = new StringBuilder();

        foreach (char c in key)
        {
            if (char.IsNumber(c))
            {
                sb.Append(c);
                continue;
            }

            if (char.IsLetter(c) && sb.Length == 0)
            {
                sb.Append(c);
                continue;
            }

            if (char.IsLetter(c) && sb.Length > 0)
            {
                output.Add(sb.ToString());
                sb.Clear();
                sb.Append(c);
                continue;
            }
        }

        // Grab the last one that wasn't captured in the foreach loop:
        output.Add(sb.ToString());
        return output;
    }




    public void PlayNextAction()
    {
        if (playbackModeOn && nextAction && index < keyLog.Count)
        {
            nextAction = false;
            FindKey(keyLog[index]);
            index += 1;
        }
    }








    private void FindKey(string tryKey)
    {
        string key = ParseStringFromKey(tryKey);
        int num = ParseIntFromKey(tryKey);

        switch (key)
        {
            case "S":
                //Selecting a Node
                Debug.Log("Node : " + num +" selected.");
                gm.NodeSelection(gm.map.GetNodeFromIndex(num));
                nextAction = true;
                break;

            case "T":
                //Setting the Team at the start
                Debug.Log("Team : " + num + " selected.");
                gm.OverrideSetTeamAtStart(num);
                nextAction = true;
                break;
            case "M":
                //Move from selected Node to destination Node
                Debug.Log("Move to Node : " + num);
                gm.NodeSelection(gm.map.GetNodeFromIndex(num));
                break;

            default:
                break;
        }
    }

    private string KeyTranslateToString(ActionKey key)
    {
        switch (key)
        {
            case ActionKey.None:
                Debug.Log("None Selected");
                break;
            case ActionKey.Select:
                return "S";
            case ActionKey.Attack:
                return "A";
            case ActionKey.Move:
                return "M";
            case ActionKey.TeamSelectStartGame:
                return "T";
            case ActionKey.EndTurn:
                return "E";
            default:
                break;
        }
        return "";
    }

}

public enum ActionKey { None, Select, Attack, Move, TeamSelectStartGame, EndTurn }