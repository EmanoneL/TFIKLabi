﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TFIKLabi
{
    public partial class Form1 : Form
    {
        string filePath;
        string standartFileName = "NewFile";
        public Form1()
        {
            InitializeComponent();
            //toolStripMenuItem1.Click += ToolStripMenuItem1_Click;

        }

        private string getNewFileName()
        {
            int count = 0;
            string fileName = standartFileName + count + ".txt";
            while (File.Exists(fileName))
            {
                fileName = $"Новый файл ({count}).txt";
                count++;
            }
            return fileName;

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)    //обработчик создать файл
        {

            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            //saveFileDialog.Title = "Save a file";

            //if (saveFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    filePath = saveFileDialog.FileName;

            //    File.WriteAllText(filePath, string.Empty);

            //    OpenFile(filePath);
            //}
            filePath = getNewFileName();
            OpenFile(filePath);


        }
        private void OpenFile(string filePath)  //открыть файл после создания
        {
            string fileName = Path.GetFileName(filePath);
            TabPage tabPage = new TabPage(fileName);
            try
            {
                

                
                tabControl1.TabPages.Add(tabPage);

                RichTextBox richTextBox = new RichTextBox();
                richTextBox.Dock = DockStyle.Fill;
                tabPage.Controls.Add(richTextBox);
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
                    if (tab.Text == Path.GetFileName(filePath))
                    {
                        MessageBox.Show("Файл уже открыт во вкладке.");
                        return;
                    }
                }

                string fileContent = File.ReadAllText(filePath);
                TabPage tabPage = new TabPage(Path.GetFileName(filePath));
                tabControl1.TabPages.Add(tabPage);

                RichTextBox richTextBox = new RichTextBox();
                richTextBox.Dock = DockStyle.Fill;
                richTextBox.Text = fileContent;
                tabPage.Controls.Add(richTextBox);

                tabPage.Controls.Add(richTextBox);
                tabControl1.SelectedTab = tabPage;

                //richTextBox.TextChanged += RichTextBox_TextChanged;
                //richTextBox.TextChanged += RichTextBox_KeyDown;

            }


        }

        private void RichTextBox_KeyDown(object sender, EventArgs e)
        {
            previousText = ((RichTextBox)sender).Text;
        }


        // Сохранить 
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                string file = tabControl1.SelectedTab.Text;
                if (richTextBox != null)
                {

                    if (System.IO.File.Exists(file))
                    {
                        File.WriteAllText(file, richTextBox.Text);

                    }
                    else
                    {
                        SaveKaK();
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
                        tabControl1.SelectedTab.Text = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
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
            //CloseFile();  
            Application.Exit();
            //  НЕ ЗНАЮ КАК ПРЕДЛОЖИТЬ СОХРАНИТЬ ФАЙЛ, ЕСЛИ НЕ СОХРАНЕН, НО ЕСТЬ ФУНКЦИЯ SaveKaK()
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
            TabControl tabControl = tabControl1;
            if (tabControl.SelectedTab != null)
            {
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
    }
}

