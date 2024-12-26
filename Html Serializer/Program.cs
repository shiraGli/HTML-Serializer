//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using Html_Serializer;
using System.Text.RegularExpressions;


//קריאה לקבצים שנמצאים בפרויקט
var tags = File.ReadAllText("JSON/HtmlTags.json");
var voidTags = File.ReadAllText("JSON/HtmlVoidTags.json");
Console.ReadLine();
// קריאת HTML מאתר
var html = await Load("https://hebrewbooks.org/beis");
//ננקה את הרווחים המיותרים נעשה ביטוי שיביא לנו את כל סוגי הרווחים
var cleanHtml = new Regex("\\s").Replace(html, "");//תחליף את כל הרווחים ל-""-כלום
//מחלקה שמזהה <text> .-זה אומר כל תו שהוא
var htmlLines=new Regex("<(.*?)>").Split(cleanHtml).Where(s=>s.Length>0);
// הגדרת משתנים עבור עץ ה-HTML
HtmlElement root = new HtmlElement { name = "root" }; // תגית שורש
HtmlElement current = root;
//נרצה לעשות html מסודר זה האוביקט שלנו
var htmlElenent = "<div id=\"my-id\" class=\"my-class-1 my-class-2\"width=\"100%\">text<div>";
//נרצה לסדר את האוביקט במערך מסודר שמכיל את האלמנטים שלנו
var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlElenent);//[^\\s]*? תיקח את כל האיברים שבעולם חןץ מרווח

foreach (var message in HtmlHelper.Instance.AllTags)
{
    Console.WriteLine(message);
}
Console.ReadLine();

foreach (var line in htmlLines)
{
    // פיצול המחרוזת למילה ראשונה ושאר התוכן
    var parts = line.Split(' ', 2);
    var firstWord = parts[0];
    var remaining = parts.Length > 1 ? parts[1] : string.Empty;

    if (firstWord == "html/") // סיום HTML
    {
        break;
    }
    else if (firstWord.StartsWith("/")) // תגית סוגרת
    {
        current = current.parent ?? current; // חזרה לרמה הקודמת בעץ
    }
    else if (HtmlHelper.Instance.AllTags.Contains(firstWord)) // תגית פתיחה
    {
        var newElement = new HtmlElement { name = firstWord,parent = current };

        // עיבוד Attributes
        var parsedAttributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(remaining);
        foreach (Match match in parsedAttributes)
        {
            var key = match.Groups[1].Value;
            var value = match.Groups[2].Value;

            if (key == "id") newElement.id = value;
            else if (key == "class") newElement.classes = value.Split(' ').ToList();
            else newElement.Attributes[key] = value;
        }

        // בדיקת תגית סגירה עצמית
        if (HtmlHelper.Instance.SelfClosingTags.Contains(firstWord) || remaining.EndsWith("/"))
        {
            current.children.Add(newElement); // הוספת הילד
        }
        else
        {
            current.children.Add(newElement); // הוספת הילד
            current = newElement; // העברת המצביע לילד
        }
    }
    else // תוכן פנימי
    {
        current.innerHtml += line.Trim();
    }
}


//קיבלת מחרוזת שמכילה את כל ה-Html
async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

// יצירת עץ HtmlElement לדוגמה
var rootElement = new HtmlElement
{
    name = "div",
    id = "root",
    classes = new List<string> { "container" },
    children = new List<HtmlElement>
            {
                new HtmlElement
                {
                    name = "div",
                    id = "mydiv",
                    classes = new List<string> { "class-name" },
                    children = new List<HtmlElement>
                    {
                        new HtmlElement { name = "span", classes = new List<string> { "class-name" } }
                    }
                },
                 new HtmlElement
                {
                    name = "div",
                    id = "child1",
                    classes = new List<string> { "content" },
                    children = new List<HtmlElement>()
                },
                new HtmlElement
                {
                    name = "span",
                    classes = new List<string> { "highlight" },
                    children = new List<HtmlElement>()
                }
            }
};

// חיפוש אלמנטים בעץ HTML לפי Selector
var result = HtmlQueryService.Query(rootElement, "div#mydiv .class-name");

// הדפסת התוצאות
foreach (var el in result)
{
    Console.WriteLine($"Found Element: {el.name}, Id: {el.id}, Classes: {string.Join(",", el.classes)}");
}
// חיפוש אלמנטים נוספים
var result2 = HtmlQueryService.Query(rootElement, "div#root .content");
// הדפסת התוצאות של השאילתה השנייה
foreach (var element in result2)
{
    Console.WriteLine($"Tag: {element.name}, ID: {element.id}, Classes: {string.Join(", ", element.classes)}");
}
// שימוש ב-Descendants
foreach (var el in root.Descendants())
{
    Console.WriteLine($"Descendant: {el.name}, ID: {el.id}, Classes: {string.Join(", ", el.classes)}");
}

// שימוש ב-Ancestors
var childElement = root.children[0];  // נניח child1
foreach (var ancestor in childElement.Ancestors())
{
    Console.WriteLine($"Ancestor: {ancestor.name}, ID: {ancestor.id}, Classes: {string.Join(", ", ancestor.classes)}");
}

// שימוש ב-Selector
var selector = Selector.FromString("div#root .content");
var matchedElements = root.FindBySelector(selector);
foreach (var matched in matchedElements)
{
    Console.WriteLine($"Matched: {matched.name}, ID: {matched.id}, Classes: {string.Join(", ", matched.classes)}");
}


