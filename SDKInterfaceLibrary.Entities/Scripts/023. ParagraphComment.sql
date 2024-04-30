USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderParagraphComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.ParagraphComment

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>BlockContentComment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderParagraphComment](
		[ParagraphCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[BlockContentCommentId] [uniqueidentifier] NULL,
		[IsWhitespace] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderParagraphComment] PRIMARY KEY CLUSTERED
(
	[ParagraphCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
