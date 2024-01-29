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
using controls = Microsoft.UI.Xaml.Controls;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace AppMAHLE
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

        }
        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var navOptions = new FrameNavigationOptions
            {
                TransitionInfoOverride = args.RecommendedNavigationTransitionInfo,
                IsNavigationStackEnabled = false,
            };

            switch (args.InvokedItemContainer.Name)
            {
                case nameof(pcb):
                    ContentFrame.NavigateToType(typeof(PCBView), null, navOptions);
                    break;
                case nameof(housing):
                    ContentFrame.NavigateToType(typeof(HousingView), null, navOptions);
                    break;
                case nameof(internalRef):
                    ContentFrame.NavigateToType(typeof(InternalRefView), null, navOptions);
                    break;
                case nameof(of):
                    ContentFrame.NavigateToType(typeof(OFView), null, navOptions);
                    break;
            }
        }
    }
}
