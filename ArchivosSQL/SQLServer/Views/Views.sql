USE XEDI_TraceabilityDB
GO

--Rol views
CREATE OR ALTER VIEW Center_RolDefinitions_View AS
SELECT C.*, D.RolDescription FROM Center AS C
INNER JOIN RolDefinition_Center AS DC ON C.IdCenter = DC.IdCenter
INNER JOIN RolDefinition AS D ON DC.IdRol = D.IdRol
GO

SELECT * FROM Center_RolDefinitions_View
GO

CREATE OR ALTER VIEW HardwarePCBReferenceLayoutBom AS
	SELECT DISTINCT ID.InternalName, ILBR.IdItemLayoutBomRework_PCB, CONCAT_WS('-','LG', IR.Reference, CONCAT_WS('.', ISNULL(IL.Layout,0), ISNULL(IB.Bom,0), ISNULL(ILBR.ItemRework,0))) AS 'HardwareReference', ILBR.Windchill  FROM ItemReferencePCB AS IR
	INNER JOIN ItemLayoutPCB AS IL ON IR.IdItemReferencePCB = IL.IdItemReference
	INNER JOIN ItemBomPCB AS IB ON IL.IdItemLayoutPCB = IB.IdItemLayoutPCB
	INNER JOIN ItemLayoutBomRework_PCB AS ILBR ON IB.IdItemBomPCB = ILBR.IdItemBomPCB
	INNER JOIN ItemDefinition_PCBLayoutBomRework AS IDLBR ON ILBR.IdItemLayoutBomRework_PCB = IDLBR.IdItemLayoutBomRework_PCB
	INNER JOIN ItemDefinition AS ID ON IDLBR.IdItemDefinition = ID.IdItemDefinition
GO

SELECT * FROM HardwarePCBReferenceLayoutBom
GO

CREATE OR ALTER VIEW HardwarePCBParameters AS
	SELECT DISTINCT ID.InternalName,  IR.Reference,IL.Layout,IB.Bom, ILBR.ItemRework, ILBR.Windchill  FROM ItemReferencePCB AS IR
	INNER JOIN ItemLayoutPCB AS IL ON IR.IdItemReferencePCB = IL.IdItemReference
	INNER JOIN ItemBomPCB AS IB ON IL.IdItemLayoutPCB = IB.IdItemLayoutPCB
	INNER JOIN ItemLayoutBomRework_PCB AS ILBR ON IB.IdItemBomPCB = ILBR.IdItemBomPCB
	INNER JOIN ItemDefinition_PCBLayoutBomRework AS IDLBR ON ILBR.IdItemLayoutBomRework_PCB = IDLBR.IdItemLayoutBomRework_PCB
	INNER JOIN ItemDefinition AS ID ON IDLBR.IdItemDefinition = ID.IdItemDefinition
GO

SELECT * FROM HardwarePCBParameters
GO

-------------------------VISTA REGISTRO PCB DIV

CREATE OR ALTER VIEW PCBDefinitionListFromProyect_View AS
	SELECT DISTINCT IPJ.ProjectName, 
	IB.IdItemBomPCB AS IdItemBomPCB,
	CONCAT_WS('-','LG', IR.Reference, CONCAT_WS('.', ISNULL(IL.Layout,0), ISNULL(IB.Bom,0), 0)) AS 'HardwareReference', 
	CONCAT_WS(' - ','LG', IR.Reference, IR.ItemDescription) AS 'ReferenceName' ,
	CONCAT_WS('.', ISNULL(IL.Layout,0), ISNULL(IB.Bom,0)) AS 'LayoutBOM'
	FROM ItemReferencePCB AS IR
	
	INNER JOIN ItemLayoutPCB AS IL ON IR.IdItemReferencePCB = IL.IdItemReference
	INNER JOIN ItemBomPCB AS IB ON IL.IdItemLayoutPCB = IB.IdItemLayoutPCB
	INNER JOIN ItemLayoutBomRework_PCB AS ILBR ON IB.IdItemBomPCB = ILBR.IdItemBomPCB
	INNER JOIN ItemDefinition_PCBLayoutBomRework AS IDLBR ON ILBR.IdItemLayoutBomRework_PCB = IDLBR.IdItemLayoutBomRework_PCB
	INNER JOIN ItemDefinition AS ID ON IDLBR.IdItemDefinition = ID.IdItemDefinition
	INNER JOIN ItemPartNumber AS IPN ON ID.IdItemPN =IPN.IdItemPN
	INNER JOIN ItemSample AS  ISP ON IPN.IdItemSample = ISP.IdItemSample
	INNER JOIN ItemProject AS IPJ ON ISP.IdItemProject = IPJ.IdItemProject
