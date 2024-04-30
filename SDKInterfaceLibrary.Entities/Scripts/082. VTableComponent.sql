USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderVTableComponent

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.VTableComponent

	<References>
		<Reference>VTableLayout</Reference>
		<Reference>Declaration</Reference>
		<Reference>IntegerValue</Reference>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderVTableComponent](
		[VTableComponentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[OwningVTableLayoutId] [uniqueidentifier] NULL,
		[DeclarationId] [uniqueidentifier] NULL,
		[OffsetIntegerValueId] [uniqueidentifier] NULL,
		[Kind] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderVTableComponent] PRIMARY KEY CLUSTERED
(
	[VTableComponentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
