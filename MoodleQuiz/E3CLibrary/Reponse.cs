using System;
using System.Collections.Generic;

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