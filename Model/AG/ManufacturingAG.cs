using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
    public class ManufacturingAG
    {
        #region atributos
        private int num;
        private int process;
        private string processDesc;
        private int operation;
        private string picture;
        private string operationDesc;
        private string satation;
        private string test;
        private string input;
        private string screwDriver;
        private string scam_dmc;

        #endregion
        #region propiedades publicas
        public int Num { get => num; set => num = value; }
        public int Process { get => process; set => process = value; }
        public string ProcessDesc { get => processDesc; set => processDesc = value; }
        public int Operation { get => operation; set => operation = value; }
        public string Picture { get => picture; set => picture = value; }
        public string OperationDesc { get => operationDesc; set => operationDesc = value; }
        public string Satation { get => satation; set => satation = value; }
        public string Test { get => test; set => test = value; }
        public string Input { get => input; set => input = value; }
        public string ScrewDriver { get => screwDriver; set => screwDriver = value; }
        public string Scam_dmc { get => scam_dmc; set => scam_dmc = value; }
        #endregion
        #region constructores
        public ManufacturingAG(int num, int process, string processDesc, int operation, string picture, string operationDesc, string satation, string test, string input, string screwDriver, string scam_dmc)
        {
            Num = num;
            Process = process;
            ProcessDesc = processDesc;
            Operation = operation;
            Picture = picture;
            OperationDesc = operationDesc;
            Satation = satation;
            Test = test;
            Input = input;
            ScrewDriver = screwDriver;
            Scam_dmc = scam_dmc;
        }
        #endregion
        #region public methods
        public bool Equals(object obj)
        {
            return obj is ManufacturingAG ag &&
                   Num == ag.Num &&
                   Process == ag.Process;/*&&
                   ProcessDesc == ag.ProcessDesc &&
                   operation == ag.Operation &&
                   Picture == ag.Picture;*/
        }
        #endregion
    }
}
