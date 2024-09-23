USE XEDI_TraceabilityDB
GO

CREATE OR ALTER PROCEDURE InsertTestVersion (
								@idItemDefinition INT,
								@testDescription NVARCHAR(30),
								@versionTest INT)
AS
	DECLARE @idProcessTest INT

	SELECT @idProcessTest = IdProcessTest FROM ProcessTest
		WHERE TestDescription = @testDescription

	IF NOT EXISTS (SELECT * FROM ItemDefinition_ProcessTest WHERE IdItemDefinition = @idItemDefinition AND IdProcessTest = @idProcessTest AND VersionTest = @versionTest)
	BEGIN
		INSERT INTO ItemDefinition_ProcessTest VALUES (@idItemDefinition, @idProcessTest, @versionTest)
	END

GO

SELECT * FROM ItemDefinition_ProcessTest
SELECT * FROM ProcessTest
GO

CREATE OR ALTER VIEW UnifiedTestParameter AS 
	SELECT IDPT.IdItemDefinition, PT.TestDescription, IDPT.VersionTest FROM ItemDefinition_ProcessTest AS IDPT
	INNER JOIN ProcessTest AS PT ON IDPT.IdProcessTest = PT.IdProcessTest
GO

SELECT * FROM UnifiedTestParameter
GO

CREATE OR ALTER PROCEDURE DeleteTestVersion(
								@idItemDefinition INT
)
AS
	
	DELETE FROM ItemDefinition_ProcessTest WHERE IdItemDefinition = @idItemDefinition
	
	-- No Borrar Test, solo se sube de forma manual.
GO