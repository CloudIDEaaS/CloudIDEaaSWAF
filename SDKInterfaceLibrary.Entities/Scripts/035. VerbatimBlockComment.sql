USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderVerbatimBlockComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.VerbatimBlockComment

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>BlockCommandComment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderVerbatimBlockComment](
		[VerbatimBlockCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[BlockCommandCommentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderVerbatimBlockComment] PRIMARY KEY CLUSTERED
(
	[VerbatimBlockCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
