using UnityEngine.SceneManagement;
using UnityEngine;

public class Map_Manager : MonoBehaviour
{
    [SerializeField] private string _stageName;
    [SerializeField] private Dialogue_Progress _dialogueProgress;
    [SerializeField] private Dialogue_Data[] _mapDialogues;

    public void Start()
    {
        // trigger MapIntro dialogue only when this is the first time the player enters the map scene
        if (SceneManager.GetActiveScene().name == "MapScene")
        {
            WorkshopDialogue_Manager.Instance.Level1Intro();
        }
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
