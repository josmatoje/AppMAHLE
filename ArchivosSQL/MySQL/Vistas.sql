use Prototipos;

-- select * from Manufacturing_Register;
-- select * from Usuarios_Registro_Permisos;

select * from Manufacturing_Register;
select * from Usuarios_Registro_Manufacturin;
select * from References_PCBs;

select idReferences_PCBs, Description_Reference, Reference, idPlataforma from References_PCBs;

CREATE OR REPLACE VIEW LayoutBomFromPCBReference AS
SELECT DISTINCT Final.idReferences_PCBs, CONCAT_WS('.', Final.Layout, IFNULL(Final.Bom, 0)) 'Layout.Bom'  FROM (
SELECT R.idReferences_PCBs, CL.Value Layout,  CB.Value Bom FROM SN_Register_PCBs R
	INNER JOIN Components_Register_PCB CL ON R.idSN_Register_PCBs = CL.idSN_Register_PCBs AND CL.idParameters_Components = 1
	LEFT JOIN Components_Register_PCB CB ON R.idSN_Register_PCBs = CB.idSN_Register_PCBs AND CB.idParameters_Components = 2
UNION
SELECT VL.idReferences_PCBs, VL.Value_Parameter, VB.Value_Parameter FROM 
    (SELECT idVersion_Parameters_HW_PCBs, idReferences_PCBs, Value_Parameter FROM Version_Parameters_HW_PCBs V
	INNER JOIN Version_Hardware_PCBs VH ON V.idVersion_Hardware_PCBs= VH.idVersion_Hardware_PCBs AND VH.idParameters_Components = 1) VL
	LEFT JOIN 
    (SELECT idVersion_Parameters_HW_PCBs, idReferences_PCBs, Value_Parameter FROM Version_Parameters_HW_PCBs V
	INNER JOIN Version_Hardware_PCBs VH ON V.idVersion_Hardware_PCBs= VH.idVersion_Hardware_PCBs AND VH.idParameters_Components = 2) VB
    ON VL.idVersion_Parameters_HW_PCBs = VB.idVersion_Parameters_HW_PCBs
    ) Final
ORDER BY Final.Layout DESC, Final.Bom DESC;
  
SELECT * FROM LayoutBomFromPCBReference
where idReferences_PCBs = 25;

SELECT idReferences_PCBs, `Layout.Bom` FROM LayoutBomFromPCBReference;

select * from SN_Register_PCBs;
select * from Components_Register_PCB;
SELECT * FROM Version_Hardware_PCBs;
SELECT * FROM Version_Parameters_HW_PCBs;

SELECT R.idSN_Register_PCBs, C.idParameters_Components, C.Value FROM SN_Register_PCBs R
	INNER JOIN Components_Register_PCB C ON R.idSN_Register_PCBs = C.idSN_Register_PCBs
WHERE R.idReferences_PCBs = 23;


CREATE OR REPLACE VIEW SN_PCBReferenceLayoutBomFrom AS
SELECT REG.idSN_Register_PCBs, REG.SN, CONCAT_WS('-','LG', REF.Reference,REF.Description_Reference) AS 'Reference', CL.Value AS Layout, IFNULL(CB.Value,0) AS BOM, IFNULL(CR.Value,0) AS 'Rework' FROM SN_Register_PCBs REG
INNER JOIN References_PCBs REF ON REG.idReferences_PCBs = REF.idReferences_PCBs 
INNER JOIN Components_Register_PCB CL ON REG.idSN_Register_PCBs = CL.idSN_Register_PCBs AND CL.idParameters_Components=1
LEFT JOIN Components_Register_PCB CB ON REG.idSN_Register_PCBs = CB.idSN_Register_PCBs AND CB.idParameters_Components=2
LEFT JOIN Components_Register_PCB CR ON REG.idSN_Register_PCBs = CR.idSN_Register_PCBs AND CR.idParameters_Components=3;

SELECT * FROM SN_PCBReferenceLayoutBomFrom
where SN = '0950427814400000023701'
ORDER BY Rework DESC 
LIMIT 1;

select * from SN_Register_PCBs
where SN = '0950426114600000016101';

select * from Components_Register_PCBSN_PCBReferenceLayoutBomFrom
where idSN_Register_PCBs = 623;

INSERT INTO Components_Register_PCB(idSN_Register_PCBs, idParameters_Components, Value, Date) VALUES (1, 3, 5, current_timestamp());

delete from Components_Register_PCB where idComponents_Register_PCB  between 541 and 542;

