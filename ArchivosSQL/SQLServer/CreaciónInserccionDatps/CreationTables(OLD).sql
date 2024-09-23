USE XEDI_TraceabilityDB
GO

--------------------------------------------------------------------------------------------------------------------------
-- Borrado de tablas (en orden inverso de creación)
-- ¡¡¡ESTA ACCIÓN BORRARÁ LOS DATOS DE LAS TABLAS!!!

DROP TABLE IF EXISTS ManufacturingRegister_Coment;
DROP TABLE IF EXISTS ManufacturingComent;
DROP TABLE IF EXISTS ManufacturingRegister_Item;
DROP TABLE IF EXISTS ManufacturingItem;
DROP TABLE IF EXISTS ManufacturingRegister_SN;
DROP TABLE IF EXISTS ManufacturingSN;
DROP TABLE IF EXISTS ManufacturingRegister_Input;
DROP TABLE IF EXISTS ManufacturingInput;
DROP TABLE IF EXISTS ManufacturingDefinitionInput;
DROP TABLE IF EXISTS ManufacturingRegister_Screw;
DROP TABLE IF EXISTS ManufacturingScrew;
DROP TABLE IF EXISTS ManufacturingRegister;
DROP TABLE IF EXISTS Result;
--------------------------------------------------------------------------------------------------------------------------
DROP TABLE IF EXISTS DefinitionProcess_Station;
DROP TABLE IF EXISTS Process_ScrewDriver_Definition;
DROP TABLE IF EXISTS DefinitionScrewDriver;
DROP TABLE IF EXISTS Process_Item_Definition;
DROP TABLE IF EXISTS DefinitionItem;
DROP TABLE IF EXISTS Process_Itnput_Definition;
DROP TABLE IF EXISTS DefinitionInput;
DROP TABLE IF EXISTS Process_Test_Definition;
DROP TABLE IF EXISTS DefinitionProcess;
DROP TABLE IF EXISTS ProcessDescription;
--------------------------------------------------------------------------------------------------------------------------
DROP TABLE IF EXISTS TestSteepDefinition;
DROP TABLE IF EXISTS TestSteep;
--------------------------------------------------------------------------------------------------------------------------
DROP TABLE IF EXISTS ItemDefinition_DefinitionTest;
DROP TABLE IF EXISTS DefinitionTest;
DROP TABLE IF EXISTS ItemDefinition_VersionProcess;
DROP TABLE IF EXISTS VersionProcess;
DROP TABLE IF EXISTS ItemDefinition_VersionLabelling;
DROP TABLE IF EXISTS VersionLabelling;
DROP TABLE IF EXISTS LabellingName;
DROP TABLE IF EXISTS ItemDefinition_VersionSoftware;
DROP TABLE IF EXISTS VersionSoftware;
DROP TABLE IF EXISTS SoftwareName;
DROP TABLE IF EXISTS ItemDefinition_VersionMechanical;
DROP TABLE IF EXISTS VersionMechanical;
DROP TABLE IF EXISTS MechanicalName;
DROP TABLE IF EXISTS Item_ReworkPCB_Definition;
--------------------------------------------------------------------------------------------------------------------------
DROP TABLE IF EXISTS ManufacturingReworkPCB;
DROP TABLE IF EXISTS SNRegisterPCB;
DROP TABLE IF EXISTS Item_ReworkPCB_Definition;
DROP TABLE IF EXISTS ReworkDefinitionPCB;
DROP TABLE IF EXISTS BomDefinition_ValidReworkPCB;
DROP TABLE IF EXISTS ValidReworkPCB;
DROP TABLE IF EXISTS BomDefinitionPCB;
DROP TABLE IF EXISTS LayoutDefinitionPCB;
--------------------------------------------------------------------------------------------------------------------------
DROP TABLE IF EXISTS SNWindchill;
DROP TABLE IF EXISTS SNStatus;
DROP TABLE IF EXISTS StatusType;
DROP TABLE IF EXISTS SNPendingOperationManufacturing;
DROP TABLE IF EXISTS PurposeType_SNRegister;
DROP TABLE IF EXISTS PurposeType;
DROP TABLE IF EXISTS SNClientCode_SNRegister;
DROP TABLE IF EXISTS SNRegister;
DROP TABLE IF EXISTS SNClientCode;
DROP TABLE IF EXISTS SNClientRestriction;
DROP TABLE IF EXISTS SNClientStatus;
DROP TABLE IF EXISTS OrderFabrication;
--------------------------------------------------------------------------------------------------------------------------
DROP TABLE IF EXISTS ItemDefinition;
DROP TABLE IF EXISTS ItemDescription;
DROP TABLE IF EXISTS ItemPartNumber;
DROP TABLE IF EXISTS ItemSample;
DROP TABLE IF EXISTS ItemCategory;
DROP TABLE IF EXISTS ItemReferencePCB;
DROP TABLE IF EXISTS ItemProject;
--------------------------------------------------------------------------------------------------------------------------
DROP TABLE IF EXISTS Peripheric;
DROP TABLE IF EXISTS PeriphericDefinition;
DROP TABLE IF EXISTS RolCenter_Station;
DROP TABLE IF EXISTS Station;
DROP TABLE IF EXISTS RolDefinition_Center;
DROP TABLE IF EXISTS RolCenter;
DROP TABLE IF EXISTS RolDefinition;
DROP TABLE IF EXISTS PictureStorage;