GO

SELECT * FROM PCBDefinitionListFromProyect_View
GO
-----------VISTA SN-DEFINICION-CodigoWindchill
CREATE OR ALTER VIEW SNDefinitionAndWindChill_View AS
SELECT 
	SNREG.ProjectName AS ProjectName,
	SNREG.CategoryDescription AS CategorySN,
	SNREG.IdSNRegister AS IdSNRegister,
	SNREG.CodeSN AS CodeSN,
	SNWC.WindchillCode AS WindchillCode,
	SNREG.Batch AS Batch,
	CASE
		WHEN SNREG.CategoryDescription = 'PCB'
			THEN CONCAT_WS('-','LG', IR.Reference, CONCAT_WS('.', ISNULL(IL.Layout,0), ISNULL(IB.Bom,0), ISNULL(MRW.Rework,0))) 
		ELSE NULL
	END AS 'HardwareReference'
FROM SNWindchill AS SNWC
RIGHT JOIN (SELECT IPJ.ProjectName, IC.CategoryDescription, SNR.IdSNRegister, SNR.CodeSN, SNPCB.Batch, SNPCB.IdSNRegisterPCB, SNPCB.IdItemBomPCB FROM ItemCategory AS IC
		INNER JOIN SNRegister AS SNR ON SNR.IdItemCategory = IC.IdItemCategory
		INNER JOIN ItemProject AS IPJ ON SNR.IdItemProject = IPJ.IdItemProject
		LEFT JOIN SNRegisterPCB AS SNPCB ON SNR.IdSNRegister = SNPCB.IdSNRegister) AS SNREG ON SNREG.IdSNRegister = SNWC.IdSNRegister
LEFT JOIN ItemBomPCB AS IB ON SNREG.IdItemBomPCB = IB.IdItemBomPCB
LEFT JOIN ItemLayoutPCB AS IL ON IB.IdItemLayoutPCB = IL.IdItemLayoutPCB
LEFT JOIN ItemReferencePCB AS IR ON IL.IdItemReference = IR.IdItemReferencePCB
LEFT JOIN (SELECT IdSNRegisterPCB, MAX(Rework) as Rework FROM ManufacturingReworkPCB
			GROUP BY IdSNRegisterPCB) AS MRW ON SNREG.IdSNRegisterPCB = MRW.IdSNRegisterPCB
GO

SELECT * FROM SNDefinitionAndWindChill_View
GO 
(SELECT IdSNRegisterPCB, MAX(Rework) as Rework FROM ManufacturingReworkPCB
GROUP BY IdSNRegisterPCB)
GO

--------------Views Reworks from Reference.Layout.BOM

