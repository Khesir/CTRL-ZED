using UnityEngine;
using Core.Shared.Events;

namespace Core.Shared.UI
{
    /// <summary>
    /// Helper class with common patterns for integrating tooltips into your services.
    /// Copy these patterns into your existing service classes.
    /// </summary>
    public static class TooltipIntegrationHelper
    {
        // ============================================
        // PATTERN 1: Try-Based Methods
        // ============================================
        // Use this pattern when operations can fail and you want to notify the user

        public static bool TryWithTooltip<T>(
            bool condition,
            System.Action action,
            string errorMessage)
        {
            if (!condition)
            {
                TooltipEventBus.PublishError(errorMessage);
                return false;
            }

            action?.Invoke();
            return true;
        }

        // Example usage in your service:
        // public bool TryPurchase(int cost)
        // {
        //     return TooltipIntegrationHelper.TryWithTooltip(
        //         coins >= cost,
        //         () => { coins -= cost; },
        //         $"Not enough coins! Need {cost}"
        //     );
        // }

        // ============================================
        // PATTERN 2: Validation with Multiple Checks
        // ============================================

        public static bool ValidateWithTooltips(params (bool condition, string errorMessage)[] validations)
        {
            foreach (var (condition, errorMessage) in validations)
            {
                if (!condition)
                {
                    TooltipEventBus.PublishError(errorMessage);
                    return false;
                }
            }
            return true;
        }

        // Example usage:
        // public bool CanDeployCharacter()
        // {
        //     return TooltipIntegrationHelper.ValidateWithTooltips(
        //         (!IsTeamFull(), "Team is full!"),
        //         (!IsInCombat(), "Cannot deploy during combat!"),
        //         (HasEnoughCoins(), "Not enough coins!")
        //     );
        // }

        // ============================================
        // PATTERN 3: Transaction with Feedback
        // ============================================

        public static bool ExecuteTransaction(
            bool canExecute,
            System.Action onSuccess,
            string errorMessage,
            string successMessage = null)
        {
            if (!canExecute)
            {
                TooltipEventBus.PublishError(errorMessage);
                return false;
            }

            onSuccess?.Invoke();

            if (!string.IsNullOrEmpty(successMessage))
            {
                TooltipEventBus.PublishSuccess(successMessage);
            }

            return true;
        }

        // Example usage:
        // public bool PurchaseUpgrade(int cost)
        // {
        //     return TooltipIntegrationHelper.ExecuteTransaction(
        //         coins >= cost,
        //         () => { coins -= cost; level++; },
        //         $"Not enough coins! Need {cost}",
        //         "Upgrade purchased!"
        //     );
        // }

        // ============================================
        // PATTERN 4: Range Check with Tooltip
        // ============================================

        public static bool IsInRange(
            float value,
            float min,
            float max,
            string valueName,
            bool showTooltip = true)
        {
            if (value < min)
            {
                if (showTooltip)
                    TooltipEventBus.PublishWarning($"{valueName} is too low! Minimum: {min}");
                return false;
            }

            if (value > max)
            {
                if (showTooltip)
                    TooltipEventBus.PublishWarning($"{valueName} is too high! Maximum: {max}");
                return false;
            }

            return true;
        }

        // ============================================
        // PATTERN 5: Cooldown Check
        // ============================================

        public static bool CheckCooldown(
            float lastUsedTime,
            float cooldownDuration,
            string actionName,
            bool showTooltip = true)
        {
            float timeSinceLastUse = Time.time - lastUsedTime;
            bool isReady = timeSinceLastUse >= cooldownDuration;

            if (!isReady && showTooltip)
            {
                float remaining = cooldownDuration - timeSinceLastUse;
                TooltipEventBus.PublishWarning($"{actionName} on cooldown! {remaining:F1}s remaining");
            }

            return isReady;
        }

        // Example usage:
        // private float lastSkillUse;
        // public bool TryUseSkill()
        // {
        //     if (!TooltipIntegrationHelper.CheckCooldown(lastSkillUse, 5f, "Heal Skill"))
        //         return false;
        //
        //     UseSkill();
        //     lastSkillUse = Time.time;
        //     return true;
        // }

        // ============================================
        // PATTERN 6: Resource Cost Check
        // ============================================

        public static bool CheckResourceCost(
            int current,
            int required,
            string resourceName,
            bool showTooltip = true)
        {
            bool hasEnough = current >= required;

            if (!hasEnough && showTooltip)
            {
                int shortage = required - current;
                TooltipEventBus.PublishError($"Not enough {resourceName}! Need {shortage} more");
            }

            return hasEnough;
        }

