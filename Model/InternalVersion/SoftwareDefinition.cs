using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
    public class SoftwareDefinition
    {
        #region atributos
        private string hw_snr;
        private string hwStatus;
        private string sw_snr;
        private string swStatus;
        private string releaseTest;
        private string releaseFinal;
        #endregion
        #region propiedades públicas
        public string Hw_snr { get => hw_snr; set => hw_snr = value; }
        public string HwStatus { get => hwStatus; set => hwStatus = value; }
        public string Sw_snr { get => sw_snr; set => sw_snr = value; }
        public string SwStatus { get => swStatus; set => swStatus = value; }
        public string ReleaseTest { get => releaseTest; set => releaseTest = value; }
        public string ReleaseFinal { get => releaseFinal; set => releaseFinal = value; }
        #endregion
        #region constructor
        public SoftwareDefinition(string hw_snr, string hwStatus, string sw_snr, string swStatus, string releaseTest, string releaseFinal)
        {
            Hw_snr = hw_snr;
            HwStatus = hwStatus;
            Sw_snr = sw_snr;
            SwStatus = swStatus;
            ReleaseTest = releaseTest;
            ReleaseFinal = releaseFinal;
        }
        #endregion
    }
}