CREATE OR ALTER VIEW ReworkFromReferenceProject_View AS
	SELECT 
		RDLB.ProjectName AS ProjectName,
		RDLB.ReferenceDescription AS ReferenceDescription,
		RDLB.HardwareReference,
		RD.ReworkDescription AS ReworkDescription,
		IBR.IdItemBomPCB AS IdBOM,
		RD.ReworkNum AS Rework
	FROM ReworkDefinitionPCB AS RD
	INNER JOIN ItemBomPCB_ReworkDefinitionPCB AS IBR ON RD.IdReworkDefinitionPCB = IBR.IdReworkDefinitionPCB
	INNER JOIN 
	(SELECT IPJ.ProjectName AS ProjectName, ITR.ItemDescription AS ReferenceDescription, CONCAT_WS('-','LG', ITR.Reference, CONCAT_WS('.', ISNULL(ILPCB.Layout,0), ISNULL(IBPCB.Bom,0))) AS 'HardwareReference', IBPCB.IdItemBomPCB AS IdBomPCB
		FROM ItemReferencePCB AS ITR
		INNER JOIN ItemProject as IPJ ON ITR.IdItemProject = ITR.IdItemProject
		INNER JOIN ItemLayoutPCB AS ILPCB ON ITR.IdItemReferencePCB = ILPCB.IdItemReference
		INNER JOIN ItemBomPCB AS IBPCB ON ILPCB.IdItemLayoutPCB = IBPCB.IdItemLayoutPCB
	) AS RDLB ON IBR.IdItemBomPCB = RDLB.IdBomPCB
GO

SELECT Rework FROM ReworkFromReferenceProject_View WHERE IdBOM=5 order by ReferenceDescription, Rework 
GO


-- Unión de referencia para las PCBs

--CREATE OR ALTER VIEW ListIdsPCB_View AS
--SELECT DISTINCT IR.IdItemProject,
--IR.IdItemReferencePCB,
--IL.IdItemLayoutPCB,
--IB.IdItemBomPCB
--FROM ItemReferencePCB AS IR
--INNER JOIN ItemLayoutPCB AS IL ON IR.IdItemReferencePCB = IL.IdItemReference
--INNER JOIN ItemBomPCB AS IB ON IL.IdItemLayoutPCB = IB.IdItemLayoutPCB
--LEFT JOIN ItemLayoutBomRework_PCB AS ILBR ON IB.IdItemBomPCB = ILBR.IdItemBomPCB
--GO
--SELECT * FROM ListIdsPCB_View
--GO

-----------
-- INNER JOIN ItemLayoutPCB AS IL ON IR.IdItemReferencePCB = IL.IdItemReference
-- INNER JOIN ItemBomPCB AS IB ON IL.IdItemLayoutPCB = IB.IdItemLayoutPCB
-- INNER JOIN ItemLayoutBomRework_PCB AS ILBR ON IB.IdItemBomPCB = ILBR.IdItemBomPCB
-- INNER JOIN ItemDefinition_PCBLayoutBomRework AS IDLBR ON ILBR.IdItemLayoutBomRework_PCB = IDLBR.IdItemLayoutBomRework_PCB
-- INNER JOIN ItemDefinition AS ID ON IDLBR.IdItemDefinition = ID.IdItemDefinition
-- GO
-----------------OF/SN

 CREATE OR ALTER VIEW OrderFabrication_SN_View AS
 SELECT OFN.IdOF AS IdOF, OFN.CodeOF AS CodeOF, SNR.CodeSN AS CodeSN, ID.IdItemDefinition, ID.InternalName
 FROM OrderFabrication_SNRegister AS OFSN
 INNER JOIN OrderFabrication AS OFN ON OFN.IdOF = OFSN.IdOF
  INNER JOIN ItemDefinition AS ID ON OFN.IdItemDefinition = ID.IdItemDefinition
 INNER JOIN SNRegister AS SNR ON OFSN.IdSNRegister = SNR.IdSNRegister
 GO
SELECT * FROM OrderFabrication_SN_View  
GO

-----------------------------------------------------

 CREATE OR ALTER VIEW Center_Station_View AS
SELECT CR.HostName, CR.ComputerName , STN.StationName, CRSN.Zocalo
FROM Center CR
INNER JOIN Center_Station CRSN ON CR.IdCenter = CRSN.IdCenter
INNER JOIN Station STN ON CRSN.IdStation = STN.IdStation

 GO
