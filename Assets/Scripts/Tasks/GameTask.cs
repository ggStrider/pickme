using UnityEngine;

namespace Tasks
{
    [CreateAssetMenu(fileName = "New Task", menuName = "Thenexy/New Task")]
    public class GameTask : ScriptableObject
    {
        public string taskName;
    }
}