--------------------------------------------------------------------------------------------------------------------------
--Creación de tablas

CREATE TABLE PictureStorage (
	IdPictureStorage INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	PictureName NVARCHAR(255) NOT NULL,
	PictureData VARBINARY(MAX) NOT NULL
)
GO

CREATE TABLE RolDefinition (
	IdRol INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	RolDescription NVARCHAR(30)
)
GO

CREATE TABLE RolCenter (
	IdRolCenter INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ComputerName NVARCHAR(30) NOT NULL,
	UserCode VARCHAR (20) UNIQUE,
	CenterIP NVARCHAR(20) UNIQUE,
	HostName NVARCHAR(20) UNIQUE
)
GO

CREATE TABLE RolDefinition_Center (
	IdRol INT NOT NULL,
	IdRolCenter INT NOT NULL
	PRIMARY KEY(IdRol, IdRolCenter),
	CONSTRAINT FK_RolDefinition_RolDefinitionCenter FOREIGN KEY (IdRol) REFERENCES RolDefinition(IdRol),
	CONSTRAINT FK_RolCenter_RolDefinitionCenter FOREIGN KEY (IdRolCenter) REFERENCES RolCenter(IdRolCenter)
)
GO

CREATE TABLE Station (
	IdStation INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	StationName NVARCHAR(30) NOT NULL
)
GO

CREATE TABLE RolCenter_Station (
	IdRolCenter INT NOT NULL,
	IdStation INT NOT NULL,
	Zocalo INT NOT NULL,
	PRIMARY KEY(IdRolCenter, IdStation),
	CONSTRAINT FK_RolCenter_RolCenter_Station FOREIGN KEY (IdRolCenter) REFERENCES RolCenter(IdRolCenter),
	CONSTRAINT FK_Station_RolCenter_Station FOREIGN KEY (IdStation) REFERENCES Station(IdStation)
)
GO

CREATE TABLE PeriphericDefinition (
	IdPeriphericDefinition INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	PeriphericDescription NVARCHAR(30) NOT NULL
)
GO

CREATE TABLE Peripheric (
	IdPeripheric INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdPeriphericDefinition INT NOT NULL,
	IdRolCenter INT NOT NULL,
	CONSTRAINT FK_PeriphericDefinition_Peripheric FOREIGN KEY (IdPeriphericDefinition) REFERENCES PeriphericDefinition(IdPeriphericDefinition),
	CONSTRAINT FK_RolCenter_Peripheric FOREIGN KEY (IdRolCenter) REFERENCES RolCenter(IdRolCenter)
)
GO

--------------------------------------------------------------------------------------------------------------------------

CREATE TABLE ItemProject (
	IdItemProject INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ProjectName NVARCHAR(60) NOT NULL,
	IdPicture INT
)
GO

CREATE TABLE ItemReferencePCB (
	IdItemReferencePCB INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdItemProject INT,
	ItemDescription NVARCHAR(50) NOT NULL,
	Reference NVARCHAR(20) NOT NULL,
	CONSTRAINT FK_ItemProject_ItemReferencePCB FOREIGN KEY (IdItemProject) REFERENCES ItemProject(IdItemProject)
)
GO

CREATE TABLE ItemCategory (
	IdItemCategory INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	CategoryDescription NVARCHAR(40) NOT NULL
)
GO

CREATE TABLE ItemSample (
	IdItemSample INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdItemProject INT,
	ItemSampleName NVARCHAR(30) NOT NULL,
	CONSTRAINT FK_ItemProject_ItemSample FOREIGN KEY (IdItemProject) REFERENCES ItemProject(IdItemProject)
)
GO

CREATE TABLE ItemPartNumber (
	IdItemPN INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdItemSample INT,
	ItemPNName NVARCHAR(60) NOT NULL,
	CONSTRAINT FK_ItemSample_ItemPartNumber FOREIGN KEY (IdItemSample) REFERENCES ItemSample(IdItemSample)
)
GO

CREATE TABLE ItemDescription (
	IdItemDescription INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	DescriptionItem NVARCHAR(60) NOT NULL
)
GO

CREATE TABLE ItemDefinition (
	IdItemDefinition INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdItemDescription INT,
	IdItemPN INT,
	InternalName NVARCHAR(30) NOT NULL,
	DescriptionReference NVARCHAR(60) NOT NULL,
	IsTrazability BIT,
	EnableItem BIT,
	IdPictureStorage INT,
	DateRegistration SMALLDATETIME,
	IdRolCenter INT,
	CONSTRAINT FK_ItemDescription_ItemDefinition FOREIGN KEY (IdItemDescription) REFERENCES ItemDescription(IdItemDescription),
	CONSTRAINT FK_ItemPartNumber_ItemDefinition FOREIGN KEY (IdItemPN) REFERENCES ItemPartNumber(IdItemPN)
)
GO

