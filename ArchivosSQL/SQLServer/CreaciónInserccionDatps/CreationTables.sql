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
DROP TABLE IF EXISTS ManufacturingParameterInput;
DROP TABLE IF EXISTS ManufacturingRegister_Screw;
DROP TABLE IF EXISTS ManufacturingScrew;
DROP TABLE IF EXISTS ManufacturingParameterScrew;
DROP TABLE IF EXISTS ManufacturingRegister;
DROP TABLE IF EXISTS ManufacturingUser_RolDefinition;
DROP TABLE IF EXISTS ManufacturingRolDefinition;
DROP TABLE IF EXISTS ManufacturingUser;
DROP TABLE IF EXISTS ManufacturingResult;
--------------------------------------------------------------------------------------------------------------------------
DROP TABLE IF EXISTS Process_Station_Definition;
DROP TABLE IF EXISTS Process_ScrewDriver_Definition;
DROP TABLE IF EXISTS ProcessScrewDriver;
DROP TABLE IF EXISTS Process_Item_Definition;
DROP TABLE IF EXISTS ProcessItem;
DROP TABLE IF EXISTS Process_Input_Definition;
DROP TABLE IF EXISTS ProcessInput;
DROP TABLE IF EXISTS Process_Test_Definition;
DROP TABLE IF EXISTS ProcessDefinition;
DROP TABLE IF EXISTS ProcessDescription;
--------------------------------------------------------------------------------------------------------------------------
DROP TABLE IF EXISTS ManufacturingRegister_StatisticsTest;
DROP TABLE IF EXISTS StatisticsTestResult;
DROP TABLE IF EXISTS TestStepDefinition;
DROP TABLE IF EXISTS TestStep;
--------------------------------------------------------------------------------------------------------------------------
DROP TABLE IF EXISTS ItemDefinition_ProcessTest;
DROP TABLE IF EXISTS ProcessTest;
DROP TABLE IF EXISTS ItemDefinition_VersionProcess;
DROP TABLE IF EXISTS VersionProcess;
DROP TABLE IF EXISTS ItemDefinition_VersionLabelling;
DROP TABLE IF EXISTS VersionLabelling;
DROP TABLE IF EXISTS LabellingParameter;
DROP TABLE IF EXISTS ItemDefinition_VersionSoftware;
DROP TABLE IF EXISTS VersionSoftware;
DROP TABLE IF EXISTS SoftwareParameter;
DROP TABLE IF EXISTS ItemDefinition_VersionMechanical;
DROP TABLE IF EXISTS VersionMechanical;
DROP TABLE IF EXISTS MechanicalParameter;
DROP TABLE IF EXISTS ItemDefinition_PCBLayoutBomRework;
--------------------------------------------------------------------------------------------------------------------------
DROP TABLE IF EXISTS ManufacturingReworkPCB;
DROP TABLE IF EXISTS SNRegisterPCB;
DROP TABLE IF EXISTS ItemLayoutBomRework_PCB;
DROP TABLE IF EXISTS ItemBomPCB_ReworkDefinitionPCB;
DROP TABLE IF EXISTS ReworkDefinitionPCB;
DROP TABLE IF EXISTS ItemBomPCB;
DROP TABLE IF EXISTS ItemLayoutPCB;
--------------------------------------------------------------------------------------------------------------------------
DROP TABLE IF EXISTS SNWindchill;
DROP TABLE IF EXISTS SNStatus_Scrap;
DROP TABLE IF EXISTS SNStatus;
DROP TABLE IF EXISTS StatusType;
DROP TABLE IF EXISTS SNPendingOperationManufacturing;
DROP TABLE IF EXISTS PurposeType_SNRegister;
DROP TABLE IF EXISTS PurposeType;
DROP TABLE IF EXISTS ClientCode_SNRegister;
DROP TABLE IF EXISTS OrderFabrication_SNRegister;
DROP TABLE IF EXISTS SNRegister;
DROP TABLE IF EXISTS ClientCode;
DROP TABLE IF EXISTS ClientRestriction;
DROP TABLE IF EXISTS ClientStatus;
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
DROP TABLE IF EXISTS Peripheric_Center;
DROP TABLE IF EXISTS PeriphericDefinition;
DROP TABLE IF EXISTS Center_Station;
DROP TABLE IF EXISTS Station;
DROP TABLE IF EXISTS RolDefinition_Center;
DROP TABLE IF EXISTS Center;
DROP TABLE IF EXISTS RolDefinition;
DROP TABLE IF EXISTS PictureStorage;
--------------------------------------------------------------------------------------------------------------------------
--Creación de tablas

