using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace TFIKLabi
{

    public partial class Form1 : Form
    {
        string filePath;
        string standartFileName = "NewFile";
        List<RegResult> results = new List<RegResult> { };
        List<Leksem> leks = new List<Leksem>();
        Sertch sertch;

        //RichTextBox richTextBox1;
        //RichTextBox richTextBox2;

        bool err = false;
        // Преобразованная константа
        string fix = "";
        // Количество ошибок
        int countErrs = 0;
        // Ожидаемые символы
        List<char> numbers = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        bool turnBack ;
        bool open ;
        bool close ;
        public Form1()
        {
            InitializeComponent();
            //toolStripMenuItem1.Click += ToolStripMenuItem1_Click;
            //if (tabControl1.SelectedTab != null)
            //{
            //   var richTextBox1 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            //   var richTextBox2 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();
            //}

        } 
        void E(ref int i)
        {
            var richTextBox1 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            var richTextBox2 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();
            if (i == 0) richTextBox2.Text += "E";
            else richTextBox2.Text += "-E";

            T(ref i);
            A(ref i);

        }
        void A(ref int i)
        {
            var richTextBox1 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            var richTextBox2 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();
            richTextBox2.Text += "-A";
            if (richTextBox1.Text[i].ToString() == "+")
            {
                richTextBox2.Text += "-+";
                i++;
                T(ref i);
                A(ref i);
            }
            else if (richTextBox1.Text[i].ToString() == "-")
            {
                richTextBox2.Text += "-'-'";
                i++;
                T(ref i);
                A(ref i);
            }
            else
            {
                richTextBox2.Text += "-ε";
                if (close)
                {
                    richTextBox2.Text += "-)";
                    open = false;
                    close = false;
                }
                if (err)
                {
                    richTextBox2.Text += "-ER";
                    open = false;
                    close = false;
                    err = false;
                }
                return;
            }
        }
        void T(ref int i)
        {
            var richTextBox1 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            var richTextBox2 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();
            richTextBox2.Text += "-T";
            O(ref i);
            B(ref i);
        }
        void O(ref int i)
        {
            var richTextBox1 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            var richTextBox2 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();
            richTextBox2.Text += "-O";
            string number = "";
            string id = "";
            for (; i < richTextBox1.TextLength; i++)
            {
                if (open && !close && !Char.IsDigit(richTextBox1.Text[i]) && !Char.IsLetter(richTextBox1.Text[i]) && richTextBox1.Text[i] != '+' && richTextBox1.Text[i] != '*'
                    && richTextBox1.Text[i] != '-' && richTextBox1.Text[i] != '/' && richTextBox1.Text[i] != ')' && richTextBox1.Text[i] != '.' && richTextBox1.Text[i] != ',')
                {
                    // Написать для дробного числа (обработать точку)
                    richTextBox2.Text += "-ER";
                    open = false;
                    close = false;
                    turnBack = true;
                    return;
                }
                if (open && !close && richTextBox1.Text[i] == ')' && (number.Length > 0 || id.Length > 0))
                {
                    close = true;
                    if (number.Length > 0) richTextBox2.Text += "-num";
                    else richTextBox2.Text += "-id";
                    return;
                }
                else if (open && !close && richTextBox1.Text[i] == ')' && number.Length == 0 && id.Length == 0)
                {
                    close = true;
                    return;
                }
                if (number.Length > 0)
                {
                    if ((Char.IsDigit(richTextBox1.Text[i]) || richTextBox1.Text[i].ToString() == "," || richTextBox1.Text[i].ToString() == ".") && i != richTextBox1.TextLength - 1)
                    {
                        if (richTextBox1.Text[i].ToString() == "," || richTextBox1.Text[i].ToString() == ".")
                        {
                            if (number.Contains(".") || number.Contains(","))
                            {
                                // Если целое число заканчивается на две точки, необходимо вернуться на 2 символа назад, а если дробное и заканчивается на точку, тогда на 1 символ
                                if (number[number.Length - 1] == '.' || number[number.Length - 1] == ',') i -= 2;
                                else i--;
                                number = "";
                                richTextBox2.Text += "-num";
                                return;
                            }
                            else
                            {
                                number += richTextBox1.Text[i].ToString();

                            }
                        }
                        else
                            number += richTextBox1.Text[i].ToString();
                    }
                    else
                    {
                        if (i == richTextBox1.TextLength - 1 && Char.IsDigit(richTextBox1.Text[i]))
                        {
                            number += richTextBox1.Text[i].ToString();
                            if (open && !close) err = true;
                        }
                        else if (i == richTextBox1.TextLength - 1 && !Char.IsDigit(richTextBox1.Text[i]))
                        {
                            err = true;
                            // Если целое число заканчивается на две точки, необходимо вернуться на 2 символа назад, а если дробное и заканчивается на точку, тогда на 1 символ
                            if (number[number.Length - 1] == '.' || number[number.Length - 1] == ',') i -= 2;
                            else if (number.Contains('.') || number.Contains(',')) i--;

                        }
                        else if (i < richTextBox1.TextLength - 1 && !Char.IsDigit(richTextBox1.Text[i]) && richTextBox1.Text[i] != '*' && richTextBox1.Text[i] != '+'
                            && richTextBox1.Text[i] != '/' && richTextBox1.Text[i] != '-') i--;
                        richTextBox2.Text += "-num";
                        return;
                    }
                }
                else if (id.Length > 0)
                {
                    if ((Char.IsDigit(richTextBox1.Text[i]) || Char.IsLetter(richTextBox1.Text[i])) && i != richTextBox1.TextLength - 1)
                    {
                        id += richTextBox1.Text[i].ToString();
                    }
                    else
                    {
                        if (i == richTextBox1.TextLength - 1 && (Char.IsDigit(richTextBox1.Text[i]) || Char.IsLetter(richTextBox1.Text[i])))
                        {
                            id += richTextBox1.Text[i].ToString();
                            err = true;
                        }
                        id = "";
                        richTextBox2.Text += "-id";
                        return;
                    }
                }
                //Начало числа
                else if (Char.IsDigit(richTextBox1.Text[i]))
                {
                    if (i < richTextBox1.TextLength - 1) number += richTextBox1.Text[i].ToString();
                    else
                    {
                        richTextBox2.Text += "-num";
                        return;
                    }
                }
                //Начало индентификатора
                else if (Char.IsLetter(richTextBox1.Text[i]))
                {
                    if (i < richTextBox1.TextLength - 1) id += richTextBox1.Text[i].ToString();
                    else
                    {
                        richTextBox2.Text += "-id";
                        return;
                    }
                }
                else if (richTextBox1.Text[i] == '(')
                {
                    open = true;
                    richTextBox2.Text += "-(";
                    i++;
                    E(ref i);
                    return;
                }
                else
                {
                    open = false;
                    richTextBox2.Text += "-ER";
                    return;
                }
            }
        }
        void B(ref int i)
        {
            var richTextBox1 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            var richTextBox2 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault(); 
            richTextBox2.Text += "-B";
            if (richTextBox1.Text[i].ToString() == "*")
            {
                richTextBox2.Text += "-*";
                i++;
                O(ref i);
                B(ref i);
            }
            else if (richTextBox1.Text[i].ToString() == "/")
            {
                richTextBox2.Text += "-/";
                i++;
                O(ref i);
                B(ref i);
            }
            else
            {
                richTextBox2.Text += "-ε";
                return;
            }
        }

        void FixErrors(ref int i, char replace, List<char> arr)
        {
            var richTextBox1 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            var richTextBox2 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();
            string temp = "";
            while (!arr.Contains(richTextBox1.Text[i]))
            {
                temp += richTextBox1.Text[i];
                i++;
                if (i == richTextBox1.TextLength) break;
            }
            if (i == richTextBox1.TextLength)
            {
                richTextBox2.Text += "Найдена ошибка :" + temp + " , индекс ошибки: " + (i - temp.Length + 1) + $" Ожидалось число\n";
                replace = '1';
            }
            else
            {
                if (Char.IsDigit(richTextBox1.Text[i]))
                {
                    richTextBox2.Text += "Найдена ошибка :" + temp + " , индекс ошибки: " + (i - temp.Length + 1) + $" Ожидалось число\n";
                    replace = '1';
                }
                else if (richTextBox1.Text[i] == '+')
                {
                    richTextBox2.Text += "Найдена ошибка :" + temp + " , индекс ошибки: " + (i - temp.Length + 1) + $" Ожидался знак +\n";
                    replace = '+';
                    if (i < richTextBox1.TextLength) i++;
                }
                else if (richTextBox1.Text[i] == '-')
                {
                    richTextBox2.Text += "Найдена ошибка :" + temp + " , индекс ошибки: " + (i - temp.Length + 1) + $" Ожидался знак -\n";
                    replace = '-';
                    if (i < richTextBox1.TextLength) i++;
                }
                else if (richTextBox1.Text[i] == 'E')
                {
                    richTextBox2.Text += "Найдена ошибка :" + temp + " , индекс ошибки: " + (i - temp.Length + 1) + $" Ожидалось E\n";
                    replace = 'E';
                    if (i < richTextBox1.TextLength) i++;
                }
            }

            fix += replace;
            countErrs++;
        }
        void FuncCH(ref int i)
        {
            var richTextBox1 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            var richTextBox2 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();
            if (richTextBox1.Text[i] == '+' || richTextBox1.Text[i] == '-')
            {
                fix += richTextBox1.Text[i];
                i++;
            }
            FuncCHBZ(ref i);
        }
        void FuncCHBZ(ref int i)
        {
            var richTextBox1 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            var richTextBox2 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();
            if (richTextBox1.Text[i] == 'E')
            {
                fix += richTextBox1.Text[i];
                i++;
                FuncCEL(ref i);
            }
            else
            {
                FuncDCH(ref i);
                if (i < richTextBox1.TextLength && richTextBox1.Text[i] == 'E')
                {
                    fix += richTextBox1.Text[i];
                    i++;
                    FuncCEL(ref i);
                }
                else if (i < richTextBox1.TextLength && richTextBox1.Text[i] != 'E')
                {
                    numbers = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                    List<char> numbersE = numbers;
                    numbersE.Add('E');
                    FixErrors(ref i, 'E', numbersE);
                    if (i < richTextBox1.TextLength)
                        FuncCEL(ref i);
                }
            }

        }
        void FuncDCH(ref int i)
        {
            var richTextBox1 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            var richTextBox2 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();
            if (richTextBox1.Text[i] == '.')
            {
                fix += richTextBox1.Text[i];
                i++;
                FuncCELBZ(ref i);
            }

            else
            {
                FuncCELBZ(ref i);
            }
        }
        void FuncCEL(ref int i)
        {
            var richTextBox1 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            var richTextBox2 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();
            if (richTextBox1.Text[i] == '+' || richTextBox1.Text[i] == '-')
            {
                fix += richTextBox1.Text[i];
                i++;
            }
            FuncCELBZ(ref i);
        }
        void FuncCELBZ(ref int i)
        {
            var richTextBox1 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            var richTextBox2 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();
            if (Char.IsDigit(richTextBox1.Text[i]))
            {
                while (Char.IsDigit(richTextBox1.Text[i]))
                {
                    fix += richTextBox1.Text[i];
                    i++;
                    if (i == richTextBox1.TextLength) return;
                }
                if (richTextBox1.Text[i] == '.')
                {
                    fix += richTextBox1.Text[i];
                    i++;
                    if (Char.IsDigit(richTextBox1.Text[i]))
                        FuncCELBZ(ref i);
                    else if (richTextBox1.Text[i] != 'E')
                    {
                        FixErrors(ref i, '1', numbers);
                        FuncCELBZ(ref i);
                    }
                }
                else if (richTextBox1.Text[i] != 'E' && richTextBox1.Text[i] != '+' && richTextBox1.Text[i] != '-')
                {
                    numbers = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                    List<char> plusMinus = numbers;
                    plusMinus.AddRange(new char[] { '+', '-', 'E' });
                    FixErrors(ref i, '+', plusMinus);
                    if (i < richTextBox1.TextLength)
                        FuncCELBZ(ref i);
                }
            }
            else if (richTextBox1.Text[i] != 'E' && richTextBox1.Text[i] != '+' && richTextBox1.Text[i] != '-')
            {
                numbers = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                List<char> plusMinus = numbers;
                plusMinus.AddRange(new char[] { '+', '-', 'E' });
                FixErrors(ref i, '+', plusMinus);
                if (i < richTextBox1.TextLength)
                    FuncCELBZ(ref i);
            }
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
                richTextBox2.Height = 300;
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
                richTextBox2.Height = 300;
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


        private void пускToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var RichTextBox1 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            var RichTextBox2 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();

            try
            {
                sertch = new Sertch(RichTextBox1.Text);
                RichTextBox2.Text = sertch.GetResult();
            }
            catch (Exception ex)
            {
                //RichTextBox2.Clear();
                MessageBox.Show($"Ошибка записи файлов! - {ex.Message}", "Ошибка!", MessageBoxButtons.OK);
            }
            //SearchFileNames();
            //Falss();
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

        private void постановкаЗадачиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string filePath = @"task.html";

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

        private void лексическийАнализаторToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var richTextBox1 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            var richTextBox2 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();

            richTextBox2.Clear();
            richTextBox2.Enabled = true;
            string[] type = new string[4] { "Число", "Скобка", "Знак", "Идентификатор" };
            List<char> skobka = new List<char>() { ')', '(', '{', '}', '[', ']', '<', '>', '|' };
            List<char> znak = new List<char>() { '!', ',', '.', ':', ';', '?', '-', '_', '+', '=', '*', '%', '~', '/', '\\', '&' };
            bool flag = false; // для обозначения была ли уже точка или запятая в строке число.
            int lineIndex = 0;
            int startPositionInStr = 0;
            int startPositionOfWord = 0;
            int endPositionOfWord = 0;

            string identif = "";
            string chislo = "";
            for (int i = 0; i < richTextBox1.TextLength; i++)
            {
                //Продолжение записи числа
                if (chislo.Length > 0)
                {
                    if ((Char.IsDigit(richTextBox1.Text[i]) || richTextBox1.Text[i].ToString() == "," || richTextBox1.Text[i].ToString() == ".") && i != richTextBox1.TextLength - 1)
                    {
                        if (richTextBox1.Text[i].ToString() == "," || richTextBox1.Text[i].ToString() == ".")
                        {
                            if (flag == false)
                            {
                                chislo += richTextBox1.Text[i].ToString();
                                flag = true;
                            }
                            else
                            {
                                endPositionOfWord = startPositionOfWord + chislo.Length;
                                richTextBox2.Text += "Номер строки: " + (lineIndex + 1) + " Начало: " + (startPositionOfWord + 1) + " Конец: " + endPositionOfWord;
                                richTextBox2.Text += " Тип: " + type[0] + ", Лексема: " + chislo + "\n";
                                Leksem a = new Leksem(type[0], chislo, startPositionOfWord + 1, endPositionOfWord, lineIndex + 1);
                                leks.Add(a);
                                chislo = "";
                                flag = false;
                            }
                        }
                        else
                            chislo += richTextBox1.Text[i].ToString();
                    }
                    else
                    {
                        if (i == richTextBox1.TextLength - 1)
                        {
                            chislo += richTextBox1.Text[i].ToString();
                        }
                        if (chislo[chislo.Length - 1] == '.' || chislo[chislo.Length - 1] == ',')
                        {
                            chislo.Remove(chislo.Length);
                        }
                        endPositionOfWord = startPositionOfWord + chislo.Length;
                        richTextBox2.Text += "Номер строки: " + (lineIndex + 1) + " Начало: " + (startPositionOfWord + 1) + " Конец: " + endPositionOfWord;
                        richTextBox2.Text += " Тип: " + type[0] + ", Лексема: " + chislo + "\n";
                        Leksem a = new Leksem(type[0], chislo, startPositionOfWord + 1, endPositionOfWord, lineIndex + 1);
                        leks.Add(a);
                        chislo = "";
                        flag = false;
                    }
                }
                //Продолжение записи идентификатора
                else if (identif.Length > 0)
                {
                    if ((Char.IsDigit(richTextBox1.Text[i]) || Char.IsLetter(richTextBox1.Text[i])) && i != richTextBox1.TextLength - 1)
                    {
                        identif += richTextBox1.Text[i].ToString();
                    }
                    else
                    {
                        if (i == richTextBox1.TextLength - 1)
                        {
                            identif += richTextBox1.Text[i].ToString();
                        }
                        endPositionOfWord = startPositionOfWord + identif.Length;
                        richTextBox2.Text += "Номер строки: " + (lineIndex + 1) + " Начало: " + (startPositionOfWord + 1) + " Конец: " + endPositionOfWord;
                        richTextBox2.Text += " Тип: " + type[3] + ", Лексема: " + identif + "\n";
                        Leksem a = new Leksem(type[3], identif, startPositionOfWord + 1, endPositionOfWord, lineIndex + 1);
                        leks.Add(a);
                        identif = "";
                    }
                }
                //Начало числа
                else if (Char.IsDigit(richTextBox1.Text[i]))
                {
                    lineIndex = richTextBox1.GetLineFromCharIndex(i);
                    startPositionInStr = richTextBox1.GetFirstCharIndexFromLine(lineIndex);
                    startPositionOfWord = i - startPositionInStr;
                    chislo += richTextBox1.Text[i].ToString();
                }
                //Начало индентификатора
                else if (Char.IsLetter(richTextBox1.Text[i]))
                {
                    lineIndex = richTextBox1.GetLineFromCharIndex(i);
                    startPositionInStr = richTextBox1.GetFirstCharIndexFromLine(lineIndex);
                    startPositionOfWord = i - startPositionInStr;
                    identif += richTextBox1.Text[i].ToString();
                }
                else if (skobka.Contains(richTextBox1.Text[i]))
                {
                    lineIndex = richTextBox1.GetLineFromCharIndex(i);
                    startPositionInStr = richTextBox1.GetFirstCharIndexFromLine(lineIndex);
                    startPositionOfWord = i - startPositionInStr;
                    endPositionOfWord = startPositionOfWord;
                    richTextBox2.Text += "Номер строки: " + (lineIndex + 1) + " Начало: " + (startPositionOfWord + 1) + " Конец: " + (endPositionOfWord + 1);
                    richTextBox2.Text += " Тип: " + type[1] + ", Лексема: " + richTextBox1.Text[i] + "\n";
                    Leksem a = new Leksem(type[1], richTextBox1.Text[i].ToString(), startPositionOfWord + 1, endPositionOfWord + 1, lineIndex + 1);
                    leks.Add(a);
                }
                else if (znak.Contains(richTextBox1.Text[i]))
                {
                    lineIndex = richTextBox1.GetLineFromCharIndex(i);
                    startPositionInStr = richTextBox1.GetFirstCharIndexFromLine(lineIndex);
                    startPositionOfWord = i - startPositionInStr;
                    endPositionOfWord = startPositionOfWord;
                    richTextBox2.Text += "Номер строки: " + (lineIndex + 1) + " Начало: " + (startPositionOfWord + 1) + " Конец: " + (endPositionOfWord + 1);
                    richTextBox2.Text += " Тип: " + type[2] + ", Лексема: " + richTextBox1.Text[i] + "\n";
                    Leksem a = new Leksem(type[2], richTextBox1.Text[i].ToString(), startPositionOfWord + 1, endPositionOfWord + 1, lineIndex + 1);
                    leks.Add(a);
                }
            }


        }

        private void сToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var richTextBox1 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            var richTextBox2 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();
            richTextBox2.Clear();
            turnBack = false;
            open = false;
            close = false;
            for (int i = 0; i < richTextBox1.TextLength; i++)
            {
                if (turnBack)
                {
                    i--;
                    turnBack = false;
                }
                E(ref i);

            }
        }



        private void анализаторToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var richTextBox1 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            var richTextBox2 = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().Skip(1).FirstOrDefault();
            richTextBox2.Clear();
            fix = "";
            countErrs = 0;
            for (int i = 0; i < richTextBox1.TextLength; i++)
            {
                FuncCH(ref i);
            }
            richTextBox2.Text += $"Общее количество ошибок: {countErrs} \n Ожидаемая константа: {fix}";
        }
    }
}

