USE XEDI_TraceabilityDB
GO

CREATE OR ALTER PROCEDURE GetWindChillListFromCategory(
								@CategoryWindChill NVARCHAR(40)
)
AS
	BEGIN TRANSACTION

	IF  @CategoryWindChill = 'PCB'
	BEGIN
		SELECT Windchill FROM HardwarePCBReferenceLayoutBom;
	END
	ELSE
	BEGIN
		SELECT WindChillCode FROM Category_WindChill_View 
		WHERE CategoryWindChill LIKE CONCAT('%', @CategoryWindChill,'%');
	END
COMMIT
GO
----------------------------------------------------------------------------

EXECUTE dbo.GetWindChillListFromCategory 'Cooler';
