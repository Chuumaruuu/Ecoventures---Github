using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance { get; private set; }

    [Header("Exploration")]
    [SerializeField] private GameObject exploreCamera;
    [SerializeField] private GameObject exploreUI;

    [Header("Selling")]
    [SerializeField] private GameObject sellingCamera;
    [SerializeField] private GameObject sellingUI;

    [Header("Popup Panels")]
    [SerializeField] private GameObject hintPanels;
    [SerializeField] private GameObject boothPanels;
    [SerializeField] private GameObject rightAnswerUI;
    [SerializeField] private GameObject wrongAnswerUI;
    [SerializeField] private GameObject continuePanel;
    [SerializeField] private GameObject continueButton;

    private Image _activeCorrectAnswerImage;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void EnterExploreMode()
    {
        Time.timeScale = 1f;
        SetActiveSafe(exploreCamera, true);
        SetActiveSafe(exploreUI, true);
        SetActiveSafe(continueButton, true);
        SetActiveSafe(sellingCamera, false);
        SetActiveSafe(sellingUI, false);
        SetAllHintPanels(false);
        SetAllBoothPanels(false);
        SetActiveSafe(rightAnswerUI, false);
        SetActiveSafe(wrongAnswerUI, false);
        SetActiveSafe(continuePanel, false);
        HideAndClearCorrectAnswerImage();
    }

    public void RegisterCorrectAnswerImage(Image image)
    {
        _activeCorrectAnswerImage = image;
    }

    private void HideAndClearCorrectAnswerImage()
    {
        if (_activeCorrectAnswerImage != null)
        {
            _activeCorrectAnswerImage.gameObject.SetActive(false);
            _activeCorrectAnswerImage = null;
        }
    }

    public void EnterSellingMode()
    {
        Time.timeScale = 1f;
        SetActiveSafe(exploreCamera, false);
        SetActiveSafe(exploreUI, false);
        SetActiveSafe(sellingCamera, true);
        SetActiveSafe(sellingUI, true);
        SetAllHintPanels(false);
        SetAllBoothPanels(false);
        SetActiveSafe(rightAnswerUI, false);
        SetActiveSafe(wrongAnswerUI, false);
        SetActiveSafe(continuePanel, false);
    }

    public void OpenHint(GameObject hintPanel)
    {
        Time.timeScale = 0f;
        SetActiveSafe(exploreUI, false);
        SetActiveSafe(sellingUI, false);
        SetAllHintPanels(false);
        SetAllBoothPanels(false);
        SetActiveSafe(rightAnswerUI, false);
        SetActiveSafe(wrongAnswerUI, false);
        SetActiveSafe(continuePanel, false);
        SetActiveSafe(continueButton, false);
        SetActiveSafe(hintPanel, true);
    }

    public void CloseHint()
    {
        EnterExploreMode();
    }

    public void OpenBooth(GameObject boothPanel)
    {
        Time.timeScale = 0f;
        SetActiveSafe(exploreUI, false);
        SetAllBoothPanels(false);
        SetActiveSafe(boothPanel, true);
        SetActiveSafe(rightAnswerUI, false);
        SetActiveSafe(wrongAnswerUI, false);
        SetActiveSafe(continuePanel, false);
        SetActiveSafe(continueButton, false);
    }

    public void CloseBooth()
    {
        EnterExploreMode();
    }

    public void ShowBoothResult(bool isCorrect)
    {
        Time.timeScale = 0f;
        SetActiveSafe(rightAnswerUI, isCorrect);
        SetActiveSafe(wrongAnswerUI, !isCorrect);
    }

    public void ShowContinuePanel()
    {
        Time.timeScale = 0f;
        SetActiveSafe(continuePanel, true);
    }

    public void HideContinuePanel()
    {
        Time.timeScale = 1f;
        SetActiveSafe(continueButton, true);
        SetActiveSafe(continuePanel, false);
    }

    private void SetAllHintPanels(bool isActive)
    {
        SetActiveSafe(hintPanels, isActive);
    }

    private void SetAllBoothPanels(bool isActive)
    {
        SetActiveSafe(boothPanels, isActive);
    }

    private static void SetActiveSafe(GameObject target, bool isActive)
    {
        if (target != null)
        {
            target.SetActive(isActive);
        }
    }

    private static void SetActiveSafe(GameObject[] targets, bool isActive)
    {
        if (targets == null)
        {
            return;
        }

        foreach (GameObject target in targets)
        {
            SetActiveSafe(target, isActive);
        }
    }
}
