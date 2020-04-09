using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MoodleQuiz
{
    public class Category
    {
        private string text;
        /// <summary>
        /// Titre de la Catégorie
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                string catText = "$course$/top";
                if (value.StartsWith(catText))
                {
                    value = value.Substring(catText.Length);
                    if ( value.StartsWith("/"))
                    {
                        value = value.Substring(1);
                    }
                }
                text = value;
            }
        }

        public string TextXML
        {
            get
            {
                string catText = "$course$/top";
                if (!String.IsNullOrEmpty(this.Text) && !this.Text.StartsWith(catText))
                {
                    catText = catText + "/" + this.Text;
                }
                return catText;
            }
        }

        public Category(string Title)
        {
            this.Text = Title;
        }

        public Category()
        {
            this.Text = "";
        }


        public XmlNode MoodleXML
        {
            /*
              <question type="category">
                <category>
                    <text>$course$/top</text>

                </category>
              </question>
             * */
            get
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode node = xmlDoc.CreateElement("question");
                node.Attributes.Append(XMLHelpers.CreateAttributeWithValue(xmlDoc, "type", "category"));
                //
                XmlNode subnode = XMLHelpers.CreateElementWithText(xmlDoc, "category", this.TextXML, false);
                node.AppendChild(subnode);
                //
                return node;
            }

            set
            {
                // Root est l'élément category
                XmlNode root = value;
                if (string.Compare(root.Name, "question", true) != 0)
                {
                    throw new Exception("Cannot find the Category root.");
                }
                //
                bool isOk = false;
                foreach (XmlAttribute attribute in root.Attributes)
                {
                    if (string.Compare(attribute.Name, "type", true) == 0)
                    {
                        if (string.Compare(attribute.Value, "category", true) == 0)
                        {
                            isOk = true;
                            break;
                        }
                    }
                }
                if (!isOk)
                {
                    throw new Exception("The Question is not a Category root.");
                }
                //
                root = XMLHelpers.FindChildNodeByName(root, "category");
                root = XMLHelpers.FindChildNodeByName(root, "text");
                //
                this.Text = root.InnerText;
            }
        }
    }
}
