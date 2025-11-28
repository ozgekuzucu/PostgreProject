using Microsoft.AspNetCore.Mvc;
using PostgreProject.Services;
using System.Text.RegularExpressions;

namespace PostgreProject.Controllers
{
	public class RecipeController : Controller
	{
		private readonly IAIService _aiService;
		private readonly ILogger<RecipeController> _logger;

		public RecipeController(IAIService aiService, ILogger<RecipeController> logger)
		{
			_aiService = aiService;
			_logger = logger;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> GenerateRecipe([FromBody] List<string> ingredients)
		{
			if (ingredients == null || !ingredients.Any())
			{
				return Json(new { success = false, message = "Lütfen en az bir malzeme girin." });
			}

			var fullRecipe = await _aiService.GenerateRecipe(ingredients);

			_logger.LogInformation("Full Recipe Response: {Recipe}", fullRecipe.Substring(0, Math.Min(500, fullRecipe.Length)));

			var recipes = ParseRecipes(fullRecipe);

			_logger.LogInformation("Parsed {Count} recipes", recipes.Count);

			return Json(new
			{
				success = true,
				recipes = recipes
			});
		}

		private List<RecipeDto> ParseRecipes(string fullRecipe)
		{
			var recipes = new List<RecipeDto>();

			try
			{
				// Gereksiz karakterleri temizle
				fullRecipe = fullRecipe
					.Replace("???", "")
					.Replace("??", "")
					.Replace("?????", "");

				// Ayırıcı çizgilere göre böl
				var parts = fullRecipe.Split(new[] { "═══════════════════════════════════════" }, StringSplitOptions.RemoveEmptyEntries);

				_logger.LogInformation("Split into {Count} parts", parts.Length);

				int recipeNum = 1;
				foreach (var part in parts)
				{
					var trimmed = part.Trim();
					if (trimmed.Length < 50) continue;

					if (!trimmed.Contains("MALZEMELER", StringComparison.OrdinalIgnoreCase))
					{
						_logger.LogWarning("Skipping part without MALZEMELER");
						continue;
					}

					var recipe = new RecipeDto();

					_logger.LogInformation("=== Processing Recipe {Num} ===", recipeNum);

					//Başlık
					var titlePattern = @"TAR[İI]F\s+\d+\s*:\s*(.+?)(?:\n|$)";
					var titleMatch = Regex.Match(trimmed, titlePattern, RegexOptions.IgnoreCase);

					recipe.Title = titleMatch.Success
						? titleMatch.Groups[1].Value.Trim()
						: $"Tarif {recipeNum}";

					_logger.LogInformation("Title: '{Title}'", recipe.Title);

					// Malzemeler - Her iki formatı da destekle
					var malzemelerIndex = trimmed.IndexOf("MALZEMELER", StringComparison.OrdinalIgnoreCase);
					var hazirlanisIndex = trimmed.IndexOf("HAZIRLAN", StringComparison.OrdinalIgnoreCase);

					if (malzemelerIndex >= 0 && hazirlanisIndex > malzemelerIndex)
					{
						var malzemelerSection = trimmed.Substring(malzemelerIndex, hazirlanisIndex - malzemelerIndex);
						_logger.LogInformation("Malzemeler section: {Section}",
							malzemelerSection.Substring(0, Math.Min(200, malzemelerSection.Length)));

						var ingredientPattern = @"-\s*([^-\r\n]+?)(?=\s*-|$|\r|\n)";
						var ingredientMatches = Regex.Matches(malzemelerSection, ingredientPattern);

						_logger.LogInformation("Found {Count} ingredient matches", ingredientMatches.Count);

						foreach (Match match in ingredientMatches)
						{
							var ingredient = match.Groups[1].Value.Trim();

							// Geçersiz içerikleri filtrele
							var invalidKeywords = new[] { "MALZEMELER", "HAZIRLAN", "TARİF", "===", "???", "??" };
							bool isValid = !string.IsNullOrWhiteSpace(ingredient)
								&& ingredient.Length > 2
								&& !invalidKeywords.Any(k => ingredient.Contains(k, StringComparison.OrdinalIgnoreCase));

							if (isValid)
							{
								recipe.Ingredients.Add(ingredient);
								_logger.LogInformation("✅ Added ingredient: {Ing}", ingredient);
							}
							else
							{
								_logger.LogWarning("❌ Rejected: '{Ing}' (len={Len})", ingredient, ingredient.Length);
							}
						}

						_logger.LogInformation("Total ingredients parsed: {Count}", recipe.Ingredients.Count);
					}

					// Hazırlanışı
					if (hazirlanisIndex >= 0)
					{
						var hazirlanisEndIndex = trimmed.IndexOf("Hazırlama Süresi", hazirlanisIndex, StringComparison.OrdinalIgnoreCase);
						if (hazirlanisEndIndex < 0)
							hazirlanisEndIndex = trimmed.IndexOf("Porsiyon", hazirlanisIndex, StringComparison.OrdinalIgnoreCase);
						if (hazirlanisEndIndex < 0)
							hazirlanisEndIndex = trimmed.Length;

						var hazirlanisSection = trimmed.Substring(hazirlanisIndex, hazirlanisEndIndex - hazirlanisIndex);

						// Numaralı adımları bul (1. 2. 3. vb)
						var stepPattern = @"(\d+)\.\s+(.+?)(?=(?:\d+\.)|$)";
						var stepMatches = Regex.Matches(hazirlanisSection, stepPattern, RegexOptions.Singleline);

						foreach (Match match in stepMatches)
						{
							var stepText = match.Groups[2].Value
								.Trim()
								.Replace("\n", " ")
								.Replace("\r", " ");

							stepText = Regex.Replace(stepText, @"\s+", " ");

							if (!string.IsNullOrWhiteSpace(stepText) && stepText.Length > 5)
							{
								recipe.Instructions.Add(stepText);
								_logger.LogInformation("✅ Added instruction: {Inst}",
									stepText.Substring(0, Math.Min(60, stepText.Length)));
							}
						}

						_logger.LogInformation("Total instructions parsed: {Count}", recipe.Instructions.Count);
					}

					// Recipe'yi ekle
					if (recipe.Ingredients.Count > 0 || recipe.Instructions.Count > 0)
					{
						recipes.Add(recipe);
						_logger.LogInformation("✅ Added recipe: {Title} | Ingredients: {IngCount} | Instructions: {InstCount}",
							recipe.Title, recipe.Ingredients.Count, recipe.Instructions.Count);
					}
					else
					{
						_logger.LogWarning("❌ Skipped recipe: {Title} (no content)", recipe.Title);
					}

					recipeNum++;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error parsing recipes");
			}

			return recipes.Take(3).ToList();
		}
	}

	public class RecipeDto
	{
		public string Title { get; set; }
		public List<string> Ingredients { get; set; } = new List<string>();
		public List<string> Instructions { get; set; } = new List<string>();
	}
}