-- Preparamos las tablas que vamos a utilizar para poder consultarlas facilmente en todo momento
SELECT * FROM  References_PCBs;
SELECT * FROM  Version_Hardware_PCBs;
SELECT * FROM  Version_Parameters_HW_PCBs;
SELECT * FROM  Hardware_Version;
SELECT * FROM  PN_InternalVersion_Definition;
SELECT * FROM  Internal_Version;

-- 1º Creamos una tabla para los parámetros de Layout otra para los de BOM y otra para los de Rework y las unimos entre si mediante la tabla de versionParameter para tener los registros que queremos en una sola fila.
SELECT * FROM 
		(SELECT VP.idVersion_Parameters_HW_PCBs, H.idReferences_PCBs, H.Value_Parameter FROM Version_Parameters_HW_PCBs VP
		INNER JOIN Version_Hardware_PCBs H ON VP.idVersion_Hardware_PCBs = H.idVersion_Hardware_PCBs AND H.idParameters_Components=1) AS Layout
	INNER JOIN (
		SELECT VP.idVersion_Parameters_HW_PCBs, H.idReferences_PCBs, H.Value_Parameter FROM Version_Parameters_HW_PCBs VP
		INNER JOIN Version_Hardware_PCBs H ON VP.idVersion_Hardware_PCBs = H.idVersion_Hardware_PCBs AND H.idParameters_Components=2) AS BOM
	ON Layout.idVersion_Parameters_HW_PCBs = BOM.idVersion_Parameters_HW_PCBs
	INNER JOIN
		(SELECT VP.idVersion_Parameters_HW_PCBs, H.idReferences_PCBs, H.Value_Parameter FROM Version_Parameters_HW_PCBs VP
		INNER JOIN Version_Hardware_PCBs H ON VP.idVersion_Hardware_PCBs = H.idVersion_Hardware_PCBs AND H.idParameters_Components=3) AS Rework
	ON Layout.idVersion_Parameters_HW_PCBs = Rework.idVersion_Parameters_HW_PCBs;

-- 2º Creamos una tabla con todos los datos que queremos recoger enlazados (Id del versionParameter para identificar la PCB, idReferenece para saber el numero de referencia y la cambinación de Layout, BOM y Rework)
SELECT Layout.idVersion_Parameters_HW_PCBs, Layout.idReferences_PCBs, CONCAT_WS('.', Layout.Value_Parameter, IFNULL(BOM.Value_Parameter,0), IFNULL(Rework.Value_Parameter,0)) AS XYZ FROM
		(SELECT VP.idVersion_Parameters_HW_PCBs, H.idReferences_PCBs, H.Value_Parameter FROM Version_Parameters_HW_PCBs VP
		INNER JOIN Version_Hardware_PCBs H ON VP.idVersion_Hardware_PCBs = H.idVersion_Hardware_PCBs AND H.idParameters_Components=1) AS Layout
	INNER JOIN (
		SELECT VP.idVersion_Parameters_HW_PCBs, H.idReferences_PCBs, H.Value_Parameter FROM Version_Parameters_HW_PCBs VP
		INNER JOIN Version_Hardware_PCBs H ON VP.idVersion_Hardware_PCBs = H.idVersion_Hardware_PCBs AND H.idParameters_Components=2) AS BOM
	ON Layout.idVersion_Parameters_HW_PCBs = BOM.idVersion_Parameters_HW_PCBs
	INNER JOIN
		(SELECT VP.idVersion_Parameters_HW_PCBs, H.idReferences_PCBs, H.Value_Parameter FROM Version_Parameters_HW_PCBs VP
		INNER JOIN Version_Hardware_PCBs H ON VP.idVersion_Hardware_PCBs = H.idVersion_Hardware_PCBs AND H.idParameters_Components=3) AS Rework
	ON Layout.idVersion_Parameters_HW_PCBs = Rework.idVersion_Parameters_HW_PCBs;

-- 3º Enlazamos con la tabla References_PCBs la consulta anterior (donde iria el nombre de la primera tabla ponemos dos parentesis y pegamos en medio la subconsulta de arriba) y concatenamos con un guión
SELECT PARAM.idVersion_Parameters_HW_PCBs, CONCAT_WS('-', REF.Reference, PARAM.XYZ) AS PCB FROM (
		SELECT Layout.idVersion_Parameters_HW_PCBs, Layout.idReferences_PCBs, CONCAT_WS('.', Layout.Value_Parameter, IFNULL(BOM.Value_Parameter,0), IFNULL(Rework.Value_Parameter,0)) AS XYZ FROM
			(SELECT VP.idVersion_Parameters_HW_PCBs, H.idReferences_PCBs, H.Value_Parameter FROM Version_Parameters_HW_PCBs VP
			INNER JOIN Version_Hardware_PCBs H ON VP.idVersion_Hardware_PCBs = H.idVersion_Hardware_PCBs AND H.idParameters_Components=1) AS Layout
		INNER JOIN (
			SELECT VP.idVersion_Parameters_HW_PCBs, H.idReferences_PCBs, H.Value_Parameter FROM Version_Parameters_HW_PCBs VP
			INNER JOIN Version_Hardware_PCBs H ON VP.idVersion_Hardware_PCBs = H.idVersion_Hardware_PCBs AND H.idParameters_Components=2) AS BOM
		ON Layout.idVersion_Parameters_HW_PCBs = BOM.idVersion_Parameters_HW_PCBs
		INNER JOIN
			(SELECT VP.idVersion_Parameters_HW_PCBs, H.idReferences_PCBs, H.Value_Parameter FROM Version_Parameters_HW_PCBs VP
			INNER JOIN Version_Hardware_PCBs H ON VP.idVersion_Hardware_PCBs = H.idVersion_Hardware_PCBs AND H.idParameters_Components=3) AS Rework
		ON Layout.idVersion_Parameters_HW_PCBs = Rework.idVersion_Parameters_HW_PCBs)AS PARAM
    INNER JOIN References_PCBs AS REF ON PARAM.idReferences_PCBs = REF.idReferences_PCBs;

