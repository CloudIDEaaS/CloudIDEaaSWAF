USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderClass

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.Class

	<References>
		<Reference>DeclarationContext</Reference>
		<Reference>ClassLayout</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>DeclarationContext</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderClass](
		[ClassId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationContextId] [uniqueidentifier] NULL,
		[OwningDeclarationContextId] [uniqueidentifier] NULL,
		[LayoutClassLayoutId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[ClassName] [nvarchar](255) NOT NULL,
		[HasNonTrivialCopyConstructor] [bit] NOT NULL,
		[HasNonTrivialDefaultConstructor] [bit] NOT NULL,
		[HasNonTrivialDestructor] [bit] NOT NULL,
		[IsAbstract] [bit] NOT NULL,
		[IsDynamic] [bit] NOT NULL,
		[IsExternCContext] [bit] NOT NULL,
		[IsPOD] [bit] NOT NULL,
		[IsPolymorphic] [bit] NOT NULL,
		[IsUnion] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderClass] PRIMARY KEY CLUSTERED
(
	[ClassId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
