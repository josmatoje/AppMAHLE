
SELECT * FROM PN_InternalVersion_Definition;
SELECT * FROM Internal_Version;
SELECT * FROM Part_Number_DEF;
SELECT * FROM Hardware_Version;
SELECT * FROM Mechanical_Version;
SELECT * FROM Software_Version;
SELECT * FROM Version_Procesos_Manufacturing;
SELECT * FROM Labeling_Version_Def;
SELECT * FROM Testing_Version;
SELECT * FROM Procesos;

-- Elimina procedimiento anterior
DROP PROCEDURE IF EXISTS InsertOrGetVersionInterna;
DELIMITER ;;
CREATE PROCEDURE InsertOrGetVersionInterna (
								plataforma VARCHAR(150), 
								isDummy INT(11), 
								sampleCode VARCHAR(45), 
								partNumber VARCHAR(150), 
								internalVersion VARCHAR(45), 
								registerDateString VARCHAR(45), 
                                out newId INT(11))
BEGIN
	DECLARE registerDate DATE;
	DECLARE idPlataform INT(11);
	DECLARE idSample INT(11);
	DECLARE idPartNumber INT(11);
	DECLARE idInternalversion INT(11);
	DECLARE exit handler for SQLEXCEPTION
    BEGIN
		RESIGNAL;
		ROLLBACK;
    END;
    
    START TRANSACTION;
    IF registerDateString IS NOT NULL THEN
		SET registerDate = STR_TO_DATE(registerDateString, '%d/%m/%Y');
	END IF;
    
    -- Comprobación de existencia de plataforma
    SELECT idPlataforma INTO idPlataform FROM Plataforma WHERE Nombre_Plataforma = plataforma LIMIT 1;
    IF idPlataform IS NULL THEN
		SIGNAL SQLSTATE '10022' SET MESSAGE_TEXT = 'Plataforma no encontrada';
    END IF;
    
    -- Comprobación de sample
    SELECT idSample_Proyect INTO idSample FROM Sample_Proyect WHERE Sample = sampleCode;
    IF idSample IS NULL THEN
		INSERT INTO Sample_Proyect (Sample) VALUES (sampleCode);
        SET idSample = LAST_INSERT_ID();
    END IF;
    -- Comprobación de part number
    SELECT idPart_Number_DEF INTO idPartNumber FROM Part_Number_DEF WHERE Nombre_Part_Number = partNumber AND idPlataforma = idPlataform AND idSample_Proyect = idSample;
    IF idPartNumber IS NULL THEN
		INSERT INTO Part_Number_DEF (Nombre_Part_Number, idPlataforma, idSample_Proyect) VALUES (partNumber, idPlataform, idSample);
        SET idPartNumber = LAST_INSERT_ID();
    END IF; 
    
    -- Comprobación de internal version
    SELECT idInternal_Version INTO idInternalversion FROM Internal_Version WHERE Internal_Version = internalVersion AND IdPart_Number_DEF = idPartNumber;
    IF idInternalversion IS NULL THEN
		INSERT INTO Internal_Version (Internal_Version, IdPart_Number_DEF) VALUES (internalVersion, idPartNumber);
        SET idInternalversion = LAST_INSERT_ID();
    END IF;
    
    -- Comprobación de internal version definition
    IF NOT EXISTS (SELECT idPN_InternalVersion_Definition FROM PN_InternalVersion_Definition WHERE idInternal_Version = idInternalversion AND Dummie = isDummy) THEN
		IF EXISTS (SELECT idPN_InternalVersion_Definition FROM PN_InternalVersion_Definition WHERE idInternal_Version = idInternalversion) THEN
			IF (SELECT `Registration Date` FROM PN_InternalVersion_Definition WHERE idInternal_Version = idInternalversion) IS NULL THEN
				UPDATE PN_InternalVersion_Definition SET `Registration Date` = registerDate;
                SELECT idPN_InternalVersion_Definition INTO newId FROM PN_InternalVersion_Definition WHERE idInternal_Version = idInternalversion AND Dummie = isDummy;
            ELSE
				SIGNAL SQLSTATE '10023' SET MESSAGE_TEXT = 'Versión interna ya definida.';
			END IF;
        ELSE
			INSERT INTO PN_InternalVersion_Definition (idInternal_Version, Dummie, `Registration Date`) VALUES (idInternalversion, isDummy, registerDate);
			SET newId = LAST_INSERT_ID();
        END IF;
	ELSE 
		IF (SELECT `Registration Date` FROM PN_InternalVersion_Definition WHERE idInternal_Version = idInternalversion) IS NULL THEN
				UPDATE PN_InternalVersion_Definition SET `Registration Date` = registerDate;
		END IF;
		SELECT idPN_InternalVersion_Definition INTO newId FROM PN_InternalVersion_Definition WHERE idInternal_Version = idInternalversion AND Dummie = isDummy;
    END IF;
	COMMIT;
