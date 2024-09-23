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
    /// Interaction logic for OFChange.xaml
    /// </summary>
    public partial class OFChange : UserControl
    {
        public OFChange()
        {
            InitializeComponent();
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if ((sender as System.Windows.Controls.TextBox).Name.Equals("descTextBox"))
                {
                    if (!descTextBox.Text.Equals(String.Empty))
                    {
                        (DataContext as OFChangeViewModel).ExistInternalVersion();
                    }
                }
                else
                {
                }

            }
        }
        private void OnKeyDownHandlerSN(object sender, KeyEventArgs e)
        {
            bool status = true;
            if (e.Key == Key.Return)
            {
                if ((sender as System.Windows.Controls.TextBox).Name.Equals("SNTextBox"))
                {
                    if (!SNTextBox.Text.Equals(String.Empty))
                    {
                        status= (DataContext as OFChangeViewModel).ExistSerialNumberOFandVI();
                        if(status==true)
                        {
                            descTextBox.Focus();
                        }
                    }
                }
                else
                {
                }

            }
        }
        private void btnSerialNumberResearch_Click(object sender, RoutedEventArgs e)
        {
            bool status = true;
            if (!SNTextBox.Text.Equals(String.Empty))
            {
                status = (DataContext as OFChangeViewModel).ExistSerialNumberOFandVI();
                if (status == true)
                {
                    descTextBox.Focus();
                }
            }
            else
            {
                MessageBox.Show("Debe introducir el serial number.",
                                "SN no indicado",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                SNTextBox.Focus();
            }
        }
        private void btnNewOFResearch_Click(object sender, RoutedEventArgs e)
        {
            bool status = true;
            if (!descTextBox.Text.Equals(String.Empty))
            {
                (DataContext as OFChangeViewModel).ExistInternalVersion();
                OPSaveTextBox.Focus();
            }
            else
            {
                MessageBox.Show("Debe introducir la orden de fabricación a la que desea cambiar el serial number.",
                                "OF no indicada",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                descTextBox.Focus();
            }
        }
        private void btnSaveChangeOF_Click(object sender, RoutedEventArgs e)
        {
            if (!OPSaveTextBox.Text.Equals(String.Empty))
            {
                (DataContext as OFChangeViewModel).uploadChangeOF();
                OPSaveTextBox.Focus();
            }
            else
            {
                MessageBox.Show("Debe introducir la operacion a la que desea enviar la pieza.",
                                    "Operacion no indicada",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                OPSaveTextBox.Focus();
            }
        }
    }
}


