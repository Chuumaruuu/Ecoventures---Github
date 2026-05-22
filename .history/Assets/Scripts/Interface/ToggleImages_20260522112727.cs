using UnityEngine;

public class ToggleImages : MonoBehaviour
{
    [SerializeField] private GameObject _toggleOn;
    [SerializeField] private GameObject _toggleOff;

    public void Toggle(bool _isOn)
    {
        if (_isOn)
        {
            _toggleOn.SetActive(true);
            _toggleOff.SetActive(false);
        }
    }

    public void ToggleOff(bool _isOff)
    {
        if (_isOff)
        {
            _toggleOn.SetActive(false);
            _toggleOff.SetActive(true);
        }
    }
}