END;;

CALL InsertOrGetVersionInterna('AMG11KW', 'B3', '01OBCAMG11/1-400/B3.0', 'B4.0.5', '20/03/2024', @newID);
select @newID;

SELECT idPart_Number_DEF FROM Part_Number_DEF WHERE Nombre_Part_Number = 'B3.prueba' AND idPlataforma = 1;
SELECT * FROM Part_Number_DEF
SELECT * FROM Internal_Version WHERE Internal_Version = 'B3.0.3' AND IdPart_Number_DEF = 6;
select date(STR_TO_DATE('20/03/2024', '%d/%m/%Y'));
SELECT * FROM PN_InternalVersion_Definition  WHERE idInternal_Version = 16 AND `Registration Date` = STR_TO_DATE('20/03/2024', '%d/%m/%Y');
 SELECT IFNULL(idPN_InternalVersion_Definition, 0) FROM PN_InternalVersion_Definition WHERE idInternal_Version = 18 AND `Registration Date` = STR_TO_DATE('20/03/2025', '%d/%m/%Y') LIMIT 1;

SELECT * FROM Part_Number_DEF;
SELECT * FROM Internal_Version;
SELECT * FROM PN_InternalVersion_Definition;

select * from Hardware_Version hw
inner join Version_Parameters_HW_PCBs vp on hw.idVersion_Parameters_HW_PCBs = vp.idVersion_Parameters_HW_PCBs
inner join Version_Hardware_PCBs vh on vp.idVersion_Hardware_PCBs = vh.idVersion_Hardware_PCBs
inner join References_PCBs rp on vh.idReferences_PCBs = rp.idReferences_PCBs

-- Elimina procedimiento anterior
DROP PROCEDURE IF EXISTS AltaVersionInterna;
DELIMITER ;;
CREATE PROCEDURE AltaVersionInterna (
								idInternalVersionDef INT(11),
                                
                                eHW1 VARCHAR(75), 
								eHW2 VARCHAR(75), 
								eHW3 VARCHAR(75), 
								eHW4 VARCHAR(75), 
								eHW5 VARCHAR(75), 
								eHW6 VARCHAR(75), 
								eHW7 VARCHAR(75), 
								eHW8 VARCHAR(75),
                                
								plataforma VARCHAR(150), 
								windchill VARCHAR(45), 
								lb VARCHAR(45),
                                
								hw_snr VARCHAR(45), 
                                hw_status VARCHAR(45), 
                                sw_snr VARCHAR(45),
                                sw_status VARCHAR(45),
                                releaseTest VARCHAR(45),
                                releaseFinal VARCHAR(45),
                                
								processName VARCHAR(150),
								trazability INT(11),
                                
                                snrValue varchar(45),
								zgsValue varchar(45),
								e_q varchar(45),
								luStatus varchar(45),
								prodLoc varchar(45),
								daiPlant varchar(45),
								br_bl varchar(45),
								lftNo varchar(45),
								p_n varchar(45),
                                
                                test1 VARCHAR(75), 
								test2 VARCHAR(75), 
								test3 VARCHAR(75), 
								test4 VARCHAR(75), 
								test5 VARCHAR(75), 
								test6 VARCHAR(75), 
								test7 VARCHAR(75), 
								test8 VARCHAR(75)
                                )
