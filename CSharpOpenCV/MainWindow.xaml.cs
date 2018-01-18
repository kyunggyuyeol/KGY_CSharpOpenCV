using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSharpOpenCV
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        VideoCapture cap;
        WriteableBitmap wb;
        const int frameWidth = 640;
        const int frameHeight = 480;
        bool loop = false;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (InitWebCamera())
            {
                MessageBox.Show("웹캠켜짐.");
            }
            else
            {
                MessageBox.Show("웹캠에 문제가 있습니다.");
            }
        }

        private bool InitWebCamera()
        {
            try
            {
                cap = VideoCapture.FromCamera(CaptureDevice.Any, 0);
                cap.FrameWidth = frameWidth;
                cap.FrameHeight = frameHeight;
                cap.Open(0);
                wb = new WriteableBitmap(cap.FrameWidth, cap.FrameHeight, 96, 96, PixelFormats.Bgr24, null);
                imagedata.Source = wb;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            loop = false;
            if (cap.IsOpened())
            {
                cap.Dispose();
            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Mat frame = new Mat();
            Cv2.NamedWindow("1", WindowMode.AutoSize);
            loop = true;
            while (loop)
            {
                if (cap.Read(frame))
                {
                    Cv2.ImShow("1", frame);
                    WriteableBitmapConverter.ToWriteableBitmap(frame, wb);
                    imagedata.Source = wb;
                }

                int c = Cv2.WaitKey(10);
                if (c != -1)
                    break;

            }
        }
    }
}