CREATE OR ALTER TRIGGER RolCenter_ItemDefinition ON ItemDefinition
	FOR INSERT, UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM RolCenter 
						WHERE IdRolCenter IN (SELECT IdRolCenter FROM inserted))
	BEGIN; -- Instruction befor THROW has to end with semicolon
		THROW 50001, 'IdRolComputer no encontrado en la BBDD.', 1
		ROLLBACK TRANSACTION
	END
	ELSE IF (SELECT IdPictureStorage FROM inserted) IS NOT NULL AND
			NOT EXISTS (SELECT * FROM PictureStorage 
						WHERE IdPictureStorage  IN (SELECT IdPictureStorage FROM inserted)) 
	BEGIN; -- Instruction befor THROW has to end with semicolon
		THROW 50002, 'IdPictureStorage no encontrado en la BBDD.', 1
		ROLLBACK TRANSACTION
	END
END
GO

--------------------------------------------------------------------------------------------------------------------------

CREATE TABLE OrderFabrication (
	IdOF INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdItemDefinition INT,
	CodeOF NVARCHAR(20) NOT NULL,
	DescritionOF NVARCHAR(200),
	Quantity INT,
	IdRolCenter INT,
	CONSTRAINT FK_ItemDefinition_OrderFabrication FOREIGN KEY (IdItemDefinition) REFERENCES ItemDefinition(IdItemDefinition)
)
GO

CREATE OR ALTER TRIGGER RolCenter_OrderFabrication ON OrderFabrication
	FOR INSERT, UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM RolCenter 
						WHERE IdRolCenter IN (SELECT IdRolCenter FROM inserted))
	BEGIN; -- Instruction befor THROW has to end with semicolon
		THROW 50001, 'IdRolComputer no encontrado en la BBDD.', 1
		ROLLBACK TRANSACTION
	END
END
GO

CREATE TABLE SNClientStatus (
	IdSNClientStatus INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	StatusType NVARCHAR(30) NOT NULL
)
GO

CREATE TABLE SNClientRestriction (
	IdSNClientRestriction INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	RestrictionCode NVARCHAR(30) NOT NULL,
	RestrictionDescription NVARCHAR(255) NOT NULL
)
GO

CREATE TABLE SNClientCode (
	IdSNClientCode INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdItemProject INT NOT NULL,
	IdSNClientStatus INT NOT NULL,
	IdSNClientRestriction INT NOT NULL,
	ClientCode NVARCHAR(20) NOT NULL,
	CONSTRAINT FK_ItemProject_SNClientCode FOREIGN KEY (IdItemProject) REFERENCES ItemProject(IdItemProject),
	CONSTRAINT FK_SNClientStatuss_SNClientCode FOREIGN KEY (IdSNClientStatus) REFERENCES SNClientStatus(IdSNClientStatus),
	CONSTRAINT FK_SNClientRestriction_SNClientCode FOREIGN KEY (IdSNClientRestriction) REFERENCES SNClientRestriction(IdSNClientRestriction)
)
GO

CREATE TABLE SNRegister (
	IdSNRegister INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdOF INT,
	IdItemProject INT,
	IdItemCategory INT,
	CodeSN NVARCHAR(30) NOT NULL UNIQUE,
	IdRolCenter INT,
	DateRegistration SMALLDATETIME,
	CONSTRAINT FK_OrderFabrication_SNRegister FOREIGN KEY (IdOF) REFERENCES OrderFabrication(IdOF),
	CONSTRAINT FK_ItemProject_SNRegister FOREIGN KEY (IdItemProject) REFERENCES ItemProject(IdItemProject),
	CONSTRAINT FK_ItemCategory_SNRegister FOREIGN KEY (IdItemCategory) REFERENCES ItemCategory(IdItemCategory)
)
GO

CREATE TABLE SNClientCode_SNRegister (
	IdSNClientCode INT NOT NULL,
	IdSNRegister INT NOT NULL,
	DateAsignation SMALLDATETIME,
	PRIMARY KEY(IdSNClientCode, IdSNRegister),
	CONSTRAINT FK_FINASRegister_SNClientCodeSNRegister FOREIGN KEY (IdSNClientCode) REFERENCES SNClientCode(IdSNClientCode),
	CONSTRAINT FK_SNRegister_SNClientCodeSNRegister FOREIGN KEY (IdSNRegister) REFERENCES SNRegister(IdSNRegister)
)
GO

CREATE OR ALTER TRIGGER RolCenter_SNRegister ON SNRegister
	FOR INSERT, UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM RolCenter 
						WHERE IdRolCenter  IN (SELECT IdRolCenter FROM inserted))
	BEGIN; -- Instruction befor THROW has to end with semicolon
		THROW 50001, 'IdRolComputer no encontrado en la BBDD.', 1
		ROLLBACK TRANSACTION
	END
