using UnityEngine;
using System;
using System.Dynamic;

public class HintsOpenerScript : MonoBehaviour
{

    bool isPlayerInside;
    [SerializeField] private GameObject Hint11;
    [SerializeField] private GameObject Hint12;
    [SerializeField] private GameObject Hint13;
    [SerializeField] private GameObject Hint21;
    [SerializeField] private GameObject Hint22;
    [SerializeField] private GameObject Hint23;
    [SerializeField] private GameObject ExploreUI;

    void Start()
    {
         Player_Input playerInput = FindFirstObjectByType<Player_Input>();
        if (playerInput != null) 
        {
            playerInput.OnInteractAlternateAction += PlayerInput_OnInteractAlternateAction;
        }
    }

    private void PlayerInput_OnInteractAlternateAction(object sender, EventArgs e) 
    {
        if (isPlayerInside) 
        {
            if (this.gameObject.name == "Hint for Stage 1-1")
            {
                Time.timeScale = 0f;
                Hint11Phase();
            }
            if (this.gameObject.name == "Hint for Stage 1-2")
            {
                Time.timeScale = 0f;
                Hint12Phase();
            }
            if (this.gameObject.name == "Hint for Stage 1-3")
            {
                Time.timeScale = 0f;
                Hint13Phase();
            }
            if (this.gameObject.name == "Hint for Stage 2-1")
            {
                Time.timeScale = 0f;
                Hint21Phase();
            }
            if (this.gameObject.name == "Hint for Stage 2-2")
            {
                Time.timeScale = 0f;
                Hint22Phase();
            }
            if (this.gameObject.name == "Hint for Stage 2-3")
            {
                Time.timeScale = 0f;
                Hint23Phase();
            }
        }   
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

        private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isPlayerInside = false;
            ExploringPhase();
        }
    }

    public void Hint11Phase()
    {
        ExploreUI.SetActive(false);
        Hint11.SetActive(true);
    }

    public void Hint12Phase()
    {
        ExploreUI.SetActive(false);
        Hint12.SetActive(true);
    }

    public void Hint13Phase()
    {
        ExploreUI.SetActive(false);
        Hint13.SetActive(true);
    }

    public void Hint21Phase()
    {
        ExploreUI.SetActive(false);
        Hint21.SetActive(true);
    }

    public void Hint22Phase()
    {
        ExploreUI.SetActive(false);
        Hint22.SetActive(true);
    }

    public void Hint23Phase()
    {
        ExploreUI.SetActive(false);
        Hint23.SetActive(true);
    }

    public void ExploringPhase()
    {
        Time.timeScale = 1.0f;
        ExploreUI.SetActive(true);
        Hint11.SetActive(false);
        Hint12.SetActive(false);
        Hint13.SetActive(false);
        Hint21.SetActive(false);
        Hint22.SetActive(false);
        Hint23.SetActive(false);
    }

}
