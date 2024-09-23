using Aspose.Cells.Drawing;
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
    /// Interaction logic for OFRegistration.xaml
    /// </summary>
    public partial class OFRegistration : UserControl
    {
        public OFRegistration()
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
                        descriptionTextBox.Focus();
                    }
                } else
                {
                    if ((sender as System.Windows.Controls.TextBox).Name.Equals("descriptionTextBox"))
                    {
                        quantTextBox.Focus();
                    }
                    else
                    {
                        if (registerButton.IsEnabled)
                        {
                            (DataContext as OFRegistrationViewModel).SaveOF();
                        }
                        else
                        {
                            
                        }
                    }
                       
                }
                
            }
        }
    }
}
