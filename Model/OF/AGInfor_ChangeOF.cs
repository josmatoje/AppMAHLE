using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
    public class AGInfor_ChangeOF
    {
        #region atributos
        private string internalVersion;
        private int process;
        private string processDesc;
        private int num;
        private string operationDesc;

        #endregion
        #region propiedades publicas

        public int Num { get => num; set => num = value; }
        public int Process { get => process; set => process = value; }
        public string ProcessDesc { get => processDesc; set => processDesc = value; }
        public string OperationDesc { get => operationDesc; set => operationDesc = value; }
        public string InternalVersion { get => internalVersion; set => internalVersion = value; }

        #endregion
        #region constructores
        public AGInfor_ChangeOF(string internalVersion, int process, string processDesc,int num, string operationDesc)
        {
            InternalVersion = internalVersion;
            Num = num;
            Process = process;
            ProcessDesc = processDesc;
            OperationDesc = operationDesc;
        }
        #endregion
        #region public methods
        public bool Equals(object obj)
        {
            return obj is AGInfor_ChangeOF ag &&
                   Num == ag.Num &&
                   Process == ag.Process;/*&&
                   ProcessDesc == ag.ProcessDesc &&
                   operation == ag.Operation &&
                   Picture == ag.Picture;*/
        }
        #endregion
    }
}
