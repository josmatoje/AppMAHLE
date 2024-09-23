using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DIV_Protos
{
    public class ItemDefinitionOF
    {
        #region atributos
        private string descriptionReference;
        private string internalName;
        //private BitmapImage imagen;
        #endregion

        #region propiedades publicas
        public string DescriptionReference { get => descriptionReference; set => descriptionReference = value; }
        public string InternalName { get => internalName; set => internalName = value; }
        //public BitmapImage Imagen { get => imagen; set => imagen = value; }
        #endregion
        #region constructor
        public ItemDefinitionOF(string descriptionReference, string internalName)
        {
            this.DescriptionReference = descriptionReference;
            this.InternalName = internalName;
            
        }
        public ItemDefinitionOF()
        {
            this.DescriptionReference = "";
            this.InternalName = "";
        }
        #endregion
    }
}
