using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class HtmlElement
    {
        public String id { get; set; }
        public String name { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public List<String> classes { get; set; } = new List<string>();
        public String innerHtml { get; set; } = "";
        public HtmlElement parent { get; set; }
        public List<HtmlElement> children { get; set; } = new List<HtmlElement>();

    // פונקציה לריצה על כל הצאצאים
    public IEnumerable<HtmlElement> Descendants()
    {
        var queue = new Queue<HtmlElement>();
        queue.Enqueue(this);  // דחוף את האלמנט הנוכחי לתור

        while (queue.Count > 0)
        {
            var currentElement = queue.Dequeue();  // שלוף את האלמנט הראשון בתור
            yield return currentElement;  // החזר את האלמנט הנוכחי

            // הוסף את כל הילדים של האלמנט הנוכחי לתור
            foreach (var child in currentElement.children)
            {
                queue.Enqueue(child);
            }
        }
    }
    // פונקציה לריצה על כל האבות
    public IEnumerable<HtmlElement> Ancestors()
    {
        var currentElement = this.parent;  // התחל מההורה

        while (currentElement != null)
        {
            yield return currentElement;  // החזר את ההורה הנוכחי
            currentElement = currentElement.parent;  // עבור להורה הבא
        }
    }
    // פונקציה חיפוש לפי סלקטור
    public HashSet<HtmlElement> FindBySelector(Selector selector)
    {
        var result = new HashSet<HtmlElement>();

        // ריצה על כל הצאצאים
        foreach (var element in this.Descendants())
        {
            // בדוק אם האלמנט מתאים לסלקטור
            if (MatchesSelector(element, selector))
            {
                result.Add(element);// הוסף את האלמנט רק אם הוא לא קיים ב-HashSet
            }
        }

        return result.ToHashSet(); // לא חובה, אבל מחזיר HashSet על מנת לשמור על ייחודיות
        }

    // פונקציה לבדוק אם האלמנט מתאים לסלקטור
    private bool MatchesSelector(HtmlElement element, Selector selector)
    {
        // התאמה לפי שם תגית
        if (selector.TagName != null && element.name != selector.TagName)
            return false;

        // התאמה לפי ID
        if (selector.Id != null && element.id != selector.Id)
            return false;

        // התאמה לפי class
        if (selector.Classes != null && !element.classes.Intersect(selector.Classes).Any())   // לדוגמה נניח חיפוש על class אחד
                return false;

        return true;
    }

    }
}