SELECT ComputerName, Zocalo, StationName 
FROM Center_Station_View  
WHERE HostName='ESMPS40308'
GO
-- Unión de referencia para las PCBs

CREATE OR ALTER VIEW ManufacturingProcessFromProduct_View AS
SELECT 
 ID.IdItemDefinition,
 ID.InternalName,
 VP.ProcessDesignation,
 PD.IdProcessDefinition,
 PD.Num,
 PD.ProcessNum,
 PDN.ProcessDescription,
 PD.Operation,
 PS.PictureName,
 PD.OperationDescription,
 SPS.StationName,
 ISNULL(PTD.TestDescription,'') AS TestDescription,
 ISNULL(PTI.InputType,'') AS InputType,
 ISNULL(PTS.ScrewCode,'') AS ScrewCode,
 ISNULL(PIM.Reference,'') AS Reference
FROM ItemDefinition ID
INNER JOIN ItemDefinition_VersionProcess IDVP ON ID.IdItemDefinition = IDVP.IdItemDefinition
INNER JOIN VersionProcess VP ON IDVP.IdVersionProcess = VP.IdVersionProcess
INNER JOIN ProcessDefinition PD ON VP.IdVersionProcess = PD.IdVersionProcess
INNER JOIN PictureStorage PS ON PD.IdPicture = PS.IdPictureStorage
LEFT JOIN ProcessDescription PDN ON PD.IdProcessDescription = PDN.IdProcessDescription
INNER JOIN (
	SELECT PSD.IdProcessDefinition, ST.StationName FROM Station ST
	INNER JOIN Process_Station_Definition PSD ON ST.IdStation = PSD.IdStation) AS SPS
	on PD.IdProcessDefinition = SPS.IdProcessDefinition
LEFT JOIN (
	SELECT PT.TestDescription, PDNT.IdProcessDefinition FROM ProcessTest PT
	INNER JOIN Process_Test_Definition PTD ON PT.IdProcessTest = PTD.IdProcessTest
	INNER JOIN ProcessDefinition PDNT ON PTD.IdProcessDefinition = PDNT.IdProcessDefinition) AS PTD
	ON PD.IdProcessDefinition = PTD.IdProcessDefinition
LEFT JOIN (
	SELECT PD.ScrewCode, PDSN.IdProcessDefinition FROM ProcessScrewDriver PD
	INNER JOIN Process_ScrewDriver_Definition PSD ON PD.IdProcessScrewDriver = PSD.IdProcessScrewDriver
	INNER JOIN ProcessDefinition PDSN ON PSD.IdProcessDefinition = PDSN.IdProcessDefinition) AS PTS
	ON PD.IdProcessDefinition = PTS.IdProcessDefinition
LEFT JOIN (
	SELECT PIT.InputType, PDSI.IdProcessDefinition FROM ProcessInput PIT
	INNER JOIN Process_Input_Definition PID on PIT.IdProcessInput = PID.IdProcessInput
	INNER JOIN ProcessDefinition PDSI ON PID.IdProcessDefinition = PDSI.IdProcessDefinition) AS PTI
	ON PD.IdProcessDefinition = PTI.IdProcessDefinition
LEFT JOIN (
	SELECT PIM.Reference, PD.IdProcessDefinition FROM ProcessItem PIM
	INNER JOIN Process_Item_Definition PIDN ON PIM.IdProcessItem = PIDN.IdProcessItem
	INNER JOIN ProcessDefinition PD ON PIDN.IdProcessDefinition = PD.IdProcessDefinition)AS PIM
	ON PD.IdProcessDefinition = PIM.IdProcessDefinition
GO

SELECT * FROM ManufacturingProcessFromProduct_View;
GO
---------------------------------------------

CREATE OR ALTER VIEW OrderFabrication_View AS
SELECT    
	PJD.ProjectName, 
	PJD.DescriptionItem,
	PJD.IdItemDefinition,
	PJD.InternalName, 
	PJD.DescriptionReference, 
	PJD.PictureName,
	PDS.ProcessDesignation, 
	OFN.IdOF, 
	OFN.CodeOF, 
	OFN.DescritionOF, 
	OFN.Quantity, 
	PJD.IsTrazability,
	PJD.EnableItem ,
	OFN.DateRegistration
