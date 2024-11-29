using Hybrid;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(DoorController))]
    public class DoorButtons : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.Space(10);
            
            var door = (DoorController)target;
            if (GUILayout.Button("Set close angle"))
            {
                door._SetClosePosition();
            }
            
            if (GUILayout.Button("Set open angle"))
            {
                door._SetOpenPosition();
            }
            
            GUILayout.Space(15);
            if (GUILayout.Button("Set pivot"))
            {
                door._SetPivot();
            }
        }
    }
}
