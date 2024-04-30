USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderTranslationUnit

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TranslationUnit

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>Namespace</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTranslationUnit](
		[TranslationUnitId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[NamespaceId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[FileName] [nvarchar](255) NOT NULL,
		[IsSystemHeader] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderTranslationUnit] PRIMARY KEY CLUSTERED
(
	[TranslationUnitId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
