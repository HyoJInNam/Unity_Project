using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EyeBlink : MonoBehaviour
{
    RectTransform upperBox;
    RectTransform lowerBox;

    [Range(0, 5)]
    public float blinkPower = 1;
    [Range(0, 1)]
    public float smoothness = 1;
    [Range(0, 1)]
    public float curvature = 1;
    [Range(0, 1)]
    public float time = 0.50f;

    [Header("Fade")]
    [Range(0, 1)]
    public float fadeOutDelay = 0;
    public AnimationCurve fadeInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve fadeOutCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    public Image upper;
    public Image lower;

    public void BlinkInEditor()
    {
    }

    public void UpdateMaterialInEditor()
    {
        upper = transform.Find("upper").GetComponent<Image>();
        lower = transform.Find("lower").GetComponent<Image>();

        ApplyToMaterial(upper);
        ApplyToMaterial(lower);
    }

    public void ApplyToMaterial(Image image)
    {
        image.material.SetColor("color", image.color);
        image.material.SetTexture("maintex", image.mainTexture);
        image.material.SetFloat("blinkPower", blinkPower);
        image.material.SetFloat("smoothness", smoothness);
        image.material.SetFloat("curvature", curvature);
    }
}

