using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        private readonly IRepository<PrivateMessage> _messageRepo;
        private readonly ILogger<ChatHub> _logger;
        public ChatHub(ILogger<ChatHub> logger, IRepository<PrivateMessage> messageRepo)
        {
            _logger = logger;
            _messageRepo = messageRepo;
        }

        public async Task SendPrivateMessage(string receiverUserId, string message)
        {
            var senderId = Context.UserIdentifier; // must be set in authentication
            var senderRole = Context.User?.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

            // Save to DB
            var privateMessage = new PrivateMessage
            {
                SenderUserId = senderId,
                ReceiverUserId = receiverUserId,
                SenderRole = senderRole,
                MessageText = message,
                SentAt = DateTime.Now
            };
            try 
            {
                await _messageRepo.AddAsync(privateMessage);
                await _messageRepo.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error sending private message");
                Console.WriteLine($"Error sending private message: {ex}");
            }

            // Send only to the specific user
            await Clients.User(receiverUserId).SendAsync("ReceivePrivateMessage", senderId, message);
        }
        public async Task<IEnumerable<PrivateMessage>> GetMessageHistory(string otherUserId, int take = 50)
        {
            var currentUserId = Context.UserIdentifier;

            // Fetch messages where current user is either sender or receiver with the given otherUserId
            var messages = await _messageRepo.FindAsync(m =>
                (m.SenderUserId == currentUserId && m.ReceiverUserId == otherUserId) ||
                (m.SenderUserId == otherUserId && m.ReceiverUserId == currentUserId));

            // Sort by SentAt and limit the number of messages
            var history = messages
                .OrderByDescending(m => m.SentAt)
                .Take(take)
                .OrderBy(m => m.SentAt) // re-order ascending so chat is in correct order
                .ToList();

            return history;
        }
    }
}
