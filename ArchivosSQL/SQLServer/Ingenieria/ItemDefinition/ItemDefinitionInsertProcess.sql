USE XEDI_TraceabilityDB
GO

CREATE OR ALTER PROCEDURE InsertProcessVersion (
								@idItemDefinition INT,
								@processDesignation NVARCHAR(40))
AS
	DECLARE @idVersionProcess INT
	
	SELECT @idVersionProcess = IdVersionProcess FROM ItemDefinition_VersionProcess 
		WHERE IdItemDefinition = @idItemDefinition

	IF @idVersionProcess IS NULL
	BEGIN
		SELECT @idVersionProcess = IdVersionProcess FROM VersionProcess
						WHERE ProcessDesignation = @processDesignation
		
		IF @idVersionProcess IS NULL
		BEGIN;
			--INSERT INTO VersionProcess (ProcessName) VALUES(@processName)
			--SET @idVersionProcess = @@IDENTITY
			
			THROW 51007, 'AG no registrado, contacte con el Ingerniero de Procesos.', 1
		END

		INSERT INTO ItemDefinition_VersionProcess(IdItemDefinition, IdVersionProcess) VALUES (@idItemDefinition, @idVersionProcess)

	END
	ELSE
	BEGIN
		IF NOT EXISTS (SELECT * FROM VersionProcess
						WHERE ProcessDesignation = @processDesignation)
			THROW 51008, 'Bloque de proceso anterior no corresponde al actual.', 1
	END
GO

SELECT * FROM ItemDefinition_VersionProcess
SELECT * FROM VersionProcess
GO

CREATE OR ALTER VIEW UnifiedProcess AS 
	SELECT IDVP.IdItemDefinition, VP.ProcessDesignation FROM ItemDefinition_VersionProcess AS IDVP
	INNER JOIN VersionProcess AS VP ON IDVP.IdVersionProcess = VP.IdVersionProcess
GO

SELECT * FROM UnifiedProcess
GO

CREATE OR ALTER PROCEDURE DeleteProcessVersion(
								@idItemDefinition INT
)
AS
	
	DELETE FROM ItemDefinition_VersionProcess WHERE IdItemDefinition = @idItemDefinition
	
	-- No Borrar Process, solo se sube mediante un AG.
GO
