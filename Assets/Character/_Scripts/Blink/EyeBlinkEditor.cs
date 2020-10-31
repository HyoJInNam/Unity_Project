using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EyeBlink))]
public class EyeBlinkEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EyeBlink eye = (EyeBlink)target;

        if(DrawDefaultInspector())
        {
            eye.UpdateMaterialInEditor();
        }
        if (GUILayout.Button("Test Animation"))
        {
            eye.BlinkInEditor();
        }
    }
}
