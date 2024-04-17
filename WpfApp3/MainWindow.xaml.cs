using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace WpfApp3
{
    public partial class MainWindow : Window
    {
        // Список для хранения строк из файла
        private List<string> _lines;

        // Список для хранения объектов типа GrammarRule
        private List<GrammarRule> _grammarRules;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Обработчик события нажатия кнопки загрузки файла
        private void FileLoad_Click(object sender, RoutedEventArgs e)
        {
            // Проверка, что текстовое поле пустое
            if (string.IsNullOrEmpty(InputTextBox.Text) == false)
            {
                MessageBox.Show("InputTextBox is not empty!");
                return;
            }

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

                    // Обработка строк для создания объектов GrammarRule
                    _grammarRules = MainClass.SplitLinesForGrammarRule(_lines); 
                    
                    // Добавление строк в InputTextBox
                    InputTextBox.Text = string.Join("\n", _lines);
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Ошибка при чтении файла: {ex.Message}");
                }
            }
        }

        // Обработчик события нажатия кнопки Start
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            _lines = new List<string>();

            try
            {
                // Считывание строк из InputTextBox и добавление их в список
                _lines = InputTextBox.Text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                // Обработка строк для создания объектов GrammarRule
                _grammarRules = MainClass.SplitLinesForGrammarRule(_lines); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Пустой метод для обработки данных после нажатия кнопки Start
        private void StartProcessing()
        {
            // Ваша логика обработки здесь
        }
    }
}