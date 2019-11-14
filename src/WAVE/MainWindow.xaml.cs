using System.Windows;
using WAVE.ViewModel;

namespace WAVE
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
    }
}