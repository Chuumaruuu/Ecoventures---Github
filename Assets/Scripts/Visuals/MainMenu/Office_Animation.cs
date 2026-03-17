using UnityEngine;

public class Office_Animation : MonoBehaviour
{
    [SerializeField] private Animator _officeAnimator;

    public void TriggerOfficeAnimation()
    {
        _officeAnimator.SetTrigger("Transition");
    }
}
