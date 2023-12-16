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
            }
        }

        private static void CreateNewNote()
        {
            Console.Write("Enter name of note file, please?");
            string noteNameInput = Console.ReadLine();

            Console.Write("Enter note you want to save");
            string noteBody = Console.ReadLine();

            XmlWriterSettings xmlFileSettings = new XmlWriterSettings();

            xmlFileSettings.Indent = true;
            xmlFileSettings.CloseOutput = true;
            xmlFileSettings.OmitXmlDeclaration = true;

            string fileName = $"{noteNameInput}{DateTime.Now.ToString("dd-MM-yy")}.xml";

            using(XmlWriter newFile = XmlWriter.Create(NoteDirectoryPath + fileName, xmlFileSettings))
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
    }
}
