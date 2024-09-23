-- Elimina procedimiento anterior
DROP PROCEDURE IF EXISTS InsertOrCheckLabelling;
DELIMITER ;;
CREATE PROCEDURE InsertOrCheckLabelling (
								idInternalVersionDef INT(11), 
                                snrValue varchar(45),
								zgsValue varchar(45),
								e_q varchar(45),
								luStatus varchar(45),
								prodLoc varchar(45),
								hwNr varchar(45),
								hwStatus varchar(45),
								idSwNr varchar(45),
								swStatus varchar(45),
								daiPlant varchar(45),
								br_bl varchar(45),
								lftNo varchar(45),
								p_n varchar(45))
BEGIN
-- Precondiciones: El idInternalVersionDef recibido deberá existir en la bbdd.
-- Postcondiciones: Los errores generados por el procedimiento corresponden a las diferentes casuisticas que se pueden dar y deben ser tratados en el exterior individualmente.

	DECLARE idLabellingVersion INT(11);
	DECLARE exit handler for SQLEXCEPTION
    BEGIN
        RESIGNAL;
		ROLLBACK;
    END;
--     
--     START TRANSACTION;
    
    SELECT idLabeling_Version INTO idLabellingVersion FROM PN_InternalVersion_Definition
		WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
    
    IF idLabellingVersion IS NULL THEN
		SELECT idLabeling_Version_Def INTO idLabellingVersion FROM Labeling_Version_Def
			WHERE SNR = snrValue AND ZGS = zgsValue AND `E/Q` = e_q AND LU_Status = luStatus AND Prod_Loc = prodLoc AND 
					HW_Nr = hwNr AND HW_Status = hwStatus AND idSW_Nr = idSwNr AND SW_Status = swStatus AND 
                    DAI_Plant = daiPlant AND `BR/BL` = br_bl AND `LFT. No` = lftNo AND `P/N` = p_n;
		IF idLabellingVersion IS NULL THEN
			INSERT INTO Labeling_Version_Def (SNR, ZGS, `E/Q`, LU_Status, Prod_Loc, HW_Nr, HW_Status, idSW_Nr, SW_Status, DAI_Plant, `BR/BL`, `LFT. No`, `P/N`) 
				VALUES (snrValue, zgsValue, e_q, luStatus, prodLoc, hwNr, hwStatus, idSwNr, swStatus, daiPlant, br_bl, lftNo, p_n);
			UPDATE PN_InternalVersion_Definition SET idLabeling_Version = LAST_INSERT_ID()
			WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
		ELSE
			UPDATE PN_InternalVersion_Definition SET idLabeling_Version = idLabellingVersion
			WHERE idPN_InternalVersion_Definition = idInternalVersionDef;
		END IF;
        
	ELSEIF NOT EXISTS (SELECT idLabeling_Version_Def FROM Labeling_Version_Def
							WHERE SNR = snrValue AND ZGS = zgsValue AND `E/Q` = e_q AND LU_Status = luStatus AND Prod_Loc = prodLoc AND 
									HW_Nr = hwNr AND HW_Status = hwStatus AND idSW_Nr = idSwNr AND SW_Status = swStatus AND 
									DAI_Plant = daiPlant AND `BR/BL` = br_bl AND `LFT. No` = lftNo AND `P/N` = p_n) THEN
        SELECT '10029';
 		SIGNAL SQLSTATE '10029';
	ELSEIF (SELECT idLabeling_Version_Def FROM Labeling_Version_Def
				WHERE SNR = snrValue AND ZGS = zgsValue AND `E/Q` = e_q AND LU_Status = luStatus AND Prod_Loc = prodLoc AND 
						HW_Nr = hwNr AND HW_Status = hwStatus AND idSW_Nr = idSwNr AND SW_Status = swStatus AND 
						DAI_Plant = daiPlant AND `BR/BL` = br_bl AND `LFT. No` = lftNo AND `P/N` = p_n) != idLabellingVersion THEN
        SELECT '10030';
		SIGNAL SQLSTATE '10030';
    END IF;
END;;

CALL InsertOrCheckLabelling(60, 'HWE0910700o', 'ea', 'E001', 'L001', 'ESP', '-', '-', '-', '-', '601', 'D5599', '15538010', '01041597');
SELECT * FROM PN_InternalVersion_Definition;
SELECT * FROM Labeling_Version_Def;
