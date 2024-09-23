USE XEDI_TraceabilityDB
GO

CREATE OR ALTER PROCEDURE InsertSoftwareVersionPrameter (
								@idItemDefinition INT,
								@parameter NVARCHAR(20), 
								@data NVARCHAR(30))
AS
-- Precondiciones: El idInternalVersionDef recibido deberá existir en la bbdd.
-- Postcondiciones: Los errores generados por el procedimiento corresponden a las diferentes casuisticas que se pueden dar y deben ser tratados en el exterior individualmente.
	DECLARE @idVersionSoftware INT
	DECLARE @idSoftwareParameter INT
	DECLARE @errorMessage NVARCHAR(200)
    
	SELECT @idSoftwareParameter = IdSoftwareParameter FROM SoftwareParameter
		WHERE SoftwareDescription = @parameter
	IF @idSoftwareParameter IS NULL 
	BEGIN
		--INSERT INTO SoftwareParameter(SoftwareDescription) VALUES (@parameter)
		--SET @idSoftwareParameter = @@IDENTITY
		SELECT @errorMessage = CONCAT('El parámetro ', @parameter, ' de Software no ha sido encontrado. Consulte con soporte si fuera necesario añadir un parámetro');
		THROW 51013, @errorMessage, 1
	END
	ELSE
	BEGIN
		SELECT @idVersionSoftware = IdVersionSoftware FROM VersionSoftware
			WHERE SoftwareData = @data AND IdSoftwareParameter = @idSoftwareParameter
		IF @idVersionSoftware IS NULL 
		BEGIN
			INSERT INTO VersionSoftware (IdSoftwareParameter, SoftwareData) VALUES (@idSoftwareParameter, @data)
			SET @idVersionSoftware = @@IDENTITY
		END	
		IF NOT EXISTS (SELECT * FROM ItemDefinition_VersionSoftware WHERE IdItemDefinition = @idItemDefinition AND IdVersionSoftware = @idVersionSoftware)
		BEGIN
			INSERT INTO ItemDefinition_VersionSoftware (IdItemDefinition, IdVersionSoftware) VALUES (@idItemDefinition, @idVersionSoftware)
		END
	END
GO

SELECT * FROM ItemDefinition_VersionSoftware
SELECT * FROM VersionSoftware
SELECT * FROM SoftwareParameter
GO

CREATE OR ALTER VIEW UnifiedSoftwareParameter AS 
	SELECT IDVS.IdItemDefinition, SP.SoftwareDescription, VS.SoftwareData FROM ItemDefinition_VersionSoftware AS IDVS
	INNER JOIN VersionSoftware AS VS ON IDVS.IdVersionSoftware = VS.IdVersionSoftware
	INNER JOIN SoftwareParameter AS SP ON SP.IdSoftwareParameter = VS.IdSoftwareParameter
GO

SELECT * FROM UnifiedSoftwareParameter
GO

CREATE OR ALTER PROCEDURE DeleteSoftwareVersion(
								@idItemDefinition INT
)
AS
	
	DELETE FROM ItemDefinition_VersionSoftware WHERE IdItemDefinition = @idItemDefinition
	
	-- Borrar Software no vinculado a una itemDefinition
	DELETE FROM VersionSoftware
		WHERE IdVersionSoftware NOT IN (
			SELECT VS.IdVersionSoftware FROM ItemDefinition_VersionSoftware AS IDVS
			INNER JOIN VersionSoftware AS VS ON IDVS.IdVersionSoftware = VS.IdVersionSoftware)
GO

SELECT * FROM VersionSoftware


INSERT INTO VersionSoftware VALUES(2, 'TINKIWINKI')

DELETE FROM VersionSoftware
WHERE IdVersionSoftware NOT IN (
	SELECT VS.IdVersionSoftware FROM ItemDefinition_VersionSoftware AS IDVS
	INNER JOIN VersionSoftware AS VS ON IDVS.IdVersionSoftware = VS.IdVersionSoftware)
