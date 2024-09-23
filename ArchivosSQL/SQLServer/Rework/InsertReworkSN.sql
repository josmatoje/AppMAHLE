USE XEDI_TraceabilityDB
GO

CREATE OR ALTER PROCEDURE InsertReworkSN(
								@ReworkNum INT,
								@SerialNumber NVARCHAR(40),
								@UserName NVARCHAR(40)
)
AS
	DECLARE @idCenter INT
	DECLARE @idSNRegisterPCB INT
	DECLARE @idManufacturing INT
	
	BEGIN TRANSACTION

	SELECT @idSNRegisterPCB=IdSNRegisterPCB FROM SNRegisterPCB SNRPCB
	INNER JOIN SNRegister AS SNR ON SNRPCB.IdSNRegister = SNR.IdSNRegister
	WHERE SNR.CodeSN=@SerialNumber;
	
	IF  @idSNRegisterPCB  IS  NULL
	BEGIN 
		ROLLBACK;
		THROW 51011, 'SN no encontrado.',1
		
	END

	SELECT @idManufacturing= IdManufacturingReworkPCB FROM ManufacturingReworkPCB
	WHERE IdSNRegisterPCB= @idSNRegisterPCB AND Rework=@ReworkNum;

	IF  @idManufacturing  IS NOT  NULL
	BEGIN 
		ROLLBACK;
		THROW 51015, 'Rework ya registrado.',1
	END

	IF @idManufacturing IS NULL AND @idSNRegisterPCB IS NOT NULL
	BEGIN 
		SELECT @idCenter = IdCenter FROM Center
		WHERE  @UserName LIKE CONCAT('%', UserCode,'%')
		IF @idCenter IS NULL
		BEGIN
			ROLLBACK;
			THROW 51010, 'Centro no encontrado.',1
		END
		INSERT INTO ManufacturingReworkPCB (IdSNRegisterPCB, Rework, IdCenter, RegistrationDate) 
						VALUES (@idSNRegisterPCB,@ReworkNum,  @idCenter, CURRENT_TIMESTAMP)
						SET @idManufacturing = @@IDENTITY

	END
COMMIT

GO
----------------------------------------------------------------------------

EXECUTE dbo.InsertReworkSN 5,'9498','M0137784';

SELECT * FROM SNDefinitionAndWindChill_View
where CodeSN = '9498'
GO 

