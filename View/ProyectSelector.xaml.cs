using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ProyectSelector.xaml
    /// </summary>
    public partial class ProyectSelector : UserControl
    {

        public ProyectSelector()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            Mediator.Notify("GoToMenuScreen", "");
        }
    }
}
