
// Level 3

// Switch case
// Switch case default send error message when other than P,S,Q
// Error handling
// Removes repeated code
// Refactored code
// Match exact not partial for search 
// Clears console for read-ability


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AddingItemsList
{
    static class Program
    {
        static void Main()
        {
            List<ProductDetails> items = new List<ProductDetails>();
            Menu ui = new Menu();
            ui.RunMenu(items);
        }
    }

    // Switch case menu. Default when other than P, S, Q

    class Menu
    {

        public void RunMenu(List<ProductDetails> items)
        {
            string userInput = "P";

            do
            {

                switch (userInput)
                {
                    case "P":
                        AddProduct(items);
                        Console.Clear();              // Clears console
                        DisplayProductDetails(items); // Show the list if not empty
                        break;
                    case "S":
                        SearchProducts(items);
                        break;
                    case "Q":
                        Console.WriteLine("Exiting the program.");
                        return;
                    default:
                        printLineWithColor("Invalid option. Please try again.", ConsoleColor.DarkRed);
                        break;
                }



                printLineWithColor("To enter a new product - enter: 'P' | To search for a product - enter: 'S' | To quit - enter: 'Q'", ConsoleColor.DarkCyan);

                userInput = Console.ReadLine()?.Trim().ToUpper() ?? "";
                Console.Clear();

            } while (userInput != "Q");
        }

        // Adding products uses New ConsoleColor Method

        private void AddProduct(List<ProductDetails> items)
        {
            while (true)
            {
                printSeparationLine();
                printLineWithColor("To enter a new product - follow the steps | To quit - enter: 'Q'", ConsoleColor.DarkYellow);
                string category = PromptForInput("Enter a Category: ");
                if (category.ToUpper() == "Q") break;

                string product = PromptForInput("Enter a Product Name: ");
                if (product.ToUpper() == "Q") break;

                decimal price = PromptForDecimal("Enter a Price: ", allowZero: false);
                if (price == -1) break;

                items.Add(new ProductDetails(category, product, price));

                printLineWithColor("The product was successfully added!", ConsoleColor.Green);
            }
        }

        private void printLineWithColor(String text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        // Method for removing Repeated code

        private void printSeparationLine()
        {
            printLineWithColor("-----------------------------------------------------------------", ConsoleColor.White);
        }

        // Search uses Exact Matches where product Equal search. Ignores case

        private void SearchProducts(List<ProductDetails> items)
        {
            string searchTerm = PromptForInput("Enter a product name to search: ");
            var exactMatches = items.Where(p => p.Product.Equals(searchTerm, StringComparison.OrdinalIgnoreCase));

            var searchResults = exactMatches.ToList();

            printSeparationLine();
            printLineWithColor($"{"Category",-30}{"Product",-20}{"Price",10}", ConsoleColor.Green);
            foreach (var item in items.OrderBy(p => p.Price)) // Still orders by price
            {
                string category = item.Category.PadRight(30);
                string product = item.Product.PadRight(20);
                string price = item.Price.ToString(item.Price == (int)item.Price ? "0" : "0.00");

                // If searchresult is Match prints in Magenta. Else White

                ConsoleColor color;
                if (searchResults.Contains(item))
                {
                    color = ConsoleColor.Magenta;
                }
                else
                {
                    color = ConsoleColor.White;
                }
                printLineWithColor($"{category}{product}{price,10}", color);

            }
            printSeparationLine();

            if (!searchResults.Any())
            {
                Console.WriteLine("\nNo products found matching your search term.");
            }

        }


        // Prompt for input is now for both Category and Product
        // Prompts are private
        // In order to show both solutions, in the prompt for input it uses recursive method
        // and in the prompt for decimals it uses while(true)

        private string PromptForInput(string message)
        {
            Console.Write(message);
            var input = Console.ReadLine() ?? string.Empty; // It returns the left-hand operand if the value is not null; otherwise, it returns the right-hand operand.

            if (input == string.Empty)
            {
                printLineWithColor("You cannot leave a empty string", ConsoleColor.DarkRed);
                return PromptForInput(message);
            }
            return input;
        }
        private decimal PromptForDecimal(string message, bool allowZero)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();
                if (input.ToUpper() == "Q") return -1;
                if (decimal.TryParse(input, out decimal value) && (allowZero && value == 0) || value > 0)
                {
                    return value;
                }
                printLineWithColor("Invalid input. Please enter a numeric value greater than zero.", ConsoleColor.DarkRed);
            }
        }

        private void DisplayProductDetails(List<ProductDetails> items)
        {
            printSeparationLine();
            printLineWithColor($"{"Category",-30}{"Product",-20}{"Price",10}", ConsoleColor.Green);
            const int categoryWidth = 30;

            foreach (var item in items.OrderBy(p => p.Price))
            {
                string category = item.Category.PadRight(30);
                string product = item.Product.PadRight(20);
                string price = item.Price.ToString(item.Price == (int)item.Price ? "0" : "0.00");
                Console.WriteLine($"{category}{product}{price,10}");
            }

            decimal totalAmount = items.Sum(item => item.Price);
            Console.WriteLine($"\n{" ",-categoryWidth}{"Total amount:      "}{totalAmount,10:N2}");
            printSeparationLine();
        }
    }

    class ProductDetails
    {
        public ProductDetails(string category, string product, decimal price)
        {
            Category = category;
            Product = product;
            Price = price;
        }

        public string Category { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
    }
}


