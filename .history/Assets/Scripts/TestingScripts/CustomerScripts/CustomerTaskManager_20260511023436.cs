using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CustomerTaskManager : MonoBehaviour
{
    public static CustomerTaskManager Instance;

    [Header("Task Settings")]
    public int totalCustomers = 20;

    private int spawnedCount = 0;
    private int servedCount = 0;
    private int activeCustomers = 0;
    private int nextSceneIndex = 2;

    [Header("UI")]
    public TextMeshProUGUI taskText;
    public GameObject taskProgressPanel;
    public GameObject taskCompletePanel;
    public TextMeshProUGUI resultText;

    [Header("Button Text")]
    [SerializeField] private TextMeshProUGUI btnText;
    // public GameObject continueButton;
    // public GameObject tryAgainButton;

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
        return spawnedCount < totalCustomers;
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

        // Wait until ALL customers have spawned AND shop is empty
        if (spawnedCount >= totalCustomers && activeCustomers <= 0)
        {
            Time.timeScale = 0f;

            if (servedCount >= totalCustomers)
                ShowResult(true);   // SUCCESS
            else
                ShowResult(false);  // FAIL
        }
    }

    void UpdateTaskUI()
    {
        if (taskText != null)
            taskText.text = "Serve Customers: " + servedCount + " / " + totalCustomers;
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
            // continueButton.SetActive(true);
            // tryAgainButton.SetActive(false);
        }
        else
        {
            resultText.text = "TRY AGAIN!";
            btnText.text = "Try Again";
            // continueButton.SetActive(false);
            // tryAgainButton.SetActive(true);
        }
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        _sceneManager.FadeToScene(nextSceneIndex);
    }

    public void TryAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}