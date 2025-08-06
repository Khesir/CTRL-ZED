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
    // Include resources plundered during battle
    public void Complete(string type, bool complete, string team = "", List<LootDropData> loots = null)
    {
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
        // ResourceFood.text = manager.GetFood() + " + " + (loots != null ? loots.food.ToString() : "0");
        // manager.AddFood(loots.food);

        // ResourceTechnology.text = manager.GetTechnology() + " + " + (loots != null ? loots.technology.ToString() : "0");
        // manager.AddTechnology(loots.technology);

        // ResourceEnergy.text = manager.GetEnergy() + " + " + (loots != null ? loots.energy.ToString() : "0");
        // manager.AddEnergy(loots.energy);

        // ResourceIntelligence.text = manager.GetIntelligence() + " + " + (loots != null ? loots.intelligence.ToString() : "0");
        // manager.AddIntelligence(loots.intelligence);

        // ResourceCoins.text = "Reward: " + manager.GetCoins() + " + " + (loots != null ? loots.coins.ToString() : "0") + " Coins";
        // manager.AddCoins(loots.coins);

        animator.SetTrigger("Complete");
    }
}
