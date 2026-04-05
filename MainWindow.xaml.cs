using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Diagnostics;

namespace SmartLauncher_Filemanager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Логика нажатия на кнопку
        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            // Создаем стандартное окно выбора файла (Проводник)
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Если пользователь выбрал файл и нажал "Открыть"
            if (openFileDialog.ShowDialog() == true)
            {
                // Добавляем путь к файлу в наш список на экране
                FilesListBox.Items.Add(openFileDialog.FileName);
            }
        }


        //ДАЛЕЕ ИДЕТ НЕЙРОКОД, НАДО БУДЕТ ИСПРАВИТЬ
        private void FilesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Проверяем, выбран ли какой-то файл в списке
            if (FilesListBox.SelectedItem != null)
            {
                string filePath = FilesListBox.SelectedItem.ToString();

                // Запускаем файл стандартной программой Windows
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
        }

    }
}