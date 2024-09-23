using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace DIV_Protos
{
    /// <summary>
    /// Interaction logic for AGRegistration.xaml
    /// </summary>
    public partial class AGRegistration : UserControl
    {
        public AGRegistration()
        {
            InitializeComponent();
        }
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            if ((bool)openFileDialog.ShowDialog())
                (DataContext as AGRegistrationViewModel).showExcel(openFileDialog);
        }

        private void btnSelectImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images (*.jpg;*.png)|*.jpg;*.png";

            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Selección de Imagenes";
            if ((bool)openFileDialog.ShowDialog())
                (DataContext as AGRegistrationViewModel).selectImages(openFileDialog);
        }
        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as AGRegistrationViewModel).uploadAG();
        }


    }
}
