using UnityEngine;

using System.Linq;
using System.Collections.Generic;

namespace Hover
{
    public class HoveredCheckRay : MonoBehaviour
    {
        private Transform _outPoint;
        private float _distance;

        private RaycastHit _hitInfo;

        private List<IHovered> _hoveredObjects;

        public void SetSettings(Transform outPoint, float distance)
        {
            _outPoint = outPoint;
            _distance = distance;
        }
        
        private void Update()
        {
            if (_hoveredObjects is not null)
            {
                Debug.Log("sigma");

                foreach (var hovered in _hoveredObjects)
                {
                    hovered.UnHovered();
                }
                _hoveredObjects = null;
            }

            if (Physics.Raycast(_outPoint.position, _outPoint.forward, out _hitInfo, _distance))
            {
                _hoveredObjects = _hitInfo.collider?.GetComponents<IHovered>().ToList();

                if (_hoveredObjects == null) return;

                foreach (var hovered in _hoveredObjects)
                {
                    Debug.Log("sssssigma");
                    hovered.Hovered();
                }
            }
        }
    }
}