USE [AprosysAccounting]
GO

/****** Object:  StoredProcedure [dbo].[Report_WeeklyCashFlow]    Script Date: 26/05/2019 12:21:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[Report_WeeklyCashFlow]
(
	@ActivityTime Datetime
	
)
AS
BEGIN
DECLARE @reportStartDate datetime,@reportEndDate datetime
SET @reportStartDate= DATEADD(DAY, 2 - DATEPART(WEEKDAY, @ActivityTime), CAST(@ActivityTime AS DATE)) 
SET @reportEndDate= DATEADD(DAY, 9 - DATEPART(WEEKDAY, @ActivityTime), CAST(@ActivityTime AS DATE)) 
        
SELECT TBWeeklyCashFlow.TYPE ,MON,TUE,WED,THU,FRI,SAT,SUN,TOTAL,TYPED,Sort
FROM 
(        
       
SELECT	
	TBCashInSales.[TYPE], SUM(TBCashInSales.TOTAL) AS MON,0 AS TUE,0 AS WED,0 AS THU,0 AS FRI,0 AS SAT,0 AS SUN, SUM(TBCashInSales.TOTAL) AS TOTAL, 1 AS TYPED ,1 'Sort'
FROM(

SELECT 
	'Previous Week' AS [TYPE],ISNULL(SUM(GL.[Debit]),0)-ISNULL(SUM(GL.[Credit]),0) AS TOTAL
FROM 
	[Acc_GL] AS GL
WHERE 
	GL.[CoaId] = 11 AND GL.[isactive] = 1 --AND GL.TranTypeId <>4
	--AND (GL.IsSalesCredit=0 OR GL.IsSalesCredit is NULL) AND (GL.[ActivityTimestamp]<@reportStartDate)
	AND ((GL.IsSalesCredit=0 OR GL.IsSalesCredit is NULL) AND GL.[ActivityTimestamp] < @reportStartDate AND GL.CreditPaidDate is null)

UNION ALL

SELECT 
	'Previous Week' AS [TYPE],ISNULL(SUM(GL.[Debit]),0)-ISNULL(SUM(GL.[Credit]),0) AS TOTAL
FROM 
	[Acc_GL] AS GL
WHERE 
	GL.[CoaId] = 11 AND GL.[isactive] = 1 --AND GL.TranTypeId <>4 and GL.CoaId  not in (30,119)
	--AND GL.IsSalesCredit=0 AND (GL.[ActivityTimestamp]<@reportStartDate)
	AND (GL.IsSalesCredit=0 AND GL.CreditPaidDate < @reportStartDate )

UNION ALL

SELECT 
	'Previous Week' AS [TYPE],ISNULL(SUM(Amount),0) AS TOTAL
FROM 
	[Acc_PartialCredit] AS GL
WHERE 
	GL.Deleted=0 AND GL.CreatedDate < @reportStartDate 

) AS TBCashInSales
GROUP BY
	TBCashInSales.[TYPE]

UNION


/*========================================SALES================================================================================*/
SELECT 
	'Cash In Sales' AS TYPE,ISNULL(SUM(TABSALEX.MON),0) AS MON,ISNULL(SUM(TABSALEX.TUE),0) AS TUE,ISNULL(SUM(TABSALEX.WED),0) AS WED,ISNULL(SUM(TABSALEX.THU),0) AS THU,
	ISNULL(SUM(TABSALEX.FRI),0) AS FRI,ISNULL(SUM(TABSALEX.SAT),0) AS SAT,ISNULL(SUM(TABSALEX.SUN),0) AS SUN,ISNULL(SUM(TABSALEX.TOTAL),0) AS TOTAL, 1 AS TYPED,3 'Sort'
