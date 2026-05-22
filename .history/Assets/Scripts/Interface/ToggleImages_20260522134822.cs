using UnityEngine;
using UnityEngine.UI;

public class ToggleImages : MonoBehaviour
{
    [SerializeField] private Toggle[] _toggles;

    private void Awake()
    {
        if (_toggles == null || _toggles.Length == 0)
        {
            _toggles = GetComponentsInChildren<Toggle>(true);
        }

        foreach (Toggle toggle in _toggles)
        {
            if (toggle == null)
            {
                continue;
            }

            toggle.onValueChanged.AddListener(_ => UpdateToggle(toggle));
            UpdateToggle(toggle);
        }
    }

    private void UpdateToggle(Toggle toggle)
    {
        if (toggle.transform.childCount < 2)
        {
            return;
        }

        GameObject toggleOn = toggle.transform.GetChild(0).gameObject;
        GameObject toggleOff = toggle.transform.GetChild(1).gameObject;

        toggleOn.SetActive(toggle.isOn);
        toggleOff.SetActive(!toggle.isOn);
        Debug.Log($"Toggle '{toggle.name}' is now {(toggle.isOn ? "ON" : "OFF")}");
    }
}
