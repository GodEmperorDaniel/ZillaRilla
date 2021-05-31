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
    
    private static IEnumerable<XElement> OldLoadXML(string fileName)
    {
        
        
        XDocument xmlDocument = XDocument.Load(fileName);
        IEnumerable<XElement> xmlElements = xmlDocument.Descendants("title").Elements();
        return xmlElements;
    }

    private static IEnumerable<XAttribute> LoadXML(string fileName)
    {
        XDocument xmlDocument = XDocument.Load(fileName);
        IEnumerable<XAttribute> xmlAttribute = xmlDocument.Descendants("category").Attributes();
        return xmlAttribute;
    }

    public static Dictionary<string, Dictionary<string, string>> GetXMLDictionary(string filePath)
    {
        Dictionary<string, Dictionary<string, string>> categoryDictionary = new Dictionary<string, Dictionary<string, string>>();
        string[] files = Directory.GetFiles(filePath, "*.xml");
        
        foreach (string file in files)
        {
            IEnumerable<XAttribute> xmlAttributes = LoadXML(file);

            // Loop that creates a dictionary of categories 
            foreach (XAttribute attribute in xmlAttributes)
            {
                Dictionary<string, string> elements = new Dictionary<string, string>();

                if (attribute.Name != "category" || attribute.Parent == null) continue;
                string category = attribute.Value;
                
                // Loop that creates a dictionary for every Title-Text pairing in the category 
                foreach (XElement element in attribute.Parent.Elements())
                {
                    string title = element.Attribute("title")?.Value;
                    string text = element.Element("text")?.Value.Trim();
                    elements.Add(title, text);
                }
                
                categoryDictionary.Add(category, elements);
            }
        }
        
        return categoryDictionary;
    }
}