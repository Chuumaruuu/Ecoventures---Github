using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{
    [SerializeField] Player_Base _player;
    [SerializeField] private Image _dropButton;
    [SerializeField] private Image _pickupButton;

    void Start()
    {
        _dropButton.gameObject.SetActive(false);
        _player.OnPlayerGrabbedObject += OnPlayerInteract;
    }

    private void OnPlayerInteract(object sender, System.EventArgs e)
    {
        _dropButton.gameObject.SetActive(_player.HasItem());
        _pickupButton.gameObject.SetActive(!_player.HasItem());
    }
}
