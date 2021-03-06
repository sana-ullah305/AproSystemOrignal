USE [AprosysAccounting]
GO
/****** Object:  StoredProcedure [dbo].[GetBanksWithBalances]    Script Date: 03/08/2019 15:27:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[GetBanksWithBalances]
AS
BEGIN

SELECT 
TAB2.TreeName AS BANKNAME, TAB2.[CoaId] AS BankID, ISNULL(TAB1.AMOUNT,0) AS AMOUNT
FROM
(
SELECT
COA.[TreeName], GL.[CoaId], SUM(ISNULL(GL.[Debit],0)) - SUM(ISNULL(GL.[Credit],0)) AS AMOUNT
FROM
[Acc_GL] AS GL
INNER JOIN [Acc_COA] AS COA ON COA.[CoaId] = GL.CoaId
WHERE
GL.[IsActive] = 1 AND (GL.IsSalesCredit =0 OR GL.IsSalesCredit IS NULL)
AND (COA.[CoaId] = 11 OR COA.[PId] = 104)
GROUP BY
COA.[TreeName], GL.[CoaId]
) AS TAB1
RIGHT JOIN
(
SELECT
COA.[CoaId],COA.[TreeName]
FROM 
[Acc_COA] AS COA
WHERE
(COA.[CoaId] = 11 OR COA.[PId] = 104) aND COA.IsActive=1 
) AS TAB2
ON TAB1.CoaId = TAB2.CoaId


END
