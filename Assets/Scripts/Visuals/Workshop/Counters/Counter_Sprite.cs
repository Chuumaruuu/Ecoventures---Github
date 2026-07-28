using UnityEngine;
using UnityEngine.UI;

public class Counter_Sprite : MonoBehaviour
{
    [SerializeField] private Counter_Base _baseCounter;
    [SerializeField] private GameObject _spritePanel;

    private void Start() 
    {
        Player_Base.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        _spritePanel.GetComponent<Image>().sprite = _baseCounter.GetSprite();
    }

    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player_Base.OnSelectedCounterChangedEventArgs e) 
    {
        if (e._selectedCounter == _baseCounter) 
        {
            if (!_baseCounter.HasItem())
            {
                Show();
            }
        } else {
            Hide();
        }
    }

    private void Show() 
    {
        _spritePanel.SetActive(true);
    }

    private void Hide() 
    {
        _spritePanel.SetActive(false);
    }
}
