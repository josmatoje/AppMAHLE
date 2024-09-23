using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
    public class SavedHousing
    {
        #region atributos
        private int order;
        private string snNumber;
        private string reference;
        private string categoryNameItem;
        private string saved;
        

        #endregion
        #region propiedades publicas
        public int Order { get => order; set => order = value; }
        public string SnNumber { get => snNumber; set => snNumber = value; }
        public string Reference { get => reference; set => reference = value; }
        public string Saved { get => saved; set => saved = value; }
        public string CategoryNameItem { get => categoryNameItem; set => categoryNameItem = value; }
        #endregion
        #region constructores
        public SavedHousing(int order, string snNumber, string reference, string category,  string error)
        {
            Order = order;
            SnNumber = snNumber;
            Reference = reference;
            CategoryNameItem = category;
            Saved = error;
        }
        public SavedHousing()
        {
            Order = -999;
            SnNumber = "";
            Reference = "";
            CategoryNameItem = "";
            Saved = "";
        }

        #endregion
        #region public methods
        public override bool Equals(object obj)
        {
            return obj is SavedHousing housing &&
                   SnNumber == housing.SnNumber;
        }
        #endregion
    }
}
