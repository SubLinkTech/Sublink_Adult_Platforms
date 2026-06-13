using Serilog;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

#if SUBLINK_FANSLY

logger.Information("[{TAG}] Fansly integration enabled", "Script");
var fansly = (FanslyRules)rules["Fansly"];

fansly.ReactToChatMessage(async chatMessage => {
    if ("yewnyx".Equals(chatMessage.Username, StringComparison.InvariantCultureIgnoreCase)) {
        OscParameter.SendAvatarParameter("JacketToggle", false);
        OscParameter.SendAvatarParameter("Sus", true);
    }

    DateTime timestamp = DateTimeOffset.FromUnixTimeMilliseconds(chatMessage.CreatedAt).DateTime;
    logger.Information(
        "Fansly message received > Username: {UserName}, Displayname: {Displayname}, Created At: {CreatedAt}, Content: {Content}",
        chatMessage.Username, chatMessage.Displayname, timestamp, chatMessage.Content);
});

fansly.ReactToTip(async tipInfo => {
    logger.Information("Fansly tip recieved : ${Amount} from {Displayname} with the following message: {Content}",
        tipInfo.Amount, tipInfo.Displayname, tipInfo.Content);

    OscParameter.SendAvatarParameter("FanslyTip", tipInfo.CentAmount);

    switch (tipInfo.CentAmount) {
        // To compare against tipInfo.Amount instead you have to use "floats", which MUST end in an `f` like: 1.23f
        // tipInfo.CentAmount is an integer, which doesn't support decimals.
        case 1000: {
            OscParameter.SendAvatarParameter("Ragdoll", true);
            break;
        }
        case 1500: {
            OscParameter.SendAvatarParameter("Yeet", true);
            break;
        }
        case 2500: {
            OscParameter.SendAvatarParameter("PopConfettiType", 1);
            break;
        }
        case 3000: {
            OscParameter.SendAvatarParameter("PopConfettiType", 2);
            break;
        }
        default: break;
    }
});

fansly.ReactToGoalUpdated(async goalInfo => {
    logger.Information("Fansly goal updated : `{Label}` is now at {CurrentAmount} of {GoalAmount} (in $-cents)",
        goalInfo.Label, goalInfo.CurrentAmount, goalInfo.GoalAmount);

    if (
        "Hocking Time".Equals(goalInfo.Label, StringComparison.InvariantCultureIgnoreCase) &&
        goalInfo.CurrentAmount >= goalInfo.GoalAmount
    ) {
        OscParameter.SendAvatarParameter("Honk", true);
    }
});

#endif

#if SUBLINK_OPENSHOCK

logger.Information("[{TAG}] OpenShock integration enabled", "Script");
var openShock = (OpenShockRules)rules["OpenShock"];

async void LogOwnShockers() {
    var shockerInfo = await openShock.GetOwnShockers();
    string resultStr = "";

    foreach (var hub in shockerInfo) {
        resultStr += $"Hub `{hub.Name}` ({hub.Id}) has the following shockers:\r\n";

        foreach (var shocker in hub.Shockers) {
            resultStr += $"  - `{shocker.Name}` ({shocker.Id}) state: {(shocker.IsPaused ? "Paused" : "Live")}\r\n";
        }
    }

    logger.Information(resultStr);
}

#endif

#if SUBLINK_JOYSTICK

logger.Information("[{TAG}] Joystick integration enabled", "Script");
var joystick = (JoystickRules)rules["Joystick"];

joystick.ReactToChatMessage(async (eventArgs) => {
    if ("yewnyx".Equals(eventArgs.AuthorSlug, StringComparison.OrdinalIgnoreCase)) {
        OscParameter.SendAvatarParameter("JacketToggle", false);
        OscParameter.SendAvatarParameter("Sus", true);
    }

    if (!string.IsNullOrWhiteSpace(eventArgs.BotCommand)) {
        string[] args = eventArgs.BotCommandArg.Split(' ');

        switch (eventArgs.BotCommand) {
            case "applejuice": {
                joystick.ChatMessage("fixes everything");
                break;
            }
            default: break; // Do nothing C:
        }
    }

    logger.Information(
        "Username: {AuthorUsername}, Slug: {AuthorSlug}, Created At: {CreatedAt}, Text: {Text}",
        eventArgs.AuthorUsername, eventArgs.AuthorSlug, eventArgs.CreatedAt, eventArgs.Text);
});

joystick.ReactToBotMessage(async (eventArgs) => {
    logger.Information(
        "Bot name: {AuthorUsername}, Created At: {CreatedAt}, Text: {Text}",
        eventArgs.AuthorUsername, eventArgs.CreatedAt, eventArgs.Text);
});

