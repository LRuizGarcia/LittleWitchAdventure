using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    public TMP_Text slot0;
    public TMP_Text slot1;
    public TMP_Text slot2;
    public TMP_Text slot3;

    public GameObject emptySlotMessage;

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
        Debug.Log($"Loading from slot {slotIndex}");

        SeedSlot saveSlot = GameController.gameController.saveSlots[slotIndex];

        // Check if the slot is empty
        if (saveSlot.seed == "")
        {
            Debug.Log($"Slot {slotIndex} is empty.");
            ShowEmptySlotMessage(); // Show a message if the slot is empty
            return;
        }

        // Store the seed temporarily
        GameController.gameController.currentGeneratedLevelSeed = saveSlot.seed;

        // Load the AutoLevel scene
        //SceneManager.sceneLoaded += OnSceneLoaded;
        //SceneManager.LoadScene("AutoLevel");

        //GameController.gameController.LoadFromSeed(saveSlot.seed);
        GameController.gameController.LoadGeneratedLevel(false);

    }
    /*
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "AutoLevel")
        {
            // Retrieve and apply the seed to the MapGenerator
            string seed = GameController.gameController.currentGeneratedLevelSeed;
            if (!string.IsNullOrEmpty(seed))
            {
                //GameController.gameController.LoadFromSeed(seed);
                GameController.gameController.LoadFromSeed(false);
            }

            // Unsubscribe from the event to prevent multiple calls
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }*/

    private void ShowEmptySlotMessage()
    {
        emptySlotMessage.SetActive(true);
    }

    public void CloseEmptySlotMessage()
    {
        emptySlotMessage.SetActive(false);
    }

}
