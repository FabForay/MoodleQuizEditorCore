namespace E3CLibrary
{
    public class Correction
    {
        public string Choix;

        public Correction()
        {
            this.Choix = "";
        }

        public Correction(string ligne) : this()
        {
            // Correction : A
            if (ligne.StartsWith(E3CTest.StartCorrection))
            {
                ligne = ligne.TrimStart();
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