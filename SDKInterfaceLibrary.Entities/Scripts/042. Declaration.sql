USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderDeclaration

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.Declaration

	<References>
		<Reference>Declaration</Reference>
		<Reference>Declaration</Reference>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderDeclaration](
		[DeclarationId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[OwningDeclarationId] [uniqueidentifier] NULL,
		[CompleteDeclarationId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[DefinitionOrder] [int] NOT NULL,
		[LineNumberStart] [int] NOT NULL,
		[LineNumberEnd] [int] NOT NULL,
		[IsDependent] [bit] NOT NULL,
		[IsImplicit] [bit] NOT NULL,
		[IsIncomplete] [bit] NOT NULL,
		[Kind] [nvarchar](255) NOT NULL,
		[USR] [nvarchar](255) NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderDeclaration] PRIMARY KEY CLUSTERED
(
	[DeclarationId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
