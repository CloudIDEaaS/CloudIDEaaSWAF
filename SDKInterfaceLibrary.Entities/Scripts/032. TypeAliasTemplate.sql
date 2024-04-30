USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderTypeAliasTemplate

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TypeAliasTemplate

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>Template</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTypeAliasTemplate](
		[TypeAliasTemplateId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TemplateId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderTypeAliasTemplate] PRIMARY KEY CLUSTERED
(
	[TypeAliasTemplateId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
