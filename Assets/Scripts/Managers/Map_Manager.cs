using UnityEngine.SceneManagement;
using UnityEngine;

public class Map_Manager : MonoBehaviour
{
    [SerializeField] private string _stageName;

    public void Start()
    {
        // trigger MapIntro dialogue only when this is the first time the player enters the map scene
    }
    public virtual void Interact(Map_Player _player)
    {
        SceneManager.LoadScene(_stageName);
    }

    public virtual void InteractAlternate(Map_Player _player)
    {
        // nothing happens
    }
    
}
