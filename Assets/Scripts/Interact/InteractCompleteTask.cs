using UnityEngine;

using Tasks;

namespace Interact
{
    public abstract class InteractCompleteTask : MonoBehaviour, IInteract
    {
        [SerializeField] protected bool CompleteTaskOnInteracted;
        [SerializeField] private CompleteTask _task;
        
        public virtual void Interact(bool isPressing)
        {
            if (CompleteTaskOnInteracted)
            {
                CompleteTaskOnInteracted = false;
                (_task ?? GetComponent<CompleteTask>()).Complete();
            }
        }
        
        #if UNITY_EDITOR

        private void OnValidate()
        {
            if(Application.isPlaying) return;
            if (CompleteTaskOnInteracted)
            {
                if (_task == null)
                {
                    UnityEditor.EditorApplication.delayCall += () =>
                    {
                        if (_task == null)
                        {
                            _task = gameObject.AddComponent<CompleteTask>();
                        }
                    };
                }
            }
            else
            {
                if (_task != null)
                {
                    UnityEditor.EditorApplication.delayCall+=()=>
                    {
                        DestroyImmediate(_task);
                    };
                }
            }
        }
        
        #endif
    }
}