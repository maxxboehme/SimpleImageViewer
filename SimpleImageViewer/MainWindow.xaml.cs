using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Text;
using System.Windows.Forms;

namespace SimpleImageViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> mSupportedExtensions;

        public ObservableCollection<Uri> Photos;

        public MainWindow()
        {
            InitializeComponent();
            Photos = new ObservableCollection<Uri>();
            mSupportedExtensions = new List<string>()
            {
                ".bmp",
                ".jpg",
                ".gif",
                ".jpe",
                ".jpeg",
                ".jfif",
                ".tif",
                "tiff",
                ".png"
            };
            this.DataContext = Photos;
        }

        private void Gallery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Uri selected = (Uri)Gallery.SelectedItem;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = selected;
            bitmap.EndInit();
            SelectedImageViewer.Source = bitmap;
        }

        private void btnLoadImages_click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadImages(dialog.SelectedPath, true);
            }
        }

        public void LoadImages(string pathLocation, bool deepSearch)
        {
            Photos.Clear();
            SearchOption opt = SearchOption.TopDirectoryOnly;
            if (deepSearch)
                opt = SearchOption.AllDirectories;


            foreach (var file in new DirectoryInfo(pathLocation).GetFiles("*", opt))
            {
                AddImage(file.FullName);
            }
        }

        public void AddImage(string location)
        {
            if (!Photos.Any(x => x.AbsolutePath == location))
            {
                FileInfo file = new FileInfo(location);
                if (IsImage(file.Extension))
                {
                    Photos.Add(new Uri(location));
                }
            }
        }

        private bool IsImage(string extension)
        {
            if (mSupportedExtensions.Contains(extension.ToLower())) return true;
            return false;
        }
    }
}
