using UnityEngine;

using Data.Items;
using Data;

namespace Interact
{
    public class InteractAddItem : MonoBehaviour, IInteract
    {
        [SerializeField] private GameItem _gameItemToAdd;
        private GameSession _gameSession;

        private void Start()
        {
            _gameSession = GameSession.Instance;
        }

        public void Interact(bool isPressing)
        {
            if (!isPressing) return;
            
            _gameSession.AddItemToInventory(_gameItemToAdd);
            gameObject.SetActive(false);
        }
    }
}
