USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderMethod

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.Method

	<References>
		<Reference>Class</Reference>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Function</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderMethod](
		[MethodId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[FunctionId] [uniqueidentifier] NULL,
		[OwningClassId] [uniqueidentifier] NULL,
		[ConversionTypeQualifiedTypeId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[IsConst] [bit] NOT NULL,
		[IsCopyConstructor] [bit] NOT NULL,
		[IsDefaultConstructor] [bit] NOT NULL,
		[IsExplicit] [bit] NOT NULL,
		[IsImplicit] [bit] NOT NULL,
		[IsMoveConstructor] [bit] NOT NULL,
		[IsOverride] [bit] NOT NULL,
		[IsPure] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderMethod] PRIMARY KEY CLUSTERED
(
	[MethodId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
