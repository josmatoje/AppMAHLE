-- Elimina procedimiento anterior
DROP PROCEDURE IF EXISTS InsertarAltaPCB;
DELIMITER ;;
CREATE PROCEDURE InsertarAltaPCB (
								ordenadorName VARCHAR(75), 
								snNumber VARCHAR(75), 
								idReference INT(11), 
								layout INT(11), 
								bom INT(11), 
								lote VARCHAR(250))
BEGIN
	DECLARE idSNRegister INT(11);
    DECLARE idRolOrdenador INT(11);
	DECLARE now_date DATETIME;
    DECLARE pcbStatus INT(11);
    
    
    SELECT idRoles_Ordenadores INTO idRolOrdenador FROM Roles_Ordenadores WHERE INSTR(ordenadorName, Ordenador) != 0
    LIMIT 1;
    SET now_date = current_timestamp();
    
    IF idReference = 23 OR idReference = 24 OR idReference = 25 THEN
		SET pcbStatus = 0;
	ELSE 
		SET pcbStatus = 1;
	END IF;
    
    INSERT INTO SN_Register_PCBs(idReferences_PCBs, SN, Date, Status, Lote) VALUES(idReference, snNumber, now_date, pcbStatus, lote);
    SELECT idSN_Register_PCBs INTO idSNRegister FROM SN_Register_PCBs WHERE Date = now_date;
    INSERT INTO Components_Register_PCB(idRolesOrdenadores, idSN_Register_PCBs, idParameters_Components, Value, Date) VALUES (idRolOrdenador, idSNRegister, 1, layout, now_date);
    INSERT INTO Components_Register_PCB(idRolesOrdenadores, idSN_Register_PCBs, idParameters_Components, Value, Date) VALUES (idRolOrdenador, idSNRegister, 2, bom, now_date);
    INSERT INTO Components_Register_PCB(idRolesOrdenadores, idSN_Register_PCBs, idParameters_Components, Value, Date) VALUES (idRolOrdenador, idSNRegister, 3, 0, now_date);

END;;


    DECLARE idRolOrdenador INT(11);
    SELECT idRoles_Ordenadores FROM Roles_Ordenadores WHERE Ordenador = 'M0176830';
    SELECT * FROM Roles_Ordenadores WHERE Ordenador = 'M0176830';

CALL InsertarAltaPCB('ITC//M0176830', 'abcPRUEBA203', 23, 26, 14, 'PRUEBA');

SELECT * FROM Roles_Ordenadores WHERE Ordenador = 'M0176830'
LIMIT 1;

SELECT * FROM SN_Register_PCBs;
SELECT * FROM Components_Register_PCB;

INSERT INTO Components_Register_PCB(idRolesOrdenadores, idSN_Register_PCBs, idParameters_Components, Value, Date) 
VALUES ((SELECT idRoles_Ordenadores FROM Roles_Ordenadores WHERE INSTR('itc/M0176830', Ordenador) != 0), 1708, 3, 1, current_timestamp());
    
INSERT INTO Components_Register_PCB(idRolesOrdenadores, idSN_Register_PCBs, idParameters_Components, Value, Date) VALUES (3, 1692, 1, 2, current_timestamp());
-- INSERT INTO Components_Register_PCB(idSN_Register_PCBs, idParameters_Components, Value, Date) VALUES (576, 3, 0, current_timestamp());
--     
-- INSERT INTO Components_Register_PCB(idRolesOrdenadores, idSN_Register_PCBs, idParameters_Components, Value, Date) 
-- VALUES ((SELECT idRoles_Ordenadores FROM Roles_Ordenadores WHERE Ordenador = 'E0164045'),634, 3, 5, current_timestamp());


SELECT * FROM References_PCBs;

CALL InsertarAltaPCB('ordenador', 'PRUEBA2', '26',14, 20, 'PRUEBA','PRUEBA');
CALL Get_Definition_PCB('2')

select * from SN_PCBReferenceLayoutBomFrom
where Layout = 7  and Reference LIKE '%In-Filter';

select * from Components_Register_PCB c
where idSN_Register_PCBs in (select idSN_Register_PCBs from SN_PCBReferenceLayoutBomFrom
where Layout = 7 and BOM = 0 and Reference LIKE '%In-Filter')


