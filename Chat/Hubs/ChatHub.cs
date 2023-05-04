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

        public async Task<bool> JoinChat(UserConnection connection)
        {
            if (!_connections.Any(c => c.Value.ChatId == connection.ChatId && c.Value.UserId == connection.UserId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatId);
                _connections[Context.ConnectionId] = connection;
                return true;
            }
            return false;
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
                    StatusId = 1
                };
                _dbContext.Messages.Add(newMessage);
                await _dbContext.SaveChangesAsync();

                var newMessageInfo = await _dbContext.Messages.Where(m => m.Id == newMessage.Id)
                                                              .Include(m => m.Sender)
                                                              .Include(m => m.Status)
                                                              .FirstOrDefaultAsync();
                var messageDto = _mapper.Map<MessageDto>(newMessageInfo);
                await Clients.Group(connection.ChatId)
                             .SendAsync("ReceiveMessage", messageDto);
            }
        }

        public async Task<IEnumerable<MessageDto>> ReadMessages(string chatId, string userId)
        {
            if (string.IsNullOrEmpty(chatId) || string.IsNullOrEmpty(userId))
                return Enumerable.Empty<MessageDto>();

            var chat = await _dbContext.Chats.Where(c => c.Id == int.Parse(chatId))
                                             .Include(c => c.User1)
                                             .Include(c => c.User2)
                                             .Include(c => c.Messages)
                                             .ThenInclude(m => m.Status)
                                             .FirstOrDefaultAsync();

            if (chat is not null && chat.Messages is not null && chat.Messages.Count > 0)
            {
                foreach (var message in chat.Messages)
                {
                    if (message.SenderId != userId)
                        message.StatusId = 2;
                }
                await _dbContext.SaveChangesAsync();
                var result = _mapper.Map<IEnumerable<MessageDto>>(chat.Messages);
                await Clients.Group(chatId).SendAsync("ReceiveReadMessages", result);
                return result;
            }
            return Enumerable.Empty<MessageDto>();
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
