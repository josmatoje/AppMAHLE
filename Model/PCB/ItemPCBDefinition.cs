using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
    public class ItemPCBDefinition
    {
        #region atributos
        private int idItemBomPCB;
        private string pcbDefinition;
        private string referenceName;
        private string layoutBOM;
        
        #endregion

        #region propiedades publicas
        public int IdItemBomPCB { get => idItemBomPCB; set => idItemBomPCB = value; }
        public string PcbDefinition { get => pcbDefinition; set => pcbDefinition = value; }
        public string ReferenceName { get => referenceName; set => referenceName = value; }
        public string LayoutBOM { get => layoutBOM; set => layoutBOM = value; }
       
        #endregion

        #region Constructor
        public ItemPCBDefinition(int idItemBomPCB, string pcbDefinition, string referenceName, string layoutBOM)
        {
            this.IdItemBomPCB = idItemBomPCB;
            this.PcbDefinition = pcbDefinition;
            this.ReferenceName = referenceName;
            this.LayoutBOM = layoutBOM;
        }
        #endregion
    }
}
