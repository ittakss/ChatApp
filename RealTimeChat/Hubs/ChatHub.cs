using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier ?? Context.ConnectionId;
        await Clients.All.SendAsync("userconnected", userId);  // broadcast user connected
        await base.OnConnectedAsync();
    }


    // Handle user disconnection
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.UserIdentifier;
        await Clients.All.SendAsync("UserDisconnected", userId);  // broadcast user disconnected
        await base.OnDisconnectedAsync(exception);
    }

    // Send a private message to a specific user - not implemented yet
    public async Task SendPrivateMessage(string recipientUserId, string message)
    {
        var senderUserId = Context.UserIdentifier;
        await Clients.User(recipientUserId).SendAsync("ReceivePrivateMessage", senderUserId, message);
    }

    // Join a specific group - not implemented yet
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("Send", $"{Context.UserIdentifier} has joined the group {groupName}.");
    }

    // Notify when a user is typing - not implemented yet
    public async Task Typing(string user, string group)
    {
        await Clients.Group(group).SendAsync("UserTyping", user);
    }
}
