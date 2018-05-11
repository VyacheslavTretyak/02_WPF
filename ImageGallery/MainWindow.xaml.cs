using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace _02_WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private List<string> imageFormats;
		public MainWindow()
		{
			imageFormats = new List<string>() { ".jpg", ".jpe", ".png", ".bmp", ".gif" };
			InitializeComponent();			
			btnOpen.Click += BtnOpen_Click;
			listBox.SelectionChanged += ListBox_SelectionChanged;
			slider.ValueChanged += Slider_ValueChanged;		
		}

		private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{			
			image.Width = slider.Value * (image.Parent as System.Windows.Controls.Control).ActualWidth * 0.9;			
		}

		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count != 0)
			{
				SetNewImage();
			}
		}

		private void BtnOpen_Click(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog openDialog = new FolderBrowserDialog();
			if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				LoadImages(openDialog.SelectedPath);
			}
		}

		private void LoadImages(string fileName)
		{
			if (listBox.Items.Count > 0)
			{
				listBox.Items.Clear();
			}
			string[] files = Directory.GetFiles(fileName);
			if(files.Length == 0)
			{
				return;
			}
			List<string> imgFiles = new List<string>();
			foreach (string file in files)
			{
				FileInfo info = new FileInfo(file);
				if (imageFormats.Contains(info.Extension.ToLower()))
				{
					imgFiles.Add(file);
				}
			}
			progressBar.Visibility = Visibility.Visible;
			progressBar.Maximum = files.Length;
			progressBar.Minimum = 0;
			progressBar.Value = 0;
			int value = 0;
			if (imgFiles.Count == 0)
			{
				return;
			}
			foreach (string file in imgFiles)
			{
				try
				{
					ListBoxItem item = new ListBoxItem();
					Image image = new Image();
					image.Source = new BitmapImage(new Uri(file));
					image.Width = 200;
					item.Content = image;
					listBox.Items.Add(item);
				}
				catch
				{

				}
				progressBar.Dispatcher.Invoke(() => progressBar.Value = value++, DispatcherPriority.Background);
			}
			progressBar.Visibility = Visibility.Hidden;
			listBox.SelectedIndex = 0;
		}		
		private void SetNewImage()
		{
			slider.Value = 1;
			Image img = (listBox.SelectedItem as ListBoxItem).Content as Image;
			image.Source = img.Source;
			image.Width = slider.Value * (image.Parent as System.Windows.Controls.Control).ActualWidth * 0.9;			
			expander.Text = $"Name : {img.Source.ToString()}\nSize : {(int)img.Source.Width}x{(int)img.Source.Height}\n";
		}
		private void Left_Click(object sender, ExecutedRoutedEventArgs e)
		{
			if (listBox.SelectedIndex > 0)
			{
				listBox.SelectedIndex--;
				SetNewImage();
				
			}
		}
		private void Right_Click(object sender, ExecutedRoutedEventArgs e)
		{
			if (listBox.SelectedIndex < listBox.Items.Count - 1)
			{
				listBox.SelectedIndex++;
				SetNewImage();
			}
		}
	}
}