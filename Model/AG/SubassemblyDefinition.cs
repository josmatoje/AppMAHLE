using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace DIV_Protos
{
    public class SubassemblyDefinition
    {
        #region atributos
        private string plataform;
        private string reference;
        private string version;
        private string descriptionRef;
        private string date;
        private ElectronicHardware electronicHardware;
        private string process;
        private bool trazability;
        private string imageName;
        private byte[] imagenData;
        #endregion
        #region propiedades publicas
        public string Plataform { get => plataform; set => plataform = value; }
        public string Reference { get => reference; set => reference = value; }
        public string Version { get => version; set => version = value; }
        public string DescriptionRef { get => descriptionRef; set => descriptionRef = value; }
        public string Date { get => date; set => date = value; }
        public ElectronicHardware ElectronicHardware { get => electronicHardware; set => electronicHardware = value; }
        public string Process { get => process; set => process = value; }
        public bool Trazability { get => trazability; set => trazability = value; }
        public string IsTrazability { get => trazability ? "Yes" : "No"; }
        public string ImageName { get => imageName; set => imageName = value; }
        public byte[] ImagenData { get => imagenData; set => imagenData = value; }
        #endregion
        #region constructor
        public SubassemblyDefinition(string plataform, string reference, string version, string descriptionRef, string date, ElectronicHardware electronicHardware, string process, bool trazability)
        {
            this.Plataform = plataform;
            this.Reference = reference;
            this.Version = version;
            this.DescriptionRef = descriptionRef;
            this.Date = date;
            this.ElectronicHardware = electronicHardware;
            this.Process = process;
            this.Trazability = trazability;
        }
        #endregion

    }
}