FROM(

SELECT 
	--'Cash In Sales' AS TYPE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 0 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS MON,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 1 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS TUE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 2 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS WED,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 3 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS THU,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 4 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS FRI,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 5 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS SAT,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 6 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS SUN,
	SUM(GL.[Credit])+SUM([TaxPercent]) AS TOTAL,
	1 AS TYPED
FROM 
	[Acc_GL] AS GL
WHERE
	GL.[CoaId] = 14 AND GL.[TranTypeId] = 2 AND GL.[isactive] = 1
	AND (GL.IsSalesCredit=0 AND GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate AND GL.CreditPaidDate is null)
	GROUP BY
	DATEDIFF(dd, @reportStartDate, [ActivityTimestamp])
	
UNION ALL

SELECT 
	--'Cash In Sales' AS TYPE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreditPaidDate) = 0 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS MON,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [CreditPaidDate]) = 1 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS TUE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreditPaidDate) = 2 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS WED,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreditPaidDate) = 3 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS THU,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreditPaidDate) = 4 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS FRI,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreditPaidDate) = 5 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS SAT,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreditPaidDate) = 6 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS SUN,
	SUM(GL.[Credit])+SUM([TaxPercent]) AS TOTAL,
	1 AS TYPED
FROM 
	[Acc_GL] AS GL
WHERE
	GL.[CoaId] = 14 AND GL.[TranTypeId] = 2 AND GL.[isactive] = 1
	AND (GL.IsSalesCredit=0 AND GL.CreditPaidDate >= @reportStartDate AND GL.CreditPaidDate<@reportEndDate)
	GROUP BY
	IsSalesCredit,CreditPaidDate
	--DATEDIFF(dd, @reportStartDate, CreditPaidDate),IsSalesCredit	
) AS TABSALEX

--GROUP BY
--	TABSALEX.[TYPE]
/*===========================================CASHOUT===========================================================================*/	
UNION

SELECT 
	--TBCASHOUT.[TYPE]
	 'Cash Out'  AS Type,-ISNULL(SUM(TBCASHOUT.MON),0) AS MON,-ISNULL(SUM(TBCASHOUT.TUE),0) AS TUE,-ISNULL(SUM(TBCASHOUT.WED),0) AS WED,-ISNULL(SUM(TBCASHOUT.THU),0) AS THU,
	-ISNULL(SUM(TBCASHOUT.FRI),0) AS FRI,-ISNULL(SUM(TBCASHOUT.SAT),0) AS SAT,-ISNULL(SUM(TBCASHOUT.SUN),0) AS SUN,-ISNULL(SUM(TBCASHOUT.TOTAL),0) AS TOTAL, 1 AS TYPED,5 'Sort'