FROM    OrderFabrication AS OFN 
	INNER JOIN (SELECT ID.IdItemDefinition, IPJ.ProjectName, PS.PictureName, ID.InternalName, ID.DescriptionReference,IDPN.DescriptionItem, ID.EnableItem, ID.IsTrazability
		FROM ItemDefinition AS ID
		INNER JOIN ItemPartNumber AS IPN ON ID.IdItemPN =IPN.IdItemPN
		INNER JOIN ItemSample AS  ISP ON IPN.IdItemSample = ISP.IdItemSample
		INNER JOIN ItemProject AS IPJ ON ISP.IdItemProject = IPJ.IdItemProject
		INNER JOIN ItemDescription IDPN ON ID.IdItemDescription = IDPN.IdItemDescription
		LEFT JOIN PictureStorage PS ON ID.IdPictureStorage = PS.IdPictureStorage
		) AS PJD ON  OFN.IdItemDefinition = PJD.IdItemDefinition
	INNER JOIN (SELECT IDN.IdItemDefinition, VP.ProcessDesignation FROM VersionProcess VP
		INNER JOIN ItemDefinition_VersionProcess IDVP ON VP.IdVersionProcess = IDVP.IdVersionProcess
		INNER JOIN ItemDefinition IDN ON IDVP.IdItemDefinition = IDN.IdItemDefinition
		) AS PDS ON OFN.IdItemDefinition = PDS.IdItemDefinition
GO

SELECT OFV.CodeOF, OFV.ProjectName, OFV.DescriptionItem, OFV.InternalName, OFV.DescriptionReference, OFV.ProjectName, OFV.ProcessDesignation, OFV.EnableItem, OFV.IsTrazability
FROM OrderFabrication_View OFV
WHERE OFV.CodeOF='498498'
GO 

SELECT * FROM OrderFabrication_View
GO 

-- vista ids
CREATE OR ALTER VIEW ItemDefinitionParameters_View AS
SELECT  ID.IdItemDefinition, ISNULL(SH.[Nº Hardware],0) AS [Nº Hardware], ISNULL(SM.[Nº Mechanical],0) AS [Nº Mechanical], ISNULL(SS.[Nº Software],0) AS [Nº Software], ISNULL(SP.[Nº Process],0) AS [Nº Process], ISNULL(SL.[Nº Labelling],0) AS [Nº Labelling], ISNULL(ST.[Nº Testing],0) AS [Nº Testing] FROM ItemDefinition ID
LEFT JOIN (
	SELECT  ID.IdItemDefinition, COUNT(*) AS [Nº Hardware]
	FROM ItemDefinition ID
	INNER JOIN UnifiedHardwarePCB IDVH ON ID.IdItemDefinition = IDVH.IdItemDefinition
	GROUP BY ID.IdItemDefinition) SH ON ID.IdItemDefinition = SH.IdItemDefinition
LEFT JOIN (
	SELECT  ID.IdItemDefinition, COUNT(*) AS [Nº Mechanical]
	FROM ItemDefinition ID
	INNER JOIN ItemDefinition_VersionMechanical IDVM ON ID.IdItemDefinition = IDVM.IdItemDefinition
	GROUP BY ID.IdItemDefinition) SM ON ID.IdItemDefinition = SM.IdItemDefinition
LEFT JOIN (
	SELECT  ID.IdItemDefinition, COUNT(*) AS [Nº Software]
	FROM ItemDefinition ID
	INNER JOIN ItemDefinition_VersionSoftware IDVS ON ID.IdItemDefinition = IDVS.IdItemDefinition
	GROUP BY ID.IdItemDefinition) SS ON ID.IdItemDefinition = SS.IdItemDefinition
