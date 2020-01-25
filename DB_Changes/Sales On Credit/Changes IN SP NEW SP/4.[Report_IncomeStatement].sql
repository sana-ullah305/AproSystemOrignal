USE [AprosysAccounting]
GO
/****** Object:  StoredProcedure [dbo].[Report_IncomeStatement]    Script Date: 03/08/2019 15:28:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[Report_IncomeStatement]
(
	@dtStart datetime,
	@dtEnd datetime
)
AS
BEGIN

SELECT
	 'Revenue' AS TYPED, ISNULL(SUM(GL.[Credit]),0)-ISNULL(SUM(GL.[Debit]),0) AS REVAMOUNT, 0 AS EXPAMOUNT, COARECUR.[TreeName]
FROM 
	[Acc_GL] AS GL
INNER JOIN [Acc_COA] AS COA ON COA.[CoaId] = GL.[CoaId] AND COA.[HeadAccount] = 5
INNER JOIN [Acc_COA] AS COARECUR ON COA.[PId] = COARECUR.[CoaId]
WHERE
	GL.[IsActive] = 1
	AND GL.[CoaId] NOT IN (14,100) AND IsSalesCredit=0
	AND (GL.[ActivityTimestamp] >= @dtStart AND GL.[ActivityTimestamp] <= @dtEnd AND GL.CreditPaidDate IS NULL)
	OR (GL.IsSalesCredit=0 AND GL.CreditPaidDate > @dtStart AND GL.CreditPaidDate<@dtEnd)
GROUP BY
	COARECUR.[TreeName]

UNION


SELECT
	 'Revenue' AS TYPED, ISNULL(SUM(GL.[Credit]),0)-ISNULL(SUM(GL.[Debit]),0) AS REVAMOUNT, 0 AS EXPAMOUNT, COA.[TreeName]
FROM 
	[Acc_GL] AS GL
INNER JOIN [Acc_COA] AS COA ON COA.[CoaId] = GL.[CoaId] AND COA.[HeadAccount] = 5
WHERE
	GL.[IsActive] = 1
	AND GL.[CoaId] IN (14,100) AND IsSalesCredit=0
	AND (GL.[ActivityTimestamp] >= @dtStart AND GL.[ActivityTimestamp] <= @dtEnd AND GL.CreditPaidDate IS NULL)
	OR (GL.IsSalesCredit=0 AND GL.CreditPaidDate > @dtStart AND GL.CreditPaidDate<@dtEnd)
GROUP BY
	COA.[TreeName]

UNION

SELECT
	 'EXPENSE' AS TYPED, 0 AS REVAMOUNT, CASE WHEN COA.CoaId = 99 THEN ISNULL(SUM(GL.[Credit]),0)-ISNULL(SUM(GL.[Debit]),0) ELSE ISNULL(SUM(GL.[Debit]),0)-ISNULL(SUM(GL.[Credit]),0)  END AS EXPAMOUNT, COA.[TreeName]
FROM 
	[Acc_GL] AS GL
INNER JOIN [Acc_COA] AS COA ON COA.[CoaId] = GL.[CoaId] AND (COA.[HeadAccount] = 4 OR COA.CoaId = 99)
WHERE
	GL.[IsActive] = 1 AND IsSalesCredit=0
	AND (GL.[ActivityTimestamp] >= @dtStart AND GL.[ActivityTimestamp] <= @dtEnd AND GL.CreditPaidDate IS NULL)
	OR (GL.IsSalesCredit=0 AND GL.CreditPaidDate > @dtStart AND GL.CreditPaidDate<@dtEnd)
GROUP BY
	COA.[TreeName],COA.CoaId

END

