
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Horde))]
public class PlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Horde myScript = (Horde)target;
        if (GUILayout.Button("Set Member Position"))
        {
            myScript.SetMembersHordePosition();
        }
    }
}