CREATE TABLE PictureStorage (
	IdPictureStorage INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	PictureName NVARCHAR(255) NOT NULL UNIQUE,
	PictureData VARBINARY(MAX) --  NOT NULL
)
GO

CREATE TABLE RolDefinition (
	IdRol INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	RolDescription NVARCHAR(30)
)
GO

CREATE TABLE Center (
	IdCenter INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ComputerName NVARCHAR(15) NOT NULL,
	UserName VARCHAR (10) UNIQUE,
	DesignationCenter VARCHAR (40) UNIQUE,
	CenterIP NVARCHAR(20) UNIQUE NOT NULL,
	HostName NVARCHAR(20) UNIQUE NOT NULL
)
GO

CREATE TABLE RolDefinition_Center (
	IdRol INT NOT NULL,
	IdCenter INT NOT NULL
	PRIMARY KEY(IdRol, IdCenter),
	CONSTRAINT FK_RolDefinition_RolDefinitionCenter FOREIGN KEY (IdRol) REFERENCES RolDefinition(IdRol),
	CONSTRAINT FK_Center_DefinitionCenter FOREIGN KEY (IdCenter) REFERENCES Center(IdCenter)
)
GO

CREATE TABLE Station (
	IdStation INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	StationName NVARCHAR(30) NOT NULL
)
GO

CREATE TABLE Center_Station (
	IdCenter INT NOT NULL,
	IdStation INT NOT NULL,
	Zocalo INT NOT NULL,
	PRIMARY KEY(IdCenter, IdStation),
	CONSTRAINT FK_RolCenter_RolCenter_Station FOREIGN KEY (IdCenter) REFERENCES Center(IdCenter),
	CONSTRAINT FK_Station_RolCenter_Station FOREIGN KEY (IdStation) REFERENCES Station(IdStation)
)
GO

CREATE TABLE PeriphericDefinition (
	IdPeriphericDefinition INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	PeriphericDescription NVARCHAR(30) NOT NULL
)
GO

CREATE TABLE Peripheric_Center (
	IdPeripheric_Center INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdPeriphericDefinition INT NOT NULL,
	IdCenter INT NOT NULL,
	CONSTRAINT FK_PeriphericDefinition_Peripheric_Center FOREIGN KEY (IdPeriphericDefinition) REFERENCES PeriphericDefinition(IdPeriphericDefinition),
	CONSTRAINT FK_Center_Peripheric_Center FOREIGN KEY (IdCenter) REFERENCES Center(IdCenter)
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
	IdCenter INT,
	CONSTRAINT FK_ItemDescription_ItemDefinition FOREIGN KEY (IdItemDescription) REFERENCES ItemDescription(IdItemDescription),
	CONSTRAINT FK_ItemPartNumber_ItemDefinition FOREIGN KEY (IdItemPN) REFERENCES ItemPartNumber(IdItemPN)
)
GO
CREATE OR ALTER TRIGGER CenterPictureVersion_ItemDefinition ON ItemDefinition
	FOR INSERT, UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM Center 
						WHERE IdCenter IN (SELECT IdCenter FROM inserted))
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
	IF 1 < (SELECT COUNT(*) FROM ItemDefinition AS ID
			INNER JOIN ItemPartNumber AS IPN ON ID.IdItemPN = IPN.IdItemPN
			INNER JOIN ItemSample AS S ON IPN.IdItemSample = S.IdItemSample
			WHERE InternalName IN (SELECT InternalName FROM inserted) AND
				S.IdItemProject IN (SELECT DISTINCT S.IdItemProject FROM inserted AS ID
									INNER JOIN ItemPartNumber AS IPN ON ID.IdItemPN = IPN.IdItemPN
									INNER JOIN ItemSample AS S ON IPN.IdItemSample = S.IdItemSample)
			)
	BEGIN; -- Instruction befor THROW has to end with semicolon
		THROW 50006, 'Version interna ya definida para este proyecto.', 1
		ROLLBACK TRANSACTION
	END
END

SELECT DISTINCT S.IdItemProject FROM ItemDefinition AS ID
INNER JOIN ItemPartNumber AS IPN ON ID.IdItemPN = IPN.IdItemPN
INNER JOIN ItemSample AS S ON IPN.IdItemSample = S.IdItemSample

