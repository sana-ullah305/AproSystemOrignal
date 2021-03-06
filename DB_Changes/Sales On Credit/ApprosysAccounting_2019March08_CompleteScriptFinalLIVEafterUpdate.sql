USE [AprosysAccounting]
GO
/****** Object:  Table [dbo].[Acc_TransactionTypes]    Script Date: 03/08/2019 15:40:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Acc_TransactionTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Acc_TransactionTypes] ON
INSERT [dbo].[Acc_TransactionTypes] ([Id], [Name], [IsActive]) VALUES (1, N'Purchase', 1)
INSERT [dbo].[Acc_TransactionTypes] ([Id], [Name], [IsActive]) VALUES (2, N'Sales', 1)
INSERT [dbo].[Acc_TransactionTypes] ([Id], [Name], [IsActive]) VALUES (3, N'ReceiptVoucher', 1)
INSERT [dbo].[Acc_TransactionTypes] ([Id], [Name], [IsActive]) VALUES (4, N'PaymentVoucher', 1)
INSERT [dbo].[Acc_TransactionTypes] ([Id], [Name], [IsActive]) VALUES (5, N'SubscriptionDue', 1)
INSERT [dbo].[Acc_TransactionTypes] ([Id], [Name], [IsActive]) VALUES (6, N'BankTransfer', 1)
SET IDENTITY_INSERT [dbo].[Acc_TransactionTypes] OFF
/****** Object:  Table [dbo].[Acc_GLTxLinks]    Script Date: 03/08/2019 15:40:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Acc_GLTxLinks](
	[GLTxLinkID] [int] IDENTITY(1,1) NOT NULL,
	[GLID] [int] NULL,
	[RelGLID] [int] NULL,
	[Quantity] [decimal](10, 2) NULL,
	[Credit] [decimal](10, 2) NULL,
	[Debit] [decimal](10, 2) NULL,
	[FiscalYear] [int] NULL,
	[TranTypeID] [int] NULL,
	[ItemID] [int] NULL,
	[UnitPrice] [decimal](10, 2) NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Acc_GLTxLinks] PRIMARY KEY CLUSTERED 
(
	[GLTxLinkID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Acc_GLTxLinks] ON
INSERT [dbo].[Acc_GLTxLinks] ([GLTxLinkID], [GLID], [RelGLID], [Quantity], [Credit], [Debit], [FiscalYear], [TranTypeID], [ItemID], [UnitPrice], [IsActive]) VALUES (1, 30, 2, CAST(2.00 AS Decimal(10, 2)), CAST(180.00 AS Decimal(10, 2)), NULL, NULL, 2, 39, CAST(90.00 AS Decimal(10, 2)), 0)
INSERT [dbo].[Acc_GLTxLinks] ([GLTxLinkID], [GLID], [RelGLID], [Quantity], [Credit], [Debit], [FiscalYear], [TranTypeID], [ItemID], [UnitPrice], [IsActive]) VALUES (2, 33, 8, CAST(2.00 AS Decimal(10, 2)), CAST(300.00 AS Decimal(10, 2)), NULL, NULL, 2, 41, CAST(150.00 AS Decimal(10, 2)), 0)
SET IDENTITY_INSERT [dbo].[Acc_GLTxLinks] OFF
/****** Object:  Table [dbo].[Acc_GL]    Script Date: 03/08/2019 15:40:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Acc_GL](
	[GlId] [int] IDENTITY(1,1) NOT NULL,
	[TranId] [int] NULL,
	[TranTypeId] [int] NULL,
	[CoaId] [int] NULL,
	[UserId] [int] NULL,
	[VendorId] [int] NULL,
	[CustId] [int] NULL,
	[ItemId] [int] NULL,
	[UnitPrice] [decimal](10, 2) NULL,
	[Quantity] [decimal](10, 2) NULL,
	[QuantityBalance] [decimal](10, 2) NULL,
	[TaxPercent] [decimal](10, 2) NULL,
	[IsGift] [bit] NULL,
	[FiscarlYear] [int] NULL,
	[Credit] [decimal](10, 2) NULL,
	[Debit] [decimal](10, 2) NULL,
	[ActivityTimestamp] [datetime] NULL,
	[Discount] [decimal](10, 2) NULL,
	[InvoiceNo] [nvarchar](50) NULL,
	[Comments] [nvarchar](256) NULL,
	[Misc] [nvarchar](256) NULL,
	[IsPostpaid] [bit] NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[SubscriptionDueDate] [datetime] NULL,
	[IsSalesCredit] [bit] NULL,
	[CreditPaidDate] [datetime] NULL,
 CONSTRAINT [PK_Acc_GL] PRIMARY KEY CLUSTERED 
(
	[GlId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Acc_GL] ON
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (1, NULL, 7, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(704.00 AS Decimal(10, 2)), CAST(704.00 AS Decimal(10, 2)), CAST(0x0000AA01010D97A4 AS DateTime), NULL, N'STI-1', N'', NULL, NULL, 1, 1, CAST(0x0000AA01010D97A4 AS DateTime), 1, CAST(0x0000AA01010D97A4 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (2, 1, 7, 6, 1, NULL, NULL, 39, CAST(44.00 AS Decimal(10, 2)), CAST(16.00 AS Decimal(10, 2)), CAST(16.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, CAST(704.00 AS Decimal(10, 2)), CAST(0x0000AA01010D97A4 AS DateTime), NULL, N'STI-1', NULL, NULL, NULL, 1, 1, CAST(0x0000AA01010D97A9 AS DateTime), 1, CAST(0x0000AA01010D97A9 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (3, 1, 7, 132, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(704.00 AS Decimal(10, 2)), NULL, CAST(0x0000AA01010D97A4 AS DateTime), NULL, N'STI-1', NULL, NULL, NULL, 1, 1, CAST(0x0000AA01010D97AE AS DateTime), 1, CAST(0x0000AA01010D97AE AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (4, NULL, 7, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(1393.00 AS Decimal(10, 2)), CAST(1393.00 AS Decimal(10, 2)), CAST(0x0000AA01010DC65E AS DateTime), NULL, N'STI-2', N'', NULL, NULL, 1, 1, CAST(0x0000AA01010DC65E AS DateTime), 1, CAST(0x0000AA01010DC65E AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (5, 4, 7, 6, 1, NULL, NULL, 40, CAST(199.00 AS Decimal(10, 2)), CAST(7.00 AS Decimal(10, 2)), CAST(7.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, CAST(1393.00 AS Decimal(10, 2)), CAST(0x0000AA01010DC65E AS DateTime), NULL, N'STI-2', NULL, NULL, NULL, 1, 1, CAST(0x0000AA01010DC65E AS DateTime), 1, CAST(0x0000AA01010DC65E AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (6, 4, 7, 132, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(1393.00 AS Decimal(10, 2)), NULL, CAST(0x0000AA01010DC65E AS DateTime), NULL, N'STI-2', NULL, NULL, NULL, 1, 1, CAST(0x0000AA01010DC65E AS DateTime), 1, CAST(0x0000AA01010DC65E AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (7, NULL, 7, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(970.00 AS Decimal(10, 2)), CAST(970.00 AS Decimal(10, 2)), CAST(0x0000AA01010F2C83 AS DateTime), NULL, N'STI-3', N'', NULL, NULL, 1, 1, CAST(0x0000AA01010F2C83 AS DateTime), 1, CAST(0x0000AA01010F2C83 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (8, 7, 7, 6, 1, NULL, NULL, 41, CAST(97.00 AS Decimal(10, 2)), CAST(10.00 AS Decimal(10, 2)), CAST(10.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, CAST(970.00 AS Decimal(10, 2)), CAST(0x0000AA01010F2C83 AS DateTime), NULL, N'STI-3', NULL, NULL, NULL, 1, 1, CAST(0x0000AA01010F2C88 AS DateTime), 1, CAST(0x0000AA01010F2C88 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (9, 7, 7, 132, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(970.00 AS Decimal(10, 2)), NULL, CAST(0x0000AA01010F2C83 AS DateTime), NULL, N'STI-3', NULL, NULL, NULL, 1, 1, CAST(0x0000AA01010F2C88 AS DateTime), 1, CAST(0x0000AA01010F2C88 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (10, NULL, 7, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(850.00 AS Decimal(10, 2)), CAST(850.00 AS Decimal(10, 2)), CAST(0x0000AA010111C855 AS DateTime), NULL, N'STI-4', N'', NULL, NULL, 1, 1, CAST(0x0000AA010111C855 AS DateTime), 1, CAST(0x0000AA010111C855 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (11, 10, 7, 6, 1, NULL, NULL, 42, CAST(85.00 AS Decimal(10, 2)), CAST(10.00 AS Decimal(10, 2)), CAST(10.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, CAST(850.00 AS Decimal(10, 2)), CAST(0x0000AA010111C855 AS DateTime), NULL, N'STI-4', NULL, NULL, NULL, 1, 1, CAST(0x0000AA010111C855 AS DateTime), 1, CAST(0x0000AA010111C855 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (12, 10, 7, 132, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(850.00 AS Decimal(10, 2)), NULL, CAST(0x0000AA010111C855 AS DateTime), NULL, N'STI-4', NULL, NULL, NULL, 1, 1, CAST(0x0000AA010111C855 AS DateTime), 1, CAST(0x0000AA010111C855 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (13, NULL, 7, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(997.50 AS Decimal(10, 2)), CAST(997.50 AS Decimal(10, 2)), CAST(0x0000AA010114CD58 AS DateTime), NULL, N'STI-5', N'', NULL, NULL, 1, 1, CAST(0x0000AA010114CD58 AS DateTime), 1, CAST(0x0000AA010114CD58 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (14, 13, 7, 6, 1, NULL, NULL, 44, CAST(47.50 AS Decimal(10, 2)), CAST(21.00 AS Decimal(10, 2)), CAST(21.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, CAST(997.50 AS Decimal(10, 2)), CAST(0x0000AA010114CD58 AS DateTime), NULL, N'STI-5', NULL, NULL, NULL, 1, 1, CAST(0x0000AA010114CD58 AS DateTime), 1, CAST(0x0000AA010114CD58 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (15, 13, 7, 132, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(997.50 AS Decimal(10, 2)), NULL, CAST(0x0000AA010114CD58 AS DateTime), NULL, N'STI-5', NULL, NULL, NULL, 1, 1, CAST(0x0000AA010114CD58 AS DateTime), 1, CAST(0x0000AA010114CD58 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (16, NULL, 7, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(604.31 AS Decimal(10, 2)), CAST(604.31 AS Decimal(10, 2)), CAST(0x0000AA010115366D AS DateTime), NULL, N'STI-6', N'', NULL, NULL, 1, 1, CAST(0x0000AA010115366D AS DateTime), 1, CAST(0x0000AA010115366D AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (17, 16, 7, 6, 1, NULL, NULL, 45, CAST(12.59 AS Decimal(10, 2)), CAST(48.00 AS Decimal(10, 2)), CAST(48.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, CAST(604.31 AS Decimal(10, 2)), CAST(0x0000AA010115366D AS DateTime), NULL, N'STI-6', NULL, NULL, NULL, 1, 1, CAST(0x0000AA0101153672 AS DateTime), 1, CAST(0x0000AA0101153672 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (18, 16, 7, 132, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(604.31 AS Decimal(10, 2)), NULL, CAST(0x0000AA010115366D AS DateTime), NULL, N'STI-6', NULL, NULL, NULL, 1, 1, CAST(0x0000AA0101153672 AS DateTime), 1, CAST(0x0000AA0101153672 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (19, NULL, 7, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(508.00 AS Decimal(10, 2)), CAST(508.00 AS Decimal(10, 2)), CAST(0x0000AA0101154F7A AS DateTime), NULL, N'STI-7', N'', NULL, NULL, 1, 1, CAST(0x0000AA0101154F7A AS DateTime), 1, CAST(0x0000AA0101154F7A AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (20, 19, 7, 6, 1, NULL, NULL, 43, CAST(127.00 AS Decimal(10, 2)), CAST(4.00 AS Decimal(10, 2)), CAST(4.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, CAST(508.00 AS Decimal(10, 2)), CAST(0x0000AA0101154F7A AS DateTime), NULL, N'STI-7', NULL, NULL, NULL, 1, 1, CAST(0x0000AA0101154F7A AS DateTime), 1, CAST(0x0000AA0101154F7A AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (21, 19, 7, 132, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(508.00 AS Decimal(10, 2)), NULL, CAST(0x0000AA0101154F7A AS DateTime), NULL, N'STI-7', NULL, NULL, NULL, 1, 1, CAST(0x0000AA0101154F7A AS DateTime), 1, CAST(0x0000AA0101154F7A AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (22, NULL, 7, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(510.00 AS Decimal(10, 2)), CAST(510.00 AS Decimal(10, 2)), CAST(0x0000AA010115E654 AS DateTime), NULL, N'STI-8', N'', NULL, NULL, 1, 1, CAST(0x0000AA010115E654 AS DateTime), 1, CAST(0x0000AA010115E654 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (23, 22, 7, 6, 1, NULL, NULL, 46, CAST(255.00 AS Decimal(10, 2)), CAST(2.00 AS Decimal(10, 2)), CAST(2.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, CAST(510.00 AS Decimal(10, 2)), CAST(0x0000AA010115E654 AS DateTime), NULL, N'STI-8', NULL, NULL, NULL, 1, 1, CAST(0x0000AA010115E654 AS DateTime), 1, CAST(0x0000AA010115E654 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (24, 22, 7, 132, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(510.00 AS Decimal(10, 2)), NULL, CAST(0x0000AA010115E654 AS DateTime), NULL, N'STI-8', NULL, NULL, NULL, 1, 1, CAST(0x0000AA010115E654 AS DateTime), 1, CAST(0x0000AA010115E654 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (29, NULL, 2, 0, 1, NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(570.00 AS Decimal(10, 2)), CAST(570.00 AS Decimal(10, 2)), CAST(0x0000AA0200E453F4 AS DateTime), NULL, N'SNV-1', N'', NULL, NULL, 0, 1, CAST(0x0000AA0200E4FE4F AS DateTime), 1, CAST(0x0000AA0200E60819 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (30, 29, 2, 6, 1, NULL, 2, 39, CAST(44.00 AS Decimal(10, 2)), CAST(-2.00 AS Decimal(10, 2)), CAST(2.00 AS Decimal(10, 2)), CAST(15.75 AS Decimal(10, 2)), NULL, NULL, CAST(88.00 AS Decimal(10, 2)), NULL, CAST(0x0000AA0200E453F4 AS DateTime), NULL, N'SNV-1', NULL, NULL, NULL, 0, 1, CAST(0x0000AA0200E4FE4F AS DateTime), 1, CAST(0x0000AA0200E60819 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (31, 29, 2, 13, 1, NULL, 2, 39, CAST(44.00 AS Decimal(10, 2)), CAST(2.00 AS Decimal(10, 2)), NULL, CAST(15.75 AS Decimal(10, 2)), NULL, NULL, NULL, CAST(88.00 AS Decimal(10, 2)), CAST(0x0000AA0200E453F4 AS DateTime), NULL, N'SNV-1', NULL, NULL, NULL, 0, 1, CAST(0x0000AA0200E4FE4F AS DateTime), 1, CAST(0x0000AA0200E60819 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (32, 29, 2, 14, 1, NULL, 2, 39, CAST(90.00 AS Decimal(10, 2)), CAST(2.00 AS Decimal(10, 2)), NULL, CAST(15.75 AS Decimal(10, 2)), NULL, NULL, CAST(164.25 AS Decimal(10, 2)), NULL, CAST(0x0000AA0200E453F4 AS DateTime), NULL, N'SNV-1', NULL, NULL, NULL, 0, 1, CAST(0x0000AA0200E4FE4F AS DateTime), 1, CAST(0x0000AA0200E60819 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (33, 29, 2, 6, 1, NULL, 2, 41, CAST(97.00 AS Decimal(10, 2)), CAST(-2.00 AS Decimal(10, 2)), CAST(2.00 AS Decimal(10, 2)), CAST(26.25 AS Decimal(10, 2)), NULL, NULL, CAST(194.00 AS Decimal(10, 2)), NULL, CAST(0x0000AA0200E453F4 AS DateTime), NULL, N'SNV-1', NULL, NULL, NULL, 0, 1, CAST(0x0000AA0200E4FE4F AS DateTime), 1, CAST(0x0000AA0200E60819 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (34, 29, 2, 13, 1, NULL, 2, 41, CAST(97.00 AS Decimal(10, 2)), CAST(2.00 AS Decimal(10, 2)), NULL, CAST(26.25 AS Decimal(10, 2)), NULL, NULL, NULL, CAST(194.00 AS Decimal(10, 2)), CAST(0x0000AA0200E453F4 AS DateTime), NULL, N'SNV-1', NULL, NULL, NULL, 0, 1, CAST(0x0000AA0200E4FE4F AS DateTime), 1, CAST(0x0000AA0200E60819 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (35, 29, 2, 14, 1, NULL, 2, 41, CAST(150.00 AS Decimal(10, 2)), CAST(2.00 AS Decimal(10, 2)), NULL, CAST(26.25 AS Decimal(10, 2)), NULL, NULL, CAST(273.75 AS Decimal(10, 2)), NULL, CAST(0x0000AA0200E453F4 AS DateTime), NULL, N'SNV-1', NULL, NULL, NULL, 0, 1, CAST(0x0000AA0200E4FE4F AS DateTime), 1, CAST(0x0000AA0200E60819 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (36, 29, 2, 136, 1, NULL, 2, NULL, CAST(90.00 AS Decimal(10, 2)), CAST(1.00 AS Decimal(10, 2)), NULL, CAST(0.00 AS Decimal(10, 2)), NULL, NULL, CAST(90.00 AS Decimal(10, 2)), NULL, CAST(0x0000AA0200E453F4 AS DateTime), NULL, N'SNV-1', NULL, NULL, 1, 0, 1, CAST(0x0000AA0200E4FE4F AS DateTime), 1, CAST(0x0000AA0200E60819 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (37, 29, 2, 11, 1, NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(570.00 AS Decimal(10, 2)), CAST(0x0000AA0200E453F4 AS DateTime), NULL, N'SNV-1', NULL, NULL, NULL, 0, 1, CAST(0x0000AA0200E4FE4F AS DateTime), 1, CAST(0x0000AA0200E60819 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Acc_GL] ([GlId], [TranId], [TranTypeId], [CoaId], [UserId], [VendorId], [CustId], [ItemId], [UnitPrice], [Quantity], [QuantityBalance], [TaxPercent], [IsGift], [FiscarlYear], [Credit], [Debit], [ActivityTimestamp], [Discount], [InvoiceNo], [Comments], [Misc], [IsPostpaid], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [SubscriptionDueDate], [IsSalesCredit], [CreditPaidDate]) VALUES (38, 29, 2, 99, 1, NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(42.00 AS Decimal(10, 2)), NULL, CAST(0x0000AA0200E453F4 AS DateTime), NULL, N'SNV-1', NULL, NULL, NULL, 0, 1, CAST(0x0000AA0200E4FE4F AS DateTime), 1, CAST(0x0000AA0200E60819 AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Acc_GL] OFF
/****** Object:  Table [dbo].[Acc_COA]    Script Date: 03/08/2019 15:40:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Acc_COA](
	[CoaId] [int] IDENTITY(1,1) NOT NULL,
	[PId] [int] NULL,
	[CoaNo] [nvarchar](50) NULL,
	[HeadAccount] [int] NULL,
	[TreeName] [nvarchar](50) NULL,
	[CoaLevel] [int] NULL,
	[OpeningBalance] [decimal](10, 2) NULL,
	[IsActive] [bit] NULL,
	[Cost] [decimal](10, 2) NULL,
 CONSTRAINT [PK_Acc_COA] PRIMARY KEY CLUSTERED 
(
	[CoaId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Acc_COA] ON
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (1, 0, N'', 0, N'Assets', 0, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (2, 0, N'', 0, N'Liablites', 0, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (3, 0, N'', 0, N'Equity', 0, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (4, 0, N'', 0, N'Expense', 0, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (5, 0, N'', 0, N'Revenue', 0, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (6, 1, N'', 1, N'Inventory', 1, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (7, 6, N'', 1, N'StockInTrade', 2, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (8, 6, N'', 1, N'Diesel', 2, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (9, 6, N'', 1, N'Oil', 2, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (10, 1, N'', 1, N'AccountReceivable', 1, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (11, 1, N'', 1, N'Cash', 1, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (12, 2, N'', 2, N'AccountPayable', 1, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (13, 4, N'', 4, N'CostOfGoodsSold', 1, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (14, 5, N'', 5, N'Sales', 1, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (15, 1, N'', 1, N'Bank', 1, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (16, 15, N'', 1, N'A/c number 1772', 2, CAST(10067.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (17, 15, N'', 1, N'A/c number 48982-3', 2, CAST(2494215.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (18, 15, N'', 1, N'A/c number 50241', 2, CAST(10215.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (19, 4, N'', 4, N'Administrative Expenses', 1, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (20, 4, N'', 4, N'Inventory Expenses', 1, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (21, 19, N'', 4, N'Salaries', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (22, 20, N'', 4, N'Generator Diesel', 2, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (23, 19, N'', 4, N'Office Expenses', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (24, 3, N'', 3, N'Shareholder''s Equity', 1, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (25, 15, N'', 1, N'A/c number 0366', 2, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (27, 19, N'', 4, N'Entertainment', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (29, 19, N'', 4, N'Office Masood', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (30, 19, N'', 4, N'Misc', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (99, 2, NULL, 2, N'Sales Tax', 1, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (100, 5, NULL, 5, N'Monthly Subscription', 1, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (101, 5, NULL, 5, N'Sales Service', 1, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (104, 1, NULL, 1, N'Bank Accounts', 1, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (105, 104, NULL, 1, N'AC# 120229', 2, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (106, 104, NULL, 1, N'AC# 21229', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (119, 19, NULL, 4, N'ShopPart', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (120, 19, NULL, 4, N'Difrence', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (122, 19, NULL, 4, N'Jeff', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (123, 19, NULL, 4, N'Rocky', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (124, 19, NULL, 4, N'Cleaning', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (125, 19, NULL, 4, N'Hector Monhly', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (126, 19, NULL, 4, N'missing', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (127, 19, NULL, 4, N'Papi', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (128, 19, NULL, 4, N'OFF', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (129, 19, NULL, 4, N'Parking', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (130, 19, NULL, 4, N'Cari', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (131, 19, NULL, 4, N'Gas', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (132, 3, N'', 3, N'Adjustment Equity', 1, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (133, 4, N'', 4, N'Adjustment Cost of goods sold', 1, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (134, 19, NULL, 4, N'sqsa', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (135, 19, NULL, 4, N'ydaydnisandisadasdusaudbu biudyiuudasiudiuadsadasd', 2, CAST(0.00 AS Decimal(10, 2)), 0, NULL)
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (136, 101, NULL, 5, N'activation 40', 2, CAST(0.00 AS Decimal(10, 2)), 1, CAST(0.00 AS Decimal(10, 2)))
INSERT [dbo].[Acc_COA] ([CoaId], [PId], [CoaNo], [HeadAccount], [TreeName], [CoaLevel], [OpeningBalance], [IsActive], [Cost]) VALUES (137, 101, NULL, 5, N'activation 30', 2, CAST(0.00 AS Decimal(10, 2)), 1, NULL)
SET IDENTITY_INSERT [dbo].[Acc_COA] OFF
/****** Object:  Table [dbo].[Customer]    Script Date: 03/08/2019 15:40:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TypeId] [int] NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[OpeningBalance] [decimal](10, 2) NULL,
	[Misc] [nvarchar](256) NULL,
	[StartDate] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Differentiate b/w Customer and Subscriber.1 for customer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'TypeId'
GO
SET IDENTITY_INSERT [dbo].[Customer] ON
INSERT [dbo].[Customer] ([Id], [TypeId], [FirstName], [LastName], [Phone], [Email], [OpeningBalance], [Misc], [StartDate], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [IsActive]) VALUES (1, 3, N'Walk IN', N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[Customer] ([Id], [TypeId], [FirstName], [LastName], [Phone], [Email], [OpeningBalance], [Misc], [StartDate], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [IsActive]) VALUES (2, 1, N'Ruso', N'Vlad', N'4755295282', N'', CAST(0.00 AS Decimal(10, 2)), N'', CAST(0x0000A9E700000000 AS DateTime), CAST(0x0000AA0200CB525B AS DateTime), 1, CAST(0x0000AA0200CD9CF0 AS DateTime), 1, 1)
SET IDENTITY_INSERT [dbo].[Customer] OFF
/****** Object:  Table [dbo].[ItemType]    Script Date: 03/08/2019 15:40:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_ItemType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ItemType] ON
INSERT [dbo].[ItemType] ([Id], [Name], [IsActive]) VALUES (1, N'Accesories', 1)
SET IDENTITY_INSERT [dbo].[ItemType] OFF
/****** Object:  Table [dbo].[Item]    Script Date: 03/08/2019 15:40:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ItemTypeId] [int] NULL,
	[ItemCode] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](256) NULL,
	[Unit] [nvarchar](50) NULL,
	[MinQty] [decimal](6, 2) NULL,
	[TaxPercent] [decimal](5, 2) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[PurchasePrice] [decimal](10, 2) NULL,
	[SellPrice] [decimal](10, 2) NULL,
	[IsActive] [bit] NOT NULL,
	[IsTaxable] [bit] NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Item] ON
INSERT [dbo].[Item] ([Id], [ItemTypeId], [ItemCode], [Name], [Description], [Unit], [MinQty], [TaxPercent], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [PurchasePrice], [SellPrice], [IsActive], [IsTaxable]) VALUES (1, 1, N'0000', N'Item New', N'', N'Piece', CAST(1.00 AS Decimal(6, 2)), CAST(0.00 AS Decimal(5, 2)), NULL, NULL, CAST(0x0000AA0100F14F02 AS DateTime), 1, CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), 0, 0)
INSERT [dbo].[Item] ([Id], [ItemTypeId], [ItemCode], [Name], [Description], [Unit], [MinQty], [TaxPercent], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [PurchasePrice], [SellPrice], [IsActive], [IsTaxable]) VALUES (39, 1, N'B07FD9ZM93', N'Bluetooth Printer (C)', N'Mini BlueTooth printer blue top', N'Piece', CAST(10.00 AS Decimal(6, 2)), CAST(0.00 AS Decimal(5, 2)), CAST(0x0000AA01010C4CED AS DateTime), 1, NULL, NULL, CAST(44.00 AS Decimal(10, 2)), CAST(90.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Item] ([Id], [ItemTypeId], [ItemCode], [Name], [Description], [Unit], [MinQty], [TaxPercent], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [PurchasePrice], [SellPrice], [IsActive], [IsTaxable]) VALUES (40, 1, N'X001TVA1VX', N'Android PDA Device ', N'Orange Pda Device V2', N'Piece', CAST(5.00 AS Decimal(6, 2)), CAST(0.00 AS Decimal(5, 2)), CAST(0x0000AA01010D39F8 AS DateTime), 1, CAST(0x0000AA0101101B2F AS DateTime), 1, CAST(199.00 AS Decimal(10, 2)), CAST(275.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Item] ([Id], [ItemTypeId], [ItemCode], [Name], [Description], [Unit], [MinQty], [TaxPercent], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [PurchasePrice], [SellPrice], [IsActive], [IsTaxable]) VALUES (41, 1, N'848958036456', N'Vivo Go Cell Phone', N'Blue Vivo Go 4G LTE', N'Piece', CAST(5.00 AS Decimal(6, 2)), CAST(0.00 AS Decimal(5, 2)), CAST(0x0000AA01010EFF61 AS DateTime), 1, NULL, NULL, CAST(97.00 AS Decimal(10, 2)), CAST(150.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Item] ([Id], [ItemTypeId], [ItemCode], [Name], [Description], [Unit], [MinQty], [TaxPercent], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [PurchasePrice], [SellPrice], [IsActive], [IsTaxable]) VALUES (42, 1, N'ITPP083USE-BK', N'Thermal Printer(C)', N'Big Printer Chino', N'Piece', CAST(5.00 AS Decimal(6, 2)), CAST(0.00 AS Decimal(5, 2)), CAST(0x0000AA010111B4DA AS DateTime), 1, NULL, NULL, CAST(85.00 AS Decimal(10, 2)), CAST(130.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Item] ([Id], [ItemTypeId], [ItemCode], [Name], [Description], [Unit], [MinQty], [TaxPercent], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [PurchasePrice], [SellPrice], [IsActive], [IsTaxable]) VALUES (43, 1, N'C31CD52062', N'Thermal Printer(E)', N'TM-T20II', N'Piece', CAST(5.00 AS Decimal(6, 2)), CAST(0.00 AS Decimal(5, 2)), CAST(0x0000AA010112FB9D AS DateTime), 1, NULL, NULL, CAST(127.00 AS Decimal(10, 2)), CAST(180.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Item] ([Id], [ItemTypeId], [ItemCode], [Name], [Description], [Unit], [MinQty], [TaxPercent], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [PurchasePrice], [SellPrice], [IsActive], [IsTaxable]) VALUES (44, 1, N'TP1', N'Thermal Paper 3.25"', N'Big box of Paper', N'Piece', CAST(5.00 AS Decimal(6, 2)), CAST(0.00 AS Decimal(5, 2)), CAST(0x0000AA010114BDAC AS DateTime), 1, CAST(0x0000AA010114DBD9 AS DateTime), 1, CAST(47.50 AS Decimal(10, 2)), CAST(65.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Item] ([Id], [ItemTypeId], [ItemCode], [Name], [Description], [Unit], [MinQty], [TaxPercent], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [PurchasePrice], [SellPrice], [IsActive], [IsTaxable]) VALUES (45, 1, N'TP2', N'Thermal Paper 2"', N'Small Box of Paper', N'Piece', CAST(5.00 AS Decimal(6, 2)), CAST(0.00 AS Decimal(5, 2)), CAST(0x0000AA01011520D6 AS DateTime), 1, NULL, NULL, CAST(12.59 AS Decimal(10, 2)), CAST(25.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Item] ([Id], [ItemTypeId], [ItemCode], [Name], [Description], [Unit], [MinQty], [TaxPercent], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [PurchasePrice], [SellPrice], [IsActive], [IsTaxable]) VALUES (46, 1, N'DPP-250+IBT', N'DPP-250', N'DPP-250', N'Piece', CAST(1.00 AS Decimal(6, 2)), CAST(0.00 AS Decimal(5, 2)), CAST(0x0000AA010115D701 AS DateTime), 1, NULL, NULL, CAST(255.00 AS Decimal(10, 2)), CAST(325.00 AS Decimal(10, 2)), 1, 1)
SET IDENTITY_INSERT [dbo].[Item] OFF
/****** Object:  Table [dbo].[Vendor]    Script Date: 03/08/2019 15:40:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vendor](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Phone] [nvarchar](20) NULL,
	[Terms] [nvarchar](50) NULL,
	[CreditLimit] [decimal](10, 2) NULL,
	[Balance] [decimal](10, 2) NULL,
	[Email] [nvarchar](50) NULL,
	[Address] [nvarchar](256) NULL,
	[Misc] [nvarchar](256) NULL,
	[IsActive] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_Vendor] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 03/08/2019 15:40:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Phone] [nvarchar](20) NULL,
	[Email] [nvarchar](50) NULL,
	[Address] [nvarchar](256) NULL,
	[JoiningDate] [datetime] NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[AdminRights] [bit] NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[User] ON
INSERT [dbo].[User] ([Id], [FirstName], [LastName], [Phone], [Email], [Address], [JoiningDate], [IsActive], [CreatedBy], [ModifiedBy], [CreatedOn], [ModifiedOn], [UserName], [Password], [AdminRights]) VALUES (1, N'admin', N'admin', N'', N'', N'', NULL, 1, 0, NULL, CAST(0x0000A9D500E9C171 AS DateTime), NULL, N'admin', N'admin', 1)
INSERT [dbo].[User] ([Id], [FirstName], [LastName], [Phone], [Email], [Address], [JoiningDate], [IsActive], [CreatedBy], [ModifiedBy], [CreatedOn], [ModifiedOn], [UserName], [Password], [AdminRights]) VALUES (12, N'ali', N'ali', N'', N'', N'', NULL, 0, 0, 1, CAST(0x0000A9DF010A522C AS DateTime), CAST(0x0000AA0100F1CCCD AS DateTime), N'aliname', N'1234', 0)
INSERT [dbo].[User] ([Id], [FirstName], [LastName], [Phone], [Email], [Address], [JoiningDate], [IsActive], [CreatedBy], [ModifiedBy], [CreatedOn], [ModifiedOn], [UserName], [Password], [AdminRights]) VALUES (13, N'ali', N'ali', N'', N'', N'', NULL, 0, 0, 1, CAST(0x0000A9DF018B6AD0 AS DateTime), CAST(0x0000AA0100F1C689 AS DateTime), N'ali', N'ali', 0)
INSERT [dbo].[User] ([Id], [FirstName], [LastName], [Phone], [Email], [Address], [JoiningDate], [IsActive], [CreatedBy], [ModifiedBy], [CreatedOn], [ModifiedOn], [UserName], [Password], [AdminRights]) VALUES (15, N'ne', N'new user ', N'', N'', N'', NULL, 0, 1, 1, CAST(0x0000A9E5013957DC AS DateTime), CAST(0x0000AA0100F1C91A AS DateTime), N'skfdj', N'kasjdflaksfja', 1)
SET IDENTITY_INSERT [dbo].[User] OFF
/****** Object:  Table [dbo].[tblinvoice]    Script Date: 03/08/2019 15:40:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblinvoice](
	[id] [int] NOT NULL,
	[name] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Taxes]    Script Date: 03/08/2019 15:40:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Taxes](
	[taxesID] [int] IDENTITY(1,1) NOT NULL,
	[taxesName] [nvarchar](50) NULL,
	[taxPercent] [decimal](6, 3) NULL,
 CONSTRAINT [PK_Taxes] PRIMARY KEY CLUSTERED 
(
	[taxesID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Taxes] ON
INSERT [dbo].[Taxes] ([taxesID], [taxesName], [taxPercent]) VALUES (1, N'Sales Tax', CAST(8.750 AS Decimal(6, 3)))
INSERT [dbo].[Taxes] ([taxesID], [taxesName], [taxPercent]) VALUES (2, N'Service Tax', CAST(0.000 AS Decimal(6, 3)))
SET IDENTITY_INSERT [dbo].[Taxes] OFF
/****** Object:  Table [dbo].[Subscription]    Script Date: 03/08/2019 15:40:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subscription](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustId] [int] NOT NULL,
	[SubscriptionAmount] [decimal](10, 2) NOT NULL,
	[DueDate] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Subscription] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportConfig]    Script Date: 03/08/2019 15:40:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportConfig](
	[repoConfigID] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](50) NULL,
	[address] [nvarchar](400) NULL,
	[detailTitle] [nvarchar](50) NULL,
	[detail] [nvarchar](max) NULL,
	[active] [bit] NULL,
	[createdBy] [nvarchar](50) NULL,
	[createdDate] [datetime] NULL,
	[modifiedBy] [nvarchar](50) NULL,
	[modifiedDate] [datetime] NULL,
 CONSTRAINT [PK_ReportConfig] PRIMARY KEY CLUSTERED 
(
	[repoConfigID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ReportConfig] ON
INSERT [dbo].[ReportConfig] ([repoConfigID], [title], [address], [detailTitle], [detail], [active], [createdBy], [createdDate], [modifiedBy], [modifiedDate]) VALUES (2, N'INFO-TECH DELTA COMPUTER,INC', N'2440 Amsterdam Avenue
New Yourk,NY 10033
Phone:(917)521-1100 Fax:(917)-521-1114', N'Thank you for your business!', N'All new computer parts are covered by manufacture warranty
6 months warranty on used laptops
2 weeks warranty on software repair', 0, N'1', CAST(0x0000A9F4008F9DB0 AS DateTime), N'1', CAST(0x0000A9F4008F9DB0 AS DateTime))
INSERT [dbo].[ReportConfig] ([repoConfigID], [title], [address], [detailTitle], [detail], [active], [createdBy], [createdDate], [modifiedBy], [modifiedDate]) VALUES (3, N'INFO-TECH DELTA COMPUTER,INC', N'2440 Amsterdam Avenue
New Yourk,NY 10033
Phone:(917)521-1100 Fax:(917)-521-1114', N'Thank you for your business!', N'
All new computer parts are covered by manufacture warranty
6 months warranty on used laptops
2 weeks warranty on software repair
3 month warranty on hardware repair
Not responsible for data loss
Any abuse or misuse will void warranty
All returns and refunds are subject to 15% restocking fee
No returns or refunds after 14 days of purchase
Not responsible for any items left over a period greater than 60 days
DCA License Numbers: 2033382, 2033280, 2033612', 0, N'1', CAST(0x0000A9F400913E71 AS DateTime), N'1', CAST(0x0000A9F400913E71 AS DateTime))
INSERT [dbo].[ReportConfig] ([repoConfigID], [title], [address], [detailTitle], [detail], [active], [createdBy], [createdDate], [modifiedBy], [modifiedDate]) VALUES (4, N'INFO-TECH DELTA COMPUTER,INC', N'2440 Amsterdam Avenue Shaheen VIEW
New Yourk,NY 10033
Phone:(917)521-1100 Fax:(917)-521-1114', N'Thank you for your business!', N'
All new computer parts are covered by manufacture warranty
6 months warranty on used laptops
2 weeks warranty on software repair
3 month warranty on hardware repair
Not responsible for data loss
Any abuse or misuse will void warranty
All returns and refunds are subject to 15% restocking fee
No returns or refunds after 14 days of purchase
Not responsible for any items left over a period greater than 60 days
DCA License Numbers: 2033382, 2033280, 2033612', 0, N'1', CAST(0x0000A9F4009A3F73 AS DateTime), N'1', CAST(0x0000A9F4009A3F73 AS DateTime))
INSERT [dbo].[ReportConfig] ([repoConfigID], [title], [address], [detailTitle], [detail], [active], [createdBy], [createdDate], [modifiedBy], [modifiedDate]) VALUES (5, N'INFO-TECH DELTA COMPUTER,INC', N'2440 Amsterdam Avenue
New Yourk,NY 10033
Phone:(917)521-1100 Fax:(917)-521-1114', N'Thank you for your business!', N'
All new computer parts are covered by manufacture warranty
6 months warranty on used laptops
2 weeks warranty on software repair
3 month warranty on hardware repair
Not responsible for data loss
Any abuse or misuse will void warranty
All returns and refunds are subject to 15% restocking fee
No returns or refunds after 14 days of purchase
Not responsible for any items left over a period greater than 60 days
DCA License Numbers: 2033382, 2033280, 2033612', 0, N'1', CAST(0x0000A9F4009A6827 AS DateTime), N'1', CAST(0x0000A9F4009A6827 AS DateTime))
INSERT [dbo].[ReportConfig] ([repoConfigID], [title], [address], [detailTitle], [detail], [active], [createdBy], [createdDate], [modifiedBy], [modifiedDate]) VALUES (6, N'INFO-TECH DELTA COMPUTER,INC', N'2440 Amsterdam Avenue
New Yourk,NY 10033
Phone:(917)521-1100 Fax:(917)-521-1114', N'Thank you for your business!', N'All new computer parts are covered by manufacture warranty
6 months warranty on used laptops
2 weeks warranty on software repair
3 month warranty on hardware repair
Not responsible for data loss
Any abuse or misuse will void warranty
All returns and refunds are subject to 15% restocking fee
No returns or refunds after 14 days of purchase
Not responsible for any items left over a period greater than 60 days
DCA License Numbers: 2033382, 2033280, 2033612', 0, N'1', CAST(0x0000AA01011066EA AS DateTime), N'1', CAST(0x0000AA01011066EA AS DateTime))
INSERT [dbo].[ReportConfig] ([repoConfigID], [title], [address], [detailTitle], [detail], [active], [createdBy], [createdDate], [modifiedBy], [modifiedDate]) VALUES (7, N'INFO-TECH DELTA COMPUTER,INC', N'2440 Amsterdam Avenue
New York, NY 10033
Phone: (917)521-1100 Fax: (917)-521-1114', N'Thank you for your business!', N'All new computer parts are covered by manufacture warranty
6 months warranty on used laptops
2 weeks warranty on software repair
3 month warranty on hardware repair
Not responsible for data loss
Any abuse or misuse will void warranty
All returns and refunds are subject to 15% restocking fee
No returns or refunds after 14 days of purchase
Not responsible for any items left over a period greater than 60 days
DCA License Numbers: 2033382, 2033280, 2033612', 1, N'1', CAST(0x0000AA0101108F40 AS DateTime), N'1', CAST(0x0000AA0101108F40 AS DateTime))
SET IDENTITY_INSERT [dbo].[ReportConfig] OFF
/****** Object:  Table [dbo].[FinancialYear]    Script Date: 03/08/2019 15:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinancialYear](
	[YearId] [int] IDENTITY(1,1) NOT NULL,
	[Financialyear] [int] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_FinancialYear] PRIMARY KEY CLUSTERED 
(
	[YearId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[FinancialYear] ON
INSERT [dbo].[FinancialYear] ([YearId], [Financialyear], [StartDate], [EndDate], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [IsActive]) VALUES (2, 2019, CAST(0x0000A9C800000000 AS DateTime), CAST(0x0000AB2A00000000 AS DateTime), NULL, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[FinancialYear] OFF
/****** Object:  StoredProcedure [dbo].[DeletePurchase]    Script Date: 03/08/2019 15:42:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[DeletePurchase]
(
	@voucherNo NVARCHAR(50),
	@userID int
)
AS
BEGIN
UPDATE 
	[Acc_GL]
SET 
	[IsActive] = 0, ModifiedDate = GETDATE(), [UserId] = @userID
WHERE
	[InvoiceNo] = @voucherNo
END
GO
/****** Object:  StoredProcedure [dbo].[Report_WeeklyCashFlow_backup]    Script Date: 03/08/2019 15:42:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Report_WeeklyCashFlow_backup]
(
	@ActivityTime Datetime
	
)
AS
BEGIN
DECLARE @reportStartDate datetime,@reportEndDate datetime
SET @reportStartDate= DATEADD(DAY, 2 - DATEPART(WEEKDAY, @ActivityTime), CAST(@ActivityTime AS DATE)) 
SET @reportEndDate= DATEADD(DAY, 9 - DATEPART(WEEKDAY, @ActivityTime), CAST(@ActivityTime AS DATE)) 
        
SELECT 
	TBCashInSales.[TYPE],SUM(TBCashInSales.MON) AS MON,SUM(TBCashInSales.TUE) AS TUE,SUM(TBCashInSales.WED) AS WED,SUM(TBCashInSales.THU) AS THU,SUM(TBCashInSales.FRI) AS FRI,
	SUM(TBCashInSales.SAT) AS SAT,SUM(TBCashInSales.SUN) AS SUN,SUM(TBCashInSales.TOTAL) AS TOTAL, 1 AS TYPED
FROM(
SELECT 
	'Previous Week' AS TYPE ,
	CASE WHEN DATEDIFF(dd, DATEADD(DAY,-7,@reportStartDate), [ActivityTimestamp]) = 0 THEN ISNULL(SUM(GL.[Debit]),0)-ISNULL(SUM(GL.[Credit]),0) ELSE 0 END AS MON,
	CASE WHEN DATEDIFF(dd, DATEADD(DAY,-7,@reportStartDate), [ActivityTimestamp]) = 1 THEN ISNULL(SUM(GL.[Debit]),0)-ISNULL(SUM(GL.[Credit]),0) ELSE 0 END AS TUE,
	CASE WHEN DATEDIFF(dd, DATEADD(DAY,-7,@reportStartDate), [ActivityTimestamp]) = 2 THEN ISNULL(SUM(GL.[Debit]),0)-ISNULL(SUM(GL.[Credit]),0) ELSE 0 END AS WED,
	CASE WHEN DATEDIFF(dd, DATEADD(DAY,-7,@reportStartDate), [ActivityTimestamp]) = 3 THEN ISNULL(SUM(GL.[Debit]),0)-ISNULL(SUM(GL.[Credit]),0) ELSE 0 END AS THU,
	CASE WHEN DATEDIFF(dd, DATEADD(DAY,-7,@reportStartDate), [ActivityTimestamp]) = 4 THEN ISNULL(SUM(GL.[Debit]),0)-ISNULL(SUM(GL.[Credit]),0) ELSE 0 END AS FRI,
	CASE WHEN DATEDIFF(dd, DATEADD(DAY,-7,@reportStartDate), [ActivityTimestamp]) = 5 THEN ISNULL(SUM(GL.[Debit]),0)-ISNULL(SUM(GL.[Credit]),0) ELSE 0 END AS SAT,
	CASE WHEN DATEDIFF(dd, DATEADD(DAY,-7,@reportStartDate), [ActivityTimestamp]) = 6 THEN ISNULL(SUM(GL.[Debit]),0)-ISNULL(SUM(GL.[Credit]),0) ELSE 0 END AS SUN,
	ISNULL(SUM(GL.[Debit]),0)-ISNULL(SUM(GL.[Credit]),0) AS TOTAL
FROM 
	[Acc_GL] AS GL
WHERE 
	GL.[CoaId] = 11
	AND (GL.[ActivityTimestamp]<@reportStartDate AND GL.[ActivityTimestamp] > DATEADD(DAY,-7,@reportStartDate))
GROUP BY
	DATEDIFF(dd, DATEADD(DAY,-7,@reportStartDate), [ActivityTimestamp])
) AS TBCashInSales
GROUP BY
	TBCashInSales.[TYPE]

UNION

SELECT 
	'Cash In Sales' AS TYPE,SUM(TABSALEX.MON) AS MON,SUM(TABSALEX.TUE) AS TUE,SUM(TABSALEX.WED) AS WED,SUM(TABSALEX.THU) AS THU,
	SUM(TABSALEX.FRI) AS FRI,SUM(TABSALEX.SAT) AS SAT,SUM(TABSALEX.SUN) AS SUN,SUM(TABSALEX.TOTAL) AS TOTAL, 1 AS TYPED
FROM(

SELECT 
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 0 THEN SUM(GL.[Credit]) ELSE 0 END AS MON,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 1 THEN SUM(GL.[Credit]) ELSE 0 END AS TUE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 2 THEN SUM(GL.[Credit]) ELSE 0 END AS WED,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 3 THEN SUM(GL.[Credit]) ELSE 0 END AS THU,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 4 THEN SUM(GL.[Credit]) ELSE 0 END AS FRI,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 5 THEN SUM(GL.[Credit]) ELSE 0 END AS SAT,
	CASE WHEN DATEDIFF(dd, @reportStartDate, [ActivityTimestamp]) = 6 THEN SUM(GL.[Credit]) ELSE 0 END AS SUN,
	SUM(GL.[Credit]) AS TOTAL,
	1 AS TYPED
FROM 
	[Acc_GL] AS GL
WHERE
	GL.[CoaId] = 14 AND GL.[TranTypeId] = 2 
	AND (GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
GROUP BY
	DATEDIFF(dd, @reportStartDate, [ActivityTimestamp])
) AS TABSALEX

UNION

SELECT 
	TBCASHOUT.[TYPE],-SUM(TBCASHOUT.MON) AS MON,-SUM(TBCASHOUT.TUE) AS TUE,-SUM(TBCASHOUT.WED) AS WED,-SUM(TBCASHOUT.THU) AS THU,
	-SUM(TBCASHOUT.FRI) AS FRI,-SUM(TBCASHOUT.SAT) AS SAT,-SUM(TBCASHOUT.SUN) AS SUN,-SUM(TBCASHOUT.TOTAL) AS TOTAL, 1 AS TYPED
FROM(
SELECT 
	'Cash Out' AS TYPE ,
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
	GL.[CoaId] = 11 AND GL.[TranTypeId] IN (1,4)
	AND (GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
GROUP BY
	DATEDIFF(dd, @reportStartDate, [ActivityTimestamp])
) AS TBCASHOUT
GROUP BY
	TBCASHOUT.[TYPE]

UNION

SELECT 
	TBCASHOUT.[TYPE],SUM(TBCASHOUT.MON) AS MON,SUM(TBCASHOUT.TUE) AS TUE,SUM(TBCASHOUT.WED) AS WED,SUM(TBCASHOUT.THU) AS THU,
	SUM(TBCASHOUT.FRI) AS FRI,SUM(TBCASHOUT.SAT) AS SAT,SUM(TBCASHOUT.SUN) AS SUN,SUM(TBCASHOUT.TOTAL) AS TOTAL, 1 AS TYPED
FROM(
SELECT 
	'Deposit' AS TYPE ,
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
	GL.[CoaId] = 11 AND GL.[TranTypeId] IN(6)
	AND (GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
GROUP BY
	DATEDIFF(dd, @reportStartDate, [ActivityTimestamp])
) AS TBCASHOUT
GROUP BY
	TBCASHOUT.[TYPE]
	
	
UNION


SELECT 
	TBCashInMONTHLY.[TYPE],SUM(TBCashInMONTHLY.MON) AS MON,SUM(TBCashInMONTHLY.TUE) AS TUE,SUM(TBCashInMONTHLY.WED) AS WED,SUM(TBCashInMONTHLY.THU) AS THU,
	SUM(TBCashInMONTHLY.FRI) AS FRI,SUM(TBCashInMONTHLY.SAT) AS SAT,SUM(TBCashInMONTHLY.SUN) AS SUN,SUM(TBCashInMONTHLY.TOTAL) AS TOTAL, 1 AS TYPED
FROM(
SELECT 
	'Cash in Monthly' AS TYPE ,
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
	GL.[CoaId] = 11 AND GL.[TranTypeId] = 3 AND GL.[IsPostpaid] = 1
	AND (GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
GROUP BY
	DATEDIFF(dd, @reportStartDate, [ActivityTimestamp])
) AS TBCashInMONTHLY
GROUP BY
	TBCashInMONTHLY.[TYPE]
	
UNION


SELECT
	TAB1.[TYPE],SUM(TAB1.[MON]) AS MON,SUM(TAB1.[TUE]) AS TUE,SUM(TAB1.[WED]) AS WED,SUM(TAB1.[THU]) AS THU,
	SUM(TAB1.[FRI]) AS FRI,SUM(TAB1.[SAT]) AS SAT,SUM(TAB1.[SUN]) AS SUN,SUM(TAB1.[TOTAL]) AS TOTAL,
	1 AS TYPED

FROM (

SELECT
	'Cash In Shop' AS [TYPE] ,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[ActivityTimestamp] ELSE TABSERV.[ActivityTimestamp] END) = 0 THEN SUM(CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[Debit] ELSE TABSERV.[Credit] END) ELSE 0 END AS MON,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[ActivityTimestamp] ELSE TABSERV.[ActivityTimestamp] END) = 1 THEN SUM(CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[Debit] ELSE TABSERV.[Credit] END) ELSE 0 END AS TUE,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[ActivityTimestamp] ELSE TABSERV.[ActivityTimestamp] END) = 2 THEN SUM(CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[Debit] ELSE TABSERV.[Credit] END) ELSE 0 END AS WED,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CASE WHEN TABITEM.[InvoiceNo]IS NULL THEN TABCASH.[ActivityTimestamp] ELSE TABSERV.[ActivityTimestamp] END) = 3 THEN SUM(CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[Debit] ELSE TABSERV.[Credit] END) ELSE 0 END AS THU,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[ActivityTimestamp] ELSE TABSERV.[ActivityTimestamp] END) = 4 THEN SUM(CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[Debit] ELSE TABSERV.[Credit] END) ELSE 0 END AS FRI,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[ActivityTimestamp] ELSE TABSERV.[ActivityTimestamp] END) = 6 THEN SUM(CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[Debit] ELSE TABSERV.[Credit] END) ELSE 0 END AS SAT,
	CASE WHEN DATEDIFF(dd, @reportStartDate, CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[ActivityTimestamp] ELSE TABSERV.[ActivityTimestamp] END) = 7 THEN SUM(CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[Debit] ELSE TABSERV.[Credit] END) ELSE 0 END AS SUN,
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
	GL.[TranTypeId] = 2 AND GL.[CoaId] = 14
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
	GL.[TranTypeId] = 2 AND GL.[CoaId] = 11
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
	GL.[TranTypeId] = 2 AND GL.[IsPostpaid] = 1
	AND (GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
	)AS TABSERV
	ON TABSERV.[TranId] = TABCASH.[TranId]
GROUP BY
	TABITEM.[InvoiceNo],
	DATEDIFF(dd, @reportStartDate, CASE WHEN TABITEM.[InvoiceNo] IS NULL THEN TABCASH.[ActivityTimestamp] ELSE TABSERV.[ActivityTimestamp] END)

	UNION

--Receipt Receivable
SELECT
	'Cash In Shop' AS TYPE ,
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
	GL.[TranTypeId] = 3 AND GL.[CoaId] = 11 AND GL.[IsPostpaid] = 0
	AND (GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
GROUP BY
	DATEDIFF(dd, @reportStartDate, [ActivityTimestamp])

) AS TAB1
WHERE TAB1.TOTAL IS NOT NULL
GROUP BY TAB1.[TYPE]

		
UNION


SELECT 
	TBCashInShop.[TYPE],SUM(TBCashInShop.SUN) AS SUN,SUM(TBCashInShop.MON) AS MON,SUM(TBCashInShop.TUE) AS TUE,SUM(TBCashInShop.WED) AS WED,SUM(TBCashInShop.THU) AS THU,
	SUM(TBCashInShop.FRI) AS FRI,SUM(TBCashInShop.SAT) AS SAT,SUM(TBCashInShop.TOTAL) AS TOTAL, 2 AS TYPED
FROM(
SELECT 
	'Tax Collected' AS TYPE ,
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
	GL.[CoaId] = 99
	AND (GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
GROUP BY
	DATEDIFF(dd, @reportStartDate, [ActivityTimestamp])
) AS TBCashInShop
GROUP BY
	TBCashInShop.[TYPE]


END
GO
/****** Object:  StoredProcedure [dbo].[Report_WeeklyCashFlow]    Script Date: 03/08/2019 15:42:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Report_WeeklyCashFlow]
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
	AND (GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
GROUP BY
	DATEDIFF(dd, @reportStartDate, [ActivityTimestamp])
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
	AND (GL.[ActivityTimestamp] >= @reportStartDate AND GL.[ActivityTimestamp] < @reportEndDate)
GROUP BY
	DATEDIFF(dd, @reportStartDate, [ActivityTimestamp])
) AS TABSALEX






) AS TBWeeklyCashFlow
order by TBWeeklyCashFlow.Sort


END
GO
/****** Object:  StoredProcedure [dbo].[Report_TrialBalance]    Script Date: 03/08/2019 15:42:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Report_TrialBalance]
(
	@dtStart datetime,
	@dtEnd datetime
)
AS
BEGIN

SELECT
	SUM(ISNULL(GL.[Debit],0))-SUM(ISNULL(GL.[Credit],0)) AS DEBIT, 0 AS CREDIT, COA.[TreeName] AS ACCOUNT
FROM
	[Acc_GL] AS GL
INNER JOIN [Acc_COA] AS COA ON COA.[CoaId] = GL.[CoaId] AND COA.[HeadAccount] IN (1,4)
WHERE
	GL.[IsActive] = 1
	AND (GL.[ActivityTimestamp] >= @dtStart AND GL.[ActivityTimestamp] <= @dtEnd)
GROUP BY
	 COA.[TreeName]

UNION

SELECT
	0 AS DEBIT, SUM(ISNULL(GL.[Credit],0))-SUM(ISNULL(GL.[Debit],0)) AS CREDIT, COA.[TreeName] AS ACCOUNT
FROM
	[Acc_GL] AS GL
INNER JOIN [Acc_COA] AS COA ON COA.[CoaId] = GL.[CoaId] AND COA.[HeadAccount] IN (2,3,5)
WHERE
	GL.[IsActive] = 1
	AND (GL.[ActivityTimestamp] >= @dtStart AND GL.[ActivityTimestamp] <= @dtEnd)
GROUP BY
	COA.[TreeName]


END
GO
/****** Object:  StoredProcedure [dbo].[Report_MonthlySubscription_Ahmer]    Script Date: 03/08/2019 15:42:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Report_MonthlySubscription_Ahmer]
(
	@year int
)
AS
BEGIN
SELECT
	CST.[Id] ,CST.[FirstName],SUBS.DueDate,SUBS.SubscriptionAmount, 
	CASE WHEN (DATEPART(MONTH,GETDATE()) =1 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >1 THEN SUBS.SubscriptionAmount ELSE 0 END AS JANDUE, ISNULL(SUM(TBLPAID.[JAN]),0) AS JANPAID, 
	CASE WHEN (DATEPART(MONTH,GETDATE()) =2 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >2 THEN SUBS.SubscriptionAmount ELSE 0 END AS FEBDUE, ISNULL(SUM(TBLPAID.[FEB]),0) AS FEBPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =3 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >3 THEN SUBS.SubscriptionAmount ELSE 0 END AS MARDUE, ISNULL(SUM(TBLPAID.[MAR]),0) AS MARPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =4 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >4 THEN SUBS.SubscriptionAmount ELSE 0 END  AS APRDUE, ISNULL(SUM(TBLPAID.[APR]),0) AS APRPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =5 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >5 THEN SUBS.SubscriptionAmount ELSE 0 END  AS MAYDUE, ISNULL(SUM(TBLPAID.[MAY]),0) AS MAYPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =6 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >6 THEN SUBS.SubscriptionAmount ELSE 0 END  AS JUNDUE, ISNULL(SUM(TBLPAID.[JUN]),0) AS JUNPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =7 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >7 THEN SUBS.SubscriptionAmount ELSE 0 END  AS JULDUE, ISNULL(SUM(TBLPAID.[JUL]),0) AS JULPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =8 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >8 THEN SUBS.SubscriptionAmount ELSE 0 END  AS AUGDUE, ISNULL(SUM(TBLPAID.[AUG]),0) AS AUGPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =9 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >9 THEN SUBS.SubscriptionAmount ELSE 0 END  AS SEPDUE, ISNULL(SUM(TBLPAID.[SEP]),0) AS SEPPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =10 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >10 THEN SUBS.SubscriptionAmount ELSE 0 END  AS OCTDUE, ISNULL(SUM(TBLPAID.[OCT]),0) AS OCTPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =11 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >11 THEN SUBS.SubscriptionAmount ELSE 0 END  AS NOVDUE, ISNULL(SUM(TBLPAID.[NOV]),0) AS NOVPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =12 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(YEAR,GETDATE()) >@year THEN SUBS.SubscriptionAmount ELSE 0 END  AS DECDUE, ISNULL(SUM(TBLPAID.[DEC]),0) AS DECPAID
FROM(
SELECT
	'PAID' AS TYPED, 
	CST.[Id] ,CST.[FirstName],--SUBS.DueDate,SUBS.SubscriptionAmount, 
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =1 THEN SUM(GL.[Debit]) ELSE 0 END AS 'JAN' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =2 THEN SUM(GL.[Debit]) ELSE 0 END AS 'FEB' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =3 THEN SUM(GL.[Debit]) ELSE 0 END AS 'MAR' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =4 THEN SUM(GL.[Debit]) ELSE 0 END AS 'APR' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =5 THEN SUM(GL.[Debit]) ELSE 0 END AS 'MAY' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =6 THEN SUM(GL.[Debit]) ELSE 0 END AS 'JUN' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =7 THEN SUM(GL.[Debit]) ELSE 0 END AS 'JUL' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =8 THEN SUM(GL.[Debit]) ELSE 0 END AS 'AUG' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =9 THEN SUM(GL.[Debit]) ELSE 0 END AS 'SEP' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =10 THEN SUM(GL.[Debit]) ELSE 0 END AS 'OCT' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =11 THEN SUM(GL.[Debit]) ELSE 0 END AS 'NOV' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =12 THEN SUM(GL.[Debit]) ELSE 0 END AS 'DEC'
FROM 
	[Acc_GL] AS GL
INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId] AND CST.[TypeId] = 2
WHERE
	GL.[CoaId] = 11 AND GL.[TranTypeId] = 3 AND GL.[IsPostpaid] = 1 AND GL.[IsActive] = 1
	AND DATEPART(YEAR,GL.[ActivityTimestamp]) = @year
GROUP BY
	CST.[Id] ,CST.[FirstName],DATEPART(MONTH,GL.[ActivityTimestamp])

) AS TBLPAID
RIGHT JOIN [Customer] AS CST ON CST.[Id] = TBLPAID.[Id] AND CST.[TypeId]= 2
INNER JOIN [Subscription] AS SUBS ON SUBS.[CustId] = CST.[Id]
GROUP BY
	CST.[Id] ,CST.[FirstName],SUBS.DueDate,SUBS.SubscriptionAmount
END
GO
/****** Object:  StoredProcedure [dbo].[Report_MonthlySubscription_15Feb]    Script Date: 03/08/2019 15:42:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Report_MonthlySubscription_15Feb]
(
	@year int
)
AS
BEGIN
SELECT
	CST.[Id] ,CST.[FirstName],SUBS.DueDate,SUBS.SubscriptionAmount, 
	CASE WHEN (DATEPART(MONTH,GETDATE()) =1 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >1 THEN (ISNULL(SUBS.SubscriptionAmount,0)- ISNULL(SUM(TBLPAID.[JAN]),0)) ELSE 0 END AS JANDUE, ISNULL(SUM(TBLPAID.[JAN]),0) AS JANPAID, 
	CASE WHEN (DATEPART(MONTH,GETDATE()) =2 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >2 THEN (ISNULL(SUBS.SubscriptionAmount,0)- ISNULL(SUM(TBLPAID.FEB),0)) ELSE 0 END AS FEBDUE, ISNULL(SUM(TBLPAID.[FEB]),0) AS FEBPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =3 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >3 THEN (ISNULL(SUBS.SubscriptionAmount,0)- ISNULL(SUM(TBLPAID.MAR),0)) ELSE 0 END AS MARDUE, ISNULL(SUM(TBLPAID.[MAR]),0) AS MARPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =4 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >4 THEN (ISNULL(SUBS.SubscriptionAmount,0)- ISNULL(SUM(TBLPAID.APR),0)) ELSE 0 END  AS APRDUE, ISNULL(SUM(TBLPAID.[APR]),0) AS APRPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =5 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >5 THEN (ISNULL(SUBS.SubscriptionAmount,0)- ISNULL(SUM(TBLPAID.MAR),0)) ELSE 0 END  AS MAYDUE, ISNULL(SUM(TBLPAID.[MAY]),0) AS MAYPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =6 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >6 THEN (ISNULL(SUBS.SubscriptionAmount,0)- ISNULL(SUM(TBLPAID.JUN),0)) ELSE 0 END  AS JUNDUE, ISNULL(SUM(TBLPAID.[JUN]),0) AS JUNPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =7 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >7 THEN (ISNULL(SUBS.SubscriptionAmount,0)- ISNULL(SUM(TBLPAID.JUL),0)) ELSE 0 END  AS JULDUE, ISNULL(SUM(TBLPAID.[JUL]),0) AS JULPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =8 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >8 THEN (ISNULL(SUBS.SubscriptionAmount,0)- ISNULL(SUM(TBLPAID.AUG),0)) ELSE 0 END  AS AUGDUE, ISNULL(SUM(TBLPAID.[AUG]),0) AS AUGPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =9 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >9 THEN (ISNULL(SUBS.SubscriptionAmount,0)- ISNULL(SUM(TBLPAID.SEP),0)) ELSE 0 END  AS SEPDUE, ISNULL(SUM(TBLPAID.[SEP]),0) AS SEPPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =10 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >10 THEN (ISNULL(SUBS.SubscriptionAmount,0)- ISNULL(SUM(TBLPAID.OCT),0)) ELSE 0 END  AS OCTDUE, ISNULL(SUM(TBLPAID.[OCT]),0) AS OCTPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =11 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(MONTH,GETDATE()) >11 THEN (ISNULL(SUBS.SubscriptionAmount,0)- ISNULL(SUM(TBLPAID.NOV),0)) ELSE 0 END  AS NOVDUE, ISNULL(SUM(TBLPAID.[NOV]),0) AS NOVPAID,
	CASE WHEN (DATEPART(MONTH,GETDATE()) =12 AND DATEPART(DAY,GETDATE()) > SUBS.DueDate) OR DATEPART(YEAR,GETDATE()) >@year THEN (ISNULL(SUBS.SubscriptionAmount,0)- ISNULL(SUM(TBLPAID.[DEC]),0)) ELSE 0 END  AS DECDUE, ISNULL(SUM(TBLPAID.[DEC]),0) AS DECPAID
FROM(
SELECT
	'PAID' AS TYPED, 
	CST.[Id] ,CST.[FirstName],--SUBS.DueDate,SUBS.SubscriptionAmount, 
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =1 THEN SUM(GL.[Debit]) ELSE 0 END AS 'JAN' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =2 THEN SUM(GL.[Debit]) ELSE 0 END AS 'FEB' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =3 THEN SUM(GL.[Debit]) ELSE 0 END AS 'MAR' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =4 THEN SUM(GL.[Debit]) ELSE 0 END AS 'APR' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =5 THEN SUM(GL.[Debit]) ELSE 0 END AS 'MAY' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =6 THEN SUM(GL.[Debit]) ELSE 0 END AS 'JUN' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =7 THEN SUM(GL.[Debit]) ELSE 0 END AS 'JUL' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =8 THEN SUM(GL.[Debit]) ELSE 0 END AS 'AUG' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =9 THEN SUM(GL.[Debit]) ELSE 0 END AS 'SEP' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =10 THEN SUM(GL.[Debit]) ELSE 0 END AS 'OCT' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =11 THEN SUM(GL.[Debit]) ELSE 0 END AS 'NOV' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =12 THEN SUM(GL.[Debit]) ELSE 0 END AS 'DEC'
FROM 
	[Acc_GL] AS GL
INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId] AND CST.[TypeId] = 2
WHERE
	GL.[CoaId] = 11 AND GL.[TranTypeId] = 3 AND GL.[IsPostpaid] = 1 AND GL.[IsActive] = 1
	AND DATEPART(YEAR,GL.[ActivityTimestamp]) = @year
GROUP BY
	CST.[Id] ,CST.[FirstName],DATEPART(MONTH,GL.[ActivityTimestamp])

) AS TBLPAID
RIGHT JOIN [Customer] AS CST ON CST.[Id] = TBLPAID.[Id] AND CST.[TypeId]= 2
INNER JOIN [Subscription] AS SUBS ON SUBS.[CustId] = CST.[Id]
GROUP BY
	CST.[Id] ,CST.[FirstName],SUBS.DueDate,SUBS.SubscriptionAmount
END
GO
/****** Object:  StoredProcedure [dbo].[Report_MonthlySubscription]    Script Date: 03/08/2019 15:42:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Report_MonthlySubscription]
(
	@year int
)
AS
BEGIN

SELECT
	CST.[Id] , CST.[lastName] + ' '+CST.[FirstName] AS FirstName,
	SUBS.DueDate,SUBS.SubscriptionAmount,
	CASE WHEN ISNULL(SUM(TBLDUE.[JAN]),0) =0 THEN 0 ELSE ISNULL(SUM(TBLDUE.[JAN]),0) - ISNULL(SUM(TBLPAID.[JAN]),0) END AS JANDUE, ISNULL(SUM(TBLPAID.[JAN]),0) AS JANPAID, 
	CASE WHEN ISNULL(SUM(TBLDUE.FEB),0) =0 THEN 0 ELSE  ISNULL(SUM(TBLDUE.FEB),0) - ISNULL(SUM(TBLPAID.FEB),0) END AS FEBDUE, ISNULL(SUM(TBLPAID.[FEB]),0) AS FEBPAID,
	CASE WHEN ISNULL(SUM(TBLDUE.[MAR]),0) =0 THEN 0 ELSE ISNULL(SUM(TBLDUE.[MAR]),0) - ISNULL(SUM(TBLPAID.[MAR]),0) END AS MARDUE, ISNULL(SUM(TBLPAID.[MAR]),0) AS MARPAID,
	CASE WHEN ISNULL(SUM(TBLDUE.[APR]),0) =0 THEN 0 ELSE ISNULL(SUM(TBLDUE.[APR]),0) - ISNULL(SUM(TBLPAID.[APR]),0) END AS APRDUE, ISNULL(SUM(TBLPAID.[APR]),0) AS APRPAID,
	CASE WHEN ISNULL(SUM(TBLDUE.[MAY]),0) =0 THEN 0 ELSE ISNULL(SUM(TBLDUE.[MAY]),0) - ISNULL(SUM(TBLPAID.[MAY]),0) END AS MAYDUE, ISNULL(SUM(TBLPAID.[MAY]),0) AS MAYPAID,
	CASE WHEN ISNULL(SUM(TBLDUE.[JUN]),0) =0 THEN 0 ELSE ISNULL(SUM(TBLDUE.[JUN]),0) - ISNULL(SUM(TBLPAID.[JUN]),0) END AS JUNDUE, ISNULL(SUM(TBLPAID.[JUN]),0) AS JUNPAID,
	CASE WHEN ISNULL(SUM(TBLDUE.[JUL]),0) =0 THEN 0 ELSE ISNULL(SUM(TBLDUE.[JUL]),0) - ISNULL(SUM(TBLPAID.[JUL]),0) END AS JULDUE, ISNULL(SUM(TBLPAID.[JUL]),0) AS JULPAID,
	CASE WHEN ISNULL(SUM(TBLDUE.[AUG]),0) =0 THEN 0 ELSE ISNULL(SUM(TBLDUE.[AUG]),0) - ISNULL(SUM(TBLPAID.[AUG]),0) END AS AUGDUE, ISNULL(SUM(TBLPAID.[AUG]),0) AS AUGPAID,
	CASE WHEN ISNULL(SUM(TBLDUE.[SEP]),0) =0 THEN 0 ELSE ISNULL(SUM(TBLDUE.[SEP]),0) - ISNULL(SUM(TBLPAID.[SEP]),0) END AS SEPDUE, ISNULL(SUM(TBLPAID.[SEP]),0) AS SEPPAID,
	CASE WHEN ISNULL(SUM(TBLDUE.[OCT]),0) =0 THEN 0 ELSE ISNULL(SUM(TBLDUE.[OCT]),0) - ISNULL(SUM(TBLPAID.[OCT]),0) END AS OCTDUE, ISNULL(SUM(TBLPAID.[OCT]),0) AS OCTPAID,
	CASE WHEN ISNULL(SUM(TBLDUE.[NOV]),0) =0 THEN 0 ELSE ISNULL(SUM(TBLDUE.[NOV]),0) - ISNULL(SUM(TBLPAID.[NOV]),0) END AS NOVDUE, ISNULL(SUM(TBLPAID.[NOV]),0) AS NOVPAID,
	CASE WHEN ISNULL(SUM(TBLDUE.[DEC]),0) =0 THEN 0 ELSE ISNULL(SUM(TBLDUE.[DEC]),0) - ISNULL(SUM(TBLPAID.[DEC]),0) END AS DECDUE, ISNULL(SUM(TBLPAID.[DEC]),0) AS DECPAID,
	CASE WHEN ISNULL(SUM(TBLDUE.[TOTDUE]),0) =0 THEN 0 ELSE ISNULL(SUM(TBLDUE.[TOTDUE]),0) - ISNULL(SUM(TBLPAID.[TOTPAID]),0) END AS TOTDUE, ISNULL(SUM(TBLPAID.[TOTPAID]),0) AS TOTPAID
FROM
(
SELECT 
	'PAID' AS TYPED, SUBDUE.[Id] ,SUBDUE.FirstName,
	ISNULL(SUM(SUBDUE.[JAN]),0) AS [JAN],
	ISNULL(SUM(SUBDUE.[FEB] ),0) AS [FEB],
	ISNULL(SUM(SUBDUE.MAR),0) AS MAR,
	ISNULL(SUM(SUBDUE.APR),0) AS APR,
	ISNULL(SUM(SUBDUE.MAY),0) AS MAY,
	ISNULL(SUM(SUBDUE.JUN),0) AS JUN,
	ISNULL(SUM(SUBDUE.JUL),0) AS JUL,
	ISNULL(SUM(SUBDUE.AUG),0) AS AUG,
	ISNULL(SUM(SUBDUE.SEP),0) AS SEP,
	ISNULL(SUM(SUBDUE.OCT),0) AS OCT,
	ISNULL(SUM(SUBDUE.NOV),0) AS NOV,
	ISNULL(SUM(SUBDUE.[DEC]),0) AS [DEC],
	ISNULL(SUM(SUBDUE.TOTPAID),0) AS TOTPAID
FROM
	(

SELECT
	'PAID' AS TYPED, 
	CST.[Id] ,CST.[FirstName],--SUBS.DueDate,SUBS.SubscriptionAmount, 
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =1 THEN SUM(GL.[Debit]) ELSE 0 END AS 'JAN' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =2 THEN SUM(GL.[Debit]) ELSE 0 END AS 'FEB' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =3 THEN SUM(GL.[Debit]) ELSE 0 END AS 'MAR' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =4 THEN SUM(GL.[Debit]) ELSE 0 END AS 'APR' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =5 THEN SUM(GL.[Debit]) ELSE 0 END AS 'MAY' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =6 THEN SUM(GL.[Debit]) ELSE 0 END AS 'JUN' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =7 THEN SUM(GL.[Debit]) ELSE 0 END AS 'JUL' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =8 THEN SUM(GL.[Debit]) ELSE 0 END AS 'AUG' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =9 THEN SUM(GL.[Debit]) ELSE 0 END AS 'SEP' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =10 THEN SUM(GL.[Debit]) ELSE 0 END AS 'OCT' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =11 THEN SUM(GL.[Debit]) ELSE 0 END AS 'NOV' ,
	CASE WHEN DATEPART(MONTH,GL.[ActivityTimestamp]) =12 THEN SUM(GL.[Debit]) ELSE 0 END AS 'DEC',
	SUM(GL.[Debit]) AS TOTPAID
FROM 
	[Acc_GL] AS GL
INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId] AND CST.[TypeId] = 2
WHERE
	GL.[CoaId] = 11 AND GL.[TranTypeId] = 3 AND GL.[IsPostpaid] = 1 AND GL.[IsActive] = 1
	AND DATEPART(YEAR,GL.[ActivityTimestamp]) = @year
GROUP BY
	CST.[Id] ,CST.[FirstName],DATEPART(MONTH,GL.[ActivityTimestamp])
) AS SUBDUE
GROUP BY
SUBDUE.[Id] ,SUBDUE.FirstName
) AS TBLPAID
RIGHT JOIN
(
SELECT 
	'DUE' AS TYPED, SUBDUE.[Id] ,SUBDUE.FirstName,
	ISNULL(SUM(SUBDUE.[JAN]),0) AS [JAN],
	ISNULL(SUM(SUBDUE.[FEB] ),0) AS [FEB],
	ISNULL(SUM(SUBDUE.MAR),0) AS MAR,
	ISNULL(SUM(SUBDUE.APR),0) AS APR,
	ISNULL(SUM(SUBDUE.MAY),0) AS MAY,
	ISNULL(SUM(SUBDUE.JUN),0) AS JUN,
	ISNULL(SUM(SUBDUE.JUL),0) AS JUL,
	ISNULL(SUM(SUBDUE.AUG),0) AS AUG,
	ISNULL(SUM(SUBDUE.SEP),0) AS SEP,
	ISNULL(SUM(SUBDUE.OCT),0) AS OCT,
	ISNULL(SUM(SUBDUE.NOV),0) AS NOV,
	ISNULL(SUM(SUBDUE.[DEC]),0) AS [DEC],
	ISNULL(SUM(SUBDUE.TOTDUE),0) AS TOTDUE
FROM
	(
SELECT
	
	CST.[Id] ,CST.[LastName]+' '+CST.[FirstName] AS FirstName,--SUBS.DueDate,SUBS.SubscriptionAmount, 
	CASE WHEN DATEPART(MONTH,GL.[SubscriptionDueDate]) =1 THEN SUM(GL.[Debit]) ELSE 0 END AS 'JAN' ,
	CASE WHEN DATEPART(MONTH,GL.[SubscriptionDueDate]) =2 THEN SUM(GL.[Debit]) ELSE 0 END AS 'FEB' ,
	CASE WHEN DATEPART(MONTH,GL.[SubscriptionDueDate]) =3 THEN SUM(GL.[Debit]) ELSE 0 END AS 'MAR' ,
	CASE WHEN DATEPART(MONTH,GL.[SubscriptionDueDate]) =4 THEN SUM(GL.[Debit]) ELSE 0 END AS 'APR' ,
	CASE WHEN DATEPART(MONTH,GL.[SubscriptionDueDate]) =5 THEN SUM(GL.[Debit]) ELSE 0 END AS 'MAY' ,
	CASE WHEN DATEPART(MONTH,GL.[SubscriptionDueDate]) =6 THEN SUM(GL.[Debit]) ELSE 0 END AS 'JUN' ,
	CASE WHEN DATEPART(MONTH,GL.[SubscriptionDueDate]) =7 THEN SUM(GL.[Debit]) ELSE 0 END AS 'JUL' ,
	CASE WHEN DATEPART(MONTH,GL.[SubscriptionDueDate]) =8 THEN SUM(GL.[Debit]) ELSE 0 END AS 'AUG' ,
	CASE WHEN DATEPART(MONTH,GL.[SubscriptionDueDate]) =9 THEN SUM(GL.[Debit]) ELSE 0 END AS 'SEP' ,
	CASE WHEN DATEPART(MONTH,GL.[SubscriptionDueDate]) =10 THEN SUM(GL.[Debit]) ELSE 0 END AS 'OCT' ,
	CASE WHEN DATEPART(MONTH,GL.[SubscriptionDueDate]) =11 THEN SUM(GL.[Debit]) ELSE 0 END AS 'NOV' ,
	CASE WHEN DATEPART(MONTH,GL.[SubscriptionDueDate]) =12 THEN SUM(GL.[Debit]) ELSE 0 END AS 'DEC',
	SUM(GL.[Debit]) AS TOTDUE
FROM 
	[Acc_GL] AS GL
INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId] AND CST.[TypeId] = 2
WHERE
	GL.[CoaId] = 10 AND GL.[TranTypeId] = 5 AND GL.[IsPostpaid] = 1 AND GL.[IsActive] = 1
	AND DATEPART(YEAR,GL.[SubscriptionDueDate]) = @year
GROUP BY
	CST.[Id] ,CST.[FirstName],CST.[LastName],DATEPART(MONTH,GL.[SubscriptionDueDate])
	

) AS SUBDUE
GROUP BY
SUBDUE.[Id] ,SUBDUE.FirstName
) AS TBLDUE
ON TBLPAID.[Id] = TBLDUE.[Id] 
RIGHT JOIN [Customer] AS CST ON CST.[Id] = TBLDUE.[Id] AND CST.[TypeId]= 2
INNER JOIN [Subscription] AS SUBS ON SUBS.[CustId] = CST.[Id]
GROUP BY
	CST.[Id] ,CST.[FirstName] ,CST.[LastName],SUBS.DueDate,SUBS.SubscriptionAmount
END
GO
/****** Object:  StoredProcedure [dbo].[Report_ItemWiseSale]    Script Date: 03/08/2019 15:42:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Report_ItemWiseSale]
(
	@dtStart datetime,
	@dtEnd datetime
)
AS
BEGIN

SELECT 
	TAB2.[ItemId] AS Id, TAB2.[ITEMNAME] AS ItemName, TAB2.[Unit], ISNULL(TAB1.[Qty],0) AS Qty, ISNULL(TAB2.[PurchRate],0) AS PurchRate, ISNULL(TAB1.SaleIncTax,0)/ISNULL(TAB1.[Qty],1) AS SaleRate, ISNULL(TAB1.SaleIncTax,0) AS SaleIncTax, ISNULL(TAB1.NetSale,0) AS NetSale, ISNULL(TAB1.TaxToPay,0) AS TaxToPay,ISNULL(TAB1.[NetSale] -(TAB1.[Qty]*TAB2.[PurchRate]),0) AS Earnings
FROM
(

SELECT
	SUM(ISNULL(GL.[Quantity],0)) AS Qty, GL.[ItemId], AVG(ISNULL(GL.[UnitPrice],0)) AS SaleRate , SUM(ISNULL(GL.[Credit],0))+SUM(ISNULL(GL.[TaxPercent],0)) AS SaleIncTax,
	SUM(ISNULL(GL.[Credit],0)) AS NetSale, SUM(ISNULL(GL.[TaxPercent],0)) AS TaxToPay
FROM 
	[Acc_GL] AS GL
WHERE
	GL.[TranTypeId] = 2 AND GL.[CoaId] = 14 AND GL.[IsActive] = 1
	AND (GL.[ActivityTimestamp] >= @dtStart AND GL.[ActivityTimestamp] <= @dtEnd)
GROUP BY
	GL.[ItemId]
)AS TAB1
RIGHT JOIN
(
SELECT
	IT.[Id] AS ItemId, IT.[Name] AS ITEMNAME, IT.[Unit], AVG(ISNULL(GL.[UnitPrice],IT.[PurchasePrice])) AS PurchRate
FROM 
	[Item] AS IT
LEFT JOIN [Acc_GL] AS GL ON GL.[ItemId] = IT.[Id] AND GL.[TranTypeId] = 2 AND GL.[CoaId] = 13 AND GL.[IsActive] = 1 
AND (GL.[ActivityTimestamp] >= @dtStart AND GL.[ActivityTimestamp] <= @dtEnd)
WHERE
	IT.[IsActive] = 1
GROUP BY
	IT.[Id], IT.[Name], IT.[Unit]
) AS TAB2
ON TAB1.ItemId = TAB2.ItemId
END
GO
/****** Object:  StoredProcedure [dbo].[Report_IncomeStatement]    Script Date: 03/08/2019 15:42:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Report_IncomeStatement]
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
GO
/****** Object:  StoredProcedure [dbo].[Report_GetSaleInvoice]    Script Date: 03/08/2019 15:42:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Report_GetSaleInvoice]
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
	'' AS ItemCode,'' AS Name,'' AS Unit,
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
GO
/****** Object:  StoredProcedure [dbo].[Report_GetPurchaseInvoice]    Script Date: 03/08/2019 15:42:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Report_GetPurchaseInvoice]
(
	@voucherNo NVARCHAR(50)
)
AS
BEGIN

SELECT
	GL.[InvoiceNo],
	ITEM.[ItemCode],ITEM.[Name], ITEM.[Unit],
	GL.[ItemId],GL.[Quantity],GL.[UnitPrice], GL.[Debit] AS AMOUNT,'' AS COMMENTS,GETDATE() AS ACTTIMESTAMP,
	'ITEMS' AS TYPED, 
	0 AS BALANCE,
	0 AS PAID,
	'' AS VENDNAME, 0 AS vendID
FROM
	[Acc_GL] AS GL
	INNER JOIN [Item] AS ITEM ON ITEM.[Id] = GL.[ItemId]
WHERE
	GL.[IsActive] = 1 AND GL.TranTypeId = 1 AND GL.[CoaId] = 6
	AND ('' = @voucherNo OR GL.[InvoiceNo] = @voucherNo)
	
	UNION

SELECT 
	TAB1.[InvoiceNo], 
	'' AS ItemCode,'' AS Name,'' AS Unit, 
	0 AS ItemId, 0 AS Quantity, 0 AS UnitPrice, SUM(AMOUNT) AS AMOUNT,MAX(TAB1.COMMENTS),MIN(TAB1.ACTTIMESTAMP) AS ACTTIMESTAMP,
	'TOTALS' AS TYPED,
	SUM(BALANCE) AS BALANCE,
	SUM(PAID) AS PAID,
	TAB1.VENDNAME, TAB1.Id AS vendID
FROM(

SELECT
	GL.[InvoiceNo],
	CASE WHEN GL.[CoaId] = 0 THEN GL.[Credit] ELSE 0 END AS AMOUNT, 
	CASE WHEN GL.[CoaId] = 0 THEN GL.[Comments] ELSE NULL END AS COMMENTS,
	GL.[ActivityTimestamp] AS ACTTIMESTAMP, 
	'TOTALS' AS TYPED, 
	CASE WHEN GL.[CoaId] = 12 THEN GL.[Credit] ELSE 0 END AS BALANCE,
	CASE WHEN GL.[CoaId] = 11 THEN GL.[Credit] ELSE 0 END AS PAID,
	VEND.[LastName] + ' '+ VEND.[FirstName] AS VENDNAME, VEND.[ID]
FROM
	[Acc_GL] AS GL
	INNER JOIN [Vendor] AS VEND ON VEND.[ID] = GL.[VendorId]
WHERE
	GL.[IsActive] = 1 AND GL.TranTypeId = 1 AND GL.[ItemId] IS NULL
	AND ('' = @voucherNo OR GL.[InvoiceNo] = @voucherNo)
) AS TAB1
GROUP BY
TAB1.[InvoiceNo],TAB1.VENDNAME, TAB1.Id
END
GO
/****** Object:  StoredProcedure [dbo].[Report_CashFlow]    Script Date: 03/08/2019 15:42:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Report_CashFlow]
(
	@dtStart datetime,
	@dtEnd datetime
)
AS
BEGIN

SELECT * FROM(
SELECT
	0 AS Debit, 0 AS Credit, SUM(GL.[Debit])-SUM(GL.[Credit]) AS OPENINGBAL, '' AS InvoiceNo, N'1/1/1900' AS ActivityTimestamp, '' AS TransType
FROM
	[Acc_GL] AS GL
	INNER JOIN [Acc_TransactionTypes] AS ACCT ON ACCT.[Id] = GL.[TranTypeId]
WHERE
	GL.CoaId = 11 AND GL.[IsActive] = 1
	AND (GL.[ActivityTimestamp] < @dtStart)

UNION

SELECT
	GL.[Debit], GL.[Credit], 0 AS OPENINGBAL, GL.[InvoiceNo], GL.[ActivityTimestamp], ACCT.[Name] AS TransType
FROM
	[Acc_GL] AS GL
	INNER JOIN [Acc_TransactionTypes] AS ACCT ON ACCT.[Id] = GL.[TranTypeId]
WHERE
	GL.CoaId = 11 AND GL.[IsActive] = 1
	AND (GL.[ActivityTimestamp] >= @dtStart AND GL.[ActivityTimestamp] <= @dtEnd)
) AS TBL
ORDER BY
	TBL.[ActivityTimestamp]

END
GO
/****** Object:  StoredProcedure [dbo].[Report_AccountsReceivable]    Script Date: 03/08/2019 15:42:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Report_AccountsReceivable]
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
GO
/****** Object:  StoredProcedure [dbo].[Report_AccountsPayable]    Script Date: 03/08/2019 15:42:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Report_AccountsPayable]
(
	@vendorID int
)
AS
BEGIN

SELECT
	SUM(ISNULL(GL.[Credit],0))-SUM(ISNULL(GL.[Debit],0)) AS AMOUNT,
	VEND.[LastName] + ' '+ VEND.[FirstName] AS VENDOR
FROM
	[Acc_GL] AS GL
	INNER JOIN [Vendor] AS VEND ON VEND.[ID] = GL.[VendorId]
WHERE
	GL.[IsActive] = 1 AND GL.CoaId = 12
	AND (@vendorID = 0 OR GL.[VendorId] = @vendorID)
GROUP BY
	VEND.[LastName], VEND.[FirstName]


END
GO
/****** Object:  StoredProcedure [dbo].[ResetGL]    Script Date: 03/08/2019 15:42:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[ResetGL]
AS
BEGIN 
truncate table Acc_GL
truncate table Acc_GLTxLinks
END
GO
/****** Object:  StoredProcedure [dbo].[GetVendorBalance]    Script Date: 03/08/2019 15:42:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetVendorBalance]
(
	@vendID int
)
AS
BEGIN
SELECT
	SUM(ISNULL(GL.[CREDIT],0))-SUM(ISNULL(GL.[DEBIT],0)) AS Balance,GL.[VendorId], VEND.[FirstName] + ' ' + VEND.[LastName] AS customer
FROM 
	[Acc_GL] AS GL
	INNER JOIN [Vendor] AS VEND ON VEND.[Id] = GL.[VendorId]
WHERE
	[CoaId] = 12
	AND (@vendID = 0 OR GL.[VendorId] = @vendID)
GROUP BY
	GL.[VendorId], VEND.[FirstName], VEND.[LastName]

END
GO
/****** Object:  StoredProcedure [dbo].[GetSubscriptionVoucherList]    Script Date: 03/08/2019 15:42:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetSubscriptionVoucherList]
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
GO
/****** Object:  StoredProcedure [dbo].[GetSubscriptionList]    Script Date: 03/08/2019 15:42:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetSubscriptionList]
AS
BEGIN
SELECT sub.Id subid,LastName,FirstName,Phone ,sub.SubscriptionAmount,sub.DueDate,sub.StartDate FROM Subscription sub 
INNER JOIN Customer cus ON sub.CustId=cus.Id
WHERE cus.IsActive=1 AND sub.IsActive=1
END
GO
/****** Object:  StoredProcedure [dbo].[GetSubscriptionByVoucherNo]    Script Date: 03/08/2019 15:42:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetSubscriptionByVoucherNo]
@voucherNo NVARCHAR(50)
AS
BEGIN

SELECT
	GL.[Debit] AS Amount, CST.[LastName] + ' '+ CST.[FirstName] AS Customer, GL.[ActivityTimestamp],GL.[InvoiceNo],CST.Id CustId,GL.Comments,SUBS.SubscriptionAmount --CST.Misc
FROM
	[Acc_GL] AS GL
	INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId] AND CST.[TypeId] = 2 AND GL.IsActive =1
	INNER JOIN [Subscription] AS SUBS ON SUBS.CustId=CST.Id AND SUBS.IsActive=1
WHERE
	GL.[CoaId] = 0 AND GL.[TranTypeId] = 3 AND GL.[InvoiceNo] = @voucherNo
	
END
GO
/****** Object:  StoredProcedure [dbo].[GetStockList]    Script Date: 03/08/2019 15:42:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetStockList]
(
	@itemID as int
)
AS
BEGIN

SELECT
	GL.[ItemId],item.[ItemCode],item.[Name],SUM(GL.[Quantity]) AS QTY
FROM
	[Acc_GL] AS GL
	INNER JOIN [Item] AS item ON item.[Id] = GL.[ItemId] AND item.[ItemTypeId] = 1
WHERE
	GL.[CoaId] = 6 AND GL.[IsActive] = 1
	AND (@itemID = 0 OR GL.[ItemId] = @itemID)
GROUP BY
	GL.[ItemId],item.[ItemCode],item.[Name]
END
GO
/****** Object:  StoredProcedure [dbo].[GetStockAlerts]    Script Date: 03/08/2019 15:42:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetStockAlerts]
AS
BEGIN

SELECT
	GL.[ItemId],item.[ItemCode],item.[Name],SUM(GL.[Quantity]) AS QTY,item.[MinQty]
FROM
	[Acc_GL] AS GL
	INNER JOIN [Item] AS item ON item.[Id] = GL.[ItemId] AND item.[ItemTypeId] = 1
WHERE
	GL.[CoaId] = 6 AND GL.[IsActive] = 1
GROUP BY
	GL.[ItemId],item.[ItemCode],item.[Name], item.[MinQty]
HAVING
	SUM(GL.[Quantity]) < item.[MinQty]

END
GO
/****** Object:  StoredProcedure [dbo].[GetServiceName]    Script Date: 03/08/2019 15:42:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetServiceName]
AS
BEGIN
select CoaId ,TreeName,Cost from Acc_COA
WHERE HeadAccount =5 AND PId=101
END
GO
/****** Object:  StoredProcedure [dbo].[GetSaleInvoiceList]    Script Date: 03/08/2019 15:42:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetSaleInvoiceList]
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
	(TABDEFAULT.ActivityTimestamp >= @startDate AND TABDEFAULT.ActivityTimestamp <= @endDate)
END
GO
/****** Object:  StoredProcedure [dbo].[GetReceiptVoucherListByVoucherNO]    Script Date: 03/08/2019 15:42:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetReceiptVoucherListByVoucherNO]
(
	@voucherNo NVARCHAR(50),
	@typeID int
	
)
AS
BEGIN

SELECT
	GL.[Debit] AS Amount, CST.[LastName] + ' '+ CST.[FirstName]  AS Customer,CST.Phone, GL.[ActivityTimestamp],GL.[InvoiceNo],CST.Id AS CustId,
	GL.[Comments],USR.[LastName] +' '+USR.FirstName AS SalePerson,
	CASE WHEN CST.[TypeId] =1 THEN 'Receivable Amount' ELSE 'Subscription Amount' END AS TYPED
FROM
	[Acc_GL] AS GL
	INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId] AND (0 = @typeID OR CST.TypeId = @typeID)
	LEFT OUTER JOIN [User] AS USR ON USR.[Id] = GL.[UserId]
WHERE
	(GL.[CoaId] = 0 AND GL.[TranTypeId] = 3) AND (@voucherNo = '' OR GL.[InvoiceNo] = @voucherNo)
	

END
GO
/****** Object:  StoredProcedure [dbo].[GetReceiptVoucherList]    Script Date: 03/08/2019 15:42:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetReceiptVoucherList]
(
	@voucherNo NVARCHAR(50),
	@typeID int,
	@dtStart datetime=null,
	@dtEnd datetime=null
)
AS
BEGIN
IF(@dtStart =null)BEGIN SET @dtStart=dateadd(dd,-365,GETDATE ()) END
IF(@dtEnd=null)BEGIN SET @dtEnd=GETDATE() END
SELECT
	GL.[Debit] AS Amount, CST.[LastName] + ' '+ CST.[FirstName]  AS Customer, GL.[ActivityTimestamp],GL.[InvoiceNo],CST.Id AS CustId,
	GL.[Comments],USR.[LastName] +' '+USR.FirstName AS SalePerson,
	CASE WHEN CST.[TypeId] =1 THEN 'Receivable Amount' ELSE 'Subscription Amount' END AS TYPED
FROM
	[Acc_GL] AS GL
	INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId] AND (0 = @typeID OR CST.TypeId = @typeID)
	LEFT OUTER JOIN [User] AS USR ON USR.[Id] = GL.[UserId]
WHERE
	(GL.[CoaId] = 0 AND GL.[TranTypeId] = 3) AND (@voucherNo = '' OR GL.[InvoiceNo] = @voucherNo)
	AND (GL.ActivityTimestamp >= @dtStart AND GL.ActivityTimestamp <= @dtEnd) AND GL.IsActive=1

END
GO
/****** Object:  StoredProcedure [dbo].[GetPurchaseList]    Script Date: 03/08/2019 15:42:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetPurchaseList]
(
	@vendorID AS INT,
	@itemID AS INT,
	@dtStart datetime,
	@dtEnd datetime
)
AS
BEGIN

SELECT
	GL.[ItemId],item.[ItemCode],item.[Name],GL.[Quantity],GL.[UnitPrice],GL.[Debit],GL.[ActivityTimestamp],GL.[InvoiceNo],VEND.[FirstName],VEND.[LastName]
FROM
	[Acc_GL] AS GL
	INNER JOIN [Item] AS item ON item.[Id] = GL.[ItemId]
	INNER JOIN [Vendor] AS VEND ON VEND.[ID] = GL.[VendorId]
WHERE
	GL.[TranTypeId] = 1 AND GL.[IsActive] = 1 AND GL.[ItemId] IS NOT NULL
	AND (@vendorID = 0 OR GL.[VendorId] = @vendorID)
	AND (@itemID = 0 OR GL.[ItemId] = @itemID)
	AND ((@dtStart IS NULL OR GL.[ActivityTimestamp] >= @dtStart) AND (@dtEnd IS NULL OR GL.[ActivityTimestamp] <= @dtEnd))
END
GO
/****** Object:  StoredProcedure [dbo].[GetPaymentVoucherList]    Script Date: 03/08/2019 15:42:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetPaymentVoucherList]
(
	@voucherNo NVARCHAR(50),
	@dtStart datetime,
	@dtEnd datetime
)
AS
BEGIN

SELECT
	GL.[Debit] AS Amount, CASE WHEN CST.[FirstName] IS NULL THEN 'PAYABLE' ELSE 'RECEIVABLE' END AS HEADTYPE, 
	ISNULL(CST.[FirstName],'')+ISNULL(VEND.[FirstName],'') + ' '+ ISNULL(CST.[LastName],'')+ISNULL(VEND.[LastName],'') AS Acting, 
	GL.[ActivityTimestamp],GL.[InvoiceNo],
	ISNULL(ISNULL(CST.Id,0)+ISNULL(VEND.ID,0),0) ActingId,
	CASE WHEN CST.[FirstName] IS NULL THEN 12 ELSE 10 END AS HEADTYPEID,
	ISNULL(CST.Misc,'')+ISNULL(Vend.Misc,'') Comments
FROM
	[Acc_GL] AS GL
	LEFT JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId]
	LEFT JOIN [Vendor] AS VEND ON VEND.[ID] = GL.[VendorId]
WHERE
	(GL.[TranId] IS NULL AND GL.[TranTypeId] = 4 AND (GL.[CustId] IS NOT NULL OR GL.[VendorId] IS NOT NULL ) AND (@voucherNo = '' OR GL.[InvoiceNo] = @voucherNo))
	AND GL.IsActive=1
	AND (GL.ActivityTimestamp >= @dtStart AND GL.ActivityTimestamp <= @dtEnd)
UNION
SELECT
	GL.[Debit] AS Amount, 'EXPENSE' AS HEADTYPE, COA.[TreeName] AS Acting, GL.[ActivityTimestamp],GL.[InvoiceNo],
	ISNULL(COA.CoaId,0) ActingId,ISNULL(COA.PId,0) HEADTYPEID,GL.Misc AS Comments
FROM
	[Acc_GL] AS GL
	INNER JOIN [Acc_COA] AS COA ON COA.[CoaId] = GL.[CoaId] AND COA.[HeadAccount] = 4
WHERE
	(GL.[TranTypeId] = 4 AND (GL.[CustId] IS NULL AND GL.[VendorId] IS NULL ) AND (@voucherNo = '' OR GL.[InvoiceNo] = @voucherNo))
	AND GL.IsActive=1
	AND (GL.ActivityTimestamp >= @dtStart AND GL.ActivityTimestamp <= @dtEnd)
END
GO
/****** Object:  StoredProcedure [dbo].[GetPaymentVoucherByVoucherNo]    Script Date: 03/08/2019 15:42:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetPaymentVoucherByVoucherNo]
(
	@voucherNo NVARCHAR(50)

)
AS
BEGIN

SELECT
	GL.[Debit] AS Amount, CASE WHEN CST.[FirstName] IS NULL THEN 'PAYABLE' ELSE 'RECEIVABLE' END AS HEADTYPE, 
	ISNULL(CST.[FirstName],'')+ISNULL(VEND.[FirstName],'') + ' '+ ISNULL(CST.[LastName],'')+ISNULL(VEND.[LastName],'') AS Acting, 
	GL.[ActivityTimestamp],GL.[InvoiceNo],
	ISNULL(ISNULL(CST.Id,0)+ISNULL(VEND.ID,0),0) ActingId,
	CASE WHEN CST.[FirstName] IS NULL THEN 12 ELSE 10 END AS HEADTYPEID,
	ISNULL(CST.Misc,'')+ISNULL(Vend.Misc,'') Comments
FROM
	[Acc_GL] AS GL
	LEFT JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId]
	LEFT JOIN [Vendor] AS VEND ON VEND.[ID] = GL.[VendorId]
WHERE
	(GL.[TranId] IS NULL AND GL.[TranTypeId] = 4 AND (GL.[CustId] IS NOT NULL OR GL.[VendorId] IS NOT NULL ) AND (@voucherNo = '' OR GL.[InvoiceNo] = @voucherNo))
	AND GL.IsActive=1
	
UNION
SELECT
	GL.[Debit] AS Amount, 'EXPENSE' AS HEADTYPE, COA.[TreeName] AS Acting, GL.[ActivityTimestamp],GL.[InvoiceNo],
	ISNULL(COA.CoaId,0) ActingId,ISNULL(COA.PId,0) HEADTYPEID,GL.Misc AS Comments
FROM
	[Acc_GL] AS GL
	INNER JOIN [Acc_COA] AS COA ON COA.[CoaId] = GL.[CoaId] AND COA.[HeadAccount] = 4
WHERE
	(GL.[TranTypeId] = 4 AND (GL.[CustId] IS NULL AND GL.[VendorId] IS NULL ) AND (@voucherNo = '' OR GL.[InvoiceNo] = @voucherNo))
	AND GL.IsActive=1
	
END
GO
/****** Object:  StoredProcedure [dbo].[GetLastVoucherNo]    Script Date: 03/08/2019 15:42:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetLastVoucherNo]
(
	@transTypeid int
)
AS
BEGIN

SELECT
	MAX(SUBSTRING([invoiceNo],0,charindex('-',[invoiceNo])+1)) + 
	CAST(ISNULL(MAX(Convert(INT,SUBSTRING([invoiceNo],charindex('-',[invoiceNo])+1,5))),0) AS varchar)
FROM 
	[Acc_GL] 
WHERE 
	[TranTypeId] = @transTypeid

--DECLARE @Counter INT
--IF(@transTypeid=1)
--	BEGIN
--	SET @Counter=(SELECT ISNULL(MAX(Convert(INT,SUBSTRING(invoiceNo,5,5))),0) FROM Acc_GL WHERE TranTypeId=@transTypeid)
--	SELECT 'PNV-'+CONVERT(VARCHAR(10),@Counter)
--	END

--IF(@transTypeid=2)
--	BEGIN
--	SET @Counter=(SELECT ISNULL(MAX(Convert(INT,SUBSTRING(invoiceNo,5,5))),0) FROM Acc_GL WHERE TranTypeId=@transTypeid)
--	SELECT 'SNV-'+CONVERT(VARCHAR(10),@Counter)
--	END

--IF(@transTypeid=3)
--	BEGIN
--	SET @Counter=(SELECT ISNULL(MAX(Convert(INT,SUBSTRING(invoiceNo,5,5))),0) FROM Acc_GL WHERE TranTypeId=@transTypeid)
--	SELECT 'RCT-'+CONVERT(VARCHAR(10),@Counter)
--	END

--IF(@transTypeid=4)
--	BEGIN
--	SET @Counter=(SELECT ISNULL(MAX(Convert(INT,SUBSTRING(invoiceNo,5,5))),0) FROM Acc_GL WHERE TranTypeId=@transTypeid)
--	SELECT 'PMT-'+CONVERT(VARCHAR(10),@Counter)
--	END

--IF(@transTypeid=5)
--	BEGIN
--	SET @Counter=(SELECT ISNULL(MAX(Convert(INT,SUBSTRING(invoiceNo,6,5))),0) FROM Acc_GL WHERE TranTypeId=@transTypeid)
--	SELECT 'SUBS-'+CONVERT(VARCHAR(10),@Counter)
--END

--IF(@transTypeid=6)
--	BEGIN
--	SET @Counter=(SELECT ISNULL(MAX(Convert(INT,SUBSTRING(invoiceNo,4,5))),0) FROM Acc_GL WHERE TranTypeId=@transTypeid)
--	SELECT 'BT-'+CONVERT(VARCHAR(10),@Counter)
--	END

--SELECT
--	TOP 1 GL.[invoiceNo]
--FROM 
--	[Acc_GL] AS GL 
--WHERE
--	GL.[tranTypeId] = @transTypeid
--	--AND GL.[IsActive] = 1
--ORDER BY
--	GL.GLID DESC
 END
GO
/****** Object:  StoredProcedure [dbo].[GetDailyCashFlow]    Script Date: 03/08/2019 15:42:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetDailyCashFlow]
(
	@date datetime
)
AS
BEGIN
CREATE TABLE #Acc_GL([TranTypeId] int,[CoaId] int,[UserId] INT,[VendorId] int,[CustId] int,[ItemId] int,[UnitPrice] decimal(10,2),[Quantity] decimal(10,2),[TaxPercent] decimal(10,2),[IsGift] bit,[Credit] decimal(10, 2) ,[Debit] decimal(10,2) ,[ActivityTimestamp] datetime,[IsPostpaid] bit,[IsSalesCredit] bit,[CreditPaidDate] datetime)
	     	INSERT INTO #Acc_GL
	  --   	SELECT TranTypeId ,CoaId,UserId,VendorId ,CustId,ItemId ,UnitPrice ,Quantity,TaxPercent ,IsGift ,Credit ,Debit,ActivityTimestamp ,IsPostpaid,IsSalesCredit,CreditPaidDate  FROM Acc_GL 
			--WHERE IsActive=1 AND Acc_GL.ActivityTimestamp > CONVERT(Date,@date) AND Acc_GL.ActivityTimestamp<DATEADD(HH,24,@date)--AND CONVERT(Date,Acc_GL.[ActivityTimestamp]) = CONVERT(Date,@date)
			--OR (Acc_GL.IsSalesCredit=1 AND Acc_GL.CreditPaidDate > CONVERT(Date,@Date) AND Acc_GL.CreditPaidDate<DATEADD(HH,24,@date))
			SELECT TranTypeId ,CoaId,UserId,VendorId ,CustId,ItemId ,UnitPrice ,Quantity,TaxPercent ,IsGift ,Credit ,Debit,ActivityTimestamp ,IsPostpaid,IsSalesCredit,CreditPaidDate  FROM Acc_GL 
			WHERE IsActive=1 AND (Acc_GL.ActivityTimestamp > CONVERT(Date,@date) AND Acc_GL.ActivityTimestamp<DATEADD(HH,24,@date)--AND CONVERT(Date,Acc_GL.[ActivityTimestamp]) = CONVERT(Date,@date)
			AND Acc_GL.CreditPaidDate is null)
			OR (Acc_GL.IsActive=1 AND Acc_GL.IsSalesCredit=0 AND Acc_GL.CreditPaidDate > CONVERT(Date,@Date) AND Acc_GL.CreditPaidDate<DATEADD(HH,24,@date))

CREATE TABLE #temp (TypeName varchar(50),ID int ,Name varchar(50),Quantity decimal(10,2),UnitPrice decimal(10,2),AMOUNT decimal(10,2),TAX decimal(10,2),TYPEID INT , GROUPID INT)

BEGIN
INSERT INTO #temp 

SELECT 
	CASE WHEN COUNT(TAB.[AMOUNT]) = 0 THEN 'OPENING' ELSE TAB.[TYPENAME] END AS TYPENAME,
	0 AS ID, DATENAME(dw,@date) AS NAME , 0 AS Quantity, 0 AS UnitPrice, ISNULL(TAB.AMOUNT,0) AS AMOUNT, 0 AS TAX, 0 AS TYPEID, 0 AS GROUPID
FROM
(SELECT SUM(ISNULL(GL.[Debit],0)) - SUM(ISNULL(GL.[Credit],0)) AS AMOUNT, 'OPENING' AS TYPENAME FROM [Acc_GL] AS GL
WHERE 
	GL.[CoaId] = 11 AND GL.[isactive] = 1 AND ( (GL.IsSalesCredit=0 OR GL.IsSalesCredit IS NULL) AND GL.[ActivityTimestamp] < CONVERT(Date,@date) AND GL.CreditPaidDate IS NULL)
	OR (GL.[CoaId] = 11 AND GL.[isactive] = 1 AND GL.IsSalesCredit=0 AND GL.CreditPaidDate<CONVERT(Date,@date))) AS TAB
GROUP BY TAB.[TYPENAME],TAB.AMOUNT
END



INSERT INTO #temp 
SELECT 
	--CASE WHEN COUNT(TAB.[AMOUNT]) = 0 THEN 'Sales' ELSE TAB.[TYPENAME] END AS TYPENAME,
	'Sales' AS TYPENAME,
	ISNULL(TAB.[ID],0) AS ID, ISNULL(TAB.[NAME],'') AS NAME , ISNULL(TAB.[Quantity],0) AS Quantity, ISNULL(TAB.[UnitPrice],0) AS UnitPrice, ISNULL(TAB.AMOUNT,0) AS AMOUNT, ISNULL(TAB.[TAX] ,0)AS TAX, 1 AS TYPEID, 1 AS GROUPID
	FROM
	(SELECT GL.[ItemId] AS [ID], IT.[Name] AS [NAME], GL.[Quantity], GL.[UnitPrice], GL.[Credit]+GL.[TaxPercent] AS AMOUNT, GL.[TaxPercent] AS TAX, 'Sales' AS TYPENAME
	FROM #Acc_GL AS GL INNER JOIN [Item] AS IT ON IT.[Id] = GL.[ItemId]
	WHERE
	GL.[CoaId] = 14 AND GL.[TranTypeId] = 2  AND GL.[ItemId] IS NOT NULL AND (IsSalesCredit=0 OR (IsSalesCredit=1 AND CreditPaidDate=@date))
	--AND CONVERT(Date,GL.[ActivityTimestamp]) = CONVERT(Date,@date)
	) AS TAB
	--GROUP BY
	--TAB.[ID],TAB.[NAME],TAB.[AMOUNT],TAB.[TYPENAME],TAB.[Quantity],TAB.[UnitPrice],TAB.[TAX]

INSERT INTO #temp 	
SELECT 
	--CASE WHEN COUNT(TAB.[AMOUNT]) = 0 THEN 'Shop' ELSE TAB.[TYPENAME] END AS TYPENAME,
	'Shop' AS TYPENAME,
	ISNULL(TAB.[ID],0) AS ID, ISNULL(TAB.[NAME],'') AS NAME , ISNULL(TAB.[Quantity],0) AS Quantity, ISNULL(TAB.[UnitPrice],0) AS UnitPrice, ISNULL(TAB.AMOUNT,0) AS AMOUNT, ISNULL(TAB.[TAX] ,0)AS TAX, 3 AS TYPEID, 1 AS GROUPID
	FROM 
	(SELECT COA.[CoaId] AS [ID], COA.[TreeName] AS [NAME], GL.[Quantity], GL.[UnitPrice], GL.[Credit] AS AMOUNT, GL.[TaxPercent] AS TAX, 'Shop' AS TYPENAME
	FROM #Acc_GL AS GL INNER JOIN [Acc_COA] AS COA ON COA.[CoaId] = GL.[CoaId]
	WHERE GL.[TranTypeId] = 2  AND GL.[ItemId] IS NULL AND GL.[IsPostpaid] = 1 AND (IsSalesCredit=0 OR (IsSalesCredit=1 AND CreditPaidDate=@date))
	--AND CONVERT(Date,GL.[ActivityTimestamp]) = CONVERT(Date,@date)
	) AS TAB
	--GROUP BY
	--TAB.[ID],TAB.[NAME],TAB.[AMOUNT],TAB.[TYPENAME],TAB.[Quantity],TAB.[UnitPrice],TAB.[TAX]


INSERT INTO #temp 
SELECT 
	CASE WHEN COUNT(TAB.[AMOUNT]) = 0 THEN 'Monthly' ELSE TAB.[TYPENAME] END AS TYPENAME,
	ISNULL(TAB.[ID],0) AS ID, ISNULL(TAB.[NAME],'') AS NAME , ISNULL(TAB.[Quantity],0) AS Quantity, 0 AS UnitPrice, ISNULL(TAB.AMOUNT,0) AS AMOUNT, 0 AS TAX, 2 AS TYPEID, 1 AS GROUPID
	FROM
	(SELECT GL.[CoaId] AS [ID] , CST.[LastName] + ' '+ CST.[FirstName] AS [NAME] ,GL.Quantity, GL.[Debit] AS AMOUNT,'Monthly' AS TYPENAME
	FROM #Acc_GL AS GL INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId]
	WHERE GL.[CoaId] = 11 AND GL.[TranTypeId] = 3 AND GL.[IsPostpaid] = 1
	--AND CONVERT(Date,GL.[ActivityTimestamp]) = CONVERT(Date,@date)
	) AS TAB
	GROUP BY
	TAB.[ID],TAB.[NAME],TAB.Quantity,TAB.[AMOUNT],TAB.[TYPENAME]

INSERT INTO #temp 
SELECT 
	CASE WHEN COUNT(TAB.[AMOUNT]) = 0 THEN 'DEPOSIT' ELSE TAB.[TYPENAME] END AS TYPENAME,
	0 AS ID, 'DEPOSIT' AS NAME , 0 AS Quantity, 0 AS UnitPrice, ISNULL(TAB.AMOUNT,0) AS AMOUNT, 0 AS TAX, 4 AS TYPEID, 2 AS GROUPID
	FROM
	(SELECT SUM(ISNULL(GL.[Debit],0)) - SUM(ISNULL(GL.[Credit],0)) AS AMOUNT, 'DEPOSIT' AS TYPENAME
	FROM #Acc_GL AS GL
	WHERE  GL.[CoaId] = 11 AND GL.[TranTypeId] IN (6) --AND GL.[isactive] = 1 
	--AND CONVERT(Date,GL.[ActivityTimestamp]) = CONVERT(Date,@date)
	) AS TAB
	GROUP BY
	TAB.[TYPENAME],TAB.AMOUNT

INSERT INTO #temp 
SELECT 
	CASE WHEN COUNT(TAB.[AMOUNT]) = 0 THEN 'CASHOUT' ELSE TAB.[TYPENAME] END AS TYPENAME,
	ISNULL(TAB.[ID],0) AS ID, ISNULL(TAB.[NAME],'') AS NAME , 0 AS Quantity, 0 AS UnitPrice, -ISNULL(TAB.AMOUNT,0) AS AMOUNT, 0 AS TAX, ISNULL(TAB.[TYPEID],5) AS TYPEID, ISNULL(TAB.[GROUPID],3) AS GROUPID
	FROM
	(SELECT GL.[VendorId] AS [ID] , VEND.[LastName] + ' '+ VEND.[FirstName] AS [NAME] , SUM(GL.[Credit]) AS AMOUNT, 'CASHOUT' AS TYPENAME,4 AS TYPEID, 3 AS GROUPID
	FROM #Acc_GL AS GL INNER JOIN [Vendor] AS VEND ON VEND.[ID] = GL.[VendorId]
	WHERE
	GL.[CoaId] = 11 AND GL.[TranTypeId] IN (1,4) AND GL.[VendorId] IS NOT NULL
	--AND CONVERT(Date,GL.[ActivityTimestamp]) = CONVERT(Date,@date)
	GROUP BY
	GL.[VendorId], VEND.[LastName], VEND.[FirstName]

	UNION

	SELECT 
	COA.[CoaId] AS [ID], COA.[TreeName] AS [NAME] , SUM(GL.[Debit]) AS AMOUNT, 'CASHOUT' AS TYPENAME, 5 AS TYPEID, 3 AS GROUPID
	FROM #Acc_GL AS GL INNER JOIN [Acc_COA] AS COA ON COA.[CoaId] = GL.[CoaId] AND COA.[HeadAccount] = 4
	WHERE
	GL.[TranTypeId] IN (4)  AND GL.[VendorId] IS NULL
	AND CONVERT(Date,GL.[ActivityTimestamp]) = CONVERT(Date,@date)
	GROUP BY
	COA.[CoaId], COA.[TreeName]
	) AS TAB
	GROUP BY
	TAB.[TYPENAME],TAB.[ID],TAB.[NAME],TAB.AMOUNT,TAB.[TYPEID],TAB.[GROUPID]

DROP TABLE #Acc_GL

IF((SELECT COUNT(*) FROM #temp WHERE TypeName='Sales')=0)
BEGIN insert into #temp 
select 'Sales' AS TypeName,0 ,'', 0,0,0,0,1,1  --group by TypeName
END

IF((SELECT COUNT(*) FROM #temp WHERE TypeName='Shop')=0)
BEGIN insert into #temp 
select 'Shop' AS TypeName,0 ,'', 0,0,0,0,3,1  --group by TypeName
END

IF((SELECT COUNT(*) FROM #temp WHERE TypeName='Monthly')=0)
BEGIN insert into #temp 
select 'Monthly' AS TypeName,0 ,'' , 0,0,0,0,2,1  --group by TypeName
END


IF((SELECT COUNT(*) FROM #temp WHERE TypeName='CASHOUT')=0)
BEGIN insert into #temp 
select 'CASHOUT' AS TypeName,0 ,'' , 0,0,0,0,5,3  --group by TypeName
END


IF((SELECT COUNT(*) FROM #temp WHERE TypeName='DEPOSIT')=0)
BEGIN insert into #temp 
select 'DEPOSIT' AS TypeName,0 ,'' , 0,0,0,0,4,2  --group by TypeName
END
--insert into #temp 
--select CASE WHEN COUNT(1)>0 THEN MAX('Monthly Collected') ELSE 'Monthly Collected' END TypeName,0 ,0 , 0,0,ISNULL(SUM(AMOUNT),0)AMOUNT,0,0,0 from #temp where TypeName ='Monthly' --group by TypeName

--insert into #temp 
--select CASE WHEN COUNT(1)>0 THEN MAX('Shop Collected') ELSE 'Shop Collected' END TypeName,0 ,0 , 0,0,ISNULL(SUM(AMOUNT),0)AMOUNT,0,0,0 from #temp where TypeName ='Shop' --group by TypeName

--insert into #temp 
--select CASE WHEN COUNT(1)>0 THEN MAX('Sales Collected') ELSE 'Sales Collected' END TypeName,0 ,0 , 0,0,ISNULL(SUM(AMOUNT),0)AMOUNT,0,0,0 from #temp where TypeName ='Sales' --group by TypeName


--insert into #temp 
--select CASE WHEN COUNT(1)>0 THEN MAX('TOTAL CASH OUT') ELSE 'TOTAL CASH OUT' END TypeName,0 ,0 , 0,0,ISNULL(SUM(AMOUNT),0)AMOUNT,0,0,0 from #temp where TypeName ='CASHOUT' --group by TypeName


--insert into #temp 
--select CASE WHEN COUNT(1)>0 THEN MAX('TOTAL CASH IN HAND') ELSE 'TOTAL CASH IN HAND' END TypeName,0 ,0 , 0,0,ISNULL(SUM(AMOUNT),0)AMOUNT,0,0,0 from #temp where TypeName IN('Sales','Shop','Monthly','OPENING','CASHOUT','DEPOSIT') --group by TypeName

Select * from #temp

DROP TABLE #temp

END
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerBalance]    Script Date: 03/08/2019 15:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetCustomerBalance]
(
	@cstID int
)
AS
BEGIN
SELECT
	SUM(ISNULL(GL.[DEBIT],0))-SUM(ISNULL(GL.[CREDIT],0)) AS Balance,GL.[CustId], CST.[FirstName] + ' ' + CST.[LastName] AS customer
FROM 
	[Acc_GL] AS GL
	INNER JOIN [Customer] AS CST ON CST.[Id] = GL.[CustId]
WHERE
	[CoaId] = 10
	AND (@cstID = 0 OR GL.[CustId] = @cstID)
	AND GL.[IsActive] = 1
GROUP BY
	GL.[CustId], CST.[FirstName], CST.[LastName]

END
GO
/****** Object:  StoredProcedure [dbo].[GetBanksWithBalances]    Script Date: 03/08/2019 15:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetBanksWithBalances]
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
GO
/****** Object:  StoredProcedure [dbo].[Credit_UpdateUnPaidCreditSales]    Script Date: 03/08/2019 15:42:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* ITS NOT IN USED */
CREATE Procedure [dbo].[Credit_UpdateUnPaidCreditSales]
@InvoiceNo NVARCHAR(50),
@CustID INT,
@CreditPaidDate DATETIME,
@ModifiedBy INT 