        // ============================================
        // PATTERN 7: Achievement/Milestone Notification
        // ============================================

        public static void NotifyMilestone(string message, float duration = 5f)
        {
            TooltipEventBus.Publish(new TooltipMessage(
                message,
                TooltipType.Success,
                duration: duration
            ));
        }

        // Example usage:
        // if (level >= 10 && !hasReachedLevel10)
        // {
        //     TooltipIntegrationHelper.NotifyMilestone("Level 10 reached! New skills unlocked!");
        //     hasReachedLevel10 = true;
        // }

        // ============================================
        // PATTERN 8: Warning Before Critical Action
        // ============================================

        public static void WarnCriticalAction(string warning, float duration = 5f)
        {
            TooltipEventBus.Publish(new TooltipMessage(
                warning,
                TooltipType.Warning,
                duration: duration
            ));
        }

        // Example usage:
        // public void OnLowHealth()
        // {
        //     if (health <= maxHealth * 0.2f)
        //     {
        //         TooltipIntegrationHelper.WarnCriticalAction("CRITICAL HEALTH!", 4f);
        //     }
        // }

        // ============================================
        // PATTERN 9: Info Notification
        // ============================================

        public static void NotifyInfo(string message, float duration = 3f)
        {
            TooltipEventBus.PublishInfo(message);
        }

        // ============================================
        // QUICK REFERENCE FOR YOUR SERVICES
        // ============================================

        /*
         * ECONOMY SERVICE INTEGRATION:
         * ----------------------------
         * public bool TryPurchase(int cost)
         * {
         *     if (!CheckResourceCost(coins, cost, "Coins"))
         *         return false;
         *
         *     coins -= cost;
         *     TooltipEventBus.PublishSuccess("Purchase successful!");
         *     return true;
         * }
         *
         * RESOURCE SERVICE INTEGRATION:
         * ----------------------------
         * public bool TrySpend(string resource, int amount)
         * {
         *     int current = GetResource(resource);
         *     if (!CheckResourceCost(current, amount, resource))
         *         return false;
         *
         *     SpendResource(resource, amount);
         *     return true;
         * }
         *
         * DRIVES SERVICE INTEGRATION:
         * ----------------------------
         * public bool TrySpendDrives(int cost)
         * {
         *     return ExecuteTransaction(
         *         drives >= cost,
         *         () => drives -= cost,
         *         $"Not enough drives! Need {cost}"
         *     );
         * }
         *
         * SKILL SERVICE INTEGRATION:
         * ----------------------------
         * public bool TryUseSkill(Skill skill)
         * {
         *     return ValidateWithTooltips(
         *         (!skill.IsOnCooldown, $"{skill.Name} on cooldown!"),
         *         (HasEnoughMana(skill.Cost), "Not enough mana!")
         *     );
         * }
         *
         * CHARACTER DEPLOYMENT:
         * ----------------------------
         * public bool TryDeploy(Character character)
         * {
         *     return ValidateWithTooltips(
         *         (!IsTeamFull(), "Team is full!"),
         *         (!IsInCombat(), "Cannot deploy during combat!"),
         *         (HasEnoughCoins(), "Not enough coins!")
         *     );
         * }
         */
    }

    // ============================================
    // EXAMPLE SERVICE WITH FULL INTEGRATION
    // ============================================

    /// <summary>
    /// Example showing a complete service with tooltip integration
    /// </summary>
    public class ExampleServiceWithTooltips
    {
        private int coins = 100;
        private int drives = 10;
        private int maxDrives = 10;

        public bool TryPurchaseItem(int cost)
        {
            return TooltipIntegrationHelper.ExecuteTransaction(
                coins >= cost,
                () => coins -= cost,
                $"Not enough coins! Need {cost}, have {coins}",
                "Item purchased!"
            );
        }

        public bool TrySpendDrives(int amount)
        {
            return TooltipIntegrationHelper.CheckResourceCost(
                drives,
                amount,
                "Drives"
            ) && ExecuteSpend(amount);
        }

        private bool ExecuteSpend(int amount)
        {
            drives -= amount;
            return true;
        }

        public void OnDrivesRecharged()
        {
            if (drives >= maxDrives)
            {
                TooltipEventBus.PublishInfo("Drives fully recharged!");
            }
        }

        public void OnLowDrives()
        {
            if (drives <= 2)
            {
                TooltipIntegrationHelper.WarnCriticalAction("Drives critically low!");
            }
        }
    }
}
