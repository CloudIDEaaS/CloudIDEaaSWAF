USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderDecayedType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.DecayedType

	<References>
		<Reference>QualifiedType</Reference>
		<Reference>QualifiedType</Reference>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderDecayedType](
		[DecayedTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[DecayedQualifiedTypeId] [uniqueidentifier] NULL,
		[OriginalQualifiedTypeId] [uniqueidentifier] NULL,
		[PointeeQualifiedTypeId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderDecayedType] PRIMARY KEY CLUSTERED
(
	[DecayedTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
