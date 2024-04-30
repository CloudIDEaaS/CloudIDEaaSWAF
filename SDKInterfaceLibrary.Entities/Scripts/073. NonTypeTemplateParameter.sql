USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderNonTypeTemplateParameter

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.NonTypeTemplateParameter

	<References>
		<Reference>Expression</Reference>
		<Reference>IntegerValue</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>TemplateParameter</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderNonTypeTemplateParameter](
		[NonTypeTemplateParameterId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TemplateParameterId] [uniqueidentifier] NULL,
		[DefaultArgumentExpressionId] [uniqueidentifier] NULL,
		[PositionIntegerValueId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[IsExpandedParameterPack] [bit] NOT NULL,
		[IsPackExpansion] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderNonTypeTemplateParameter] PRIMARY KEY CLUSTERED
(
	[NonTypeTemplateParameterId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
