using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
    public class OF
    {
        #region atributos
        private string description;
        private string value;
        private string quantity;
        #endregion
        #region propiedades publicas
        public string Description { get => description; set => description = value; }
        public string Value { get => value; set => this.value = value; }
        public string Quantity { get => quantity; set => quantity = value; }
        #endregion
        #region constructor
        public OF(string description, string value, string quantity)
        {
            this.Description = description;
            this.Value = value;
            this.Quantity = quantity;
        }
        public OF()
        {
            this.Description = "";
            this.Value = "";
            this.Quantity = "";
        }
        #endregion
    }
}
