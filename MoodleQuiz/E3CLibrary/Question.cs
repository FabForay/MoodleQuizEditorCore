using System;
using System.Collections.Generic;
using System.Text;

namespace E3CLibrary
{
    public class Question
    {
        public string NewLine;

        public string Titre;

        public List<String> Lignes;

        public List<Reponse> Reponses;

        public Correction Correction;

        public String TexteQuestion
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (Lignes.Count > 0)
                {
                    for (int i = 0; i < Lignes.Count - 1; i++)
                    {
                        sb.Append(Lignes[i]);
                        sb.Append(NewLine);
                    }
                    sb.Append(Lignes[Lignes.Count - 1]);
                }
                return sb.ToString();
            }

        }

        public Question()
        {
            NewLine = Environment.NewLine;
            Titre = "";
            Lignes = new List<string>();
            Reponses = new List<Reponse>();
            Correction = new Correction("");
        }

        public Question(string ligne) : this()
        {
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