LEFT JOIN (
	SELECT  ID.IdItemDefinition, COUNT(*) AS [Nº Process]
	FROM ItemDefinition ID
	INNER JOIN ItemDefinition_VersionProcess IDVP ON ID.IdItemDefinition = IDVP.IdItemDefinition
	GROUP BY ID.IdItemDefinition) SP ON ID.IdItemDefinition = SP.IdItemDefinition
LEFT JOIN (
	SELECT  ID.IdItemDefinition, COUNT(*) AS [Nº Labelling]
	FROM ItemDefinition ID
	INNER JOIN ItemDefinition_VersionLabelling IDVL ON ID.IdItemDefinition = IDVL.IdItemDefinition
	GROUP BY ID.IdItemDefinition) SL ON ID.IdItemDefinition = SL.IdItemDefinition
LEFT JOIN (
	SELECT  ID.IdItemDefinition, COUNT(*) AS [Nº Testing]
	FROM ItemDefinition ID
	INNER JOIN ItemDefinition_ProcessTest IDVT ON ID.IdItemDefinition = IDVT.IdItemDefinition
	GROUP BY ID.IdItemDefinition) ST ON ID.IdItemDefinition = ST.IdItemDefinition
GO

SELECT * FROM ItemDefinitionParameters_View

GO
-------------------------ManufacturingRegisterFromSN

CREATE OR ALTER VIEW ManufacturingRegisterFromSN AS
SELECT 
MRSN.IdManufacturingRegister,
SNR.CodeSN ,
IPJ.ProjectName,
OFSN.CodeOF,
OFSN.IdItemDefinition,
OFSN.InternalName,
MRSN.Num, 
MRSN.Coment, 
MRSN.ScrewDescription, 
MRSN.ScrewData, 
MRSN.Lote, 
MRSN.InputDescription, 
MRSN.InputData,
MRSN.HostName, 
MRSN.StationName, 
MRSN.TypeManufacturingResult,
MRSN.UserName, 
MRSN.DateStart, 
MRSN.DateEnd
FROM SNRegister SNR
INNER JOIN OrderFabrication_SN_View OFSN ON SNR.CodeSN = OFSN.CodeSN
INNER JOIN ItemProject IPJ ON SNR.IdItemProject = IPJ.IdItemProject
INNER JOIN 
(
	SELECT 
	  MRR.IdManufacturingRegister, MRR.IdSNRegister, MRR.Num, MRC.Coment, MRSN.CodeSN, MRS.ScrewDescription, MRS.ScrewData, MI.Lote, MRI.InputDescription, MRI.InputData,
	  CN.HostName, MPOP.StationName, MR.TypeManufacturingResult,MRU.UserName, MRR.DateStart, MRR.DateEnd
	FROM ManufacturingRegister MRR
	INNER JOIN 
	(
		SELECT MPFPV.Num, MPFPV.StationName  FROM SNRegister SRR
		INNER JOIN OrderFabrication_SN_View OFSNV ON SRR.CodeSN = OFSNV.CodeSN
		INNER JOIN ManufacturingProcessFromProduct_View MPFPV ON OFSNV.IdItemDefinition = MPFPV.IdItemDefinition
	) AS MPOP ON MRR.Num = MPOP.Num
	INNER JOIN ManufacturingUser MRU ON MRR.IdManufacturingUser = MRU.IdManufacturingUser
	INNER JOIN ManufacturingResult MR ON MRR.IdManufacturingResult = MR.IdManufacturingResult
	INNER JOIN Center CN ON MRR.IdCenter = CN.IdCenter
	LEFT JOIN (
			SELECT MRS.IdManufacturingRegister, MPS.ScrewDescription, MS.ScrewData FROM ManufacturingParameterScrew MPS
			INNER JOIN ManufacturingScrew MS ON MPS.IdManufacturingParameterScrew = MS.IdManufacturingParameterScrew
			INNER JOIN ManufacturingRegister_Screw MRS ON MS.IdManufacturingScrew = MRS.IdManufacturingScrew
			) AS MRS ON MRR.IdManufacturingRegister = MRS.IdManufacturingRegister
	LEFT JOIN (
			SELECT MRSNN.IdManufacturingRegister, SRRN.CodeSN FROM ManufacturingSN MSN
			INNER JOIN SNRegister SRRN ON MSN.IdSNRegister = SRRN.IdSNRegister
			INNER JOIN ManufacturingRegister_SN MRSNN ON MSN.IdManufacturingSN = MRSNN.IdManufacturingSN
			) AS MRSN ON MRR.IdManufacturingRegister =MRSN.IdManufacturingRegister
	LEFT JOIN (
			SELECT MRI.IdManufacturingRegister, MI.Lote FROM ManufacturingItem MI
			INNER JOIN ManufacturingRegister_Item MRI ON MI.IdManufacturingItem = MRI.IdManufacturingItem
			) AS MI ON MRR.IdManufacturingRegister =MI.IdManufacturingRegister
	LEFT JOIN (
			SELECT MRI.IdManufacturingRegister, MPI.InputDescription, MI.InputData FROM ManufacturingParameterInput MPI 
			INNER JOIN ManufacturingInput MI ON MPI.IdManufacturingParameterInput = MI.IdManufacturingParameterInput
			INNER JOIN ManufacturingRegister_Input MRI ON MI.IdManufacturingInput = MRI.IdManufacturingInput
			) AS MRI ON MRR.IdManufacturingRegister = MRI.IdManufacturingRegister
	LEFT JOIN 
			(
			SELECT MRC.IdManufacturingRegister, MC.Coment FROM ManufacturingComent MC
			INNER JOIN ManufacturingRegister_Coment MRC ON MC.IdManufacturingComent = MRC.IdManufacturingComent
			) AS MRC ON MRR.IdManufacturingRegister = MRC.IdManufacturingRegister
) AS MRSN ON SNR.IdSNRegister = MRSN.IdSNRegister
GO 

