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

namespace DIV_Protos
{
    /// <summary>
    /// Lógica de interacción para HousingRegistration.xaml
    /// </summary>
    public partial class HousingRegistration : UserControl
    {
        public HousingRegistration()
        {
            InitializeComponent();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                bool status = true;
                if (!snTextBox.Text.Equals(String.Empty))
                {
                    (DataContext as HousingRegistrationViewModel).SaveHousing();
                    if (status == true)
                    {
                        snTextBox.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Debe introducir el serial number.",
                                    "SN no indicado",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    snTextBox.Focus();
                }

            }
        }

        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            snTextBox.Focus();
        }
        private void btnSaveSN_Click(object sender, RoutedEventArgs e)
        {
            bool status = true;
            if (!snTextBox.Text.Equals(String.Empty))
            {
                (DataContext as HousingRegistrationViewModel).SaveHousing();
                if (status == true)
                {
                    snTextBox.Focus();
                }
            }
            else
            {
                MessageBox.Show("Debe introducir el serial number.",
                                "SN no indicado",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                snTextBox.Focus();
            }
        }

        private void referencesSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            snTextBox.Focus();
        }

        private void refBOMSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            snTextBox.Focus();
        }
    }
}
