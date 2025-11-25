using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Core.Shared.Events;

namespace Core.Shared.UI
{
    /// <summary>
    /// Global tooltip/notification UI that slides in from top or bottom.
    /// Displays messages for unsuccessful transactions, errors, warnings, and info.
    /// </summary>
    public class GlobalTooltipUI : MonoBehaviour
    {
        public static GlobalTooltipUI Instance { get; private set; }

        [Header("References")]
        [SerializeField] private RectTransform tooltipContainer;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image iconImage;

        [Header("Animation Settings")]
        [SerializeField] private SlideDirection slideDirection = SlideDirection.Down;
        [SerializeField] private float slideDistance = 150f;
        [SerializeField] private float slideDuration = 0.4f;
        [SerializeField] private Ease slideInEase = Ease.OutBack;
        [SerializeField] private Ease slideOutEase = Ease.InBack;

        [Header("Display Settings")]
        [SerializeField] private float defaultDisplayDuration = 3f;
        [SerializeField] private float minDisplayDuration = 1.5f;
        [SerializeField] private float maxDisplayDuration = 8f;

        [Header("Colors")]
        [SerializeField] private Color errorColor = new Color(0.8f, 0.2f, 0.2f, 0.95f);
        [SerializeField] private Color warningColor = new Color(0.9f, 0.7f, 0.2f, 0.95f);
        [SerializeField] private Color infoColor = new Color(0.2f, 0.5f, 0.8f, 0.95f);
        [SerializeField] private Color successColor = new Color(0.2f, 0.7f, 0.3f, 0.95f);

        [Header("Optional Effects")]
        [SerializeField] private bool useIconPulse = true;
        [SerializeField] private bool useTextTypewriter = false;
        [SerializeField] private float typewriterSpeed = 0.03f;

        private Queue<TooltipMessage> messageQueue = new Queue<TooltipMessage>();
        private bool isDisplaying = false;
        private Vector2 hiddenPosition;
        private Vector2 visiblePosition;
        private Tweener currentTween;

        public enum SlideDirection
        {
            Up,     // Slides from bottom to top
            Down    // Slides from top to bottom
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializePositions();
            HideImmediate();

            // Subscribe to tooltip events
            SubscribeToEvents();
        }

        private void InitializePositions()
        {
            visiblePosition = tooltipContainer.anchoredPosition;

            if (slideDirection == SlideDirection.Down)
            {
                // Hidden above screen
                hiddenPosition = new Vector2(visiblePosition.x, visiblePosition.y + slideDistance);
            }
            else
            {
                // Hidden below screen
                hiddenPosition = new Vector2(visiblePosition.x, visiblePosition.y - slideDistance);
            }
        }

