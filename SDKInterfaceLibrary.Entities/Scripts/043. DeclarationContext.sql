USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderDeclarationContext

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.DeclarationContext

	<References>
		<Reference>Comment</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Declaration</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderDeclarationContext](
		[DeclarationContextId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationId] [uniqueidentifier] NULL,
		[CommentId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[IsAnonymous] [bit] NOT NULL,
		[Access] [nvarchar](255) NOT NULL,
		[DebugText] [ntext] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderDeclarationContext] PRIMARY KEY CLUSTERED
(
	[DeclarationContextId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
