
-- Elimina funcióm anterior
DROP FUNCTION IF EXISTS GetIdTesting;
DELIMITER ;;
CREATE FUNCTION GetIdTesting (
								test1 VARCHAR(75), 
								test2 VARCHAR(75), 
								test3 VARCHAR(75), 
								test4 VARCHAR(75), 
								test5 VARCHAR(75), 
								test6 VARCHAR(75), 
								test7 VARCHAR(75), 
								test8 VARCHAR(75))
RETURNS INT(11)
BEGIN
-- Precondiciones: test1 debe estar siempre relleno. Los test que no se rellenen deberán de pasarse como nulo. Si se introducen varios nombresiguales se contabilizarán como el mismo test.
	DECLARE idTesting INT(11);

	SELECT ID INTO idTesting FROM (
		SELECT idTesting_Version ID, COUNT(*) numOfTest FROM Testing_Version T
		INNER JOIN Procesos P ON T.idProceso = P.idProceso
		WHERE idTesting_Version IN (SELECT idTesting_Version FROM Testing_Version T
									INNER JOIN Procesos P ON T.idProceso = P.idProceso
									WHERE Nombre_Proceso LIKE test1
									INTERSECT
									SELECT idTesting_Version FROM Testing_Version T
									INNER JOIN Procesos P ON T.idProceso = P.idProceso
									WHERE Nombre_Proceso LIKE IFNULL(test2, test1)
									INTERSECT
									SELECT idTesting_Version FROM Testing_Version T
									INNER JOIN Procesos P ON T.idProceso = P.idProceso
									WHERE Nombre_Proceso LIKE IFNULL(test3, test1)
									INTERSECT
									SELECT idTesting_Version FROM Testing_Version T
									INNER JOIN Procesos P ON T.idProceso = P.idProceso
									WHERE Nombre_Proceso LIKE IFNULL(test4, test1)
									INTERSECT
									SELECT idTesting_Version FROM Testing_Version T
									INNER JOIN Procesos P ON T.idProceso = P.idProceso
									WHERE Nombre_Proceso LIKE IFNULL(test5, test1)
									INTERSECT
									SELECT idTesting_Version FROM Testing_Version T
									INNER JOIN Procesos P ON T.idProceso = P.idProceso
									WHERE Nombre_Proceso LIKE IFNULL(test6, test1)
									INTERSECT
									SELECT idTesting_Version FROM Testing_Version T
									INNER JOIN Procesos P ON T.idProceso = P.idProceso
									WHERE Nombre_Proceso LIKE IFNULL(test7, test1)
									INTERSECT
									SELECT idTesting_Version FROM Testing_Version T
									INNER JOIN Procesos P ON T.idProceso = P.idProceso
									WHERE Nombre_Proceso LIKE IFNULL(test8, test1)
									)
		GROUP BY ID
		) INTER
	WHERE numOfTest = (SELECT COUNT(*) FROM (
						SELECT test1
						UNION 
						SELECT IFNULL(test2, test1)
						UNION 
						SELECT IFNULL(test3, test1)
						UNION 
						SELECT IFNULL(test4, test1)
						UNION 
						SELECT IFNULL(test5, test1)
						UNION 
						SELECT IFNULL(test6, test1)
						UNION 
						SELECT IFNULL(test7, test1)
						UNION 
						SELECT IFNULL(test8, test1))NUM);
	RETURN idTesting;
END;;