FROM(
SELECT 
	--'Cash Out' AS TYPE ,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 0 THEN SUM(GL.[Credit]) ELSE 0 END AS MON,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 1 THEN SUM(GL.[Credit]) ELSE 0 END AS TUE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 2 THEN SUM(GL.[Credit]) ELSE 0 END AS WED,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 3 THEN SUM(GL.[Credit]) ELSE 0 END AS THU,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 4 THEN SUM(GL.[Credit]) ELSE 0 END AS FRI,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 5 THEN SUM(GL.[Credit]) ELSE 0 END AS SAT,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 6 THEN SUM(GL.[Credit]) ELSE 0 END AS SUN,
	SUM(GL.[Credit]) AS TOTAL
FROM 
	[Acc_GL] AS GL
WHERE 
	GL.[CoaId] = 11 AND GL.[TranTypeId] IN (1,4) AND GL.[isactive] = 1
	AND (GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
GROUP BY
	DATEDIFF(dd, @reportStartDate, [ActivityTimestamp])
) AS TBCASHOUT
--GROUP BY
--	TBCASHOUT.[TYPE]

UNION

SELECT 
	--TBCASHOUT.[TYPE]
	'Deposit' AS TYPE,ISNULL(SUM(TBDeposit.MON),0) AS MON,ISNULL(SUM(TBDeposit.TUE),0) AS TUE,ISNULL(SUM(TBDeposit.WED),0) AS WED,ISNULL(SUM(TBDeposit.THU),0) AS THU,
	ISNULL(SUM(TBDeposit.FRI),0) AS FRI,ISNULL(SUM(TBDeposit.SAT),0) AS SAT,ISNULL(SUM(TBDeposit.SUN),0) AS SUN,ISNULL(SUM(TBDeposit.TOTAL),0) AS TOTAL, 1 AS TYPED,6 'Sort'
FROM(
SELECT 
	--'Deposit' AS TYPE ,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 0 THEN SUM(ISNULL(GL.[Debit],0))- SUM(ISNULL(GL.[Credit],0)) ELSE 0 END AS MON,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 1 THEN SUM(ISNULL(GL.[Debit],0))- SUM(ISNULL(GL.[Credit],0)) ELSE 0 END AS TUE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 2 THEN SUM(ISNULL(GL.[Debit],0))- SUM(ISNULL(GL.[Credit],0)) ELSE 0 END AS WED,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 3 THEN SUM(ISNULL(GL.[Debit],0))- SUM(ISNULL(GL.[Credit],0))ELSE 0 END AS THU,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 4 THEN SUM(ISNULL(GL.[Debit],0))- SUM(ISNULL(GL.[Credit],0)) ELSE 0 END AS FRI,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 5 THEN SUM(ISNULL(GL.[Debit],0))- SUM(ISNULL(GL.[Credit],0)) ELSE 0 END AS SAT,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 6 THEN SUM(ISNULL(GL.[Debit],0))- SUM(ISNULL(GL.[Credit],0)) ELSE 0 END AS SUN,
	SUM(ISNULL(GL.[Debit],0))- SUM(ISNULL(GL.[Credit],0)) AS TOTAL
FROM 
	[Acc_GL] AS GL
WHERE 
	GL.[CoaId] = 11 AND GL.[TranTypeId] IN(6) AND GL.[isactive] = 1
	AND (GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
GROUP BY
	DATEDIFF(dd, @reportStartDate, [ActivityTimestamp])
) AS TBDeposit
--GROUP BY
--	TBDeposit.[TYPE]
	
	
UNION


SELECT 
	--TBCashInMONTHLY.[TYPE]
	'Cash in Monthly' AS TYPE ,ISNULL(SUM(TBCashInMONTHLY.MON),0) AS MON,ISNULL(SUM(TBCashInMONTHLY.TUE),0) AS TUE,ISNULL(SUM(TBCashInMONTHLY.WED),0) AS WED,ISNULL(SUM(TBCashInMONTHLY.THU),0) AS THU,
	ISNULL(SUM(TBCashInMONTHLY.FRI),0) AS FRI,ISNULL(SUM(TBCashInMONTHLY.SAT),0) AS SAT,ISNULL(SUM(TBCashInMONTHLY.SUN),0) AS SUN,ISNULL(SUM(TBCashInMONTHLY.TOTAL),0) AS TOTAL, 1 AS TYPED,2 'Sort'
FROM(
SELECT 
	--'Cash in Monthly' AS TYPE ,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 0 THEN SUM(GL.[Debit]) ELSE 0 END AS MON,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 1 THEN SUM(GL.[Debit]) ELSE 0 END AS TUE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 2 THEN SUM(GL.[Debit]) ELSE 0 END AS WED,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 3 THEN SUM(GL.[Debit]) ELSE 0 END AS THU,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 4 THEN SUM(GL.[Debit]) ELSE 0 END AS FRI,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 5 THEN SUM(GL.[Debit]) ELSE 0 END AS SAT,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 6 THEN SUM(GL.[Debit]) ELSE 0 END AS SUN,
	SUM(GL.[Debit]) AS TOTAL
FROM 
	[Acc_GL] AS GL
WHERE 
	GL.[CoaId] = 11 AND GL.[TranTypeId] = 3 AND GL.[IsPostpaid] = 1 AND GL.[isactive] = 1
	AND (GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
GROUP BY
	DATEDIFF(dd, @reportStartDate, [ActivityTimestamp])
) AS TBCashInMONTHLY
--GROUP BY
--	TBCashInMONTHLY.[TYPE]
	
UNION

/*=========================================Cash IN SHOP=====================================================*/
SELECT
	'Cash In Shop' AS TYPE,ISNULL(SUM(TABSHOPX.MON),0) AS MON,ISNULL(SUM(TABSHOPX.TUE),0) AS TUE,ISNULL(SUM(TABSHOPX.WED),0) AS WED,ISNULL(SUM(TABSHOPX.THU),0) AS THU,
	ISNULL(SUM(TABSHOPX.FRI),0) AS FRI,ISNULL(SUM(TABSHOPX.SAT),0) AS SAT,ISNULL(SUM(TABSHOPX.SUN),0) AS SUN,ISNULL(SUM(TABSHOPX.TOTAL),0) AS TOTAL, 1 AS TYPED,4 'Sort'
FROM(

SELECT 
	--'Cash In Sales' AS TYPE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 0 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS MON,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 1 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS TUE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 2 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS WED,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 3 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS THU,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 4 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS FRI,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 5 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS SAT,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 6 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS SUN,
	SUM(GL.[Credit])+SUM([TaxPercent]) AS TOTAL,
	1 AS TYPED
FROM 
	[Acc_GL] AS GL
WHERE
	GL.[TranTypeId] = 2 AND GL.[isactive] = 1 AND CoaId NOT IN(0,6,13,14,11,99)
	AND (GL.IsSalesCredit=0 AND GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate AND GL.CreditPaidDate is null)
	GROUP BY
	DATEDIFF(dd, @reportStartDate, [ActivityTimestamp])
	
UNION ALL

SELECT 
	--'Cash In Sales' AS TYPE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreditPaidDate) = 0 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS MON,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [CreditPaidDate]) = 1 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS TUE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreditPaidDate) = 2 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS WED,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreditPaidDate) = 3 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS THU,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreditPaidDate) = 4 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS FRI,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreditPaidDate) = 5 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS SAT,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreditPaidDate) = 6 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS SUN,
	SUM(GL.[Credit])+SUM([TaxPercent]) AS TOTAL,
	1 AS TYPED
