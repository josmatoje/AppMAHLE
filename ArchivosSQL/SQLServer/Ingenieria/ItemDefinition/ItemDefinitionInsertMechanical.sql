USE XEDI_TraceabilityDB
GO

CREATE OR ALTER PROCEDURE InsertMechanicalVersionPrameter (
								@idItemDefinition INT,
								@parameter NVARCHAR(20), 
								@data NVARCHAR(30))
AS
-- Precondiciones: El idInternalVersionDef recibido deberá existir en la bbdd.
-- Postcondiciones: Los errores generados por el procedimiento corresponden a las diferentes casuisticas que se pueden dar y deben ser tratados en el exterior individualmente.
	DECLARE @idVersionMechanical INT
	DECLARE @idMechanicalParameter INT
	DECLARE @errorMessage NVARCHAR(200)
    
	SELECT @idMechanicalParameter = IdMechanicalParameter FROM MechanicalParameter
		WHERE MechanicalDescription LIKE @parameter
	IF @idMechanicalParameter IS NULL 
	BEGIN
		--INSERT INTO MechanicalParameter(MechanicalDescription) VALUES (@parameter)
		--SET @idMechanicalParameter = @@IDENTITY
		SELECT @errorMessage = CONCAT_WS(' ', 'El parámetro', @parameter, 'de mecánica no ha sido encontrado. Consulte con soporte si fuera necesario añadir un parámetro.');
		THROW 51012, @errorMessage, 1
	END
	ELSE
	BEGIN
		SELECT @idVersionMechanical = IdVersionMechanical FROM VersionMechanical
			WHERE MechanicalData LIKE @data AND IdMechanicalParameter = @idMechanicalParameter
		IF @idVersionMechanical IS NULL 
		BEGIN
			INSERT INTO VersionMechanical (IdMechanicalParameter, MechanicalData) VALUES (@idMechanicalParameter, @data)
			SET @idVersionMechanical = @@IDENTITY
		END

		IF NOT EXISTS (SELECT * FROM ItemDefinition_VersionMechanical WHERE IdItemDefinition = @idItemDefinition AND IdVersionMechanical = @idVersionMechanical)
		BEGIN
			INSERT INTO ItemDefinition_VersionMechanical (IdItemDefinition, IdVersionMechanical) VALUES (@idItemDefinition, @idVersionMechanical)
		END
		ELSE 
		BEGIN;
			THROW 51005, 'Parámetro de mecánica ya insertado para esta versión interna.', 1
		END
	END
GO

EXECUTE InsertMechanicalVersionPrameter 2, 'Bea', '234556'
GO
EXECUTE InsertMechanicalVersionPrameter 3, 'Windchill 1', '943589'
GO
EXECUTE InsertMechanicalVersionPrameter 4, 'Windchill 1', '490549'
GO
EXECUTE InsertMechanicalVersionPrameter 5, 'Windchill 1', '349589'

GO
SELECT * FROM ItemDefinition_VersionMechanical
SELECT * FROM VersionMechanical
SELECT * FROM MechanicalParameter
GO

CREATE OR ALTER VIEW UnifiedMechanicalParameter AS 
	SELECT IDVM.IdItemDefinition, MP.MechanicalDescription, VM.MechanicalData FROM ItemDefinition_VersionMechanical AS IDVM
	INNER JOIN VersionMechanical AS VM ON IDVM.IdVersionMechanical = VM.IdVersionMechanical
	INNER JOIN MechanicalParameter AS MP ON MP.IdMechanicalParameter = VM.IdMechanicalParameter
GO

SELECT * FROM UnifiedMechanicalParameter

DELETE FROM ItemDefinition_VersionMechanical WHERE IdItemDefinition = 25
GO

CREATE OR ALTER PROCEDURE DeleteMechanicalVersion(
								@idItemDefinition INT
)
AS
	
	DELETE FROM ItemDefinition_VersionMechanical WHERE IdItemDefinition = @idItemDefinition
	
	-- Borrar Mechanical no vinculado a una itemDefinition
GO