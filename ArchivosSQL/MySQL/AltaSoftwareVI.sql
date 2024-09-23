
-- Elimina procedimiento anterior
DROP PROCEDURE IF EXISTS InsertOrCheckSoftware;
DELIMITER ;;
CREATE PROCEDURE InsertOrCheckSoftware (
								idInternalVersionDef INT(11), 
								hw_snr VARCHAR(45) COLLATE utf8_spanish2_ci, 
                                hw_status VARCHAR(45) COLLATE utf8_spanish2_ci, 
                                sw_snr VARCHAR(45) COLLATE utf8_spanish2_ci,
                                sw_status VARCHAR(45) COLLATE utf8_spanish2_ci,
                                releaseTest VARCHAR(45) COLLATE utf8_spanish2_ci,
                                releaseFinal VARCHAR(45) COLLATE utf8_spanish2_ci)
BEGIN
-- Precondiciones: El idInternalVersionDef recibido deberá existir en la bbdd.
-- Postcondiciones: Los errores generados por el procedimiento corresponden a las diferentes casuisticas que se pueden dar y deben ser tratados en el exterior individualmente.

	DECLARE idSoftWareVersion INT(11);
	DECLARE exit handler for SQLEXCEPTION
    BEGIN
        RESIGNAL;
		ROLLBACK;
    END;
--     
--     START TRANSACTION;
    
    SELECT idSoftware_Version INTO idSoftWareVersion FROM PN_InternalVersion_Definition 
		WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
    IF idSoftWareVersion IS NULL THEN
		SELECT idSoftware_Version INTO idSoftWareVersion FROM Software_Version
				WHERE `HW-SNR` = hw_snr AND `HW status` = hw_status AND `SW-SNR` = sw_snr AND `SW status` = sw_status AND `Release_Test` = releaseTest AND `Release_Final` = releaseFinal
				LIMIT 1;
		IF idSoftWareVersion IS NULL THEN
			INSERT INTO Software_Version (`HW-SNR`, `HW status`, `SW-SNR`, `SW status`, `Release_Test`, `Release_Final`) VALUES (hw_snr, hw_status, sw_snr, sw_status, releaseTest, releaseFinal);
			UPDATE PN_InternalVersion_Definition SET idSoftware_Version = LAST_INSERT_ID()
			WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
		ELSE 
			UPDATE PN_InternalVersion_Definition SET idSoftware_Version = idSoftWareVersion
			WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
		END IF;
	ELSEIF (SELECT IFNULL(
				(SELECT idSoftware_Version FROM Software_Version
					WHERE `HW-SNR` = hw_snr AND `HW status` = hw_status AND `SW-SNR` = sw_snr AND `SW status` = sw_status AND `Release_Test` = releaseTest AND `Release_Final` = releaseFinal
					LIMIT 1)
                ,0))!= idSoftWareVersion THEN
		SIGNAL SQLSTATE '10025' SET MESSAGE_TEXT = 'Bloque de software anterior (en Base de Datos) no corresponde al actual (Excel).';
    END IF;
END;;

CALL InsertOrCheckSoftware (111, 
								'A5909014500', 
                                '24/12.00', 
                                'A590fgl9023400',
                                '24/08.03',
                                'R9.0.4',
                                'R9.0.4');
SELECT * FROM PN_InternalVersion_Definition;
SELECT * FROM Software_Version;

    SELECT idSoftware_Version  FROM PN_InternalVersion_Definition 
		WHERE idPN_InternalVersion_Definition = 99;
        
SELECT IFNULL(
	(SELECT idSoftware_Version FROM Software_Version
 				WHERE `HW-SNR` = 'A5909014500' AND 
                `HW status` = '24/12.00' AND 
                `SW-SNR` = 'A5909023400' AND 
                `SW status` = '24/08.03'  AND 
                `Release_Test` = 'R9.U7.4' AND 
                `Release_Final` = 'R9.0.4'
                LIMIT 1)
,0)