select * from Components_Register_PCB;

SELECT MAX(Value) FROM Components_Register_PCB
WHERE idSN_Register_PCBs = 5 AND idParameters_Components = 3;


CREATE OR REPLACE VIEW PCBReferenceLayoutBom AS
SELECT REFXYZ.idReferences_PCBs, CONCAT_WS('-','LG', REFXYZ.Reference, REFXYZ.XYZ) AS 'Reference' FROM (
SELECT REF.idReferences_PCBS, REF.Reference, CONCAT_WS('.', HL.Value_Parameter, IFNULL(HB.Value_Parameter,0), IFNULL(HR.Value_Parameter,0)) AS XYZ FROM References_PCBs REF
INNER JOIN Version_Hardware_PCBs HL ON REF.idReferences_PCBs = HL.idReferences_PCBs AND HL.idParameters_Components=1
LEFT JOIN Version_Hardware_PCBs HB ON REF.idReferences_PCBs = HB.idReferences_PCBs AND HB.idParameters_Components=2
LEFT JOIN Version_Hardware_PCBs HR ON REF.idReferences_PCBs = HR.idReferences_PCBs AND HR.idParameters_Components=3)AS REFXYZ;

SELECT * FROM PCBReferenceLayoutBom;

CREATE OR REPLACE VIEW HardwarePCBReferenceLayoutBom AS
SELECT DISTINCT VPH.idVersion_Parameters_HW_PCBs, CONCAT_WS('-','LG', REF.Reference,CONCAT_WS('.', IFNULL(PL.Value_parameter,0), IFNULL(PB.Value_parameter,0), IFNULL(PR.Value_parameter,0))) AS 'HardwareReference' FROM Version_Parameters_HW_PCBs VPH
INNER JOIN (
	SELECT VPH.idVersion_Parameters_HW_PCBs,VH.idParameters_Components, VH.Value_Parameter, VH.idReferences_PCBs FROM Version_Parameters_HW_PCBs VPH
	INNER JOIN Version_Hardware_PCBs VH ON VPH.idVersion_Hardware_PCBs = VH.idVersion_Hardware_PCBs
	) PL ON VPH.idVersion_Parameters_HW_PCBs = PL.idVersion_Parameters_HW_PCBs AND PL.idParameters_Components = 1
LEFT JOIN (
	SELECT VPH.idVersion_Parameters_HW_PCBs,VH.idParameters_Components, VH.Value_Parameter, VH.idReferences_PCBs FROM Version_Parameters_HW_PCBs VPH
	INNER JOIN Version_Hardware_PCBs VH ON VPH.idVersion_Hardware_PCBs = VH.idVersion_Hardware_PCBs
	) PB ON VPH.idVersion_Parameters_HW_PCBs = PB.idVersion_Parameters_HW_PCBs AND PB.idParameters_Components = 2
LEFT JOIN (
	SELECT VPH.idVersion_Parameters_HW_PCBs,VH.idParameters_Components, VH.Value_Parameter, VH.idReferences_PCBs FROM Version_Parameters_HW_PCBs VPH
	INNER JOIN Version_Hardware_PCBs VH ON VPH.idVersion_Hardware_PCBs = VH.idVersion_Hardware_PCBs
	) PR ON VPH.idVersion_Parameters_HW_PCBs = PR.idVersion_Parameters_HW_PCBs AND PR.idParameters_Components = 3
LEFT JOIN References_PCBs REF ON PL.idReferences_PCBs = REF.idReferences_PCBs ;

SELECT * FROM HardwarePCBReferenceLayoutBom;
select * from References_PCBs;


CREATE OR REPLACE VIEW RolesUsers AS
SELECT Ordenador, Nombre_Usuario, Descripcion FROM Roles_Ordenadores RO
LEFT JOIN Roles_Descrip_Assig RDA ON RO.idRoles_Ordenadores = RDA.idRoles_Ordenadores
LEFT JOIN RolesOrdenadores_Descripcion RD ON RDA.idRolesOrdenadores_Definicion = RD.idRolesOrdenadores_Definicion;

SELECT * FROM RolesUsers;


CREATE OR REPLACE VIEW InternalVersionsPlataformEnable AS
SELECT IV.Internal_Version, PN.idPlataforma, ID.Enable, ID.`Registration Date` as Fecha FROM PN_InternalVersion_Definition ID
INNER JOIN Internal_Version IV ON ID.idInternal_Version = IV.idInternal_Version
INNER JOIN Part_Number_DEF PN ON IV.IdPart_Number_DEF = PN.idPart_Number_DEF;

