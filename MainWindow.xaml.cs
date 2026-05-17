using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace SmartLauncher_Filemanager
{
    // Модель теперь уведомляет WPF об изменениях свойств (имени и иконки)
    public class AppInfo : INotifyPropertyChanged
    {
        private string _name;
        private ImageSource _icon;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Path { get; set; }

        public ImageSource Icon
        {
            get => _icon;
            set { _icon = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public partial class MainWindow : Window
    {
        // Используем ObservableCollection вместо прямой работы с Items
        public ObservableCollection<AppInfo> Apps { get; set; } = new ObservableCollection<AppInfo>();

        private AppInfo _editingApp; // Переменная для хранения редактируемого элемента
        private ImageSource _tempNewIcon; // Временное хранилище новой иконки

        public MainWindow()
        {
            InitializeComponent();
            FilesListBox.ItemsSource = Apps; // Привязываем коллекцию к списку
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Applications (*.exe)|*.exe|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                var newApp = new AppInfo
                {
                    Path = filePath,
                    Name = System.IO.Path.GetFileNameWithoutExtension(filePath),
                    Icon = ExtractIcon(filePath)
                };

                Apps.Add(newApp); // Добавляем в коллекцию
            }
        }

        private void FilesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (FilesListBox.SelectedItem is AppInfo selectedApp)
            {
                try
                {
                    if (File.Exists(selectedApp.Path))
                    {
                        Process.Start(new ProcessStartInfo { FileName = selectedApp.Path, UseShellExecute = true });
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        // КЛИК ПКМ: Открытие диалогового окна редактирования
        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (FilesListBox.SelectedItem is AppInfo selectedApp)
            {
                _editingApp = selectedApp;
                _tempNewIcon = selectedApp.Icon; // Копируем текущую иконку во временную переменную
                EditNameTextBox.Text = selectedApp.Name; // Заполняем поле текущим именем

                EditDialogHost.IsOpen = true; // Показываем окно
            }
        }

        // ВНУТРИ ДИАЛОГА: Кнопка выбора новой иконки (любой картинки)
        private void ChangeIcon_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images|*.png;*.jpg;*.jpeg;*.ico;*.bmp|Apps|*.exe";

            if (openFileDialog.ShowDialog() == true)
            {
                if (openFileDialog.FileName.ToLower().EndsWith(".exe"))
                {
                    _tempNewIcon = ExtractIcon(openFileDialog.FileName);
                }
                else
                {
                    // Загружаем обычное изображение png/jpg
                    _tempNewIcon = new BitmapImage(new Uri(openFileDialog.FileName));
                }
            }
        }

        // ВНУТРИ ДИАЛОГА: Кнопка "Сохранить"
        private void SaveEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_editingApp != null)
            {
                _editingApp.Name = EditNameTextBox.Text; // Применит новое имя
                _editingApp.Icon = _tempNewIcon;         // Применит новую иконку
            }
            EditDialogHost.IsOpen = false; // Закрываем окно
        }

        // ВНУТРИ ДИАЛОГА: Кнопка "Отмена"
        private void CancelEdit_Click(object sender, RoutedEventArgs e)
        {
            EditDialogHost.IsOpen = false; // Просто закрываем окно без изменений
        }

        private ImageSource ExtractIcon(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return null;
                using (System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(filePath))
                {
                    if (icon == null) return null;
                    BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                        icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    bitmapSource.Freeze();
                    return bitmapSource;
                }
            }
            catch { return null; }
        }

        private bool _isPlaying = false; // Состояние плеера (играет или нет)

        // Клик на кнопку папки: выбор музыкального файла
        private void OpenAudio_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio Files|*.mp3;*.wav;*.wma;*.m4a;*.mp4|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                AudioPlayer.Source = new Uri(openFileDialog.FileName);
                TrackNameTextBlock.Text = System.IO.Path.GetFileName(openFileDialog.FileName);

                // Сразу запускаем после выбора
                AudioPlayer.Play();
                PlayPauseButton.Content = "⏸";
                _isPlaying = true;
            }
        }

        // Клик на кнопку Воспроизведение / Пауза
        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (AudioPlayer.Source == null) return;

            if (_isPlaying)
            {
                AudioPlayer.Pause();
                PlayPauseButton.Content = "▶";
            }
            else
            {
                AudioPlayer.Play();
                PlayPauseButton.Content = "⏸";
            }

            _isPlaying = !_isPlaying; // Меняем флаг состояния на противоположный
        }

        // Клик на кнопку Стоп
        private void StopAudio_Click(object sender, RoutedEventArgs e)
        {
            if (AudioPlayer.Source == null) return;

            AudioPlayer.Stop();
            PlayPauseButton.Content = "▶";
            _isPlaying = false;
        }

    }
}
