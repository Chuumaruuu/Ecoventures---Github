using UnityEngine;

public interface IInteractable
{
    void Interact(Interaction_Manager interactionManager);

    void InteractAlternate(Interaction_Manager interactionManager);

    void OnFocusEnter(Interaction_Manager interactionManager);

    void OnFocusExit(Interaction_Manager interactionManager);
}
