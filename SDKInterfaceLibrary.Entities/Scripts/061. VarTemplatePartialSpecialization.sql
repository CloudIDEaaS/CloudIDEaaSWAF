USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderVarTemplatePartialSpecialization

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.VarTemplatePartialSpecialization

	<References>
		<Reference>VarTemplate</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>VarTemplateSpecialization</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderVarTemplatePartialSpecialization](
		[VarTemplatePartialSpecializationId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[VarTemplateSpecializationId] [uniqueidentifier] NULL,
		[OwningVarTemplateId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderVarTemplatePartialSpecialization] PRIMARY KEY CLUSTERED
(
	[VarTemplatePartialSpecializationId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
