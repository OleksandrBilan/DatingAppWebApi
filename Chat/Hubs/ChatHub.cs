using Microsoft.AspNetCore.SignalR;

namespace DatingApp.Chat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IDictionary<string, UserConnection> _connections;

        public ChatHub(IDictionary<string, UserConnection> connections)
        {
            _connections = connections;
        }

        public async Task JoinChat(UserConnection connection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatId);
            _connections[Context.ConnectionId] = connection;
        }

        public async Task SendMessage(string message)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection connection))
            {
                await Clients.Group(connection.ChatId).SendAsync("ReceiveMessage", connection.UserId, message);
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection connection))
            {
                _connections.Remove(Context.ConnectionId);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
