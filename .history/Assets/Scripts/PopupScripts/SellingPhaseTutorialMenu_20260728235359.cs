using UnityEngine;
using UnityEngine.UI;

public class SellingPhaseTutorialMenu : MonoBehaviour
{
    [SerializeField] private Image _tutorialImage;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Recipe_Data[] _tutorialSteps;
    [SerializeField] private Dialogue_Progress _dialogueProgress;
    [SerializeField] private GameObject _mainSellingUI;
    private int _currentTutorialIndex;

    private void Start()
    {
        if(_dialogueProgress.STAGE1_INTRO)
        {
            gameObject.SetActive(false);
            _mainSellingUI.SetActive(true);
        }
        else
        {
            ShowTutorial();
            _mainSellingUI.SetActive(false);
        }
        
        _nextButton.onClick.AddListener(NextTutorialStep);
        _previousButton.onClick.AddListener(PreviousTutorialStep);
        _closeButton.onClick.AddListener(CloseTutorial);
    }

    public void ShowTutorial()
    {
        if (_tutorialSteps == null || _tutorialSteps.Length == 0)
        {
            Debug.LogWarning("No tutorial steps assigned.");
            return;
        }

        _currentTutorialIndex = 0;
        UpdateTutorialUI();
        gameObject.SetActive(true);
    }

    private bool CanGoNext()
    {
        return _tutorialSteps != null && _currentTutorialIndex < _tutorialSteps.Length - 1;
    }

    private bool CanGoPrevious()
    {
        return _tutorialSteps != null && _currentTutorialIndex > 0;
    }

    public void NextTutorialStep()
    {
        if (CanGoNext())
        {
            _currentTutorialIndex++;
            UpdateTutorialUI();
        }
    }

    public void PreviousTutorialStep()
    {
        if (CanGoPrevious())
        {
            _currentTutorialIndex--;
            UpdateTutorialUI();
        }
    }

    private void UpdateTutorialUI()
    {
        if (_tutorialSteps == null || _tutorialSteps.Length == 0)
        {
            Debug.LogWarning("No tutorial steps assigned.");
            return;
        }

        Recipe_Data currentStep = _tutorialSteps[_currentTutorialIndex];
        if (currentStep != null && currentStep._recipeSprite != null && currentStep._recipeSprite.Length > 0)
        {
            _tutorialImage.sprite = currentStep._recipeSprite[0];
            _tutorialImage.gameObject.SetActive(true);
        }
        else
        {
            _tutorialImage.gameObject.SetActive(false);
        }

        _previousButton.interactable = CanGoPrevious();
        _nextButton.interactable = CanGoNext();
    }

    private void ShowCloseButton()
    {
        if(!CanGoNext())
        {
            _closeButton.gameObject.SetActive(true);
        }
        else
        {
            _closeButton.gameObject.SetActive(false);
        }
    }

    private void CloseTutorial()
    {
        gameObject.SetActive(false);
    }
}