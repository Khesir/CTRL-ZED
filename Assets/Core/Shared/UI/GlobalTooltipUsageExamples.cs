using UnityEngine;
using Core.Shared.UI;
using Core.Shared.Events;

namespace Core.Shared.UI
{
    /// <summary>
    /// Examples showing how to integrate GlobalTooltipUI with your domain services
    /// and handle unsuccessful transactions, errors, and notifications.
    /// </summary>
    public class GlobalTooltipUsageExamples : MonoBehaviour
    {
        // ============================================
        // INTEGRATION WITH ECONOMY SERVICE
        // ============================================

        public void ExamplePurchaseItem(int cost)
        {
            // Assuming you have access to economy service
            // var economyService = GameManager.Instance.Player.Economy;

            // Example: Check if player has enough coins
            int currentCoins = 100; // economyService.GetCoins();

            if (currentCoins < cost)
            {
                // Send error message via event bus
                TooltipEventBus.PublishError($"Not enough coins! Need {cost}, have {currentCoins}");
                return;
            }

            // Successful purchase
            // economyService.SpendCoins(cost);
            TooltipEventBus.PublishSuccess("Item purchased successfully!");
        }

        // ============================================
        // INTEGRATION WITH RESOURCE SERVICE
        // ============================================

        public void ExampleSpendResource(string resourceType, int amount)
        {
            // Example: Check resource availability
            // var resourceService = GameManager.Instance.Player.Resource;

            // if (!resourceService.HasEnough(resourceType, amount))
            // {
            //     TooltipEventBus.PublishError($"Not enough {resourceType}! Need {amount} more");
            //     return;
            // }

            // resourceService.Spend(resourceType, amount);
            TooltipEventBus.PublishSuccess($"Spent {amount} {resourceType}");
        }

        // ============================================
        // INTEGRATION WITH DRIVES SERVICE
        // ============================================

        public void ExampleSpendDrives(int cost)
        {
            // Example: Action point system
            // var drivesService = GameManager.Instance.Player.Drives;

            int currentDrives = 5; // drivesService.CurrentDrives;

            if (currentDrives < cost)
            {
                TooltipEventBus.PublishWarning($"Not enough drives! Need {cost}, have {currentDrives}");
                return;
            }

            // drivesService.Spend(cost);
            TooltipEventBus.PublishInfo($"Spent {cost} drives");
        }

        // ============================================
        // INTEGRATION WITH CHARACTER DEPLOYMENT
        // ============================================

        public void ExampleDeployCharacter(string characterName)
        {
            // Example validation
            bool teamFull = false; // Check team capacity
            bool inCombat = true;  // Check game state

            if (teamFull)
            {
                TooltipEventBus.PublishError("Team is full! Cannot deploy more characters");
                return;
            }

            if (inCombat)
            {
                TooltipEventBus.PublishWarning("Cannot deploy during combat!");
                return;
            }

            // Deploy character
            TooltipEventBus.PublishSuccess($"{characterName} deployed!");
        }

        // ============================================
        // INTEGRATION WITH SKILL SYSTEM
        // ============================================

        public void ExampleUseSkill(string skillName)
        {
            bool onCooldown = false;  // Check cooldown
            bool hasResources = true; // Check resource cost

            if (onCooldown)
            {
                TooltipEventBus.PublishWarning($"{skillName} is on cooldown!");
                return;
            }

            if (!hasResources)
            {
                TooltipEventBus.PublishError($"Not enough resources to use {skillName}!");
                return;
            }

            // Use skill
            TooltipEventBus.PublishInfo($"{skillName} activated!");
        }

        // ============================================
        // INTEGRATION WITH WAVE SYSTEM
        // ============================================

        public void ExampleWaveNotifications()
        {
            // Wave starting
            TooltipEventBus.PublishInfo("Wave 5 incoming!");

            // Wave completed
            TooltipEventBus.PublishSuccess("Wave 5 cleared! +100 coins");

            // Boss wave warning
            TooltipEventBus.Publish(new TooltipMessage(
                "BOSS WAVE INCOMING!",
                TooltipType.Warning,
                duration: 5f
            ));
        }

        // ============================================
        // INTEGRATION WITH ANTIVIRUS/UPGRADE SYSTEM
        // ============================================

        public void ExampleUpgradeAntivirus(int cost)
        {
            // var economyService = GameManager.Instance.Player.Economy;

            int currentCoins = 500; // economyService.GetCoins();
            int maxLevel = 10;
            int currentLevel = 9;

            if (currentLevel >= maxLevel)
            {
                TooltipEventBus.PublishWarning("Antivirus already at max level!");
                return;
            }

            if (currentCoins < cost)
            {
                TooltipEventBus.PublishError($"Cannot upgrade! Need {cost} coins, have {currentCoins}");
                return;
            }

            // Perform upgrade
            TooltipEventBus.PublishSuccess($"Antivirus upgraded to level {currentLevel + 1}!");
        }

        // ============================================
        // CUSTOM MESSAGE WITH ICON
        // ============================================

        public void ExampleCustomMessageWithIcon()
        {
            // Load an icon sprite
            Sprite customIcon = null; // Resources.Load<Sprite>("Icons/CoinIcon");

            TooltipMessage customMessage = new TooltipMessage(
                "Special event: Double coins for 30 seconds!",
                TooltipType.Success,
                duration: 5f,
                icon: customIcon
            );

            TooltipEventBus.Publish(customMessage);
        }

        // ============================================
        // DIRECT CALL WITHOUT EVENT BUS
        // ============================================

        public void ExampleDirectCall()
        {
            // If you need immediate display without event bus
            if (GlobalTooltipUI.Instance != null)
            {
                GlobalTooltipUI.Instance.ShowError("Critical error occurred!");
            }
        }

        // ============================================
        // REAL INTEGRATION EXAMPLE: ECONOMY SERVICE
        // ============================================

        /// <summary>
        /// Example of how to modify your EconomyService to send tooltip messages
        /// </summary>
        public class EconomyServiceExample
        {
            private int coins;

            public bool TrySpendCoins(int amount)
            {
                if (coins < amount)
                {
                    // Send tooltip for unsuccessful transaction
                    TooltipEventBus.PublishError($"Not enough coins! Need {amount}, have {coins}");
                    return false;
                }

                coins -= amount;

                // Optional: Send success message
                // TooltipEventBus.PublishSuccess($"Spent {amount} coins");

                return true;
            }

            public void AddCoins(int amount)
            {
                coins += amount;
                TooltipEventBus.PublishSuccess($"+{amount} coins!");
            }
        }

        // ============================================
        // TESTING
        // ============================================

        [ContextMenu("Test Transaction Failure")]
        private void TestTransactionFailure()
        {
            ExamplePurchaseItem(1000); // Assuming player has less than 1000
        }

        [ContextMenu("Test Multiple Notifications")]
        private void TestMultipleNotifications()
        {
            TooltipEventBus.PublishError("Error message 1");
            TooltipEventBus.PublishWarning("Warning message 2");
            TooltipEventBus.PublishInfo("Info message 3");
            TooltipEventBus.PublishSuccess("Success message 4");
        }

        [ContextMenu("Test Event Bus")]
        private void TestEventBus()
        {
            // Send custom message via event bus
            TooltipEventBus.Publish(new TooltipMessage(
                "This is a custom tooltip message sent via event bus!",
                TooltipType.Info,
                duration: 4f
            ));
        }
    }
}
