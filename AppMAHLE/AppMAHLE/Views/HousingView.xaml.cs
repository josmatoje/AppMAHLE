using AppMAHLE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace AppMAHLE
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class HousingView : Page
    {
        public HousingView()
        {
            this.InitializeComponent();

        }

        private void onClickSearch(object sender, RoutedEventArgs e)
        {
            clearFields();
            snSearch.Visibility = Visibility.Visible;
            dato1.Visibility = Visibility.Visible;
            dato2.Visibility = Visibility.Visible;
        }

        private void onClickSave(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void clearFields()
        {
            sNumberBox.DataContext = string.Empty;
            referenceBox.DataContext = string.Empty;
            stateBox.DataContext = string.Empty;
            loteBox.DataContext = string.Empty;
        }
    }
}
