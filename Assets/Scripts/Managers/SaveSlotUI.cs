using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SaveSlotUI : MonoBehaviour
{
    [System.Serializable]
    public class SlotDisplay
    {
        public TextMeshProUGUI dateText;
        public TextMeshProUGUI percentText;
        public Button      button;
    }

    [SerializeField] private SlotDisplay[] slots = new SlotDisplay[3];

    [Header("Text when slot is empty")]
    [SerializeField] private string emptyDateText    = "— Empty —";
    [SerializeField] private string emptyPercentText = "";

    private void OnEnable()
    {
        RefreshAllSlots();
    }

    public void RefreshAllSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            RefreshSlot(i);
        }
    }

    private void RefreshSlot(int index)
    {
        SlotDisplay slot = slots[index];
        if (slot == null) return;

        SaveAndLoad_Manager.SaveData data = ReadSaveData(index);

        if (data == null || data.isEmpty)
        {
            // Empty slot
            if (slot.dateText    != null) slot.dateText.text    = emptyDateText;
            if (slot.percentText != null) slot.percentText.text = emptyPercentText;
        }
        else
        {
            // Populated slot
            if (slot.dateText    != null) slot.dateText.text    = data.saveDate;
            if (slot.percentText != null) slot.percentText.text = data.completionPercentage + "%";
        }
    }

    private SaveAndLoad_Manager.SaveData ReadSaveData(int slotIndex)
    {
        string path = Path.Combine(Application.persistentDataPath, $"save_slot_{slotIndex}.json");
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveAndLoad_Manager.SaveData>(json);
    }
}
