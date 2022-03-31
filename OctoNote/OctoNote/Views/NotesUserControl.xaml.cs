//-----------------------------------------------------------------------
// <copyright file="NotesUserControl.xaml.cs" company="Jay Bautista Mendoza">
//     Copyright (c) Jay Bautista Mendoza. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace OctoNote.Views
{
    using System;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>Interaction logic for NotesUserControl.</summary>
    public partial class NotesUserControl : UserControl
    {
        #region CONSTANT FIELDS │ PRIVATE │ NON-STATIC

        /// <summary>Font name for a mono-spaced font.</summary>
        private const string MonoFont = "DejaVu Sans Mono";

        /// <summary>Font name for a regular spaced font.</summary>
        private const string SansFont = "DejaVu Sans"; 

        #endregion

        #region CONSTRUCTORS │ PUBLIC │ NON-STATIC

        /// <summary>Initializes a new instance of the <see cref="NotesUserControl" /> class.</summary>
        public NotesUserControl()
        {
            this.InitializeComponent();
            this.SetFontFamily(SansFont);
            this.FontSizeTextBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, this.DisablePaste));
            ////this.CharacterCountText.Text = this.MainText.Count().ToString();
        }

        #endregion

        #region PROPERTIES │ PUBLIC │ NON-STATIC

        /// <summary>Gets or sets the text value of the notes' font size.</summary>
        public string MainFontSize
        {
            get { return this.FontSizeTextBox.Text; }
            set { this.FontSizeTextBox.Text = value; }
        }

        /// <summary>Gets or sets a value indicating whether or not the font family is mono-space.</summary>
        public bool IsMonopaced
        {
            get { return (bool)this.IsMonospacedCheckBox.IsChecked; }
            set { this.IsMonospacedCheckBox.IsChecked = value; }
        }

        /// <summary>Gets or sets the main content text of the notes.</summary>
        public string MainText
        {
            get { return this.MainTextBox.Text; }
            set { this.MainTextBox.Text = value; }
        }

        /// <summary>Gets or sets the main title text of the notes.</summary>
        public string TitleText
        {
            get { return this.TitleTextBox.Text; }
            set { this.TitleTextBox.Text = value; }
        }

        /// <summary>Gets or sets a value indicating whether or not it is on clipboard mode.</summary>
        public bool IsClipboard
        {
            get { return (bool)this.IsClipboardCheckBox.IsChecked; }
            set { this.IsClipboardCheckBox.IsChecked = value; }
        }

        /// <summary>Gets or sets a value indicating whether or not it is on return included.</summary>
        public bool IsReturnIncluded
        {
            get { return (bool)this.IsReturnIncludedCheckBox.IsChecked; }
            set { this.IsReturnIncludedCheckBox.IsChecked = value; }
        }

        /// <summary>Gets or sets a value indicating whether or not it is on return included.</summary>
        public bool IsWordOnly
        {
            get
            {
                return (bool)this.IsWordOnlyCheckBox.IsChecked;
            }

            set
            {
                this.IsWordOnlyCheckBox.IsChecked = value;
                this.SetCheckBoxVisibility();
            }
        }
        
        /// <summary>Gets or sets a value indicating whether or not it is on clipboard mode.</summary>
        public bool IsReadOnly
        {
            get { return (bool)this.MainTextBox.IsReadOnly; }
            set { this.MainTextBox.IsReadOnly = value; }
        }

        #endregion

        #region METHODS │ PRIVATE │ STATIC

        /// <summary>Determines if a text is allowed and a valid size (font).</summary>
        /// <param name="text">Text to test for validity.</param>
        /// <returns>Returns 'true' if valid, otherwise, returns 'false'.</returns>
        private static bool IsTextValidSize(string text)
        {
            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(text);
        }

        #endregion

        #region EVENTS │ PRIVATE │ NON-STATIC

        /// <summary>Disable paste functionality on a control.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">ExecutedRoutedEventArgs 'e'.</param>
        private void DisablePaste(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>Preview Text Input event method for the font size Text Box.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">TextCompositionEventArgs 'e'.</param>
        private void FontSizeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NotesUserControl.IsTextValidSize(e.Text);
        }

        /// <summary>Text Changed event method for the font size Text Box.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">TextChangedEventArgs 'e'.</param>
        private void FontSizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SetFontSize(this.FontSizeTextBox.Text);
        }

        /// <summary>Checked event method for the mono-space Check Box.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">RoutedEventArgs 'e'.</param>
        private void IsMonospacedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.SetFontFamily(MonoFont);
        }

        /// <summary>Unchecked event method for the mono-space Check Box.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">RoutedEventArgs 'e'.</param>
        private void IsMonospacedCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.SetFontFamily(SansFont);
        }

        /// <summary>Text Changed event method for the main Text Box.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">TextChangedEventArgs 'e'.</param>
        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateCharacterCount(this.MainTextBox.Text);
        }

        /// <summary>Preview Key Down event method of main Text Box, for keyboard shortcuts.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">KeyEventArgs 'e'.</param>
        private void MainTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.C:
                        if (string.IsNullOrEmpty(this.MainTextBox.SelectedText))
                        {
                            this.CopyDelete(true, false);
                        }

                        break;
                    case Key.X:
                        if (string.IsNullOrEmpty(this.MainTextBox.SelectedText))
                        {
                            this.CopyDelete(true, true);
                        }

                        break;
                    default: break;
                }
            }

            if (e.KeyboardDevice.IsKeyDown(Key.LeftShift) || e.KeyboardDevice.IsKeyDown(Key.RightShift))
            {
                switch (e.Key)
                {
                    case Key.Delete:
                        this.CopyDelete(false, true);
                        break;
                    default: break;
                }
            }
        }

        /// <summary>Click event method for the ClipBoard Check Box.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">RoutedEventArgs 'e'.</param>
        private void IsClipboardCheckBox_Click(object sender, RoutedEventArgs e)
        {
            this.MainTextBox.IsReadOnly = (bool)this.IsClipboardCheckBox.IsChecked;
        }

        /// <summary>Click event method for the Word Only Check Box.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">RoutedEventArgs 'e'.</param>
        private void IsWordOnlyCheckBox_Click(object sender, RoutedEventArgs e)
        {
            this.SetCheckBoxVisibility();
        }

        /// <summary>Preview LLeft Mouse Button Down event method of main Text Box.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">MouseButtonEventArgs 'e'.</param>
        private void MainTextBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.IsClipboard)
            {
                this.CopyDelete(true, false);
            }
        }
        
        #endregion
        
        #region METHODS │ PRIVATE │ NON-STATIC
       
        /// <summary>Sets the font family of the main Text Box.</summary>
        /// <param name="fontName">Name of the font type.</param>
        private void SetFontFamily(string fontName)
        {
            this.MainTextBox.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "JayWpf/Resources/Fonts/#" + fontName);
        }

        /// <summary>Sets the font size of the main Text Box.</summary>
        /// <param name="fontSize">Font size in string.</param>
        private void SetFontSize(string fontSize)
        {
            double parsedSize = new double();

            bool parseable = double.TryParse(fontSize, out parsedSize);

            if (!parseable || parsedSize < 1)
            {
                parsedSize = 11;
            }

            this.MainTextBox.FontSize = parsedSize;
        }

        /// <summary>Updates the character count Text Block.</summary>
        /// <param name="text">The text to count to show on the Text Block.</param>
        private void UpdateCharacterCount(string text)
        {
            this.CharacterCountText.Text = text.Replace("\r\n", "\n").Length.ToString();
        }

        /// <summary>Performs a copy and/or deletion of a word or text line.</summary>
        /// <param name="copy">Copy the entire word or line of text.</param>
        /// <param name="delete">Delete the entire word or line of text.</param>
        private void CopyDelete(bool copy, bool delete)
        {
            if (this.IsWordOnly)
            {
                this.CopyDeleteWord(copy, delete);
            }
            else
            {
                this.CopyDeleteLine(copy, delete);
            }
        }

        /// <summary>Performs a copy and/or deletion of a text line.</summary>
        /// <param name="copy">Copy the entire line of text.</param>
        /// <param name="delete">Delete the entire line of text.</param>
        private void CopyDeleteLine(bool copy, bool delete)
        {
            int lineIndex = this.MainTextBox.GetLineIndexFromCharacterIndex(this.MainTextBox.CaretIndex);
            string lineText = this.MainTextBox.GetLineText(lineIndex);

            if (this.IsClipboard)
            {
                this.MainTextBox.Select(this.MainTextBox.GetCharacterIndexFromLineIndex(lineIndex), lineText.Length);
            }

            if (!this.IsReturnIncluded)
            {
                lineText = lineText.Replace("\r", string.Empty);
                lineText = lineText.Replace("\n", string.Empty);
            }

            if (copy)
            {
                Clipboard.SetText(lineText);
            }

            if (delete)
            {
                int lineStartIndex = this.MainTextBox.GetCharacterIndexFromLineIndex(lineIndex);
                this.MainTextBox.Text = this.MainTextBox.Text.Remove(lineStartIndex, lineText.Length);
                this.MainTextBox.CaretIndex = lineStartIndex;
            }
        }

        /// <summary>Performs a copy and/or deletion of a text word.</summary>
        /// <param name="copy">Copy the entire word.</param>
        /// <param name="delete">Delete the entire word.</param>
        private void CopyDeleteWord(bool copy, bool delete)
        {
            string text = this.MainTextBox.Text;
            int caretIndex = this.MainTextBox.CaretIndex;
            char[] wordEnds = { ' ', '\r', '\n' };

            string partA = text.Substring(0, caretIndex);
            int startIndex = partA.LastIndexOfAny(wordEnds) + 1;
            partA = partA.Substring(startIndex);

            string partB = text.Substring(caretIndex);
            
            if (partB.IndexOfAny(wordEnds) >= 0)
            {
                partB = partB.Substring(0, partB.IndexOfAny(wordEnds));
            }

            text = string.Concat(partA, partB);

            if (this.IsClipboard)
            {
                this.MainTextBox.Select(startIndex, text.Length);
            }

            if (copy)
            {
                Clipboard.SetText(text);
            }

            if (delete)
            {
                this.MainTextBox.Text = this.MainTextBox.Text.Remove(startIndex, text.Length);
                this.MainTextBox.CaretIndex = startIndex;
            }
        }

        /// <summary>Sets the visibility of checkboxes.</summary>
        private void SetCheckBoxVisibility()
        {
            if ((bool)this.IsWordOnlyCheckBox.IsChecked)
            {
                this.IsReturnIncludedCheckBox.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                this.IsReturnIncludedCheckBox.Visibility = System.Windows.Visibility.Visible;
            }
        }

        #endregion
    }
}