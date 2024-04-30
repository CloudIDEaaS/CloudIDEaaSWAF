USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderBlockContentComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.BlockContentComment

	<References>
		<Reference>Comment</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Comment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderBlockContentComment](
		[BlockContentCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[CommentId] [uniqueidentifier] NULL,
		[OwningCommentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderBlockContentComment] PRIMARY KEY CLUSTERED
(
	[BlockContentCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
