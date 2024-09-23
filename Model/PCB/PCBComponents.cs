using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
    public class PCBComponents
    {
        private int idSN;
        private int layout;
        private int bom;

        public int Layout { get => layout; set => layout = value; }
        public int Bom { get => bom; set => bom = value; }
        public int IdSN { get => idSN; set => idSN = value; }

        public PCBComponents(int idsn, int layout, int bom)
        {
            IdSN = idsn;
            Layout = layout;
            Bom = bom;
        }
        public PCBComponents(int idsn, int layout)
        {
            IdSN = idsn;
            Layout = layout;
            Bom = 0;
        }
        public override bool Equals(object obj)
        {
            return obj is PCBComponents components &&
                   Layout == components.Layout &&
                   Bom == components.Bom;
        }

        public override string ToString()
        {
            return Layout + "." + Bom;
        }

    }
}
