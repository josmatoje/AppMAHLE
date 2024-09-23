USE XEDI_TraceabilityDB
GO

CREATE OR ALTER PROCEDURE InsertLabellingVersionPrameter (
								@idItemDefinition INT,
								@parameter NVARCHAR(20), 
								@data NVARCHAR(30))
AS
-- Precondiciones: El idInternalVersionDef recibido deberá existir en la bbdd.
-- Postcondiciones: Los errores generados por el procedimiento corresponden a las diferentes casuisticas que se pueden dar y deben ser tratados en el exterior individualmente.
	DECLARE @idVersionLabelling INT
	DECLARE @idLabellingParameter INT
	DECLARE @errorMessage NVARCHAR(200)
    

	SELECT @idLabellingParameter = IdLabellingParameter FROM LabellingParameter
		WHERE LabellingDescription = @parameter
	IF @idLabellingParameter IS NULL 
	BEGIN
		INSERT INTO LabellingParameter(LabellingDescription) VALUES (@parameter)
		SET @idLabellingParameter = @@IDENTITY
		--SELECT @errorMessage = CONCAT('El parámetro ', @parameter, ' de labelling no ha sido encontrado. Consulte con soporte si fuera necesario añadir un parámetro');
		--THROW 51013, @errorMessage, 1
	END
	ELSE
	BEGIN
		SELECT @idVersionLabelling = IdVersionLabelling FROM VersionLabelling
			WHERE LabellingData = @data AND IdLabellingParameter = @idLabellingParameter
		IF @idVersionLabelling IS NULL 
		BEGIN
			INSERT INTO VersionLabelling (IdLabellingParameter, LabellingData) VALUES (@idLabellingParameter, @data)
			SET @idVersionLabelling = @@IDENTITY
		END	

		IF NOT EXISTS (SELECT * FROM ItemDefinition_VersionLabelling WHERE IdItemDefinition = @idItemDefinition AND IdVersionLabelling = @idVersionLabelling)
		BEGIN
			INSERT INTO ItemDefinition_VersionLabelling (IdItemDefinition, IdVersionLabelling) VALUES (@idItemDefinition, @idVersionLabelling)
		END
		--ELSE 
		--BEGIN;
			--THROW 51009, 'Parámetro de labelling ya insertado para esta versión interna.', 1
		--END
	END

GO

SELECT * FROM ItemDefinition_VersionLabelling
SELECT * FROM VersionLabelling
SELECT * FROM LabellingParameter
GO

CREATE OR ALTER VIEW UnifiedLabellingParameter AS 
	SELECT IDVL.IdItemDefinition, LP.LabellingDescription, VL.LabellingData FROM ItemDefinition_VersionLabelling AS IDVL
	INNER JOIN VersionLabelling AS VL ON IDVL.IdVersionLabelling = VL.IdVersionLabelling
	INNER JOIN LabellingParameter AS LP ON LP.IdLabellingParameter = VL.IdLabellingParameter
GO

SELECT * FROM UnifiedLabellingParameter
GO

CREATE OR ALTER PROCEDURE DeleteLabellingVersion(
								@idItemDefinition INT
)
AS
	
	DELETE FROM ItemDefinition_VersionLabelling WHERE IdItemDefinition = @idItemDefinition
	
	-- Borrar Labelling no vinculado a una itemDefinition
GO