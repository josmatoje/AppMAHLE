-- Elimina procedimiento anterior
DROP FUNCTION IF EXISTS InsertOrCheckVersionHardware_On_Parameter;
DELIMITER ;;
CREATE FUNCTION InsertOrCheckVersionHardware_On_Parameter (
						PCBReference VARCHAR(45),
						ParameterComponent INT(11),
						ParamValue INT(11))
RETURNS INT(11)
BEGIN
-- Postcondiciones: Los errores generados por el procedimiento corresponden a las diferentes casuisticas que se pueden dar y deben ser tratados en el exterior individualmente.
	DECLARE idVersionParam INT(11);
    
	SELECT idVersion_Hardware_PCBs INTO idVersionParam FROM  Version_Hardware_PCBs VH 
	INNER JOIN References_PCBs RP ON VH.idReferences_PCBs = RP.idReferences_PCBs
	WHERE RP.Reference = PCBReference AND (VH.idParameters_Components = ParameterComponent AND VH.Value_Parameter = ParamValue)
    LIMIT 1;
	
    IF idVersionParam IS NULL THEN
		INSERT INTO Version_Hardware_PCBs (idReferences_PCBs, idParameters_Components, Value_Parameter) VALUES ((SELECT idReferences_PCBs FROM References_PCBs WHERE Reference = PCBReference),ParameterComponent, ParamValue);
		SET idVersionParam = LAST_INSERT_ID();
	END IF;
    
	RETURN idVersionParam;
END;;

SELECT InsertOrCheckVersionHardware_On_Parameter('1826', 2, 1);

SELECT * FROM  Version_Hardware_PCBs VH 
INNER JOIN References_PCBs RP ON VH.idReferences_PCBs = RP.idReferences_PCBs
WHERE RP.Reference = 1826 AND 
	(VH.idParameters_Components = 1 AND VH.Value_Parameter = 8) OR
	(VH.idParameters_Components = 2 AND VH.Value_Parameter = 1) OR
	(VH.idParameters_Components = 3 AND VH.Value_Parameter = 1);

-- Elimina funcióm anterior
DROP FUNCTION IF EXISTS InsertOrCheckVersionParameterHardware;
DELIMITER ;;
CREATE FUNCTION InsertOrCheckVersionParameterHardware (
								PCBReference VARCHAR(45),
                                Layout INT(11),
                                BOM INT(11),
                                Rework INT(11))
RETURNS INT(11)
BEGIN
	DECLARE idVersionParameter INT(11);
	DECLARE idLayout INT(11);
	DECLARE idBOM INT(11);
	DECLARE idRework INT(11);
    
    SELECT idVersion_Parameters_HW_PCBs INTO idVersionParameter FROM (
		SELECT idVersion_Parameters_HW_PCBs, COUNT(*) NumFil FROM Version_Parameters_HW_PCBs VP
		INNER JOIN Version_Hardware_PCBs VH ON VP.idVersion_Hardware_PCBs=VH.idVersion_Hardware_PCBs
		INNER JOIN References_PCBs RP ON VH.idReferences_PCBs = RP.idReferences_PCBs
		WHERE RP.Reference = PCBReference AND (
			(VH.idParameters_Components = 1 AND VH.Value_Parameter = Layout) OR
			(VH.idParameters_Components = 2 AND VH.Value_Parameter = BOM) OR
			(VH.idParameters_Components = 3 AND VH.Value_Parameter = Rework))
		GROUP BY idVersion_Parameters_HW_PCBs
		HAVING NumFil = 3
		LIMIT 1 ) VCONT;
        
	IF idVersionParameter IS NULL THEN
		SELECT MAX(idVersion_Parameters_HW_PCBs)+1 INTO idVersionParameter FROM Version_Parameters_HW_PCBs;
		INSERT INTO Version_Parameters_HW_PCBs VALUES (idVersionParameter, (SELECT InsertOrCheckVersionHardware_On_Parameter(PCBReference, 1, Layout)));
		INSERT INTO Version_Parameters_HW_PCBs VALUES (idVersionParameter, (SELECT InsertOrCheckVersionHardware_On_Parameter(PCBReference, 2, BOM)));
		INSERT INTO Version_Parameters_HW_PCBs VALUES (idVersionParameter, (SELECT InsertOrCheckVersionHardware_On_Parameter(PCBReference, 3, Rework)));
	END IF;

	RETURN idVersionParameter;
END;;

