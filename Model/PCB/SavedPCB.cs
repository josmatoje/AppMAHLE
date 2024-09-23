using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
    public class SavedPCB
    {
        #region atributos
        private int order;
        private string snNumber;
        private string reference;
        private string layoutBom;
        private string lote;
        private string saved;
        #endregion
        #region propiedades publicas
        public int Order { get => order; set => order = value; }
        public string SnNumber { get => snNumber; set => snNumber = value; }
        public string Reference { get => reference; set => reference = value; }
        public string LayoutBom { get => layoutBom; set => layoutBom = value; }
        public string Lote { get => lote; set => lote = value; }
        public string Saved { get => saved; set => saved = value; }
        #endregion
        #region constructores
        public SavedPCB(int order, string snNumber, string reference, string layoutBom, string lote, bool saved)
        {
            Order = order;
            SnNumber = snNumber;
            Reference = reference;
            LayoutBom = layoutBom;
            Lote = lote;
            if (saved)
            {
                Saved = "OK";
            }
            else
            {
                Saved = "ERROR";
            }
        }

        #endregion
        #region public methods
        public override bool Equals(object obj)
        {
            return obj is SavedPCB pCB &&
                   SnNumber == pCB.SnNumber;
        }
        #endregion
    }
}
