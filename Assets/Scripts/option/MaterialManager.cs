using UnityEngine;
using UnityEngine.UI;

public class MaterialManager : MonoBehaviour
{
    private Material material;
    private string property;

    public void SetMaterial(Material mat)
    {
        material = mat;
    }

    public void SetProperty(string name)
    {
        property = name;
    }

    public void ChangedSlider(Slider slider)
    {
        slider.onValueChanged.AddListener(delegate { ChangedMaterial_ToFloat(slider, material, property); });
    }

    private void ChangedMaterial_ToFloat(Slider slider, Material material, string property)
    {
        material.SetFloat(property, slider.value);
    }
}
