namespace World_Countries;

public static class Utilities
{
    public static void DisplayMenu(string title, string[] options, string exitText = "Exit Program")
    {
        Console.WriteLine("\n\n");
        Console.WriteLine($"  {title}  ");
        Console.WriteLine(new string('=', title.Length + 2));

        if (options.Length > 10)
        {
            // Two-column layout
            for (int i = 0; i < options.Length; i += 2)
            {
                Console.Write($"{i + 1}. {options[i].PadRight(30)} ");
                if ((i + 1) < options.Length)
                    Console.WriteLine($"\t{i + 2}. {options[i + 1]} ");
                else
                    Console.WriteLine();
            }
        }
        else
        {
            // Single-column layout
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]} ");
            }
        }

        Console.WriteLine($"0. {exitText} ");
    }


}

