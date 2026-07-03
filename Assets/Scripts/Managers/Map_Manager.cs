using UnityEngine;

public class Map_Manager : MonoBehaviour
{
    [SerializeField] private int _stageNum;
    [SerializeField] private Scene_Manager _sceneManager;
    private PlayerLocation _playerLocation;

    public virtual void Interact(Map_Player _player)
    {
        _sceneManager.FadeToScene(_stageNum);
        GetComponent<AllowedProducts_Manager>().SetItems();
    }

    public virtual void InteractAlternate(Map_Player _player)
    {
        // nothing happens
    }

    public enum PlayerLocation
    {
        Workshop, Stage1, Stage2, Stage3
    }

    public void ChangeSpawnPoint()
    {
        
    }
}
