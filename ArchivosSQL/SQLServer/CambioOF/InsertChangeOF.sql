USE XEDI_TraceabilityDB
GO

CREATE OR ALTER PROCEDURE InsertChangeOF(
								@SerialNumber NVARCHAR(40),
								@oldOF NVARCHAR(40),
								@newOF NVARCHAR(40),
								@comments NVARCHAR(40),
								@opStart INT,
								@UserName NVARCHAR(40)
)
AS
	DECLARE @idoldOF INT
	DECLARE @idnewOF INT
	DECLARE @idSerialNumber INT
	DECLARE @countProcess INT
	DECLARE @internalName NVARCHAR(40)
	DECLARE @lastStatus NVARCHAR(40)
	DECLARE @idnewStatus INT
	DECLARE @lastOpSN INT
	DECLARE @idCenter INT
	DECLARE @idManufacturing INT
	DECLARE @idpending INT
	DECLARE @idManufacturingComent INT
	DECLARE @idStatus INT
	DECLARE @StatusSN NVARCHAR(40)
	
	BEGIN TRANSACTION

	SELECT @idoldOF= OFN.IdOF FROM OrderFabrication AS OFN
	WHERE OFN.CodeOF=@oldOF

	SELECT @idnewOF= OFN.IdOF, @internalName = ID.InternalName FROM OrderFabrication AS OFN
	INNER JOIN ItemDefinition AS ID ON OFN.IdItemDefinition = ID.IdItemDefinition
	WHERE OFN.CodeOF=@newOF
	
	IF  @idoldOF  IS  NULL OR @idnewOF IS NULL
	BEGIN 
		ROLLBACK;
		THROW 51018, 'Código OF no encontrado.',1
		
	END
	IF @idoldOF = @idnewOF
	BEGIN
		ROLLBACK;
		THROW 51025, 'Códigos OFs idénticos.',1
	END

	SELECT @idSerialNumber= SNR.IdSNRegister FROM SNRegister AS SNR
	WHERE SNR.CodeSN=@SerialNumber

	IF @idSerialNumber IS NULL
	BEGIN 
		ROLLBACK;
		THROW 51011, 'SN no encontrado.',1
		
	END

	SELECT TOP 1 @StatusSN = StatusValue FROM SNStatus
	INNER JOIN StatusType ON SNStatus.IdStatus = StatusType.IdStatus
	WHERE IdSNRegister = @idSerialNumber
	ORDER BY IdSNStatus DESC

	SELECT * FROM SNStatus
	INNER JOIN StatusType ON SNStatus.IdStatus = StatusType.IdStatus
	WHERE IdSNRegister = 7
	ORDER BY IdSNStatus DESC


	IF @StatusSN != 'REWORK'
		BEGIN 
			ROLLBACK;
			THROW 51030, 'SN no se encuentra en un estado de Rework.',1
		
		END

	SELECT @idCenter = IdCenter FROM Center
		WHERE  @UserName LIKE CONCAT('%', UserCode,'%')
		IF @idCenter IS NULL
		BEGIN
			ROLLBACK;
			THROW 51010, 'Centro no encontrado.',1
		END

	SELECT @countProcess = count (PD.IdProcessDefinition) FROM ProcessDefinition PD
	RIGHT JOIN VersionProcess AS VP ON PD.IdVersionProcess = VP.IdVersionProcess
	RIGHT JOIN ItemDefinition_VersionProcess AS IDVP ON VP.IdVersionProcess = IDVP.IdVersionProcess
	RIGHT JOIN ItemDefinition AS ID ON IDVP.IdItemDefinition = ID.IdItemDefinition
	WHERE ID.InternalName =@internalName

	SELECT  TOP 1 @lastOpSN = MFR.Num, @lastStatus = MRR.TypeManufacturingResult
	FROM SNRegister AS SNR
	INNER JOIN ManufacturingRegister AS MFR ON SNR.IdSNRegister=MFR.IdSNRegister
	INNER JOIN ManufacturingResult AS MRR ON MFR.IdManufacturingResult = MRR.IdManufacturingResult
	WHERE SNR.CodeSN= @SerialNumber
	ORDER BY MFR.DateStart DESC, MFR.Num DESC

	IF @lastOpSN IS NULL OR @lastStatus != 'REWORK'
	BEGIN 
		ROLLBACK;
		THROW 51019, 'Última operacion de Rework no encontrada para este SN.',1
		
	END

	IF @opStart >= @countProcess OR  @opStart > @lastOpSN
	BEGIN
		IF @opStart >= @countProcess
		BEGIN
			ROLLBACK;
			THROW 51020, 'Operacion introducida mayor que el numero total de operaciones del AG.',1
		END
		ELSE
		BEGIN
			ROLLBACK;
			THROW 51021, 'Operacion introducida mayor que la última operación registrada del SN.',1
		END
	END
	ELSE
	BEGIN 
		SELECT @idnewStatus= MRR.IdManufacturingResult FROM ManufacturingResult AS MRR WHERE MRR.TypeManufacturingResult ='CHANGE_OF';
		INSERT INTO ManufacturingRegister (IdSNRegister, IdManufacturingResult, Num, DateStart, DateEnd, IdCenter,idOF)
		VALUES(@idSerialNumber, @idnewStatus, @opStart,CURRENT_TIMESTAMP, CURRENT_TIMESTAMP,@idCenter,  @idnewOF)
		SET @idManufacturing = @@IDENTITY
		SELECT @idStatus = IdStatus FROM StatusType ST WHERE ST.StatusValue ='OK'
		INSERT INTO SNStatus (IdStatus, IdSNRegister, IdCenter, DateRegister)
		VALUES (@idStatus, @idSerialNumber,@idCenter, CURRENT_TIMESTAMP)
		IF @idManufacturing IS NULL
			BEGIN
				ROLLBACK;
				THROW 51022, 'No se han podido guardar los datos en el registro de Manufacturing.',1
			END
		ELSE
		BEGIN
			IF @comments IS NOT NULL
			BEGIN
				SELECT @idManufacturingComent = IdManufacturingComent  FROM ManufacturingComent WHERE Coment=@comments
				IF @idManufacturingComent IS NULL
				BEGIN
					INSERT INTO ManufacturingComent (Coment)
					VALUES (@comments)
					SET @idManufacturingComent = @@IDENTITY
				END
				INSERT INTO ManufacturingRegister_Coment (IdManufacturingRegister, IdManufacturingComent)
				VALUES (@idManufacturing, @idManufacturingComent)
			END
			UPDATE OrderFabrication_SNRegister
			SET IdOF = @idnewOF
			WHERE  IdSNRegister= @idSerialNumber
			INSERT INTO SNPendingOperationManufacturing (IdSNRegister, NumPending,  DateRegister)
			VALUES (@idSerialNumber, @opStart, CURRENT_TIMESTAMP)
			SET @idpending = @@IDENTITY
			IF @idpending IS NULL
			BEGIN
				ROLLBACK;
				THROW 51024, 'No se ha podido insertar la operacion como pendiente.',1
			END
			

		END
	END
	