SELECT * FROM SN_Register_Housing;
-- SELECT * FROM Internal_Version;
-- SELECT * FROM Status_Register_SubAssembly;
-- SELECT * FROM StatusNumber_FINAS;
-- SELECT * FROM Mechanical_Version;
SELECT * FROM SNHousing_MechanicalVersion;
SELECT * FROM SNHousing_MechanicalWindchill;

-- Elimina procedimiento anterior
DROP PROCEDURE IF EXISTS InsertarAltaHousing;
DELIMITER ;;
CREATE PROCEDURE InsertarAltaHousing (
							ordenadorName VARCHAR(75), 
							snNumber VARCHAR(75), 
                            idMechanicalVersion INT(11)
)
BEGIN
	DECLARE idSNRegister INT(11);
    DECLARE idRolOrdenador INT(11);
	DECLARE now_date DATETIME;
    
    SELECT idRoles_Ordenadores INTO idRolOrdenador FROM Roles_Ordenadores WHERE INSTR(ordenadorName, Ordenador) != 0
    LIMIT 1;
    SET now_date = current_timestamp();
    
	-- CALL Get_OnlyStringNumeric(snNumber, snNumber);
    
    INSERT INTO SN_Register_Housing(idRolesOrdenadores, SN, Status, Date) VALUES (idRolOrdenador, snNumber, 0, now_date);
    -- SELECT idSN_Register_Housing INTO idSNRegister FROM SN_Register_Housing  WHERE Date = now_date;
    SET idSNRegister = LAST_INSERT_ID();
	INSERT INTO SNHousing_MechanicalWindchill (idSN_Register_Housing, idMechanical_Windchill)VALUES(idSNRegister, idMechanicalVersion);
END;;

-- Elimina procedimiento anterior
DROP PROCEDURE IF EXISTS ProcedimientoPrueba;
DELIMITER ;;
CREATE PROCEDURE ProcedimientoPrueba (
							snNumber VARCHAR(75)
)
BEGIN
    SELECT snNumber;
	CALL Get_OnlyStringNumeric(snNumber, snNumber);
    SELECT snNumber;
END;;

CALL ProcedimientoPrueba('SN''234252');


CALL InsertarAltaHousing ('E0164372', 'SN''219149', 1);

SELECT * FROM SN_Register_Housing;
SELECT * FROM SNHousing_MechanicalWindchill;
SELECT * FROM Mechanical_WindchillCode_Housing;

 delete from SNHousing_MechanicalWindchill where idSN_Register_Housing = 1499;
 delete from SN_Register_Housing where idSN_Register_Housing between 349 and 356;
 

 DROP TRIGGER IF EXISTS updateStatus;
 DELIMITER ;;
 CREATE TRIGGER updateStatus AFTER INSERT 
 ON Components_Register_PCB FOR EACH ROW
 BEGIN
	IF NEW.idParameters_Components = 3 AND Value > 0 THEN
		IF (SELECT idReferences_PCBs FROM SN_Register_PCBs WHERE idSN_Register_PCBs = NEW.idSN_Register_PCBs) = 23 OR 24 OR 25 THEN
			UPDATE SN_Register_PCBs SET Status = 0 
            WHERE idSN_Register_PCBs = NEW.idSN_Register_PCBs;
        END IF;
    END IF;
 END;;

INSERT INTO Components_Register_PCB(idSN_Register_PCBs, idParameters_Components, Value, Date) VALUES (576, 2, 2, current_timestamp());

SELECT * FROM References_PCBs;

