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

        private List<IAddedItem> _addedItemObservers = new List<IAddedItem>();

        public static GameSession Instance { get; private set; }
        
        private void Awake()
        {
            if(Instance == null) Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public bool IsInventoryHasItem(GameItem gameItem)
        {
            return _data.Items.Contains(gameItem);
        }

        public void AddItemToInventory(GameItem gameItem)
        {
            _data.Items.Add(gameItem);
            
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