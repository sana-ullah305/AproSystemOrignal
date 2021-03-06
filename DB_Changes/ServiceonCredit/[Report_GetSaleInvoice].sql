USE [AprosysAccounting]
GO
/****** Object:  StoredProcedure [dbo].[Report_GetSaleInvoice]    Script Date: 03/11/2019 15:30:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[Report_GetSaleInvoice]
(
	@voucherNo NVARCHAR(50)
)
AS
BEGIN

SELECT
	GL.[InvoiceNo],
	ITEM.[ItemCode],ITEM.[Name], ITEM.[Unit],
	GL.[ItemId],GL.[Quantity],GL.[UnitPrice], GL.[Credit]+GL.[TaxPercent] AS AMOUNT,'' AS COMMENTS,GL.[ActivityTimestamp] AS ACTTIMESTAMP,
	'ITEMS' AS TYPED, '' AS SERVICETYPE,
	0 AS CoaId,
	0 AS BALANCE,
	0 AS PAID,
	ISNULL(GL.[TaxPercent],0) AS TAX,
	CST.[LastName] + ' '+ CST.[FirstName] AS CSTNAME, CST.Id AS cstID, CST.Phone,GL.[IsSalesCredit],GL.CreditPaidDate
FROM
	[Acc_GL] AS GL
	INNER JOIN [Item] AS ITEM ON ITEM.[Id] = GL.[ItemId]
	INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId]
WHERE
	GL.[IsActive] = 1 AND GL.TranTypeId = 2 AND GL.[CoaId] = 14
	AND ('' = @voucherNo OR GL.[InvoiceNo] = @voucherNo)
	
	UNION

SELECT
	GL.[InvoiceNo],	
	COA.ServiceCode AS ItemCode,'' AS Name,'' AS Unit,
	 0 AS ItemId,GL.[Quantity] AS Quantity,GL.[UnitPrice] AS UnitPrice, ISNULL(GL.[Credit],0)+ISNULL(GL.[TaxPercent],0) AS AMOUNT,'' AS COMMENTS,GL.[ActivityTimestamp] AS ACTTIMESTAMP,
	'SERVICES' AS TYPED, COA.[TreeName] AS SERVICETYPE,
	GL.[CoaId],
	0 AS BALANCE,
	0 AS PAID,
	ISNULL(GL.[TaxPercent],0) AS TAX,
	CST.[LastName] + ' '+ CST.[FirstName] AS CSTNAME, CST.Id AS cstID, CST.Phone,GL.[IsSalesCredit],GL.CreditPaidDate
FROM
	[Acc_GL] AS GL
	INNER JOIN [Acc_COA] AS COA ON COA.[CoaId] = GL.[CoaId] AND COA.IsActive = 1 AND COA.[HeadAccount] = 5
	INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId]
WHERE
	GL.[IsActive] = 1 AND GL.TranTypeId = 2 AND GL.[IsPostpaid] = 1
	AND ('' = @voucherNo OR GL.[InvoiceNo] = @voucherNo)

	UNION

SELECT 
	TAB1.[InvoiceNo], '' AS ItemCode,'' AS Name,'' AS Unit, 0 AS ItemId, 0 AS Quantity, 0 AS UnitPrice, 
	SUM(AMOUNT) AS AMOUNT,
	MAX(TAB1.COMMENTS),MIN(TAB1.ACTTIMESTAMP),
	'TOTALS' AS TYPED, 
	'' AS SERVICETYPE, 0 AS CoaId,
	SUM(BALANCE) AS BALANCE,
	SUM(PAID) AS PAID,
	SUM(ISNULL(TAX,0)) AS TAX,
	TAB1.CSTNAME, TAB1.Id AS cstID, MAX(TAB1.Phone),SUM(CAST(TAB1.IsSalesCredit AS INT))/*IsSalesCredit=0 then sum will be 0 always,means fully Paid,otherwise onCredit*/
	,MAX(TAB1.CreditPaidDate)
FROM(

SELECT
	GL.[InvoiceNo],
	CASE WHEN GL.[CoaId] = 0 THEN GL.[Credit] ELSE 0 END AS AMOUNT, 
	CASE WHEN GL.[CoaId] = 0 THEN GL.[Comments] ELSE NULL END AS COMMENTS,
	GL.[ActivityTimestamp] AS ACTTIMESTAMP, 
	'TOTALS' AS TYPED, 
	CASE WHEN GL.[CoaId] = 10 THEN GL.[Debit] ELSE 0 END AS BALANCE,
	CASE WHEN GL.[CoaId] = 11 THEN GL.[Debit] ELSE 0 END AS PAID,
	CASE WHEN GL.[CoaId] = 99 THEN ISNULL(GL.[Credit],0) ELSE 0 END AS TAX,
	CST.[LastName] + ' '+ CST.[FirstName] AS CSTNAME, CST.Id, CST.Phone,GL.IsSalesCredit,GL.CreditPaidDate

FROM
	[Acc_GL] AS GL
	INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId]
WHERE
	GL.[IsActive] = 1 AND GL.TranTypeId = 2 AND GL.[ItemId] IS NULL
	AND ('' = @voucherNo OR GL.[InvoiceNo] = @voucherNo)
) AS TAB1
GROUP BY
TAB1.[InvoiceNo],TAB1.CSTNAME, TAB1.Id
END








