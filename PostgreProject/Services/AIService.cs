using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PostgreProject.Services
{
	public class AIService : IAIService
	{
		private readonly IConfiguration _configuration;
		private readonly HttpClient _httpClient;

		public AIService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
		{
			_configuration = configuration;
			_httpClient = httpClientFactory.CreateClient();
			_httpClient.Timeout = TimeSpan.FromSeconds(120);
		}

		// About Text
		public async Task<string> GenerateAboutText(string prompt)
		{
			var fullPrompt = $@"Profesyonel bir 'Hakkımızda' metni yaz.
								Konu: {prompt}
								Gereksinimler:
								- Akıcı ve doğal bir Türkçe dilinde yaz
								- 3-4 paragraf
								- Samimi ve profesyonel bir ton
								- Kalite, güven ve lezzet vurgusu olsun

								Hakkımızda:";
			return await GenerateTextAsync(fullPrompt, 400);
		}

		// Product Description
		public async Task<string> GenerateProductDescription(string productName)
		{
			var fullPrompt = $@"{productName} için Türkçe bir ürün açıklaması yaz.
							Kısa, doğal ve etkileyici olsun.
							2-3 cümle.
							Kalite ve lezzeti ön plana çıkar.";
			return await GenerateTextAsync(fullPrompt, 150);
		}

		// Chatbot Response
		public async Task<string> GetChatbotResponse(string userMessage)
		{
			var fullPrompt = $@"Sen bir pastane müşteri hizmetleri asistanısın. Sıcak, samimi ama profesyonel bir şekilde konuş.
								Görevin:
								- Müşterilere yardımcı olmak
								- Ürünler hakkında bilgi vermek
								- Sipariş süreciyle ilgili destek sağlamak
								- Her zaman nazik olmak
								Önemli: Türkçe gramer kurallarına tam uygun, düzgün cümleler kur. Devrik yapılar kullanma.
								Müşteri: {userMessage}

								Yanıtın (sadece yanıtı yaz, başka açıklama yapma):";
			return await GenerateTextAsync(fullPrompt, 200, temperature: 0.8);
		}

		// Recipe Generator
		public async Task<string> GenerateRecipe(List<string> ingredients)
		{
			var ingredientList = string.Join(", ", ingredients);

			var fullPrompt = $@"Şu malzemelerle 3 FARKLI tatlı tarifi öner: {ingredientList}

								Her tarif için TAM olarak şu formatı kullan:

								═══════════════════════════════════════
								 TARİF 1: [Tatlı Adı]
								═══════════════════════════════════════

								 MALZEMELER:
								- [Malzeme 1 ve miktarı]
								- [Malzeme 2 ve miktarı]
								- [Malzeme 3 ve miktarı]
								(Gerekirse ek malzemeler ekle)

								 HAZIRLANIŞI:
								1. [İlk adım detaylı açıklama]
								2. [İkinci adım detaylı açıklama]
								3. [Üçüncü adım detaylı açıklama]
								(Gerekirse ve pişme adımı tamamlanmamışsa daha fazla adım ekle)

								 Hazırlama Süresi: [X dakika]
								 Porsiyon: [Y kişilik]
								 İpucu: [Özel bir ipucu]

								═══════════════════════════════════════
								 TARİF 2: [Farklı Bir Tatlı Adı]
								═══════════════════════════════════════

								 MALZEMELER:
								- [Malzeme listesi]

								 HAZIRLANIŞI:
								1. [Adımlar]

								 Hazırlama Süresi: [X dakika]
								 Porsiyon: [Y kişilik]
								 İpucu: [Özel bir ipucu]

								═══════════════════════════════════════
								 TARİF 3: [Üçüncü Farklı Tatlı Adı]
								═══════════════════════════════════════

								 MALZEMELER:
								- [Malzeme listesi]

								 HAZIRLANIŞI:
								1. [Adımlar]

								 Hazırlama Süresi: [X dakika]
								 Porsiyon: [Y kişilik]
								 İpucu: [Özel bir ipucu]

								ÖNEMLİ KURALLAR:
								- 3 farklı tarif öner (aynı yemek olmasın)
								- Her tarif pratik ve yapılabilir olsun
								- Türkçe gramer kurallarına uy
								- Sadece tarifleri yaz, ekstra açıklama yapma
								- Her tarifi ayırıcı çizgilerle (═══) ayır";

			return await GenerateTextAsync(fullPrompt, 1500, temperature: 0.8);
		}
		// Ortak Text Generation
		private async Task<string> GenerateTextAsync(string prompt, int maxTokens = 300, double temperature = 0.7)
		{
			try
			{
				var apiKey = _configuration["ApiKeys:HuggingFace"];
				if (string.IsNullOrEmpty(apiKey))
					return "API Key bulunamadı. appsettings.json kontrol edin.";

				var url = "https://router.huggingface.co/v1/chat/completions";

				_httpClient.DefaultRequestHeaders.Clear();
				_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
				_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

				var requestBody = new
				{
					model = "meta-llama/Meta-Llama-3-8B-Instruct",
					messages = new[]
					{
						new {
							role = "system",
							content = @"Sen profesyonel bir Türkçe asistansın. 
										Kurallar:
										1. Her zaman doğru Türkçe gramer kullan
										2. Özne-nesne-yüklem sırasına dikkat et
										3. Doğal ve akıcı cümleler kur
										4. Devrik yapılar kullanma
										5. Çeviri yapar gibi konuşma

										Örnek YANLIŞ: 'size ne gibi bir ihtiyacınız var siz?'
										Örnek DOĞRU: 'Size nasıl yardımcı olabilirim?'

										Sadece düzgün Türkçe yanıt ver."
						},
						new { role = "user", content = prompt }
					},
					max_tokens = maxTokens,
					temperature = temperature,
					top_p = 0.9
				};

				var json = JsonConvert.SerializeObject(requestBody);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				Console.WriteLine($"API Request to: {url}");

				var response = await _httpClient.PostAsync(url, content);
				var responseString = await response.Content.ReadAsStringAsync();

				Console.WriteLine($"API Response Status: {response.StatusCode}");
				Console.WriteLine($"API Response: {responseString}");

				if (!response.IsSuccessStatusCode)
				{
					if (response.StatusCode == HttpStatusCode.NotFound)
					{
						return $" API Hatası (404): Model '{requestBody.model}' bulunamadı veya router'da desteklenmiyor.";
					}
					return $" API Hatası ({response.StatusCode}): {responseString}";
				}

				dynamic result = JsonConvert.DeserializeObject(responseString);
				string responseText = result?.choices?[0]?.message?.content?.ToString()?.Trim();

				if (!string.IsNullOrEmpty(responseText))
				{
					responseText = CleanTurkishResponse(responseText);
				}

				return string.IsNullOrEmpty(responseText) ? "⚠️ Boş çıktı döndü." : responseText;
			}
			catch (TaskCanceledException)
			{
				return " İstek zaman aşımına uğradı. Lütfen birkaç saniye sonra tekrar deneyin.";
			}
			catch (Exception ex)
			{
				return $" Hata: {ex.Message}";
			}
		}

		private string CleanTurkishResponse(string text)
		{
			text = System.Text.RegularExpressions.Regex.Replace(text, @"\b(\w+)\b\s+\1\b", "$1");

			text = System.Text.RegularExpressions.Regex.Replace(text, @"\?+", "?");

			text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ");

			return text.Trim();
		}
	}
}