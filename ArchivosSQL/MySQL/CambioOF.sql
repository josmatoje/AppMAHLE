
SELECT * FROM OF
where Description_OF LIKE '24004496';

SELECT * FROM Internal_Version;

select * from OF_SNs; -- 22
SELECT * FROM OF 
where Description_OF ='24001564'

SELECT * FROM OF o
inner join OF_SNs os on o.idOF = os.id_OF
inner join SN_Register_Housing h on os.idSN_Register_Housing = h.idSN_Register_Housing 
where sn like '%218644%' -- or

-- 		sn like '%219157%';

select * from SN_Register_Housing
where sn like '%218644%' or
-- 		sn like '%219157%';

SELECT * FROM OF_SNs
where idSN_Register_Housing in (
select idSN_Register_Housing from SN_Register_Housing
where sn like '%226486%');

SELECT * FROM OF;
SELECT * FROM OF_SNs OS
INNER JOIN OF O ON OS.id_OF = O.idOF
 where idSN_Register_Housing=1553;
 
-- INSERT INTO OF_SNs VALUES (308, 16);
-- INSERT INTO OF_SNs VALUES (210, 28);
-- CALL Insert_OF_SNHousing('218644', '24000790');

SELECT * FROM SN_Register_Housing H
inner join OF_SNs OS ON H.idSN_Register_Housing = OS.idSN_Register_Housing
INNER JOIN OF O ON OS.id_OF = O.idOF
 where H.sn like '%226486%' OR
		H.sn like '%226487%';
        
-- Actualizaciones a petición de Ismael
update OF_SNs set id_OF = 61
 where idSN_Register_Housing=1553;
update OF_SNs set id_OF = 61
 where idSN_Register_Housing=1554;
 
 
insert into OF_SNs values(481,22);

SELECT * FROM SN_Register_Housing
where SN LIKE '%225885';-- , '224163';
SELECT * FROM Manufacturing_Register
where idSN_Register_Housing = '653';

SELECT * FROM  Manufacturing_Register
where idSN_Register_Housing = 890;
SELECT * FROM ManufacturingRegisterFromSN
where Serial_Number = '224163';



