USE XEDI_TraceabilityDB
GO
BEGIN TRANSACTION

GO
CREATE OR ALTER PROCEDURE InsertProcessDefinition(
								@processDesignation NVARCHAR(40),
								@num INT,
								@processNum INT,
								@processDescription NVARCHAR(40),
								@operation INT,
								@operationDescription NVARCHAR(60),
								@pictureName NVARCHAR(255),
								@stationName NVARCHAR(30),
								@definitionTest NVARCHAR(30),
								@inputType NVARCHAR(20),
								@screwCode NVARCHAR(100),
								@referenceAssembly NVARCHAR(20)
)
AS
	DECLARE @idVersionProcess INT
	DECLARE @idProcessDescription INT
	DECLARE @idPicture INT
	DECLARE @idStation INT
	DECLARE @idDefinitionProcess INT
	DECLARE @idDefinitionTest INT
	DECLARE @idDefinitionInput INT
	DECLARE @idDefinitionScrew INT
	DECLARE @idDefinitionItem INT
	DECLARE @errorMessage NVARCHAR(200)
	
	SELECT @idPicture = IdPictureStorage FROM PictureStorage
	WHERE PictureName = @pictureName
	IF @idPicture IS NULL
	BEGIN
		SELECT @errorMessage =  CONCAT_WS(' ', 'Imagen', @pictureName, 'no encontrada. Debe seleccionar la imagen en la carpeta de imagenes para ser añadida a la BBDD.');
		THROW 51028, 'Imagen no encontrada. Debe subir la imagen.', 1
	END

	SELECT @idStation = IdStation FROM Station
	WHERE StationName = @stationName
	IF @idStation IS NULL
	BEGIN
		SELECT @errorMessage =  CONCAT_WS(' ', 'Estación', @stationName, 'no encontrada. Compruebe que el nombre de la estación es correcta.');
		THROW 51027,@errorMessage , 1
	END
	
	SELECT @idVersionProcess = IdVersionProcess FROM VersionProcess WHERE ProcessDesignation = @processDesignation
	IF @idVersionProcess IS NULL
	BEGIN
		INSERT INTO VersionProcess (ProcessDesignation) VALUES (@processDesignation)
		SET @idVersionProcess = @@IDENTITY
	END
	
	SELECT @idProcessDescription = IdProcessDescription FROM ProcessDescription AS PD WHERE PD.ProcessDescription = @processDescription
	IF @idProcessDescription IS NULL
	BEGIN
		INSERT INTO ProcessDescription (ProcessDescription) VALUES (@processDescription)
		SET @idProcessDescription = @@IDENTITY
	END

	IF NOT EXISTS (SELECT * FROM ProcessDefinition WHERE IdVersionProcess = @idVersionProcess AND Num = @num AND ProcessNum = @processNum AND IdProcessDescription = @idProcessDescription AND Operation = @operation AND OperationDescription = @operationDescription AND IdPicture = @idPicture)
	BEGIN
		INSERT INTO ProcessDefinition(IdVersionProcess ,Num ,ProcessNum ,IdProcessDescription ,Operation ,OperationDescription ,IdPicture ,DateRegistration)
			VALUES (@idVersionProcess, @num, @processNum, @idProcessDescription, @operation, @operationDescription, @idPicture, CURRENT_TIMESTAMP)
		SET @idDefinitionProcess = @@IDENTITY
	
		INSERT INTO Process_Station_Definition(IdProcessDefinition, IdStation) VALUES (@idDefinitionProcess, @idStation)

		IF @definitionTest IS NOT NULL AND @definitionTest != '-'
		BEGIN
			SELECT @idDefinitionTest = IdProcessTest FROM ProcessTest WHERE TestDescription = @definitionTest
			IF @idDefinitionTest IS NULL
			BEGIN
				INSERT INTO ProcessTest (TestDescription, IdStation) VALUES (@definitionTest, @idStation)
				SET @idDefinitionTest = @@IDENTITY
			END
			INSERT INTO Process_Test_Definition VALUES(@idDefinitionProcess, @idDefinitionTest)
		END

		IF @inputType IS NOT NULL AND @inputType != '-'
		BEGIN
			SELECT @idDefinitionInput = IdProcessInput FROM ProcessInput
			WHERE InputType = @inputType
			IF @idDefinitionInput IS NULL
			BEGIN
				INSERT INTO ProcessInput(InputType) VALUES(@inputType)
				SET @idDefinitionInput = @@IDENTITY
			END
			INSERT INTO Process_Input_Definition VALUES(@idDefinitionProcess, @idDefinitionInput)
		END

		IF @screwCode IS NOT NULL AND @screwCode != '-'
		BEGIN
			SELECT @idDefinitionScrew = IdProcessScrewDriver FROM ProcessScrewDriver
			WHERE ScrewCode = @screwCode
			IF @idDefinitionScrew IS NULL
			BEGIN
				INSERT INTO ProcessScrewDriver(ScrewCode) VALUES(@screwCode)
				SET @idDefinitionScrew = @@IDENTITY
			END
			INSERT INTO Process_ScrewDriver_Definition VALUES(@idDefinitionProcess, @idDefinitionScrew)
		END

		IF @referenceAssembly IS NOT NULL AND @referenceAssembly != '-'
		BEGIN
			SELECT @idDefinitionItem = IdProcessItem FROM ProcessItem
			WHERE Reference = @referenceAssembly
			IF @idDefinitionItem IS NULL
			BEGIN
				INSERT INTO ProcessItem(Reference) VALUES(@referenceAssembly)
				SET @idDefinitionItem = @@IDENTITY
			END
			INSERT INTO Process_Item_Definition VALUES(@idDefinitionProcess, @idDefinitionItem)
		END
	END
GO

DECLARE @idProcessDescription INT
SELECT @idProcessDescription = IdProcessDescription FROM ProcessDescription AS PD WHERE PD.ProcessDescription = 'GP12'
SELECT * FROM ProcessDescription AS PD WHERE PD.ProcessDescription = 'GP12'
SELECT  @idProcessDescription
GO

CREATE OR ALTER PROCEDURE DeleteProcessDefinition(
								@processDesignation NVARCHAR(40)
)
AS
	DECLARE @idVersionProcess INT

	SELECT @idVersionProcess = IdVersionProcess FROM VersionProcess WHERE ProcessDesignation = @processDesignation
	
	DELETE FROM Process_Item_Definition WHERE IdProcessDefinition IN (SELECT IdProcessDefinition FROM ProcessDefinition WHERE IdVersionProcess = @idVersionProcess)
	-- Borrar resto de parámetros y ProcessDefinition
GO

BEGIN TRANSACTION
DELETE FROM Process_Item_Definition WHERE IdProcessDefinition IN 
				(SELECT IdProcessDefinition FROM ProcessDefinition WHERE IdVersionProcess = 7)

ROLLBACK

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
