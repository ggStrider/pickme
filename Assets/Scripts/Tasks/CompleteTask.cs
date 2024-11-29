using UnityEngine;

namespace Tasks
{
    public class CompleteTask : MonoBehaviour
    {
        [SerializeField] private GameTask _gameTaskToComplete;

        public void Complete()
        {
            TaskManager.Instance.CompleteGameTask(_gameTaskToComplete);
        }
    }
}