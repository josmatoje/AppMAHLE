﻿#pragma checksum "D:\Mi carpeta\GS DAM\Repositorios\AppMahle\AppMAHLE\AppMAHLE\Views\OFView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "0A7A9C326A4F2ED2A796169DF36395CCCFE5ECBB7950DC86E5DBD7D5D7E71E6E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppMAHLE
{
    partial class OFView : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 0.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // Views\OFView.xaml line 24
                {
                    this.orderNumber = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 3: // Views\OFView.xaml line 25
                {
                    this.internalVersion = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 4: // Views\OFView.xaml line 27
                {
                    this.ordNumBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 5: // Views\OFView.xaml line 28
                {
                    this.internalVersBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 6: // Views\OFView.xaml line 30
                {
                    this.saveBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.saveBtn).Click += this.onClickSave;
                }
                break;
            case 7: // Views\OFView.xaml line 32
                {
                    this.searchBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.searchBtn).Click += this.onClickSearch;
                }
                break;
            case 8: // Views\OFView.xaml line 34
                {
                    this.orderNumberSearch = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 9: // Views\OFView.xaml line 35
                {
                    this.dato1 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10: // Views\OFView.xaml line 36
                {
                    this.dato2 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 0.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

