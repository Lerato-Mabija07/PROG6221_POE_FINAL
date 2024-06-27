using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecipeAppWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RecipeController recipeController;
        private List<Ingredient> currentIngredients;
        private List<string> currentSteps;

        public MainWindow()
        {
            InitializeComponent();
            InitializeComponent();
            recipeController = new RecipeController(10);
            currentIngredients = new List<Ingredient>();
            currentSteps = new List<string>();

            // Populate RecipeList initially
            UpdateRecipeList();
        }

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            string name = IngredientName.Text;
            if (double.TryParse(IngredientQuantity.Text, out double quantity) &&
                double.TryParse(IngredientCalories.Text, out double calories))
            {
                string unit = IngredientUnit.Text;
                string foodGroup = IngredientFoodGroup.Text;
                Ingredient ingredient = new Ingredient(name, quantity, unit, calories, foodGroup);
                currentIngredients.Add(ingredient);

                // Clear input fields
                IngredientName.Clear();
                IngredientQuantity.Clear();
                IngredientUnit.Clear();
                IngredientCalories.Clear();
                IngredientFoodGroup.Clear();
            }
            else
            {
                MessageBox.Show("Invalid input for quantity or calories.");
            }
        }

        private void AddStep_Click(object sender, RoutedEventArgs e)
        {
            currentSteps.Add(StepDescription.Text);
            StepDescription.Clear();
        }

        private void SubmitRecipe_Click(object sender, RoutedEventArgs e)
        {
            string name = RecipeName.Text;
            if (string.IsNullOrEmpty(name) || currentIngredients.Count == 0 || currentSteps.Count == 0)
            {
                MessageBox.Show("Please enter all recipe details.");
                return;
            }

            Recipe recipe = new Recipe(name, currentIngredients.ToArray(), currentSteps.ToArray());
            recipeController.AddRecipe(recipe);

            // Clear input fields and lists
            RecipeName.Clear();
            currentIngredients.Clear();
            currentSteps.Clear();
            MessageBox.Show("Recipe added successfully!");

            // Update the RecipeList with all recipes
            UpdateRecipeList();
        }

        private void UpdateRecipeList()
        {
            // Get all recipes from the controller
            var allRecipes = recipeController.GetAllRecipes();

            // Display all recipes in the RecipeList
            RecipeList.ItemsSource = allRecipes.Select(r => r.Name).ToList();
        }


        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            string ingredient = FilterIngredient.Text;
            string foodGroup = FilterFoodGroup.Text;
            if (double.TryParse(FilterCalories.Text, out double maxCalories))
            {
                var filteredRecipes = recipeController.FilterRecipes(ingredient, foodGroup, maxCalories);

                // Update RecipeList in Display Recipes tab
                RecipeList.ItemsSource = filteredRecipes.Select(r => r.Name).ToList();

                // Automatically select the first item in the filtered list
                if (filteredRecipes.Count > 0)
                {
                    RecipeList.SelectedIndex = 0;
                }

                // Update RecipeDetails in Recipe Details tab
                DisplaySelectedRecipeDetails();
            }
            else
            {
                MessageBox.Show("Invalid input for calories.");
            }
        }

        private void DisplaySelectedRecipeDetails()
        {
            if (RecipeList.SelectedItem != null)
            {
                string selectedRecipe = RecipeList.SelectedItem.ToString();
                Recipe recipe = recipeController.GetRecipeByName(selectedRecipe);
                if (recipe != null)
                {
                    RecipeDetails.Text = recipe.GetDetails();
                }
            }
        }

        private void RecipeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecipeList.SelectedItem != null)
            {
                string selectedRecipe = RecipeList.SelectedItem.ToString();
                Recipe recipe = recipeController.GetRecipeByName(selectedRecipe);
                if (recipe != null)
                {
                    RecipeDetails.Text = recipe.GetDetails();
                }
            }
        }

        private void ClearData_Click(object sender, RoutedEventArgs e)
        {
            // Clear all input fields and reset lists
            RecipeName.Clear();
            IngredientName.Clear();
            IngredientQuantity.Clear();
            IngredientUnit.Clear();
            IngredientCalories.Clear();
            IngredientFoodGroup.Clear();
            StepDescription.Clear();
            currentIngredients.Clear();
            currentSteps.Clear();
            RecipeList.ItemsSource = null; // Clear filtered recipe list
            RecipeDetails.Text = ""; // Clear recipe details display
        }

        private void ExitApplication_Click(object sender, RoutedEventArgs e)
        {
            // Close the application
            Application.Current.Shutdown();
        }
    }

    public class RecipeController
    {
        private List<Recipe> recipes;

        public RecipeController(int capacity)
        {
            recipes = new List<Recipe>(capacity);
        }

        public void AddRecipe(Recipe recipe)
        {
            recipes.Add(recipe);
        }

        public List<Recipe> GetAllRecipes()
        {
            return recipes;
        }

        public List<Recipe> FilterRecipes(string ingredient, string foodGroup, double maxCalories)
        {
            return recipes.Where(r =>
                (string.IsNullOrEmpty(ingredient) || r.Ingredients.Any(i => i.Name.Contains(ingredient))) &&
                (string.IsNullOrEmpty(foodGroup) || r.Ingredients.Any(i => i.FoodGroup == foodGroup)) &&
                r.CalculateTotalCalories() <= maxCalories).ToList();
        }

        public Recipe GetRecipeByName(string name)
        {
            return recipes.FirstOrDefault(r => r.Name == name);
        }
    }

    public class Recipe
    {
        public string Name { get; }
        public Ingredient[] Ingredients { get; }
        public string[] Steps { get; }

        public Recipe(string name, Ingredient[] ingredients, string[] steps)
        {
            Name = name;
            Ingredients = ingredients;
            Steps = steps;
        }

        public double CalculateTotalCalories()
        {
            return Ingredients.Sum(i => i.Calories);
        }

        public string GetDetails()
        {
            StringBuilder detailsBuilder = new StringBuilder();

            detailsBuilder.AppendLine($"Recipe: {Name}\n");

            detailsBuilder.AppendLine("Ingredients:");
            foreach (var ingredient in Ingredients)
            {
                detailsBuilder.AppendLine($"{ingredient.Name}: {ingredient.CurrentQuantity} {ingredient.Unit} " +
                                           $"({ingredient.Calories} calories, {ingredient.FoodGroup})");
            }

            detailsBuilder.AppendLine($"\nTotal Calories: {CalculateTotalCalories()}\n");

            detailsBuilder.AppendLine("Steps:");
            for (int i = 0; i < Steps.Length; i++)
            {
                detailsBuilder.AppendLine($"{i + 1}. {Steps[i]}");
            }

            return detailsBuilder.ToString();
        }
    }

    public class Ingredient
    {
        public string Name { get; }
        public double OriginalQuantity { get; }
        public double CurrentQuantity { get; set; }
        public string Unit { get; }
        public double Calories { get; }
        public string FoodGroup { get; }

        public Ingredient(string name, double quantity, string unit, double calories, string foodGroup)
        {
            Name = name;
            OriginalQuantity = quantity;
            CurrentQuantity = quantity;
            Unit = unit;
            Calories = calories;
            FoodGroup = foodGroup;
        }
    }
}