-- 4º Enlazamos las tablas para llegar hasta la InternalVersion y mostramos los campos que estamos buscando.

SELECT IV.Internal_Version, VPARAM.PCB FROM (
		SELECT PARAM.idVersion_Parameters_HW_PCBs, CONCAT_WS('-', REF.Reference, PARAM.XYZ) AS PCB FROM (
			SELECT Layout.idVersion_Parameters_HW_PCBs, Layout.idReferences_PCBs, CONCAT_WS('.', Layout.Value_Parameter, IFNULL(BOM.Value_Parameter,0), IFNULL(Rework.Value_Parameter,0)) AS XYZ FROM
				(SELECT VP.idVersion_Parameters_HW_PCBs, H.idReferences_PCBs, H.Value_Parameter FROM Version_Parameters_HW_PCBs VP
				INNER JOIN Version_Hardware_PCBs H ON VP.idVersion_Hardware_PCBs = H.idVersion_Hardware_PCBs AND H.idParameters_Components=1) AS Layout
			INNER JOIN (
				SELECT VP.idVersion_Parameters_HW_PCBs, H.idReferences_PCBs, H.Value_Parameter FROM Version_Parameters_HW_PCBs VP
				INNER JOIN Version_Hardware_PCBs H ON VP.idVersion_Hardware_PCBs = H.idVersion_Hardware_PCBs AND H.idParameters_Components=2) AS BOM
			ON Layout.idVersion_Parameters_HW_PCBs = BOM.idVersion_Parameters_HW_PCBs
			INNER JOIN
				(SELECT VP.idVersion_Parameters_HW_PCBs, H.idReferences_PCBs, H.Value_Parameter FROM Version_Parameters_HW_PCBs VP
				INNER JOIN Version_Hardware_PCBs H ON VP.idVersion_Hardware_PCBs = H.idVersion_Hardware_PCBs AND H.idParameters_Components=3) AS Rework
			ON Layout.idVersion_Parameters_HW_PCBs = Rework.idVersion_Parameters_HW_PCBs)AS PARAM
			INNER JOIN References_PCBs AS REF ON PARAM.idReferences_PCBs = REF.idReferences_PCBs) AS VPARAM
	INNER JOIN  Hardware_Version AS HV ON VPARAM.idVersion_Parameters_HW_PCBs = HV.idVersion_Parameters_HW_PCBs
	INNER JOIN  PN_InternalVersion_Definition AS IVD ON HV.idHardware_Version = IVD.idHardware_Version
	INNER JOIN  Internal_Version AS IV ON IVD.idInternal_Version = IV.idInternal_Version;

-- 5º Creamos la vista una vez nuestra consulta muestra lo que deseamos.
CREATE OR REPLACE VIEW ListPCB_FromIV_Pruebas AS
	SELECT IV.Internal_Version, VPARAM.PCB FROM (
		SELECT PARAM.idVersion_Parameters_HW_PCBs, CONCAT_WS('-', REF.Reference, PARAM.XYZ) AS PCB FROM (
			SELECT Layout.idVersion_Parameters_HW_PCBs, Layout.idReferences_PCBs, CONCAT_WS('.', Layout.Value_Parameter, IFNULL(BOM.Value_Parameter,0), IFNULL(Rework.Value_Parameter,0)) AS XYZ FROM
				(SELECT VP.idVersion_Parameters_HW_PCBs, H.idReferences_PCBs, H.Value_Parameter FROM Version_Parameters_HW_PCBs VP
				INNER JOIN Version_Hardware_PCBs H ON VP.idVersion_Hardware_PCBs = H.idVersion_Hardware_PCBs AND H.idParameters_Components=1) AS Layout
			INNER JOIN (
				SELECT VP.idVersion_Parameters_HW_PCBs, H.idReferences_PCBs, H.Value_Parameter FROM Version_Parameters_HW_PCBs VP
				INNER JOIN Version_Hardware_PCBs H ON VP.idVersion_Hardware_PCBs = H.idVersion_Hardware_PCBs AND H.idParameters_Components=2) AS BOM
			ON Layout.idVersion_Parameters_HW_PCBs = BOM.idVersion_Parameters_HW_PCBs
			INNER JOIN
				(SELECT VP.idVersion_Parameters_HW_PCBs, H.idReferences_PCBs, H.Value_Parameter FROM Version_Parameters_HW_PCBs VP
				INNER JOIN Version_Hardware_PCBs H ON VP.idVersion_Hardware_PCBs = H.idVersion_Hardware_PCBs AND H.idParameters_Components=3) AS Rework
			ON Layout.idVersion_Parameters_HW_PCBs = Rework.idVersion_Parameters_HW_PCBs)AS PARAM
			INNER JOIN References_PCBs AS REF ON PARAM.idReferences_PCBs = REF.idReferences_PCBs) AS VPARAM
	INNER JOIN  Hardware_Version AS HV ON VPARAM.idVersion_Parameters_HW_PCBs = HV.idVersion_Parameters_HW_PCBs
	INNER JOIN  PN_InternalVersion_Definition AS IVD ON HV.idHardware_Version = IVD.idHardware_Version
	INNER JOIN  Internal_Version AS IV ON IVD.idInternal_Version = IV.idInternal_Version;


-- Comprobamos
SELECT * FROM ListPCB_FromIV_Pruebas;