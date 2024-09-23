USE XEDI_TraceabilityDB
GO

CREATE OR ALTER FUNCTION GetHardwareParameter(
										@numberPart INT,	
										@hardwarePCB NVARCHAR(30)
) RETURNS INT AS
BEGIN
	DECLARE @hardwareParameter INT

	SELECT  @hardwareParameter = C.valueData FROM (
		SELECT row_number() OVER(ORDER BY (SELECT NULL)) AS id, P.valueData
		FROM (SELECT value AS valueData FROM STRING_SPLIT((
							SELECT TOP 1 value FROM STRING_SPLIT(@hardwarePCB, '-')
							WHERE value NOT IN (SELECT TOP 2 value FROM STRING_SPLIT(@hardwarePCB, '-'))
						), '.')) AS P) AS C
	WHERE C.id = @numberPart

	RETURN @hardwareParameter
END
GO

SELECT dbo.GetHardwareParameter (1, 'LG-1826-4.7.7')
GO

CREATE OR ALTER PROCEDURE InsertVersionHardware(
							@idItemDefinition INT,
							@hardwarePCB NVARCHAR(30)
)
AS
-- Precondiciones: El hardware deberá seguir el formato LG-REFF-X.Y.Z (Ej: LB-1234-1.2.3).
-- Postcondiciones: Los errores generados por el procedimiento corresponden a las diferentes casuisticas que se pueden dar y deben ser tratados en el exterior individualmente.
	
	DECLARE @Reference INT
	DECLARE @Layout INT
	DECLARE @Bom INT
	DECLARE @Rework INT
	DECLARE @idReference INT
	DECLARE @idLayout INT
	DECLARE @idBom INT
	DECLARE @idRework INT

	--Asignación de referencia
	SELECT TOP 1 @Reference = value FROM STRING_SPLIT(@hardwarePCB, '-')
	WHERE value NOT IN (SELECT TOP 1 value FROM STRING_SPLIT(@hardwarePCB, '-'))
	--Asignación de layout
	SELECT @Layout = dbo.GetHardwareParameter (1, @hardwarePCB)
	--Asignación de bom
	SELECT @Bom = dbo.GetHardwareParameter (2, @hardwarePCB)
	--Asignación de rework
	SELECT @Rework = dbo.GetHardwareParameter (3, @hardwarePCB)
		
	SELECT @IdReference = IdItemReferencePCB FROM ItemReferencePCB
	WHERE Reference = @Reference
	IF @idReference IS NULL
	BEGIN;
		THROW 51001, 'Referencia no encontrada.', 1
	END

	SELECT @idLayout = IdItemLayoutPCB FROM ItemLayoutPCB
	WHERE Layout = @Layout AND IdItemReference = @idReference
	IF @idLayout IS NULL
	BEGIN
		INSERT INTO ItemLayoutPCB(IdItemReference, Layout) VALUES(@idReference, @Layout)
		SET @idLayout = @@IDENTITY
	END
		
	SELECT @idBom = IdItemBomPCB FROM ItemBomPCB
	WHERE Bom = @Bom AND IdItemLayoutPCB = @idLayout
	IF @idBom IS NULL
	BEGIN
		INSERT INTO ItemBomPCB(IdItemLayoutPCB, Bom) VALUES(@idLayout, @Bom)
		SET @idBom = @@IDENTITY
	END

	SELECT @idRework = IdItemLayoutBomRework_PCB FROM ItemLayoutBomRework_PCB
	WHERE ItemRework = @Rework AND IdItemBomPCB= @idBom
	IF @idRework IS NULL
	BEGIN
		INSERT INTO ItemLayoutBomRework_PCB(IdItemBomPCB, ItemRework) VALUES(@idBom, @Rework)
		SET @idRework = @@IDENTITY
	END

	IF NOT EXISTS (SELECT * FROM ItemDefinition_PCBLayoutBomRework WHERE IdItemDefinition = @idItemDefinition AND IdItemLayoutBomRework_PCB = @idRework)
	BEGIN
		INSERT INTO ItemDefinition_PCBLayoutBomRework VALUES (@idItemDefinition, @idRework)
	END

GO

EXECUTE dbo.InsertVersionHardware 12, 'LG-1826-8.2.0'
GO
DELETE FROM ItemDefinition_PCBLayoutBomRework WHERE IdItemDefinition = 18
GO

SELECT * FROM ItemReferencePCB
SELECT * FROM ItemLayoutPCB
SELECT * FROM ItemBomPCB
SELECT * FROM ItemLayoutBomRework_PCB
SELECT * FROM ItemDefinition_PCBLayoutBomRework

GO
SELECT  IdItemReferencePCB FROM ItemReferencePCB
	WHERE Reference = (
						SELECT TOP 1 value FROM STRING_SPLIT('LG-1854-4.1.1', '-')
						WHERE value NOT IN (SELECT TOP 1 value FROM STRING_SPLIT('LG-1854-4.1.1', '-'))
						)

GO

SELECT row_number() OVER(ORDER BY (SELECT NULL)) AS id, P.valueData
FROM (SELECT value AS valueData FROM STRING_SPLIT((
					SELECT TOP 1 value FROM STRING_SPLIT('LG-1854-4.1.1', '-')
					WHERE value NOT IN (SELECT TOP 2 value FROM STRING_SPLIT('LG-1854-4.1.1', '-'))
				), '.')) AS P
GO

SELECT * FROM HardwarePCB_Parameters_View
SELECT * FROM ReworkFromHWRef_Project_View
GO

CREATE OR ALTER VIEW UnifiedHardwarePCB AS 
	SELECT IDPCB.IdItemDefinition, HPP.ReferenceName, HPP.HardwareReference FROM ItemDefinition_PCBLayoutBomRework AS IDPCB
	INNER JOIN HardwarePCB_Parameters_View AS HPP ON IDPCB.IdItemLayoutBomRework_PCB = HPP.IdItemLayoutBomRework_PCB
GO

SELECT * FROM UnifiedHardwarePCB
WHERE IdItemDefinition = 28
GO

CREATE OR ALTER PROCEDURE DeleteHardwareVersion(
								@idItemDefinition INT
)
AS
	
	DELETE FROM ItemDefinition_PCBLayoutBomRework WHERE IdItemDefinition = @idItemDefinition
	
	-- Borrar hardware no vinculado a una itemDefinition
GO