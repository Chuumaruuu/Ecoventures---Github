using UnityEngine;
using UnityEngine.UI;

public class SellingPhaseTutorialMenu : MonoBehaviour
{
    [SerializeField] private Image _tutorialImage;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Recipe_Data _tutorialSteps;
    [SerializeField] private Dialogue_Progress _dialogueProgress;
    [SerializeField] private GameObject _mainSellingUI;
    private int _currentTutorialIndex;

    private void Start()
    {
        if (_tutorialSteps == null || _tutorialSteps._recipeSprite == null || _tutorialSteps._recipeSprite.Length == 0)
        {
            Debug.LogWarning("No tutorial steps assigned.");
            return;
        }

        if(_dialogueProgress.STAGE1_INTRO)
        {
            gameObject.SetActive(false);
            _mainSellingUI.SetActive(true);
            return;
        }
        else
        {
            _mainSellingUI.SetActive(false);
            gameObject.SetActive(true);
            ShowTutorial();
        }

        _nextButton.onClick.AddListener(NextTutorialStep);
        _previousButton.onClick.AddListener(PreviousTutorialStep);
        _closeButton.onClick.AddListener(CloseTutorial);
    }

    public void ShowTutorial()
    {
        if (_tutorialSteps == null || _tutorialSteps._recipeSprite == null || _tutorialSteps._recipeSprite.Length == 0)
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
        return _tutorialSteps != null && _currentTutorialIndex < _tutorialSteps._recipeSprite.Length - 1;
    }

    private bool CanGoPrevious()
    {
        return _tutorialSteps != null && _currentTutorialIndex > 0;
    }

    public void NextTutorialStep()
    {
        Debug.Log("Next button clicked. Current index: " + _currentTutorialIndex);
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
        Sprite[] tutorialSprites = _tutorialSteps != null ? _tutorialSteps._recipeSprite : null;

        if (tutorialSprites == null || tutorialSprites.Length == 0)
        {
            Debug.LogWarning("No tutorial sprites available.");
            return;
        }

        if (_tutorialImage != null)
        {
            _tutorialImage.sprite = tutorialSprites[_currentTutorialIndex];
        }

        SetButtonActive(_previousButton, CanGoPrevious());
        SetButtonActive(_nextButton, CanGoNext());
        SetButtonActive(_closeButton, !CanGoNext());
    }

    private void CloseTutorial()
    {
        gameObject.SetActive(false);
        _mainSellingUI.SetActive(true);
        _dialogueProgress.STAGE1_INTRO = true;
    }

    private void SetButtonActive(Button button, bool isActive)
    {
        if (button != null)
        {
            button.gameObject.SetActive(isActive);
        }
    }
}