using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
    public class MechanicalVersion
    {

        private int id;
        private string code;
        private string category;


        public int Id { get => id; set => id = value; }
        public string Code { get => code; set => code = value; }
        public string Category { get => category; set => category = value; }

        public MechanicalVersion()
        {
            this.id = 0;
            this.code = "";
            this.category = "";
        }
        public MechanicalVersion(int id, string code, string category)
        {
            this.id = id;
            this.code = code;
            this.category = category;
        }

        public override string ToString()
        {
            return this.code;
        }
    }
}