        private void SubscribeToEvents()
        {
            // Subscribe to tooltip message events from event bus
            TooltipEventBus.Subscribe(OnTooltipMessageReceived);
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            TooltipEventBus.Unsubscribe(OnTooltipMessageReceived);

            currentTween?.Kill();

            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void OnTooltipMessageReceived(TooltipMessage message)
        {
            ShowMessage(message);
        }

        /// <summary>
        /// Show a tooltip message (can be called directly or via event bus)
        /// </summary>
        public void ShowMessage(TooltipMessage message)
        {
            messageQueue.Enqueue(message);

            if (!isDisplaying)
            {
                ProcessNextMessage().Forget();
            }
        }

        /// <summary>
        /// Show a simple error message
        /// </summary>
        public void ShowError(string message)
        {
            ShowMessage(TooltipMessage.Error(message));
        }

        /// <summary>
        /// Show a simple warning message
        /// </summary>
        public void ShowWarning(string message)
        {
            ShowMessage(TooltipMessage.Warning(message));
        }

        /// <summary>
        /// Show a simple info message
        /// </summary>
        public void ShowInfo(string message)
        {
            ShowMessage(TooltipMessage.Info(message));
        }

        /// <summary>
        /// Show a simple success message
        /// </summary>
        public void ShowSuccess(string message)
        {
            ShowMessage(TooltipMessage.Success(message));
        }

        private async UniTaskVoid ProcessNextMessage()
        {
            if (messageQueue.Count == 0)
            {
                isDisplaying = false;
                return;
            }

            isDisplaying = true;
            TooltipMessage message = messageQueue.Dequeue();

            // Setup UI
            SetupTooltipDisplay(message);

            // Slide in
            await SlideIn();

            // Calculate display duration
            float duration = message.Duration > 0
                ? Mathf.Clamp(message.Duration, minDisplayDuration, maxDisplayDuration)
                : defaultDisplayDuration;

            // Wait for display duration
            await UniTask.Delay((int)(duration * 1000));

            // Slide out
            await SlideOut();

            // Process next message if any
            ProcessNextMessage().Forget();
        }

        private void SetupTooltipDisplay(TooltipMessage message)
        {
            // Set message text
            if (useTextTypewriter)
            {
                AnimateTextTypewriter(message.Message).Forget();
            }
            else
            {
                messageText.text = message.Message;
            }

            // Set background color based on type
            backgroundImage.color = GetColorForType(message.Type);

            // Set icon if provided
            if (iconImage != null)
            {
                if (message.Icon != null)
                {
                    iconImage.sprite = message.Icon;
                    iconImage.gameObject.SetActive(true);

                    if (useIconPulse)
                    {
                        PulseIcon();
                    }
                }
                else
                {
                    iconImage.gameObject.SetActive(false);
                }
            }
        }

        private Color GetColorForType(TooltipType type)
        {
            return type switch
            {
                TooltipType.Error => errorColor,
                TooltipType.Warning => warningColor,
                TooltipType.Info => infoColor,
                TooltipType.Success => successColor,
                _ => infoColor
            };
        }

        private async UniTask SlideIn()
        {
            currentTween?.Kill();

            tooltipContainer.anchoredPosition = hiddenPosition;
            tooltipContainer.gameObject.SetActive(true);

            currentTween = tooltipContainer.DOAnchorPos(visiblePosition, slideDuration)
                .SetEase(slideInEase)
                .SetUpdate(true); // Use unscaled time

            await currentTween.AsyncWaitForCompletion();
        }

        private async UniTask SlideOut()
        {
            currentTween?.Kill();

            currentTween = tooltipContainer.DOAnchorPos(hiddenPosition, slideDuration)
                .SetEase(slideOutEase)
                .SetUpdate(true);

            await currentTween.AsyncWaitForCompletion();

            HideImmediate();
        }

        private void HideImmediate()
        {
            tooltipContainer.anchoredPosition = hiddenPosition;
            tooltipContainer.gameObject.SetActive(false);
        }

        private async UniTaskVoid AnimateTextTypewriter(string fullText)
        {
            messageText.text = "";

            foreach (char c in fullText)
            {
                messageText.text += c;
                await UniTask.Delay((int)(typewriterSpeed * 1000));
            }
        }

        private void PulseIcon()
        {
            if (iconImage == null) return;

            iconImage.transform.localScale = Vector3.zero;
            iconImage.transform.DOScale(1f, 0.3f)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);
        }

        /// <summary>
        /// Clear all queued messages and hide current tooltip
        /// </summary>
        public void ClearAll()
        {
            messageQueue.Clear();
            currentTween?.Kill();
            HideImmediate();
            isDisplaying = false;
        }

        #region Editor Testing
        [ContextMenu("Test Error Message")]
        private void TestError()
        {
            ShowError("Not enough coins to purchase this item!");
        }

        [ContextMenu("Test Warning Message")]
        private void TestWarning()
        {
            ShowWarning("Your drives are running low!");
        }

        [ContextMenu("Test Info Message")]
        private void TestInfo()
        {
            ShowInfo("New wave incoming in 5 seconds");
        }

        [ContextMenu("Test Success Message")]
        private void TestSuccess()
        {
            ShowSuccess("Character deployed successfully!");
        }

        [ContextMenu("Test Multiple Messages")]
        private void TestMultiple()
        {
            ShowError("First error message");
            ShowWarning("Second warning message");
            ShowSuccess("Third success message");
        }
        #endregion
    }
}
