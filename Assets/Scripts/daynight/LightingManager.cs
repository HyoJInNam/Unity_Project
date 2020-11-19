using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    public Material skybox_day;
    public Material skybox_night;

    [SerializeField]
    private Light DirectionalLight;
    [SerializeField]
    private LightingPreset Preset;

    public float GameTime = 60f;
    [SerializeField, Range(0, 60f)]
    private float TimeOfDay;

    public Vector2 dayTime;

    [Header("Day Text")]
    public GameObject text;
    public Text num;
    private int day;
    public int startTimeOfDay = 20;
    private int deltaTimeOfDay = 3;

    private void Update()
    {
        if (Preset == null) return;

        if (Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= GameTime;
        }

        UpdateLighting(TimeOfDay / GameTime);
        UpdateSkyBox(TimeOfDay / GameTime);

        UpdateDayText((int)TimeOfDay);
    }

    public GameObject end;
    public Text t;
    private void UpdateDayText(int TimeOfDay)
    {
        if ((int)TimeOfDay > 50)
        {
            end.SetActive(true);
            t.gameObject.SetActive(true);
            t.text = "GameOver";
        }

        if (!text.activeSelf && startTimeOfDay == TimeOfDay)
        {
            //text.SetActive(true);
            //day += 1;
            //num.text = day.ToString() + "Day";
            num.text = "TIME: " + (GameTime - (int)TimeOfDay).ToString();
        }
        else if (startTimeOfDay + deltaTimeOfDay == TimeOfDay)
        {
            //text.SetActive(false);
        }
    }

    private void UpdateSkyBox(float timePercent)
    {
        if (timePercent > dayTime.x / GameTime && timePercent < dayTime.y / TimeOfDay) {
            RenderSettings.skybox = skybox_day;
        } else { RenderSettings.skybox = skybox_night; }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null) {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }
    
    private void OnValidate()
    {
        if (DirectionalLight != null) return;
        
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}