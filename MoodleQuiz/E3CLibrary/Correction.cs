using System.Text;

namespace E3CLibrary
{
    public class Correction
    {
        public string Choix;

        public Correction()
        {
            this.Choix = "";
        }

        public string Texte
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(E3CTest.StartCorrection);
                sb.Append(" : ");
                sb.Append(this.Choix);
                sb.Append(E3CTest.NewLine);
                return sb.ToString();
            }
        }

        public Correction(string ligne) : this()
        {
            ligne = ligne.TrimStart();
            // Correction : A
            if (ligne.StartsWith(E3CTest.StartCorrection))
            {
                string[] info = ligne.Split(new char[] { ':' });
                if (info.Length >= 2)
                {
                    Choix = info[1].Trim();
                }
            }
            else
                this.Choix = ligne;
        }
    }
}