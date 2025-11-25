using UnityEngine;
using Core.Shared.UI;
using Cysharp.Threading.Tasks;

namespace Core.Shared.UI
{
    /// <summary>
    /// Example implementations showing how to use FillBarAnimator for different purposes
    /// </summary>
    public class FillBarUsageExamples : MonoBehaviour
    {
        [Header("Health Bar Example")]
        [SerializeField] private FillBarAnimator healthBar;

        [Header("Experience Bar Example")]
        [SerializeField] private FillBarAnimator experienceBar;

        [Header("Resource Bar Example")]
        [SerializeField] private FillBarAnimator resourceBar;

        // ============================================
        // HEALTH BAR USAGE
        // ============================================

        public void InitializeHealthBar(float maxHealth, float currentHealth)
        {
            healthBar.Initialize(maxHealth, currentHealth);
        }

        public void OnHealthChanged(float newHealth)
        {
            healthBar.UpdateValue(newHealth);
        }

        public void OnTakeDamage(float newHealth)
        {
            healthBar.UpdateValue(newHealth);
            // Optional: Flash red on damage
            healthBar.Flash(Color.red, 0.3f);
        }

        public void OnHeal(float newHealth)
        {
            healthBar.UpdateValue(newHealth);
            // Optional: Flash green on heal
            healthBar.Flash(Color.green, 0.3f);
        }

        public void OnMaxHealthChanged(float newMaxHealth, float currentHealth)
        {
            // When max health changes (e.g., from buffs/debuffs)
            healthBar.UpdateValues(currentHealth, newMaxHealth);
        }

        // ============================================
        // EXPERIENCE BAR USAGE
        // ============================================

        public void InitializeExperienceBar(float expToNextLevel)
        {
            experienceBar.Initialize(expToNextLevel, 0f);
        }

        public void OnExperienceGained(float currentExp, float expToNextLevel)
        {
            // Check if level up occurred
            if (currentExp >= expToNextLevel)
            {
                // Handle overflow with smooth animation
                HandleExperienceLevelUp(currentExp, expToNextLevel).Forget();
            }
            else
            {
                experienceBar.UpdateValue(currentExp);
            }
        }

        private async UniTaskVoid HandleExperienceLevelUp(float totalExp, float oldExpToNextLevel)
        {
            float overflow = totalExp - oldExpToNextLevel;

            // Animate to full, then loop back with overflow
            await experienceBar.AnimateOverflow(overflow);

            // Update max value for new level
            float newExpToNextLevel = CalculateExpForNextLevel(); // Your calculation
            experienceBar.UpdateMaxValue(newExpToNextLevel, instant: true);
        }

        // ============================================
        // RESOURCE BAR USAGE (Drives, Energy, etc.)
        // ============================================

        public void InitializeResourceBar(float maxResource, float currentResource)
        {
            resourceBar.Initialize(maxResource, currentResource);
        }

        public void OnResourceSpent(float newAmount)
        {
            resourceBar.UpdateValue(newAmount);
        }

        public void OnResourceGained(float newAmount)
        {
            resourceBar.UpdateValue(newAmount);
        }

        // ============================================
        // DIRECT FILL AMOUNT USAGE
        // ============================================

        public void SetProgressPercentage(FillBarAnimator bar, float percentage)
        {
            // When you just want to set a 0-1 percentage directly
            bar.SetFillAmount(percentage);
        }

        // ============================================
        // UTILITY
        // ============================================

        private float CalculateExpForNextLevel()
        {
            // Replace with your actual calculation
            return 100f;
        }

        // ============================================
        // TESTING IN EDITOR
        // ============================================

        [ContextMenu("Test Health Damage")]
        private void TestHealthDamage()
        {
            float current = healthBar.GetCurrentValue();
            OnTakeDamage(Mathf.Max(0, current - 20f));
        }

        [ContextMenu("Test Health Heal")]
        private void TestHealthHeal()
        {
            float current = healthBar.GetCurrentValue();
            float max = healthBar.GetMaxValue();
            OnHeal(Mathf.Min(max, current + 30f));
        }

        [ContextMenu("Test Experience Gain")]
        private void TestExperienceGain()
        {
            float current = experienceBar.GetCurrentValue();
            OnExperienceGained(current + 25f, experienceBar.GetMaxValue());
        }
    }
}
