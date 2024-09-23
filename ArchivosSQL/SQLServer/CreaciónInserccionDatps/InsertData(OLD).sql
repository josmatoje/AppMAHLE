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
GO

INSERT INTO StatusType (StatusValue)
				VALUES('Estado 0'),
						('Estado 1')
GO
SELECT * FROM StatusType
GO

INSERT INTO ItemDescription(DescriptionItem)
				VALUES ('Versión interna')
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

DELETE FROM Process_Test_Definition WHERE IdProcessDefinition > 12
DELETE FROM ProcessTest WHERE IdProcessTest = 22
DELETE FROM Process_ScrewDriver_Definition WHERE IdProcessDefinition > 12
DELETE FROM ProcessScrewDriver WHERE IdProcessScrewDriver > 0
DELETE FROM Process_Input_Definition WHERE IdProcessDefinition > 12
DELETE FROM ProcessInput WHERE IdProcessInput > 0
DELETE FROM Process_Item_Definition WHERE IdProcessDefinition > 12
DELETE FROM ProcessItem WHERE IdProcessItem > 0
DELETE FROM ProcessDefinition WHERE IdVersionProcess > 1
DELETE FROM ProcessDescription WHERE IdProcessDescription > 1
DELETE FROM VersionProcess WHERE IdVersionProcess > 1

COMMIT
GO

DECLARE @idIV INT
SET @idIV = 27

DELETE FROM ItemDefinition_PCBLayoutBomRework WHERE IdItemDefinition = @idIV
DELETE FROM ItemDefinition_VersionMechanical WHERE IdItemDefinition = @idIV
DELETE FROM ItemDefinition_VersionSoftware WHERE IdItemDefinition = @idIV
DELETE FROM ItemDefinition_VersionProcess WHERE IdItemDefinition = @idIV
DELETE FROM ItemDefinition_PCBLayoutBomRework WHERE IdItemDefinition = @idIV

DELETE FROM ItemDefinition WHERE IdItemDefinition = @idIV