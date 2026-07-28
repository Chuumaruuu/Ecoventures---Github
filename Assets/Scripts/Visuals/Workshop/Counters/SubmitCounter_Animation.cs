using UnityEngine;

public class SubmitCounter_Animation : MonoBehaviour
{
    private const string INTERACT = "Interact";

    [SerializeField] private Counter_Submit _submitCounter;

    private Animator _containerAnimator;

    private void Awake()
    {
        _containerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        _submitCounter.OnItemSubmit += ContainerCounterOnPlayerGrabbedObject;
    }

    private void ContainerCounterOnPlayerGrabbedObject(object sender, System.EventArgs e)
    {
        _containerAnimator.SetTrigger(INTERACT);
    }
}
