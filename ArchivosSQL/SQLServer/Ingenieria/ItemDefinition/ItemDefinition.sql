USE XEDI_TraceabilityDB
GO

CREATE OR ALTER PROCEDURE InsertItemDefinition (
							@projectName NVARCHAR(60),
							@sampleName NVARCHAR(30),
							@partNumber NVARCHAR(60),
							@descriptionItem NVARCHAR(60),
							@internalName NVARCHAR(30),
							@descriptionReference NVARCHAR(60),
							@isTrazability BIT,
							@pictureName NVARCHAR(255),
							@pictureData VARBINARY(MAX), 
                            @dateRegistration NVARCHAR(10),
							@userName NVARCHAR(30),
							@idInternalVersion INT OUTPUT
)
AS
    DECLARE @idCenter INT
	DECLARE @idProject INT
	DECLARE @idSample INT
	DECLARE @idPartNumber INT
	DECLARE @idPicture INT

	IF EXISTS (	SELECT * FROM ItemDefinition AS ID
				INNER JOIN ItemPartNumber AS IPN ON ID.IdItemPN = IPN.IdItemPN
				INNER JOIN ItemSample AS S ON IPN.IdItemSample = S.IdItemSample
				INNER JOIN ItemProject AS P ON S.IdItemProject = P.IdItemProject
				WHERE InternalName = @internalName AND
						ItemPNName = @partNumber AND
						ItemSampleName = @sampleName AND
						ProjectName = @projectName)
	BEGIN;
		THROW 51004, 'Versión interna ya definida.', 1
	END
	
    SELECT @idCenter = IdCenter FROM Center 
	WHERE @userName LIKE CONCAT('%', UserCode,'%')

	SELECT @idProject = IdItemProject FROM ItemProject
	WHERE ProjectName = @projectName
	IF @idProject IS NULL
	BEGIN;
		THROW 51003, 'Proyecto no encontrado.', 1
	END

	SELECT @idSample = IdItemSample FROM ItemSample 
	WHERE ItemSampleName = @sampleName AND IdItemProject = @idProject
	IF @idSample IS NULL
	BEGIN
		INSERT INTO ItemSample(IdItemProject, ItemSampleName)
			VALUES (@idProject, @sampleName)
		SET @idSample = @@IDENTITY
	END

	SELECT @idPartNumber = IdItemPN FROM ItemPartNumber 
	WHERE ItemPNName = @partNumber AND IdItemSample = @idSample
	IF @idPartNumber IS NULL
	BEGIN
		INSERT INTO ItemPartNumber(IdItemSample, ItemPNName)
			VALUES (@idSample, @partNumber)
		SET @idPartNumber = @@IDENTITY
	END
	
	SELECT @idPicture = IdPictureStorage FROM PictureStorage WHERE PictureName = @pictureName
	IF @idPicture IS NULL AND @pictureName IS NOT NULL
	BEGIN
		INSERT INTO PictureStorage(PictureName, PictureData) VALUES (@pictureName, @pictureData)
		SET @idPicture = @@IDENTITY
	END

	INSERT INTO ItemDefinition(IdItemDescription,IdItemPN,InternalName,DescriptionReference,IsTrazability,EnableItem,IdPictureStorage,DateRegistration,IdCenter)
		 VALUES ((SELECT IdItemDescription FROM ItemDescription WHERE DescriptionItem = @descriptionItem),@idPartNumber,@internalName,@descriptionReference,@isTrazability,0,@idPicture,CONVERT(DATETIME, @dateRegistration, 103),@idCenter)
		 
	SET @idInternalVersion = @@IDENTITY

GO
--FIN   


DECLARE @idIV INT
EXECUTE dbo.InsertItemDefinition 'OBCAMG11', 'sample', 'part number', 'Versión interna', 'prueba7', 'prueba description7', 0, NULL, NULL, '2024-08-06' , 'E0164372', @idInternalVersion = @idIV OUTPUT
SELECT @idIV

GO
DELETE FROM ItemDefinition WHERE IdItemDefinition = 19
GO

SELECT * FROM ItemPartNumber
SELECT * FROM ItemSample
SELECT * FROM ItemProject
SELECT * FROM ItemDefinition
SELECT * FROM ItemDescription

SELECT * FROM ItemDefinitionParameters_View

begin transaction
		INSERT INTO PictureStorage(PictureName, PictureData) VALUES (null, null)
rollback
commit
go
SELECT CONVERT(DATETIME, '20/07/2012', 103)

DELETE FROM ItemDefinition WHERE IdItemDefinition = 34
-------------------------------------------------------------------------------------------------------

SELECT * FROM ProcessDescription
GO

SELECT COUNT(*) FROM ItemDefinition AS ID
			INNER JOIN ItemPartNumber AS IPN ON ID.IdItemPN = IPN.IdItemPN
			INNER JOIN ItemSample AS S ON IPN.IdItemSample = S.IdItemSample
			WHERE InternalName IN (SELECT InternalName FROM ItemDefinition) AND
				S.IdItemProject IN (SELECT DISTINCT S.IdItemProject FROM ItemDefinition AS ID
									INNER JOIN ItemPartNumber AS IPN ON ID.IdItemPN = IPN.IdItemPN
									INNER JOIN ItemSample AS S ON IPN.IdItemSample = S.IdItemSample)

SELECT IdItemDefinition FROM ItemDefinition AS ID
				INNER JOIN ItemPartNumber AS IPN ON ID.IdItemPN = IPN.IdItemPN
				INNER JOIN ItemSample AS S ON IPN.IdItemSample = S.IdItemSample
				INNER JOIN ItemProject AS P ON S.IdItemProject = P.IdItemProject
				WHERE ProjectName = 'OBCAMG11' AND
						ItemSampleName = 'B3' AND
						ItemPNName = '01OBCAMG11/1-400/B3.1' AND
						InternalName = 'B3.3.3' AND
						DescriptionReference = 'B3.3.3 SAMPLE' AND
						IsTrazability = 0 AND
						DateRegistration = CONVERT(DATETIME, '15/09/2024', 103) 
GO
						
CREATE OR ALTER PROCEDURE DeleteItemDefinition(
								@idItemDefinition INT
)
AS
	
	DELETE FROM ItemDefinition WHERE IdItemDefinition = @idItemDefinition
GO