SELECT TOP 1 ISNULL(MRFSN.ProjectName,'') AS ProjectName,ISNULL(MRFSN.CodeOF,'') AS CodeOF,ISNULL(MRFSN.InternalName,'') AS InternalName,ISNULL(MRFSN.Num,'') AS OperationNum,ISNULL(MRFSN.Coment,'') AS Coment,
ISNULL(MRFSN.ScrewData,'') AS ScrewData ,ISNULL(MRFSN.Lote,'') AS Lote,ISNULL(MRFSN.InputData,'') AS InputData,ISNULL(MRFSN.HostName,'') AS HostName,ISNULL(MRFSN.TypeManufacturingResult,'') AS Result,ISNULL(MRFSN.UserName,'') AS UserName,ISNULL(MRFSN.DateStart,'') AS DateStart,ISNULL(MRFSN.DateEnd,'') AS DateEnd
FROM ManufacturingRegisterFromSN MRFSN WHERE MRFSN.CodeSN='123456'
ORDER BY MRFSN.IdManufacturingRegister DESC

SELECT * FROM ManufacturingRegisterFromSN
SELECT * FROM ManufacturingRegister
GO



-------------------------ManufacturingRegisterFromSN

CREATE OR ALTER VIEW ManufacturingOPPendingFromSN_View AS
SELECT SNP.IdSNPendingOperation ,SNR.CodeSN,SNP.NumPending  FROM SNRegister SNR
INNER JOIN SNPendingOperationManufacturing SNP ON SNR.IdSNRegister = SNP.IdSNRegister
GO

SELECT * FROM ManufacturingOPPendingFromSN_View  ORDER BY id
GO
 SELECT count(*) FROM ManufacturingOPPendingFromSN_View WHERE CodeSN = '123456' ORDER BY NumPending
 SELECT * FROM ManufacturingOPPendingFromSN_View WHERE CodeSN = '123456' ORDER BY NumPending