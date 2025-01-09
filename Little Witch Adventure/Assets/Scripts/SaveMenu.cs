using UnityEngine;
using TMPro;

public class SaveMenu : MonoBehaviour
{
    public GameObject saveMenu;

    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;

    public TMP_Text slot0;
    public TMP_Text slot1;
    public TMP_Text slot2;
    public TMP_Text slot3;

    //public TMP_InputField slotNameInput;
    public GameObject nameInputMenu;
    public GameObject overwriteMenu;

    private int selectedSlot = -1;

    public enum SaveMenuSource
    {
        PauseMenu,
        WinScreen,
        LoseScreen
    }

    private SaveMenuSource sourceMenu;

    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        UpdateSlots();
    }

    public void SetSaveMenu(bool save)
    {
        saveMenu.SetActive(true);
        UpdateSlots();
    }

    public void SetSourceMenu(SaveMenuSource source)
    {
        sourceMenu = source;
    }

    public void BackToPauseMenu()
    {
        saveMenu.SetActive(false);
        audioManager.PlaySFX(audioManager.button);
        switch (sourceMenu)
        {
            case SaveMenuSource.PauseMenu:
                PauseMenu pauseMenuScript = pauseMenu.GetComponentInParent<PauseMenu>();
                pauseMenuScript.CloseSaveMenu();
                break;
            case SaveMenuSource.WinScreen:
                winMenu.SetActive(true);
                break;
            case SaveMenuSource.LoseScreen:
                loseMenu.SetActive(true);
                break;
        }
        
    }

    // Updates save slot names
    void UpdateSlots()
    {
        slot0.text = GameController.gameController.GetSeedSlotName(0);
        slot1.text = GameController.gameController.GetSeedSlotName(1);
        slot2.text = GameController.gameController.GetSeedSlotName(2);
        slot3.text = GameController.gameController.GetSeedSlotName(3);
    }

    public void SaveIntoSlot(int slotIndex)
    {
        Debug.Log($"Selecting save slot {slotIndex}");
        audioManager.PlaySFX(audioManager.button);
        selectedSlot = slotIndex;

        // Check if the slot is empty
        if (GameController.gameController.saveSlots[slotIndex].seed == "")
        {
            Debug.Log($"SEED IS NULL");
            // Ask save slot name
            
            string defaultName = $"Save {slotIndex + 1}";
            nameInputMenu.GetComponentInChildren<TMP_InputField>().text = defaultName;
            nameInputMenu.SetActive(true);

        }
        else
        {
            // Overwrite save slot
            ShowOverwriteConfirmation();
        }
    }

    public void CancelNameInput()
    {
        audioManager.PlaySFX(audioManager.button);
        nameInputMenu.SetActive(false);
    }

    public void ConfirmSave()
    {
        audioManager.PlaySFX(audioManager.button);
        string saveName = nameInputMenu.GetComponentInChildren<TMP_InputField>().text;

        // If name is empty
        if (saveName == null)
        {
            saveName = $"Save {selectedSlot + 1}"; // Default name
        }

        // Save the data into the selected slot
        GameController.gameController.SaveCurrentGeneratedLevel(selectedSlot, saveName);

        // Update the save slot names
        UpdateSlots();

        nameInputMenu.SetActive(false);

        Debug.Log($"Level saved to slot {selectedSlot} with name: {saveName}");
    }

    private void ShowOverwriteConfirmation()
    {
        audioManager.PlaySFX(audioManager.button);
        overwriteMenu.SetActive(true);
    }

    public void ConfirmOverwrite()
    {
        audioManager.PlaySFX(audioManager.button);
        // Show the name input field to rename if desired
        nameInputMenu.GetComponentInChildren<TMP_InputField>().text = GameController.gameController.saveSlots[selectedSlot].name; // Populate with current name
        nameInputMenu.SetActive(true);

        overwriteMenu.SetActive(false);
    }

    public void CancelOverwrite()
    {
        audioManager.PlaySFX(audioManager.button);
        // Cancel the overwrite action
        overwriteMenu.SetActive(false);
        selectedSlot = -1;
    }



}
