using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompleteScreenUI : MonoBehaviour
{
    public Animator animator;
    public TMP_Text title;
    public TMP_Text message;
    public TMP_Text ResourceFood;

    public TMP_Text ResourceTechnology;

    public TMP_Text ResourceEnergy;

    public TMP_Text ResourceIntelligence;

    public TMP_Text ResourceCoins;
    public bool isTriggered = false;
    // Include resources plundered during battle
    public void Complete(string type, bool complete, string team = "")
    {
        // Pause the world

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
        PlayerService manager = GameManager.Instance.PlayerManager.playerService;
        var lootHolder = GameplayManager.Instance.gameplayUI.lootHolder;

        manager.AddFood(lootHolder.GetLoot(ItemType.Food));
        manager.AddTechnology(lootHolder.GetLoot(ItemType.Technology));
        manager.AddEnergy(lootHolder.GetLoot(ItemType.Energy));
        manager.AddIntelligence(lootHolder.GetLoot(ItemType.Intelligence));
        manager.AddCoins(lootHolder.GetLoot(ItemType.Coins));

        animator.SetTrigger("Complete");
        isTriggered = true;
    }
    public void BackToMainMenu()
    {
        if (!isTriggered) return;
        Debug.Log("Going Back to mainmenu");
        GameInitiator.Instance.SwitchStates(GameState.MainMenu);
    }
}
