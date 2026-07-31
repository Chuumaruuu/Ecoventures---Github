using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndLoad_Manager : MonoBehaviour
{
    [SerializeField] private GameObject saveSuccessPanel;

    [Header("ScriptableObject References")]
    [SerializeField] private GameInventory_Data gameInventoryData;
    // [SerializeField] private Tutorial_Progress  tutorialProgress;
    [SerializeField] private Dialogue_Progress  dialogueProgress;
    [SerializeField] private List<Item_Data> allItems = new List<Item_Data>();

    [System.Serializable]
    public class SaveData
    {
        public int    slotIndex;
        public string saveDate;
        public bool   isEmpty = true;
        public int    sceneIndex;
        public int playerMoney;
        public int totalSales;
        public List<string> unlockedProductNames = new List<string>();
        public List<string> finalProductNames    = new List<string>();
        public bool moveTutorialDone;
        public bool grabTutorialDone;
        public bool moneyTutorialDone;
        public bool guidebookTutorialDone;
        public bool interactTutorialDone;
        public bool workshopMapDone;
        public bool workshopStageDone;

        public bool workshopIntro;
        public bool workshopGuidebookIntro;
        public bool workshopGuidebookIntro2;
        public bool workshopGuidebookIntro3;
        public bool workshopGuidebookIntro4;
        public bool workshopGuidebookIntro5;
        public bool workshopGuidebookIntro6;
        public bool workshopGuidebookIntro7;
        public bool workshopMovementTutorial;
        public bool workshopGrabTutorial;
        public bool mapIntro;
        public bool mapToStage1;
        public bool mapDemandIntro;
        public bool mapOutro;
        public bool mapBackToWorkshop;

        public int completionPercentage;
    }


    private string GetSavePath(int slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, $"save_slot_{slotIndex}.json");
    }

    public SaveData LoadSlot(int slotIndex)
    {
        string path = GetSavePath(slotIndex);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }
        return new SaveData { slotIndex = slotIndex, isEmpty = true };
    }

    public int CalculateCompletion()
    {
        if (allItems.Count == 0) return 0;

        int done = 0;
        foreach (Item_Data item in allItems)
            if (item != null && item.isUnlocked) done++;

        return Mathf.RoundToInt((float)done / allItems.Count * 100);
    }

    public void SaveSlot(int slotIndex)
    {
        SaveData data = new SaveData
        {
            slotIndex            = slotIndex,
            saveDate             = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
            isEmpty              = false,
            completionPercentage = CalculateCompletion(),
            sceneIndex           = SceneManager.GetActiveScene().buildIndex,
        };

        if (gameInventoryData != null)
            data.playerMoney = gameInventoryData._playerMoney;

        if (SalesTracker.Instance != null)
            data.totalSales = SalesTracker.Instance.totalSales;

        foreach (Item_Data item in allItems)
        {
            if (item != null && item.isUnlocked)
                data.unlockedProductNames.Add(item.name);
        }

        // Save crafted items ready to sell
        if (gameInventoryData != null)
        {
            foreach (Item_Data item in gameInventoryData._finalProducts)
            {
                if (item != null)
                    data.finalProductNames.Add(item.name);
            }
        }

        if (tutorialProgress != null)
        {
            data.moveTutorialDone      = tutorialProgress._moveTutorialDone;
            data.grabTutorialDone      = tutorialProgress._grabTutorialDone;
            data.moneyTutorialDone     = tutorialProgress._moneyTutorialDone;
            data.guidebookTutorialDone = tutorialProgress._guidebookTutorialDone;
            data.interactTutorialDone  = tutorialProgress._interactTutorialDone;
            data.workshopMapDone       = tutorialProgress._workshopMapDone;
            data.workshopStageDone     = tutorialProgress._workshopStageDone;
        }

        if (dialogueProgress != null)
        {
            data.workshopIntro            = dialogueProgress.WORKSHOP_INTRO;
            data.workshopGuidebookIntro   = dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO;
            data.workshopGuidebookIntro2  = dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO2;
            data.workshopGuidebookIntro3  = dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO3;
            data.workshopGuidebookIntro4  = dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO4;
            data.workshopGuidebookIntro5  = dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO5;
            data.workshopGuidebookIntro6  = dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO6;
            data.workshopGuidebookIntro7  = dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO7;
            data.workshopMovementTutorial = dialogueProgress.WORKSHOP_MOVEMENT_TUTORIAL;
            data.workshopGrabTutorial     = dialogueProgress.WORKSHOP_GRAB_TUTORIAL;
            data.mapIntro                 = dialogueProgress.MAP_INTRO;
            data.mapToStage1              = dialogueProgress.MAP_TOSTAGE1;
            data.mapDemandIntro           = dialogueProgress.MAP_DEMANDINTRO;
            data.mapOutro                 = dialogueProgress.MAP_OUTRO;
            data.mapBackToWorkshop        = dialogueProgress.MAP_BACKTOWORKSHOP;
        }

        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(GetSavePath(slotIndex), json);
        Debug.Log($"Game saved to slot {slotIndex} at {GetSavePath(slotIndex)}");
    }

    public void ApplyLoadedData(SaveData data)
    {
        if (data == null || data.isEmpty) return;

        if (gameInventoryData != null)
        {
            gameInventoryData._playerMoney = data.playerMoney;

            // Restore crafted items ready to sell
            gameInventoryData._finalProducts.Clear();
            foreach (string itemName in data.finalProductNames)
            {
                Item_Data match = allItems.Find(i => i != null && i.name == itemName);
                if (match != null)
                    gameInventoryData._finalProducts.Add(match);
            }
        }

        if (SalesTracker.Instance != null)
            SalesTracker.Instance.totalSales = data.totalSales;

        // Restore unlocked products
        if (Unlock_Manager.Instance != null)
        {
            foreach (Item_Data item in allItems)
            {
                if (item == null) continue;
                bool shouldBeUnlocked = data.unlockedProductNames.Contains(item.name);
                Unlock_Manager.Instance.SetUnlocked(item, shouldBeUnlocked);
            }
        }

        if (tutorialProgress != null)
        {
            tutorialProgress._moveTutorialDone      = data.moveTutorialDone;
            tutorialProgress._grabTutorialDone      = data.grabTutorialDone;
            tutorialProgress._moneyTutorialDone     = data.moneyTutorialDone;
            tutorialProgress._guidebookTutorialDone = data.guidebookTutorialDone;
            tutorialProgress._interactTutorialDone  = data.interactTutorialDone;
            tutorialProgress._workshopMapDone       = data.workshopMapDone;
            tutorialProgress._workshopStageDone     = data.workshopStageDone;
        }

        if (dialogueProgress != null)
        {
            dialogueProgress.WORKSHOP_INTRO            = data.workshopIntro;
            dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO  = data.workshopGuidebookIntro;
            dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO2 = data.workshopGuidebookIntro2;
            dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO3 = data.workshopGuidebookIntro3;
            dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO4 = data.workshopGuidebookIntro4;
            dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO5 = data.workshopGuidebookIntro5;
            dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO6 = data.workshopGuidebookIntro6;
            dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO7 = data.workshopGuidebookIntro7;
            dialogueProgress.WORKSHOP_MOVEMENT_TUTORIAL = data.workshopMovementTutorial;
            dialogueProgress.WORKSHOP_GRAB_TUTORIAL    = data.workshopGrabTutorial;
            dialogueProgress.MAP_INTRO                 = data.mapIntro;
            dialogueProgress.MAP_TOSTAGE1              = data.mapToStage1;
            dialogueProgress.MAP_DEMANDINTRO           = data.mapDemandIntro;
            dialogueProgress.MAP_OUTRO                 = data.mapOutro;
            dialogueProgress.MAP_BACKTOWORKSHOP        = data.mapBackToWorkshop;
        }
    }

    public void DeleteSlot(int slotIndex)
    {
        string path = GetSavePath(slotIndex);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Save slot {slotIndex} deleted.");
        }
    }

    public void OnSaveSlotSelected(int slotIndex)
    {
        SaveSlot(slotIndex);
        Debug.Log($"Slot {slotIndex} saved!");

        if (saveSuccessPanel != null)
            saveSuccessPanel.SetActive(true);
    }

    public void OnLoadSlotSelected(int slotIndex)
    {
        string path = System.IO.Path.Combine(Application.persistentDataPath, $"save_slot_{slotIndex}.json");
        Debug.Log($"[Load] Looking for save file at: {path}");
        Debug.Log($"[Load] File exists: {System.IO.File.Exists(path)}");

        SaveData data = LoadSlot(slotIndex);
        Debug.Log($"[Load] data.isEmpty = {data.isEmpty}, data.sceneIndex = {data.sceneIndex}");

        if (data.isEmpty)
        {
            Debug.Log($"[Load] Slot {slotIndex} is empty. Aborting.");
            return;
        }

        Debug.Log($"[Load] Applying data and loading scene {data.sceneIndex}");
        ApplyLoadedData(data);
        SceneManager.LoadScene(data.sceneIndex);
    }

    public void Save()
    {
        SaveSlot(0);
    }

    public void Done()
    {
        SceneManager.LoadScene(0);
    }
}
