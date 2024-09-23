using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
    public class Testing
    {
        #region atributos
        Dictionary<string, string> partsToValidate;
        #endregion
        #region propiedades publicas
        public Dictionary<string, string> PartsToValidate { get => partsToValidate; set => partsToValidate = value; }
        public string Key1 { get => GetKeyOrValueAt(true, 0); }
        public string Value1 { get => GetKeyOrValueAt(false, 0); }
        public string Key2 { get => GetKeyOrValueAt(true, 1); }
        public string Value2 { get => GetKeyOrValueAt(false, 1); }
        public string Key3 { get => GetKeyOrValueAt(true, 2); }
        public string Value3 { get => GetKeyOrValueAt(false, 2); }
        public string Key4 { get => GetKeyOrValueAt(true, 3); }
        public string Value4 { get => GetKeyOrValueAt(false, 3); }
        public string Key5 { get => GetKeyOrValueAt(true, 4); }
        public string Value5 { get => GetKeyOrValueAt(false, 4); }
        public string Key6 { get => GetKeyOrValueAt(true, 5); }
        public string Value6 { get => GetKeyOrValueAt(false, 5); }
        public string Key7 { get => GetKeyOrValueAt(true, 6); }
        public string Value7 { get => GetKeyOrValueAt(false, 6); }
        public string Key8 { get => GetKeyOrValueAt(true, 7); }
        public string Value8 { get => GetKeyOrValueAt(false, 7); }
        #endregion
        #region constructor
        public Testing(Dictionary<string, string> partsToValidate)
        {
            this.PartsToValidate = partsToValidate;
        }
        #endregion
        #region metodos privados
        private string GetKeyOrValueAt(bool key, int position)
        {
            if (position >= PartsToValidate.Count)
            {
                position = -1;
            }

            return position==-1 ? "" : key ? $"{PartsToValidate.ElementAt(position).Key}" : PartsToValidate.ElementAt(position).Value;
        }
        #endregion
    }
}