-- Elimina procedimiento anterior
DROP PROCEDURE IF EXISTS InsertarOF;
DELIMITER ;;
CREATE PROCEDURE InsertarOF (
							ofDesc VARCHAR(150), 
							internalVersionDef VARCHAR(45), 
                            ofQuantity INT(11)
)
BEGIN
	DECLARE idInternalVersionDef INT(11);
    
    SELECT idInternal_Version INTO idInternalVersionDef FROM Internal_Version WHERE Internal_Version = internalVersionDef
    LIMIT 1;
    IF idInternalVersionDef IS NULL THEN
		SIGNAL SQLSTATE '10050' SET MESSAGE_TEXT = '10050: No existe versión interna.';
    ELSEIF NOT EXISTS (SELECT * FROM OF WHERE Description_OF = ofDesc) THEN
		INSERT INTO OF (Description_OF, idInternal_Version, Quantity) VALUES (ofDesc, idInternalVersionDef, ofQuantity);
	ELSE
		SIGNAL SQLSTATE '10051' SET MESSAGE_TEXT = '10051: Orden de fabricación ya creada.';
	END IF;
END;;

SELECT idInternal_Version FROM Internal_Version WHERE Internal_Version = '24003065';
INSERT INTO OF(Description_OF, idInternal_Version, Quantity) VALUES ('24003065', 35, 3);
CALL InsertarOF('24003065', 'B3.1.5', 3);
SELECT * FROM Prototipos.OF ;
SELECT * FROM OF WHERE Description_OF = '24003065'

SELECT * FROM Internal_Version

-- Elimina procedimiento anterior
DROP PROCEDURE IF EXISTS InsertarSA;
DELIMITER ;;
CREATE PROCEDURE InsertarSA (
							idPlataform INT(11),
							ofDesc VARCHAR(45),
							subAssembCode VARCHAR(75), 
                            ofQuantity INT(11)
)
BEGIN
	DECLARE idSubAssembly INT(11);
    
    SELECT idSubAssembly_Definition INTO idSubAssembly FROM SubAssembly_Definition WHERE Code_Referencia_SubAssembly = subAssembCode
    LIMIT 1;
    IF idSubAssembly IS NULL THEN
		SIGNAL SQLSTATE '10052' SET MESSAGE_TEXT = '10052: No existe versión interna.';
    ELSEIF NOT EXISTS (SELECT * FROM OF_SubAssembly WHERE OF_SubAssembly_Description = ofDesc) THEN
		INSERT INTO OF_SubAssembly (idPlataforma, OF_SubAssembly_Description, idSubAssembly_Definition, Quantity) VALUES (idPlataform, ofDesc, idSubAssembly, ofQuantity);
	ELSE
		SIGNAL SQLSTATE '10053' SET MESSAGE_TEXT = '10053: Orden de fabricación ya creada.';
	END IF;
END;;

SELECT * FROM OF_SubAssembly;
SELECT * FROM SubAssembly_Definition;

DROP PROCEDURE IF EXISTS actualizaManufacturing;
DELIMITER ;;

CREATE PROCEDURE actualizaManufacturing()
BEGIN
  DECLARE done INT DEFAULT FALSE;
  
  DECLARE idDuplicateRework INT;
  DECLARE idComponentsRegisterPCB INT;
  
  DECLARE migrate_ids CURSOR FOR 
  # modify the select statement to returns IDs, which will be assigned the variable `_post_id`
  # the following statement gets all wp attachments that are missing attachment metadata
  SELECT idComponents_Register_PCB FROM Components_Register_PCB;
    
  DECLARE CONTINUE HANDLER FOR NOT FOUND SET done=TRUE;

  OPEN migrate_ids;

  read_loop: LOOP
    FETCH migrate_ids INTO idComponentsRegisterPCB;

    IF done THEN
      LEAVE read_loop;
    END IF;

    # modify the insert statement to perform your operation with the `_post_id` 
    # the following insert statement adds missing metadata in wordpress
    SELECT DC.idComponents_Register_PCB INTO idDuplicateRework FROM Components_Register_PCB C
    INNER JOIN Components_Register_PCB DC ON C.idSN_Register_PCBs = DC.idSN_Register_PCBs AND 
											C.idParameters_Components = DC.idParameters_Components AND 
                                            C.Value = DC.Value AND 
                                            C.idComponents_Register_PCB != DC.idComponents_Register_PCB
		WHERE C.idComponents_Register_PCB = idComponentsRegisterPCB;
    
     
    IF idDuplicateRework IS NOT NULL THEN
		DELETE FROM Components_Register_PCB
		WHERE idComponents_Register_PCB = idDuplicateRework;
	END IF;
    
     SET done=FALSE;

  END LOOP;

  CLOSE migrate_ids;
