using UnityEngine;
using Hybrid.PressAndHold;

namespace Hybrid
{
    [RequireComponent(typeof(PressAndHoldEvent))]
    public class HoldDebug : MonoBehaviour, IPressAndHold
    {
        private void Start()
        {
            GetComponent<PressAndHoldEvent>().SubscribeObserver(this);
        }
        
        public void Completed()
        {
            Debug.Log("Completed");
        }
    }
}