/*

// Level 2


// Update: 
// Now sorts values from low to high using LINQ
// Sums up prices under total amount using LINQ
// Uses return -1 to quit

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;

namespace AddingItemsList
{
    static class Program
    {
        static void Main()
        {
            List<ProductDetails> items = GetItemDetails();
            DisplayProductDetails(items);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static List<ProductDetails> GetItemDetails()
        {
            var items = new List<ProductDetails>();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("To Enter a product details - Follow the steps | Type 'q' when you are finished: ");
            Console.ResetColor();

            while (true)
            {
                string category = PromptForInput("Enter a Category: ");  // you can now quit and display a empty list
                if (category.ToLower() == "q") break;

                string product = PromptForInput("Enter a Product Name: ");
                if (product.ToLower() == "q") break;

                decimal price = PromptForDecimal("Enter a Price: ", allowZero: false);
                if (price == -1) break; 

                items.Add(new ProductDetails(category, product, price));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The product was successfully added!");
                Console.ResetColor();
                Console.WriteLine("-----------------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("To Enter a product details - Follow the steps | Type 'q' when you are finished: ");
                Console.ResetColor();
            }

            return items;
        }

        static string PromptForInput(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine() ?? string.Empty;
            return input;
        }

        static decimal PromptForDecimal(string message, bool allowZero)
        {
            decimal value;
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();
                
                // Quit 

                if (input.ToLower() == "q") return -1; // Return -1 as a signal to quit
                if (decimal.TryParse(input, out value) && (allowZero || value != 0))
                {
                    return value;
                }
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(allowZero ? "Invalid input. Please enter a numeric value." : "Invalid input. Please enter a numeric value greater than zero.");
                Console.ResetColor();
            }
        }

        static void DisplayProductDetails(List<ProductDetails> items)
        {
            const int categoryWidth = 30;
            const int productWidth = 20;
            const int priceWidth = 10; 

            // Sort items by price using LINQ

            var sortedItems = items.OrderBy(item => item.Price).ToList();

            // Display Headers

            Console.WriteLine(new string('-', categoryWidth + productWidth + priceWidth));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"Category",-categoryWidth}{"Product",-productWidth}{"Price",-priceWidth}");
            Console.ResetColor();
            
            // Display Product List

            foreach (var item in sortedItems)
            {
                string category = item.Category.PadRight(categoryWidth);
                string product = item.Product.PadRight(productWidth);
                string priceFormat = item.Price == Math.Floor(item.Price) ? "0" : "0.##";
                string price = item.Price.ToString(priceFormat).PadRight(priceWidth);
                Console.WriteLine($"{category}{product}{price}");
            }

            // Display Total Amount

            var totalPrice = sortedItems.Sum(item => item.Price);
            string totalAmountLabel = "Total amount:";
            string totalAmountFormat = totalPrice == Math.Floor(totalPrice) ? "0" : "0.##";
            string totalAmount = totalPrice.ToString(totalAmountFormat).PadLeft(priceWidth-1);
            Console.WriteLine();
            Console.WriteLine($"{" ",-categoryWidth}{totalAmountLabel}{" ",2}{totalAmount}");
            Console.WriteLine(new string('-', categoryWidth + productWidth + priceWidth));
        }
    }

    class ProductDetails
    {
        public ProductDetails(string category, string product, decimal price)
        {
            Category = category;
            Product = product;
            Price = price;
        }

        public string Category { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
    }
}

*/

