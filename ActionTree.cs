using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTree
{
    private List<Action> actionTree;

    public ActionTree()
    {
        actionTree = new List<Action>();
    }

    public Action RequestNextTask()
    {
        //Request Task
        if (actionTree.Count > 0)
        {
            //Give first Task
            Action task = actionTree[0];
            actionTree.RemoveAt(0);
            return task;
        }
        else
        {
            //No Task
            return null;
        }
    }

    public void PlayNextTask()
    {
        Action nextTask = RequestNextTask();
        if (nextTask != null)
        {
            nextTask();
        }
    }


    public void AddTask(Action task)
    {
        actionTree.Add(task);
    }



}
