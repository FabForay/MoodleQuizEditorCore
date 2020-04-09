using System;
using System.Collections.Generic;
using System.Text;

namespace E3CLibrary
{
    public class Theme
    {
        
        public string Titre;
        public string Categorie;

        public List<Question> Questions;

        public Theme()
        {
            Titre = "";
            Categorie = "A";
            Questions = new List<Question>();
        }

        public Theme(string ligne) : this()
        {
            // Thème A : types de base
            if (ligne.StartsWith(E3CTest.StartTheme))
            {
                ligne = ligne.Substring(E3CTest.StartTheme.Length);
                ligne = ligne.TrimStart();
                string[] info = ligne.Split(new char[] { ':' });
                if ( info.Length >= 2)
                {
                    Categorie = info[0].Trim();
                    Titre = info[1].Trim();
                }
                else
                    Titre = info[0].Trim();
            } 
            else
                this.Titre = ligne;
        }
    }
}
