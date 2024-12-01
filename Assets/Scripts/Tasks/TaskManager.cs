using UnityEngine;
using Data.Observers;

using System.Collections.Generic;

namespace Tasks
{
    public class TaskManager : MonoBehaviour, IAddedItem
    {
        [SerializeField] private List<GameTaskInfo> _tasks = new List<GameTaskInfo>();
        public static TaskManager Instance { get; private set; }

        private void Start()
        {
            if(Instance == null) Instance = this;
        }

        public void CheckCompleteCondition()
        {
            foreach (var task in _tasks)
            {
                if (task.taskCondition.IsConditionMet())
                {
                    task.taskCompleteAction.ExecuteAction();
                }
            }
        }

        public void OnItemAdded()
        {
            CheckCompleteCondition();
        }
    }
}