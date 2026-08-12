using System.Windows;
using System.Windows.Controls;

namespace AccountingSystem.Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                TxtStatus.Text = $"تم فتح قسم: {button.Content}";
            }
        }
    }
}
