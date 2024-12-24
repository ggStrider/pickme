using UnityEngine;

namespace Tasks
{
    public abstract class TaskCompleteAction : ScriptableObject
    {
        public abstract void ExecuteAction();
    }
}