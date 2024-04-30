USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderInlineCommandComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.InlineCommandComment

	<References>
		<Reference>IntegerValue</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>InlineContentComment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderInlineCommandComment](
		[InlineCommandCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[InlineContentCommentId] [uniqueidentifier] NULL,
		[CommandIdIntegerValueId] [uniqueidentifier] NULL,
		[CommentRenderKind] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderInlineCommandComment] PRIMARY KEY CLUSTERED
(
	[InlineCommandCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
