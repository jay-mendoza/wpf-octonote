//-----------------------------------------------------------------------
// <copyright file="NotesService.cs" company="Jay Bautista Mendoza">
//     Copyright (c) Jay Bautista Mendoza. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace OctoNote.Services
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>Service class for Notes.</summary>
    public class NotesService
    {
        #region FIELDS │ PRIVATE │ NON-STATIC │ NON-READONLY

        /// <summary>Private instance of the xmlFile.</summary>
        private string xmlFile;

        #endregion

        #region CONSTRUCTORS │ PUBLIC │ NON-STATIC

        /// <summary>Initializes a new instance of the <see cref="NotesService" /> class.</summary>
        public NotesService()
        {
            this.xmlFile = Process.GetCurrentProcess().MainModule.FileName.Replace(".exe", ".xml");
        }

        #endregion

        #region PROPERTIES │ PUBLIC │ NON-STATIC
        
        /// <summary>Gets the XML file path and name based on the program's name.</summary>
        public string XmlFile
        {
            get
            {
                return this.xmlFile;
            }
        }

        #endregion

        #region METHODS │ PUBLIC │ NON-STATIC

        /// <summary>Create a blank notes file.</summary>
        /// <param name="noteCount">Number of note objects to create in the notes file.</param>
        public void CreateBlankNotesFile(int noteCount)
        {
            NotesRoot notes = new NotesRoot();

            for (int ctr = 1; ctr <= noteCount; ctr++)
            {
                notes.Notes.Add(new Note { Title = "Note0" + ctr.ToString(), FontSize = "10", IsMonoSpaced = "false", IsClipboard = "false" });
            }

            this.SaveNotes(notes);
        }

        /// <summary>Load the NotesRoot object from the XML notes file.</summary>
        /// <returns>NotesRoot object of the XML notes file.</returns>
        public NotesRoot LoadNotes()
        {
            using (Stream fileStream = new FileStream(this.xmlFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(NotesRoot));
                NotesRoot root = (NotesRoot)deserializer.Deserialize(fileStream);
                return root;
            }
        }

        /// <summary>Save a NotesRoot object to the XML notes file.</summary>
        /// <param name="root">NotesRoot object for the XML notes file.</param>
        public void SaveNotes(NotesRoot root)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.NewLineHandling = NewLineHandling.Entitize;

            XmlSerializer serializer = new XmlSerializer(typeof(NotesRoot));
            using (XmlWriter writer = XmlWriter.Create(this.xmlFile, settings))
            {
                serializer.Serialize(writer, root);
            }
        }
        #endregion
    }
}
