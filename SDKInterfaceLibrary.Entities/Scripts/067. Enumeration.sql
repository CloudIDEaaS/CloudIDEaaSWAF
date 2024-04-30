USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderEnumeration

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.Enumeration

	<References>
		<Reference>DeclarationContext</Reference>
		<Reference>Type</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>DeclarationContext</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderEnumeration](
		[EnumerationId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationContextId] [uniqueidentifier] NULL,
		[OwningDeclarationContextId] [uniqueidentifier] NULL,
		[BuiltInTypeId] [uniqueidentifier] NULL,
		[EnumIndex] [int] NOT NULL,
		[LocationIdentifier] [bigint] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderEnumeration] PRIMARY KEY CLUSTERED
(
	[EnumerationId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