END;;

call actualizaManufacturing();

    SELECT * FROM Components_Register_PCB C
    INNER JOIN Components_Register_PCB DC ON C.idSN_Register_PCBs = DC.idSN_Register_PCBs AND 
											C.idParameters_Components = DC.idParameters_Components AND 
                                            C.Value = DC.Value AND 
                                            C.idComponents_Register_PCB != DC.idComponents_Register_PCB;
        
-- Comparativa versiones internas
	SELECT DISTINCT O.id_OF, MR.idSN_Register_Housing, PNI.idPN_InternalVersion_Definition InternalVersionOF, MR.idPN_InternalVersion_Definition InternalVersionManufacturing FROM OF_SNs O
    INNER JOIN Manufacturing_Register MR ON O.idSN_Register_Housing = MR.idSN_Register_Housing
    INNER JOIN PN_InternalVersion_Definition P ON MR.IdPN_InternalVersion_Definition = P.IdPN_InternalVersion_Definition
    INNER JOIN Prototipos.OF F ON O.id_OF = F.idOF
    INNER JOIN PN_InternalVersion_Definition PNI ON PNI.idInternal_Version = F.idInternal_Version
    where  PNI.idPN_InternalVersion_Definition != MR.idPN_InternalVersion_Definition;
    
select idPictures_Manufacturing_Process, Picture_Description from Pictures_Manufacturing_Process;
select * from SubAssembly_Definition;
select * from Manufacturing_Process;

-- Elimina procedimiento anterior
DROP PROCEDURE IF EXISTS Insert_Manufacturing_Process;
DELIMITER ;;
CREATE PROCEDURE Insert_Manufacturing_Process (
											in_Version_Manuf VARCHAR(150),
											inNum INT(11),
											inProcess INT (11),
											in_Process_Descrip VARCHAR(100),
											in_Operation INT(11),
											in_Picture VARCHAR(45),
											in_Operation_Descript VARCHAR(100),
											in_NomProc VARCHAR(45),
											in_Test VARCHAR(45),
											in_Input VARCHAR(45),
											in_Descrew VARCHAR(250),
											in_AUXPCB varchar(45) 
)
BEGIN
	declare proc varchar(45);
	declare inver bool;
	declare screw int(11);
	declare id_manuf int(11);
	declare id_PCBAux int(11);
	declare id_Test int(11);
	declare id_Picture int(11);
    
	DECLARE exit handler for SQLEXCEPTION
    BEGIN
		-- Borrar
        SELECT idVersion_Procesos_Manufacturing into id_manuf
			FROM Version_Procesos_Manufacturing
			WHERE Nombre = in_Version_Manuf;
		IF id_manuf IS NOT NULL THEN
			DELETE FROM Manufacturing_Process
				WHERE idVersion_Procesos_Manufacturng = id_manuf;
			DELETE FROM Version_Procesos_Manufacturing
				WHERE idVersion_Procesos_Manufacturing = id_manuf;
        END IF;
        RESIGNAL;
    END;
	SET inver = FALSE;
		
	SELECT idVersion_Procesos_Manufacturing into id_manuf
				FROM Version_Procesos_Manufacturing
				WHERE Nombre = in_Version_Manuf;
	IF id_manuf IS NULL THEN
		-- Insertamos el Version_Procesos_Manufacturing
        INSERT INTO Version_Procesos_Manufacturing(Nombre) VALUES (in_Version_Manuf);
        SET id_manuf = LAST_INSERT_ID();
        
        IF id_manuf IS NULL THEN
			SELECT idVersion_Procesos_Manufacturing into id_manuf
				FROM Version_Procesos_Manufacturing
				WHERE Nombre = in_Version_Manuf;
			IF id_manuf IS NULL THEN
					SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Error: Version de Proceso no registrada', MYSQL_ERRNO = 45000;
			END IF;
          END IF;
	END IF;
    
	SELECT idProceso into proc From Procesos WHERE Nombre_Proceso = in_NomProc;
	IF proc IS NULL THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Error: Nombre_Proceso No encontrado', MYSQL_ERRNO = 45000;
	END IF;
				
	IF in_Process_Descrip = '-' THEN 
		SET in_Process_Descrip = NULL;
	END IF;
    
	IF in_Test != '-' THEN
		SELECT idProceso into id_Test From Procesos WHERE Nombre_Proceso = in_Test;
		IF id_Test IS NULL THEN
			SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Error: Nombre_Test No encontrado', MYSQL_ERRNO = 45000;
		END IF;
	END IF;
				
	IF in_Input = '-' THEN 
		SET in_Input = NULL;
	END IF;

	SELECT idPictures_Manufacturing_Process INTO id_Picture FROM Pictures_Manufacturing_Process WHERE Picture_Description = in_Picture;
	IF id_Picture IS NULL THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Error: Imagen no encontrada. Asegurese de haber subido la imagen del proceso.', MYSQL_ERRNO = 45000;
	END IF;

	IF in_AUXPCB = '-' THEN 
		SET id_PCBAux = NULL;
	ELSE
		select idReferences_PCBs into id_PCBAux from References_PCBs where References_PCBs.Reference=in_AUXPCB;
	END IF;
	if in_AUXPCB != '-' and id_PCBAux IS NULL then
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Error: Reference PCB no registrada', MYSQL_ERRNO = 45000;
	end if;

	IF in_Descrew = '-' THEN 
		SET screw = NULL;
	ELSE
		SELECT idScrewDriver_Register into screw FROM ScrewDriver_Register WHERE ScrewDriver_Register.Descripcion = in_Descrew;
		IF screw IS NULL then
			INSERT INTO ScrewDriver_Register(Descripcion) VALUES(in_Descrew);
            SET screw = LAST_INSERT_ID();
		END IF;
	END IF;
	
    IF NOT EXISTS (SELECT * FROM Manufacturing_Process 
					WHERE idVersion_Procesos_Manufacturng = id_manuf AND
										Num = inNum) THEN
		INSERT INTO Manufacturing_Process (idVersion_Procesos_Manufacturng,
											Num, 
											Process_Num, 
											Process_Description, 
											Operation, 
											idPicture_Manufact, 
											Operation_Description, 
											idProceso, 
											idTest,
											Input,
											idScrewdriver, 
											SubAssembly_AUX)
									VALUES (id_manuf, 
											inNum, 
											inProcess, 
											in_Process_Descrip, 
											in_Operation, 
											id_Picture, 
											in_Operation_Descript, 
											proc,
											id_Test,
											in_Input,
											screw, 
											id_PCBAux);
	END IF;
