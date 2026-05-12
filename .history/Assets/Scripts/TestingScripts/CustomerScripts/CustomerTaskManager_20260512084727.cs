using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomerTaskManager : MonoBehaviour
{
    public static CustomerTaskManager Instance;

    [Header("Task Settings")]
    public int maximumCustomers = 10;
    public int maxServed = 5;

    private int spawnedCount = 0;
    private int servedCount = 0;
    private int activeCustomers = 0;
    private int nextSceneIndex = 1;

    [Header("UI")]
    public TextMeshProUGUI taskText;
    public GameObject taskProgressPanel;
    public GameObject taskCompletePanel;
    public TextMeshProUGUI resultText;

    [Header("Button")]
    [SerializeField] private TextMeshProUGUI btnText;

    [Header("Scene Management")]
    [SerializeField] private Scene_Manager _sceneManager;
    private bool taskEnded = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateTaskUI();

        if (taskProgressPanel != null)
            taskProgressPanel.SetActive(true);

        if (taskCompletePanel != null)
            taskCompletePanel.SetActive(false);
    }

    // CUSTOMER TRACKING
    public bool CanSpawn()
    {
        return spawnedCount < maximumCustomers && servedCount < maxServed;
    }

    public void RegisterSpawn()
    {
        spawnedCount++;
        activeCustomers++;
    }

    public void RegisterServed()
    {
        if (taskEnded) return;

        servedCount++;
        activeCustomers--;

        UpdateTaskUI();
    }

    public void RegisterCustomerExit()
    {
        if (taskEnded) return;

        activeCustomers--;
        CheckEndCondition();
    }

    // END CONDITION CHECK
    void CheckEndCondition()
    {
        if (taskEnded) return;

        // Task ends when either spawned count reaches maximumCustomers OR served count reaches maxServed
        if (spawnedCount >= maximumCustomers || servedCount >= maxServed)
        {
            Time.timeScale = 0f;
            ShowResult(true);   // SUCCESS
        }
    }

    void UpdateTaskUI()
    {
        if (taskText != null)
            taskText.text = "Served: " + servedCount + " / " + maxServed;
    }

    void ShowResult(bool isSuccess)
    {
        if (taskEnded) return;

        taskEnded = true;

        taskProgressPanel.SetActive(false);
        taskCompletePanel.SetActive(true);

        if (isSuccess)
        {
            resultText.text = "TASK COMPLETE!";
            btnText.text = "Continue";
        }
        else
        {
            resultText.text = "TRY AGAIN!";
            btnText.text = "Try Again";
        }
    }

    public void PressButton()
    {
        if (!taskEnded) return;

        if (btnText.text == "Continue")
        {
            Time.timeScale = 1f;
            _sceneManager.FadeToScene(nextSceneIndex);
        }
        else
        {
            Time.timeScale = 1f;
            _sceneManager.FadeToScene(nextSceneIndex);
        }
    }
}