BEGIN
-- Precondiciones: El idInternalVersionDef recibido deberá existir en la bbdd.
	DECLARE EnableDefinition INT(11);
	DECLARE exit handler for SQLEXCEPTION
    BEGIN
		ROLLBACK;
        RESIGNAL;
    END;
    
    START TRANSACTION;
    IF eHW1 IS NOT NULL THEN
		CALL InsertOrCheckElectronicHardware (
								idInternalVersionDef,
								eHW1, 
								eHW2, 
								eHW3, 
								eHW4, 
								eHW5, 
								eHW6, 
								eHW7, 
								eHW8);
	END IF;
    IF windchill IS NOT NULL THEN
		CALL InsertOrCheckMechanicalVersion(
								idInternalVersionDef,
								plataforma,  
								windchill, 
								lb);
	END IF;
    IF hw_snr IS NOT NULL THEN
		CALL InsertOrCheckSoftware (
								idInternalVersionDef,
								hw_snr, 
								hw_status, 
								sw_snr,
								sw_status,
								releaseTest,
								releaseFinal);
	END IF;
    IF processName IS NOT NULL THEN 
		CALL InsertOrCheckProcess(
								idInternalVersionDef,
								processName,
								trazability);
	END IF;
    IF snrValue IS NOT NULL THEN
		CALL InsertOrCheckLabelling (
								idInternalVersionDef, 
								snrValue,
								zgsValue,
								e_q,
								luStatus,
								prodLoc,
								hw_snr, 
								hw_status, 
								sw_snr,
								sw_status,
								daiPlant,
								br_bl,
								lftNo,
								p_n);
	END IF;
    IF test1 IS NOT NULL THEN
		CALL InsertOrCheckTesting (
								idInternalVersionDef, 
                                test1, 
								test2, 
								test3, 
								test4, 
								test5, 
								test6, 
								test7, 
								test8);
	END IF;
	
    IF (SELECT `Registration Date` FROM PN_InternalVersion_Definition WHERE idPN_InternalVersion_Definition = idInternalVersionDef) IS NOT NULL AND
		eHW1 IS NOT NULL AND windchill IS NOT NULL AND hw_snr IS NOT NULL AND processName IS NOT NULL AND snrValue IS NOT NULL AND test1 IS NOT NULL THEN
        SET EnableDefinition = 1;
	ELSE
		SET EnableDefinition = 0;
	END IF;
    
    UPDATE PN_InternalVersion_Definition SET Enable = EnableDefinition 
		WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
    
	COMMIT;
END;;


CALL InsertOrGetVersionInterna('AMG11KW', 'B3', '01OBCAMG11/1-400/B3.0', 'B3.0.4', STR_TO_DATE('1/04/2024', '%d/%m/%Y'), @newID);
select @newID;

CALL AltaVersionInterna (
								103,
                                NULL, 
                                'LG-1854-8.0.1', 
                                'LG-1835-7.2.3', 
                                'LG-1845-11.1.0', 
                                NULL, 
                                'LG-1860-6.0.0', 
                                'LG-1950-3.0.0', 
                                NULL,
                                
								'AMG11KW',  
								'2017934A00', 
								'LB_B3.1.0 Ed02',
                                
								'A5909014500', 
                                '24/12.00', 
                                'A5909023400',
                                '24/08.03',
                                'R9.0.4',
                                'R9.0.4',
                                
								'APP ManurBGHHGing_OMG11_B3.1.4_Rev0',
                                
                                'A5909006000',
								'001',
								'E004',
								'L004',
								'Mae in Spain',
								'601',
								'D5599',
								'15538010',
								'01041596',
                                
                                'FCT Control', 
                                'FCT In-Filter',
                                'FCT In-Filter',
                                'Passthrough',
                                'EOL1',
                                'HiPot', 
                                NULL, 
                                NULL
                                )