SELECT COUNT(*) FROM ItemDefinition AS ID
INNER JOIN ItemPartNumber AS IPN ON ID.IdItemPN = IPN.IdItemPN
INNER JOIN ItemSample AS S ON IPN.IdItemSample = S.IdItemSample
INNER JOIN ItemProject AS P ON S.IdItemProject = P.IdItemProject
WHERE InternalName = 'prueba2'
GO
--------------------------------------------------------------------------------------------------------------------------

CREATE TABLE OrderFabrication (
	IdOF INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdItemDefinition INT,
	CodeOF NVARCHAR(20) NOT NULL,
	DescritionOF NVARCHAR(200),
	Quantity INT,
	IdCenter INT,
	DateRegistration SMALLDATETIME
	CONSTRAINT FK_ItemDefinition_OrderFabrication FOREIGN KEY (IdItemDefinition) REFERENCES ItemDefinition(IdItemDefinition)
)
GO

CREATE OR ALTER TRIGGER Center_OrderFabrication ON OrderFabrication
	FOR INSERT, UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM Center 
						WHERE IdCenter IN (SELECT IdCenter FROM inserted))
	BEGIN; -- Instruction befor THROW has to end with semicolon
		THROW 50001, 'IdRolComputer no encontrado en la BBDD.', 1
		ROLLBACK TRANSACTION
	END
END
GO

CREATE TABLE ClientStatus (
	IdClientStatus INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	StatusType NVARCHAR(30) NOT NULL
)
GO

CREATE TABLE ClientRestriction (
	IdClientRestriction INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	RestrictionCode NVARCHAR(30) NOT NULL,
	RestrictionDescription NVARCHAR(255) NOT NULL
)
GO

CREATE TABLE ClientCode (
	IdClientCode INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdItemProject INT NOT NULL,
	IdClientStatus INT NOT NULL,
	IdClientRestriction INT NOT NULL,
	ClientCode NVARCHAR(20) NOT NULL,
	CONSTRAINT FK_ItemProject_ClientCode FOREIGN KEY (IdItemProject) REFERENCES ItemProject(IdItemProject),
	CONSTRAINT FK_ClientStatuss_ClientCode FOREIGN KEY (IdClientStatus) REFERENCES ClientStatus(IdClientStatus),
	CONSTRAINT FK_ClientRestriction_ClientCode FOREIGN KEY (IdClientRestriction) REFERENCES ClientRestriction(IdClientRestriction)
)
GO

CREATE TABLE SNRegister (
	IdSNRegister INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdItemProject INT,
	IdItemCategory INT,
	CodeSN NVARCHAR(30) NOT NULL UNIQUE,
	IdCenter INT,
	DateRegistration SMALLDATETIME,
	CONSTRAINT FK_ItemProject_SNRegister FOREIGN KEY (IdItemProject) REFERENCES ItemProject(IdItemProject),
	CONSTRAINT FK_ItemCategory_SNRegister FOREIGN KEY (IdItemCategory) REFERENCES ItemCategory(IdItemCategory)
)
GO
CREATE OR ALTER TRIGGER Center_SNRegister ON SNRegister
	FOR INSERT, UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM Center 
						WHERE IdCenter  IN (SELECT IdCenter FROM inserted))
	BEGIN; -- Instruction befor THROW has to end with semicolon
		THROW 50001, 'IdRolComputer no encontrado en la BBDD.', 1
		ROLLBACK TRANSACTION
	END
END
GO

CREATE TABLE OrderFabrication_SNRegister (
	IdSNRegister INT NOT NULL PRIMARY KEY,
	IdOF INT NOT NULL,
	CONSTRAINT FK_SNRegister_OrderFabricationSNRegister FOREIGN KEY (IdSNRegister) REFERENCES SNRegister(IdSNRegister),
	CONSTRAINT FK_OrderFabrication_OrderFabricationSNRegister FOREIGN KEY (IdOF) REFERENCES OrderFabrication(IdOF)
)
GO

CREATE TABLE ClientCode_SNRegister (
	IdSNRegister INT NOT NULL PRIMARY KEY,
	IdClientCode INT NOT NULL UNIQUE,
	DateAsignation SMALLDATETIME,
	CONSTRAINT FK_FINASRegister_ClientCodeSNRegister FOREIGN KEY (IdClientCode) REFERENCES ClientCode(IdClientCode),
	CONSTRAINT FK_SNRegister_SNClientCodeSNRegister FOREIGN KEY (IdSNRegister) REFERENCES SNRegister(IdSNRegister)
)
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
	NumPending INT NOT NULL,
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
	IdCenter INT,
	DateRegister SMALLDATETIME NOT NULL,
	CONSTRAINT FK_StatusType_SNStatus FOREIGN KEY (IdStatus) REFERENCES StatusType(IdStatus),
	CONSTRAINT FK_SNRegister_SNStatus FOREIGN KEY (IdSNRegister) REFERENCES SNRegister(IdSNRegister)
)
GO

