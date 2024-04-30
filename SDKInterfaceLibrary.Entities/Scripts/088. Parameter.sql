USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderParameter

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.Parameter

	<References>
		<Reference>FunctionType</Reference>
		<Reference>Function</Reference>
		<Reference>Comment</Reference>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Declaration</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderParameter](
		[ParameterId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationId] [uniqueidentifier] NULL,
		[OwningFunctionTypeId] [uniqueidentifier] NULL,
		[OwningFunctionId] [uniqueidentifier] NULL,
		[CommentId] [uniqueidentifier] NULL,
		[QualifiedTypeId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[DebugText] [ntext] NOT NULL,
		[Access] [nvarchar](255) NOT NULL,
		[DefaultArgument] [nvarchar](255) NOT NULL,
		[IsIndirect] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderParameter] PRIMARY KEY CLUSTERED
(
	[ParameterId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
