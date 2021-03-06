USE [AprosysAccounting]
GO
/****** Object:  StoredProcedure [dbo].[Report_AccountsReceivable]    Script Date: 03/08/2019 15:28:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[Report_AccountsReceivable]
(
	@custID int,
	@typeID int
)
AS
BEGIN
--ISNULL(SUM(CAST(IsSalesCredit AS TINYINT)),0)>0 THEN 
IF(@typeID=1)
	BEGIN
	SELECT
		 SUM(ISNULL(GL.[Debit],0))  AS AMOUNT,
		CST.[LastName] + ' '+ CST.[FirstName] AS VENDOR,
		CASE WHEN CST.[TypeId] = 1 THEN 'Receivables' ELSE 'Subscription' END AS TYPED
	FROM
	[Acc_GL] AS GL
	INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId] AND (@typeID = 0 OR CST.[TypeId] = @typeID)
	WHERE
	GL.[IsActive] = 1 and IsSalesCredit=1 AND GL.CoaId=11 --AND GL.CoaId = 10
	AND (@custID = 0 OR GL.[CustId] = @custID)
	GROUP BY
	CST.[LastName], CST.[FirstName], CST.[TypeId]
	END

ELSE
	BEGIN 
	SELECT
		SUM(ISNULL(GL.[Debit],0))-SUM(ISNULL(GL.[Credit],0)) AS AMOUNT,
		CST.[LastName] + ' '+ CST.[FirstName] AS VENDOR,
		CASE WHEN CST.[TypeId] = 1 THEN 'Receivables' ELSE 'Subscription' END AS TYPED
	FROM
	[Acc_GL] AS GL
	INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId] AND (@typeID = 0 OR CST.[TypeId] = @typeID)
	WHERE
	GL.[IsActive] = 1 AND GL.CoaId = 10
	AND (@custID = 0 OR GL.[CustId] = @custID)
	GROUP BY
	CST.[LastName], CST.[FirstName], CST.[TypeId]
	END

END
