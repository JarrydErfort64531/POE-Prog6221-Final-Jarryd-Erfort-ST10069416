using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace RecipeApp
{
    public enum FilterOption
    {
        Ingredient,
        FoodGroup,
        Calories
    }

    class Program
    {
        static void Main(string[] args)
        {
            RecipeManager recipeManager = new RecipeManager();

            while (true)
            {
                Console.WriteLine("Enter command (add/display/filter/exit):");
                string command = Console.ReadLine().ToLower();

                switch (command)
                {
                    case "add":
                        Full_Recipe recipe = new Full_Recipe();

                        Console.WriteLine("Enter the name of the recipe:");
                        string name = Console.ReadLine();
                        recipe.Name = name;

                        Console.WriteLine("Enter the number of ingredients (or 'exit' to quit):");
                        int numIngredients = int.Parse(Console.ReadLine());

                        for (int i = 0; i < numIngredients; i++)
                        {
                            Console.WriteLine($"Enter the name of ingredient {i + 1}:");
                            string ingredientName = Console.ReadLine();

                            Console.WriteLine($"Enter the quantity of {ingredientName}:");
                            double quantity = double.Parse(Console.ReadLine());

                            Console.WriteLine($"Enter the unit of measurement for {ingredientName}:");
                            string unit = Console.ReadLine();

                            Console.WriteLine($"Enter the number of calories for {ingredientName}:");
                            int calories = int.Parse(Console.ReadLine());

                            Console.WriteLine($"Enter the food group for {ingredientName}:");
                            string foodGroup = Console.ReadLine();

                            recipe.AddIngredient(ingredientName, quantity, unit, calories, foodGroup);
                        }

                        Console.WriteLine("Enter the number of steps:");
                        int numSteps = int.Parse(Console.ReadLine());

                        for (int i = 0; i < numSteps; i++)
                        {
                            Console.WriteLine($"Enter step {i + 1}:");
                            string step = Console.ReadLine();

                            recipe.AddStep(step);
                        }

                        recipeManager.AddRecipe(recipe);
                        Console.WriteLine("Recipe added!");
                        break;

                    case "display":
                        List<Full_Recipe> recipes = recipeManager.GetRecipes();

                        if (recipes.Count == 0)
                        {
                            Console.WriteLine("No recipes found!");
                            break;
                        }

                        recipes.Sort((a, b) => string.Compare(a.Name, b.Name));

                        Console.WriteLine("Select a recipe:");
                        for (int i = 0; i < recipes.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {recipes[i].Name}");
                        }

                        int selectedRecipeIndex = int.Parse(Console.ReadLine()) - 1;

                        Full_Recipe selectedRecipe = recipes[selectedRecipeIndex];
                        selectedRecipe.DisplayRecipe();

                        int totalCalories = selectedRecipe.GetTotalCalories();
                        Console.WriteLine($"Total calories: {totalCalories}");

                        if (totalCalories > 300)
                        {
                            Console.WriteLine("Warning: Recipe has high calorie content and may not be suitable for all diets.");
                        }

                        break;

                    case "filter":
                        Console.WriteLine("Choose a filter option:");
                        Console.WriteLine("1. Ingredient name");
                        Console.WriteLine("2. Food group");
                        Console.WriteLine("3. Maximum calories");
                        int filterOption = int.Parse(Console.ReadLine());

                        List<Full_Recipe> filteredRecipes;

                        switch (filterOption)
                        {
                            case 1:
                                Console.WriteLine("Enter ingredient name:");
                                string ingredientName = Console.ReadLine();
                                filteredRecipes = recipeManager.GetFilteredRecipes(FilterOption.Ingredient, ingredientName, 0);
                                break;

                            case 2:
                                Console.WriteLine("Enter food group:");
                                string foodGroup = Console.ReadLine();
                                filteredRecipes = recipeManager.GetFilteredRecipes(FilterOption.FoodGroup, foodGroup, 0);
                                break;

                            case 3:
                                Console.WriteLine("Enter maximum calories:");
                                int maxCalories = int.Parse(Console.ReadLine());
                                filteredRecipes = recipeManager.GetFilteredRecipes(FilterOption.Calories, "", maxCalories);
                                break;

                            default:
                                Console.WriteLine("Invalid filter option!");
                                continue;
                        }

                        if (filteredRecipes.Count == 0)
                        {
                            Console.WriteLine("No recipes found!");
                            break;
                        }

                        filteredRecipes.Sort((a, b) => string.Compare(a.Name, b.Name));

                        Console.WriteLine("Select a recipe:");
                        for (int i = 0; i < filteredRecipes.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {filteredRecipes[i].Name}");
                        }

                        int selectedFilteredRecipeIndex = int.Parse(Console.ReadLine()) - 1;

                        Full_Recipe selectedFilteredRecipe = filteredRecipes[selectedFilteredRecipeIndex];
                        selectedFilteredRecipe.DisplayRecipe();

                        int totalFilteredCalories = selectedFilteredRecipe.GetTotalCalories();
                        Console.WriteLine($"Total calories: {totalFilteredCalories}");

                        if (totalFilteredCalories > 300)
                        {
                            Console.WriteLine("Warning:Recipe has high calorie content and may not be suitable for all diets.");
                        }

                        break;

                    case "exit":
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid command!");
                        break;
                }
            }
        }
    }

    public class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public int Calories { get; set; }
        public string FoodGroup { get; set; }

        public Ingredient(string name, double quantity, string unit, int calories, string foodGroup)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
            Calories = calories;
            FoodGroup = foodGroup;
        }

        public string DisplayIngredient()
        {
            return $"{Quantity} {Unit} {Name}";
        }
    }

    public class Full_Recipe
    {
        public string Name { get; set; }
        private List<Ingredient> ingredients;
        private List<string> steps;

        public Full_Recipe()
        {
            ingredients = new List<Ingredient>();
            steps = new List<string>();
        }

        public void AddIngredient(string name, double quantity, string unit, int calories, string foodGroup)
        {
            Ingredient ingredient = new Ingredient(name, quantity, unit, calories, foodGroup);
            ingredients.Add(ingredient);
        }

        public void AddStep(string step)
        {
            steps.Add(step);
        }

        public void DisplayRecipe()
        {
            Console.WriteLine($"Recipe: {Name}");

            Console.WriteLine("Ingredients:");
            foreach (Ingredient ingredient in ingredients)
            {
                Console.WriteLine($"- {ingredient.DisplayIngredient()}");
            }

            Console.WriteLine("Steps:");
            for (int i = 0; i < steps.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {steps[i]}");
            }
        }

        public int GetTotalCalories()
        {
            int totalCalories = 0;

            foreach (Ingredient ingredient in ingredients)
            {
                totalCalories += ingredient.Calories;
            }

            return totalCalories;
        }

        public List<Ingredient> GetIngredients()
        {
            return ingredients;
        }
    }

    public class RecipeManager
    {
        private List<Full_Recipe> recipes;

        public RecipeManager()
        {
            recipes = new List<Full_Recipe>();
        }

        public void AddRecipe(Full_Recipe recipe)
        {
            recipes.Add(recipe);
        }

        public List<Full_Recipe> GetRecipes()
        {
            return recipes;
        }

        public List<Full_Recipe> GetFilteredRecipes(FilterOption option, string filterValue, int maxCalories)
        {
            List<Full_Recipe> filteredRecipes = new List<Full_Recipe>();

            foreach (Full_Recipe recipe in recipes)
            {
                bool isMatch = false;

                switch (option)
                {
                    case FilterOption.Ingredient:
                        isMatch = recipe.GetIngredients().Any(i => i.Name.ToLower().Contains(filterValue.ToLower()));
                        break;
                    case FilterOption.FoodGroup:
                        isMatch = recipe.GetIngredients().Any(i => i.FoodGroup.ToLower() == filterValue.ToLower());
                        break;
                    case FilterOption.Calories:
                        isMatch = recipe.GetTotalCalories() <= maxCalories;
                        break;
                }

                if (isMatch)
                {
                    filteredRecipes.Add(recipe);
                }
            }

            return filteredRecipes;
        }
    }
}