/*


// Level 1 



// 1:  Recieve a List of Products from user in GetItemDetails Method

// 2:  Type q in "Price" Prompt 
//     to display a list of the products

/*

using System;
using System.Collections.Generic;

// Entry Point

namespace AddingItemsList
{
    static class Program
    {
        static void Main()
        {
            List<ProductDetails> items = GetItemDetails();
            DisplayProductDetails(items);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        // Recieve Item Details

        static List<ProductDetails> GetItemDetails()
        {
            List<ProductDetails> items = new List<ProductDetails>();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("To enter a new product - follow the steps | To quit - enter: \"Q\"");
                Console.ResetColor();

                string category = PromptForInput("Enter a Category: "); // Type "q" to break the while-loop
                if (category.ToLower() == "q") break;

                string product = PromptForInput("Enter a Product Name: ");
                decimal price = PromptForDecimal("Enter a Price: ", allowZero: false);  // Numerical input may not be zero

                items.Add(new ProductDetails(category, product, price));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The product was successfully added!");
                Console.ResetColor();
                Console.WriteLine("-----------------------------------------------------------------");
            }

            return items;
        }

        // Prompt for String

        static string PromptForInput(string message)
        {
            Console.Write(message);
            return Console.ReadLine() ?? string.Empty; // Prevent null input for strings
        }

        // Prompt for Decimal

        static decimal PromptForDecimal(string message, bool allowZero = true)
        {
            decimal value;
            while (true)
            {
                Console.Write(message);
                if (decimal.TryParse(Console.ReadLine(), out value) && (allowZero || value != 0)) // If we can parse and value is not zero then return value
                {
                    return value;
                }

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(allowZero ? "Invalid input. Please enter a numeric value." : "Invalid input. Please enter a numeric value greater than zero.");
            }
        }

        // Display List

        static void DisplayProductDetails(List<ProductDetails> items)
        {

            // Width for categories
            const int categoryWidth = 20;  
            const int productWidth = 20;
            const int priceWidth = 15;



            // Divider

            Console.WriteLine();


            // Categories
            Console.WriteLine("-----------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"Category",-categoryWidth} {"Product",-productWidth} {"Price",-priceWidth}");
            Console.ResetColor();


            foreach (var item in items)
            {
                string category = item.Category.Length > categoryWidth ? item.Category.Substring(0, categoryWidth - 3) + "..." : item.Category;
                string product = item.Product.Length > productWidth ? item.Product.Substring(0, productWidth - 3) + "..." : item.Product;


                // Ensuring the price is formatted and aligned correctly

                string price = String.Format("{0:C}", item.Price).PadRight(priceWidth); //padding and format

                Console.WriteLine($"{category,-categoryWidth} {product,-productWidth} {price,-priceWidth}");
            }
        }
    }

    // Instantiate Details for Each Product
    // Getters and Setters

    class ProductDetails
    {
        public ProductDetails(string category, string product, decimal price)
        {
            Category = category;
            Product = product;
            Price = price;
        }

        public string Category { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
    }
}




/* First version. Bad

using System;
using System.Collections.Generic;

namespace AddingItemsList
{
    class Program
    {
        static void Main()
        {
            List<ProductDetails> items = GetItemDetails(); // Calls method using the list as argument

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        
        static List<ProductDetails> GetItemDetails()
        {
            List<ProductDetails> items = new List<ProductDetails>(); 
            string input;

            do
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\To enter a new product - follow the steps | To quit - enter: \"Q\""); //starting statement
                Console.ResetColor();

                Console.Write("\nEnter a Category: "); 
                input = Console.ReadLine();
                if (input?.ToLower() == "q") break;  

                Console.Write("Enter a Product Name: "); 
                string product = Console.ReadLine();

                Console.Write("Enter a Price: "); // Default color for input prompt
                if (!decimal.TryParse(Console.ReadLine(), out decimal price)) // Attempts to parse for a numeric value. If input is not numeric returns false
                {
                    Console.ForegroundColor = ConsoleColor.Red; // Red color for error message
                    Console.WriteLine("Invalid input. Please enter a numeric value for the price.");
                    Console.ResetColor(); // Reset to default color
                    continue;
                }

                items.Add(new ProductDetails(input, product, price));
                Console.ForegroundColor = ConsoleColor.DarkGreen; // Dark green color for success message
                Console.WriteLine("The product was successfully added!");
                Console.ResetColor();
                Console.WriteLine("--------------------------------------------------");
                Console.ResetColor(); // Reset to default color

            } while (true);

            return items;
        }
    }

    class ProductDetails
    {
        public ProductDetails(string category, string product, decimal price)
        {
            Category = category;
            Product = product;
            Price = price;
        }

        public string Category { get; set; } // Constructor
        public string Product { get; set; }
        public decimal Price { get; set; }
    }
}

 */


