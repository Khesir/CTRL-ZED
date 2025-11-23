using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompleteScreenUI : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text message;
    public TMP_Text ResourceFood;

    public TMP_Text ResourceTechnology;

    public TMP_Text ResourceEnergy;

    public TMP_Text ResourceIntelligence;

    public TMP_Text ResourceCoins;
    public Button backToMenuBtn;
    public bool isTriggered = false;
    private PanelAnimator animator;
    // Include resources plundered during battle
    public async UniTask CompleteAsync(string type, bool complete, string team = "")
    {
        // Pause the world
        animator = GetComponent<PanelAnimator>();
        backToMenuBtn.onClick.RemoveAllListeners();
        backToMenuBtn.onClick.AddListener(() => BackToMainMenu());
        switch (type)
        {
            case "os":
                title.text = "";
                message.text = $"OS HP runned out!";
                break;
            case "character":
                if (complete)
                {
                    title.text = "Wave Cleared!";
                    message.text = $"{team} has succesfully! Stopped the wave";
                    // GameManager.Instance.LevelManager.currentLevelIndex++;
                }
                else
                {
                    title.text = "Game Over";
                    message.text = $"{team} has been wiped! Go back to base to repair or use other team";
                }
                break;
        }
        PlayerService manager = ServiceLocator.Get<IPlayerManager>().playerService;
        var lootHolder = GameplayManager.Instance.GameplayUI.lootHolder;

        manager.AddFood(lootHolder.GetLoot(ItemType.Food));
        manager.AddTechnology(lootHolder.GetLoot(ItemType.Technology));
        manager.AddEnergy(lootHolder.GetLoot(ItemType.Energy));
        manager.AddIntelligence(lootHolder.GetLoot(ItemType.Intelligence));
        manager.AddCoins(lootHolder.GetLoot(ItemType.Coins));
        gameObject.SetActive(true);
        await animator.Show();
        isTriggered = true;
    }
    public async void BackToMainMenu()
    {
        if (!isTriggered) return;
        Debug.Log("Going Back to mainmenu");
        gameObject.SetActive(false);
        await animator.Hide();
        GameInitiator.Instance.SwitchStates(GameState.MainMenu);
    }
}
