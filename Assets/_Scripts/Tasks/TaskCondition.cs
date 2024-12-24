using UnityEngine;

namespace Tasks
{
    public abstract class TaskCondition : ScriptableObject
    {
        public abstract bool IsConditionMet();
    }
}