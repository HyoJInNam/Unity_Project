using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class NoiseData : UpdatableData
{
    public Noise.NormalizeMode normalizeMode;

    public float noiseScale = 40;

    public int octaves = 4;
    [Range(0, 1)]
    public float persistance = 0.5f;
    public float lacunarity = 2.0f;

    public int seed;
    public Vector2 offset;
    
    protected override void OnValidate()
    {
        if (lacunarity < 1) { lacunarity = 1; }
        if (octaves < 0) { octaves = 0; }
        base.OnValidate();
    }
}
