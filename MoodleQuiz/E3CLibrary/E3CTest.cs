using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace E3CLibrary
{
    enum State
    {
        None,
        Theme,
        Question,
        Reponse,
        Correction
    };


    /// <summary>
    /// Extrait les informations d'un fichier E3C
    /// </summary>
    public class E3CTest
    {
        public static readonly string StartTheme = "Thème";
        public static readonly string StartQuestion = "Question";
        public static readonly string StartReponses = "Réponses";
        public static readonly string StartCorrection = "Correction";

        public static string NewLine = Environment.NewLine;

        public static string SpacesForTab = "    ";

        public List<Theme> Themes;

        private Theme lastTheme;

        public E3CTest()
        {
            Themes = new List<Theme>();
            lastTheme = null;
        }

        public void Load(String filePath)
        {
            TextReader reader;
            if (Path.GetExtension(filePath).ToLower() == ".docx")
            {
                String allText = DocxToText(filePath);
                // File.WriteAllText(filePath + ".bkp", allText, Encoding.UTF8);
                reader = new StringReader(allText);
            }
            else
            {
                reader = new StreamReader(filePath);
            }
            //
            this.Load(reader);
            //
            reader.Close();
        }

        public void Save(String filePath)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Theme th in Themes)
            {
                sb.Append(th.Texte);
                sb.Append(Environment.NewLine);
            }
            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            //
        }


        public void Load(TextReader textStream)
        {
            State currentState = State.None;
            //
            Theme currentTheme = new Theme("aucun");
            Question currentQuestion = null;
            Reponse currentReponse = null;
            Correction currentCorrection = null;
            //
            while (textStream.Peek() > 0)
            {
                string ligne = textStream.ReadLine();
                string toCheck = ligne.Trim();
                //
                switch (currentState)
                {
                    case State.None:
                        if (toCheck.StartsWith(E3CTest.StartTheme))
                        {
                            currentTheme = new Theme(ligne);
                            currentState = State.Theme;
                        }
                        break;
                    case State.Theme:
                        if (toCheck.StartsWith(E3CTest.StartTheme))
                        {
                            currentTheme = new Theme(ligne);
                        }
                        else if (toCheck.StartsWith(E3CTest.StartQuestion))
                        {
                            //
                            currentQuestion = new Question(ligne);
                            currentTheme.Questions.Add(currentQuestion);
                            this.AddTheme(currentTheme);
                            currentState = State.Question;
                        }
                        break;
                    case State.Question:
                        if (toCheck.StartsWith(E3CTest.StartReponses))
                        {
                            // ligne est ignorée
                            currentState = State.Reponse;
                        }
                        else
                        {
                            currentQuestion.Lignes.Add(ligne);
                        }
                        break;
                    case State.Reponse:
                        if (toCheck.StartsWith(E3CTest.StartTheme))
                        {
                            currentTheme = new Theme(ligne);
                            currentState = State.Theme;
                            break;
                        }
                        else if (toCheck.StartsWith(E3CTest.StartQuestion))
                        {
                            currentQuestion = new Question(ligne);
                            currentTheme.Questions.Add(currentQuestion);
                            this.AddTheme(currentTheme);
                            currentState = State.Question;
                            break;
                        }
                        else if (toCheck.StartsWith(E3CTest.StartCorrection))
                        {
                            currentCorrection = new Correction(ligne);
                            currentState = State.Theme;
                            break;
                        }
                        // Il y a quelquechose ?
                        if (ligne.Length > 0)
                        {
                            // Rien dans le premier caractère ?
                            if (String.IsNullOrWhiteSpace(ligne.Substring(0, 1)))
                            {
                                // Soit elle est vide, soit c'est la suite de la réponse
                                if (!String.IsNullOrWhiteSpace(ligne))
                                {
                                    currentReponse.Lignes.Add(ligne);
                                }
                                //
                            }
                            else
                            {
                                // Début d'une réponse
                                currentReponse = new Reponse(ligne);
                                currentQuestion.Reponses.Add(currentReponse);
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
        }

        private void AddTheme(Theme currentTheme)
        {
            if (currentTheme != lastTheme)
            {
                this.Themes.Add(currentTheme);
                lastTheme = currentTheme;
            }
        }

        public static string DocxToText(string docxFile)
        {
            StringBuilder stringBuilder;
            StringBuilder sbTemp = new StringBuilder();
            using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(docxFile, false))
            {
                NameTable nameTable = new NameTable();
                XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
                xmlNamespaceManager.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
                xmlNamespaceManager.AddNamespace("m", "http://schemas.openxmlformats.org/officeDocument/2006/math");

                string wordprocessingDocumentText;
                using (StreamReader streamReader = new StreamReader(wordprocessingDocument.MainDocumentPart.GetStream()))
                {
                    wordprocessingDocumentText = streamReader.ReadToEnd();
                }

                stringBuilder = new StringBuilder(wordprocessingDocumentText.Length);

                XmlDocument xmlDocument = new XmlDocument(nameTable);
                xmlDocument.LoadXml(wordprocessingDocumentText);

                XmlNodeList paragraphNodes = xmlDocument.SelectNodes("//w:p", xmlNamespaceManager);
                foreach (XmlNode paragraphNode in paragraphNodes)
                {
                    XmlNodeList textNodes = paragraphNode.SelectNodes(".//w:t | .//w:tab | .//w:br | .//m:oMath | .//m:sup", xmlNamespaceManager);
                    foreach (XmlNode textNode in textNodes)
                    {
                        switch (textNode.Name)
                        {
                            case "w:t":
                                stringBuilder.Append(textNode.InnerText);
                                break;

                            case "w:tab":
                                stringBuilder.Append("\t");
                                break;

                            case "w:br":
                                stringBuilder.Append(Environment.NewLine);
                                break;
                            case "m:oMath":
                                //stringBuilder.Append(textNode.InnerText);
                                XmlNodeList mathTextNodes = textNode.SelectNodes(".//m:t | .//m:sup", xmlNamespaceManager);
                                foreach (XmlNode mathTextNode in mathTextNodes)
                                {
                                    switch (mathTextNode.Name)
                                    {
                                        case "m:t":
                                            string inner = mathTextNode.InnerText;
                                            char point = (char)8943;
                                            if (inner.Contains( point ))
                                            {
                                                sbTemp.Append(point);
                                                string pointPoint = sbTemp.ToString();
                                                sbTemp.Clear();
                                                inner = inner.Replace(pointPoint, "...");
                                            }
                                            stringBuilder.Append(inner);
                                            break;
                                        case "m:sup":
                                            stringBuilder.Append("^");
                                            break;
                                    }
                                }
                                break;
                        }
                    }

                    stringBuilder.Append(Environment.NewLine);
                }
            }

            return stringBuilder.ToString();
        }



    }
}