SELECT InsertOrCheckVersionParameterHardware('1826', 9, 1, 2);

SELECT idVersion_Parameters_HW_PCBs, COUNT(*) NumFil FROM Version_Parameters_HW_PCBs VP
-- SELECT * FROM Version_Parameters_HW_PCBs VP
INNER JOIN Version_Hardware_PCBs VH ON VP.idVersion_Hardware_PCBs=VH.idVersion_Hardware_PCBs
INNER JOIN References_PCBs RP ON VH.idReferences_PCBs = RP.idReferences_PCBs
WHERE RP.Reference = 1826 AND (
	(VH.idParameters_Components = 1 AND VH.Value_Parameter = 8) OR
	(VH.idParameters_Components = 2 AND VH.Value_Parameter = 1) OR
	(VH.idParameters_Components = 3 AND VH.Value_Parameter = 1))
GROUP BY idVersion_Parameters_HW_PCBs
HAVING NumFil = 3;

-- Elimina funcióm anterior
DROP FUNCTION IF EXISTS GetIdHardware;
DELIMITER ;;
CREATE FUNCTION GetIdHardware (
								hardw1 VARCHAR(75), 
								hardw2 VARCHAR(75), 
								hardw3 VARCHAR(75), 
								hardw4 VARCHAR(75), 
								hardw5 VARCHAR(75), 
								hardw6 VARCHAR(75), 
								hardw7 VARCHAR(75), 
								hardw8 VARCHAR(75))
RETURNS INT(11)
BEGIN
	-- Precondiciones: El idInternalVersionDef recibido deberá existir en la bbdd. Los hardware deberán seguir el formato LB-REFF-X.Y.Z (Ej: LB-1234-1.2.3).
-- 						eHW1 debe estar siempre relleno. Los hardware que no se rellenen deberán de pasarse como nulo. Si se introducen varios hardwares iguales se contabilizarán como el mismo hardware.
	DECLARE idVersionHardware INT(11);

	SELECT ID INTO idVersionHardware FROM (
		SELECT idHardware_Version ID, COUNT(*) numOfHardware FROM Hardware_Version HV
		-- SELECT * FROM Hardware_Version HV
		WHERE idVersion_Parameters_HW_PCBs IN (SELECT idVersion_Parameters_HW_PCBs FROM HardwarePCBReferenceLayoutBom
													WHERE HardwareReference LIKE hardw1 OR
														HardwareReference LIKE IFNULL(hardw2, hardw1) OR
														HardwareReference LIKE IFNULL(hardw3, hardw1) OR
														HardwareReference LIKE IFNULL(hardw4, hardw1) OR
														HardwareReference LIKE IFNULL(hardw5, hardw1) OR
														HardwareReference LIKE IFNULL(hardw6, hardw1) OR
														HardwareReference LIKE IFNULL(hardw7, hardw1) OR
														HardwareReference LIKE IFNULL(hardw8, hardw1)
												)
		GROUP BY ID
		) INTER
	WHERE numOfHardware = (SELECT COUNT(*) FROM (
						SELECT hardw1
						UNION 
						SELECT IFNULL(hardw2, hardw1)
						UNION 
						SELECT IFNULL(hardw3, hardw1)
						UNION 
						SELECT IFNULL(hardw4, hardw1)
						UNION 
						SELECT IFNULL(hardw5, hardw1)
						UNION 
						SELECT IFNULL(hardw6, hardw1)
						UNION 
						SELECT IFNULL(hardw7, hardw1)
						UNION 
						SELECT IFNULL(hardw8, hardw1))NUM)
	LIMIT 1;

	RETURN idVersionHardware;
END;;

SELECT GetIdHardware('LG-1826-9.1.2', 
					'LG-1854-9.1.1', 
					'LG-1835-8.1.0', 
					'LG-1845-12.1.0', 
					'LG-1855-4.0.0', 
					'LG-1860-6.0.0', 
					'LG-1950-3.0.0', 
					NULL)