FROM 
	[Acc_GL] AS GL
WHERE
	GL.[TranTypeId] = 2 AND GL.[isactive] = 1 AND CoaId NOT IN(0,6,13,14,11,99)
	AND (GL.IsSalesCredit=0 AND GL.CreditPaidDate > @reportStartDate AND GL.CreditPaidDate<@reportEndDate)
	GROUP BY
	IsSalesCredit,CreditPaidDate
	--DATEDIFF(dd, @reportStartDate, CreditPaidDate),IsSalesCredit	
) AS TABSHOPX

/*
SELECT
	--'Cash In Shop' AS [TYPE] ,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[ActivityTimestamp] ELSE TABSERV.[ActivityTimestamp] END) = 0 THEN SUM(CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[Debit] ELSE TABSERV.[Credit] END) ELSE 0 END AS MON,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[ActivityTimestamp] ELSE TABSERV.[ActivityTimestamp] END) = 1 THEN SUM(CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[Debit] ELSE TABSERV.[Credit] END) ELSE 0 END AS TUE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[ActivityTimestamp] ELSE TABSERV.[ActivityTimestamp] END) = 2 THEN SUM(CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[Debit] ELSE TABSERV.[Credit] END) ELSE 0 END AS WED,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CASE WHEN TABITEM.[InvoiceNo]IS NULL THEN TABCASH.[ActivityTimestamp] ELSE TABSERV.[ActivityTimestamp] END) = 3 THEN SUM(CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[Debit] ELSE TABSERV.[Credit] END) ELSE 0 END AS THU,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[ActivityTimestamp] ELSE TABSERV.[ActivityTimestamp] END) = 4 THEN SUM(CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[Debit] ELSE TABSERV.[Credit] END) ELSE 0 END AS FRI,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[ActivityTimestamp] ELSE TABSERV.[ActivityTimestamp] END) = 5 THEN SUM(CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[Debit] ELSE TABSERV.[Credit] END) ELSE 0 END AS SAT,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[ActivityTimestamp] ELSE TABSERV.[ActivityTimestamp] END) = 6 THEN SUM(CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[Debit] ELSE TABSERV.[Credit] END) ELSE 0 END AS SUN,
	CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN SUM(TABCASH.[Debit]) ELSE SUM(TABSERV.[Credit]) END AS TOTAL

FROM(
--ITEM ROW
SELECT 
	SUM(GL.[Credit]) AS Credit,SUM(GL.[Debit]) AS Debit,GL.[ActivityTimestamp],GL.[InvoiceNo],
	1 AS TYPED,
	GL.[TranId]
FROM 
	[Acc_GL] AS GL
	INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId]
WHERE
	GL.[TranTypeId] = 2 AND GL.[CoaId] = 14 AND GL.[isactive] = 1
	AND (GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
GROUP BY
	GL.[ActivityTimestamp],GL.[InvoiceNo],GL.[TranId]
) AS TABITEM
RIGHT JOIN
(
SELECT 
	GL.[Credit],GL.[Debit],GL.[ActivityTimestamp],GL.[InvoiceNo],
	1 AS TYPED,
	GL.[TranId]
FROM 
	[Acc_GL] AS GL
	INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId]
WHERE
	GL.[TranTypeId] = 2 AND GL.[CoaId] = 11 AND GL.[isactive] = 1
	AND (GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
	)AS TABCASH
	ON TABITEM.[TranId] = TABCASH.[TranId]

	RIGHT JOIN
(
SELECT 
	GL.[Credit],GL.[Debit],GL.[ActivityTimestamp],GL.[InvoiceNo],
	1 AS TYPED,
	GL.[TranId]
FROM 
	[Acc_GL] AS GL
	INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId]
WHERE
	GL.[TranTypeId] = 2 AND GL.[IsPostpaid] = 1 AND GL.[isactive] = 1
	AND (GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
	)AS TABSERV
	ON TABSERV.[TranId] = TABCASH.[TranId]
GROUP BY
	TABITEM.[InvoiceNo],
	DATEDIFF(dd, @reportStartDate, CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[ActivityTimestamp] ELSE TABSERV.[ActivityTimestamp] END)

	UNION

--Receipt Receivable
SELECT
	--'Cash In Shop' AS TYPE ,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 0 THEN SUM(GL.[Debit]) ELSE 0 END AS MON,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 1 THEN SUM(GL.[Debit]) ELSE 0 END AS TUE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 2 THEN SUM(GL.[Debit]) ELSE 0 END AS WED,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 3 THEN SUM(GL.[Debit]) ELSE 0 END AS THU,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 4 THEN SUM(GL.[Debit]) ELSE 0 END AS FRI,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 5 THEN SUM(GL.[Debit]) ELSE 0 END AS SAT,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 6 THEN SUM(GL.[Debit]) ELSE 0 END AS SUN,
	SUM(GL.[Debit]) AS TOTAL
FROM
	[Acc_GL] AS GL
WHERE
	GL.[TranTypeId] = 3 AND GL.[CoaId] = 11 AND GL.[IsPostpaid] = 0 AND GL.[isactive] = 1
	AND (GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
GROUP BY
	DATEDIFF(dd, @reportStartDate, [ActivityTimestamp])

) AS TAB1
WHERE TAB1.TOTAL IS NOT NULL
--GROUP BY TAB1.[TYPE]
*/
		
