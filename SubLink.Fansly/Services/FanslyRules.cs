using JetBrains.Annotations;
using tech.SubLink.Platforms;
using tech.SubLink.Fansly.FanslyClient.Events;
using System;
using System.Threading.Tasks;

namespace tech.SubLink.Fansly.Services;

[PublicAPI]
public sealed class FanslyRules : IPlatformRules {
    internal Func<ChatMessageEvent, Task>? OnChatMessageEvent;
    internal Func<TipEvent, Task>? OnTipEvent;
    internal Func<GoalUpdatedEvent, Task>? OnGoalUpdatedEvent;

    public void ReactToChatMessage(Func<ChatMessageEvent, Task> with) { OnChatMessageEvent = with; }
    public void ReactToTip(Func<TipEvent, Task> with) { OnTipEvent = with; }
    public void ReactToGoalUpdated(Func<GoalUpdatedEvent, Task> with) { OnGoalUpdatedEvent = with; }
}
