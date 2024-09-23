using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
    public class PCBReference
    {
        #region atributos
        private int id;
        private string referenceName;
        private string reference;
        private int plataforma;
        #endregion
        #region propiedades publicas
        public int Id { get => id; set => id = value; }
        public string ReferenceName { get => referenceName; set => referenceName = value; }
        public string Reference { get => reference; set => reference = value; }
        public int Plataforma { get => plataforma; set => plataforma = value; }
        #endregion

        #region Constructor
        public PCBReference(int id, string referenceName, string reference, int plataforma)
        {
            this.id = id;
            this.ReferenceName = referenceName;
            this.Reference = reference;
            this.Plataforma = plataforma;
        }
        public PCBReference(string referenceName, string reference)
        {
            this.id = -1;
            this.ReferenceName = referenceName;
            this.Reference = reference;
            this.Plataforma = -1;
        }
        public PCBReference(string[] row)
        {
            if (row.Length >= 4) {
                this.id = Int32.Parse(row[0]);
                this.ReferenceName = row[1];
                this.Reference = row[2];
                this.Plataforma = Int32.Parse(row[3]);
            } else
            {
                this.id = -1;
                this.ReferenceName = String.Empty;
                this.Reference = String.Empty;
                this.Plataforma = -1;
            }
        }
        #endregion
    }
}