joystick.ReactToEnterStream(async (eventArgs) => {
    logger.Information("User `{Who}` entered the chat at {CreatedAt}", eventArgs.Who, eventArgs.CreatedAt);
});

joystick.ReactToLeaveStream(async (eventArgs) => {
    logger.Information("User `{Who}` left the chat at {CreatedAt}", eventArgs.Who, eventArgs.CreatedAt);
});

joystick.ReactToStarted(async (eventArgs) => {
    logger.Information("Stream started at {CreatedAt}", eventArgs.CreatedAt);
});

joystick.ReactToStreamResuming(async (eventArgs) => {
    logger.Information("Stream resumed at {CreatedAt}", eventArgs.CreatedAt);
});

joystick.ReactToStreamEnding(async (eventArgs) => {
    logger.Information("Stream ending at {CreatedAt}", eventArgs.CreatedAt);
});

joystick.ReactToEnded(async (eventArgs) => {
    logger.Information("Stream ended at {CreatedAt}", eventArgs.CreatedAt);
});

joystick.ReactToFollowed(async (eventArgs) => {
    OscParameter.SendAvatarParameter("NewFollower", true);
    logger.Information("`{Who}` followed the stream at {CreatedAt}", eventArgs.Who, eventArgs.CreatedAt);
});

joystick.ReactToFollowerCountUpdated(async (eventArgs) => {
    logger.Information(
        "Follower count updated to {NumberOfFollowers} at {CreatedAt}",
        eventArgs.NumberOfFollowers, eventArgs.CreatedAt);
});

joystick.ReactToTipped(async (eventArgs) => {
    logger.Information(
        "`{Username}` tipped `{TipMenuItem}` worth {Amount} tokens",
        eventArgs.Who, eventArgs.TipMenuItem, eventArgs.Amount);
    OscParameter.SendAvatarParameter("JoystickTip", eventArgs.Amount);
});

joystick.ReactToTipGoalCreated(async (eventArgs) => {
    logger.Information(
        "Created new goal `{Title}` for {Amount} tokens at {CreatedAt}",
        eventArgs.Title, eventArgs.Amount, eventArgs.CreatedAt);
});

joystick.ReactToTipGoalDeleted(async (eventArgs) => {
    logger.Information(
        "Deleted goal `{Title}` for {Amount} tokens at {CreatedAt}",
        eventArgs.Title, eventArgs.Amount, eventArgs.CreatedAt);
});

joystick.ReactToTipGoalIncreased(async (eventArgs) => {
    logger.Information(
        "User `{ByUser}` increased the tip goals with {Amount} tokens at {CreatedAt}. Current: {Current}, Previous: {Previous}",
        eventArgs.ByUser, eventArgs.Amount, eventArgs.CreatedAt, eventArgs.Current, eventArgs.Previous);
});

joystick.ReactToTipGoalMet(async (eventArgs) => {
    logger.Information(
        "User `{Who}` caused goal `{Title}` to be met with {Amount} tokens at {CreatedAt}",
        eventArgs.Who, eventArgs.Title, eventArgs.Amount, eventArgs.CreatedAt);
});

joystick.ReactToTipGoalUpdated(async (eventArgs) => {
    logger.Information(
        "Updated goal `{Title}` with {Amount} tokens at {CreatedAt}",
        eventArgs.Title, eventArgs.Amount, eventArgs.CreatedAt);
});

joystick.ReactToTipMenuItemLocked(async (eventArgs) => {
    logger.Information(
        "Tip menu item `{Title}` worth {Amount} tokens locked at {CreatedAt}",
        eventArgs.Title, eventArgs.Amount, eventArgs.CreatedAt);
});

joystick.ReactToTipMenuItemUnlocked(async (eventArgs) => {
    logger.Information(
        "Tip menu item `{Title}` worth {Amount} tokens unlocked at {CreatedAt}",
        eventArgs.Title, eventArgs.Amount, eventArgs.CreatedAt);
});

joystick.ReactToChatTimerStarted(async (eventArgs) => {
    logger.Information(
        "New chat timer `{Name}` started, it ends at {EndsAt}",
        eventArgs.Name, eventArgs.EndsAt);
});

joystick.ReactToChatTimersCleared(async (eventArgs) => {
    logger.Information("Chat timers have been cleared by `{Who}`", eventArgs.Who);
});

joystick.ReactToDropinStream(async (eventArgs) => {
    logger.Information(
        "Outgoing drop-in started, targeting `{Destination}` with {NumberOfViewers} viewers at {CreatedAt}",
        eventArgs.Destination, eventArgs.NumberOfViewers, eventArgs.CreatedAt);
});

