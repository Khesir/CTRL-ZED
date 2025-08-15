using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveContainer : MonoBehaviour
{
    public GameObject nullState;
    public GameObject normalState;
    private SaveData data;
    private int index;

    [Header("Debug GO -- Don't Touch")]
    [SerializeField] private TMP_Text information;
    [SerializeField] private Button actionButton;
    public void Initialize(SaveData data, int index)
    {
        this.index = index;
        this.data = data;

        nullState.SetActive(false);
        normalState.SetActive(false);

        if (data == null)
        {
            nullState.SetActive(true);
            actionButton = nullState.GetComponentInChildren<Button>(true);
        }
        else
        {
            normalState.SetActive(true);

            // Get the tagged TMP_Text safely
            information = normalState
                .GetComponentsInChildren<TMP_Text>(true)
                .FirstOrDefault(t => t.gameObject.name == "Information");

            if (information != null)
            {
                information.text =
                    $"OSLevel: {data.playerData.level}\n" +
                    $"Funds: {data.playerData.coins}\n" +
                    $"Drives:\n" +
                    $" - {data.playerData.usableDrives}\n" +
                    $" - {data.playerData.chargedDrives}\n" +
                    $"\nTeams: {data.teams.Count}\n" +
                    $"Owned Characters: {data.ownedCharacters.Count}";
            }
            else
            {
                Debug.LogWarning("No TMP_Text found with tag 'MyTextTag'.");
            }

            actionButton = normalState.GetComponentInChildren<Button>(true);
        }

        // Setup button listener
        if (actionButton != null)
        {
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(OnActionButtonClicked);
        }

        Debug.Log("Save Slots Container Generated");
    }

    private async void OnActionButtonClicked()
    {
        SoundManager.PlaySound(SoundCategory.UI, SoundType.UI_OnTransition);

        if (data == null)
        {
            var saveData = new SaveData();
            GameManager.Instance.PlayerDataManager.SaveToSlot(index, saveData);
            await GameInitiator.Instance.PrepareGame(saveData);
        }
        else
        {
            await GameInitiator.Instance.PrepareGame(data);
            GameInitiator.Instance.SwitchStates(GameState.MainMenu);
        }

    }
}
