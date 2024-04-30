USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderEnumerationItem

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.EnumerationItem

	<References>
		<Reference>Enumeration</Reference>
		<Reference>IntegerValue</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Declaration</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderEnumerationItem](
		[EnumerationItemId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationId] [uniqueidentifier] NULL,
		[OwningEnumerationId] [uniqueidentifier] NULL,
		[ValueIntegerValueId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[Expression] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderEnumerationItem] PRIMARY KEY CLUSTERED
(
	[EnumerationItemId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