UNION


/*=========================================Cash IN Partial Credit=====================================================*/
SELECT
	'Cash In Partial Credit' AS TYPE,ISNULL(SUM(TABSHOPX.MON),0) AS MON,ISNULL(SUM(TABSHOPX.TUE),0) AS TUE,ISNULL(SUM(TABSHOPX.WED),0) AS WED,ISNULL(SUM(TABSHOPX.THU),0) AS THU,
	ISNULL(SUM(TABSHOPX.FRI),0) AS FRI,ISNULL(SUM(TABSHOPX.SAT),0) AS SAT,ISNULL(SUM(TABSHOPX.SUN),0) AS SUN,ISNULL(SUM(TABSHOPX.TOTAL),0) AS TOTAL, 1 AS TYPED,5 'Sort'
FROM(

SELECT 
	--'Cash In Sales' AS TYPE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreatedDate) = 0 THEN SUM(GL.Amount) ELSE 0 END AS MON,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreatedDate) = 1 THEN SUM(GL.Amount) ELSE 0 END AS TUE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreatedDate) = 2 THEN SUM(GL.Amount) ELSE 0 END AS WED,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreatedDate) = 3 THEN SUM(GL.Amount) ELSE 0 END AS THU,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreatedDate) = 4 THEN SUM(GL.Amount) ELSE 0 END AS FRI,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreatedDate) = 5 THEN SUM(GL.Amount) ELSE 0 END AS SAT,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CreatedDate) = 6 THEN SUM(GL.Amount) ELSE 0 END AS SUN,
	SUM(GL.Amount) AS TOTAL,
	1 AS TYPED
