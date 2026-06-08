using UnityEngine.SceneManagement;
using UnityEngine;

public class Map_Manager : MonoBehaviour
{
    [SerializeField] private int _stageNum;
    [SerializeField] private Scene_Manager _sceneManager;

    public virtual void Interact(Map_Player _player)
    {
        // SceneManager.LoadScene(_stageName);
        _sceneManager.FadeToScene(_stageNum);
        GetComponent<AllowedProducts_Manager>().SetItems();
    }

    public virtual void InteractAlternate(Map_Player _player)
    {
        // nothing happens
    }
}
