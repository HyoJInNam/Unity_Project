using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TerrainData : UpdatableData
{
    public float uniformScale = 2.5f;

    public bool useFlatShading;
    public bool useFalloff;

    public float meshHeightMultiplier = 40.0f;
    public AnimationCurve meshHeightCurve = AnimationCurve.EaseInOut(0,0,1,1);

    public float minHeight {
        get {
            return uniformScale * meshHeightMultiplier * meshHeightCurve.Evaluate(0);
        }
    }

    public float maxHeight {
        get {
            return uniformScale * meshHeightMultiplier * meshHeightCurve.Evaluate(1);
        }
    }

}
