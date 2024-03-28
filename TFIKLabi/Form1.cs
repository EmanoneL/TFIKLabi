using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TFIKLabi
{
    public partial class Form1 : Form
    {
        string filePath;
        string standartFileName = "NewFile";
        List<RegResult> results = new List<RegResult> { };

        public Form1()
        {
            InitializeComponent();
            //toolStripMenuItem1.Click += ToolStripMenuItem1_Click;

        }

        private string getNewFileName()
        {
            string fileName = standartFileName + ".txt";
            return fileName;

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)    //обработчик создать файл
        {


            // Получаем полный путь к будущему файлу
            filePath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + getNewFileName();
            OpenFile(filePath);
            // toolStripMenuItem3_Click


        }
        private void OpenFile(string filePath)  //открыть файл после создания
        {
            //string fileName = Path.GetFileName(filePath);
            //TabPage tabPage = new TabPage(fileName);
            //try
            //{
            //    tabControl1.TabPages.Add(tabPage);

            //    RichTextBox richTextBox = new RichTextBox();
            //    richTextBox.Dock = DockStyle.Fill;
            //    tabPage.Controls.Add(richTextBox);
            //    tabControl1.SelectedTab = tabPage;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Error reading the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    tabControl1.TabPages.Remove(tabPage);
            //}

            string fileName = Path.GetFileName(filePath);
            TabPage tabPage = new TabPage(fileName);
            try
            {
                tabPage.Tag = filePath;

                tabControl1.TabPages.Add(tabPage);

                RichTextBox richTextBox1 = new RichTextBox();
                richTextBox1.Width = 780; // Установка ширины элемента
                richTextBox1.Height = 150;
                //richTextBox.Dock = DockStyle.Fill;
                tabPage.Controls.Add(richTextBox1);


                RichTextBox richTextBox2 = new RichTextBox();
                richTextBox2.Width = 780; // Установка ширины элемента
                richTextBox2.Height = 130;
                richTextBox2.Location = new Point(0, richTextBox1.Height + 30);
                //richTextBox.Dock = DockStyle.Fill;
                tabPage.Controls.Add(richTextBox2);



                tabControl1.SelectedTab = tabPage;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabControl1.TabPages.Remove(tabPage);
            }

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)    //обработчик открыть файл
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;

                foreach (TabPage tab in tabControl1.TabPages)
                {
                    if (tab.Tag.ToString() == filePath)
                    {
                        MessageBox.Show("Файл уже открыт во вкладке.");
                        return;
                    }
                }

                string fileContent = File.ReadAllText(filePath);
                TabPage tabPage = new TabPage(Path.GetFileName(filePath));
                tabControl1.TabPages.Add(tabPage);
                tabPage.Tag = filePath;

                RichTextBox richTextBox1 = new RichTextBox();
                richTextBox1.Width = 780; // Установка ширины элемента
                richTextBox1.Height = 150;
                //richTextBox.Dock = DockStyle.Fill;
                richTextBox1.Text = fileContent;
                tabPage.Controls.Add(richTextBox1);


                RichTextBox richTextBox2 = new RichTextBox();
                richTextBox2.Width = 780; // Установка ширины элемента
                richTextBox2.Height = 130;
                richTextBox2.Location = new Point(0, richTextBox1.Height + 30);
                //richTextBox.Dock = DockStyle.Fill;
                tabPage.Controls.Add(richTextBox2);


                tabControl1.SelectedTab = tabPage;


            }

        }

        private void RichTextBox_KeyDown(object sender, EventArgs e)
        {
            previousText = ((RichTextBox)sender).Text;
        }



        // Проверка, сохранен ли файл
        private bool IsFileSaved(TabPage tabPage)
        {
            RichTextBox richTextBox = tabPage.Controls.OfType<RichTextBox>().FirstOrDefault();

            if (richTextBox != null && tabPage.Tag != null)
            {
                string filePath = tabPage.Tag.ToString();
                bool fileExist = System.IO.File.Exists(filePath);
                if (fileExist)
                {
                    string fileContent = File.ReadAllText(filePath);
                    // Если содержимое textbox такое же, как в файле, то он считается сохраненным
                    return richTextBox.Text == fileContent; // Вернуть должен false FIX
                }
                else
                {
                    return System.IO.File.Exists(filePath);
                }
            }

            return false;
        }

        // Сохранить 
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedTab != null)
            //{
            //    RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            //    string file = tabControl1.SelectedTab.Text;
            //    if (richTextBox != null)
            //    {

            //        if (System.IO.File.Exists(System.IO.Path.GetFullPath(file)))
            //        {
            //            File.WriteAllText(file, richTextBox.Text);

            //        }
            //        else
            //        {
            //            SaveKaK();
            //        }
            //    }
            //}
            //////////////////////////////////////////////////////////////////////////последний вариант
            if (tabControl1.SelectedTab != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                if (richTextBox != null)
                {
                    if (!System.IO.File.Exists(filePath)) // Возможно поменять на exist
                    {
                        SaveKaK();
                    }
                    else
                    {
                        File.WriteAllText(filePath, richTextBox.Text);
                    }
                }
            }


        }



        private void SaveKaK()
        {

            if (tabControl1.SelectedTab != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                if (richTextBox != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
                    saveFileDialog.FileName = tabControl1.SelectedTab.Text;
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(saveFileDialog.FileName, richTextBox.Text);
                        tabControl1.SelectedTab.Text = Path.GetFileNameWithoutExtension(saveFileDialog.FileName) + ".txt";
                        filePath = saveFileDialog.FileName; // Обновляем путь к файлу
                        tabControl1.SelectedTab.Tag = filePath; // Сохраняем путь к файлу в Tag
                    }
                }
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)      //обработчик - сохранить как
        {
            SaveKaK();
        }

        private void CloseFile()       //не знаю как закрывать файл и надо ли
        {
            //TabPage currentPage = tabControl1.SelectedTab;
            //if (currentPage != null)
            //{
            //    RichTextBox currentRichTextBox = currentPage.Controls.OfType<RichTextBox>().FirstOrDefault();
            //    if (currentRichTextBox != null)
            //    {
            //        string fileContent = currentRichTextBox.Text;

            //        if (fileContent != File.ReadAllText(currentPage.Text))
            //        {
            //            DialogResult result = MessageBox.Show("Сохранить изменения?", "Предупреждение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            //            if (result == DialogResult.Yes)
            //            {
            //                SaveKaK();
            //            }
            //            else if (result == DialogResult.Cancel)
            //            {
            //                return; // Отмена закрытия файла
            //            }
            //        }

            //        tabControl1.TabPages.Remove(currentPage);
            //    }
            //}
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)    //выход из программы но не сохранили изменения
        {
            TabControl tabControl = tabControl1;

            foreach (TabPage tabPage in tabControl.TabPages)
            {
                if (!IsFileSaved(tabPage))
                {
                    DialogResult result = MessageBox.Show("Файл " + tabPage.Text + " не сохранен. Хотите сохранить перед выходом?", "Предупреждение", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        // Сохранение файла
                        SaveKaK();
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return; // Отмена выхода из программы
                    }
                }
            }

            //Application.Exit();
        }


        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (tabControl1.TabPages.Count == 0)
            {
                // Если нет открытых файлов, выходим из метода
                return;
            }

            if (tabControl1.SelectedTab.Controls.Count > 0 && tabControl1.SelectedTab.Controls[0] is RichTextBox)
            {
                RichTextBox richTextBox = (RichTextBox)tabControl1.SelectedTab.Controls[0];
                richTextBox.Undo();
            }
        }

        // richTextBox.TextChanged += RichTextBox_TextChanged;

        private string previousText = "";


        private void RichTextBox_TextChanged(object sender, EventArgs e)
        {
            if (sender is RichTextBox)
            {
                RichTextBox richTextBox = (RichTextBox)sender;
                previousText = richTextBox.Text;
            }
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage currentTab = tabControl1.SelectedTab;
            if (currentTab != null)
            {
                RichTextBox currentRichTextBox = currentTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                if (currentRichTextBox != null && currentRichTextBox.SelectionLength > 0)
                {
                    Clipboard.SetText(currentRichTextBox.SelectedText); // Копируем выделенный текст в буфер обмена
                    currentRichTextBox.SelectedText = ""; // Удаляем выделенный текст из текущего RichTextBox
                }
            }
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage currentTab = tabControl1.SelectedTab;
            if (currentTab != null)
            {
                RichTextBox currentRichTextBox = currentTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                if (currentRichTextBox != null && currentRichTextBox.SelectionLength > 0)
                {
                    Clipboard.SetText(currentRichTextBox.SelectedText); // Копируем выделенный текст в буфер обмена
                }
            }
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage currentTab = tabControl1.SelectedTab;
            if (currentTab != null)
            {
                RichTextBox currentRichTextBox = currentTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                if (currentRichTextBox != null)
                {
                    currentRichTextBox.SelectedText = Clipboard.GetText(); // Вставляем текст из буфера обмена в текущий RichTextBox
                }
            }
        }

        private void выделитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage currentTab = tabControl1.SelectedTab;
            if (currentTab != null)
            {
                RichTextBox currentRichTextBox = currentTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                if (currentRichTextBox != null)
                {
                    currentRichTextBox.SelectAll(); // Выделяем весь текст в текущем RichTextBox
                }
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage currentTab = tabControl1.SelectedTab;
            if (currentTab != null)
            {
                RichTextBox currentRichTextBox = currentTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                if (currentRichTextBox != null && currentRichTextBox.SelectionLength > 0)
                {
                    currentRichTextBox.SelectedText = ""; // Удаляем выделенный текст
                }
            }
        }

        private void повторитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count == 0)
            {
                // Если нет открытых файлов, выходим из метода
                return;
            }
            if (tabControl1.SelectedTab.Controls.Count > 0 && tabControl1.SelectedTab.Controls[0] is RichTextBox)
            {
                RichTextBox richTextBox = (RichTextBox)tabControl1.SelectedTab.Controls[0];
                richTextBox.Redo();
            }
        }

        private void вызовСправкиToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string filePath = @"help.html";

            try
            {
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                try
                {
                    Process.Start("cmd", $"/c start {filePath}");
                }
                catch (Exception innerEx)
                {
                    MessageBox.Show("Ошибка при открытии файла: " + innerEx.Message);
                }
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = @"about.html";

            try
            {
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                try
                {
                    Process.Start("cmd", $"/c start {filePath}");
                }
                catch (Exception innerEx)
                {
                    MessageBox.Show("Ошибка при открытии файла: " + innerEx.Message);
                }
            }
        }

        private void закрытьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TabControl tabControl = tabControl1;
            //if (tabControl.SelectedTab != null)
            //{
            //    tabControl.TabPages.Remove(tabControl.SelectedTab);
            //}

            TabControl tabControl = tabControl1;
            if (tabControl.SelectedTab != null)
            {
                if (!IsFileSaved(tabControl.SelectedTab))
                {

                    // Пример вызова диалогового окна с предупреждением
                    DialogResult result = MessageBox.Show("Файл не сохранен. Хотите сохранить перед закрытием?", "Предупреждение", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        // Сохранение файла. Если файл существует, то сохранить просто
                        toolStripMenuItem4_Click(sender, e);
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return; // Отмена закрытия файла
                    }
                }

                tabControl.TabPages.Remove(tabControl.SelectedTab);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                if (richTextBox != null)
                {
                    richTextBox.Clear();
                }
            }
        }

        private void Falss()
        {
            //results.Clear();
            TabPage activeTab = tabControl1.SelectedTab;
            RichTextBox activeRichTextBox = activeTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            string text = activeRichTextBox.Text;

            RichTextBox textOutput = activeTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();
            string currentState = "q0";
            textOutput.AppendText($"Состояние: {currentState.ToString()}");
            string fileName = "";
            

            if (!new char[] { '#', '/', ':' }.Contains(text[0]))
            {
                fileName += text[0];
                textOutput.AppendText($"Состояние: ");
                currentState = "q2"; // Обновляем состояние до q1 при добавлении первого символа в имя файла
            }

            Dictionary<string, Dictionary<char, string>> states = new Dictionary<string, Dictionary<char, string>>()
            {
            { "q0", new Dictionary<char, string>() { { '\b', "q1" } } },
            { "q1", new Dictionary<char, string>() { { ' ', "q2" } } },
            { "q2", new Dictionary<char, string>() { { '.', "q3" }, { ' ', "q2" } } },
            { "q3", new Dictionary<char, string>() { { 'd', "q4" }, { 't', "q7" }, { 'p', "q9" } } },
            { "q4", new Dictionary<char, string>() { { 'o', "q5" } } },
            { "q5", new Dictionary<char, string>() { { 'c', "q6" } } },
            { "q6", new Dictionary<char, string>() { { ' ', "q12" }, { '\t', "q12" }, { '\n', "q12" }, { '\r', "q12" } } },
            { "q7", new Dictionary<char, string>() { { 'x', "q8" } } },
            { "q8", new Dictionary<char, string>() { { 't', "q11" } } },
            { "q9", new Dictionary<char, string>() { { 'd', "q10" } } },
            { "q10", new Dictionary<char, string>() { { 'f', "q11" } } },
            { "q11", new Dictionary<char, string>() { { ' ', "q2" }, { '\t', "q2" }, { '\n', "q2" }, { '\r', "q2" } } }
            };
           
           
            if (activeRichTextBox != null && !string.IsNullOrEmpty(activeRichTextBox.Text))
            {
                
                foreach (char c in text.Skip(1))
                {
                    if (new char[] { '#', '/', ',', ':' }.Contains(c))
                    {
                        fileName = "";
                        currentState = "q0";
                        
                        continue;
                    }
                    if (states[currentState].ContainsKey(c))
                    {
                        currentState = states[currentState][c];
                    }

                    if(currentState == "q1" || currentState == "q2")
                    {
                        if (Char.IsLetter(c) || c == '_')
                        {
                            currentState = "q2";
                        }
                    }
                    else
                    {
                        // Логика для обработки непредусмотренных символов
                    }
                }
            }
            else
            {
                MessageBox.Show("Открытый файл пуст или не содержит текста.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }  
        }



        private void SearchFileNames()
        {
            results.Clear();

            if (tabControl1.TabPages.Count == 0)
            {
                MessageBox.Show("Нет открытых файлов для поиска.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            TabPage activeTab = tabControl1.SelectedTab;
            RichTextBox activeRichTextBox = activeTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            if (activeRichTextBox != null && !string.IsNullOrEmpty(activeRichTextBox.Text))
            {
                string text = activeRichTextBox.Text;
                //Regex regex = new Regex(@"\b\w+\.(doc|docx|txt|pdf)\b", RegexOptions.IgnoreCase);
                //Regex regex = new Regex(@"\b\w+(\.\w+)+\.(doc|docx|txt|pdf)\b", RegexOptions.IgnoreCase);
                Regex regex = new Regex(@"\b\w+(\.\w+)*\.(doc|docx|txt|pdf)\b", RegexOptions.IgnoreCase);
                MatchCollection matches = regex.Matches(text);
                RichTextBox textOutput = activeTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();
                textOutput.Clear();

                //foreach (Match match in matches)
                //{
                //    int state = 0;
                //    string currentMatch = "";
                //    foreach (char c in match.Value)
                //    {
                //        state++;
                //        currentMatch += c;
                //        textOutput.AppendText($"Состояние{state}: {currentMatch}" + Environment.NewLine);
                //    }
                //    int matchStartIndex = match.Index;
                //    int currentLineNumber = text.Substring(0, matchStartIndex).Count(c => c == '\n') + 1;
                //    results.Add(new RegResult(match.Value, match.Index, 0)); // Замените 0 на текущий номер строки
                //}
                foreach (Match match in matches)
                {
                    //RichTextBox textOutput = activeTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();

                    textOutput.AppendText("Совпадение: " + match.Value + Environment.NewLine);

                    int matchStartIndex = match.Index;
                    int matchEndIndex = matchStartIndex + match.Length - 1; // Исправлено

                    int lineStartIndex = text.LastIndexOf('\n', matchStartIndex) + 1;
                    int lineEndIndex = text.IndexOf('\n', matchEndIndex);
                    if (lineEndIndex == -1)
                    {
                        lineEndIndex = text.Length - 1;
                    }

                    int currentLineNumber = text.Substring(0, matchStartIndex).Count(c => c == '\n') + 1;

                    textOutput.AppendText($"Номер строки: {currentLineNumber}, Начало: {matchStartIndex}, Конец: {matchEndIndex}" + Environment.NewLine);
                    results.Add(new RegResult(match.Value, match.Index, currentLineNumber));
                }
            }
            else
            {
                MessageBox.Show("Открытый файл пуст или не содержит текста.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void пускToolStripMenuItem_Click(object sender, EventArgs e)
        {

             //SearchFileNames();
            Falss();
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Вызываем метод обработки события пускToolStripMenuItem_Click для обновления результатов при смене вкладки
            //пускToolStripMenuItem_Click(sender, e);
            if (tabControl1.SelectedTab != null)
            {
                filePath = tabControl1.SelectedTab.Tag.ToString();
            }
            //
        }
    }
}

