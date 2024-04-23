using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using ClassLeftRecurs;
using System.Windows.Controls;
using System.Text;

namespace WpfApp3
{
    public partial class MainWindow : Window
    {
        // Список для хранения строк из файла
        private List<string> _lines;

        // Список для хранения объектов типа GrammarRule
        private Dictionary<string, List<string>> _grammarRules;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Обработчик события нажатия кнопки загрузки файла
        private void FileLoad_Click(object sender, RoutedEventArgs e)
        {
            
            InputTextBox.Text = "";
            // Создание диалогового окна для выбора файла
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                InitialDirectory = @"C:\"
            };

            // Показ диалогового окна и обработка выбранного файла
            if (openFileDialog.ShowDialog() == true)
            {
                _lines = new List<string>();

                try
                {
                    // Считывание строк из выбранного файла
                    using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            _lines.Add(line);
                        }
                    }

                    
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Ошибка чтения файла. Файл пуст или повреждён");
                }
                
                // Добавление строк в InputTextBox
                InputTextBox.Text = string.Join("\n", _lines);
            }
        }

        // Обработчик события нажатия кнопки Start
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            _lines = new List<string>();

            try
            {
                if(InputTextBox.Text.Trim().Length> 0)
                {

                    // Обработка строк для создания объектов GrammarRule
                    List<ClassLeftRecurs.GrammarRule> grammarRules = GrammarOperation.SplitLinesForGrammarRule(_lines);



                    // Считывание строк из InputTextBox и добавление их в список
                    _lines = InputTextBox.Text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    // Обработка строк для создания объектов GrammarRule
                    _grammarRules = GrammarOperation.GetGrammarRules(_lines);
                    string str;
                    foreach (var key in _grammarRules)
                    {
                        str = "";
                        OutputTextBox.Text += key.Key.ToString() + " -> ";
                        foreach (var rules in key.Value)
                        {
                            str += rules.ToString() + "|";
                        }
                        StringBuilder sb = new StringBuilder(str);
                        sb.Length--;
                        //str.Remove(str.Length- 1);
                            //foreach(var rule in rules)
                           
                        
                        // OutputTextBox.Text.Remove(OutputTextBox.Text.Length - 1, 2);
                        OutputTextBox.Text += sb.ToString()+"\n";

                    }
                }
                else
                {
                    MessageBox.Show($"Ошибка! Вы не ввели данные");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка! Неверно задана грамматика. При задании грамматики следуйте следующим правилам:\n 1. Нетерминалы должны быть заглавными буквами латинского алфавита\n2.В левой части должен стоять только нетерминал\n3.Переход из нетерминала в цепочкку обозначайте следующим сочетанием символов '->'\n4.Разделяйте правила прямой чертой ('|')");

            }
        }
        // Пустой метод для обработки данных после нажатия кнопки Start
        private void StartProcessing()
        {

            
        }

        private void FileUpLoad_Click(object sender, RoutedEventArgs e)
        {
            /*SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*";
            saveFileDialog.ShowDialog();
            if (DialogResult.)
            {
                StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName);
                streamWriter.WriteLine(OutputTextBox.Text);
                streamWriter.Close();
            }*/


        }

        private void QA_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Что такое левая рекурсия?\r\n\r\nПравило вида A → Aγ в КС-грамматике называется леворекурсивным, и тогда говорят, что в грамматике имеется левая рекурсия. \r\n----------------------------------------------------------" +
                "\r\nАлгоритм удаления левой рекурсии\r\n\r\nЗапишем все правила вывода из A в виде: A→Aα1∣…∣Aαn∣β1∣…∣βm, где α — непустая последовательность терминалов инетерминалов(α↛ε); β — непустая последовательность терминалов и нетерминалов, не начинающаяся с A." +
                "\r\nЗаменим правила вывода из A на A→β1A′∣… ∣βmA′∣β1∣…∣βm.\r\nСоздадим новый нетерминал A′→α1A′∣…∣αnA′∣α1∣…∣αn.\r\n1.Упорядочить множество нетерминалов произвольным образом \r\n2.Для первого нетерминала удалить непосредственную левую рекурсию\r" +
                "\n3.Для остальных нетерминалов последовательно применять алгоритмы устранения произвольной и непосредственной рекурсии\r\n\r\n\r\nУдаление непосредственной левой рекурсии:\r" +
                "\n1. Представить грамматику в виде множества правил вида A -> Aα | β, где A - нетерминал, α и β - строки терминалов и нетерминалов.\r\n2. Для каждого нетерминала A с глубокой левой рекурсией создать новый нетерминал A' и замените правила вида A -> Aα | β на A -> βA', A' -> αA' | ε" +
                "\r\nУдаление произвольной левой рекурсии:\r\n1. Все правила вида Ai→Ajγ, где j<i заменить на Ai→δ1γ∣…∣δkγ, где Aj→δ1∣…∣δk\r\n2. Устранить непосредственную рекурсию в получившихся правилах.\r\n----------------------------------------------------------" +
                "\r\nПравила задания КС-граймматики\r\n\r\n1.Нетерменалы должны быть представлены заглавными буквами латинского алфавита\r\n2.Правила задаются в виде 'А->sdF|rtA', где A и F-нетерминалы(см. п.1). При этом каждое новое правило задаётся с новой строки\r\n" +
                "2.1 Переход нетерминала в образующиеся цепочки обозначается \"стрелкой\" ('->')\r\n2.2 Правила разделяются с помощью символа '|'\r\n----------------------------------------------------------\r\nВзаимодействие с интерфейсом\r\n\r\n1.Чтобы получить КС-грамматику без левой рекурсии с помощью этого приложения, " +
                "необходимо задать грамматику в соответствующее поле ввода (вручную или с помощью файла(кнопка \"Загрузить из файла\").\r\n2. После шага 1 нужно нажать на кнопку со стрелкой, расположенную между полем ввода и вывода.\r\n3.В поле \"Результат\" будет выведена КС-грамматика без левой рекурсии.\r\n" +
                "4. Чтобы очистить любое из полей, можно нажать кнопку \"Очистить\"\r\n\t", "Справка",MessageBoxButton.OK,MessageBoxImage.Question);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            InputTextBox.Text = "";
        }

        private void ClearOut_Click(object sender, RoutedEventArgs e)
        {
            OutputTextBox.Text = "";
        }
    }
}