-- Prueba
-- SELECT ID FROM (
-- 	SELECT idTesting_Version ID, COUNT(*) numOfTest FROM Testing_Version T
-- 	INNER JOIN Procesos P ON T.idProceso = P.idProceso
-- 	WHERE idTesting_Version IN (SELECT idTesting_Version FROM Testing_Version T
-- 								INNER JOIN Procesos P ON T.idProceso = P.idProceso
-- 								WHERE Nombre_Proceso LIKE 'FCT Control' 
--                                 INTERSECT
--                                 SELECT idTesting_Version FROM Testing_Version T
-- 								INNER JOIN Procesos P ON T.idProceso = P.idProceso
-- 								WHERE Nombre_Proceso LIKE IFNULL(NULL, 'FCT Control')
--                                 INTERSECT
--                                 SELECT idTesting_Version FROM Testing_Version T
-- 								INNER JOIN Procesos P ON T.idProceso = P.idProceso
-- 								WHERE Nombre_Proceso LIKE IFNULL('FCT In-Filter', 'FCT Control')
--                                 INTERSECT
--                                 SELECT idTesting_Version FROM Testing_Version T
-- 								INNER JOIN Procesos P ON T.idProceso = P.idProceso
-- 								WHERE Nombre_Proceso LIKE IFNULL('Passthrough', 'FCT Control')
--                                 INTERSECT
--                                 SELECT idTesting_Version FROM Testing_Version T
-- 								INNER JOIN Procesos P ON T.idProceso = P.idProceso
-- 								WHERE Nombre_Proceso LIKE IFNULL(NULL, 'FCT Control')
--                                 INTERSECT
--                                 SELECT idTesting_Version FROM Testing_Version T
-- 								INNER JOIN Procesos P ON T.idProceso = P.idProceso
-- 								WHERE Nombre_Proceso LIKE IFNULL('EOL1', 'FCT Control')
--                                 INTERSECT
--                                 SELECT idTesting_Version FROM Testing_Version T
-- 								INNER JOIN Procesos P ON T.idProceso = P.idProceso
-- 								WHERE Nombre_Proceso LIKE IFNULL(NULL, 'FCT Control')
--                                 INTERSECT
--                                 SELECT idTesting_Version FROM Testing_Version T
-- 								INNER JOIN Procesos P ON T.idProceso = P.idProceso
-- 								WHERE Nombre_Proceso LIKE IFNULL(NULL, 'FCT Control')
-- 								)
-- 	GROUP BY ID
-- 	) INTER
-- WHERE numOfTest = (SELECT COUNT(*) FROM (
-- 					SELECT 'FCT Control'
-- 					UNION 
-- 					SELECT IFNULL(NULL, 'FCT Control')
-- 					UNION 
-- 					SELECT IFNULL('FCT In-Filter', 'FCT Control')
-- 					UNION 
-- 					SELECT IFNULL('Passthrough', 'FCT Control')
-- 					UNION 
-- 					SELECT IFNULL(NULL, 'FCT Control')
-- 					UNION 
-- 					SELECT IFNULL('EOL1', 'FCT Control') 
-- 					UNION 
-- 					SELECT IFNULL(NULL, 'FCT Control')
-- 					UNION 
-- 					SELECT IFNULL(NULL, 'FCT Control'))NUM);

SELECT GetIdTesting('FCT Control', 'FCT In-Filter',NULL,'Passthrough','EOL1','EOL1', NULL, NULL);                 
                                
SELECT * FROM Testing_Version T
	INNER JOIN Procesos P ON T.idProceso = P.idProceso
    
-- Elimina funcióm anterior
DROP FUNCTION IF EXISTS ContainsAllTest;
DELIMITER ;;
CREATE FUNCTION ContainsAllTest (
								test1 VARCHAR(75), 
								test2 VARCHAR(75), 
								test3 VARCHAR(75), 
								test4 VARCHAR(75), 
								test5 VARCHAR(75), 
								test6 VARCHAR(75), 
								test7 VARCHAR(75), 
								test8 VARCHAR(75))
RETURNS INT(11)
BEGIN
-- Precondiciones: test1 debe estar siempre relleno. Los test que no se rellenen deberán de pasarse como nulo.
-- Descripción: Devuelve 1 si todos los nombres existen en la bbdd si se le pasa el 'idInternalVersionTest' a NULL o si coinciden con los almacenados en
	DECLARE containTests INT(11);
	IF (SELECT COUNT(*) FROM (
			SELECT * FROM Procesos 
				WHERE Nombre_Proceso LIKE test1 
			UNION
			SELECT * FROM Procesos 
				WHERE Nombre_Proceso LIKE IFNULL(test2, test1) 
			UNION
			SELECT * FROM Procesos 
				WHERE Nombre_Proceso LIKE IFNULL(test3, test1)
			UNION
			SELECT * FROM Procesos 
				WHERE Nombre_Proceso LIKE IFNULL(test4, test1)
			UNION
			SELECT * FROM Procesos 
				WHERE Nombre_Proceso LIKE IFNULL(test5, test1) 
			UNION
			SELECT * FROM Procesos 
				WHERE Nombre_Proceso LIKE IFNULL(test6, test1) 
			UNION
			SELECT * FROM Procesos 
				WHERE Nombre_Proceso LIKE IFNULL(test7, test1) 
			UNION
			SELECT * FROM Procesos 
				WHERE Nombre_Proceso LIKE IFNULL(test8, test1)) A) 	
			=
			(SELECT COUNT(*) FROM (
				SELECT test1
				UNION 
				SELECT IFNULL(test2, test1) 
				UNION 
				SELECT IFNULL(test3, test1) 
				UNION 
				SELECT IFNULL(test4, test1) 
				UNION 
				SELECT IFNULL(test5, test1) 
				UNION 
				SELECT IFNULL(test6, test1) 
				UNION 
				SELECT IFNULL(test7, test1) 
				UNION 
				SELECT IFNULL(test8, test1)) B)
		THEN
			SET containTests = 1;
	ELSE
			SET containTests = 0;
    END IF;
	RETURN containTests;
