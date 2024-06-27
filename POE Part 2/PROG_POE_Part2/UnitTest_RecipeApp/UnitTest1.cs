using System;
using System.Collections.Generic;

public class RecipeController
{
    private Recipe[] recipes;
    private int numRecipes;

    public delegate void DisplayMethod();

    // Constructor to initialize the RecipeController with a specified capacity
    public RecipeController(int capacity)
    {
        recipes = new Recipe[capacity];
        numRecipes = 0;
    }

    // Method to enter recipe details
    public void EnterRecipe()
    {
        // Check if the storage is full
        if (numRecipes >= recipes.Length)
        {
            Console.WriteLine("Recipe storage is full. Cannot add more recipes.");
            return;
        }

        // Prompt for recipe name
        Console.Write("\nEnter the name for the recipe: ");
        string name = Console.ReadLine();

        // Prompt for number of ingredients
        Console.Write("\nEnter the number of ingredients: ");
        int numIngredients;
        if (!int.TryParse(Console.ReadLine(), out numIngredients) || numIngredients <= 0)
        {
            Console.WriteLine("Invalid number of ingredients. Please enter a positive number.");
            return;
        }

        Ingredient[] ingredients = new Ingredient[numIngredients];

        // Loop to get details for each ingredient
        for (int i = 0; i < numIngredients; i++)
        {
            Console.Write($"Enter ingredient {i + 1} name: ");
            string ingredientName = Console.ReadLine();

            Console.Write($"Enter quantity for {ingredientName}: ");
            double quantity;
            if (!double.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
            {
                Console.WriteLine("Invalid quantity. Please enter a positive number.");
                return;
            }

            Console.Write($"Enter unit of measurement for {ingredientName}: ");
            string unit = Console.ReadLine();

            Console.Write($"Enter number of calories for {ingredientName}: ");
            double calories;
            if (!double.TryParse(Console.ReadLine(), out calories) || calories < 0)
            {
                Console.WriteLine("Invalid number of calories. Please enter a non-negative number.");
                return;
            }

            Console.Write($"Enter food group for {ingredientName}: ");
            string foodGroup = Console.ReadLine();

            ingredients[i] = new Ingredient(ingredientName, quantity, unit, calories, foodGroup);
        }

        // Prompt for number of steps
        Console.Write("\nEnter the number of steps: ");
        int numSteps;
        if (!int.TryParse(Console.ReadLine(), out numSteps) || numSteps <= 0)
        {
            Console.WriteLine("Invalid number of steps. Please enter a positive number.");
            return;
        }

        string[] steps = new string[numSteps];

        // Loop to get details for each step
        for (int i = 0; i < numSteps; i++)
        {
            Console.Write($"Enter step {i + 1} description: ");
            steps[i] = Console.ReadLine();
        }

        // Add the new recipe to the array and increment the count
        recipes[numRecipes] = new Recipe(name, ingredients, steps);
        numRecipes++;

        Console.WriteLine("\nRecipe details entered successfully!");
    }

    // Method to display recipe list using a specified display method
    public void DisplayRecipeList(DisplayMethod displayMethod)
    {
        if (numRecipes == 0)
        {
            Console.WriteLine("No recipes available.");
            return;
        }

        displayMethod();
    }

    // Method to display recipe list alphabetically
    public void DisplayRecipeListAlphabetical()
    {
        Array.Sort(recipes, 0, numRecipes, Comparer<Recipe>.Create((x, y) => x.Name.CompareTo(y.Name)));
        for (int i = 0; i < numRecipes; i++)
        {
            Console.WriteLine($"{i + 1}. {recipes[i].Name}");
        }
    }

    // Method to display recipe list by calories
    public void DisplayRecipeListByCalories()
    {
        Array.Sort(recipes, 0, numRecipes, Comparer<Recipe>.Create((x, y) => x.CalculateTotalCalories().CompareTo(y.CalculateTotalCalories())));
        for (int i = 0; i < numRecipes; i++)
        {
            Console.WriteLine($"{i + 1}. {recipes[i].Name} - {recipes[i].CalculateTotalCalories()} calories");
        }
    }

    // Method to display a specific recipe
    public void DisplayRecipe()
    {
        if (numRecipes == 0)
        {
            Console.WriteLine("No recipes available.");
            return;
        }

        Console.Write("\nEnter the number of the recipe to display: ");
        int recipeNumber;
        if (!int.TryParse(Console.ReadLine(), out recipeNumber) || recipeNumber < 1 || recipeNumber > numRecipes)
        {
            Console.WriteLine("Invalid recipe number. Please enter a number between 1 and " + numRecipes);
            return;
        }

        Recipe recipe = recipes[recipeNumber - 1];
        recipe.Display();
    }

    // Method to scale a specific recipe
    public void ScaleRecipe()
    {
        if (numRecipes == 0)
        {
            Console.WriteLine("No recipes available.");
            return;
        }

        Console.Write("\nEnter the number of the recipe to scale: ");
        int recipeNumber;
        if (!int.TryParse(Console.ReadLine(), out recipeNumber) || recipeNumber < 1 || recipeNumber > numRecipes)
        {
            Console.WriteLine("Invalid recipe number. Please enter a number between 1 and " + numRecipes);
            return;
        }

        Recipe recipe = recipes[recipeNumber - 1];
        recipe.Scale();
    }

    // Method to reset quantities of ingredients in a specific recipe
    public void ResetQuantities()
    {
        if (numRecipes == 0)
        {
            Console.WriteLine("No recipes available.");
            return;
        }

        Console.Write("\nEnter the number of the recipe to reset quantities: ");
        int recipeNumber;
        if (!int.TryParse(Console.ReadLine(), out recipeNumber) || recipeNumber < 1 || recipeNumber > numRecipes)
        {
            Console.WriteLine("Invalid recipe number. Please enter a number between 1 and " + numRecipes);
            return;
        }

        Recipe recipe = recipes[recipeNumber - 1];
        recipe.ResetQuantities();
    }

    // Method to clear all recipe data
    public void ClearData()
    {
        for (int i = 0; i < numRecipes; i++)
        {
            recipes[i] = null;
        }

        numRecipes = 0;

        Console.WriteLine("All data cleared successfully!");
    }
}

class Program
{
    static void Main(string[] args)
    {
        RecipeController recipeController = new RecipeController(10); // Initial capacity can be adjusted as needed

        Console.WriteLine("Welcome to the Recipe Application!");

        // Main loop to handle user input
        while (true)
        {
            Console.WriteLine("\n1. Enter Recipe Details");
            Console.WriteLine("2. Display Recipe List (Alphabetical)");
            Console.WriteLine("3. Display Recipe List (By Calories)");
            Console.WriteLine("4. Display Recipe");
            Console.WriteLine("5. Scale Recipe");
            Console.WriteLine("6. Reset Quantities");
            Console.WriteLine("7. Clear All Data");
            Console.WriteLine("8. Exit");
            Console.Write("Select an option: ");

            int option;
            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Invalid option. Please enter a number.");
                continue;
            }

            try
            {
                switch (option)
                {
                    case 1:
                        recipeController.EnterRecipe();
                        break;
                    case 2:
                        recipeController.DisplayRecipeList(recipeController.DisplayRecipeListAlphabetical);
                        break;
                    case 3:
                        recipeController.DisplayRecipeList(recipeController.DisplayRecipeListByCalories);
                        break;
                    case 4:
                        recipeController.DisplayRecipe();
                        break;
                    case 5:
                        recipeController.ScaleRecipe();
                        break;
                    case 6:
                        recipeController.ResetQuantities();
                        break;
                    case 7:
                        recipeController.ClearData();
                        break;
                    case 8:
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please enter a number between 1 and 8.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}

public class Recipe
{
    public string Name { get; }
    public Ingredient[] Ingredients { get; }
    public string[] Steps { get; }

    // Constructor to initialize a recipe with a name, ingredients, and steps
    public Recipe(string name, Ingredient[] ingredients, string[] steps)
    {
        Name = name;
        Ingredients = ingredients;
        Steps = steps;
    }

    // Method to display the recipe details
    public void Display()
    {
        Console.WriteLine($"\nRecipe: {Name}");

        Console.WriteLine("\nIngredients:");
        foreach (var ingredient in Ingredients)
        {
            Console.WriteLine($"{ingredient.Name}: {ingredient.CurrentQuantity} {ingredient.Unit} ({ingredient.Calories} calories, {ingredient.FoodGroup})");
        }

        double totalCalories = 0;
        foreach (var ingredient in Ingredients)
        {
            totalCalories += ingredient.Calories;
        }
        Console.WriteLine($"\nTotal Calories: {totalCalories}");
        if (totalCalories > 300)
        {
            Console.WriteLine("Warning: Total calories exceed 300!");
        }

        Console.WriteLine("\nSteps:");
        for (int i = 0; i < Steps.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {Steps[i]}");
        }
    }

    // Method to scale the quantities of ingredients in the recipe
    public void Scale()
    {
        Console.Write("\nEnter scale factor (0.5 for half, 2 for double, 3 for triple): ");
        double factor;
        if (!double.TryParse(Console.ReadLine(), out factor) || factor <= 0)
        {
            Console.WriteLine("Invalid scale factor. Please enter a positive number.");
            return;
        }

        foreach (var ingredient in Ingredients)
        {
            ingredient.CurrentQuantity *= factor;
        }

        Console.WriteLine("Recipe scaled successfully!");
    }

    // Method to reset the quantities of ingredients to their original values
    public void ResetQuantities()
    {
        foreach (var ingredient in Ingredients)
        {
            ingredient.ResetQuantity();
        }

        Console.WriteLine("Quantities reset successfully!");
    }

    // Method to calculate the total calories of the recipe
    public double CalculateTotalCalories()
    {
        double totalCalories = 0;
        foreach (var ingredient in Ingredients)
        {
            totalCalories += ingredient.Calories;
        }
        return totalCalories;
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

    // Constructor to initialize an ingredient with its details
    public Ingredient(string name, double quantity, string unit, double calories, string foodGroup)
    {
        Name = name;
        OriginalQuantity = quantity;
        CurrentQuantity = quantity;
        Unit = unit;
        Calories = calories;
        FoodGroup = foodGroup;
    }

    // Method to reset the quantity to the original value
    public void ResetQuantity()
    {
        CurrentQuantity = OriginalQuantity;
    }
}
