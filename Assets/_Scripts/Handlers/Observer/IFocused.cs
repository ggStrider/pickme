using UnityEngine;

namespace Handlers.Observer
{
    public interface IFocused
    {
        public void OnCameraFocused(Vector3 newRotation);
    }
}