END;;

SELECT ContainsAllTest('Passthrough', 'HiPot', 'FCT In-Filter', 'FCT MainBoard', NULL, 'EOL1', NULL, NULL)


-- Elimina procedimiento anterior
DROP PROCEDURE IF EXISTS InsertOrCheckTesting;
DELIMITER ;;
CREATE PROCEDURE InsertOrCheckTesting (
								idInternalVersionDef INT(11),
                                test1 VARCHAR(75), 
								test2 VARCHAR(75), 
								test3 VARCHAR(75), 
								test4 VARCHAR(75), 
								test5 VARCHAR(75), 
								test6 VARCHAR(75), 
								test7 VARCHAR(75), 
								test8 VARCHAR(75))
BEGIN
-- Precondiciones: El idInternalVersionDef recibido deberá existir en la bbdd. test1 debe estar siempre relleno. Los test que no se rellenen deberán de pasarse como nulo.
-- Postcondiciones: Los errores generados por el procedimiento corresponden a las diferentes casuisticas que se pueden dar y deben ser tratados en el exterior individualmente.

	DECLARE idTestingVersion INT(11);
-- 	DECLARE exit handler for SQLEXCEPTION
--     BEGIN
-- 		SHOW ERRORS;
-- 		ROLLBACK;
--     END;
--     
--     START TRANSACTION;
    
    IF (SELECT containsAllTest(test1, test2, test3, test4, test5, test6, test7, test8) = 0)
		THEN
			SIGNAL SQLSTATE '10031' SET MESSAGE_TEXT = '10031: Nombre/s test no existe/n'; 
	ELSE
		SELECT idTesting_Version INTO idTestingVersion FROM PN_InternalVersion_Definition
			WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
		
		IF idTestingVersion IS NULL THEN
			SELECT getIdTesting(test1, test2, test3, test4, test5, test6, test7, test8) INTO idTestingVersion;
			IF idTestingVersion IS NULL THEN -- Añadimos un nuevo bloque de tests para vesion interna
				SELECT MAX(idTesting_Version)+1 INTO idTestingVersion FROM Testing_Version;
				
				INSERT INTO Testing_Version 
				SELECT idTestingVersion, idProceso FROM Procesos 
				WHERE Nombre_Proceso LIKE test1 AND
					Nombre_Proceso LIKE IFNULL(test2, test1) OR
					Nombre_Proceso LIKE IFNULL(test3, test1) OR
					Nombre_Proceso LIKE IFNULL(test4, test1) OR
					Nombre_Proceso LIKE IFNULL(test5, test1) OR
					Nombre_Proceso LIKE IFNULL(test6, test1) OR
					Nombre_Proceso LIKE IFNULL(test7, test1) OR
					Nombre_Proceso LIKE IFNULL(test8, test1);
			END IF;
			
			UPDATE PN_InternalVersion_Definition SET idTesting_Version = idTestingVersion
			WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
			
		ELSEIF (SELECT getIdTesting(test1, test2, test3, test4, test5, test6, test7, test8) != idTestingVersion) THEN
			SIGNAL SQLSTATE '10032' SET MESSAGE_TEXT = '10032: Id testing no corresponde';
		END IF;
	END IF;
END;;

CALL InsertOrCheckTesting(99,'FCT Control', NULL,'FCT In-Filter','Passthrough',Null,'EOL1', NULL, NULL);
SELECT getIdTesting('FCT Control', NULL,'FCT In-Filter','Passthrough',Null,'EOL1', NULL, NULL);


SELECT * FROM Procesos 
            WHERE Nombre_Proceso LIKE 'FCT Control' OR
				Nombre_Proceso LIKE IFNULL(NULL, 'FCT Control') OR
				Nombre_Proceso LIKE IFNULL('FCT In-Filter', 'FCT Control') OR
				Nombre_Proceso LIKE IFNULL('Passthrough', 'FCT Control') OR
				Nombre_Proceso LIKE IFNULL(NULL, 'FCT Control') OR
				Nombre_Proceso LIKE IFNULL('EOL1', 'FCT Control') OR
				Nombre_Proceso LIKE IFNULL(NULL, 'FCT Control') OR
				Nombre_Proceso LIKE IFNULL(NULL, 'FCT Control')
                

