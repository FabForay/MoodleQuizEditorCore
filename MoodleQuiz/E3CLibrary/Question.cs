using System;
using System.Collections.Generic;
using System.Text;

namespace E3CLibrary
{
    public class Question
    {

        public string Titre;

        public List<String> Lignes;

        public List<Reponse> Reponses;

        public Correction Correction;

        public String Texte
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(E3CTest.StartQuestion);
                sb.Append(" ");
                sb.Append(this.Titre);
                sb.Append(E3CTest.NewLine);
                sb.Append(this.TexteQuestion);
                sb.Append(E3CTest.StartReponses);
                sb.Append(E3CTest.NewLine);
                foreach (Reponse rep in Reponses)
                {
                    sb.Append(rep.Texte);
                }
                sb.Append(this.Correction.Texte);
                sb.Append(E3CTest.NewLine);
                return sb.ToString();
            }

        }

        public String TexteQuestion
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (string ligne in Lignes)
                {
                    string temp = ligne.Replace("\t", E3CTest.SpacesForTab);
                    sb.Append(temp);
                    sb.Append(E3CTest.NewLine);
                }
                return sb.ToString();
            }

        }

        public Question()
        {
            Titre = "";
            Lignes = new List<string>();
            Reponses = new List<Reponse>();
            Correction = new Correction("");
        }

        public Question(string ligne) : this()
        {
            ligne = ligne.TrimStart();
            // Question A.2
            if (ligne.StartsWith(E3CTest.StartQuestion))
            {
                ligne = ligne.Substring(E3CTest.StartQuestion.Length);
                ligne = ligne.TrimStart();
            }
            this.Titre = ligne;
        }
    }
}
