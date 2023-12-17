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
            Directory.CreateDirectory(NoteDirectoryPath);
            string userInput = Console.ReadLine();

            switch (userInput.ToLower())
            {
                case "new":
                    CreateNewNote();
                    break;
                case "edit":
                    EditNote();
                    break;
            }
        }

        private static void CreateNewNote()
        {
            Console.WriteLine("Enter name of note file, please?");
            string noteNameInput = Console.ReadLine();

            Console.WriteLine("Enter note you want to save");
            string noteBody = Console.ReadLine();

            XmlWriterSettings xmlFileSettings = new XmlWriterSettings();

            xmlFileSettings.Indent = true;
            xmlFileSettings.CloseOutput = true;
            xmlFileSettings.OmitXmlDeclaration = true;

            //string fileName = $"{noteNameInput}{DateTime.Now.ToString("dd-MM-yy")}.xml";
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
            Environment.Exit(0);
        }

        private static void EditNote()
        {
            string[] files = Directory.GetFiles(NoteDirectoryPath);
            foreach (string file in files)
            {
                Console.WriteLine(Path.GetFileNameWithoutExtension(file));
            }
            Console.WriteLine("Enter the name of file you want to edit.");
            string input = Console.ReadLine();
            if (File.Exists(NoteDirectoryPath + input + ".xml"))
            {

                XmlDocument xmldoc = new XmlDocument();
                try
                {
                    xmldoc.Load(NoteDirectoryPath + input + ".xml");
                    var tekstfajla = xmldoc.SelectSingleNode("//noteBody").InnerText;
                    Console.WriteLine(tekstfajla);

                    string editedString = Console.ReadLine();
                    xmldoc.SelectSingleNode("//noteBody").InnerText = editedString;
                    xmldoc.Save(NoteDirectoryPath + input + ".xml");

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
    }
}
