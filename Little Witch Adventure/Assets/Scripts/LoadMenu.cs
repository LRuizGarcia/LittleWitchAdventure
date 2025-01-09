using UnityEngine;
using TMPro;

public class LoadMenu : MonoBehaviour
{
    public TMP_Text slot0;
    public TMP_Text slot1;
    public TMP_Text slot2;
    public TMP_Text slot3;

    public GameObject emptySlotMessage;

    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        UpdateSlots();
    }

    // Updates load slot names
    void UpdateSlots()
    {
        slot0.text = GameController.gameController.GetSeedSlotName(0);
        slot1.text = GameController.gameController.GetSeedSlotName(1);
        slot2.text = GameController.gameController.GetSeedSlotName(2);
        slot3.text = GameController.gameController.GetSeedSlotName(3);
    }

    public void LoadFromSlot(int slotIndex)
    {

        audioManager.PlaySFX(audioManager.button);

        Debug.Log($"Loading from slot {slotIndex}");

        SeedSlot saveSlot = GameController.gameController.saveSlots[slotIndex];

        // Check if the slot is empty
        if (saveSlot.seed == "")
        {
            Debug.Log($"Slot {slotIndex} is empty.");
            ShowEmptySlotMessage(); // Show a message if the slot is empty
            return;
        }


        GameController.gameController.currentGeneratedLevelSeed = saveSlot.seed;

        GameController.gameController.LoadGeneratedLevel(false);

    }

    private void ShowEmptySlotMessage()
    {
        emptySlotMessage.SetActive(true);
    }

    public void CloseEmptySlotMessage()
    {
        audioManager.PlaySFX(audioManager.button);
        emptySlotMessage.SetActive(false);
    }

}
