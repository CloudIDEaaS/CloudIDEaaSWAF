USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderInlineContentComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.InlineContentComment

	<References>
		<Reference>Comment</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Comment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderInlineContentComment](
		[InlineContentCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[CommentId] [uniqueidentifier] NULL,
		[OwningCommentId] [uniqueidentifier] NULL,
		[HasTrailingNewline] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderInlineContentComment] PRIMARY KEY CLUSTERED
(
	[InlineContentCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
