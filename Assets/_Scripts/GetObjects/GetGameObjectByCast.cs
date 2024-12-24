using UnityEngine;

namespace GetObjects
{
    public class GetGameObjectByCast
    {
        /// <summary>
        /// Takes game object using ray. No check by layer
        /// </summary>
        /// <param name="position">where ray starts casting</param>
        /// <param name="direction">direction where ray casts</param>
        /// <param name="distance">distance of ray</param>
        /// <param name="ignoreLayer">layer to not check by ray (skipping obj with this layer)</param>
        /// <returns>GameObject which was in ray or null</returns>
        public GameObject GetByRay(Vector3 position, Vector3 direction, float distance, LayerMask ignoreLayer)
        {
            return Physics.Raycast(position, 
                direction, out var hitInfo, distance, ~ignoreLayer) ? hitInfo.collider.gameObject : null;
        }
    }
}