using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIV_Protos
{
    public class Labeling
    {
        #region atributos
        private string snr;
        private string zgs;
        private string eq;
        private string luStatus;
        private string prodLoc;
        private string manufacturingDate;
        private string daiPlant;
        private string brbl;
        private string lft;
        private string pnMahle;
        #endregion
        #region propuiedades públicas
        public string Snr { get => snr; set => snr = value; }
        public string Zgs { get => zgs; set => zgs = value; }
        public string Eq { get => eq; set => eq = value; }
        public string LuStatus { get => luStatus; set => luStatus = value; }
        public string ProdLoc { get => prodLoc; set => prodLoc = value; }
        public string ManufacturingDate { get => manufacturingDate; set => manufacturingDate = value; }
        public string DaiPlant { get => daiPlant; set => daiPlant = value; }
        public string Brbl { get => brbl; set => brbl = value; }
        public string Lft { get => lft; set => lft = value; }
        public string PnMahle { get => pnMahle; set => pnMahle = value; }
        #endregion
        #region constructor
        public Labeling(string snr, string zgs, string eq, string luStatus, string prodLoc, string manufacturingDate, string daiPlant, string brbl, string lft, string pnMahle)
        {
            this.Snr = snr;
            this.Zgs = zgs;
            this.Eq = eq;
            this.LuStatus = luStatus;
            this.ProdLoc = prodLoc;
            this.ManufacturingDate = manufacturingDate;
            this.DaiPlant = daiPlant;
            this.Brbl = brbl;
            this.Lft = lft;
            this.PnMahle = pnMahle;
        }
        #endregion
    }
}
