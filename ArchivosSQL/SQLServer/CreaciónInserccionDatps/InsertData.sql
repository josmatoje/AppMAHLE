USE XEDI_TraceabilityDB
GO

DELETE FROM RolDefinition
DELETE FROM Center
DELETE FROM RolDefinition_Center
DELETE FROM ItemCategory
DELETE FROM ItemProject
DELETE FROM ItemReferencePCB
DELETE FROM StatusType
DELETE FROM ItemDescription
DELETE FROM ItemDefinition
DELETE FROM ItemPartNumber
DELETE FROM ItemSample
DELETE FROM PictureStorage
DELETE FROM Station

BEGIN TRANSACTION

ROLLBACK

COMMIT

INSERT INTO RolDefinition(RolDescription) 
					VALUES ('Input_SN_PCB'),
							('Input_Rework'),
							('Input_SN_Housing'),
							('Input_VI'),
							('Input_OF'),
							('Input_AG')
GO
SELECT * FROM RolDefinition
SELECT * FROM Center_RolDefinitions_View 

INSERT INTO Center (ComputerName, UserCode,CenterIP, HostName)
     VALUES('Jose Maria Mata', 'E0164372','10.144.216.196', 'ESVLN40313'),
		('Beatriz Perez', 'M0137784', '192.168.0.24', 'ESVLN40192')
GO
SELECT * FROM Center
order by idcenter

INSERT INTO RolDefinition_Center
     VALUES(1,1),
			(2,1),
			(3,1),
			(4,1),
			(5,1),
			(6,1),
			(1,2),
			(2,2),
			(3,2),
			(4,2),
			(5,2),
			(6,2)
GO
SELECT * FROM RolDefinition_Center
order by idcenter

INSERT INTO ItemCategory(CategoryDescription)
				VALUES ('PCB'),
						('Housing'),
						('Subassembly')
GO
SELECT * FROM ItemCategory


INSERT INTO ItemProject(ProjectName)
				VALUES ('OBCAMG11')
GO
SELECT * FROM ItemProject
GO
SELECT * FROM PictureStorage

INSERT INTO ItemReferencePCB(IdItemProject, ItemDescription, Reference) 
					VALUES (1, 'In-Filter', '1835'),
							(1, 'MainBoard', '1826'),
							(1, 'Control', '1854'),
							(1, 'IMS', '1845'),
							(1, 'Aux-IN', '1860'),
							(1, 'Aux-MA', '1855'),
							(1, 'Rogowski', '1950'),
							--(2, 1, 'Housing', '21394'),
							--(2, 1, 'Housing', '45674'),
							--(2, 1, 'Housing', '67889'),
							--(2, 1, 'Housing', '65689'),
							(1, 'Pieza3', '3245889')
GO
SELECT * FROM ItemReferencePCB

INSERT INTO StatusType (StatusValue)
				VALUES('Estado 0'),
						('Estado 1')
GO
SELECT * FROM StatusType
GO

INSERT INTO ItemDescription(DescriptionItem)
				VALUES ('Versión interna'),
				('SubAssembly')
GO
SELECT * FROM ItemDescription -- WHERE DescriptionItem = 'Version interna'
GO


SELECT * FROM SNRegister


INSERT INTO MechanicalParameter(MechanicalDescription)
			VALUES -- ('Windchill Housing'),
					--('Windchill Cooler'),
					--('Windchill 1'),
					--('Windchill 2'),
					('LB')
SELECT * FROM MechanicalParameter
GO

INSERT INTO Station (StationName)
			VALUES ('Protos_Station1'),
					('Protos_Station2'),
					('Protos_Station3'),
					('Protos_Station4'),
					('Protos_Station5'),
					('Passthrough'),
					('HiPot'),
					('EOL1'),
					('Protos_StationGP12')
SELECT * FROM Station

GO

INSERT INTO ProcessTest(TestDescription, IdStation)
			VALUES ('FCT MainBoard',1),
					('FCT Control',2),
					('FCT In-Filter',3),
					('Passthrough',6),
					('HiPot',7),
					('EOL1',8)
SELECT * FROM ProcessTest

GO

----------------------------------------------------------

DELETE FROM MechanicalParameter
WHERE IdMechanicalParameter = 1
GO

