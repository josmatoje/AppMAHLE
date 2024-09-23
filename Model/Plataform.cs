using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
    public class Plataform
    {
        private int id;
        private string plataformName;

        public int Id { get => id; set => id = value; }
        public string PlataformName { get => plataformName; set => plataformName = value; }

        public Plataform(int id, string plataformName)
        {
            this.id = id;
            this.plataformName = plataformName;
        }

        public override string ToString()
        {
            return PlataformName;
        }
    }
}
