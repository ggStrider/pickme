using UnityEngine;

namespace Tasks
{
    [CreateAssetMenu(fileName = "New Task", menuName = "Thenexy/Tasks/New Task")]
    public class GameTaskInfo : ScriptableObject
    {
        public string taskObjective;
        public bool isCompleted;
        
        public TaskCondition taskCondition;
        public TaskCompleteAction taskCompleteAction;
    }
}