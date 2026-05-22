using UnityEngine;
using System;

public class BoothOpenerScript : MonoBehaviour
{

    bool isPlayerInside;    
    [SerializeField] private GameObject SellingCamera;
    [SerializeField] private GameObject BoothUI;
    [SerializeField] private GameObject Booth2UI;
    [SerializeField] private GameObject Booth3UI;
    [SerializeField] private GameObject ExploreCamera;
    [SerializeField] private GameObject SellingUI;
    [SerializeField] private GameObject ExploreUI;
    [SerializeField] private GameObject RightAnswerUI;
    [SerializeField] private GameObject WrongAnswerUI;

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
            if (this.gameObject.name == "BoothOpener")
            {
                BoothPhase();
            }
             if (this.gameObject.name == "BoothOpener2")
            {
                BoothPhase2();
            }
             if (this.gameObject.name == "BoothOpener3")
            {
                BoothPhase3();
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
            ExplorePhase();
        }
    }

    public void BoothPhase()
    {
        ExploreUI.SetActive(false);
        BoothUI.SetActive(true);
    }

    public void BoothPhase2()
    {
        ExploreUI.SetActive(false);
        Booth2UI.SetActive(true);
    }

    public void BoothPhase3()
    {
        ExploreUI.SetActive(false);
        Booth3UI.SetActive(true);
    }

    public void ExplorePhase()
    {
        SellingCamera.SetActive(false);
        SellingUI.SetActive(false);
        BoothUI.SetActive(false);
        Booth2UI.SetActive(false);
        Booth3UI.SetActive(false);
        ExploreCamera.SetActive(true);
        ExploreUI.SetActive(true);
    }

    public bool Answer(bool ans)
    {
        if(ans == true)
        {
            RightAnswerUI.SetActive(true);
            WrongAnswerUI.SetActive(false);
            return true;
        }
        else
        {
            RightAnswerUI.SetActive(false);
            WrongAnswerUI.SetActive(true);
            return false;
        }
    }
}
