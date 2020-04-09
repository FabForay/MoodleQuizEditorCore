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

        public List<Theme> Themes;

        private Theme lastTheme;

        public E3CTest()
        {
            Themes = new List<Theme>();
            lastTheme = null;
        }

        public void Load( String filePath )
        {
            TextReader reader;
            if (Path.GetExtension(filePath).ToLower() == ".docx ")
            {
                String allText = DocxToText(filePath);
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
                //
                switch (currentState)
                {
                    case State.None:
                        if (ligne.StartsWith(E3CTest.StartTheme))
                        {
                            currentTheme = new Theme(ligne);
                            currentState = State.Theme;
                        }
                        break;
                    case State.Theme:
                        if (ligne.StartsWith(E3CTest.StartTheme))
                        {
                            currentTheme = new Theme(ligne);
                        }
                        else if (ligne.StartsWith(E3CTest.StartQuestion))
                        {
                            //
                            currentQuestion = new Question(ligne);
                            currentTheme.Questions.Add(currentQuestion);
                            this.AddTheme(currentTheme);
                            currentState = State.Question;
                        }
                        break;
                    case State.Question:
                        if (ligne.StartsWith(E3CTest.StartReponses))
                        {
                            // ligne est ignorée
                            currentState = State.Reponse;
                        }
                        else
                        {
                            currentQuestion.Lignes.Add( ligne );
                        }
                        break;
                    case State.Reponse:
                        if (ligne.StartsWith(E3CTest.StartTheme))
                        {
                            currentTheme = new Theme(ligne);
                            currentState = State.Theme;
                            break;
                        }
                        else if (ligne.StartsWith(E3CTest.StartQuestion))
                        {
                            currentQuestion = new Question(ligne);
                            currentTheme.Questions.Add(currentQuestion);
                            this.AddTheme(currentTheme);
                            currentState = State.Question;
                            break;
                        }
                        else if (ligne.StartsWith(E3CTest.StartCorrection))
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

        private string DocxToText(string docxFile)
        {
            XmlNamespaceManager NsMgr = new XmlNamespaceManager(new NameTable());
            NsMgr.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

            using (var archive = ZipFile.OpenRead(docxFile))
            {
                return XDocument
                    .Load(archive.GetEntry(@"word/document.xml").Open())
                    .XPathSelectElements("//w:p", NsMgr)
                    .Aggregate(new StringBuilder(), (sb, p) => p
                        .XPathSelectElements(".//w:t|.//w:tab|.//w:cr|.//w:br", NsMgr)
                        .Select(e => { switch (e.Name.LocalName) { case "cr": case "br": return Environment.NewLine; case "tab": return "\t"; } return e.Value; })
                        .Aggregate(sb, (sb1, v) => sb1.Append(v)))
                    .ToString();
            }
        }
    }
}
