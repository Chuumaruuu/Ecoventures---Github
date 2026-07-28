using UnityEngine;

public class Counter_Selected : MonoBehaviour
{
    [SerializeField] private Counter_Base _baseCounter;
    [SerializeField] private GameObject[] _visualGameObjects;



    private void Start() 
    {
        Player_Base.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player_Base.OnSelectedCounterChangedEventArgs e) 
    {
        if (e._selectedCounter == _baseCounter) 
        {
            Show();
        } else {
            Hide();
        }

    }

    private void Show() 
    {
        foreach(GameObject _visualGameObject in _visualGameObjects)
        {
            _visualGameObject.SetActive(true);
        }
    }

    private void Hide() 
    {
        foreach(GameObject _visualGameObject in _visualGameObjects)
        {
            _visualGameObject.SetActive(false);
        }
    }
}
