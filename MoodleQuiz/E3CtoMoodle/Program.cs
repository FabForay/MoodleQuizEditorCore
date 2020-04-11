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
            if ((args.Length < 2) || (args.Length > 3))
            {
                AfficherAide("Nombre de paramètres incorrects");
            }
            if ((args[0].ToLower() != "-c") && (args[0].ToLower() != "-p"))
            {
                AfficherAide("switch de commande inconnu.");
            }
            if ((args[1] != "*") && !File.Exists(args[1]))
            {
                AfficherAide("Fichier Inconnu : " + args[1]);
            }
            string destFile;
            if (args.Length == 3)
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
                    Conversion(args[1], destFile);
                    break;
                case "-p":
                    Preparation(args[1], destFile);
                    break;

            }
            //
        }

        private static void Conversion(string orgFile, string destFile)
        {
            List<String> files = new List<string>();
            if (orgFile == "*")
            {
                foreach (var file in Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory))
                {
                    string ext = Path.GetExtension(file).ToLower();
                    if (ext == ".e3c")
                    {
                        files.Add(file);
                    }
                }
            }
            else
            {
                files.Add(orgFile);
            }
            //
            foreach (String fullPath in files)
            {
                try
                {
                    E3CTest test = new E3CTest();
                    Console.WriteLine("Lecture : " + Path.GetFileName(fullPath));
                    test.Load(fullPath);
                    Console.WriteLine("  --> Terminé.");
                    //
                    test.SaveAsMoodle(Path.GetFileNameWithoutExtension(fullPath) + ".xml");
                    Console.WriteLine("  --> Sauvegardé.");
                    //
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : {0}", ex.Message);
                }
            }
            //
        }

        private static void Preparation(string orgFile, string destFile)
        {
            List<String> files = new List<string>();
            if (orgFile == "*")
            {
                foreach (var file in Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory))
                {
                    string ext = Path.GetExtension(file).ToLower();
                    if ((ext == ".docx") || (ext == ".txt"))
                    {
                        files.Add(file);
                    }
                }
            }
            else
            {
                files.Add(orgFile);
            }
            //
            foreach (String fullPath in files)
            {
                try
                {
                    E3CTest test = new E3CTest();
                    Console.WriteLine("Traitement : " + Path.GetFileName(fullPath));
                    test.Load(fullPath);
                    Console.WriteLine("  --> Terminé.");
                    //
                    test.Save(Path.GetFileNameWithoutExtension(fullPath) + ".e3c.txt");
                    Console.WriteLine("  --> Sauvegardé.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : {0}", ex.Message);
                }
            }
            //
        }

        private static void AfficherAide(String message)
        {
            Console.WriteLine("E3CMoodle : Préparation/Convertion un formulaire d'E3C en Quiz Moodle");
            Console.WriteLine("Erreur " + message);
            Console.WriteLine();
            Console.WriteLine("Usage :");
            Console.WriteLine("Préparer : Ajoute le bloc Réponse à chaque Question s'il est manquant");
            Console.WriteLine("E3CMoodle.exe -p fichierE3C_original [fichierE3C]");
            Console.WriteLine("E3CMoodle.exe -p *");
            Console.WriteLine("Si fichierE3C_original est un fichier .docx, le texte est extrait avant conversion.");
            Console.WriteLine("Si * est utilisé, tous les fichiers .txt/.docx du dossier courant sont traités.");
            Console.WriteLine("fichierE3C a le nom du fichier d'origine avec l'extension .e3c.txt");
            Console.WriteLine();
            Console.WriteLine("Convertir : Convertit un formulaire E3C complet en Quiz Moodle");
            Console.WriteLine("E3CMoodle.exe -c fichierE3C [fichierE3C_XMLMoodle]");
            Console.WriteLine("E3CMoodle.exe -c *");
            Console.WriteLine("Le fichierE3C est un fichier texte avec extension .e3c, utilisé pour générer un Quiz Moodle en XML.");
            Console.WriteLine("Si * est utilisé, tous les fichiers .e3c du dossier courant sont traités.");
            Console.WriteLine("fichierE3C_XMLMoodle a le nom du fichier d'origine avec l'extension .xml");
            Console.WriteLine();
            Console.WriteLine();
            Environment.Exit(-1);
        }
    }
}
