using UnityEngine;

namespace Tasks
{
    public static class CheckTaskHelper
    {
        public static void HaveToCompleteTask(MonoBehaviour monoBehaviour, ref bool completeTask)
        {
            monoBehaviour?.GetComponent<CompleteTask>()?.Complete();
            completeTask = false;
        }
    }
}