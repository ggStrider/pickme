using UnityEngine;

namespace _Configs.Player.Movement
{
    [CreateAssetMenu(fileName = "New PlayerConfig", menuName = "Thenexy/Configs/Player/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField] public float MaxSpeed { get; private set; }
        [field: SerializeField] public float SpeedAcceleration { get; private set; }
    }
}