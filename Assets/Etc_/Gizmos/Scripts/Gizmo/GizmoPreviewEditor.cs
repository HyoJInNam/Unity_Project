using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GizmoIconsPreview))]
public class GizmoPreviewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GizmoIconsPreview poissonDiscGen = (GizmoIconsPreview)target;

        if (DrawDefaultInspector())
        {
            if (poissonDiscGen.autoUpdate)
            {
                poissonDiscGen.GetPointsInEditor();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            poissonDiscGen.GetPointsInEditor();
        }
    }
}
