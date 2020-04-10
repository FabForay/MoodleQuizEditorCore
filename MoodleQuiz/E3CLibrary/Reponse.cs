using System;
using System.Collections.Generic;
using System.Text;

namespace E3CLibrary
{
    public class Reponse
    {
        public string Choix;

        public List<string> Lignes;

        public Reponse()
        {
            Choix = "";
            Lignes = new List<string>();
        }

        public String Texte
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append( this.Choix);
                sb.Append(" ");
                bool first = true;
                foreach (string ligne in Lignes)
                {
                    string temp = ligne.Replace("\t", E3CTest.SpacesForTab);
                    if ( first )
                    {
                        if ( !String.IsNullOrWhiteSpace(temp))
                        {
                            sb.Append(" ");
                        }
                    }
                    sb.Append(temp);
                    sb.Append(E3CTest.NewLine);
                }
                return sb.ToString();
            }

        }

        public Reponse(string ligne) : this()
        {
            //
            if (ligne.Length > 0)
            {
                // Rien dans le premier caractère ?
                if (!String.IsNullOrWhiteSpace(ligne.Substring(0, 1)))
                {
                    Choix = ligne.Substring(0, 1);
                    ligne = ligne.Substring(1);
                    Lignes.Add(ligne);
                }
            }
        }
    }
}