AS

BEGIN 

UPDATE Acc_GL 
SET IsSalesCredit=0 ,CreditPaidDate=@CreditPaidDate ,ModifiedBy=@ModifiedBy ,ModifiedDate=GETDATE()
WHERE InvoiceNo=@InvoiceNo AND CustId=@CustID

END
GO
/****** Object:  StoredProcedure [dbo].[Credit_GetUnPaidCustomersInvoice]    Script Date: 03/08/2019 15:42:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Credit_GetUnPaidCustomersInvoice]
@CustomerID int
AS
BEGIN 
SELECT  InvoiceNo,ActivityTimestamp SalesDate,Debit Amount  FROM Acc_GL 
WHERE
TranTypeId=2 AND CoaId=11 AND CustId=@CustomerID AND IsSalesCredit=1 AND IsActive=1
END
GO
/****** Object:  StoredProcedure [dbo].[Credit_GetUnPaidCustomers]    Script Date: 03/08/2019 15:42:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Credit_GetUnPaidCustomers]

AS

BEGIN

SELECT Distinct C.Id,C.LastName+' '+C.FirstName CustomerName FROM Customer C 
INNER JOIN Acc_GL GL ON C.Id=GL.CustId
WHERE IsSalesCredit=1 AND GL.IsActive=1

END
GO
/****** Object:  StoredProcedure [dbo].[Credit_GetUnpaidCreditSales]    Script Date: 03/08/2019 15:42:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Credit_GetUnpaidCreditSales]
@ActivityStartDate DATETIME,
@ActivityEndDate DATETIME

AS

BEGIN

SELECT InvoiceNo ,C.Id CustId,C.LastName+' '+C.FirstName CustomerName,ActivityTimestamp SalesDate,Debit  Amount FROM Acc_GL GL 
INNER Join Customer C ON GL.CustId=C.Id WHERE 
GL.[TranTypeId] = 2 AND CoaId=11 AND IsSalesCredit=1 AND GL.IsActive=1
AND Gl.ActivityTimestamp>@ActivityStartDate AND Gl.ActivityTimestamp < @ActivityEndDate

END
GO