SELECT PictureName FROM PictureStorage
-- SELECT * FROM PictureStorage
SELECT * FROM VersionProcess
SELECT * FROM ProcessDefinition
SELECT * FROM ProcessDescription
SELECT * FROM Process_Test_Definition
SELECT * FROM ProcessTest
SELECT * FROM Process_ScrewDriver_Definition
SELECT * FROM ProcessScrewDriver
SELECT * FROM Process_Input_Definition
SELECT * FROM ProcessInput
SELECT * FROM Process_Item_Definition
SELECT * FROM ProcessItem

BEGIN TRANSACTION

DECLARE @idProcess INT
SET @idProcess = 0

DELETE FROM Process_Station_Definition WHERE IdProcessDefinition > @idProcess
DELETE FROM Process_Test_Definition WHERE IdProcessDefinition > @idProcess
-- DELETE FROM ProcessTest WHERE IdProcessTest > 22
DELETE FROM Process_ScrewDriver_Definition WHERE IdProcessDefinition > @idProcess
DELETE FROM ProcessScrewDriver WHERE IdProcessScrewDriver > 0
DELETE FROM Process_Input_Definition WHERE IdProcessDefinition > @idProcess
DELETE FROM ProcessInput WHERE IdProcessInput > 0
DELETE FROM Process_Item_Definition WHERE IdProcessDefinition > @idProcess
DELETE FROM ProcessItem WHERE IdProcessItem > 0
DELETE FROM ProcessDefinition WHERE IdProcessDefinition > @idProcess
DELETE FROM ProcessDescription WHERE IdProcessDescription > 1
 DELETE FROM VersionProcess WHERE IdVersionProcess = 1
 UPDATE VersionProcess SET ProcessDesignation = 'Proceso vacio' WHERE IdVersionProcess = 1

SELECT * FROM VersionProcess
SELECT * FROM ProcessDefinition
SELECT * FROM Process_Station_Definition
SELECT * FROM Process_Test_Definition
SELECT * FROM ProcessTest
SELECT * FROM Process_Input_Definition
SELECT * FROM ProcessInput
SELECT * FROM Process_Item_Definition
SELECT * FROM ProcessItem
SELECT * FROM Process_ScrewDriver_Definition
SELECT * FROM ProcessScrewDriver

COMMIT
ROLLBACK
GO

DECLARE @idIV INT
SET @idIV = 27

DELETE FROM ItemDefinition_PCBLayoutBomRework WHERE IdItemDefinition = @idIV
DELETE FROM ItemDefinition_VersionMechanical WHERE IdItemDefinition = @idIV
DELETE FROM ItemDefinition_VersionSoftware WHERE IdItemDefinition = @idIV
DELETE FROM ItemDefinition_VersionProcess WHERE IdItemDefinition = @idIV
DELETE FROM ItemDefinition_PCBLayoutBomRework WHERE IdItemDefinition = @idIV

DELETE FROM ItemDefinition WHERE IdItemDefinition = @idIV

------------------------------------

DELETE FROM ManufacturingResult;
DELETE FROM ManufacturingRegister;
DELETE FROM ManufacturingRegister_Coment;
DELETE FROM StatusType;
INSERT INTO ManufacturingResult (TypeManufacturingResult) 
VALUES ('OK'),
('NOK'),
('REWORK'),
('CHANGE_OF')

SELECT * FROM ManufacturingResult
SELECT * FROM StatusType
GO

INSERT INTO SNStatus ( IdStatus, IdSNRegister,IdCenter, DateRegister)
VALUES (4,7, 1, CURRENT_TIMESTAMP)
INSERT ManufacturingRegister (IdSN, IdManufacturingResult, Num, DateStart, DateEnd, IdCenter, IdOF)
VALUES ('7',1,1,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,1,7),
 ('7',1,2,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,1,7),
 ('7',1,3,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,1,7),
  ('7',3,4,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,1,7)

  SELECT * FROM SNStatus WHERE IdSNRegister=7
SELECT * FROM ManufacturingRegister



---------------------------------------ALTA_PCB

SELECT * FROM StatusType;
INSERT INTO StatusType (StatusValue)
VALUES ('ALMACEN'),
('OK'),
('NOK')
--------------------------
INSERT INTO ItemReferencePCB (IdItemProject, ItemDescription, Reference)
VALUES(1,'LG_1835_InFilter','1835'),
(1,'LG_1826_MainBoard','1826'),
(1,'LG_1854_Control','1854'),
(1,'LG_1845_IMS','1845'),
(1,'LG_1860_AuxIN','1860'),
(1,'LG_1855_AuxMA','1855'),
(1,'LG_1950_Rogowski','1950')
