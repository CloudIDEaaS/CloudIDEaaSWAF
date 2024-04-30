USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderClassTemplateSpecialization

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.ClassTemplateSpecialization

	<References>
		<Reference>ClassTemplate</Reference>
		<Reference>ClassTemplate</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Class</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderClassTemplateSpecialization](
		[ClassTemplateSpecializationId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[ClassId] [uniqueidentifier] NULL,
		[OwningClassTemplateId] [uniqueidentifier] NULL,
		[TemplatedDeclClassTemplateId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[SpecializationKind] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderClassTemplateSpecialization] PRIMARY KEY CLUSTERED
(
	[ClassTemplateSpecializationId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
