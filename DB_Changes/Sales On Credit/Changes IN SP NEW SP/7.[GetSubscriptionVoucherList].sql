USE [AprosysAccounting]
GO
/****** Object:  StoredProcedure [dbo].[GetSubscriptionVoucherList]    Script Date: 03/08/2019 15:28:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[GetSubscriptionVoucherList]
@voucherNo NVARCHAR(50),
@ActivityStartDate DATETIME,
@ActivityEndDate DATETIME
AS
BEGIN

SELECT
	GL.[Debit] AS Amount, CST.[LastName] + ' '+ CST.[FirstName] AS Customer, GL.[ActivityTimestamp],GL.[InvoiceNo],CST.Id CustId,GL.Comments,SUBS.SubscriptionAmount--,CAST( Replace(InvoiceNo ,'RCT-','' ) AS INT) sort --CST.Misc
FROM
	[Acc_GL] AS GL
	INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId] AND CST.[TypeId] = 2 AND GL.IsActive =1
	INNER JOIN [Subscription] AS SUBS ON SUBS.CustId=CST.Id AND SUBS.IsActive=1
WHERE
	(GL.[CoaId] = 0 AND GL.[TranTypeId] = 3)--AND (@voucherNo = '' OR GL.[InvoiceNo] = @voucherNo)
	AND GL.ActivityTimestamp >=@ActivityStartDate AND GL.ActivityTimestamp <=@ActivityEndDate
	order by CAST( Replace(InvoiceNo ,'RCT-','' ) AS INT) desc
END
