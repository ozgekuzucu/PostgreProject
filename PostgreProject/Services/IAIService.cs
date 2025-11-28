namespace PostgreProject.Services
{
	public interface IAIService
	{
		// About yazısı üretimi
		Task<string> GenerateAboutText(string prompt);

		// Product detayı üretimi
		Task<string> GenerateProductDescription(string productName);

		// Chatbot yanıtı
		Task<string> GetChatbotResponse(string userMessage);

		// Tarif önerisi
		Task<string> GenerateRecipe(List<string> ingredients);
	}
}