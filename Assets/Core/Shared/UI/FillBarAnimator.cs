using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Core.Shared.UI
{
    /// <summary>
    /// Generalized fill bar animator using DOTween.
    /// Can be used for HP bars, experience bars, level progression, or any gauge.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class FillBarAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image fillImage;

        [Header("Animation Settings")]
        [SerializeField] private float animationDuration = 0.3f;
        [SerializeField] private Ease easeType = Ease.OutQuad;
        [SerializeField] private bool useUnscaledTime = false;

        [Header("Optional Color Transitions")]
        [SerializeField] private bool useColorTransition = false;
        [SerializeField] private Gradient colorGradient;

        [Header("Optional Effects")]
        [SerializeField] private bool punchScaleOnChange = false;
        [SerializeField] private float punchScale = 1.1f;
        [SerializeField] private float punchDuration = 0.2f;

        private Tweener currentTween;
        private float currentValue;
        private float maxValue;

        private void Awake()
        {
            if (fillImage == null)
            {
                fillImage = GetComponent<Image>();
            }

            // Ensure Image is set to Filled type
            if (fillImage.type != Image.Type.Filled)
            {
                Debug.LogWarning($"[FillBarAnimator] Image on {gameObject.name} is not set to Filled type. Setting it now.");
                fillImage.type = Image.Type.Filled;
                fillImage.fillMethod = Image.FillMethod.Horizontal;
            }
        }

        /// <summary>
        /// Initialize the fill bar with max value and starting value
        /// </summary>
        public void Initialize(float maxValue, float startingValue = 0f)
        {
            this.maxValue = maxValue;
            this.currentValue = startingValue;

            float fillAmount = maxValue > 0 ? Mathf.Clamp01(startingValue / maxValue) : 0f;
            fillImage.fillAmount = fillAmount;

            if (useColorTransition && colorGradient != null)
            {
                fillImage.color = colorGradient.Evaluate(fillAmount);
            }
        }

        /// <summary>
        /// Update the fill bar to a new value with animation
        /// </summary>
        public void UpdateValue(float newValue, bool instant = false)
        {
            currentValue = newValue;
            float targetFillAmount = maxValue > 0 ? Mathf.Clamp01(newValue / maxValue) : 0f;

            AnimateToFill(targetFillAmount, instant);
        }

        /// <summary>
        /// Update both current and max values (useful for level ups where max changes)
        /// </summary>
        public void UpdateValues(float newValue, float newMaxValue, bool instant = false)
        {
            currentValue = newValue;
            maxValue = newMaxValue;

            float targetFillAmount = maxValue > 0 ? Mathf.Clamp01(newValue / maxValue) : 0f;
            AnimateToFill(targetFillAmount, instant);
        }

        /// <summary>
        /// Update the max value only (recalculates fill percentage)
        /// </summary>
        public void UpdateMaxValue(float newMaxValue, bool instant = false)
        {
            maxValue = newMaxValue;
            float targetFillAmount = maxValue > 0 ? Mathf.Clamp01(currentValue / maxValue) : 0f;
            AnimateToFill(targetFillAmount, instant);
        }

        /// <summary>
        /// Set fill amount directly (0-1 range)
        /// </summary>
        public void SetFillAmount(float fillAmount, bool instant = false)
        {
            float clampedFill = Mathf.Clamp01(fillAmount);
            AnimateToFill(clampedFill, instant);
        }

        /// <summary>
        /// Get current fill amount (0-1)
        /// </summary>
        public float GetFillAmount() => fillImage.fillAmount;

        /// <summary>
        /// Get current value
        /// </summary>
        public float GetCurrentValue() => currentValue;

        /// <summary>
        /// Get max value
        /// </summary>
        public float GetMaxValue() => maxValue;

        private void AnimateToFill(float targetFillAmount, bool instant)
        {
            // Kill existing tween
            currentTween?.Kill();

            if (instant)
            {
                fillImage.fillAmount = targetFillAmount;

                if (useColorTransition && colorGradient != null)
                {
                    fillImage.color = colorGradient.Evaluate(targetFillAmount);
                }
                return;
            }

            // Animate fill amount
            currentTween = fillImage.DOFillAmount(targetFillAmount, animationDuration)
                .SetEase(easeType)
                .SetUpdate(useUnscaledTime)
                .OnUpdate(() =>
                {
                    if (useColorTransition && colorGradient != null)
                    {
                        fillImage.color = colorGradient.Evaluate(fillImage.fillAmount);
                    }
                });

            // Optional punch effect
            if (punchScaleOnChange)
            {
                transform.DOPunchScale(Vector3.one * (punchScale - 1f), punchDuration, 1, 0.5f)
                    .SetUpdate(useUnscaledTime);
            }
        }

        /// <summary>
        /// Smooth transition for when overflow happens (like exp bar looping)
        /// </summary>
        public async UniTask AnimateOverflow(float overfillValue)
        {
            // Animate to full
            currentTween?.Kill();
            currentTween = fillImage.DOFillAmount(1f, animationDuration * 0.5f)
                .SetEase(easeType)
                .SetUpdate(useUnscaledTime);

            await currentTween.AsyncWaitForCompletion();

            // Reset to zero
            fillImage.fillAmount = 0f;

            // Animate to overflow amount
            float targetFill = maxValue > 0 ? Mathf.Clamp01(overfillValue / maxValue) : 0f;
            currentTween = fillImage.DOFillAmount(targetFill, animationDuration * 0.5f)
                .SetEase(easeType)
                .SetUpdate(useUnscaledTime);

            await currentTween.AsyncWaitForCompletion();
        }

        /// <summary>
        /// Flash the fill bar (useful for damage indication)
        /// </summary>
        public void Flash(Color flashColor, float flashDuration = 0.2f)
        {
            Color originalColor = fillImage.color;

            fillImage.DOColor(flashColor, flashDuration * 0.5f)
                .SetUpdate(useUnscaledTime)
                .OnComplete(() =>
                {
                    fillImage.DOColor(originalColor, flashDuration * 0.5f)
                        .SetUpdate(useUnscaledTime);
                });
        }

        private void OnDestroy()
        {
            currentTween?.Kill();
        }

        private void OnDisable()
        {
            currentTween?.Kill();
        }

        #region Editor Helpers
        private void OnValidate()
        {
            if (fillImage == null)
            {
                fillImage = GetComponent<Image>();
            }
        }
        #endregion
    }
}
