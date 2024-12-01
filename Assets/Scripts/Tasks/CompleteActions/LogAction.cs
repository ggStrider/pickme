using UnityEngine;

namespace Tasks.CompleteActions
{
    [CreateAssetMenu(fileName = "New Log Action", menuName = "Thenexy/Tasks/CompleteAction/Log on Complete")]
    public class LogAction : TaskCompleteAction
    {
        public string message;
        
        public override void ExecuteAction()
        {
            Debug.Log(message);
        }
    }
}