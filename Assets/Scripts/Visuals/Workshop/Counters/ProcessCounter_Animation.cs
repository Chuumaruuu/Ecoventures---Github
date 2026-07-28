using UnityEngine;

public class ProcessCounter_Animation : MonoBehaviour
{
    private const string INTERACT = "Interact";

    [SerializeField] private Counter_Process _processCounter;

    private Animator _containerAnimator;

    private void Awake()
    {
        _containerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        _processCounter.OnProcessCounterInteract += ProcessCounterOnInteractAlternate;
    }

    private void ProcessCounterOnInteractAlternate(object sender, System.EventArgs e)
    {
        _containerAnimator.SetTrigger(INTERACT);
    }
}
