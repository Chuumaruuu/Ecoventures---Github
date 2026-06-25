using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoothQuizPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Image itemSpriteImage;
    [SerializeField] private Button[] answerButtons = new Button[4];
    [SerializeField] private TextMeshProUGUI[] answerLabels = new TextMeshProUGUI[4];
    [SerializeField] private Image NPCSpriteImage;

    private readonly List<AnswerOption> answerOptions = new List<AnswerOption>(4);
    private Questions_Data currentQuestionData;
    private Item_Data currentUnlockableItem;
    private BoothInteractable currentOwner;

    private bool listenersBound;

    private void Awake()
    {
        BindButtonListeners();
    }

    private void OnDisable()
    {
        LockAnswerButtons();
    }

    public void Setup(Questions_Data questionData, Item_Data unlockableItemData, BoothInteractable owner)
    {
        currentQuestionData = questionData;
        currentUnlockableItem = unlockableItemData;
        currentOwner = owner;

        if (questionText != null)
        {
            questionText.text = currentQuestionData != null ? currentQuestionData._questionBody : string.Empty;
        }

        if (itemSpriteImage != null)
        {
            itemSpriteImage.sprite = currentUnlockableItem != null ? currentUnlockableItem._itemSprite : null;
            itemSpriteImage.enabled = itemSpriteImage.sprite != null;
        }

        BuildAnswerOptions();
        RefreshAnswerButtons();
        SetNPCSprite();
    }

    public void LockAnswerButtons()
    {
        if (answerButtons == null)
        {
            return;
        }

        foreach (Button button in answerButtons)
        {
            if (button != null)
            {
                button.interactable = false;
            }
        }
    }

    private void BindButtonListeners()
    {
        if (listenersBound || answerButtons == null)
        {
            return;
        }

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int capturedIndex = i;
            Button button = answerButtons[i];
            if (button == null)
            {
                continue;
            }

            button.onClick.AddListener(() => HandleAnswerSelected(capturedIndex));
        }

        listenersBound = true;
    }

    private void BuildAnswerOptions()
    {
        answerOptions.Clear();

        if (currentQuestionData == null)
        {
            return;
        }

        answerOptions.Add(new AnswerOption(currentQuestionData._rightAnswer, true));
        answerOptions.Add(new AnswerOption(currentQuestionData._wrongAnswer1, false));
        answerOptions.Add(new AnswerOption(currentQuestionData._wrongAnswer2, false));
        answerOptions.Add(new AnswerOption(currentQuestionData._wrongAnswer3, false));

        for (int i = answerOptions.Count - 1; i > 0; i--)
        {
            int swapIndex = Random.Range(0, i + 1);
            AnswerOption tempOption = answerOptions[i];
            answerOptions[i] = answerOptions[swapIndex];
            answerOptions[swapIndex] = tempOption;
        }
    }

    private void RefreshAnswerButtons()
    {
        if (answerButtons == null)
        {
            return;
        }

        for (int i = 0; i < answerButtons.Length; i++)
        {
            bool hasOption = i < answerOptions.Count;
            Button button = answerButtons[i];
            if (button == null)
            {
                continue;
            }

            button.interactable = hasOption;

            if (hasOption && i < answerLabels.Length && answerLabels[i] != null)
            {
                answerLabels[i].text = answerOptions[i].text;
            }
        }
    }

    private void SetNPCSprite()
    {
        if (NPCSpriteImage == null || currentQuestionData == null)
        {
            return;
        }

        NPCSpriteImage.sprite = currentQuestionData._NPCSprite;
        NPCSpriteImage.enabled = NPCSpriteImage.sprite != null;
    }

    private void HandleAnswerSelected(int index)
    {
        if (index < 0 || index >= answerOptions.Count)
        {
            return;
        }

        AnswerOption selectedOption = answerOptions[index];

        if (selectedOption.isCorrect)
        {
            currentOwner?.RightAnswer();
        }
        else
        {
            currentOwner?.WrongAnswer();
        }
    }

    private readonly struct AnswerOption
    {
        public readonly string text;
        public readonly bool isCorrect;

        public AnswerOption(string text, bool isCorrect)
        {
            this.text = text;
            this.isCorrect = isCorrect;
        }
    }
}