USE XEDI_TraceabilityDB
GO

CREATE OR ALTER PROCEDURE InsertOFItemDefinition(
								@codeOF NVARCHAR(40),
								@internalName NVARCHAR(40),
								@UserName NVARCHAR(40),
								@DescripcionOF NVARCHAR(40),
								@quantity INT
)
AS
	DECLARE @idOF INT
	DECLARE @idItemDefinition INT
	DECLARE @idOrderFabrication INT
	DECLARE @idCenter INT

	BEGIN TRANSACTION

	SELECT @idOF=IdOF FROM OrderFabrication AS IDOF
	WHERE IDOF.CodeOF=@codeOF
	
	IF  @idOF  IS NOT NULL
	BEGIN 
		ROLLBACK;
		THROW 51016, 'Código OF ya registrado.',1
		
	END
		SELECT @idItemDefinition=IdItemDefinition FROM ItemDefinition AS ID
		WHERE ID.InternalName = @internalName AND ID.EnableItem=1
		IF @idItemDefinition IS NULL
		BEGIN
			ROLLBACK;
			THROW 51017, 'Internal Name no encontrado.',1
		END
		SELECT @idCenter = IdCenter FROM Center
		WHERE  @UserName LIKE CONCAT('%', UserCode,'%')
		IF @idCenter IS NULL
		BEGIN
			ROLLBACK;
			THROW 51010, 'Centro no encontrado.',1
		END
			INSERT INTO OrderFabrication (IdItemDefinition, CodeOF, DescritionOF, Quantity, IdCenter, DateRegistration) 
						VALUES (@idItemDefinition,@codeOF, @DescripcionOF, @quantity, @idCenter, CURRENT_TIMESTAMP)
						SET @idOrderFabrication = @@IDENTITY
		
COMMIT
GO
----------------------------------------------------------------------------

EXECUTE dbo.InsertOFItemDefinition '545477','prueba4','M0137784','Prueba creacion OF',50;
GO

SELECT * FROM OrderFabrication_View
