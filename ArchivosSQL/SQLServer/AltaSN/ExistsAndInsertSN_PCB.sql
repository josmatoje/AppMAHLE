USE XEDI_TraceabilityDB
GO

CREATE OR ALTER PROCEDURE ExistsAndInsertSN_PCB(
								@SerialNumber NVARCHAR(40),
								@IdItemBomPCB INT,
								@ProyectName NVARCHAR(40),
								@CategorySN NVARCHAR(40),
								@InsertSN BIT,
								@Batch NVARCHAR(40),
								@UserName NVARCHAR(40),
								@WindChillCode NVARCHAR(40),
								@idSerialNumber INT OUT
)
AS
	DECLARE @idItemProject INT
	DECLARE @idItemCategory INT
	DECLARE @idCenter INT
	DECLARE @idWindChillCode INT
	DECLARE @idSNRegisterPCB INT
	DECLARE @batchIN VARCHAR(40)
	DECLARE @idStatus INT
	
	BEGIN TRANSACTION

	SELECT @idSerialNumber = IdSNRegister FROM SNRegister
	WHERE CodeSN = @SerialNumber
	
	IF  @InsertSN = 0
	BEGIN
		IF @idSerialNumber IS NOT NULL
		BEGIN 
			SELECT @idSerialNumber;
		END
		ELSE
		BEGIN
			ROLLBACK;
			THROW 51011, 'SN no encontrado.',1
		END
	END

	ELSE
	BEGIN
		IF @idSerialNumber IS NOT NULL 
		BEGIN
			ROLLBACK;
			THROW 51008, 'SN ya registrado.',1
		END
		ELSE
		BEGIN
			IF @Batch != ''
			BEGIN
				SET @batchIN = @Batch
			END
			SELECT @idItemProject= idItemProject FROM ItemProject
			WHERE ProjectName= @ProyectName
			IF @idItemProject IS NULL
			BEGIN
				ROLLBACK;
				THROW 51003, 'Proyecto no encontrado.',1
			END
			ELSE
			BEGIN
				SELECT @idItemCategory = IdItemCategory FROM ItemCategory
				WHERE CategoryDescription = @CategorySN
				IF @idItemCategory IS NULL
				BEGIN
					ROLLBACK;
					THROW 51009, 'Categoria no definida.',1
				END
				ELSE
				BEGIN
					SELECT @idCenter = IdCenter FROM Center
					WHERE  @UserName LIKE CONCAT('%', UserCode,'%')
					IF @idCenter IS NULL
					BEGIN
						ROLLBACK;
						THROW 51010, 'Centro no encontrado.',1
					END
					ELSE
					BEGIN
						INSERT INTO SNRegister (IdItemProject, IdItemCategory, CodeSN, IdCenter, DateRegistration) 
						VALUES (@idItemProject, @idItemCategory, @SerialNumber, @idCenter, CURRENT_TIMESTAMP)
						SET @idSerialNumber = @@IDENTITY
						INSERT INTO SNRegisterPCB  (IdSNRegister, IdItemBomPCB, Batch)
						VALUES( @idSerialNumber, @IdItemBomPCB,@batchIN )
						SET @idSNRegisterPCB = @@IDENTITY
						SELECT @idStatus = IdStatus FROM StatusType ST WHERE ST.StatusValue ='ALMACEN'
						INSERT INTO SNStatus (IdStatus, IdSNRegister, IdCenter, DateRegister)
						VALUES (@idStatus, @idSerialNumber,@idCenter, CURRENT_TIMESTAMP)
						IF @WindChillCode != ''
						BEGIN
							INSERT INTO SNWindchill (IdSNRegister, WindchillCode)
							VALUES(@idSerialNumber, @WindChillCode)
							SET @idWindChillCode = @@IDENTITY
						END
					END
				END
			END
		END
	END
COMMIT
GO
----------------------------------------------------------------------------
DECLARE @idSNRegister INT;
EXECUTE dbo.ExistsAndInsertSN_PCB '898963',2, 'OBCAMG11', 'PCB' ,1, '','M0137784', '', @idSNRegister;

SELECT * FROM SNDefinitionAndWindChill_View
GO 

select * from SNStatus;


