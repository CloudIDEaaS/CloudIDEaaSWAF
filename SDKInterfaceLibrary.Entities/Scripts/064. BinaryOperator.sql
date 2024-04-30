USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderBinaryOperator

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.BinaryOperator

	<References>
		<Reference>Expression</Reference>
		<Reference>Expression</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Expression</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderBinaryOperator](
		[BinaryOperatorId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[ExpressionId] [uniqueidentifier] NULL,
		[LHSExpressionId] [uniqueidentifier] NULL,
		[RHSExpressionId] [uniqueidentifier] NULL,
		[OpCodeString] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderBinaryOperator] PRIMARY KEY CLUSTERED
(
	[BinaryOperatorId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
