using System.Collections.Generic;
using Data.Items;
using Data.Observers;
using UnityEngine;

namespace Data
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;

        private List<IAddedItem> _addedItemObservers;

        public static GameSession Instance { get; private set; }
        
        private void Awake()
        {
            if(Instance == null) Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public bool IsInventoryHasItem(Item item)
        {
            return _data.Items.Contains(item);
        }

        public void AddItemToInventory(Item item)
        {
            _data.Items.Add(item);
            
            NotifyItemAdded();
        }

        #region Observer stuff

        public void SubscribeItemAdded(IAddedItem observer)
        {
            if(_addedItemObservers.Contains(observer)) return;
            _addedItemObservers.Add(observer);
        }

        private void NotifyItemAdded()
        {
            if(_addedItemObservers.Count == 0) return;
            
            foreach (var observer in _addedItemObservers)
            {
                observer.OnItemAdded();
            }
        }

        #endregion
    }
}