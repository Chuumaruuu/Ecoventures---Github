using UnityEngine;

public class ProgressCounter_Animation : MonoBehaviour
{
    private const string SMELTING = "Smelting";

    [SerializeField] private Counter_Progress _progressCounter;

    private Animator _containerAnimator;

    private void Awake()
    {
        _containerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        _progressCounter.OnProgressCooking += ProgressCounterOnChangeCookingState;
    }

    private void ProgressCounterOnChangeCookingState(object sender, System.EventArgs e)
    {
        _containerAnimator.SetBool(SMELTING, _progressCounter._isSmelting);
    }
}
