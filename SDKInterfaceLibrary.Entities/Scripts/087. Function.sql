USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderFunction

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.Function

	<References>
		<Reference>DeclarationContext</Reference>
		<Reference>Comment</Reference>
		<Reference>Function</Reference>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Declaration</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderFunction](
		[FunctionId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationId] [uniqueidentifier] NULL,
		[OwningDeclarationContextId] [uniqueidentifier] NULL,
		[CommentId] [uniqueidentifier] NULL,
		[InstantiatedFromFunctionId] [uniqueidentifier] NULL,
		[ReturnTypeQualifiedTypeId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[FunctionName] [nvarchar](255) NOT NULL,
		[Access] [nvarchar](255) NOT NULL,
		[CallingConvention] [nvarchar](255) NOT NULL,
		[HasThisReturn] [bit] NOT NULL,
		[IsDeleted] [bit] NOT NULL,
		[IsInline] [bit] NOT NULL,
		[IsPure] [bit] NOT NULL,
		[IsReturnIndirect] [bit] NOT NULL,
		[IsVariadic] [bit] NOT NULL,
		[OperatorKind] [nvarchar](255) NOT NULL,
		[Mangled] [nvarchar](255) NOT NULL,
		[Signature] [nvarchar](255) NOT NULL,
		[DebugText] [ntext] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderFunction] PRIMARY KEY CLUSTERED
(
	[FunctionId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
