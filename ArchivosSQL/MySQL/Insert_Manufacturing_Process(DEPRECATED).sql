delimiter ;;
CREATE PROCEDURE `Insert_Manufacturing_Process`(IN in_Version_Manuf VARCHAR(150), IN inNum INT(11), IN inProcess INT (11), IN in_Process_Descrip VARCHAR(100), IN in_Operation INT(11), IN in_Picture VARCHAR(45), IN in_Operation_Descript VARCHAR(100), IN in_NomProc VARCHAR(45),IN in_Descrew VARCHAR(250), IN in_AUXPCB varchar(45) )
BEGIN

declare proc varchar(45);
declare inver bool;
declare screw int(11);
declare id_manuf int(11);
declare id_PCBAux int(11);
declare hora datetime;

SET inver = FALSE;
SET proc = -1;
SET screw = -1;
SET id_manuf = -1;
set id_PCBAux=-1;
SET hora = NOW();
    
SELECT idProceso into proc From Procesos WHERE Nombre_Proceso = in_NomProc;

IF proc = -1 THEN
	SELECT 3 as Resultado;
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Error: Nombre_Proceso No encontrado', MYSQL_ERRNO = 45000;
      Leave sp;
END IF;
            
IF in_Process_Descrip = '-' THEN 
	SET in_Process_Descrip = NULL;
END IF;

IF in_Picture = '-' THEN 
	SET in_Picture = NULL;
END IF;

IF in_AUXPCB = '-' THEN 
	SET id_PCBAux = NULL;
END IF;
IF in_AUXPCB != '-' THEN 
	select idReferences_PCBs into id_PCBAux from References_PCBs where References_PCBs.Reference=in_AUXPCB;
END IF;
if in_AUXPCB != '-' and id_PCBAux= -1 then
	SELECT 3 as Resultado;
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Error: Reference PCB no registrada', MYSQL_ERRNO = 45000;
end if;

SELECT idVersion_Procesos_Manufacturing into id_manuf
            FROM Version_Procesos_Manufacturing
            WHERE Nombre = in_Version_Manuf;
IF id_manuf = -1 THEN
	SELECT 3 as Resultado;
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Error: Version de Proceso no registrada', MYSQL_ERRNO = 45000;
END IF;

IF in_Descrew = '-' THEN 
	SET screw = NULL;
END IF;

IF in_Descrew != '-' THEN 
		SELECT idScrewDriver_Register into screw FROM ScrewDriver_Register WHERE ScrewDriver_Register.Descripcion = in_Descrew;
END IF;

INSERT INTO Manufacturing_Process (idVersion_Procesos_Manufacturng ,Num, Process_Num, Process_Description, Operation , Picture, Operation_Description, idProceso,idScrewdriver, SubAssembly_AUX,`Date`)
VALUES (id_manuf, inNum, inProcess,in_Process_Descrip, in_Operation, in_Picture,in_Operation_Descript,  proc, screw,id_PCBAux,hora);

SELECT 
	CASE 
		WHEN EXISTS (
			SELECT * FROM Manufacturing_Process
            WHERE idVersion_Procesos_Manufacturng = id_manuf AND Num = inNum AND Process_Num =inProcess AND  Operation=in_Operation AND  Picture= in_Picture
            AND Operation_Description = in_Operation_Descript AND idProceso = idProceso AND idScrewdriver= screw AND SubAssembly_AUX=id_PCBAux AND  `Date` = hora
            LIMIT 1)
            THEN 1
		ELSE 3
        
END AS Resultado;
END;;