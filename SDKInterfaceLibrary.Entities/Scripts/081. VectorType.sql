USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderVectorType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.VectorType

	<References>
		<Reference>QualifiedType</Reference>
		<Reference>IntegerValue</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderVectorType](
		[VectorTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[ElementTypeQualifiedTypeId] [uniqueidentifier] NULL,
		[NumElementsIntegerValueId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderVectorType] PRIMARY KEY CLUSTERED
(
	[VectorTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