-- 	IF NOT EXISTS (SELECT * FROM Manufacturing_Process
-- 				WHERE idVersion_Procesos_Manufacturng = id_manuf AND Num = inNum AND Process_Num =inProcess AND Operation=in_Operation AND Picture= in_Picture
-- 				AND Operation_Description = in_Operation_Descript AND idProceso = proc AND idScrewdriver= screw AND SubAssembly_AUX=id_PCBAux)
-- 				THEN 
-- 		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Error: 	No se ha insertado el registro.', MYSQL_ERRNO = 45000;
-- 	END IF;
END;;

SELECT * FROM ScrewDriver_Register 

SELECT * FROM Manufacturing_Process;
CALL Insert_Manufacturing_Process ('PRUEBA INSERT',1,1, 'COOL', 1, 'HCUD', 'PRUEBA DESCRIPCION', 'Complete_AG','EOL1', '-','-', '-'); 
SELECT * FROM Manufacturing_Process;
SELECT * FROM Prototipos.Version_Procesos_Manufacturing;

-- Elimina procedimiento anterior
DROP PROCEDURE IF EXISTS Insert_SubAssembly;
DELIMITER ;;
CREATE PROCEDURE Insert_SubAssembly (
									plataforma VARCHAR(150), 
									ref VARCHAR(75), 
									descriptionRef VARCHAR(255), 
									registerDateString VARCHAR(45), 
                                    
									eHW1 VARCHAR(75), 
									eHW2 VARCHAR(75), 
									eHW3 VARCHAR(75), 
									eHW4 VARCHAR(75), 
									eHW5 VARCHAR(75), 
									eHW6 VARCHAR(75), 
									eHW7 VARCHAR(75), 
									eHW8 VARCHAR(75),
                                    
									processName VARCHAR(150),
                                    trazability INT(11),
                                    
                                    imageName VARCHAR(255),
                                    picture LONGBLOB
)
BEGIN
-- Precondiciones: El idInternalVersionDef recibido deberá existir en la bbdd. Los hardware deberán seguir el formato LB-REFF-X.Y.Z (Ej: LB-1234-1.2.3).
-- 					eHW1 debe estar siempre relleno. Los hardware que no se rellenen deberán de pasarse como nulo.
-- Postcondiciones: Los errores generados por el procedimiento corresponden a las diferentes casuisticas que se pueden dar y deben ser tratados en el exterior individualmente.

	DECLARE registerDate DATE;
	DECLARE idPlataform INT(11);
	DECLARE idProcess INT(11);
	DECLARE idHardwareVersion INT(11);
	DECLARE idPicture INT(11);
	DECLARE exit handler for SQLEXCEPTION
    BEGIN
		RESIGNAL;
		ROLLBACK;
    END;
    
    SET registerDate = STR_TO_DATE(registerDateString, '%d/%m/%Y');
    
    START TRANSACTION;
    -- Comprobación de existencia de plataforma
    SELECT idPlataforma INTO idPlataform FROM Plataforma WHERE Nombre_Plataforma = plataforma LIMIT 1;
    IF idPlataform IS NULL THEN
		SIGNAL SQLSTATE '10022' SET MESSAGE_TEXT = 'Plataforma no encontrada';
    END IF;
    
    SELECT idVersion_Procesos_Manufacturing into idProcess FROM Version_Procesos_Manufacturing
	WHERE Nombre = processName;
	IF idProcess IS NULL THEN
		-- INSERT INTO Version_Procesos_Manufacturing (Nombre) VALUES (processName);
		-- SET idProcess = LAST_INSERT_ID();
		
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Error: Version de Proceso no registrada', MYSQL_ERRNO = 45000;
	END IF;
            
	-- Creación o busqueda del bloque de hardware
    IF eHW1 IS NOT NULL THEN
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
    END IF;
	
    IF imageName IS NULL THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Error: Imagen no insertada', MYSQL_ERRNO = 45000;
	ELSE
		SELECT idPictures_Manufacturing_Process INTO idPicture FROM Pictures_Manufacturing_Process
        WHERE Picture_Description = imageName;
        IF idPicture IS NULL THEN
			INSERT INTO Pictures_Manufacturing_Process (Picture_Description, Picture_JPG) VALUES (imageName, picture);
			SELECT idPictures_Manufacturing_Process INTO idPicture FROM Pictures_Manufacturing_Process
				WHERE Picture_Description = imageName;
			SET idPicture = LAST_INSERT_ID();
            -- IF idPicture IS NULL THEN
-- 				SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Error al subir la imágen.', MYSQL_ERRNO = 45000;
-- 			END IF;
        END IF;
	END IF;
        
	INSERT INTO SubAssembly_Definition (Code_Referencia_SubAssembly, 
										Description, 
                                        idVersion_Procesos_Manufacturing, 
                                        idHardware_Version, 
                                        idPicture_SubAssembly, 
                                        Trazability, 
                                        Enable, 
                                        Date)
								VALUES(	ref, 
										descriptionRef, 
                                        idProcess, 
                                        idHardwareVersion, 
                                        idPicture, 
                                        trazability,
                                        1,
                                        registerDate);
	COMMIT;
END;;
-- ERROR: idPicture_SubAssembly can't be null
CALL  Insert_SubAssembly (
									'AMG11KW', 
									'09704249_Ed00', 
									'BusbarDCLV Subassembly', 
									'22/05/2024', 
                                    
									NULL, 
									NULL, 
									NULL, 
									NULL, 
									NULL, 
									NULL, 
									NULL, 
									NULL, 
                                    
									'APP Manufacturing 09704249_Ed00',
                                    0,
                                    
                                    'BusbarDCLV_Subassembly',
                                    NULL
);

SELECT * FROM SubAssembly_Definition;