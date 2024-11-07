using UnityEngine;

using Data.Items;
using Data;

namespace Interact
{
    public class InteractAddItem : MonoBehaviour, IInteract
    {
        [SerializeField] private Item _itemToAdd;
        private GameSession _gameSession;

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
        }
        
        public void Interact()
        {
            _gameSession.AddItemToInventory(_itemToAdd);
            gameObject.SetActive(false);
        }
    }
}
