using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
    public class ItemPCBRework
    {
        #region atributos
        private string layoutBOM;
        private int idSNRegisterPCB;
        private string codeSN;
        private string hardwareReference;
        private int reworkPCB;

        #endregion

        #region propiedades publicas
        public string LayoutBOM { get => layoutBOM; set => layoutBOM = value; }
        public int IdSNRegisterPCB { get => idSNRegisterPCB; set => idSNRegisterPCB = value; }
        public string CodeSN { get => codeSN; set => codeSN = value; }
        public string HardwareReference { get => hardwareReference; set => hardwareReference = value; }
        public int ReworkPCB { get => reworkPCB; set => reworkPCB = value; }

        #endregion

        #region Constructor
        public ItemPCBRework()
        {
            this.LayoutBOM = "";
            this.IdSNRegisterPCB = -9999;
            this.CodeSN = "";
            this.HardwareReference = "";
            this.ReworkPCB = -9999;
        }
        public ItemPCBRework(string layoutBOM, int idSNRegisterPCB, string codeSN, string hardwareReference, int reworkPCB)
        {
            this.LayoutBOM = layoutBOM;
            this.IdSNRegisterPCB = idSNRegisterPCB;
            this.CodeSN = codeSN;
            this.HardwareReference = hardwareReference;
            this.ReworkPCB = reworkPCB;
        }

        
        #endregion
    }
}
