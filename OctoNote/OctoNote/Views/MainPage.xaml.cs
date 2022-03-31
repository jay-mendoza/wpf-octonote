// ······································································//
// <copyright file="MainPage.xaml.cs" company="Jay Bautista Mendoza">    //
//     Copyright (c) Jay Bautista Mendoza. All rights reserved.          //
// </copyright>                                                          //
// ······································································//


namespace OctoNote.Views
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using OctoNote.Services;
    using SevenZip;

    /// <summary>Interaction logic for MainPage.</summary>
    public partial class MainPage : Page
    {
        #region FIELDS │ PRIVATE │ NON-STATIC │ NON-READONLY

        /// <summary>Declare an instance of Seven ZIP.</summary>
        private SevenZip sevenZip;

        /// <summary>An instance of the Notes Service.</summary>
        private NotesService notesService;

        /// <summary>Number of columns for the selected layout.</summary>
        private int columnCount;

        /// <summary>Number of rows for the selected layout.</summary>
        private int rowCount;
                
        #endregion
              
        #region CONSTRUCTORS │ PUBLIC │ NON-STATIC

        /// <summary>Initializes a new instance of the <see cref="MainPage" /> class.</summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.notesService = new NotesService();
            this.sevenZip = new SevenZip();

            try
            {
                this.SelectedTabIndex = int.Parse(ConfigurationManager.AppSettings.Get("tab"));
                this.SelectedLayout = "RB" + ConfigurationManager.AppSettings.Get("layout");
            }
            catch
            {
                this.SelectedTabIndex = 0;
                this.SelectedLayout = "RB1";
            }
            
            this.SetLayout(this.SelectedLayout);
        }

        #endregion

        #region PROPERTIES │ PUBLIC │ NON-STATIC

        /// <summary>Gets or sets the selected tab index of tab control.</summary>
        public int SelectedTabIndex { get; set; }

        /// <summary>Gets or sets the selected layout.</summary>
        public string SelectedLayout { get; set; } 

        #endregion

        #region METHODS │ PUBLIC │ NON-STATIC

        /// <summary>Save the current notes data to the save XML file.</summary>
        public void SaveNotes()
        {
            NotesRoot notes = new NotesRoot();

            foreach (TabItem item in MainTabControl.Items.Cast<TabItem>())
            {
                Grid grid = item.Content as Grid;

                foreach (NotesUserControl notesUC in grid.Children.Cast<NotesUserControl>())
                { 
                    ////NotesUserControl notesUC = item.Content as NotesUserControl;
                    Note note = new Note()
                    {
                        Title = notesUC.TitleText,
                        Content = notesUC.MainText,
                        IsMonoSpaced = notesUC.IsMonopaced.ToString(),
                        FontSize = notesUC.MainFontSize,
                        IsClipboard = notesUC.IsClipboard.ToString(),
                        IsWordOnly = notesUC.IsWordOnly.ToString(),
                        IsReturnIncluded = notesUC.IsReturnIncluded.ToString()
                    };

                    notes.Notes.Add(note);
                }
            }

            this.notesService.SaveNotes(notes);
        }

        /// <summary>Ask for archiving the current notes.</summary>
        public void ArchiveNotes()
        {
            if (MessageBox.Show("Do you want to create a data backup archive file?.", "Archive Current Data", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                string filename = Process.GetCurrentProcess().MainModule.FileName.Replace(".exe", ".xml");
                string archivename = Path.Combine(Path.GetDirectoryName(filename), "Archives", string.Format("xmlarchive-{0:yyyyMMddHHmmss}.7z", DateTime.Now));

                this.sevenZip.Archive(filename, archivename, SevenZip.CompressionLevel.Ultra);
            }
        }

        #endregion

        #region EVENTS │ PRIVATE │ NON-STATIC

        /// <summary>Click event method for the Save button.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">RoutedEventArgs 'e'.</param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.SaveNotes();
        }

        /// <summary>Click event method for the Reload button.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">RoutedEventArgs 'e'.</param>
        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Discard pending changes and load last saved data.", "Reload Data", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.OK)
            {
                this.LoadNotes();
            }
        }

        /// <summary>Click event method for the Archive button.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">RoutedEventArgs 'e'.</param>
        private void ArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            this.ArchiveNotes();
        }

        /// <summary>Click event method for the Explorer button.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">RoutedEventArgs 'e'.</param>
        private void ExplorerButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
        }

        /// <summary>Click event method for the Layout button.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">RoutedEventArgs 'e'.</param>
        /// <remarks>Select the Layout of the notes.</remarks>
        private void LayoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.SaveNotes();
            this.SelectedTabIndex = 0;
            RadioButton selectedButton  = sender as RadioButton;
            this.SetLayout(selectedButton.Name);
        }

        /// <summary>Selection Changed event method of the Main Tab Control.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">SelectionChangedEventArgs 'e'.</param>
        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.MainTabControl.SelectedIndex >= 0)
            { 
                this.SelectedTabIndex = this.MainTabControl.SelectedIndex;
            }
        }
        #endregion

        #region METHODS │ PRIVATE │ NON-STATIC

        /// <summary>Set the layout and reload data.</summary>
        /// <param name="layout">Layout string.</param>
        private void SetLayout(string layout)
        {
            switch (layout)
            {
                case "RB1":
                    this.columnCount = 1;
                    this.rowCount = 1;
                    this.RB1.IsChecked = true;
                    break;
                case "RB2":
                    this.columnCount = 2;
                    this.rowCount = 1;
                    this.RB2.IsChecked = true;
                    break;
                case "RB3":
                    this.columnCount = 1;
                    this.rowCount = 2;
                    this.RB3.IsChecked = true;
                    break;
                case "RB4":
                    this.columnCount = 2;
                    this.rowCount = 2;
                    this.RB4.IsChecked = true;
                    break;
                case "RB5":
                    this.columnCount = 4;
                    this.rowCount = 1;
                    this.RB5.IsChecked = true;
                    break;
                case "RB6":
                    this.columnCount = 1;
                    this.rowCount = 4;
                    this.RB6.IsChecked = true;
                    break;
                case "RB7":
                    this.columnCount = 4;
                    this.rowCount = 2;
                    this.RB7.IsChecked = true;
                    break;
                case "RB8":
                    this.columnCount = 2;
                    this.rowCount = 4;
                    this.RB8.IsChecked = true;
                    break;
                default: break;
            }

            this.SelectedLayout = layout.Substring(2);

            this.LoadNotes();
        }

        /// <summary>Load the notes from the save XML file.</summary>
        private void LoadNotes()
        {
            int columnCount = this.columnCount;
            int rowCount = this.rowCount;

            this.MainTabControl.Items.Clear();

            if (!File.Exists(this.notesService.XmlFile))
            {
                this.notesService.CreateBlankNotesFile(8);
            }

            List<Note> notes = this.notesService.LoadNotes().Notes;
            int notesPerPage = columnCount * rowCount;
            int pages = notes.Count / notesPerPage;
            if (notes.Count % notesPerPage > 0)
            {
                pages++;
            }

            int noteIndex = 0;

            for (int page = 0; page < pages; page++)
            {
                TabItem tabItem = new TabItem();
                tabItem.Style = this.FindResource("TabItemStyle") as Style;

                Grid tabGrid = new Grid();

                for (int row = 0; row < rowCount; row++)
                {
                    tabGrid.RowDefinitions.Add(new RowDefinition());
                    tabGrid.ColumnDefinitions.Clear();
                    
                    for (int column = 0; column < columnCount; column++)
                    {
                        tabGrid.ColumnDefinitions.Add(new ColumnDefinition());

                        if (noteIndex == notes.Count)
                        {
                            continue;
                        }

                        NotesUserControl notesUC = new NotesUserControl();
                        notesUC.Name = string.Concat("uc", noteIndex.ToString());

                        notesUC.TitleText = notes[noteIndex].Title;
                        notesUC.MainText = notes[noteIndex].Content;
                        notesUC.MainFontSize = notes[noteIndex].FontSize;
                        notesUC.IsMonopaced = Convert.ToBoolean(notes[noteIndex].IsMonoSpaced);
                        notesUC.IsClipboard = Convert.ToBoolean(notes[noteIndex].IsClipboard);
                        notesUC.IsReadOnly = Convert.ToBoolean(notes[noteIndex].IsClipboard);
                        notesUC.IsWordOnly = Convert.ToBoolean(notes[noteIndex].IsWordOnly);
                        notesUC.IsReturnIncluded = Convert.ToBoolean(notes[noteIndex].IsReturnIncluded);

                        notesUC.Margin = new Thickness(2);

                        Grid.SetColumn(notesUC, column);
                        Grid.SetRow(notesUC, row);

                        tabGrid.Children.Add(notesUC);

                        noteIndex++;
                    }
                }

                tabItem.Content = tabGrid;
                this.MainTabControl.Items.Add(tabItem);
            }

            this.MainTabControl.SelectedIndex = this.SelectedTabIndex;
        }
                
        #endregion
    }
}
