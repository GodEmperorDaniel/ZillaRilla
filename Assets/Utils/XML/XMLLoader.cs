using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Xml.Linq;


public static class XMLLoader
{
    // TODO: Add build XML file location
    
#if DEBUG
    private static string xmlFileLocation = "Assets/Utils/XML/XML Files/";
#else
    private static string xmlFileLocation = "";
#endif
    
    private static IEnumerable<XElement> LoadXML(string fileName)
    {
        XDocument xmlDocument = XDocument.Load(fileName);
        IEnumerable<XElement> xmlElements = xmlDocument.Descendants("title").Elements();
        return xmlElements;
    }

    public static Dictionary<string, string> GetXMLDictionary(string fileName)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        IEnumerable<XElement> xmlElements = LoadXML($"{xmlFileLocation}{fileName}");

        foreach (XElement element in xmlElements)
        {
            if (element.Parent == null) continue;
            string title = element.Parent.Attribute("title")?.Value;
            string text = element.Parent.Element("text")?.Value.Trim();
            dictionary.Add(title, text);
        }

        return dictionary;
    }
}