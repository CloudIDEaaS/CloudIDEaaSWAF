USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderBlockCommandComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.BlockCommandComment

	<References>
		<Reference>ParagraphComment</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>BlockContentComment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderBlockCommandComment](
		[BlockCommandCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[BlockContentCommentId] [uniqueidentifier] NULL,
		[ParagraphCommentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderBlockCommandComment] PRIMARY KEY CLUSTERED
(
	[BlockCommandCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
