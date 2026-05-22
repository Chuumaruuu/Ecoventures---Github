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
        else
        {
            _toggleOn.SetActive(false);
            _toggleOff.SetActive(true);
        }
    }
}