SELECT Internal_Version FROM InternalVersionsPlataformEnable
WHERE idPlataforma = 1 AND Enable = 1
ORDER BY Fecha DESC;

SELECT * FROM SubAssembly_Components;
SELECT * FROM SubAssembly_Components_Group;
SELECT * FROM SubAssembly_Definition;
SELECT * FROM References_SubAssembly;
SELECT * FROM SubAssembly_Components;

use Prototipos;
CREATE OR REPLACE VIEW Get_Manufacturing_ProcessFromIV AS
    SELECT 
		Internal_Version.Internal_Version AS Product,
        Version_Procesos_Manufacturing.Nombre AS Nombre_Proceso,
        Manufacturing_Process.Num AS Num_Operation,
        Manufacturing_Process.Process_Num AS Num_Process,
        Manufacturing_Process.Process_Description AS Process_Description,
        Manufacturing_Process.Operation AS Operation_Process,
        Manufacturing_Process.Picture AS Picture,
        Manufacturing_Process.Operation_Description AS Operation_Description,
        Procesos.Nombre_Proceso AS Process_Name,
        COALESCE(ScrewDriver_Register.Descripcion, ' ') AS Screwdriver,
        COALESCE(References_PCBs.Reference, ' ') AS SubAssembly_PCB
    FROM Manufacturing_Process
        INNER JOIN Procesos ON Manufacturing_Process.idProceso = Procesos.idProceso
        INNER JOIN PN_InternalVersion_Definition ON Manufacturing_Process.idVersion_Procesos_Manufacturng = PN_InternalVersion_Definition.idVersion_Procesos_Manufacturing
        INNER JOIN Internal_Version ON PN_InternalVersion_Definition.idInternal_Version = Internal_Version.idInternal_Version
        INNER JOIN Version_Procesos_Manufacturing ON Manufacturing_Process.idVersion_Procesos_Manufacturng = Version_Procesos_Manufacturing.idVersion_Procesos_Manufacturing
         LEFT JOIN ScrewDriver_Register ON Manufacturing_Process.idScrewdriver = ScrewDriver_Register.idScrewDriver_Register
         LEFT JOIN References_PCBs ON Manufacturing_Process.SubAssembly_AUX = References_PCBs.idReferences_PCBs
UNION
SELECT 
		SubAssembly_Definition.Code_Referencia_SubAssembly AS Product,
        Version_Procesos_Manufacturing.Nombre AS Nombre_Proceso,
        Manufacturing_Process.Num AS Num_Operation,
        Manufacturing_Process.Process_Num AS Num_Process,
        Manufacturing_Process.Process_Description AS Process_Description,
        Manufacturing_Process.Operation AS Operation_Process,
        Manufacturing_Process.Picture AS Picture,
        Manufacturing_Process.Operation_Description AS Operation_Description,
        Procesos.Nombre_Proceso AS Process_Name,
        COALESCE(ScrewDriver_Register.Descripcion, ' ') AS Screwdriver,
        COALESCE(References_PCBs.Reference, ' ') AS SubAssembly_PCB
	FROM Manufacturing_Process
		INNER JOIN Procesos ON Manufacturing_Process.idProceso = Procesos.idProceso
		INNER JOIN SubAssembly_Definition ON Manufacturing_Process.idVersion_Procesos_Manufacturng = SubAssembly_Definition.idVersion_Procesos_Manufacturing
        INNER JOIN Version_Procesos_Manufacturing ON Manufacturing_Process.idVersion_Procesos_Manufacturng = Version_Procesos_Manufacturing.idVersion_Procesos_Manufacturing
        LEFT JOIN ScrewDriver_Register ON Manufacturing_Process.idScrewdriver = ScrewDriver_Register.idScrewDriver_Register
        LEFT JOIN References_PCBs ON Manufacturing_Process.SubAssembly_AUX = References_PCBs.idReferences_PCBs;
    -- ORDER BY Manufacturing_Process.Date, Manufacturing_Process.Num;

SELECT Code_Referencia_SubAssembly, SUBSTRING_INDEX(HardwareReference, "-", -2) AS PCB_BomLayoutRework FROM SubAssembly_Definition S
INNER JOIN Hardware_Version HV ON S.idHardware_Version = HV.idHardware_Version
INNER JOIN HardwarePCBReferenceLayoutBom HR ON HV.idVersion_Parameters_HW_PCBs = HR.idVersion_Parameters_HW_PCBs;