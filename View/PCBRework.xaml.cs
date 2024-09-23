using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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
    /// Interaction logic for PCBRegistration.xaml
    /// </summary>
    public partial class PCBRework : UserControl
    {
        public PCBRework()
        {
            InitializeComponent();
        }

        private void OnKeyDownHandlerSN(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && snTextBox.Text != String.Empty)
            {
                if ((DataContext as PCBReworkViewModel).ExistPCB()){
                    reworkTextBox.Focus();
                }
            }
        }
        private void OnKeyDownHandlerRework(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && reworkText.Text != String.Empty)
            {
                if ((DataContext as PCBReworkViewModel).SavePCBRework())
                {
                    snTextBox.Focus();
                }
            }
        }

    }
}
