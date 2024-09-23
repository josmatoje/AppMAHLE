using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace DIV_Protos
{
    public class ReleaseDefinition
    {
        #region atributos
        private string projectName;
        private string sample;
        private string partNumber;
        private string internalName;
        private string descriptionReference;
        private bool trazability;
        private string date;
        private bool electronicBlock;
        private bool mechanicalBlock;
        private bool softwareBlock;
        private bool processBlock;
        private bool labellingBlock;
        private bool testingBlock;
        private Dictionary<string, List<string>> electronicHardware;
        private Dictionary<string, string> mechanicalHardware;
        private Dictionary<string, string> softwareHardware;
        private string process;
        private Dictionary<string, string> labellingHardware;
        private Dictionary<string, string> testingHardware;
        private bool insertHardware;
        private bool insertMechanical;
        private bool insertSoftware;
        private bool insertProcess;
        private bool insertLabelling;
        private bool insertTest;
        #endregion

        #region propiedades publicas
        public string ProjectName { get => projectName; set => projectName = value; }
        public string Sample { get => sample; set => sample = value; }
        public string PartNumber { get => partNumber; set => partNumber = value; }
        public string InternalName { get => internalName; set => internalName = value; }
        public string DescriptionReference { get => descriptionReference; set => descriptionReference = value; }
        public bool Trazability { get => trazability; set => trazability = value; }
        public string Date { get => date; set => date = value; }
        public bool ElectronicBlock { get => electronicBlock; set => electronicBlock = value; }
        public bool MechanicalBlock { get => mechanicalBlock; set => mechanicalBlock = value; }
        public bool SoftwareBlock { get => softwareBlock; set => softwareBlock = value; }
        public bool ProcessBlock { get => processBlock; set => processBlock = value; }
        public bool LabellingBlock { get => labellingBlock; set => labellingBlock = value; }
        public bool TestingBlock { get => testingBlock; set => testingBlock = value; }
        public Dictionary<string, List<string>> ElectronicHardware { get => electronicHardware; set => electronicHardware = value; }
        public Dictionary<string, string> MechanicalHardware { get => mechanicalHardware; set => mechanicalHardware = value; }
        public Dictionary<string, string> SoftwareHardware { get => softwareHardware; set => softwareHardware = value; }
        public string Process { get => process; set => process = value; }
        public Dictionary<string, string> LabellingHardware { get => labellingHardware; set => labellingHardware = value; }
        public Dictionary<string, string> TestingHardware { get => testingHardware; set => testingHardware = value; }
        public bool InsertHardware { get => insertHardware; set => insertHardware = value; }
        public bool InsertMechanical { get => insertMechanical; set => insertMechanical = value; }
        public bool InsertSoftware { get => insertSoftware; set => insertSoftware = value; }
        public bool InsertProcess { get => insertProcess; set => insertProcess = value; }
        public bool InsertLabelling { get => insertLabelling; set => insertLabelling = value; }
        public bool InsertTest { get => insertTest; set => insertTest = value; }

        #region electrónica
        public string ElectronicKey1 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 0); }
        public string ElectronicValue11 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 0); }
        public string ElectronicValue12 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 0, 1); }
        public string ElectronicKey2 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 1); }
        public string ElectronicValue21 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 1); }
        public string ElectronicValue22 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 1, 1); }
        public string ElectronicKey3 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 2); }
        public string ElectronicValue31 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 2); }
        public string ElectronicValue32 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 2, 1); }
        public string ElectronicKey4 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 3); }
        public string ElectronicValue41 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 3); }
        public string ElectronicValue42 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 3, 1); }
        public string ElectronicKey5 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 4); }
        public string ElectronicValue51 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 4); }
        public string ElectronicValue52 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 4, 1); }
        public string ElectronicKey6 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 5); }
        public string ElectronicValue61 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 5); }
        public string ElectronicValue62 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 5, 1); }
        public string ElectronicKey7 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 6); }
        public string ElectronicValue71 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 6); }
        public string ElectronicValue72 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 6, 1); }
        public string ElectronicKey8 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 7); }
        public string ElectronicValue81 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 7); }
        public string ElectronicValue82 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 7, 1); }
        public string ElectronicKey9 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 8); }
        public string ElectronicValue91 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 8); }
        public string ElectronicValue92 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 8, 1); }
        public string ElectronicKey10 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 9); }
        public string ElectronicValue101 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 9); }
        public string ElectronicValue102 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 9, 1); }
        /*
        public string ElectronicKey11 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 10); }
        public string ElectronicValue11 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 10); }
        public string ElectronicKey12 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 11); }
        public string ElectronicValue12 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 11); }
        public string ElectronicKey13 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 12); }
        public string ElectronicValue13 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 12); }
        public string ElectronicKey14 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 13); }
        public string ElectronicValue14 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 13); }
        public string ElectronicKey15 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 14); }
        public string ElectronicValue15 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 14); }
        public string ElectronicKey16 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 15); }
        public string ElectronicValue16 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 15); }
        public string ElectronicKey17 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 16); }
        public string ElectronicValue17 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 16); }
        public string ElectronicKey18 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 17); }
        public string ElectronicValue18 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 17); }
        public string ElectronicKey19 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 18); }
        public string ElectronicValue19 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 18); }
        public string ElectronicKey20 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, true, 19); }
        public string ElectronicValue20 { get => GetKeyOrValueAt(ReleaseBlock.Electronic, false, 19); }
        */
        #endregion
        #region mecánica
        public string MechanicalKey1 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 0); }
        public string MechanicalValue1 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 0); }
        public string MechanicalKey2 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 1); }
        public string MechanicalValue2 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 1); }
        public string MechanicalKey3 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 2); }
        public string MechanicalValue3 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 2); }
        public string MechanicalKey4 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 3); }
        public string MechanicalValue4 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 3); }
        public string MechanicalKey5 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 4); }
        public string MechanicalValue5 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 4); }
        public string MechanicalKey6 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 5); }
        public string MechanicalValue6 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 5); }
        public string MechanicalKey7 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 6); }
        public string MechanicalValue7 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 6); }
        public string MechanicalKey8 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical   , true, 7); }
        public string MechanicalValue8 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 7); }
        public string MechanicalKey9 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 8); }
        public string MechanicalValue9 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 8); }
        public string MechanicalKey10 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 9); }
        public string MechanicalValue10 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 9); }
        public string MechanicalKey11 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 10); }
        public string MechanicalValue11 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 10); }
        public string MechanicalKey12 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 11); }
        public string MechanicalValue12 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 11); }
        public string MechanicalKey13 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 12); }
        public string MechanicalValue13 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 12); }
        public string MechanicalKey14 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 13); }
        public string MechanicalValue14 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 13); }
        public string MechanicalKey15 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 14); }
        public string MechanicalValue15 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 14); }
        public string MechanicalKey16 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 15); }
        public string MechanicalValue16 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 15); }
        public string MechanicalKey17 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 16); }
        public string MechanicalValue17 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 16); }
        public string MechanicalKey18 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 17); }
        public string MechanicalValue18 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 17); }
        public string MechanicalKey19 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 18); }
        public string MechanicalValue19 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 18); }
        public string MechanicalKey20 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, true, 19); }
        public string MechanicalValue20 { get => GetKeyOrValueAt(ReleaseBlock.Mechanical, false, 19); }
        #endregion
        #region software
        public string SoftwareKey1 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 0); }
        public string SoftwareValue1 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 0); }
        public string SoftwareKey2 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 1); }
        public string SoftwareValue2 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 1); }
        public string SoftwareKey3 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 2); }
        public string SoftwareValue3 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 2); }
        public string SoftwareKey4 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 3); }
        public string SoftwareValue4 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 3); }
        public string SoftwareKey5 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 4); }
        public string SoftwareValue5 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 4); }
        public string SoftwareKey6 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 5); }
        public string SoftwareValue6 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 5); }
        public string SoftwareKey7 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 6); }
        public string SoftwareValue7 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 6); }
        public string SoftwareKey8 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 7); }
        public string SoftwareValue8 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 7); }
        public string SoftwareKey9 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 8); }
        public string SoftwareValue9 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 8); }
        public string SoftwareKey10 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 9); }
        public string SoftwareValue10 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 9); }
        public string SoftwareKey11 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 10); }
        public string SoftwareValue11 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 10); }
        public string SoftwareKey12 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 11); }
        public string SoftwareValue12 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 11); }
        public string SoftwareKey13 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 12); }
        public string SoftwareValue13 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 12); }
        public string SoftwareKey14 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 13); }
        public string SoftwareValue14 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 13); }
        public string SoftwareKey15 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 14); }
        public string SoftwareValue15 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 14); }
        public string SoftwareKey16 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 15); }
        public string SoftwareValue16 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 15); }
        public string SoftwareKey17 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 16); }
        public string SoftwareValue17 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 16); }
        public string SoftwareKey18 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 17); }
        public string SoftwareValue18 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 17); }
        public string SoftwareKey19 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 18); }
        public string SoftwareValue19 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 18); }
        public string SoftwareKey20 { get => GetKeyOrValueAt(ReleaseBlock.Software, true, 19); }
        public string SoftwareValue20 { get => GetKeyOrValueAt(ReleaseBlock.Software, false, 19); }
        #endregion
        #region etiqueta
        public string LabellingKey1 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 0); }
        public string LabellingValue1 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 0); }
        public string LabellingKey2 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 1); }
        public string LabellingValue2 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 1); }
        public string LabellingKey3 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 2); }
        public string LabellingValue3 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 2); }
        public string LabellingKey4 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 3); }
        public string LabellingValue4 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 3); }
        public string LabellingKey5 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 4); }
        public string LabellingValue5 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 4); }
        public string LabellingKey6 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 5); }
        public string LabellingValue6 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 5); }
        public string LabellingKey7 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 6); }
        public string LabellingValue7 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 6); }
        public string LabellingKey8 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 7); }
        public string LabellingValue8 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 7); }
        public string LabellingKey9 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 8); }
        public string LabellingValue9 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 8); }
        public string LabellingKey10 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 9); }
        public string LabellingValue10 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 9); }
        public string LabellingKey11 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 10); }
        public string LabellingValue11 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 10); }
        public string LabellingKey12 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 11); }
        public string LabellingValue12 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 11); }
        public string LabellingKey13 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 12); }
        public string LabellingValue13 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 12); }
        public string LabellingKey14 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 13); }
        public string LabellingValue14 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 13); }
        public string LabellingKey15 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 14); }
        public string LabellingValue15 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 14); }
        public string LabellingKey16 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 15); }
        public string LabellingValue16 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 15); }
        public string LabellingKey17 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 16); }
        public string LabellingValue17 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 16); }
        public string LabellingKey18 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 17); }
        public string LabellingValue18 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 17); }
        public string LabellingKey19 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 18); }
        public string LabellingValue19 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 18); }
        public string LabellingKey20 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, true, 19); }
        public string LabellingValue20 { get => GetKeyOrValueAt(ReleaseBlock.Labelling, false, 19); }
        #endregion
        #region testing
        public string TestingKey1 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 0); }
        public string TestingValue1 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 0); }
        public string TestingKey2 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 1); }
        public string TestingValue2 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 1); }
        public string TestingKey3 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 2); }
        public string TestingValue3 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 2); }
        public string TestingKey4 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 3); }
        public string TestingValue4 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 3); }
        public string TestingKey5 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 4); }
        public string TestingValue5 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 4); }
        public string TestingKey6 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 5); }
        public string TestingValue6 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 5); }
        public string TestingKey7 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 6); }
        public string TestingValue7 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 6); }
        public string TestingKey8 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 7); }
        public string TestingValue8 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 7); }
        public string TestingKey9 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 8); }
        public string TestingValue9 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 8); }
        public string TestingKey10 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 9); }
        public string TestingValue10 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 9); }
        public string TestingKey11 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 10); }
        public string TestingValue11 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 10); }
        public string TestingKey12 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 11); }
        public string TestingValue12 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 11); }
        public string TestingKey13 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 12); }
        public string TestingValue13 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 12); }
        public string TestingKey14 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 13); }
        public string TestingValue14 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 13); }
        public string TestingKey15 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 14); }
        public string TestingValue15 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 14); }
        public string TestingKey16 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 15); }
        public string TestingValue16 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 15); }
        public string TestingKey17 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 16); }
        public string TestingValue17 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 16); }
        public string TestingKey18 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 17); }
        public string TestingValue18 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 17); }
        public string TestingKey19 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 18); }
        public string TestingValue19 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 18); }
        public string TestingKey20 { get => GetKeyOrValueAt(ReleaseBlock.Testing, true, 19); }
        public string TestingValue20 { get => GetKeyOrValueAt(ReleaseBlock.Testing, false, 19); }
        #endregion

        #endregion

        #region constructor

        public ReleaseDefinition(string plataform, string sample, string partNumber, string internalName, string descriptionReference, bool trazability, string date,
                                    bool electronicBlock, bool mechanicalBlock, bool softwareBlock, bool processBlock, bool labellingBlock, bool testingBlock,
                                    Dictionary<string, List<string>> electronicHardware,
                                    Dictionary<string, string> mechanicalHardware,
                                    Dictionary<string, string> softwareHardware,
                                    string process,
                                    Dictionary<string, string> labellingHardware,
                                    Dictionary<string, string> testingHardware)
        {
            ProjectName = plataform;
            Sample = sample;
            PartNumber = partNumber;
            InternalName = internalName;
            DescriptionReference = descriptionReference;
            Trazability = trazability;
            Date = date;
            this.electronicBlock = electronicBlock;
            this.mechanicalBlock = mechanicalBlock;
            this.softwareBlock = softwareBlock;
            this.processBlock = processBlock;
            this.labellingBlock = labellingBlock;
            this.testingBlock = testingBlock;
            ElectronicHardware = electronicHardware;
            MechanicalHardware = mechanicalHardware;
            SoftwareHardware = softwareHardware;
            Process = process;
            LabellingHardware = labellingHardware;
            TestingHardware = testingHardware;
        }
        public ReleaseDefinition(string plataform, string sample, string partNumber, string internalName, string descriptionReference, string date, 
                                    bool electronicBlock, bool mechanicalBlock, bool softwareBlock, bool processBlock, bool labellingBlock, bool testingBlock)
        {
            this.projectName = plataform;
            this.sample = sample;
            this.partNumber = partNumber;
            this.internalName = internalName;
            this.descriptionReference = descriptionReference;
            this.date = date;
            this.electronicBlock = electronicBlock;
            this.mechanicalBlock = mechanicalBlock;
            this.softwareBlock = softwareBlock;
            this.processBlock = processBlock;
            this.labellingBlock = labellingBlock;
            this.testingBlock = testingBlock;
        }
        public ReleaseDefinition(string plataform, string sample, string partNumber, string internalName, string descriptionReference, bool trazability, string date)
        {
            ProjectName = plataform;
            Sample = sample;
            PartNumber = partNumber;
            InternalName = internalName;
            DescriptionReference = descriptionReference;
            Trazability = trazability;
            Date = date;
        }
        #endregion

        #region metodos privados
        private string GetKeyOrValueAt(ReleaseBlock block, bool key, int position, int subposition = 0) //Subposition only for ElectronicBlock
        {
            switch (block)
            {
                case ReleaseBlock.Electronic:
                    if (position >= ElectronicHardware.Count)
                    {
                        position = -1;
                    }
                    return position == -1 ? "" : key ? $"{ElectronicHardware.ElementAt(position).Key}:" : ElectronicHardware.ElementAt(position).Value.ElementAt(subposition) ?? "";

                case ReleaseBlock.Mechanical:
                    if (position >= MechanicalHardware.Count)
                    {
                        position = -1;
                    }
                    return position == -1 ? "" : key ? $"{MechanicalHardware.ElementAt(position).Key}:" : MechanicalHardware.ElementAt(position).Value;
                case ReleaseBlock.Software:
                    if (position >= SoftwareHardware.Count)
                    {
                        position = -1;
                    }
                    return position == -1 ? "" : key ? $"{SoftwareHardware.ElementAt(position).Key}:" : SoftwareHardware.ElementAt(position).Value;
                case ReleaseBlock.Labelling:
                    if (position >= LabellingHardware.Count)
                    {
                        position = -1;
                    }
                    return position == -1 ? "" : key ? $"{LabellingHardware.ElementAt(position).Key}:" : LabellingHardware.ElementAt(position).Value;
                case ReleaseBlock.Testing:
                    if (position >= TestingHardware.Count)
                    {
                        position = -1;
                    }
                    return position == -1 ? "" : key ? $"{TestingHardware.ElementAt(position).Key}:" : TestingHardware.ElementAt(position).Value;
                default: return "";
            }
        }
        #endregion
    }
    public enum ReleaseBlock
    {
        Electronic, Mechanical, Software, Labelling, Testing
    }
}
