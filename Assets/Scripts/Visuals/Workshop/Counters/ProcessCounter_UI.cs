using UnityEngine;
using UnityEngine.UI;

public class ProcessCounter_UI : MonoBehaviour
{
    [SerializeField] private Counter_Process _processCounter;
    [SerializeField] private Image _barImage;

    private void Start()
    {
        _processCounter.OnProgressChanged += CounterOnProgressChanged;
        _barImage.fillAmount = 0f;
        HideUI();
    }

    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }

    private void CounterOnProgressChanged(object sender, Counter_Process.OnProgressChangedEventArgs e) 
    {
        _barImage.fillAmount = e._progressTimerNormalized;

        if (e._progressTimerNormalized == 0f || e._progressTimerNormalized == 1f)
        {
            HideUI();
        }
        else
        {
            ShowUI();
        }
    }

    private void ShowUI()
    {
        this.gameObject.SetActive(true);
    }

    private void HideUI()
    {
        this.gameObject.SetActive(false); 
    }
}
