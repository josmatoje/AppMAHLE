using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
    public class PCBDefinition
    {
        #region atributos
        private int order;
        private string snNumber;
        private string reference;
        private string rework;
        private string saved;

        #endregion
        #region propiedades publicas
        public int Order { get => order; set => order = value; }
        public string SnNumber { get => snNumber; set => snNumber = value; }
        public string Reference { get => reference; set => reference = value; }
        public string Rework { get => rework; set => rework = value; }
        
        public string Saved { get => saved; set => saved = value; }
        #endregion
        #region constructores
        public PCBDefinition()
        {
            Order = 0;
            SnNumber = "";
            Reference = "";
            Rework = "";
            Saved = "";
        }
        public PCBDefinition(int order,  string snNumber, string reference, string rework, string saved)
        {
            Order = order;
            SnNumber = snNumber;
            Reference = reference;
            Rework = rework;
            Saved = saved;
        }

        #endregion
        #region public methods
        public override bool Equals(object obj)
        {
            return obj is PCBDefinition pCB &&
                   SnNumber == pCB.SnNumber;
        }
        #endregion
    }
}
