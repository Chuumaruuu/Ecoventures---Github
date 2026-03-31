using UnityEngine.SceneManagement;
using UnityEngine;

public class Map_Manager : MonoBehaviour
{
    [SerializeField] private string _stageName;

    public virtual void Interact(Map_Player _player)
    {
        SceneManager.LoadScene(_stageName);
    }

    public virtual void InteractAlternate(Map_Player _player)
    {
        // nothing happens
    }
}
