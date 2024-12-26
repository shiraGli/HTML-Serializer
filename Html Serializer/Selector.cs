using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class Selector 
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector Parse(string query)
        {
            var parts = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Selector root = null;
            Selector current = null;

            foreach (var part in parts)
            {
                var newSelector = new Selector();
                var components = part.Split(new[] { '#', '.' }, StringSplitOptions.RemoveEmptyEntries);
                int tagIndex = 0;

                if (!part.StartsWith("#") && !part.StartsWith("."))
                {
                    newSelector.TagName = components[tagIndex];
                    tagIndex++;
                }

                for (int i = tagIndex; i < components.Length; i++)
                {
                    if (part[part.IndexOf(components[i]) - 1] == '#')
                    {
                        newSelector.Id = components[i];
                    }
                    else if (part[part.IndexOf(components[i]) - 1] == '.')
                    {
                        newSelector.Classes.Add(components[i]);
                    }
                }

                if (root == null)
                {
                    root = newSelector;
                }
                else
                {
                    current.Child = newSelector;
                    newSelector.Parent = current;
                }

                current = newSelector;
            }

            return root;
        }
    
    // פונקציה שממירה מחרוזת לסלקטור
    public static Selector FromString(string selectorString)
    {
        var selector = new Selector();
        var parts = selectorString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        Selector currentSelector = selector;

        foreach (var part in parts)
        {
            var childSelector = new Selector();
            currentSelector.Child = childSelector;
            currentSelector = childSelector;

            if (part.StartsWith("#"))
                childSelector.Id = part.Substring(1);
            else if (part.StartsWith("."))
                childSelector.Classes.Add(part.Substring(1));
            else
                childSelector.TagName = part;
        }

        return selector;
    }
    }
}
