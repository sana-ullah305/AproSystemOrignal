USE [AprosysAccounting]
GO
/****** Object:  StoredProcedure [dbo].[GetServiceName]    Script Date: 03/11/2019 11:28:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[GetServiceName]
AS
BEGIN
select CoaId ,TreeName,Cost,ServiceCode from Acc_COA
WHERE HeadAccount =5 AND PId=101
END