joystick.ReactToStreamDroppedIn(async (eventArgs) => {
    logger.Information(
        "Incoming drop-in from `{Who}` with {NumberOfViewers} viewers at {CreatedAt}",
        eventArgs.Who, eventArgs.NumberOfViewers, eventArgs.CreatedAt);
});

joystick.ReactToSubscribed(async (eventArgs) => {
    logger.Information("`{Who}` subscribed for {Amount} months", eventArgs.Who, eventArgs.Amount);
    OscParameter.SendAvatarParameter("JoystickSubscription", eventArgs.Amount);
});

joystick.ReactToResubscribed(async (eventArgs) => {
    logger.Information(
        "`{Who}` resubscribed for {Amount} months, total sub length {Length}",
        eventArgs.Who, eventArgs.Amount, eventArgs.Length);
    OscParameter.SendAvatarParameter("JoystickResub", eventArgs.Length);
});

joystick.ReactToGiftedSubscriptions(async (eventArgs) => {
    logger.Information("`{Who}` gifted {Amount} subscriptions", eventArgs.Who, eventArgs.Amount);
    OscParameter.SendAvatarParameter("JoystickGifts", eventArgs.Amount);
});

joystick.ReactToWheelSpinClaimed(async (eventArgs) => {
    logger.Information(
        "`{Who}` spun the wheel and got {Amount} x `{Prize}`",
        eventArgs.Who, eventArgs.Amount, eventArgs.Prize);
});

joystick.ReactToViewerCountUpdated(async (eventArgs) => {
    logger.Information("Viewer count updated to {Amount}", eventArgs.Amount);
});

joystick.ReactToSubscriberCountUpdated(async (eventArgs) => {
    logger.Information("Subscriber count updated to {Amount}", eventArgs.Amount);
});

joystick.ReactToMilestoneCompleted(async (eventArgs) => {
    logger.Information("Milestone `{Title}` completed with value {Amount}", eventArgs.Title, eventArgs.Amount);
});

joystick.ReactToPvpSessionRequested(async (eventArgs) => {
    logger.Information(
        "New PVP session request by `{Who}` with cost {Cost} on {When}",
        eventArgs.Who, eventArgs.Cost, eventArgs.When);
});

joystick.ReactToPvpSessionReady(async (eventArgs) => {
    logger.Information(
        "PVP session ready for `{Who}` with cost {Cost} on {When}",
        eventArgs.Who, eventArgs.Cost, eventArgs.When);
});

joystick.ReactToPvpSessionStarted(async (eventArgs) => {
    logger.Information("PVP session started for `{Who}` on {When}", eventArgs.Who, eventArgs.When);
});

joystick.ReactToPvpSessionEnding(async (eventArgs) => {
    logger.Information("PVP session ending for `{Who}` on {When}", eventArgs.Who, eventArgs.When);
});

joystick.ReactToPvpSessionEnded(async (eventArgs) => {
    logger.Information("PVP session ended for `{Who}` on {When}", eventArgs.Who, eventArgs.When);
});

joystick.ReactToSceneUpdated(async (eventArgs) => {
    logger.Information(
        "Scene item `{Name}` updated. [ Font Size: {FontSize}, Title Color: {TitleColor}, Progress Color: {ProgressColor}, Completed Color: {CompletedColor} ]",
        eventArgs.Name, eventArgs.FontSize, eventArgs.TitleColor, eventArgs.ProgressColor, eventArgs.CompletedColor);
});

joystick.ReactToSettingsUpdated(async (eventArgs) => {
    logger.Information("Settings were updated at {CreatedAt}", eventArgs.CreatedAt);
});

joystick.ReactToStreamModeUpdated(async (eventArgs) => {
    logger.Information("Stream mode updated by `{Who}` at {CreatedAt}", eventArgs.Who, eventArgs.CreatedAt);
});

joystick.ReactToUserMuted(async (eventArgs) => {
    logger.Information("User `{Who}` has been muted at {CreatedAt}", eventArgs.Who, eventArgs.CreatedAt);
});

joystick.ReactToUserUnmuted(async (eventArgs) => {
    logger.Information("User `{Who}` has been unmuted at {CreatedAt}", eventArgs.Who, eventArgs.CreatedAt);
});

joystick.ReactToDeviceConnected(async (eventArgs) => {
    logger.Information("Device connected at {CreatedAt}", eventArgs.CreatedAt);
});

joystick.ReactToDeviceDisconnected(async (eventArgs) => {
    logger.Information("Device disconnected at {CreatedAt}", eventArgs.CreatedAt);
});

joystick.ReactToDeviceSettingsUpdated(async (eventArgs) => {
    logger.Information("Device settings were updated at {CreatedAt}", eventArgs.CreatedAt);
});

#endif
