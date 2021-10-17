
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Gate))]
public class GateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Gate myScript = (Gate)target;
        if (GUILayout.Button("Set Gate UI"))
        {
            myScript.SetOperatorUI();
        }
    }
}
