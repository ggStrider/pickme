using UnityEngine;

using System.Linq;
using System.Collections.Generic;
using Data;

namespace Hover
{
    public class HoveredCheckRay : MonoBehaviour
    {
        private Transform _outPoint;
        private float _distance;

        private RaycastHit _hitInfo;

        private HashSet<IHovered> _hoveredObjects = new HashSet<IHovered>();

        public void SetSettings(Transform outPoint, float distance)
        {
            _outPoint = outPoint;
            _distance = distance;
        }
        
        private void Update()
        {
            if (Physics.Raycast(_outPoint.position, _outPoint.forward, out _hitInfo, _distance, ~DefaultData.TriggerLayer))
            {
                Debug.Log(_hitInfo.collider.gameObject.name);
                if (_hitInfo.collider.TryGetComponent(out IHovered hovered))
                {
                    var newHovered = _hitInfo.collider?.GetComponents<IHovered>().ToHashSet();
                    if (!_hoveredObjects.SetEquals(newHovered))
                    {
                        ClearHovered(ref _hoveredObjects);
                        
                        _hoveredObjects = newHovered;
                        InvokeAllHovered(_hoveredObjects);
                    }
                }
                else
                {
                    ClearHovered(ref _hoveredObjects);
                }
            }
            // Raycast == null
            else
            {
                ClearHovered(ref _hoveredObjects);
            }
        }

        private static void ClearHovered(ref HashSet<IHovered> hoveredObjects)
        {
            if(hoveredObjects.Count == 0) return;
            foreach (var hovered in hoveredObjects)
            {
                hovered.UnHovered();
            }
            hoveredObjects.Clear();
        }

        private void InvokeAllHovered(HashSet<IHovered> hoveredObjects)
        {
            foreach (var hovered in hoveredObjects)
            {
                hovered.Hovered();
            }
        }
    }
}