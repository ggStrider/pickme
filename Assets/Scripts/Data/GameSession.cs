using Data.Items;
using UnityEngine;

namespace Data
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;

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
    }
}