using UnityEngine;

using Data;
using Data.Items;

namespace Tasks.Conditions
{
    [CreateAssetMenu(fileName = "New ItemCollectedCondition",
        menuName = "Thenexy/Tasks/Conditions/ItemCollectedCondition")]
    public class ItemCollectedCondition : TaskCondition
    {
        public GameItem requiredItem; 
            
        public override bool IsConditionMet()
        {
            return GameSession.Instance.IsInventoryHasItem(requiredItem);
        }
    }
}
