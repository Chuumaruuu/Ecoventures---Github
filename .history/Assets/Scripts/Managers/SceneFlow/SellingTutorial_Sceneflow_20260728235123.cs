using UnityEngine;

public class SellingTutorial_Sceneflow : MonoBehaviour
{
    [SerializeField] private GameObject _sellingTutorialMenuUI;
    [SerializeField] private GameObject _mainSellingUI;
    [SerializeField] private Dialogue_Progression _dialogueProgress;

    private void Start()
    {
        if(!_dialogueProgress.STAGE1_INTRO)
        {
            _sellingTutorialMenuUI.SetActive(true);
            _mainSellingUI.SetActive(false);
        }
        else
        {
            _sellingTutorialMenuUI.SetActive(false);
            _mainSellingUI.SetActive(true);
        }
    }
}