COMMIT
GO
----------------------------------------------------------------------------

EXECUTE dbo.InsertChangeOF '123456', '123', '84', 'Se cambia porque fallo', 1,'M0137784'

SELECT * FROM SNStatus WHERE IdSNRegister=7

SELECT SNR.CodeSN, SNPM.NumPending FROM SNPendingOperationManufacturing SNPM
INNER JOIN SNRegister SNR ON SNPM.IdSNRegister = SNR.IdSNRegister
WHERE  SNR.CodeSN = '123456'

SELECT * FROM ManufacturingComent;

 SELECT * FROM ManufacturingComent MC
 RIGHT JOIN ManufacturingRegister_Coment MCC ON MC.IdManufacturingComent = MCC.IdManufacturingComent
 RIGHT JOIN ManufacturingRegister MR ON MCC.IdManufacturingRegister = MR.IdManufacturingRegister
 RIGHT JOIN SNRegister SNR ON MR.IdSNRegister = SNR.IdSNRegister
 WHERE  SNR.CodeSN = '123456'

SELECT * FROM ListOF_SN_View  WHERE CodeSN = '123456'

	SELECT * FROM SNRegister AS SNR
	INNER JOIN ManufacturingRegister AS MFR ON SNR.IdSNRegister=MFR.IdSNRegister
	WHERE SNR.CodeSN='123456'
	ORDER BY MFR.DateStart DESC, MFR.Num DESC

	SELECT * FROM SNStatus