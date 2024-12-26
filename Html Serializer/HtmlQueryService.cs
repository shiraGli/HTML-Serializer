using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    internal class HtmlQueryService
    {
            public static List<HtmlElement> Query(HtmlElement root, string selector)
            {
                var selectors = selector.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var currentLevel = new List<HtmlElement> { root };

                foreach (var sel in selectors)
                {
                    currentLevel = ApplySelector(currentLevel, sel);
                }

                return currentLevel;
            }

            private static List<HtmlElement> ApplySelector(List<HtmlElement> elements, string selector)
            {
                var result = new List<HtmlElement>();

                foreach (var element in elements)
                {
                    if (selector.StartsWith("#")) // ID Selector
                    {
                        var id = selector.Substring(1);
                        result.AddRange(FindById(element, id));
                    }
                    else if (selector.StartsWith(".")) // Class Selector
                    {
                        var className = selector.Substring(1);
                        result.AddRange(FindByClass(element, className));
                    }
                    else // Tag Name Selector
                    {
                        result.AddRange(FindByTagName(element, selector));
                    }
                }

                return result;
            }

            private static List<HtmlElement> FindById(HtmlElement element, string id)
            {
                var result = new List<HtmlElement>();
                if (element.id == id)
                {
                    result.Add(element);
                }

                foreach (var child in element.children)
                {
                    result.AddRange(FindById(child, id));
                }

                return result;
            }

            private static List<HtmlElement> FindByClass(HtmlElement element, string className)
            {
                var result = new List<HtmlElement>();
                if (element.classes.Contains(className))
                {
                    result.Add(element);
                }

                foreach (var child in element.children)
                {
                    result.AddRange(FindByClass(child, className));
                }

                return result;
            }

            private static List<HtmlElement> FindByTagName(HtmlElement element, string tagName)
            {
                var result = new List<HtmlElement>();
                if (element.name == tagName)
                {
                    result.Add(element);
                }

                foreach (var child in element.children)
                {
                    result.AddRange(FindByTagName(child, tagName));
                }

                return result;
            }

        }
    }