FROM 
	[ACC_PartialCredit] AS GL
WHERE
	
	 (GL.CreatedDate >= @reportStartDate AND GL.CreatedDate < @reportEndDate )
	GROUP BY
	DATEDIFF(dd, @reportStartDate, GL.CreatedDate)

) AS TABSHOPX

UNION

/*=========================================Tax Collected=====================================================*/

SELECT 
	--TBCashInShop.[TYPE]
	'Tax Collected' AS TYPE,ISNULL(SUM(TBCashInShop.MON),0) AS MON,ISNULL(SUM(TBCashInShop.TUE),0) AS TUE,ISNULL(SUM(TBCashInShop.WED),0) AS WED,ISNULL(SUM(TBCashInShop.THU),0) AS THU,
	ISNULL(SUM(TBCashInShop.FRI),0) AS FRI,ISNULL(SUM(TBCashInShop.SAT),0) AS SAT,ISNULL(SUM(TBCashInShop.SUN),0) AS SUN,ISNULL(SUM(TBCashInShop.TOTAL),0) AS TOTAL, 2 AS TYPED,7 'Sort'
FROM(
SELECT 
	--'Tax Collected' AS TYPE ,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 0 THEN SUM(GL.[Credit]) ELSE 0 END AS MON,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 1 THEN SUM(GL.[Credit]) ELSE 0 END AS TUE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 2 THEN SUM(GL.[Credit]) ELSE 0 END AS WED,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 3 THEN SUM(GL.[Credit]) ELSE 0 END AS THU,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 4 THEN SUM(GL.[Credit]) ELSE 0 END AS FRI,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 5 THEN SUM(GL.[Credit]) ELSE 0 END AS SAT,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 6 THEN SUM(GL.[Credit]) ELSE 0 END AS SUN,
	SUM(GL.[Credit]) AS TOTAL
FROM 
	[Acc_GL] AS GL
WHERE 
	GL.[CoaId] = 99 AND GL.[isactive] = 1
	AND (GL.IsSalesCredit=0 AND GL.CreditPaidDate is NULL AND GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
GROUP BY
	DATEDIFF(dd, @reportStartDate, [ActivityTimestamp])
UNION
	SELECT 
	--'Tax Collected' AS TYPE ,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [CreditPaidDate]) = 0 THEN SUM(GL.[Credit]) ELSE 0 END AS MON,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [CreditPaidDate]) = 1 THEN SUM(GL.[Credit]) ELSE 0 END AS TUE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [CreditPaidDate]) = 2 THEN SUM(GL.[Credit]) ELSE 0 END AS WED,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [CreditPaidDate]) = 3 THEN SUM(GL.[Credit]) ELSE 0 END AS THU,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [CreditPaidDate]) = 4 THEN SUM(GL.[Credit]) ELSE 0 END AS FRI,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [CreditPaidDate]) = 5 THEN SUM(GL.[Credit]) ELSE 0 END AS SAT,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [CreditPaidDate]) = 6 THEN SUM(GL.[Credit]) ELSE 0 END AS SUN,
	SUM(GL.[Credit]) AS TOTAL