CREATE OR ALTER TRIGGER Center_SNStatus ON SNStatus
	FOR INSERT, UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM Center 
						WHERE IdCenter IN (SELECT IdCenter FROM inserted))
	BEGIN; -- Instruction befor THROW has to end with semicolon
		THROW 50001, 'IdRolComputer no encontrado en la BBDD.', 1
		ROLLBACK TRANSACTION
	END
END
GO

CREATE TABLE SNStatus_Scrap(
	IdSNStatus_Scrap INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSNStatus INT NOT NULL,
	Reason  NVARCHAR(30),
	CONSTRAINT FK_SNStatus_SNStatus_Scrap FOREIGN KEY (IdSNStatus) REFERENCES SNStatus(IdSNStatus),
)
GO
CREATE OR ALTER TRIGGER Center_SNStatus_Scrap ON SNStatus_Scrap
	FOR INSERT, UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM Center 
						WHERE IdCenter IN (SELECT IdCenter FROM inserted))
	BEGIN; -- Instruction befor THROW has to end with semicolon
		THROW 50001, 'IdRolComputer no encontrado en la BBDD.', 1
		ROLLBACK TRANSACTION
	END
END
GO

CREATE TABLE SNWindchill (
	IdSNWindchill INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSNRegister INT NOT NULL UNIQUE,
	WindchillCode NVARCHAR(30)
	CONSTRAINT FK_SNRegister_SNWindchill FOREIGN KEY (IdSNRegister) REFERENCES SNRegister(IdSNRegister)
)
GO

--------------------------------------------------------------------------------------------------------------------------

CREATE TABLE ItemLayoutPCB (
	IdItemLayoutPCB INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdItemReference INT NOT NULL,
	Layout INT NOT NULL
	CONSTRAINT FK_ItemReference_ItemLayoutPCB FOREIGN KEY (IdItemReference) REFERENCES ItemReferencePCB(IdItemReferencePCB)
)
GO

CREATE TABLE ItemBomPCB (
	IdItemBomPCB INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdItemLayoutPCB INT NOT NULL,
	Bom INT NOT NULL
	CONSTRAINT FK_ItemLayoutPCB_ItemBomPCB FOREIGN KEY (IdItemLayoutPCB) REFERENCES ItemLayoutPCB(IdItemLayoutPCB)
)
GO

CREATE TABLE ReworkDefinitionPCB (
	IdReworkDefinitionPCB INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ReworkNum INT NOT NULL,
	ReworkDescription NVARCHAR(50)NOT NULL
)
GO

CREATE TABLE ItemBomPCB_ReworkDefinitionPCB (
	IdItemBomPCB INT NOT NULL,
	IdReworkDefinitionPCB INT NOT NULL,
	PRIMARY KEY(IdItemBomPCB, IdReworkDefinitionPCB),
	CONSTRAINT FK_ItemBomPCB_ReworkDefinitionPCB FOREIGN KEY (IdItemBomPCB) REFERENCES ItemBomPCB(IdItemBomPCB),
	CONSTRAINT FK_ReworkDefinitionPCB_ReworkDefinitionPCB FOREIGN KEY (IdReworkDefinitionPCB) REFERENCES ReworkDefinitionPCB(IdReworkDefinitionPCB)
)
GO

CREATE TABLE ItemLayoutBomRework_PCB (
	IdItemLayoutBomRework_PCB INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdItemBomPCB INT NOT NULL,
	ItemRework INT NOT NULL,
	Windchill NVARCHAR(30),
	CONSTRAINT FK_ItemBomPCB_ItemLayoutBomRework_PCB FOREIGN KEY (IdItemBomPCB) REFERENCES ItemBomPCB(IdItemBomPCB)
)
GO

CREATE TABLE SNRegisterPCB (
	IdSNRegisterPCB INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSNRegister INT NOT NULL UNIQUE,
	IdItemBomPCB INT NOT NULL,
	Batch NVARCHAR(20),
	CONSTRAINT FK_SNRegister_SNRegisterPCB FOREIGN KEY (IdSNRegister) REFERENCES SNRegister(IdSNRegister),
	CONSTRAINT FK_ItemBomPCB_SNRegisterPCB FOREIGN KEY (IdItemBomPCB) REFERENCES ItemBomPCB(IdItemBomPCB)
)
GO

