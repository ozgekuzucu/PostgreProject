using Microsoft.AspNetCore.SignalR;
using PostgreProject.Services;
using System;
using System.Threading.Tasks;

namespace PostgreProject.Hubs
{
	public class ChatHub : Hub
	{
		private readonly IAIService _aiService;

		public ChatHub(IAIService aiService)
		{
			_aiService = aiService;
		}

		public async Task SendMessage(string user, string message)
		{
			try
			{
				await Clients.Caller.SendAsync("ReceiveMessage", user, message);

				await Clients.Caller.SendAsync("BotTyping", true);

				var aiResponse = await _aiService.GetChatbotResponse(message);

				await Clients.Caller.SendAsync("BotTyping", false);

				await Clients.Caller.SendAsync("ReceiveMessage", "AI Asistan", aiResponse);
			}
			catch (Exception ex)
			{
				await Clients.Caller.SendAsync("BotTyping", false);
				await Clients.Caller.SendAsync("ReceiveMessage", "Sistem", $"❌ Hata: {ex.Message}");
			}
		}

		public override async Task OnConnectedAsync()
		{
			await Clients.Caller.SendAsync("ReceiveMessage", "Sistem", "Chatbot'a hoş geldiniz! Size nasıl yardımcı olabilirim?", DateTime.Now.ToString("HH:mm"));
			await base.OnConnectedAsync();
		}

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			await base.OnDisconnectedAsync(exception);
		}
	}
}