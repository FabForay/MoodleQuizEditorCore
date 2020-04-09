using E3CLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace E3CtoMoodle
{
    class Program
    {
        static void Main(string[] args)
        {
            if ( (args.Length < 2 ) || (args.Length > 3))
            {
                AfficherAide("Nombre de paramètres incorrects");
            }
            if ((args[0].ToLower() != "-c") && (args[0].ToLower() != "-p"))
            {
                AfficherAide("switch de commande inconnu.");
            }
            if (!File.Exists(args[1]))
            {
                AfficherAide("Fichier Inconnu : " + args[1]);
            }
            string destFile;
            if (args.Length == 3 )
            {
                destFile = args[2];
            }
            else
            {
                string temp = Path.GetFileNameWithoutExtension(args[1]);
                destFile = temp + "_reponse" + Path.GetExtension(args[1]);
            }
            //
            switch (args[0].ToLower())
            {
                case "-c":
                    Convertion(args[1], destFile);
                    break;
                case "-p":
                    Preparation(args[1], destFile);
                    break;

            }
            //
        }

        private static void Preparation(string v, string destFile)
        {
            throw new NotImplementedException();
        }

        private static void Convertion(string v, string destFile)
        {
            E3CTest test = new E3CTest();
            test.Load(v);
            //
        }

        private static void AfficherAide(String message)
        {
            Console.WriteLine("E3CMoodle : Préparation/Convertion un formulaire d'E3C en Quiz Moodle");
            Console.WriteLine("Erreur " + message);
            Console.WriteLine();
            Console.WriteLine("Usage :");
            Console.WriteLine("Préparer : Ajoute le bloc Réponse à chaque Question s'il est manquant");
            Console.WriteLine("E3CMoodle.exe -p fichierTexteE3C [fichierTexteE3C_Reponse]");
            Console.WriteLine();
            Console.WriteLine("Convertir : Convertit un formulaire E3C en Quiz Moodle");
            Console.WriteLine("E3CMoodle.exe -c fichierTexteE3C [fichierTexteE3C_Moodle]");
            Console.WriteLine();
            Console.WriteLine();
            Environment.Exit(-1);
        }
    }
}