CREATE TABLE ManufacturingReworkPCB (
	IdManufacturingReworkPCB INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSNRegisterPCB INT NOT NULL,
	Rework INT NOT NULL,
	IdCenter INT NOT NULL,
	RegistrationDate SMALLDATETIME NOT NULL
	CONSTRAINT FK_SNRegisterPCB_ManufacturingReworkPCB FOREIGN KEY (IdSNRegisterPCB) REFERENCES SNRegisterPCB(IdSNRegisterPCB)
)
GO

CREATE OR ALTER TRIGGER Center_ManufacturingReworkPCB ON ManufacturingReworkPCB
	FOR INSERT, UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM Center 
						WHERE IdCenter  IN (SELECT IdCenter FROM inserted))
	BEGIN; -- Instruction befor THROW has to end with semicolon
		THROW 50001, 'IdCenter no encontrado en la BBDD.', 1
		ROLLBACK TRANSACTION
	END
END
GO

--------------------------------------------------------------------------------------------------------------------------

CREATE TABLE ItemDefinition_PCBLayoutBomRework (
	IdItemDefinition INT NOT NULL,
	IdItemLayoutBomRework_PCB INT NOT NULL,
	PRIMARY KEY(IdItemDefinition, IdItemLayoutBomRework_PCB),
	CONSTRAINT FK_ItemDefinition_Item_Definition_PCB FOREIGN KEY (IdItemDefinition) REFERENCES ItemDefinition(IdItemDefinition),
	CONSTRAINT FK_ItemLayoutBomRework_PCB_Item_Definition_PCB FOREIGN KEY (IdItemLayoutBomRework_PCB) REFERENCES ItemLayoutBomRework_PCB(IdItemLayoutBomRework_PCB)
)
GO

CREATE TABLE MechanicalParameter (
	IdMechanicalParameter INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	MechanicalDescription NVARCHAR(50) NOT NULL
)
GO

