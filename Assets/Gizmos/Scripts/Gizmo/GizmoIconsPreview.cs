using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoIconsPreview : MonoBehaviour
{

    public float cellRadius = 1;
    public Vector2 gridSize = Vector2.one; //regionSize
    public int rejectionSamples = 30;

    public GizmoIconSetting gizmoIconSetting;
    List<Vector2> points;

    public bool autoUpdate;

    List<Vector2> GetGeneratedPoints(float radius, Vector2 sampleRegionSize, int numSamplesBeforeRejection = 30)
    {
        return PoissonDiscSampling.GeneratePoints(radius, sampleRegionSize, numSamplesBeforeRejection);
    }

    public void GetPointsInEditor()
    {
        points = GetGeneratedPoints(cellRadius, gridSize, rejectionSamples);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(gridSize / 2, gridSize);
        if (points == null) return;

        foreach (GizmoIconSetting.GizmoIcon gizmoIcon in gizmoIconSetting.iconSamples)
        {
            if (gizmoIcon.icon != null)
            {
                points = GetGeneratedPoints(gizmoIcon.size, gridSize, gizmoIcon.samples);
                foreach (Vector2 point in points)
                {
                    Gizmos.DrawIcon(point, "Icons/" + gizmoIcon.icon.name);
                }
            }
        }
    }
}
