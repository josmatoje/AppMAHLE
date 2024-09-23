using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
    public class SavedOFs
    {
        #region atributos
        private int order;
        private string codeOF;
        private int quantity;
        private string internalName;
        private string saved;
        #endregion
        #region propiedades publicas
        public int Order { get => order; set => order = value; }
        public string CodeOF { get => codeOF; set => codeOF = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public string InternalName { get => internalName; set => internalName = value; }
        public string Saved { get => saved; set => saved = value; }
        #endregion
        #region public methods
        public SavedOFs(int order, string codeOF, int quantity, string internalName, string saved)
        {
            Order = order;
            CodeOF = codeOF;
            Quantity = quantity;
            InternalName = internalName;
            Saved = saved;
        }
        public SavedOFs()
        {
            Order = -999;
            CodeOF = "";
            Quantity = -999;
            InternalName = "";
            Saved = "";
        }
        #endregion
        #region public methods
        public override bool Equals(object obj)
        {
            return obj is SavedOFs oF &&
                   CodeOF == oF.CodeOF;
        }
        #endregion

    }
}