END
GO

CREATE TABLE PurposeType (
	IdPurposeType INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	PurposeDefinition NVARCHAR(20) NOT NULL
)
GO

CREATE TABLE PurposeType_SNRegister (
	IdPurposeType INT NOT NULL,
	IdSNRegister INT NOT NULL,
	PRIMARY KEY(IdPurposeType, IdSNRegister),
	CONSTRAINT FK_PurposeType_PurposeTypeSNRegister FOREIGN KEY (IdPurposeType) REFERENCES PurposeType(IdPurposeType),
	CONSTRAINT FK_SNRegister_PurposeTypeSNRegister FOREIGN KEY (IdSNRegister) REFERENCES SNRegister(IdSNRegister)
)
GO

CREATE TABLE SNPendingOperationManufacturing (
	IdSNPendingOperation INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSNRegister INT NOT NULL,
	DateRegister SMALLDATETIME NOT NULL
	CONSTRAINT FK_SNRegister_SNPendingOperationManufacturing FOREIGN KEY (IdSNRegister) REFERENCES SNRegister(IdSNRegister)
)
GO

CREATE TABLE StatusType (
	IdStatus INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	StatusValue NVARCHAR(30) NOT NULL
)
GO

CREATE TABLE SNStatus (
	IdSNStatus INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdStatus INT NOT NULL,
	IdSNRegister INT NOT NULL,
	DateRegister SMALLDATETIME NOT NULL,
	CONSTRAINT FK_StatusType_SNStatus FOREIGN KEY (IdStatus) REFERENCES StatusType(IdStatus),
	CONSTRAINT FK_SNRegister_SNStatus FOREIGN KEY (IdSNRegister) REFERENCES SNRegister(IdSNRegister)
)
GO

CREATE TABLE SNWindchill (
	IdSNWindchill INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSNRegister INT NOT NULL UNIQUE,
	WindchillCode NVARCHAR(30)
	CONSTRAINT FK_SNRegister_SNWindchill FOREIGN KEY (IdSNRegister) REFERENCES SNRegister(IdSNRegister)
)
GO

--------------------------------------------------------------------------------------------------------------------------

CREATE TABLE LayoutDefinitionPCB (
	IdLayoutDefinitionPCB INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdItemReference INT NOT NULL,
	Layout INT NOT NULL
	CONSTRAINT FK_ItemReference_LayoutDefinitionPCB FOREIGN KEY (IdItemReference) REFERENCES ItemReferencePCB(IdItemReferencePCB)
)
GO

CREATE TABLE BomDefinitionPCB (
	IdBomDefinitionPCB INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdLayoutDefinitionPCB INT NOT NULL,
	Bom INT NOT NULL
	CONSTRAINT FK_LayoutDefinitionPCB_BomDefinitionPCB FOREIGN KEY (IdLayoutDefinitionPCB) REFERENCES LayoutDefinitionPCB(IdLayoutDefinitionPCB)
)
GO

CREATE TABLE ValidReworkPCB (
	IdValidReworkPCB INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ValidRework INT NOT NULL,
	ReworkDescription NVARCHAR(50)NOT NULL
)
GO

CREATE TABLE BomDefinition_ValidReworkPCB (
	IdBomDefinitionPCB INT NOT NULL,
	IdValidReworkPCB INT NOT NULL,
	PRIMARY KEY(IdBomDefinitionPCB, IdValidReworkPCB),
	CONSTRAINT FK_BomDefinitionPCB_BomDefinitionValidReworkPCB FOREIGN KEY (IdBomDefinitionPCB) REFERENCES BomDefinitionPCB(IdBomDefinitionPCB),
	CONSTRAINT FK_ValidReworkPCB_BomDefinitionValidReworkPCB FOREIGN KEY (IdValidReworkPCB) REFERENCES ValidReworkPCB(IdValidReworkPCB)
)
GO

CREATE TABLE ReworkDefinitionPCB (
	IdReworkDefinitionPCB INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdBomDefinitionPCB INT NOT NULL,
	ReworkDefinition INT NOT NULL,
	Windchill NVARCHAR(30)NOT NULL,
	CONSTRAINT FK_BomDefinitionPCB_ReworkDefinitionPCB FOREIGN KEY (IdBomDefinitionPCB) REFERENCES BomDefinitionPCB(IdBomDefinitionPCB)
)
GO

CREATE TABLE SNRegisterPCB (
	IdSNRegisterPCB INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSNRegister INT NOT NULL UNIQUE,
	IdBomDefinitionPCB INT NOT NULL,
	Batch NVARCHAR(20) NOT NULL,
	CONSTRAINT FK_SNRegister_SNRegisterPCB FOREIGN KEY (IdSNRegister) REFERENCES SNRegister(IdSNRegister),
	CONSTRAINT FK_BomDefinitionPCB_SNRegisterPCB FOREIGN KEY (IdBomDefinitionPCB) REFERENCES BomDefinitionPCB(IdBomDefinitionPCB)
)
GO

