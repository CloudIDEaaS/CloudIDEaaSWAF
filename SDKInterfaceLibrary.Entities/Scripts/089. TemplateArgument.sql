USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderTemplateArgument

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TemplateArgument

	<References>
		<Reference>ClassTemplateSpecialization</Reference>
		<Reference>DependentTemplateSpecializationType</Reference>
		<Reference>VarTemplateSpecialization</Reference>
		<Reference>TemplateSpecializationType</Reference>
		<Reference>FunctionTemplateSpecialization</Reference>
		<Reference>Declaration</Reference>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTemplateArgument](
		[TemplateArgumentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[OwningClassTemplateSpecializationId] [uniqueidentifier] NULL,
		[OwningDependentTemplateSpecializationTypeId] [uniqueidentifier] NULL,
		[OwningVarTemplateSpecializationId] [uniqueidentifier] NULL,
		[OwningTemplateSpecializationTypeId] [uniqueidentifier] NULL,
		[OwningFunctionTemplateSpecializationId] [uniqueidentifier] NULL,
		[DeclarationId] [uniqueidentifier] NULL,
		[TypeQualifiedTypeId] [uniqueidentifier] NULL,
		[Integral] [int] NOT NULL,
		[Kind] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderTemplateArgument] PRIMARY KEY CLUSTERED
(
	[TemplateArgumentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
