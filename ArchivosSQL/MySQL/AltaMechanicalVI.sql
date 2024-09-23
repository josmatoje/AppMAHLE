
-- Elimina funcióm anterior
DROP FUNCTION IF EXISTS GetIdMechanicalVersion;
DELIMITER ;;
CREATE FUNCTION GetIdMechanicalVersion (
								plataforma INT(11), 
								windchill VARCHAR(45), 
								lb VARCHAR(45))
RETURNS INT(11)
BEGIN
	DECLARE idMechanicalVersion INT(11);
    
    SELECT idMechanical_Version INTO idMechanicalVersion FROM Mechanical_Version MV
	INNER JOIN Mechanical_WindchillCode_Housing MWH ON MV.idWindchillCode = MWH.idMechanical_WindchillCode_Housing
    WHERE MV.LB = lb AND MWH.Code = windchill AND MWH.idPlataforma = plataforma;
    
	RETURN idMechanicalVersion;
END;;

SELECT GetIdMechanicalVersion(1, '2017176A00', 'B2.1.2');

-- Elimina procedimiento anterior
DROP PROCEDURE IF EXISTS InsertOrCheckMechanicalVersion;
DELIMITER ;;
CREATE PROCEDURE InsertOrCheckMechanicalVersion (
								idInternalVersionDef INT(11), 
								plataforma VARCHAR(150),  
								windchill VARCHAR(45), 
								lbCode VARCHAR(45))
BEGIN
	DECLARE idMechanicalVersion INT(11);
	DECLARE idMechanicalWindchill INT(11);
	DECLARE idSavedPlataform INT(11);
	DECLARE exit handler for SQLEXCEPTION
    BEGIN
        RESIGNAL;
		ROLLBACK;
    END;
--     
--     START TRANSACTION;
    
    SELECT idPlataforma INTO idSavedPlataform FROM Plataforma WHERE Nombre_Plataforma = plataforma;
    
    IF idSavedPlataform IS NULL THEN
		SIGNAL SQLSTATE '10022' SET MESSAGE_TEXT = 'Plataforma no encontrada';
	ELSE
		SELECT idMechanical_Version INTO idMechanicalVersion FROM PN_InternalVersion_Definition WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
		IF idMechanicalVersion IS NULL THEN
			SELECT idMechanical_WindchillCode_Housing INTO idMechanicalWindchill FROM Mechanical_WindchillCode_Housing
			WHERE Code = windchill AND idPlataforma = idSavedPlataform;
			IF idMechanicalWindchill IS NULL THEN
				SELECT MAX(idMechanical_WindchillCode_Housing) + 1 INTO idMechanicalWindchill FROM Mechanical_WindchillCode_Housing;
				INSERT INTO Mechanical_WindchillCode_Housing  VALUES (idMechanicalWindchill, windchill, idSavedPlataform);
                
				INSERT INTO Mechanical_Version (idWindchillCode, LB) VALUES (idMechanicalWindchill, lbCode);
                
				UPDATE PN_InternalVersion_Definition SET idMechanical_Version = LAST_INSERT_ID()
                WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
			ELSE 
				SELECT idMechanical_Version INTO idMechanicalVersion FROM Mechanical_Version
                WHERE idWindchillCode = idMechanicalWindchill AND LB = lbCode;
                
                IF idMechanicalVersion IS NULL THEN
					INSERT INTO Mechanical_Version (idWindchillCode, LB) VALUES (idMechanicalWindchill, lbCode);
					
					UPDATE PN_InternalVersion_Definition SET idMechanical_Version = LAST_INSERT_ID()
					WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
				ELSE
					UPDATE PN_InternalVersion_Definition SET idMechanical_Version = idMechanicalVersion
					WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
				END IF;
			END IF;
		ELSEIF (SELECT IFNULL(getIdMechanicalVersion(idSavedPlataform, windchill, lbCode),0)) != idMechanicalVersion THEN
			SIGNAL SQLSTATE '10024'SET MESSAGE_TEXT = 'Bloque de mecánica anterior no corresponde al actual.';
		END IF;
	END IF;
END;;

call InsertOrCheckMechanicalVersion(113, 'AMG11KW',  
								'2017176A00', 
								'3');

SELECT * FROM PN_InternalVersion_Definition;
SELECT * FROM Mechanical_Version;
SELECT * FROM Mechanical_WindchillCode_Housing;
                                
SELECT idMechanical_Version FROM PN_InternalVersion_Definition -- WHERE idPN_InternalVersion_Definition = 12;
SELECT IFNULL(SELECT getIdMechanicalVersion(1, 'HHH', 'LB'), 8);
