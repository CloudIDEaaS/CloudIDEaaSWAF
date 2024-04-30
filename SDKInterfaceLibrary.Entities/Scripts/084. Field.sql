USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderField

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.Field

	<References>
		<Reference>Class</Reference>
		<Reference>IntegerValue</Reference>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Declaration</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderField](
		[FieldId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationId] [uniqueidentifier] NULL,
		[OwningClassId] [uniqueidentifier] NULL,
		[BitWidthIntegerValueId] [uniqueidentifier] NULL,
		[QualifiedTypeId] [uniqueidentifier] NULL,
		[FieldIndex] [int] NOT NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[IsBitField] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderField] PRIMARY KEY CLUSTERED
(
	[FieldId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