CREATE TABLE ManufacturingReworkPCB (
	IdManufacturingReworkPCB INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSNRegisterPCB INT NOT NULL,
	Rework INT NOT NULL,
	IdRolCenter INT NOT NULL,
	RegistrationDate SMALLDATETIME NOT NULL
	CONSTRAINT FK_SNRegisterPCB_ManufacturingReworkPCB FOREIGN KEY (IdSNRegisterPCB) REFERENCES SNRegisterPCB(IdSNRegisterPCB)
)
GO

CREATE OR ALTER TRIGGER RolCenter_ManufacturingReworkPCB ON ManufacturingReworkPCB
	FOR INSERT, UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM RolCenter 
						WHERE IdRolCenter  IN (SELECT IdRolCenter FROM inserted))
	BEGIN; -- Instruction befor THROW has to end with semicolon
		THROW 50001, 'IdRolComputer no encontrado en la BBDD.', 1
		ROLLBACK TRANSACTION
	END
END
GO

--------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Item_ReworkPCB_Definition (
	IdItemDefinition INT NOT NULL,
	IdReworkDefinitionPCB INT NOT NULL,
	PRIMARY KEY(IdItemDefinition, IdReworkDefinitionPCB),
	CONSTRAINT FK_ItemDefinition_ItemDefinitionBomDefinitionPCB FOREIGN KEY (IdItemDefinition) REFERENCES ItemDefinition(IdItemDefinition),
	CONSTRAINT FK_BomDefinitionPCB_ItemDefinitionBomDefinitionPCB FOREIGN KEY (IdReworkDefinitionPCB) REFERENCES ReworkDefinitionPCB(IdReworkDefinitionPCB)
)
GO

CREATE TABLE MechanicalName (
	IdMechanicalName INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	MechanicalDescription NVARCHAR(50) NOT NULL
)
GO

CREATE TABLE VersionMechanical (
	IdVersionMechanical INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdMechanicalName INT NOT NULL ,
	MechanicalData NVARCHAR(20) NOT NULL,
	CONSTRAINT FK_MechanicalName_VersionMechanical FOREIGN KEY (IdMechanicalName) REFERENCES MechanicalName(IdMechanicalName)
)
GO

CREATE TABLE ItemDefinition_VersionMechanical (
	IdItemDefinition INT NOT NULL,
	IdVersionMechanical INT NOT NULL,
	PRIMARY KEY(IdItemDefinition, IdVersionMechanical),
	CONSTRAINT FK_ItemDefinition_ItemDefinitionVersionMechanical FOREIGN KEY (IdItemDefinition) REFERENCES ItemDefinition(IdItemDefinition),
	CONSTRAINT FK_VersionMechanical_ItemDefinitionVersionMechanical FOREIGN KEY (IdVersionMechanical) REFERENCES VersionMechanical(IdVersionMechanical)
)
GO

CREATE TABLE SoftwareName (
	IdSoftwareName INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	SoftwareDescription NVARCHAR(50) NOT NULL
)
GO

CREATE TABLE VersionSoftware (
	IdVersionSoftware INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSoftwareName INT NOT NULL ,
	SoftwareData NVARCHAR(20) NOT NULL,
	CONSTRAINT FK_SoftwareName_VersionSoftware FOREIGN KEY (IdSoftwareName) REFERENCES SoftwareName(IdSoftwareName)
)
GO

CREATE TABLE ItemDefinition_VersionSoftware (
	IdItemDefinition INT NOT NULL,
	IdVersionSoftware INT NOT NULL,
	PRIMARY KEY(IdItemDefinition, IdVersionSoftware),
	CONSTRAINT FK_ItemDefinition_ItemDefinitionVersionSoftware FOREIGN KEY (IdItemDefinition) REFERENCES ItemDefinition(IdItemDefinition),
	CONSTRAINT FK_VersionSoftware_ItemDefinitionVersionSoftware FOREIGN KEY (IdVersionSoftware) REFERENCES VersionSoftware(IdVersionSoftware)
)
GO

CREATE TABLE LabellingName (
	IdLabellingName INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	LabellingDescription NVARCHAR(50) NOT NULL
)
GO

CREATE TABLE VersionLabelling (
	IdVersionLabelling INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdLabellingName INT NOT NULL ,
	LabellingData NVARCHAR(20) NOT NULL,
	CONSTRAINT FK_LabellingName_VersionLabelling FOREIGN KEY (IdLabellingName) REFERENCES LabellingName(IdLabellingName)
)
GO

CREATE TABLE ItemDefinition_VersionLabelling (
	IdItemDefinition INT NOT NULL,
	IdVersionLabelling INT NOT NULL,
	PRIMARY KEY(IdItemDefinition, IdVersionLabelling),
	CONSTRAINT FK_ItemDefinition_ItemDefinitionVersionLabelling FOREIGN KEY (IdItemDefinition) REFERENCES ItemDefinition(IdItemDefinition),
	CONSTRAINT FK_VersionLabelling_ItemDefinitionVersionLabelling FOREIGN KEY (IdVersionLabelling) REFERENCES VersionLabelling(IdVersionLabelling)
)
GO