FROM 
	[Acc_GL] AS GL
WHERE 
	GL.[CoaId] = 99 AND GL.[isactive] = 1
	AND (GL.IsSalesCredit=0  AND GL.[CreditPaidDate] >= @reportStartDate AND GL.[CreditPaidDate] < @reportEndDate)
GROUP BY
	DATEDIFF(dd, @reportStartDate, [CreditPaidDate])
) AS TBCashInShop
--GROUP BY
--	TBCashInShop.[TYPE]


UNION

SELECT 
	'Sales to be Reported' AS TYPE,ISNULL(SUM(TABSALEX.MON)/1.0875,0) AS MON,ISNULL(SUM(TABSALEX.TUE)/1.0875,0) AS TUE,ISNULL(SUM(TABSALEX.WED)/1.0875,0) AS WED,ISNULL(SUM(TABSALEX.THU)/1.0875,0) AS THU,
	ISNULL(SUM(TABSALEX.FRI)/1.0875,0) AS FRI,ISNULL(SUM(TABSALEX.SAT)/1.0875,0) AS SAT,ISNULL(SUM(TABSALEX.SUN)/1.0875,0) AS SUN,ISNULL(SUM(TABSALEX.TOTAL)/1.0875,0) AS TOTAL, 2 AS TYPED,8 'Sort'
FROM(

SELECT 
	--'Sales to be Reported' AS TYPE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 0 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS MON,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 1 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS TUE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 2 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS WED,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 3 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS THU,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 4 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS FRI,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 5 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS SAT,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 6 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS SUN,
	SUM(GL.[Credit])+SUM([TaxPercent]) AS TOTAL,
	2 AS TYPED
FROM 
	[Acc_GL] AS GL
WHERE
	GL.[CoaId] = 14 AND GL.[TranTypeId] = 2 AND GL.[isactive] = 1
	AND (GL.TaxPercent>0 AND GL.IsSalesCredit=0 AND Gl.CreditPaidDate IS  NULL AND GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
GROUP BY
	DATEDIFF(dd, @reportStartDate, [ActivityTimestamp])

	UNION

	SELECT 
	--'Sales to be Reported' AS TYPE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [CreditPaidDate]) = 0 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS MON,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [CreditPaidDate]) = 1 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS TUE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [CreditPaidDate]) = 2 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS WED,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [CreditPaidDate]) = 3 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS THU,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [CreditPaidDate]) = 4 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS FRI,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [CreditPaidDate]) = 5 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS SAT,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [CreditPaidDate]) = 6 THEN SUM(GL.[Credit])+SUM([TaxPercent]) ELSE 0 END AS SUN,
	SUM(GL.[Credit])+SUM([TaxPercent]) AS TOTAL,
	2 AS TYPED
FROM 
	[Acc_GL] AS GL
WHERE
	GL.[CoaId] = 14 AND GL.[TranTypeId] = 2 AND GL.[isactive] = 1
	AND (GL.TaxPercent>0 AND GL.IsSalesCredit=0 AND Gl.CreditPaidDate IS NOT NULL AND GL.[CreditPaidDate] >= @reportStartDate AND GL.[CreditPaidDate] < @reportEndDate)
GROUP BY
	DATEDIFF(dd, @reportStartDate, [CreditPaidDate])
) AS TABSALEX






) AS TBWeeklyCashFlow
order by TBWeeklyCashFlow.Sort


END






GO


