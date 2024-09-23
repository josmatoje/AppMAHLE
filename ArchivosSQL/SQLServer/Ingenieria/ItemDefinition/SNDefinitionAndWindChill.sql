CREATE OR ALTER VIEW SNDefinitionAndWindChill_View AS
SELECT 
	SNREG.CategoryDescription AS CategorySN,
	SNREG.CodeSN AS SerialNumber,
	SNWC.WindchillCode AS WindchillCode,
	CASE
		WHEN SNREG.CategoryDescription = 'PCB'
			THEN CONCAT_WS('-','LG', IR.Reference, CONCAT_WS('.', ISNULL(IL.Layout,0), ISNULL(IB.Bom,0), ISNULL(MRW.Rework,0))) 
		ELSE NULL
	END AS 'HardwareReference'
FROM SNWindchill AS SNWC
RIGHT JOIN (SELECT IC.CategoryDescription, SNR.IdSNRegister, SNR.CodeSN, SNPCB.IdSNRegisterPCB, SNPCB.IdItemBomPCB FROM ItemCategory AS IC
		INNER JOIN SNRegister AS SNR ON SNR.IdItemCategory = IC.IdItemCategory
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
