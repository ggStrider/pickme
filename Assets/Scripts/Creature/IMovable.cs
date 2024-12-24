using System;
using UnityEngine;

namespace Creature
{
    public interface IMovable
    {
        public event Action<Vector2> OnIsMoving;
        
        public void SetDirection(Vector2 direction);
        public void Move();
    }
}