CREATE TABLE VersionProcess (
	IdVersionProcess INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ProcessName NVARCHAR(40) NOT NULL
)
GO

CREATE TABLE ItemDefinition_VersionProcess (
	IdItemDefinition INT NOT NULL PRIMARY KEY,
	IdVersionProcess INT NOT NULL,
	CONSTRAINT FK_ItemDefinition_ItemDefinitionVersionProcess FOREIGN KEY (IdItemDefinition) REFERENCES ItemDefinition(IdItemDefinition),
	CONSTRAINT FK_VersionProcess_ItemDefinitionVersionProcess FOREIGN KEY (IdVersionProcess) REFERENCES VersionProcess(IdVersionProcess)
)
GO

CREATE TABLE DefinitionTest (
	IdDefinitionTest INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	TestDescription NVARCHAR(30) NOT NULL,
	IdStation INT NOT NULL,
	CONSTRAINT FK_Station_DefinitionTest FOREIGN KEY (IdStation) REFERENCES Station(IdStation)
)
GO

CREATE TABLE ItemDefinition_DefinitionTest (
	IdItemDefinition INT NOT NULL,
	IdDefinitionTest INT NOT NULL,
	VersionTest INT NOT NULL,
	PRIMARY KEY(IdItemDefinition, IdDefinitionTest),
	CONSTRAINT FK_ItemDefinition_ItemDefinitionDefinitionTest FOREIGN KEY (IdItemDefinition) REFERENCES ItemDefinition(IdItemDefinition),
	CONSTRAINT FK_DefinitionTest_ItemDefinitionDefinitionTest FOREIGN KEY (IdDefinitionTest) REFERENCES DefinitionTest(IdDefinitionTest)
)
GO

--------------------------------------------------------------------------------------------------------------------------

CREATE TABLE TestSteep (
	IdTestSteep INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdDefinitionTest INT NOT NULL,
	TestName NVARCHAR(30),
	TestVersion INT,
	ExecutionOrder INT,
	RegistrationDate SMALLDATETIME,
	CONSTRAINT FK_IdDefinitionTest_TestSteep FOREIGN KEY (IdDefinitionTest) REFERENCES DefinitionTest(IdDefinitionTest)
)
GO

CREATE TABLE TestSteepDefinition (
	IdTestSteepDefinition INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdTestSteep INT NOT NULL,
	IdPeriphericDefinition INT NOT NULL,
	Command NVARCHAR(100),
	LimL NVARCHAR(15),
	LimH NVARCHAR(15),
	Units  NVARCHAR(10),
	Gain  NVARCHAR(10),
	Offset  NVARCHAR(10),
	Notes NVARCHAR(100),
	Report BIT,
	CONSTRAINT FK_TestSteep_TestSteepDefinition FOREIGN KEY (IdTestSteep) REFERENCES TestSteep(IdTestSteep),
	CONSTRAINT FK_PeriphericDefinition_TestSteepDefinition FOREIGN KEY (IdPeriphericDefinition) REFERENCES PeriphericDefinition(IdPeriphericDefinition)
)
GO

--------------------------------------------------------------------------------------------------------------------------

CREATE TABLE ProcessDescription (
	IdProcessDescription INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ProcessDescription NVARCHAR(40) NOT NULL
)
GO

CREATE TABLE DefinitionProcess (
	IdDefinitionProcess INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdVersionProcess INT NOT NULL,
	Num INT NOT NULL,
	ProcessNum INT NOT NULL,
	IdProcessDescription INT NOT NULL,
	Operation INT NOT NULL,
	OperationDescription NVARCHAR(60),
	IdPicture INT,
	DateRegistration SMALLDATETIME,
	CONSTRAINT FK_VersionProcess_DefinitionProcess FOREIGN KEY (IdVersionProcess) REFERENCES VersionProcess(IdVersionProcess),
	CONSTRAINT FK_ProcessDescription_DefinitionProcess FOREIGN KEY (IdProcessDescription) REFERENCES ProcessDescription(IdProcessDescription)
)
GO

CREATE OR ALTER TRIGGER Picture_DefinitionProcess ON DefinitionProcess
	FOR INSERT, UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM PictureStorage 
						WHERE IdPictureStorage  IN (SELECT IdPictureStorage FROM inserted))
	BEGIN; -- Instruction befor THROW has to end with semicolon
		THROW 50002, 'IdPictureStorage no encontrado en la BBDD.', 1
		ROLLBACK TRANSACTION
	END
END

GO

