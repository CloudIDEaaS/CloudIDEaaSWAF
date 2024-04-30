USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderNamespace

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.Namespace

	<References>
		<Reference>DeclarationContext</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>DeclarationContext</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderNamespace](
		[NamespaceId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationContextId] [uniqueidentifier] NULL,
		[OwningDeclarationContextId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[IsInline] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderNamespace] PRIMARY KEY CLUSTERED
(
	[NamespaceId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
