using AutoMapper;
using DatingApp.DB;
using DatingApp.DB.Models.Chats;
using DatingApp.DTOs.Recommendations;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Chat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IDictionary<string, UserConnection> _connections;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ChatHub(IDictionary<string, UserConnection> connections, AppDbContext dbContext, IMapper mapper)
        {
            _connections = connections;
            _dbContext = dbContext;
            _mapper = mapper;
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
                var newMessage = new Message
                {
                    ChatId = int.Parse(connection.ChatId),
                    SenderId = connection.UserId,
                    Text = message,
                    DateTime = DateTime.Now,
                };
                _dbContext.Messages.Add(newMessage);
                await _dbContext.SaveChangesAsync();

                var newMessageInfo = await _dbContext.Messages.Where(m => m.Id == newMessage.Id).Include(m => m.Sender).FirstOrDefaultAsync();
                var messageDto = _mapper.Map<MessageDto>(newMessageInfo);
                await Clients.Group(connection.ChatId)
                             .SendAsync("ReceiveMessage", messageDto);
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out _))
            {
                _connections.Remove(Context.ConnectionId);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