CREATE TABLE VersionMechanical (
	IdVersionMechanical INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdMechanicalParameter INT NOT NULL ,
	MechanicalData NVARCHAR(20) NOT NULL,
	CONSTRAINT FK_MechanicalParameter_VersionMechanical FOREIGN KEY (IdMechanicalParameter) REFERENCES MechanicalParameter(IdMechanicalParameter)
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

CREATE TABLE SoftwareParameter (
	IdSoftwareParameter INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	SoftwareDescription NVARCHAR(50) NOT NULL
)
GO

CREATE TABLE VersionSoftware (
	IdVersionSoftware INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSoftwareParameter INT NOT NULL ,
	SoftwareData NVARCHAR(20) NOT NULL,
	CONSTRAINT FK_SoftwareParameter_VersionSoftware FOREIGN KEY (IdSoftwareParameter) REFERENCES SoftwareParameter(IdSoftwareParameter)
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

CREATE TABLE LabellingParameter (
	IdLabellingParameter INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	LabellingDescription NVARCHAR(50) NOT NULL
)
GO

CREATE TABLE VersionLabelling (
	IdVersionLabelling INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdLabellingParameter INT NOT NULL ,
	LabellingData NVARCHAR(20) NOT NULL,
	CONSTRAINT FK_IdLabellingParameter_VersionLabelling FOREIGN KEY (IdLabellingParameter) REFERENCES LabellingParameter(IdLabellingParameter)
)
GO

CREATE TABLE ItemDefinition_VersionLabelling (
	IdItemDefinition INT NOT NULL,
	IdVersionLabelling INT NOT NULL,
	PRIMARY KEY(IdItemDefinition, IdVersionLabelling),
	CONSTRAINT FK_IdItemDefinition_ItemDefinitionVersionLabelling FOREIGN KEY (IdItemDefinition) REFERENCES ItemDefinition(IdItemDefinition),
	CONSTRAINT FK_IdVersionLabelling_ItemDefinitionVersionLabelling FOREIGN KEY (IdVersionLabelling) REFERENCES VersionLabelling(IdVersionLabelling)
)
GO

CREATE TABLE VersionProcess (
	IdVersionProcess INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ProcessDesignation NVARCHAR(40) NOT NULL
)
GO

CREATE TABLE ItemDefinition_VersionProcess (
	IdItemDefinition INT NOT NULL PRIMARY KEY,
	IdVersionProcess INT NOT NULL,
	CONSTRAINT FK_IdItemDefinition_ItemDefinitionVersionProcess FOREIGN KEY (IdItemDefinition) REFERENCES ItemDefinition(IdItemDefinition),
	CONSTRAINT FK_IdVersionProcess_ItemDefinitionVersionProcess FOREIGN KEY (IdVersionProcess) REFERENCES VersionProcess(IdVersionProcess)
)
GO

CREATE TABLE ProcessTest (
	IdProcessTest INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	TestDescription NVARCHAR(30) NOT NULL,
	IdStation INT NOT NULL,
	CONSTRAINT FK_IdStation_ProcessTest FOREIGN KEY (IdStation) REFERENCES Station(IdStation)
)
GO

CREATE TABLE ItemDefinition_ProcessTest (
	IdItemDefinition INT NOT NULL,
	IdProcessTest INT NOT NULL,
	VersionTest INT NOT NULL,
	PRIMARY KEY(IdItemDefinition, IdProcessTest),
	CONSTRAINT FK_IdItemDefinition_ItemDefinition_ProcessTest FOREIGN KEY (IdItemDefinition) REFERENCES ItemDefinition(IdItemDefinition),
	CONSTRAINT FK_IdProcessTest_ItemDefinition_ProcessTest FOREIGN KEY (IdProcessTest) REFERENCES ProcessTest(IdProcessTest)
)
GO

--------------------------------------------------------------------------------------------------------------------------

CREATE TABLE TestStep (
	IdTestStep INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdProcessTest INT NOT NULL,
	TestName NVARCHAR(30),
	TestVersion INT,
	ExecutionOrder INT,
	RegistrationDate SMALLDATETIME,
	CONSTRAINT FK_IdProcessTest_TestStep FOREIGN KEY (IdProcessTest) REFERENCES ProcessTest(IdProcessTest)
)
GO

CREATE TABLE TestStepDefinition (
	IdTestStepDefinition INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdTestStep INT NOT NULL,
	IdPeriphericDefinition INT NOT NULL,
	Command NVARCHAR(100),
	LimL NVARCHAR(15),
	LimH NVARCHAR(15),
	Units  NVARCHAR(10),
	Gain  NVARCHAR(10),
	Offset  NVARCHAR(10),
	Notes NVARCHAR(100),
	Report BIT,
	CONSTRAINT FK_TestSteep_TestStepDefinition FOREIGN KEY (IdTestStep) REFERENCES TestStep(IdTestStep),
	CONSTRAINT FK_PeriphericDefinition_TestStepDefinition FOREIGN KEY (IdPeriphericDefinition) REFERENCES PeriphericDefinition(IdPeriphericDefinition)
)
GO

CREATE TABLE StatisticsTestResult (
	IdStatisticsTestResult INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ValueTest varchar (40) NOT NULL
)
GO

CREATE TABLE ManufacturingRegister_StatisticsTest (
	IdManufacturingRegister_EstadisticaTest INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSNRegister INT NOT NULL,
	IdCenter INT NOT NULL,
	IdProcessTest INT NOT NULL,
	IdStatisticsTestResult INT NOT NULL,
	DateRegistration SMALLDATETIME
	CONSTRAINT FK_SNRegister_ManufacturingRegister_EstadisticaTest FOREIGN KEY (IdSNRegister) REFERENCES SNRegister(IdSNRegister),
	CONSTRAINT FK_StatisticsTestResult_ManufacturingRegister_EstadisticaTest FOREIGN KEY (IdStatisticsTestResult) REFERENCES StatisticsTestResult(IdStatisticsTestResult),
)
GO

CREATE OR ALTER TRIGGER Center_ManufacturingRegister_TestStep ON ManufacturingRegister_StatisticsTest
	FOR INSERT, UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM Center 
						WHERE IdCenter IN (SELECT IdCenter FROM inserted))
	BEGIN; -- Instruction befor THROW has to end with semicolon
		THROW 50001, 'Centro no encontrado en la BBDD.', 1
		ROLLBACK TRANSACTION
	END
END
GO
CREATE OR ALTER TRIGGER ProcessTest_ManufacturingRegister_TestStep ON ManufacturingRegister_StatisticsTest
	FOR INSERT, UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM ProcessTest 
						WHERE IdProcessTest IN (SELECT IdProcessTest FROM inserted))
	BEGIN; -- Instruction befor THROW has to end with semicolon
		THROW 51031, 'ProcessTest no encontrado en la BBDD.', 1
		ROLLBACK TRANSACTION
	END
END
GO


--------------------------------------------------------------------------------------------------------------------------

CREATE TABLE ProcessDescription (
	IdProcessDescription INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ProcessDescription NVARCHAR(40) NOT NULL
)
GO

