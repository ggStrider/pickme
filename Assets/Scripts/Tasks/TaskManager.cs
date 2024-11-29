using UnityEngine;
using TMPro;

using System;
using System.Collections.Generic;

namespace Tasks
{
    public class TaskManager : MonoBehaviour
    {
        [SerializeField] private List<GameTaskComponents> _tasksData;
        public static TaskManager Instance { get; private set; }

        private void Start()
        {
            if(Instance == null) Instance = this;
        }

        private void InitializeAllTasks()
        {
            foreach (var taskData in _tasksData)
            {
                taskData.TextComponent.text = taskData.Task.taskName;
            }
        }

        public void CompleteGameTask(GameTask gameTaskToComplete)
        {
            if(IsTaskExist(gameTaskToComplete, out var taskRef)) 
            {
                if(!taskRef.Completed)
                {
                    taskRef.Completed = true;
                }
            }
            else
            {
                Debug.Log("<color=red>No task!!!</color>");
            }
        }

        private bool IsTaskExist(GameTask taskToCheck, out GameTaskComponents taskDataComponent) 
        {
            foreach (var data in _tasksData)
            {
                if(data.Task == taskToCheck) 
                {
                    taskDataComponent = data;
                    return true;
                }
            }

            taskDataComponent = null;
            return false;
        }

        [Serializable]
        public class GameTaskComponents
        {
            [field: SerializeField] public GameTask Task { get; private set; }
            [field: SerializeField] public TextMeshProUGUI TextComponent { get; private set; }
            [field: SerializeField] public bool Completed;
        }
    }
}