SELECT * FROM (
		SELECT idHardware_Version ID, COUNT(*) numOfHardware FROM Hardware_Version HV
		-- SELECT * FROM Hardware_Version HV
		WHERE idVersion_Parameters_HW_PCBs IN (SELECT idVersion_Parameters_HW_PCBs FROM HardwarePCBReferenceLayoutBom
													WHERE HardwareReference LIKE 'LG-1826-8.1.1' OR
														HardwareReference LIKE IFNULL('LG-1854-8.0.1', 'LG-1826-8.1.1') OR
														HardwareReference LIKE IFNULL( 'LG-1835-7.2.0', 'LG-1826-8.1.1') OR
														HardwareReference LIKE IFNULL('LG-1845-11.1.0', 'LG-1826-8.1.1') OR
														HardwareReference LIKE IFNULL('LG-1855-4.0.0', 'LG-1826-8.1.1') OR
														HardwareReference LIKE IFNULL('LG-1860-6.0.0', 'LG-1826-8.1.1') OR
														HardwareReference LIKE IFNULL('LG-1950-3.0.0', 'LG-1826-8.1.1') OR
														HardwareReference LIKE IFNULL(NULL, 'LG-1826-8.1.1')
												)
		GROUP BY ID
		) INTER
	WHERE numOfHardware = (SELECT COUNT(*) FROM (
						SELECT 'LG-1826-8.1.1'
						UNION 
						SELECT IFNULL('LG-1854-8.0.1', 'LG-1826-8.1.1')
						UNION 
						SELECT IFNULL( 'LG-1835-7.2.0', 'LG-1826-8.1.1')
						UNION 
						SELECT IFNULL('LG-1845-11.1.0', 'LG-1826-8.1.1')
						UNION 
						SELECT IFNULL('LG-1855-4.0.0', 'LG-1826-8.1.1')
						UNION 
						SELECT IFNULL('LG-1860-6.0.0', 'LG-1826-8.1.1')
						UNION 
						SELECT IFNULL('LG-1950-3.0.0', 'LG-1826-8.1.1')
						UNION 
						SELECT IFNULL(NULL, 'LG-1826-8.1.1'))NUM);

SELECT * FROM HardwarePCBReferenceLayoutBom;

-- Elimina funcióm anterior
DROP PROCEDURE IF EXISTS InsertIndividualHardware;
DELIMITER ;;
CREATE PROCEDURE InsertIndividualHardware (
											idHardwareVersion INT(11),
											hardw VARCHAR(75))
BEGIN
	-- Precondiciones: El idInternalVersionDef recibido deberá existir en la bbdd. Los hardware deberán seguir el formato LB-REFF-X.Y.Z (Ej: LB-1234-1.2.3).
	INSERT INTO Hardware_Version VALUES (idHardwareVersion, 
											(SELECT InsertOrCheckVersionParameterHardware(
												(SELECT SUBSTRING_INDEX(SUBSTRING_INDEX(hardw, "-", -2), "-", 1)),
												(SELECT SUBSTRING_INDEX(SUBSTRING_INDEX(hardw, "-", -1), ".", 1)),
												(SELECT SUBSTRING_INDEX(SUBSTRING_INDEX(SUBSTRING_INDEX(hardw, "-", -1), ".", -2), ".", 1)),
												(SELECT SUBSTRING_INDEX(SUBSTRING_INDEX(hardw, "-", -1), ".", -1)))
											)
										);
END;;

-- Elimina procedimiento anterior
DROP PROCEDURE IF EXISTS InsertOrCheckElectronicHardware;
DELIMITER ;;
CREATE PROCEDURE InsertOrCheckElectronicHardware (
								idInternalVersionDef INT(11), 
								eHW1 VARCHAR(75), 
								eHW2 VARCHAR(75), 
								eHW3 VARCHAR(75), 
								eHW4 VARCHAR(75), 
								eHW5 VARCHAR(75), 
								eHW6 VARCHAR(75), 
								eHW7 VARCHAR(75), 
								eHW8 VARCHAR(75))
