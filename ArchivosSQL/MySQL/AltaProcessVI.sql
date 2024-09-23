
-- Elimina procedimiento anterior
DROP PROCEDURE IF EXISTS InsertOrCheckProcess;
DELIMITER ;;
CREATE PROCEDURE InsertOrCheckProcess (
								idInternalVersionDef INT(11), 
								processName VARCHAR(150),
                                trazability INT(11))
BEGIN
-- Precondiciones: El idInternalVersionDef recibido deberá existir en la bbdd.
-- Postcondiciones: Los errores generados por el procedimiento corresponden a las diferentes casuisticas que se pueden dar y deben ser tratados en el exterior individualmente.

	DECLARE idProcessVersion INT(11);
-- 	DECLARE exit handler for SQLEXCEPTION
--     BEGIN
-- 		SHOW ERRORS;
-- 		ROLLBACK;
--     END;
--     
--     START TRANSACTION;
    
    SELECT idVersion_Procesos_Manufacturing INTO idProcessVersion FROM PN_InternalVersion_Definition
		WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
        
    IF idProcessVersion IS NULL AND processName IS NOT NULL THEN
		SELECT idVersion_Procesos_Manufacturing INTO idProcessVersion FROM Version_Procesos_Manufacturing
		WHERE Nombre = processName;
		IF idProcessVersion IS NULL THEN
			-- INSERT INTO Version_Procesos_Manufacturing (Nombre) VALUES (processName);
-- 			UPDATE PN_InternalVersion_Definition SET idVersion_Procesos_Manufacturing = LAST_INSERT_ID()
-- 			WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
            
			SIGNAL SQLSTATE '10027' SET MESSAGE_TEXT = 'AG no registrado, contacte con el Ingerniero de Procesos.';
		ELSE
			UPDATE PN_InternalVersion_Definition SET idVersion_Procesos_Manufacturing = idProcessVersion, Trazability = trazability
			WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
		END IF;
	ELSEIF (SELECT IFNULL(
				(SELECT idVersion_Procesos_Manufacturing FROM Version_Procesos_Manufacturing
					WHERE Nombre = processName)
				,0))!= idProcessVersion THEN
		SIGNAL SQLSTATE '10028' SET MESSAGE_TEXT = 'El proceso anterior (en Base de Datos) no corresponde al actual (Excel).';
    END IF;
END;;

CALL InsertOrCheckProcess (130, 'APP Manufacturing_OBCAMG11_B3.1.9_Rev0', 1);
SELECT * FROM PN_InternalVersion_Definition;
SELECT * FROM Version_Procesos_Manufacturing;
delete from Version_Procesos_Manufacturing where idVersion_Procesos_Manufacturing = 92;