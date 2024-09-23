USE XEDI_TraceabilityDB
GO

CREATE OR ALTER PROCEDURE GetLastInforFromSN(
								@SerialNumber NVARCHAR(40)
)
AS
	DECLARE @idSerialNumber INT
	DECLARE @nameOF NVARCHAR(40)
	DECLARE @internalName NVARCHAR(40)
	DECLARE @lastOp INT
	DECLARE @countOPPending INT


	SET @countOPPending=0;
	
	BEGIN TRANSACTION

	SELECT @idSerialNumber= SNR.IdSNRegister FROM SNRegister AS SNR
	WHERE SNR.CodeSN=@SerialNumber
	
	IF  @idSerialNumber  IS  NULL
	BEGIN 
		ROLLBACK;
		THROW 51011, 'SN no encontrado.',1
		
	END

	SELECT @nameOF = OFN.CodeOF, @internalName = ID.InternalName 
	FROM OrderFabrication_SNRegister AS OFSN
	INNER JOIN  OrderFabrication AS OFN ON OFSN.IdOF = OFN.IdOF 
	INNER JOIN ItemDefinition AS ID ON OFN.IdItemDefinition = ID.IdItemDefinition
	WHERE OFSN.IdSNRegister = @idSerialNumber

	IF  @nameOF  IS  NULL
	BEGIN 
		ROLLBACK;
		THROW 51018, 'Código OF no encontrado.',1
	END

	SELECT  TOP 1 @lastOp = Num FROM ManufacturingRegister MR
	INNER JOIN SNRegister SNR ON MR.IdSN = SNR.IdSNRegister
	INNER JOIN ManufacturingResult MFR ON MR.IdManufacturingResult = MFR.IdManufacturingResult
	WHERE SNR.CodeSN=@SerialNumber AND MFR.TypeManufacturingResult='REWORK'
	ORDER BY MR.DateStart DESC, MR.Num DESC

	IF  @lastOp  IS  NULL
	BEGIN 
		ROLLBACK;
		THROW 51026, 'Última operacion no encontrada.',1
	END

	SELECT @countOPPending = COUNT( IdSNPendingOperation) FROM SNPendingOperationManufacturing
	WHERE IdSNRegister=@idSerialNumber



	SELECT @nameOF AS CodeOF, @internalName AS InternalName, @lastOp AS LastOP, @countOPPending AS CountOpPending

	
COMMIT
GO
----------------------------------------------------------------------------

EXECUTE dbo.GetLastInforFromSN '123456';



