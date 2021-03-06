USE [AprosysAccounting]
GO
/****** Object:  StoredProcedure [dbo].[GetSaleInvoiceList]    Script Date: 5/30/2019 5:26:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[GetSaleInvoiceList]
@startDate datetime=null,
@endDate datetime=null
AS
BEGIN
IF(@startDate =null)BEGIN SET @startDate=dateadd(dd,-7,GETDATE ()) END
IF(@endDate=null)BEGIN SET @endDate=GETDATE() END

SELECT
	TABDEFAULT.InvoiceNo,TABDEFAULT.ActivityTimestamp,TABDEFAULT.cstName, TABDEFAULT.isCustomerActive, ISNULL(PAID.PAIDAMOUNT,0)+ISNULL(REC.RECAMOUNT,0) AS NETAMOUNT,ISNULL(PAID.PAIDAMOUNT,0) AS PAID,(ISNULL(PAIDAMOUNT,0)+ISNULL(REC.RECAMOUNT,0))-ISNULL(PAID.PAIDAMOUNT,0) AS BAL,
	CASE WHEN DELETABLE.[InvoiceNo] IS NULL THEN 1 ELSE 0 END AS IsDeletable,IsSalesCredit,TABDEFAULT.CreditPaidDate
FROM (

SELECT
	GL.[InvoiceNo],CST.[LastName]+' '+CST.[FirstName] AS cstName ,GL.[ActivityTimestamp], CST.[IsActive] AS isCustomerActive,IsSalesCredit ,CreditPaidDate
FROM 
	[Acc_GL] AS GL
	INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId]
WHERE
	GL.[tranTypeId] = 2 AND GL.[CoaId] = 0 AND GL.IsActive=1
) 
AS TABDEFAULT
LEFT JOIN
(
SELECT
 GL.[InvoiceNo],SUM(GL.[Debit]) AS PAIDAMOUNT
FROM 
 [Acc_GL] AS GL
WHERE
 GL.[tranTypeId] = 2 AND GL.[CoaId] IN(11) AND GL.IsActive=1
GROUP BY
 GL.[InvoiceNo]
)
 AS PAID
ON PAID.InvoiceNo = TABDEFAULT.InvoiceNo
LEFT JOIN
(
SELECT
 GL.[InvoiceNo],SUM(GL.[Debit]) AS RECAMOUNT
FROM 
 [Acc_GL] AS GL
WHERE
 GL.[tranTypeId] = 2 AND GL.[CoaId] IN(10) AND GL.IsActive=1
GROUP BY
 GL.[InvoiceNo]
) 
AS REC
ON REC.InvoiceNo = TABDEFAULT.InvoiceNo 
LEFT JOIN
(
SELECT
	GL.[InvoiceNo]
FROM 
	[Acc_GL] AS GL
WHERE
	GL.[tranTypeId] = 2 AND GL.[CoaId] IN(7) AND Gl.Quantity != GL.QuantityBalance AND GL.IsActive=1
)
AS DELETABLE
ON DELETABLE.InvoiceNo = TABDEFAULT.InvoiceNo
WHERE
(TABDEFAULT.CreditPaidDate is NOT NULL AND TABDEFAULT.CreditPaidDate >= @startDate AND TABDEFAULT.CreditPaidDate <= @endDate)
OR	( TABDEFAULT.CreditPaidDate is NULL AND TABDEFAULT.ActivityTimestamp >= @startDate AND TABDEFAULT.ActivityTimestamp <= @endDate)
END

