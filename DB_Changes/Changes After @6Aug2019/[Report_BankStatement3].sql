USE [AprosysAccounting]
GO
/****** Object:  StoredProcedure [dbo].[Report_BankStatement3]    Script Date: 10/26/2019 11:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[Report_BankStatement3]
	-- Add the parameters for the stored procedure here
	@coaID INT,
	@beginDate DateTime2,
	@endDate DateTime2
AS
BEGIN
	Declare @temp table (GlId INT,Balance Decimal);
	Declare @openingBalance decimal;
	--We Keep Current Balance Against Each Transaction in a temp Table
	
	Insert into  @temp SELECT G1.GlId,COALESCE (sum(G2.Debit),0)-COALESCE(sum(G2.Credit),0) as Balance
	
  FROM [Acc_GL]  G1
  
  inner join
   [Acc_GL]  G2 on (G1.ActivityTimestamp>=G2.ActivityTimestamp OR 
  (G1.ActivityTimestamp=G2.ActivityTimestamp AND G1.GlId>=G2.GlID )
  )
  where ( G1.CoaId=@coaID AND G2.CoaId=@coaID)
  AND G1.ActivityTimestamp>=@beginDate AND G1.ActivityTimestamp<=@endDate
  AND G1.IsActive=1 AND G2.IsActive=1
  Group by G1.GlId

  Select @openingBalance=COALESCE (sum([Acc_GL].Debit),0)-COALESCE(sum([Acc_GL].Credit),0)
  from [Acc_GL]  
  where ACC_GL.IsActive=1  AND ACC_GL.ActivityTimestamp<@beginDate AND Acc_GL.CoaId=@coaID

  
  SELECT 0 GlId,0 TranId,0 TranTypeId,@coaID CoaId,0 Credit,0 Debit,@beginDate ActivityTimeStamp,'' LastName,'' FirstName,'' Comments,Acc_COA.TreeName AccountName,@openingBalance Balance
  from 
  ACC_COa
  where ACC_COa.CoaId=@coaid AND Acc_COA .IsActive =1
  UNION

  
  SELECT ACC_GL.GlId,TranId,TranTypeId,[Acc_GL].CoaId, Credit,Debit,ActivityTimeStamp,[User].LastName,[User].FirstName,ACC_GL.Comments,Acc_COA.TreeName AccountName,k.Balance
  FROM [Acc_GL]
  inner join
  [User] on Acc_GL.UserId=[User].Id
  inner join
  @temp as k on Acc_GL.GlId=k.GlId
  inner join Acc_COA on Acc_GL.CoaId=Acc_COA.CoaId
  AND Acc_GL.IsActive =1
  
  order by ActivityTimestamp desc,CoaId


  
  

END