CREATE TABLE Process_Test_Definition (
	IdDefinitionProcess INT NOT NULL PRIMARY KEY,
	IdDefinitionTest INT NOT NULL,
	CONSTRAINT FK_DefinitionProcess_ProcessTestDefinition FOREIGN KEY (IdDefinitionProcess) REFERENCES DefinitionProcess(IdDefinitionProcess),
	CONSTRAINT FK_DefinitionTest_ProcessTestDefinition FOREIGN KEY (IdDefinitionTest) REFERENCES DefinitionTest(IdDefinitionTest)
)
GO

CREATE TABLE DefinitionInput (
	IdDefinitionInput INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	InputType NVARCHAR(20) NOT NULL -- Imagen, int, float, string
)
GO

CREATE TABLE Process_Itnput_Definition (
	IdDefinitionProcess INT NOT NULL,
	IdDefinitionInput INT NOT NULL,
	PRIMARY KEY(IdDefinitionProcess, IdDefinitionInput),
	CONSTRAINT FK_DefinitionProcess_ProcessItnputDefinition FOREIGN KEY (IdDefinitionProcess) REFERENCES DefinitionProcess(IdDefinitionProcess),
	CONSTRAINT FK_DefinitionInput_ProcessItnputDefinition FOREIGN KEY (IdDefinitionInput) REFERENCES DefinitionInput(IdDefinitionInput)
)
GO

CREATE TABLE DefinitionItem (
	IdDefinitionItem INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Reference NVARCHAR(20) NOT NULL 
)
GO

CREATE TABLE Process_Item_Definition (
	IdDefinitionProcess INT NOT NULL,
	IdDefinitionItem INT NOT NULL,
	PRIMARY KEY(IdDefinitionProcess, IdDefinitionItem),
	CONSTRAINT FK_DefinitionProcess_ProcessItemDefinition FOREIGN KEY (IdDefinitionProcess) REFERENCES DefinitionProcess(IdDefinitionProcess),
	CONSTRAINT FK_DefinitionItem_ProcessItemDefinition FOREIGN KEY (IdDefinitionItem) REFERENCES DefinitionItem(IdDefinitionItem)
)
GO

CREATE TABLE DefinitionScrewDriver (
	IdDefinitionScrewDriver INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ScrewCode NVARCHAR(30) NOT NULL
)
GO

CREATE TABLE Process_ScrewDriver_Definition (
	IdDefinitionProcess INT NOT NULL,
	IdDefinitionScrewDriver INT NOT NULL,
	PRIMARY KEY(IdDefinitionProcess, IdDefinitionScrewDriver),
	CONSTRAINT FK_DefinitionProcess_ProcessScrewDriverDefinition FOREIGN KEY (IdDefinitionProcess) REFERENCES DefinitionProcess(IdDefinitionProcess),
	CONSTRAINT FK_DefinitionScrewDriver_ProcessScrewDriverDefinition FOREIGN KEY (IdDefinitionScrewDriver) REFERENCES DefinitionScrewDriver(IdDefinitionScrewDriver)
)
GO



CREATE TABLE DefinitionProcess_Station (
	IdDefinitionProcess INT NOT NULL,
	IdStation INT NOT NULL,
	PRIMARY KEY(IdDefinitionProcess, IdStation),
	CONSTRAINT FK_DefinitionProcess_DefinitionProcessStation FOREIGN KEY (IdDefinitionProcess) REFERENCES DefinitionProcess(IdDefinitionProcess),
	CONSTRAINT FK_Station_DefinitionProcessStation FOREIGN KEY (IdStation) REFERENCES Station(IdStation)
)
GO

--------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Result (
	IdResult INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	TypeResult NVARCHAR(20) NOT NULL,
	ResultValue NVARCHAR(100)
)
GO

CREATE TABLE ManufacturingRegister (
	IdManufacturingRegister INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSn INT NOT NULL,
	IdResult INT NOT NULL,
	Num INT NOT NULL,
	DateStar SMALLDATETIME NOT NULL,
	DateEnd SMALLDATETIME,
	IdRolCenter INT NOT NULL,
	IdOF INT,  --OF actual (puede cambiar con el tiempo)
	CONSTRAINT FK_SNRegister_ManufacturingRegister FOREIGN KEY (IdSn) REFERENCES SNRegister(IdSNRegister),
	CONSTRAINT FK_Result_ManufacturingRegister FOREIGN KEY (IdResult) REFERENCES Result(IdResult)
)
GO

CREATE OR ALTER TRIGGER Center_ManufacturingRegister ON ManufacturingRegister
	FOR INSERT, UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM RolCenter 
						WHERE IdRolCenter IN (SELECT IdRolCenter FROM inserted))
	BEGIN; -- Instruction befor THROW has to end with semicolon
		THROW 50001, 'Centro no encontrado en la BBDD.', 1
		ROLLBACK TRANSACTION
	END
END
GO

CREATE TABLE ManufacturingDefinitionScrew (
	IdManufacturingDefinitionScrew INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ScrewDescription NVARCHAR(40)
)
GO

