using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters;

namespace NoteTakingApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ReadUserInput();
        }

        private static string NoteDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Notes\";

        private static void ReadUserInput()
        {
            Console.Write("Hello what would you like to do?\n");
            Console.WriteLine("1. Create a new note\n2. Edit existing note" +
                "\n3. Read a note\n4. Delete a note\n5. Show existing notes");
            Directory.CreateDirectory(NoteDirectoryPath);
            ConsoleKey input2 = Console.ReadKey().Key;

            switch (input2)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    CreateNewNote();
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    EditNote();
                    break;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    ReadNote();
                    break;
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    DeleteNote();
                    break;
                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    ShowExistingNotes();
                    break;
                default:
                    DefaultFunc();
                    break;
            }
        }

        private static void CreateNewNote()
        {
            Console.Clear();
            Console.WriteLine("Enter the name of the note you want to create, please.");
            string noteNameInput = Console.ReadLine();

            if (File.Exists(NoteDirectoryPath + noteNameInput + ".xml"))
            {
                Console.WriteLine("Note with that name already exists!");
                Console.WriteLine("Click ESC to go back or any other key to exit.");
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    Main(null);
                }
                else
                {
                    Environment.Exit(0);
                }
            }

            else
            {
                Console.WriteLine("Enter the note text you want to save.");
                string noteBody = Console.ReadLine();

                XmlWriterSettings xmlFileSettings = new XmlWriterSettings();

                xmlFileSettings.Indent = true;
                xmlFileSettings.CloseOutput = true;
                xmlFileSettings.OmitXmlDeclaration = true;

                string fileName = $"{noteNameInput}.xml";

                using (XmlWriter newFile = XmlWriter.Create(NoteDirectoryPath + fileName, xmlFileSettings))
                {
                    newFile.WriteStartDocument();
                    newFile.WriteStartElement("note");
                    newFile.WriteElementString("noteBody", noteBody);
                    newFile.WriteEndElement();
                    newFile.Flush();
                    newFile.Close();
                }
                Console.Clear();
                Console.WriteLine("Note added succesfully.\nClick any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        private static void EditNote()
        {
            Console.Clear();
            Console.WriteLine("Enter the name of the note you would like to edit.");
            string[] files = Directory.GetFiles(NoteDirectoryPath);
            foreach (string file in files)
            {
                Console.WriteLine(Path.GetFileNameWithoutExtension(file));
            }
            
            string input = Console.ReadLine();
            if (File.Exists(NoteDirectoryPath + input + ".xml"))
            {

                XmlDocument xmldoc = new XmlDocument();
                try
                {
                    xmldoc.Load(NoteDirectoryPath + input + ".xml");
                    string tekstfajla = xmldoc.SelectSingleNode("//noteBody").InnerText;
                    Console.WriteLine(tekstfajla);

                    string editedString = Console.ReadLine();
                    xmldoc.SelectSingleNode("//noteBody").InnerText = editedString;
                    xmldoc.Save(NoteDirectoryPath + input + ".xml");
                    Console.Clear();
                    Console.WriteLine("Note has been edited succesfully.\nClick any key to exit.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Couldn't edit the note because of the following error: {e.Message}");
                }

            }
            else
            {
                Console.WriteLine("File not found!\n");
            }
        }

        private static void DefaultFunc()
        {
            Console.Clear();
            Console.WriteLine("Please chose key from the list!");
            Console.WriteLine("Press any key to go back.");
            Console.ReadKey();
            Console.Clear();
            Main(null);
        }

        private static void ReadNote()
        {
            Console.Clear();
            
            string[] files = Directory.GetFiles(NoteDirectoryPath);
            if(files.Length != 0)
            {
                Console.WriteLine("Enter the name of the note you would like to read.");
                foreach (string file in files)
                {
                    Console.WriteLine(Path.GetFileNameWithoutExtension(file));
                }
            }
            else
            {
                Console.WriteLine("There are no notes yet.\nCreate one first.\nPress any key to go back.");
                Console.ReadKey();
                Console.Clear();
                Main(null);
            }
            string userInput = Console.ReadLine();
            if(File.Exists(NoteDirectoryPath + userInput + ".xml"))
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(NoteDirectoryPath + userInput + ".xml");
                    Console.Clear();
                    Console.WriteLine($"Note text:{doc.SelectSingleNode("//noteBody").InnerText}");
                    Console.WriteLine("Click any key to exit!");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Couldn't load the note because of the following error {e.Message}");
                }
                
            }
            else
            {
                Console.Clear();
                Console.WriteLine("File not found!");
                Console.WriteLine("Click any key to go back!");
                Console.ReadKey();
                ReadNote();
            }
        }

        private static void DeleteNote()
        {
            Console.Clear();
            Console.WriteLine("Please enter the name of the note you would like to delete.");
            string[] files = Directory.GetFiles(NoteDirectoryPath);
            foreach (string file in files)
            {
                Console.WriteLine(Path.GetFileNameWithoutExtension(file));
            }
            string userInput = Console.ReadLine();
            if (File.Exists(NoteDirectoryPath + userInput + ".xml"))
            {
                Console.WriteLine("Are you sure you want to delete it? Y/N?");
                ConsoleKeyInfo confirmation = Console.ReadKey();
                if(confirmation.Key == ConsoleKey.Y)
                {
                    try
                    {
                        File.Delete(NoteDirectoryPath + userInput + ".xml");
                        Console.Clear();
                        Console.WriteLine($"Note has been deleted successfully!");
                        Console.WriteLine("Click any key to exit!");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Couldn't delete the note because of the following error {e.Message}");
                    }
                }  
            }
            else
            {
                Console.Clear();
                Console.WriteLine("File not found!");
                Console.WriteLine("Click any key to go back!");
                Console.ReadKey();
                ReadNote();
            }
        }

        private static void ShowExistingNotes()
        {
            Console.Clear();
            Console.WriteLine("List of existing notes:");
            var files = Directory.GetFiles(NoteDirectoryPath);
            if(files.Length > 0)
            {
                foreach (string file in files)
                {
                    Console.WriteLine(Path.GetFileNameWithoutExtension(file));
                }
            }
            else
            {
                Console.WriteLine("There are no existing files.");
                Console.WriteLine("Click ESC to go back or any other key to exit.");
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    Main(null);
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
