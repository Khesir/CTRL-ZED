using UnityEngine;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Core.Shared.UI
{
    /// <summary>
    /// Animated money display that can show previews of costs
    /// Shows: "1000 Coins" normally, "1000 - 250 = 750" when previewing cost
    /// </summary>
    public class AnimatedMoneyDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text moneyText;

        [Header("Animation Settings")]
        [SerializeField] private float previewDuration = 2f;
        [SerializeField] private bool autoRevertPreview = true;

        [Header("Colors")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color affordableColor = Color.green;
        [SerializeField] private Color unaffordableColor = Color.red;

        private int currentAmount;
        private bool isPreviewing;
        private Tweener colorTween;

        private void Awake()
        {
            if (moneyText == null)
            {
                moneyText = GetComponent<TMP_Text>();
            }
        }

        /// <summary>
        /// Update the current money amount (normal display)
        /// </summary>
        public void UpdateAmount(int amount)
        {
            currentAmount = amount;

            if (!isPreviewing)
            {
                DisplayNormal();
            }
        }

        /// <summary>
        /// Show a cost preview with calculation
        /// Example: "1000 - 250 = 750 Coins"
        /// </summary>
        public void PreviewCost(int cost, bool autoRevert = true)
        {
            if (moneyText == null) return;

            isPreviewing = true;
            int resultAmount = currentAmount - cost;
            bool canAfford = currentAmount >= cost;

            // Format: "1000 - 250 = 750"
            moneyText.text = $"{currentAmount} <color=#FF6666>-</color> {cost} <color=#888888>=</color> {resultAmount} Coins";

            // Color based on affordability
            Color targetColor = canAfford ? affordableColor : unaffordableColor;
            AnimateColor(targetColor);

            // Optional: Auto revert after delay
            if (autoRevert && autoRevertPreview)
            {
                AutoRevertAfterDelay().Forget();
            }
        }

        /// <summary>
        /// Show normal display without preview
        /// </summary>
        public void DisplayNormal()
        {
            if (moneyText == null) return;

            isPreviewing = false;
            moneyText.text = $"{currentAmount} Coins";

            AnimateColor(normalColor);
        }

        /// <summary>
        /// Flash the money display (e.g., when gaining/losing coins)
        /// </summary>
        public void Flash(Color flashColor, float duration = 0.3f)
        {
            if (moneyText == null) return;

            Color originalColor = moneyText.color;

            colorTween?.Kill();
            colorTween = moneyText.DOColor(flashColor, duration * 0.5f)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    moneyText.DOColor(originalColor, duration * 0.5f)
                        .SetUpdate(true);
                });
        }

        /// <summary>
        /// Pulse the money text (for attention)
        /// </summary>
        public void Pulse(float scale = 1.1f, float duration = 0.2f)
        {
            transform.DOPunchScale(Vector3.one * (scale - 1f), duration, 1, 0.5f)
                .SetUpdate(true);
        }

        private void AnimateColor(Color targetColor)
        {
            if (moneyText == null) return;

            colorTween?.Kill();
            colorTween = moneyText.DOColor(targetColor, 0.3f)
                .SetUpdate(true);
        }

        private async UniTaskVoid AutoRevertAfterDelay()
        {
            await UniTask.Delay((int)(previewDuration * 1000));

            if (isPreviewing)
            {
                DisplayNormal();
            }
        }

        private void OnDestroy()
        {
            colorTween?.Kill();
        }

        #region Editor Helpers
        [ContextMenu("Test Preview Affordable")]
        private void TestPreviewAffordable()
        {
            currentAmount = 1000;
            PreviewCost(250);
        }

        [ContextMenu("Test Preview Unaffordable")]
        private void TestPreviewUnaffordable()
        {
            currentAmount = 100;
            PreviewCost(250);
        }

        [ContextMenu("Test Flash Gain")]
        private void TestFlashGain()
        {
            Flash(Color.green);
        }

        [ContextMenu("Test Flash Loss")]
        private void TestFlashLoss()
        {
            Flash(Color.red);
        }
        #endregion
    }
}