CREATE TABLE ProcessDefinition (
	IdProcessDefinition INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdVersionProcess INT NOT NULL,
	Num INT NOT NULL,
	ProcessNum INT NOT NULL,
	IdProcessDescription INT NOT NULL,
	Operation INT NOT NULL,
	OperationDescription NVARCHAR(60),
	IdPicture INT,
	DateRegistration SMALLDATETIME,
	CONSTRAINT FK_VersionProcess_ProcessDefinition FOREIGN KEY (IdVersionProcess) REFERENCES VersionProcess(IdVersionProcess),
	CONSTRAINT FK_ProcessDescription_ProcessDefinition FOREIGN KEY (IdProcessDescription) REFERENCES ProcessDescription(IdProcessDescription)
)
GO

CREATE OR ALTER TRIGGER Picture_ProcessDefinition ON ProcessDefinition
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
	IdProcessDefinition INT NOT NULL PRIMARY KEY,
	IdProcessTest INT NOT NULL,
	CONSTRAINT FK_ProcessDefinition_Process_Test_Definition FOREIGN KEY (IdProcessDefinition) REFERENCES ProcessDefinition(IdProcessDefinition),
	CONSTRAINT FK_ProcessTest_Process_Test_Definition FOREIGN KEY (IdProcessTest) REFERENCES ProcessTest(IdProcessTest)
)
GO

CREATE TABLE ProcessInput (
	IdProcessInput INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	InputType NVARCHAR(20) NOT NULL -- Imagen, int, float, string
)
GO

CREATE TABLE Process_Input_Definition(
	IdProcessDefinition INT NOT NULL,
	IdProcessInput INT NOT NULL,
	PRIMARY KEY(IdProcessDefinition, IdProcessInput),
	CONSTRAINT FK_DefinitionProcess_Process_Input_Definition FOREIGN KEY (IdProcessDefinition) REFERENCES ProcessDefinition(IdProcessDefinition),
	CONSTRAINT FK_ProcessInput_Process_Input_Definition FOREIGN KEY (IdProcessInput) REFERENCES ProcessInput(IdProcessInput)
)
GO

CREATE TABLE ProcessItem(
	IdProcessItem INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Reference NVARCHAR(20) NOT NULL 
)
GO

CREATE TABLE Process_Item_Definition (
	IdProcessDefinition INT NOT NULL,
	IdProcessItem INT NOT NULL,
	PRIMARY KEY(IdProcessDefinition, IdProcessItem),
	CONSTRAINT FK_ProcessDefinition_Definition_Process_Item FOREIGN KEY (IdProcessDefinition) REFERENCES ProcessDefinition(IdProcessDefinition),
	CONSTRAINT FK_ProcessItem_Definition_Process_Item FOREIGN KEY (IdProcessItem) REFERENCES ProcessItem(IdProcessItem)
)
GO

CREATE TABLE ProcessScrewDriver (
	IdProcessScrewDriver INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ScrewCode NVARCHAR(100) NOT NULL
)
GO


CREATE TABLE Process_ScrewDriver_Definition (
	IdProcessDefinition INT NOT NULL,
	IdProcessScrewDriver INT NOT NULL,
	PRIMARY KEY(IdProcessDefinition, IdProcessScrewDriver),
	CONSTRAINT FK_ProcessDefinition_Process_ScrewDriver_Definition FOREIGN KEY (IdProcessDefinition) REFERENCES ProcessDefinition(IdProcessDefinition),
	CONSTRAINT FK_ProcessScrewDriver_Process_ScrewDriver_Definition FOREIGN KEY (IdProcessScrewDriver) REFERENCES ProcessScrewDriver(IdProcessScrewDriver)
)
GO



CREATE TABLE Process_Station_Definition (
	IdProcessDefinition INT NOT NULL,
	IdStation INT NOT NULL,
	PRIMARY KEY(IdProcessDefinition, IdStation),
	CONSTRAINT FK_ProcessDefinition_Process_Station_Definition FOREIGN KEY (IdProcessDefinition) REFERENCES ProcessDefinition(IdProcessDefinition),
	CONSTRAINT FK_Station_Process_Station_Definition FOREIGN KEY (IdStation) REFERENCES Station(IdStation)
)
GO

--------------------------------------------------------------------------------------------------------------------------

CREATE TABLE ManufacturingResult (
	IdManufacturingResult INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	TypeManufacturingResult NVARCHAR(20) NOT NULL
)
GO

CREATE TABLE ManufacturingRolDefinition (
	IdManufacturingRolDefinition INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ManufacturingRolDescription NVARCHAR(20) NOT NULL
)
GO

