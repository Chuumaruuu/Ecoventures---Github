using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;

    [Header("Time Settings")]
    [SerializeField, Range(0, 360)]
    private float TimeofDay = 80f; // Start at morning

    [Tooltip("Seconds for a full 360° cycle")]
    [SerializeField] private float CycleDuration = 600f; // Bigger = slower

    [Tooltip("Time speed multiplier")]
    [SerializeField] private float TimeSpeed = 1f;

    [Header("Debug")]
    [SerializeField] private float CurrentHour;

    private void Start()
    {
        TimeofDay = 90f; // force start value
        UpdateLighting(TimeofDay / 360f);
    }
    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            TimeofDay +=
                (360f / CycleDuration) *
                Time.deltaTime *
                TimeSpeed;

            TimeofDay %= 360f;
        }

        float timePercent = TimeofDay / 360f;

        // Debug 24-hour clock
        CurrentHour = timePercent * 24f;

        UpdateLighting(timePercent);
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight =
            Preset.AmbientColor.Evaluate(timePercent);

        RenderSettings.fogColor =
            Preset.fogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color =
                Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.rotation =
                Quaternion.Euler(
                    (timePercent * 360f) - 90f,
                    -170f,
                    0f
                );
        }
    }

    
    // AUTO FIND DIRECTIONAL LIGHT
    private void OnValidate()
    {
        if (DirectionalLight == null)
        {
            if (RenderSettings.sun != null)
            {
                DirectionalLight = RenderSettings.sun;
            }
            else
            {
                Light[] lights =
                    Object.FindObjectsByType<Light>(
                        FindObjectsSortMode.None);

                foreach (Light light in lights)
                {
                    if (light.type == LightType.Directional)
                    {
                        DirectionalLight = light;
                        break;
                    }
                }
            }
        }

        if (Preset != null)
            UpdateLighting(TimeofDay / 360f);
    }
}