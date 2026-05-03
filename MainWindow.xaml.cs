using System;
using System.Diagnostics;
using System.Drawing; // Нужно добавить в ссылках, если будет ругаться
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Microsoft.Win32;

namespace SmartLauncher_Filemanager
{
    // Класс, который описывает наше приложение в списке
    public class AppInfo
    {
        public string Name { get; set; }      // Имя (например, "Steam")
        public string Path { get; set; }      // Путь к .exe
        public ImageSource Icon { get; set; } // Картинка иконки
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Applications (*.exe)|*.exe|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                // Создаем новый объект приложения
                var newApp = new AppInfo
                {
                    Path = filePath,
                    Name = System.IO.Path.GetFileNameWithoutExtension(filePath), // Берем имя файла без .exe
                    Icon = ExtractIcon(filePath) // Вытаскиваем иконку
                };

                FilesListBox.Items.Add(newApp);
            }
        }

        // Метод для вытягивания иконки из .exe файла
        private ImageSource ExtractIcon(string filePath)
        {
            try
            {
                // Явно указываем System.Drawing.Icon, чтобы не было конфликта с WPF типами
                using (System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(filePath))
                {
                    if (icon == null) return null;

                    return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                        icon.Handle,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                }
            }
            catch
            {
                return null;
            }
        }


        private void FilesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (FilesListBox.SelectedItem is AppInfo selectedApp)
            {
                try
                {
                    Process.Start(new ProcessStartInfo(selectedApp.Path) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }
    }
}