CREATE TABLE ManufacturingUser (
	IdManufacturingUser INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	UserName NVARCHAR(20) NOT NULL,
	UserCode NVARCHAR(20) NOT NULL,
	EnableUser BIT NOT NULL
)
GO

CREATE TABLE ManufacturingUser_RolDefinition (
	IdManufacturingUser INT NOT NULL,
	IdManufacturingRolDefinition INT NOT NULL,
	PRIMARY KEY(IdManufacturingUser, IdManufacturingRolDefinition),
	CONSTRAINT FK_ManufacturingUser_ManufacturingRolDefinition FOREIGN KEY (IdManufacturingUser) REFERENCES ManufacturingUser(IdManufacturingUser),
	CONSTRAINT FK_ManufacturingRolDefinition_ManufacturingRolDefinition FOREIGN KEY (IdManufacturingRolDefinition) REFERENCES ManufacturingRolDefinition(IdManufacturingRolDefinition)
)
GO

CREATE TABLE ManufacturingRegister (
	IdManufacturingRegister INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdSNRegister INT NOT NULL,
	IdManufacturingResult INT NOT NULL,
	IdManufacturingUser INT NOT NULL,
	Num INT NOT NULL,
	DateStart SMALLDATETIME NOT NULL,
	DateEnd SMALLDATETIME,
	IdCenter INT NOT NULL,
	IdOF INT,  --OF actual (puede cambiar con el tiempo)
	CONSTRAINT FK_SNRegister_ManufacturingRegister FOREIGN KEY (IdSNRegister) REFERENCES SNRegister(IdSNRegister),
	CONSTRAINT FK_ManufacturingResult_ManufacturingRegister FOREIGN KEY (IdManufacturingResult) REFERENCES ManufacturingResult(IdManufacturingResult),
	CONSTRAINT FK_ManufacturingUser_ManufacturingRegister FOREIGN KEY (IdManufacturingUser) REFERENCES ManufacturingUser(IdManufacturingUser)
)
GO

CREATE OR ALTER TRIGGER Center_ManufacturingRegister ON ManufacturingRegister
	FOR INSERT, UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM Center 
						WHERE IdCenter IN (SELECT IdCenter FROM inserted))
	BEGIN; -- Instruction befor THROW has to end with semicolon
		THROW 50001, 'Centro no encontrado en la BBDD.', 1
		ROLLBACK TRANSACTION
	END
END
GO

CREATE TABLE ManufacturingParameterScrew (
	IdManufacturingParameterScrew INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ScrewDescription NVARCHAR(40)
)
GO

CREATE TABLE ManufacturingScrew (
	IdManufacturingScrew INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdManufacturingParameterScrew INT NOT NULL,
	ScrewData NVARCHAR(40) NOT NULL,
	CONSTRAINT FK_ManufacturingParameterScrew_ManufacturingScrew FOREIGN KEY (IdManufacturingParameterScrew) REFERENCES ManufacturingParameterScrew(IdManufacturingParameterScrew),
)
GO

CREATE TABLE ManufacturingRegister_Screw (
	IdManufacturingRegister INT NOT NULL,
	IdManufacturingScrew INT NOT NULL,
	PRIMARY KEY (IdManufacturingRegister, IdManufacturingScrew),
	CONSTRAINT FK_ManufacturingRegister_ManufacturingRegisterScrew FOREIGN KEY (IdManufacturingRegister) REFERENCES ManufacturingRegister(IdManufacturingRegister),
	CONSTRAINT FK_ManufacturingScrew_ManufacturingRegisterScrew FOREIGN KEY (IdManufacturingScrew) REFERENCES ManufacturingScrew(IdManufacturingScrew)
)
GO

CREATE TABLE ManufacturingParameterInput (
	IdManufacturingParameterInput INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	InputDescription NVARCHAR(40)
)
GO

CREATE TABLE ManufacturingInput (
	IdManufacturingInput INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdManufacturingParameterInput INT NOT NULL,
	InputData NVARCHAR(40) NOT NULL,
	CONSTRAINT FK_ManufacturingParameterInput_ManufacturingInput FOREIGN KEY (IdManufacturingParameterInput) REFERENCES ManufacturingParameterInput(IdManufacturingParameterInput),
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
	IdSNRegister INT NOT NULL
)
GO

CREATE OR ALTER TRIGGER SNRegister_ManufacturingSN ON ManufacturingSN
	FOR INSERT, UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM SNRegister 
						WHERE IdSNRegister IN (SELECT IdSNRegister FROM inserted))
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