BEGIN
-- Precondiciones: El idInternalVersionDef recibido deberá existir en la bbdd. Los hardware deberán seguir el formato LB-REFF-X.Y.Z (Ej: LB-1234-1.2.3).
-- 					eHW1 debe estar siempre relleno. Los hardware que no se rellenen deberán de pasarse como nulo.
-- Postcondiciones: Los errores generados por el procedimiento corresponden a las diferentes casuisticas que se pueden dar y deben ser tratados en el exterior individualmente.

	DECLARE idHardwareVersion INT(11);
 	DECLARE exit handler for SQLEXCEPTION
		BEGIN
        RESIGNAL;
 		ROLLBACK;
     END;
     
     -- START TRANSACTION;
    
    SELECT idHardware_Version INTO idHardwareVersion FROM PN_InternalVersion_Definition 
		WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
        
    IF idHardwareVersion IS NULL THEN
		SELECT GetIdHardware(eHW1, eHW2, eHW3, eHW4, eHW5, eHW6, eHW7, eHW8) INTO idHardwareVersion;
		IF idHardwareVersion IS NULL THEN -- Añadimos un nuevo bloque de hardware para vesion interna
			SELECT MAX(idHardware_Version)+1 INTO idHardwareVersion FROM Hardware_Version;
            
			CALL InsertIndividualHardware(idHardwareVersion, eHW1);
            
			IF eHW2 IS NOT NULL THEN
				CALL InsertIndividualHardware(idHardwareVersion, eHW2);
            END IF;
			IF eHW3 IS NOT NULL THEN
				CALL InsertIndividualHardware(idHardwareVersion, eHW3);
            END IF;
			IF eHW4 IS NOT NULL THEN
				CALL InsertIndividualHardware(idHardwareVersion, eHW4);
            END IF;
			IF eHW5 IS NOT NULL THEN
				CALL InsertIndividualHardware(idHardwareVersion, eHW5);
            END IF;
			IF eHW6 IS NOT NULL THEN
				CALL InsertIndividualHardware(idHardwareVersion, eHW6);
            END IF;
			IF eHW7 IS NOT NULL THEN
				CALL InsertIndividualHardware(idHardwareVersion, eHW7);
            END IF;
			IF eHW8 IS NOT NULL THEN
				CALL InsertIndividualHardware(idHardwareVersion, eHW8);
            END IF;
        END IF;
        
		UPDATE PN_InternalVersion_Definition SET idHardware_Version = idHardwareVersion 
		WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
	ELSEIF (SELECT IFNULL(GetIdHardware(eHW1, eHW2, eHW3, eHW4, eHW5, eHW6, eHW7, eHW8), 0)) != idHardwareVersion THEN
		SIGNAL SQLSTATE '10023' SET MESSAGE_TEXT = 'Bloque de hardwares no corresponde';
    END IF;
END;;

CALL InsertOrCheckElectronicHardware(121,
									'LG-1826-8.1.1', 
									'LG-1854-8.0.1', 
									'LG-1835-7.2.3', 
									'LG-1845-11.1.0', 
									'LG-1855-4.0.0', 
									'LG-1860-6.0.0', 
									'LG-1950-3.0.0', 
									NULL);
SELECT * FROM Hardware_Version HV
INNER JOIN HardwarePCBReferenceLayoutBom HR ON HV.idVersion_Parameters_HW_PCBs = HR.idVersion_Parameters_HW_PCBs;

select * from References_PCBs;
select * from Hardware_Version H
inner join HardwarePCBReferenceLayoutBom HPCB ON H.idVersion_Parameters_HW_PCBs = HPCB.idVersion_Parameters_HW_PCBs;

SELECT SUBSTRING_INDEX(SUBSTRING_INDEX("LG-1826-8.6.0", "-", -2), "-", 1);
SELECT SUBSTRING_INDEX(SUBSTRING_INDEX("LG-1826-8.6.0", "-", -1), ".", 1);
SELECT SUBSTRING_INDEX(SUBSTRING_INDEX(SUBSTRING_INDEX("LG-1826-8.6.0", "-", -1), ".", -2), ".", 1);
SELECT SUBSTRING_INDEX(SUBSTRING_INDEX("LG-1826-8.6.0", "-", -1), ".", -1);
-- Inserción de nuevo grupo de parámetros
insert into Version_Parameters_HW_PCBs values (13,22);
insert into Version_Parameters_HW_PCBs values (13,23);
insert into Version_Parameters_HW_PCBs values (13,47);

-- Inserción de nuevo grupo de hardwares
insert into Hardware_Version values (16,15);
insert into Hardware_Version values (16,16);
insert into Hardware_Version values (16,17);
insert into Hardware_Version values (16,18);
insert into Hardware_Version values (16,5);
insert into Hardware_Version values (16,6);
insert into Hardware_Version values (16,7);
CALL InsertIndividualHardware(17, 'LG-1950-3.0.0');

SELECT InsertOrCheckVersionParameterHardware('1950', 3, 0, 0);

-- PRUEBAS TRANSACCIONES
DELETE FROM Hardware_Version WHERE idHardware_Version = 20 -- AND idVersion_Parameters_HW_PCBs = 7;
insert into Hardware_Version values (19,5);
insert into Hardware_Version values (19,6);
insert into Hardware_Version values (19,7);
SELECT * FROM Hardware_Version;
