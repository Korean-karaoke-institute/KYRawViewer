using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kyRawViewer
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow1.Title = "로드된 이미지 없음";

            //loadKyRawImage("id0140.raw");

           
        }

        private void loadKyRawImage(string filename)
        {
            
            using (BinaryReader reader = new BinaryReader(File.OpenRead(filename)))
            {
                reader.BaseStream.Seek(0xc, SeekOrigin.Begin);
                int width = reader.ReadInt32();
                int height = reader.ReadInt32();

                Console.WriteLine($"Size: {width}x{height}");
                mainWindow1.Title = $"{filename} [{width}x{height}]";

                Color[] colors = new Color[width * height];
                reader.BaseStream.Seek(0xe0, SeekOrigin.Begin);

             
                WriteableBitmap writeableBitmap = new WriteableBitmap(width,height, 96, 96, PixelFormats.Bgra32, null);
                writeableBitmap.WritePixels(new Int32Rect(0, 0, width, height), reader.ReadBytes((width * height)*4), width * 4, 0);


                MyImage.Source = writeableBitmap;

            }
        }

        private byte[] ConvertColorsToByteArray(Color[] colors)
        {
            byte[] bytes = new byte[colors.Length * 4];

            for (int i = 0; i < colors.Length; i++)
            {
                bytes[i * 4] = colors[i].B;
                bytes[i * 4 + 1] = colors[i].G;
                bytes[i * 4 + 2] = colors[i].R;
                bytes[i * 4 + 3] = colors[i].A;
            }

            return bytes;
        }

        private void MyImage_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var file = files[0];
                loadKyRawImage(file);
            }
        }

        private void MyImage_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) 
            {
                
            }
            e.Handled = true;
        }
    }
}
