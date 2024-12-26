using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class HtmlHelper
    {
        //נמנע את הגישה כל פעם לטעון מחדש אלא רק פעם אחת לכן זה גם יהיה פעולה בונה private
      //נוכל להשתמש בפרןפרטי instance שהוא יחיד ונוצר פעם אחת
        private readonly static HtmlHelper _instance = new HtmlHelper("path_to_all_tags.json", "path_to_self_closing_tags.json");
        public static HtmlHelper Instance => _instance;
        public string[] AllTags { get; set; }
        public string[] SelfClosingTags { get; set; }
        private HtmlHelper(string allTagsFilePath, string selfClosingTagsFilePath)
        {
            // טעינת ההודעות מהקובץ לתוך המערך
            // Load All Tags
            AllTags = LoadTagsFromJson(allTagsFilePath);

            // Load Self-Closing Tags
            SelfClosingTags = LoadTagsFromJson(selfClosingTagsFilePath);

        }
        // Method to load tags from a JSON file
        private string[] LoadTagsFromJson(string filePath)
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath); // Read file content
                return JsonSerializer.Deserialize<string[]>(jsonContent); // Deserialize to string array
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tags from file {filePath}: {ex.Message}");
                return Array.Empty<string>();
            }
        }

        // Example: Print all tags (for debug purposes)
        public void PrintTags()
        {
            Console.WriteLine("All Tags:");
            foreach (var tag in AllTags)
            {
                Console.WriteLine(tag);
            }

            Console.WriteLine("\nSelf-Closing Tags:");
            foreach (var tag in SelfClosingTags)
            {
                Console.WriteLine(tag);
            }
        }
    }
}

