using System.Collections;
using UnityEngine;

[CreateAssetMenu()]
public class GizmoIconSetting : UpdatableData
{
    public GizmoIcon[] iconSamples;

    [System.Serializable]
    public class GizmoIcon{
        public Texture icon;
        public float size;
        public int samples;
    }
}
