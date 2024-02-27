using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Controls.Primitives;

namespace Prakticheskaya_2
{
    public partial class MainWindow : Window
    {
        Notebook notebook = new Notebook();
        List<Notebook> NotebookList = new List<Notebook>();
        public MainWindow()
        {
            InitializeComponent();
            DeserializeJsonFile();
            DateChoice.Text = DateTime.Now.ToString();
            ShowItemsInListBox();
        }
        
        public void SerializeJsonFile()
        {
            JSONchik.JsonSerialize(NotebookList);
        }

        public void ShowItemsInListBox()
        {
            DeserializeJsonFile();
            string selectedDate = DateChoice.Text;
            var filteredNotebooks = NotebookList.Where(notebook => notebook.DateOfCreation == selectedDate);
            ListBox.ItemsSource = filteredNotebooks.Select(item => $"#{item.ID}: {item.NameOfNotebook}");
        }

        public void DeserializeJsonFile()
        {
            string PathToDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"\\Notebooks.json";
            if (File.Exists(PathToDesktop))
            {
                if (File.ReadAllText(PathToDesktop) != "")
                {
                    NotebookList = JSONchik.JsonDeserialize<List<Notebook>>();
                }
            }
            else
            {
                File.Create(PathToDesktop);
                MessageBox.Show("Был создал json файл для работы программы, зайдите еще раз");
            }
        }

        private void CreateNotebook_Click(object sender, RoutedEventArgs e)
        {
            DeserializeJsonFile();
            notebook.ID = NotebookList.Count+1;
            notebook.DateOfCreation = DateChoice.Text;
            notebook.NameOfNotebook = TextBoxName.Text;
            notebook.DescriptionOfNotebook = TextBoxDescription.Text;
            NotebookList.Add(notebook);
            SerializeJsonFile();
            MessageBox.Show("Заметка создана");
            ShowItemsInListBox();
            TextBoxName.Text = "";
            TextBoxDescription.Text = "";
        }
        private void DeleteNotebook_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox.SelectedItem != null)
            {
                string selectedItem = ListBox.SelectedItem.ToString();
                int idStartIndex = selectedItem.IndexOf("#") + 1;
                int idEndIndex = selectedItem.IndexOf(":");
                string idString = selectedItem.Substring(idStartIndex, idEndIndex - idStartIndex).Trim();
                int id = int.Parse(idString);

                Notebook notebookToRemove = NotebookList.FirstOrDefault(notebook => notebook.ID == id);
                if (notebookToRemove != null)
                {
                    NotebookList.Remove(notebookToRemove);
                    int index = id - 1;
                    foreach (var notebook in NotebookList.GetRange(index, NotebookList.Count - index))
                    {
                        notebook.ID -= 1;
                    }

                    SerializeJsonFile();
                    MessageBox.Show("Заметка удалена");
                    ShowItemsInListBox();
                    TextBoxName.Text = "";
                    TextBoxDescription.Text = "";
                }
            }
        }
        private void SaveNotebook_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox.SelectedItem != null)
            {
                string selectedItem = ListBox.SelectedItem.ToString();
                int idStartIndex = selectedItem.IndexOf("#") + 1;
                int idEndIndex = selectedItem.IndexOf(":");
                string idString = selectedItem.Substring(idStartIndex, idEndIndex - idStartIndex).Trim();
                int id = int.Parse(idString);

                Notebook notebookToSave = NotebookList.FirstOrDefault(notebook => notebook.ID == id);
                if (notebookToSave != null)
                {
                    notebookToSave.NameOfNotebook = TextBoxName.Text;
                    notebookToSave.DescriptionOfNotebook = TextBoxDescription.Text;
                    SerializeJsonFile(); // Сохраняем изменения в файл JSON
                    MessageBox.Show("Заметка сохранена");
                    ShowItemsInListBox();
                }
            }
        }
        private void DateChoice_SelectedDateChanged(object sender, SelectionChangedEventArgs e) //zbs
        {
            ShowItemsInListBox();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBox.SelectedItem != null)
            {
                string selectedItem = ListBox.SelectedItem.ToString();
                int idStartIndex = selectedItem.IndexOf("#") + 1;
                int idEndIndex = selectedItem.IndexOf(":");
                string idString = selectedItem.Substring(idStartIndex, idEndIndex - idStartIndex).Trim();
                int id = int.Parse(idString);

                Notebook selectedNotebook = NotebookList.FirstOrDefault(notebook => notebook.ID == id);
                if (selectedNotebook != null)
                {
                    TextBoxName.Text = selectedNotebook.NameOfNotebook;
                    TextBoxDescription.Text = selectedNotebook.DescriptionOfNotebook;
                }
            }
        }
        public void ShowAllNotebooks()
        {
            ListBox.ItemsSource = NotebookList.Select(item => $"#{item.ID}: Название заметки: {item.NameOfNotebook} | Дата создания: {item.DateOfCreation}");
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox.IsChecked == true)
            {
                DeserializeJsonFile();
                ShowAllNotebooks();
            }
            else
            {
                ShowItemsInListBox();
            }
        }
    }
}
