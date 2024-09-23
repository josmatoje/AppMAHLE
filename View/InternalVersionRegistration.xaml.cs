using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace DIV_Protos
{
    /// <summary>
    /// Interaction logic for InternalVersionRegistration.xaml
    /// </summary>
    public partial class InternalVersionRegistration : UserControl
    {
        private Regex internalVersionRegex = new Regex(@"[B]\d+[.]\d+[.]\d+");
        public InternalVersionRegistration()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (internalVersionRegex.Match(VersionTextBox.Text).Success)
            {
            */
                OpenFileDialog openFileDialog = new OpenFileDialog(); 
                openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if ((bool)openFileDialog.ShowDialog())
                    (DataContext as InternalVersionViewModel).showExcel(openFileDialog);
           /* }
            else
            {
                MessageBox.Show("El formato de la nueva versión interna no es el correcto.\n Debe seguir el formato 'BX.Y.Z' (Ej: B1.2.3).", 
                                "Error de formato", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Warning);
            }
            */
            
        }
        private void btnUpload_InternalVersion(object sender, RoutedEventArgs e)
        {
            (DataContext as InternalVersionViewModel).uploadVersion();
        }
            
    }
}
