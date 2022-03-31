//-----------------------------------------------------------------------
// <copyright file="Notes.cs" company="Jay Bautista Mendoza">
//     Copyright (c) Jay Bautista Mendoza. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace OctoNote.Services
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>Serializable class for a collection of Notes objects.</summary>
    [Serializable]
    [XmlRoot(ElementName = "notes")]
    public class NotesRoot
    {
        /// <summary>Initializes a new instance of the <see cref="NotesRoot" /> class.</summary>
        public NotesRoot()
        {
            this.Notes = new List<Note>();
        }

        /// <summary>Gets or sets the list of Notes objects.</summary>
        [XmlElement(ElementName = "note")]
        public List<Note> Notes { get; set; }
    }

    /// <summary>Serializable class of the Note object.</summary>
    [Serializable]
    public class Note
    {
        /// <summary>Gets or sets the Font Size attribute.</summary>
        [XmlAttribute(AttributeName = "size")]
        public string FontSize { get; set; }
        
        /// <summary>Gets or sets the Is Mono Spaced attribute.</summary>
        [XmlAttribute(AttributeName = "mono")]
        public string IsMonoSpaced { get; set; }
        
        /// <summary>Gets or sets the name of title element.</summary>
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        
        /// <summary>Gets or sets the name of content element.</summary>
        [XmlElement(ElementName = "content")]
        public string Content { get; set; }
        
        /// <summary>Gets or sets the Is Clipboard attribute.</summary>
        [XmlAttribute(AttributeName = "clipboard")]
        public string IsClipboard { get; set; }
        
        /// <summary>Gets or sets the Is Word Only attribute.</summary>
        [XmlAttribute(AttributeName = "wordonly")]
        public string IsWordOnly { get; set; }

        /// <summary>Gets or sets the Is Return Included attribute.</summary>
        [XmlAttribute(AttributeName = "copyreturn")]
        public string IsReturnIncluded { get; set; }
    }
}
