using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CM_TaskSystem
{
    public class CM_TaskSystem
    {

        public class Task
        {

        }


        private List<Task> taskList;
        public CM_TaskSystem()
        {
            taskList = new List<Task>();
        }

        public Task RequestNextTask()
        {
            //Request Task
            if (taskList.Count < 0)
            {
                //Give first Task
                Task task = taskList[0];
                taskList.RemoveAt(0);
                return task;
            }
            else
            {
                //No Task
                return null;
            }
        }



        public void AddTask(Task task)
        {
            taskList.Add(task);
        }
    }









}

