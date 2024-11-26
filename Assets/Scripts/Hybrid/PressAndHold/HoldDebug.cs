using UnityEngine;

namespace Hybrid.PressAndHold
{
    [RequireComponent(typeof(PressAndHoldManager))]
    public class HoldDebug : MonoBehaviour, IPressAndHold
    {
        private void Start()
        {
            GetComponent<PressAndHoldManager>().SubscribeObserver(this);
        }
        
        public void Completed()
        {
            Debug.Log("Completed");
        }
    }
}