CREATE TABLE ManufacturingScrew (
	IdManufacturingDefinitionScrew INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdDefinitionScrew INT NOT NULL,
	ScrewValue INT NOT NULL,
	CONSTRAINT FK_DefinitionScrew_ManufacturingScrew FOREIGN KEY (IdManufacturingDefinitionScrew) REFERENCES ManufacturingDefinitionScrew(IdManufacturingDefinitionScrew),
)
GO

CREATE TABLE ManufacturingRegister_Screw (
	IdManufacturingRegister INT NOT NULL,
	IdManufacturingDefinitionScrew INT NOT NULL,
	PRIMARY KEY (IdManufacturingRegister, IdManufacturingDefinitionScrew),
	CONSTRAINT FK_ManufacturingRegister_ManufacturingRegisterScrew FOREIGN KEY (IdManufacturingRegister) REFERENCES ManufacturingRegister(IdManufacturingRegister),
	CONSTRAINT FK_ManufacturingScrew_ManufacturingRegisterScrew FOREIGN KEY (IdManufacturingDefinitionScrew) REFERENCES ManufacturingScrew(IdManufacturingDefinitionScrew)
)
GO

CREATE TABLE ManufacturingDefinitionInput (
	IdManufacturingDefinitionInput INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	InputDescription NVARCHAR(40)
)
GO

CREATE TABLE ManufacturingInput (
	IdManufacturingInput INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdManufacturingDefinitionInput INT NOT NULL,
	InputValue INT NOT NULL,
	CONSTRAINT FK_ManufacturingDefinitionInput_ManufacturingInput FOREIGN KEY (IdManufacturingDefinitionInput) REFERENCES ManufacturingDefinitionInput(IdManufacturingDefinitionInput),
)
GO

CREATE TABLE ManufacturingRegister_Input (
	IdManufacturingRegister INT NOT NULL,
	IdManufacturingInput INT NOT NULL,
	PRIMARY KEY (IdManufacturingRegister, IdManufacturingInput),
	CONSTRAINT FK_ManufacturingRegister_ManufacturingRegisterInput FOREIGN KEY (IdManufacturingRegister) REFERENCES ManufacturingRegister(IdManufacturingRegister),
	CONSTRAINT FK_ManufacturingInput_ManufacturingRegisterInput FOREIGN KEY (IdManufacturingInput) REFERENCES ManufacturingInput(IdManufacturingInput)
)
GO

CREATE TABLE ManufacturingSN (
	IdManufacturingSN INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSn INT NOT NULL
)
GO

CREATE OR ALTER TRIGGER SNRegister_ManufacturingSN ON ManufacturingSN
	FOR INSERT, UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM SNRegister 
						WHERE IdSNRegister IN (SELECT IdSn FROM inserted))
	BEGIN; -- Instruction befor THROW has to end with semicolon
		THROW 50004, 'SN no registrado en la BBDD.', 1
		ROLLBACK TRANSACTION
	END
END
GO

CREATE TABLE ManufacturingRegister_SN (
	IdManufacturingRegister INT NOT NULL PRIMARY KEY,
	IdManufacturingSN INT NOT NULL,
	CONSTRAINT FK_ManufacturingRegister_ManufacturingRegisteSN FOREIGN KEY (IdManufacturingRegister) REFERENCES ManufacturingRegister(IdManufacturingRegister),
	CONSTRAINT FK_ManufacturingScrew_ManufacturingRegisterSN FOREIGN KEY (IdManufacturingSN) REFERENCES ManufacturingSN(IdManufacturingSN)
)
GO

CREATE TABLE ManufacturingItem (
	IdManufacturingItem INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Lote NVARCHAR(20) NOT NULL
)
GO

CREATE TABLE ManufacturingRegister_Item (
	IdManufacturingRegister INT NOT NULL,
	IdManufacturingItem INT NOT NULL,
	PRIMARY KEY (IdManufacturingRegister, IdManufacturingItem),
	CONSTRAINT FK_ManufacturingRegister_ManufacturingRegisterItem FOREIGN KEY (IdManufacturingRegister) REFERENCES ManufacturingRegister(IdManufacturingRegister),
	CONSTRAINT FK_ManufacturingItem_ManufacturingRegisterItem FOREIGN KEY (IdManufacturingItem) REFERENCES ManufacturingItem(IdManufacturingItem)
)
GO

CREATE TABLE ManufacturingComent (
	IdManufacturingComent INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Coment NVARCHAR(255) NOT NULL
)
GO

CREATE TABLE ManufacturingRegister_Coment (
	IdManufacturingRegister INT NOT NULL PRIMARY KEY,
	IdManufacturingComent INT NOT NULL,
	CONSTRAINT FK_ManufacturingRegister_ManufacturingRegisterComent FOREIGN KEY (IdManufacturingRegister) REFERENCES ManufacturingRegister(IdManufacturingRegister),
	CONSTRAINT FK_ManufacturingComent_ManufacturingRegisterComent FOREIGN KEY (IdManufacturingComent) REFERENCES ManufacturingComent(IdManufacturingComent)
)
GO