using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
   public  class OFChangeDescrip
    {
        #region atributos
        private string internalName;
        private string codeOF;
        #endregion
        #region propiedades publicas
        public string InternalName { get => internalName; set => internalName = value; }
        public string CodeOF { get => codeOF; set => codeOF = value; }
        #endregion
        #region public methods
        public OFChangeDescrip(string codeOF, string internalName)
        {
            CodeOF = codeOF;
            InternalName = internalName;
        }
        public OFChangeDescrip()
        {
            CodeOF = "";
            InternalName = "";
        }
        #endregion
        #region public methods
        public override bool Equals(object obj)
        {
            return obj is OFChangeDescrip oF &&
                   CodeOF == oF.CodeOF;
        }
        #endregion
    }
}
