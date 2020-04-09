using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MoodleQuiz
{
    internal static class XMLHelpers
    {

        internal static XmlNode AppendAttributeWithValue(XmlDocument xmlDoc, XmlNode node, List<KeyValuePair<string, string>> attributesNValue)
        {
            foreach (var kvp in attributesNValue)
            {
                XmlAttribute attribute = xmlDoc.CreateAttribute(kvp.Key);
                attribute.Value = kvp.Value;
                node.Attributes.Append(attribute);
            }
            return node;
        }


        internal static XmlAttribute CreateAttributeWithValue(XmlDocument xmlDoc, string attributeName, string value)
        {
            XmlAttribute attribute = xmlDoc.CreateAttribute(attributeName);
            attribute.Value = value;
            return attribute;
        }

        internal static XmlNode CreateElementWithText(XmlDocument xmlDoc, string elementName, string Text)
        {
            XmlNode node = xmlDoc.CreateElement(elementName);
            AddTextToElement(xmlDoc, node, Text );
            return node;
        }

        internal static XmlNode CreateElementWithText(XmlDocument xmlDoc, string elementName, string Text, bool cdata)
        {
            XmlNode node = xmlDoc.CreateElement(elementName);
            AddTextToElement(xmlDoc, node, Text, cdata);
            return node;
        }

        internal static XmlNode AddTextToElement(XmlDocument xmlDoc, XmlNode node, string Text )
        {
            return AddTextToElement(xmlDoc, node, Text, true);
        }

        internal static XmlNode AddTextToElement(XmlDocument xmlDoc, XmlNode node, string Text, bool cdata)
        {
            XmlNode innerNode = xmlDoc.CreateElement("text");
            if (!String.IsNullOrEmpty(Text))
            {
                if (cdata)
                {
                    innerNode.AppendChild(xmlDoc.CreateCDataSection(Text));
                }
                else
                {
                    innerNode.InnerText = Text;
                }
            }
            node.AppendChild(innerNode);
            return node;
        }

        internal static XmlNode FindChildNodeByName( XmlNode root, string nodeName )
        {
            XmlNode retValue = null;
            foreach (XmlNode node in root.ChildNodes)
            {
                string name = node.Name.ToLower();
                //
                if ( String.Compare( name, nodeName, true) == 0 )
                {
                        retValue = node;
                        break;
                }
            }
            if (retValue == null )
            {
                throw new Exception("Cannot find the " + nodeName + " node.");
            }
            return retValue;
        }

        internal static List<XmlNode> FindChildNodesByName(XmlNode root, string nodeName)
        {
            List<XmlNode> nodes = new List<XmlNode>();
            foreach (XmlNode node in root.ChildNodes)
            {
                string name = node.Name.ToLower();
                //
                if (String.Compare(name, nodeName, true) == 0)
                {
                    nodes.Add( node );
                }
            }
            return nodes;
        }

    }
}
