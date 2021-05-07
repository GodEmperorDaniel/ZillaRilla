using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;


public static class XMLLoader
{
    //private static XDocument xmlDocument; // File to load
    //private static IEnumerable<XElement> items; // For storing XML Items
    //private static List<XMLData> xmlDataList = new List<XMLData>();

    private static IEnumerable<XElement> LoadXML(string fileName)
    {
        XDocument xmlDocument = XDocument.Load(fileName);
        IEnumerable<XElement> xmlElements = xmlDocument.Descendants("").Elements();
        return xmlElements;
    }
    
    public static List<string> GetXMLStrings(string fileName)
    {
        List<string> stringList = new List<string>();
        
        
        
        return stringList;
    }
    
}
