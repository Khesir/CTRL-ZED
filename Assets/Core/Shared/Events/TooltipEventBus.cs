using System;
using UnityEngine;
using Core.Shared.UI;

namespace Core.Shared.Events
{
    /// <summary>
    /// Simple event bus for global tooltip messages.
    /// Can be integrated with existing event system or used standalone.
    /// </summary>
    public static class TooltipEventBus
    {
        public static event Action<TooltipMessage> OnTooltipMessageRequested;

        /// <summary>
        /// Publish a tooltip message to all subscribers
        /// </summary>
        public static void Publish(TooltipMessage message)
        {
            OnTooltipMessageRequested?.Invoke(message);
        }

        /// <summary>
        /// Publish an error tooltip
        /// </summary>
        public static void PublishError(string message)
        {
            Publish(TooltipMessage.Error(message));
        }

        /// <summary>
        /// Publish a warning tooltip
        /// </summary>
        public static void PublishWarning(string message)
        {
            Publish(TooltipMessage.Warning(message));
        }

        /// <summary>
        /// Publish an info tooltip
        /// </summary>
        public static void PublishInfo(string message)
        {
            Publish(TooltipMessage.Info(message));
        }

        /// <summary>
        /// Publish a success tooltip
        /// </summary>
        public static void PublishSuccess(string message)
        {
            Publish(TooltipMessage.Success(message));
        }

        /// <summary>
        /// Subscribe to tooltip messages
        /// </summary>
        public static void Subscribe(Action<TooltipMessage> callback)
        {
            OnTooltipMessageRequested += callback;
        }

        /// <summary>
        /// Unsubscribe from tooltip messages
        /// </summary>
        public static void Unsubscribe(Action<TooltipMessage> callback)
        {
            OnTooltipMessageRequested -= callback;
        }

        /// <summary>
        /// Clear all subscribers (use with caution)
        /// </summary>
        public static void ClearAllSubscribers()
        {
            OnTooltipMessageRequested = null;
        }
    }
}
