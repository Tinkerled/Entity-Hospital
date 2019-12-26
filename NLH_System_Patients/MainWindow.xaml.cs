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

namespace NLH_System_Patients
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void BtnLoginAdmin_Click(object sender, RoutedEventArgs e)
        {
            // ouvrir fenetre Admin
            Prepose FenPrep = new Prepose();
            FenPrep.user = "admin";
            FenPrep.Show();
            this.Close();
        }

        private void BtnLoginMed_Click(object sender, RoutedEventArgs e)
        {
            // ouvrir fenetre Medecin
            Prepose FenPrep = new Prepose();
            FenPrep.user = "med";
            FenPrep.Show();
            this.Close();
        }

        private void BtnLoginPrep_Click(object sender, RoutedEventArgs e)
        {
            // ouvrir fenetre Preposer
            Prepose FenPrep = new Prepose();
            FenPrep.user = "prep";
            FenPrep.Show();
            this.Close();
        }
    }
}
