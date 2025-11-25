using UnityEngine;

namespace Core.Shared.UI
{
    /// <summary>
    /// Message type for tooltip notifications
    /// </summary>
    public enum TooltipType
    {
        Error,      // Red - Failed transactions, errors
        Warning,    // Yellow - Warnings, cautions
        Info,       // Blue - Information
        Success     // Green - Successful operations
    }

    /// <summary>
    /// Data structure for tooltip messages passed through event bus
    /// </summary>
    public struct TooltipMessage
    {
        public string Message;
        public TooltipType Type;
        public float Duration;      // How long to display (0 = use default)
        public Sprite Icon;         // Optional icon

        public TooltipMessage(string message, TooltipType type = TooltipType.Info, float duration = 0f, Sprite icon = null)
        {
            Message = message;
            Type = type;
            Duration = duration;
            Icon = icon;
        }

        // Convenience factory methods
        public static TooltipMessage Error(string message, float duration = 0f)
            => new TooltipMessage(message, TooltipType.Error, duration);

        public static TooltipMessage Warning(string message, float duration = 0f)
            => new TooltipMessage(message, TooltipType.Warning, duration);

        public static TooltipMessage Info(string message, float duration = 0f)
            => new TooltipMessage(message, TooltipType.Info, duration);

        public static TooltipMessage Success(string message, float duration = 0f)
            => new TooltipMessage(message, TooltipType.Success, duration);
    }
}
