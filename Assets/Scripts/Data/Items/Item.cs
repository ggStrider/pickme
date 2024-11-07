using System;
using UnityEngine;

namespace Data.Items
{
    [Serializable, CreateAssetMenu(fileName = "New Item", menuName = "Thenexy/Item")]
    public class Item : ScriptableObject
    {
        public string ItemName;
    }
}