USE [SDKInterfaceLibrary]
GO

/*
	Table: tblSDKHeaderIntegerValue

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.IntegerValue

	<References>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderIntegerValue](
		[IntegerValueId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[Value] [bigint] NOT NULL,
		[StringValue] [nvarchar](255) NOT NULL,
		[ValueType] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderIntegerValue] PRIMARY KEY CLUSTERED
(
	[IntegerValueId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.Type

	<References>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderType](
		[TypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[Kind] [nvarchar](255) NOT NULL,
		[IsDependent] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderType] PRIMARY KEY CLUSTERED
(
	[TypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderVTableLayout

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.VTableLayout

	<References>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderVTableLayout](
		[VTableLayoutId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderVTableLayout] PRIMARY KEY CLUSTERED
(
	[VTableLayoutId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderBlockCommandCommentArgument

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.BlockCommandCommentArgument

	<References>
		<Reference>BlockCommandComment</Reference>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderBlockCommandCommentArgument](
		[BlockCommandCommentArgumentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[OwningBlockCommandCommentId] [uniqueidentifier] NULL,
		[Text] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderBlockCommandCommentArgument] PRIMARY KEY CLUSTERED
(
	[BlockCommandCommentArgumentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderBuiltinType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.BuiltinType

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderBuiltinType](
		[BuiltinTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[Type] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderBuiltinType] PRIMARY KEY CLUSTERED
(
	[BuiltinTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderBuiltinTypeExpression

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.BuiltinTypeExpression

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>Expression</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderBuiltinTypeExpression](
		[BuiltinTypeExpressionId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[ExpressionId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderBuiltinTypeExpression] PRIMARY KEY CLUSTERED
(
	[BuiltinTypeExpressionId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderCallExpr

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.CallExpr

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>Expression</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderCallExpr](
		[CallExprId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[ExpressionId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderCallExpr] PRIMARY KEY CLUSTERED
(
	[CallExprId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderClassLayout

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.ClassLayout

	<References>
		<Reference>VTableLayout</Reference>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderClassLayout](
		[ClassLayoutId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[LayoutVTableLayoutId] [uniqueidentifier] NULL,
		[ABI] [nvarchar](255) NOT NULL,
		[Alignment] [int] NOT NULL,
		[DataSize] [int] NOT NULL,
		[HasOwnVFPtr] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderClassLayout] PRIMARY KEY CLUSTERED
(
	[ClassLayoutId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderClassTemplate

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.ClassTemplate

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>Template</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderClassTemplate](
		[ClassTemplateId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TemplateId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderClassTemplate] PRIMARY KEY CLUSTERED
(
	[ClassTemplateId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderClassTemplatePartialSpecialization

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.ClassTemplatePartialSpecialization

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>ClassTemplateSpecialization</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderClassTemplatePartialSpecialization](
		[ClassTemplatePartialSpecializationId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[ClassTemplateSpecializationId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderClassTemplatePartialSpecialization] PRIMARY KEY CLUSTERED
(
	[ClassTemplatePartialSpecializationId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.Comment

	<References>
		<Reference>Declaration</Reference>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderComment](
		[CommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[ParentDeclarationId] [uniqueidentifier] NULL,
		[Kind] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderComment] PRIMARY KEY CLUSTERED
(
	[CommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderCXXConstructExpr

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.CXXConstructExpr

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>Expression</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderCXXConstructExpr](
		[CXXConstructExprId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[ExpressionId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderCXXConstructExpr] PRIMARY KEY CLUSTERED
(
	[CXXConstructExprId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderExpression

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.Expression

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>Statement</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderExpression](
		[ExpressionId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[StatementId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderExpression] PRIMARY KEY CLUSTERED
(
	[ExpressionId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderFunctionTemplate

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.FunctionTemplate

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>Template</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderFunctionTemplate](
		[FunctionTemplateId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TemplateId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderFunctionTemplate] PRIMARY KEY CLUSTERED
(
	[FunctionTemplateId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderHeader

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.Header

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>Namespace</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderHeader](
		[HeaderId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[NamespaceId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[FileName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderHeader] PRIMARY KEY CLUSTERED
(
	[HeaderId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderHTMLEndTagComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.HTMLEndTagComment

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>HTMLTagComment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderHTMLEndTagComment](
		[HTMLEndTagCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[HTMLTagCommentId] [uniqueidentifier] NULL,
		[TagName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderHTMLEndTagComment] PRIMARY KEY CLUSTERED
(
	[HTMLEndTagCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderHTMLStartTagComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.HTMLStartTagComment

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>HTMLTagComment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderHTMLStartTagComment](
		[HTMLStartTagCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[HTMLTagCommentId] [uniqueidentifier] NULL,
		[TagName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderHTMLStartTagComment] PRIMARY KEY CLUSTERED
(
	[HTMLStartTagCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderHTMLStartTagCommentAttribute

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.HTMLStartTagCommentAttribute

	<References>
		<Reference>HTMLStartTagComment</Reference>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderHTMLStartTagCommentAttribute](
		[HTMLStartTagCommentAttributeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[OwningHTMLStartTagCommentId] [uniqueidentifier] NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Value] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderHTMLStartTagCommentAttribute] PRIMARY KEY CLUSTERED
(
	[HTMLStartTagCommentAttributeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderHTMLTagComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.HTMLTagComment

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>InlineContentComment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderHTMLTagComment](
		[HTMLTagCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[InlineContentCommentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderHTMLTagComment] PRIMARY KEY CLUSTERED
(
	[HTMLTagCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderInlineCommandCommentArgument

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.InlineCommandCommentArgument

	<References>
		<Reference>InlineCommandComment</Reference>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderInlineCommandCommentArgument](
		[InlineCommandCommentArgumentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[OwningInlineCommandCommentId] [uniqueidentifier] NULL,
		[Text] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderInlineCommandCommentArgument] PRIMARY KEY CLUSTERED
(
	[InlineCommandCommentArgumentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderMacroDefinition

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.MacroDefinition

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>PreprocessedEntity</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderMacroDefinition](
		[MacroDefinitionId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[PreprocessedEntityId] [uniqueidentifier] NULL,
		[Expression] [nvarchar](255) NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[LineNumberStart] [int] NOT NULL,
		[LineNumberEnd] [int] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderMacroDefinition] PRIMARY KEY CLUSTERED
(
	[MacroDefinitionId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderPackExpansionType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.PackExpansionType

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderPackExpansionType](
		[PackExpansionTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderPackExpansionType] PRIMARY KEY CLUSTERED
(
	[PackExpansionTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderParagraphComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.ParagraphComment

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>BlockContentComment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderParagraphComment](
		[ParagraphCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[BlockContentCommentId] [uniqueidentifier] NULL,
		[IsWhitespace] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderParagraphComment] PRIMARY KEY CLUSTERED
(
	[ParagraphCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderPreprocessedEntity

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.PreprocessedEntity

	<References>
		<Reference>Declaration</Reference>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderPreprocessedEntity](
		[PreprocessedEntityId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[OwningDeclarationId] [uniqueidentifier] NULL,
		[Kind] [nvarchar](255) NOT NULL,
		[MacroLocation] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderPreprocessedEntity] PRIMARY KEY CLUSTERED
(
	[PreprocessedEntityId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderQualifiedType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.QualifiedType

	<References>
		<Reference>Type</Reference>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderQualifiedType](
		[QualifiedTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[Qualifiers] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderQualifiedType] PRIMARY KEY CLUSTERED
(
	[QualifiedTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderRawComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.RawComment

	<References>
		<Reference>FullComment</Reference>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderRawComment](
		[RawCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[CommentBlockFullCommentId] [uniqueidentifier] NULL,
		[BriefText] [nvarchar](255) NOT NULL,
		[Text] [nvarchar](255) NOT NULL,
		[Kind] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderRawComment] PRIMARY KEY CLUSTERED
(
	[RawCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderStatement

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.Statement

	<References>
		<Reference>Declaration</Reference>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderStatement](
		[StatementId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclDeclarationId] [uniqueidentifier] NULL,
		[Class] [nvarchar](255) NOT NULL,
		[String] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderStatement] PRIMARY KEY CLUSTERED
(
	[StatementId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderTemplateTemplateParameter

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TemplateTemplateParameter

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>Template</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTemplateTemplateParameter](
		[TemplateTemplateParameterId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TemplateId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[IsExpandedParameterPack] [bit] NOT NULL,
		[IsPackExpansion] [bit] NOT NULL,
		[IsParameterPack] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderTemplateTemplateParameter] PRIMARY KEY CLUSTERED
(
	[TemplateTemplateParameterId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderTextComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TextComment

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>InlineContentComment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTextComment](
		[TextCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[InlineContentCommentId] [uniqueidentifier] NULL,
		[Text] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderTextComment] PRIMARY KEY CLUSTERED
(
	[TextCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderTParamCommandComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TParamCommandComment

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>BlockCommandComment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTParamCommandComment](
		[TParamCommandCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[BlockCommandCommentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderTParamCommandComment] PRIMARY KEY CLUSTERED
(
	[TParamCommandCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderTranslationUnit

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TranslationUnit

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>Namespace</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTranslationUnit](
		[TranslationUnitId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[NamespaceId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[FileName] [nvarchar](255) NOT NULL,
		[IsSystemHeader] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderTranslationUnit] PRIMARY KEY CLUSTERED
(
	[TranslationUnitId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderTypeAliasTemplate

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TypeAliasTemplate

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>Template</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTypeAliasTemplate](
		[TypeAliasTemplateId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TemplateId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderTypeAliasTemplate] PRIMARY KEY CLUSTERED
(
	[TypeAliasTemplateId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderTypeDefDecl

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TypeDefDecl

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>TypeDefNameDecl</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTypeDefDecl](
		[TypeDefDeclId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeDefNameDeclId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderTypeDefDecl] PRIMARY KEY CLUSTERED
(
	[TypeDefDeclId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderVarTemplate

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.VarTemplate

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>Template</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderVarTemplate](
		[VarTemplateId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TemplateId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderVarTemplate] PRIMARY KEY CLUSTERED
(
	[VarTemplateId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderVerbatimBlockComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.VerbatimBlockComment

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>BlockCommandComment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderVerbatimBlockComment](
		[VerbatimBlockCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[BlockCommandCommentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderVerbatimBlockComment] PRIMARY KEY CLUSTERED
(
	[VerbatimBlockCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderVerbatimLineComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.VerbatimLineComment

	<References>
		<Reference>HeaderFile</Reference>
		<Reference>BlockCommandComment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderVerbatimLineComment](
		[VerbatimLineCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[BlockCommandCommentId] [uniqueidentifier] NULL,
		[Text] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderVerbatimLineComment] PRIMARY KEY CLUSTERED
(
	[VerbatimLineCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderAccessSpecifierDecl

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.AccessSpecifierDecl

	<References>
		<Reference>Class</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Declaration</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderAccessSpecifierDecl](
		[AccessSpecifierDeclId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationId] [uniqueidentifier] NULL,
		[OwningClassId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderAccessSpecifierDecl] PRIMARY KEY CLUSTERED
(
	[AccessSpecifierDeclId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderArrayType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.ArrayType

	<References>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderArrayType](
		[ArrayTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[QualifiedTypeId] [uniqueidentifier] NULL,
		[ElementSize] [int] NOT NULL,
		[Size] [int] NOT NULL,
		[SizeType] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderArrayType] PRIMARY KEY CLUSTERED
(
	[ArrayTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderBaseClassSpecifier

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.BaseClassSpecifier

	<References>
		<Reference>Class</Reference>
		<Reference>Type</Reference>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderBaseClassSpecifier](
		[BaseClassSpecifierId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[OwningClassId] [uniqueidentifier] NULL,
		[TypeId] [uniqueidentifier] NULL,
		[Access] [nvarchar](255) NOT NULL,
		[IsVirtual] [bit] NOT NULL,
		[Offset] [int] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderBaseClassSpecifier] PRIMARY KEY CLUSTERED
(
	[BaseClassSpecifierId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderBlockCommandComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.BlockCommandComment

	<References>
		<Reference>ParagraphComment</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>BlockContentComment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderBlockCommandComment](
		[BlockCommandCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[BlockContentCommentId] [uniqueidentifier] NULL,
		[ParagraphCommentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderBlockCommandComment] PRIMARY KEY CLUSTERED
(
	[BlockCommandCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderBlockContentComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.BlockContentComment

	<References>
		<Reference>Comment</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Comment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderBlockContentComment](
		[BlockContentCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[CommentId] [uniqueidentifier] NULL,
		[OwningCommentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderBlockContentComment] PRIMARY KEY CLUSTERED
(
	[BlockContentCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderDeclaration

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.Declaration

	<References>
		<Reference>Declaration</Reference>
		<Reference>Declaration</Reference>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderDeclaration](
		[DeclarationId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[OwningDeclarationId] [uniqueidentifier] NULL,
		[CompleteDeclarationId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[DefinitionOrder] [int] NOT NULL,
		[LineNumberStart] [int] NOT NULL,
		[LineNumberEnd] [int] NOT NULL,
		[IsDependent] [bit] NOT NULL,
		[IsImplicit] [bit] NOT NULL,
		[IsIncomplete] [bit] NOT NULL,
		[Kind] [nvarchar](255) NOT NULL,
		[USR] [nvarchar](255) NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderDeclaration] PRIMARY KEY CLUSTERED
(
	[DeclarationId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderDeclarationContext

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.DeclarationContext

	<References>
		<Reference>Comment</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Declaration</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderDeclarationContext](
		[DeclarationContextId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationId] [uniqueidentifier] NULL,
		[CommentId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[IsAnonymous] [bit] NOT NULL,
		[Access] [nvarchar](255) NOT NULL,
		[DebugText] [ntext] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderDeclarationContext] PRIMARY KEY CLUSTERED
(
	[DeclarationContextId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/*
	Table: tblSDKHeaderDependentNameType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.DependentNameType

	<References>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderDependentNameType](
		[DependentNameTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[DesugaredQualifiedTypeId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderDependentNameType] PRIMARY KEY CLUSTERED
(
	[DependentNameTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderDependentTemplateSpecializationType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.DependentTemplateSpecializationType

	<References>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderDependentTemplateSpecializationType](
		[DependentTemplateSpecializationTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[DesugaredQualifiedTypeId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderDependentTemplateSpecializationType] PRIMARY KEY CLUSTERED
(
	[DependentTemplateSpecializationTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderFunctionTemplateSpecialization

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.FunctionTemplateSpecialization

	<References>
		<Reference>FunctionTemplate</Reference>
		<Reference>Function</Reference>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderFunctionTemplateSpecialization](
		[FunctionTemplateSpecializationId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[OwningFunctionTemplateId] [uniqueidentifier] NULL,
		[SpecializedFunctionId] [uniqueidentifier] NULL,
		[SpecializationKind] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderFunctionTemplateSpecialization] PRIMARY KEY CLUSTERED
(
	[FunctionTemplateSpecializationId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderFunctionType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.FunctionType

	<References>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderFunctionType](
		[FunctionTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[ReturnTypeQualifiedTypeId] [uniqueidentifier] NULL,
		[CallingConvention] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderFunctionType] PRIMARY KEY CLUSTERED
(
	[FunctionTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderInlineCommandComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.InlineCommandComment

	<References>
		<Reference>IntegerValue</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>InlineContentComment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderInlineCommandComment](
		[InlineCommandCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[InlineContentCommentId] [uniqueidentifier] NULL,
		[CommandIdIntegerValueId] [uniqueidentifier] NULL,
		[CommentRenderKind] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderInlineCommandComment] PRIMARY KEY CLUSTERED
(
	[InlineCommandCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderInlineContentComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.InlineContentComment

	<References>
		<Reference>Comment</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Comment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderInlineContentComment](
		[InlineContentCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[CommentId] [uniqueidentifier] NULL,
		[OwningCommentId] [uniqueidentifier] NULL,
		[HasTrailingNewline] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderInlineContentComment] PRIMARY KEY CLUSTERED
(
	[InlineContentCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderMacro

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.Macro

	<References>
		<Reference>Namespace</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>PreprocessedEntity</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderMacro](
		[MacroId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[PreprocessedEntityId] [uniqueidentifier] NULL,
		[OwningNamespaceId] [uniqueidentifier] NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Expression] [nvarchar](255) NOT NULL,
		[LineNumberStart] [int] NOT NULL,
		[LineNumberEnd] [int] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderMacro] PRIMARY KEY CLUSTERED
(
	[MacroId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderMacroExpansion

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.MacroExpansion

	<References>
		<Reference>MacroDefinition</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>PreprocessedEntity</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderMacroExpansion](
		[MacroExpansionId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[PreprocessedEntityId] [uniqueidentifier] NULL,
		[DefinitionMacroDefinitionId] [uniqueidentifier] NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Text] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderMacroExpansion] PRIMARY KEY CLUSTERED
(
	[MacroExpansionId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderMemberPointerType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.MemberPointerType

	<References>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderMemberPointerType](
		[MemberPointerTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[PointeeQualifiedTypeId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderMemberPointerType] PRIMARY KEY CLUSTERED
(
	[MemberPointerTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderNamespace

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.Namespace

	<References>
		<Reference>DeclarationContext</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>DeclarationContext</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderNamespace](
		[NamespaceId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationContextId] [uniqueidentifier] NULL,
		[OwningDeclarationContextId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[IsInline] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderNamespace] PRIMARY KEY CLUSTERED
(
	[NamespaceId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderParamCommandComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.ParamCommandComment

	<References>
		<Reference>IntegerValue</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>BlockCommandComment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderParamCommandComment](
		[ParamCommandCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[BlockCommandCommentId] [uniqueidentifier] NULL,
		[ParamIndexIntegerValueId] [uniqueidentifier] NULL,
		[Direction] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderParamCommandComment] PRIMARY KEY CLUSTERED
(
	[ParamCommandCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderPointerType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.PointerType

	<References>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderPointerType](
		[PointerTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[QualifiedPointeeQualifiedTypeId] [uniqueidentifier] NULL,
		[Modifier] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderPointerType] PRIMARY KEY CLUSTERED
(
	[PointerTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderTagType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TagType

	<References>
		<Reference>Declaration</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTagType](
		[TagTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[DeclarationId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderTagType] PRIMARY KEY CLUSTERED
(
	[TagTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderTemplateParameterSubstitutionType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TemplateParameterSubstitutionType

	<References>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTemplateParameterSubstitutionType](
		[TemplateParameterSubstitutionTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[ReplacementQualifiedTypeId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderTemplateParameterSubstitutionType] PRIMARY KEY CLUSTERED
(
	[TemplateParameterSubstitutionTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderTypeDefNameDecl

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TypeDefNameDecl

	<References>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Declaration</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTypeDefNameDecl](
		[TypeDefNameDeclId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationId] [uniqueidentifier] NULL,
		[QualifiedTypeId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderTypeDefNameDecl] PRIMARY KEY CLUSTERED
(
	[TypeDefNameDeclId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderTypedefType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TypedefType

	<References>
		<Reference>Declaration</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTypedefType](
		[TypedefTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[DeclarationId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderTypedefType] PRIMARY KEY CLUSTERED
(
	[TypedefTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderTypeTemplateParameter

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TypeTemplateParameter

	<References>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>TemplateParameter</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTypeTemplateParameter](
		[TypeTemplateParameterId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TemplateParameterId] [uniqueidentifier] NULL,
		[DefaultArgumentQualifiedTypeId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderTypeTemplateParameter] PRIMARY KEY CLUSTERED
(
	[TypeTemplateParameterId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

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
/*
	Table: tblSDKHeaderVerbatimBlockLineComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.VerbatimBlockLineComment

	<References>
		<Reference>Comment</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Comment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderVerbatimBlockLineComment](
		[VerbatimBlockLineCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[CommentId] [uniqueidentifier] NULL,
		[OwningCommentId] [uniqueidentifier] NULL,
		[Text] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderVerbatimBlockLineComment] PRIMARY KEY CLUSTERED
(
	[VerbatimBlockLineCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderAttributedType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.AttributedType

	<References>
		<Reference>QualifiedType</Reference>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderAttributedType](
		[AttributedTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[EquivalentQualifiedTypeId] [uniqueidentifier] NULL,
		[ModifiedQualifiedTypeId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderAttributedType] PRIMARY KEY CLUSTERED
(
	[AttributedTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderBinaryOperator

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.BinaryOperator

	<References>
		<Reference>Expression</Reference>
		<Reference>Expression</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Expression</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderBinaryOperator](
		[BinaryOperatorId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[ExpressionId] [uniqueidentifier] NULL,
		[LHSExpressionId] [uniqueidentifier] NULL,
		[RHSExpressionId] [uniqueidentifier] NULL,
		[OpCodeString] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderBinaryOperator] PRIMARY KEY CLUSTERED
(
	[BinaryOperatorId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderClass

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.Class

	<References>
		<Reference>DeclarationContext</Reference>
		<Reference>ClassLayout</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>DeclarationContext</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderClass](
		[ClassId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationContextId] [uniqueidentifier] NULL,
		[OwningDeclarationContextId] [uniqueidentifier] NULL,
		[LayoutClassLayoutId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[ClassName] [nvarchar](255) NOT NULL,
		[HasNonTrivialCopyConstructor] [bit] NOT NULL,
		[HasNonTrivialDefaultConstructor] [bit] NOT NULL,
		[HasNonTrivialDestructor] [bit] NOT NULL,
		[IsAbstract] [bit] NOT NULL,
		[IsDynamic] [bit] NOT NULL,
		[IsExternCContext] [bit] NOT NULL,
		[IsPOD] [bit] NOT NULL,
		[IsPolymorphic] [bit] NOT NULL,
		[IsUnion] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderClass] PRIMARY KEY CLUSTERED
(
	[ClassId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

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
/*
	Table: tblSDKHeaderEnumeration

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.Enumeration

	<References>
		<Reference>DeclarationContext</Reference>
		<Reference>Type</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>DeclarationContext</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderEnumeration](
		[EnumerationId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationContextId] [uniqueidentifier] NULL,
		[OwningDeclarationContextId] [uniqueidentifier] NULL,
		[BuiltInTypeId] [uniqueidentifier] NULL,
		[EnumIndex] [int] NOT NULL,
		[LocationIdentifier] [bigint] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderEnumeration] PRIMARY KEY CLUSTERED
(
	[EnumerationId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

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
/*
	Table: tblSDKHeaderFriend

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.Friend

	<References>
		<Reference>DeclarationContext</Reference>
		<Reference>Declaration</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Declaration</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderFriend](
		[FriendId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationId] [uniqueidentifier] NULL,
		[OwningDeclarationContextId] [uniqueidentifier] NULL,
		[FriendDeclarationId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderFriend] PRIMARY KEY CLUSTERED
(
	[FriendId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderFullComment

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:40 AM
	Maps to class VisualStudioProvider.PDB.Headers.FullComment

	<References>
		<Reference>Comment</Reference>
		<Reference>RawComment</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Comment</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderFullComment](
		[FullCommentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[CommentId] [uniqueidentifier] NULL,
		[OwningCommentId] [uniqueidentifier] NULL,
		[OwningRawCommentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderFullComment] PRIMARY KEY CLUSTERED
(
	[FullCommentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderInjectedClassNameType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.InjectedClassNameType

	<References>
		<Reference>Class</Reference>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderInjectedClassNameType](
		[InjectedClassNameTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[ClassId] [uniqueidentifier] NULL,
		[InjectedSpecializationTypeQualifiedTypeId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderInjectedClassNameType] PRIMARY KEY CLUSTERED
(
	[InjectedClassNameTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

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
/*
	Table: tblSDKHeaderNonTypeTemplateParameter

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.NonTypeTemplateParameter

	<References>
		<Reference>Expression</Reference>
		<Reference>IntegerValue</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>TemplateParameter</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderNonTypeTemplateParameter](
		[NonTypeTemplateParameterId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TemplateParameterId] [uniqueidentifier] NULL,
		[DefaultArgumentExpressionId] [uniqueidentifier] NULL,
		[PositionIntegerValueId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[IsExpandedParameterPack] [bit] NOT NULL,
		[IsPackExpansion] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderNonTypeTemplateParameter] PRIMARY KEY CLUSTERED
(
	[NonTypeTemplateParameterId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderTemplate

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.Template

	<References>
		<Reference>DeclarationContext</Reference>
		<Reference>Declaration</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Declaration</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTemplate](
		[TemplateId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationId] [uniqueidentifier] NULL,
		[OwningDeclarationContextId] [uniqueidentifier] NULL,
		[TemplateDeclarationId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderTemplate] PRIMARY KEY CLUSTERED
(
	[TemplateId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderTemplateParameter

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TemplateParameter

	<References>
		<Reference>IntegerValue</Reference>
		<Reference>IntegerValue</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Declaration</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTemplateParameter](
		[TemplateParameterId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationId] [uniqueidentifier] NULL,
		[DepthIntegerValueId] [uniqueidentifier] NULL,
		[IndexIntegerValueId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[IsParameterPack] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderTemplateParameter] PRIMARY KEY CLUSTERED
(
	[TemplateParameterId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderTemplateSpecializationType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TemplateSpecializationType

	<References>
		<Reference>Template</Reference>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTemplateSpecializationType](
		[TemplateSpecializationTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[TemplateId] [uniqueidentifier] NULL,
		[DesugardQualifiedTypeId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderTemplateSpecializationType] PRIMARY KEY CLUSTERED
(
	[TemplateSpecializationTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderTypeDef

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TypeDef

	<References>
		<Reference>DeclarationContext</Reference>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>TypeDefNameDecl</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTypeDef](
		[TypeDefId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeDefNameDeclId] [uniqueidentifier] NULL,
		[OwningDeclarationContextId] [uniqueidentifier] NULL,
		[QualifiedTypeId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderTypeDef] PRIMARY KEY CLUSTERED
(
	[TypeDefId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderUnaryTransformType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.UnaryTransformType

	<References>
		<Reference>QualifiedType</Reference>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderUnaryTransformType](
		[UnaryTransformTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[DesugardedQualifiedTypeId] [uniqueidentifier] NULL,
		[BaseTypeQualifiedTypeId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblSDKHeaderUnaryTransformType] PRIMARY KEY CLUSTERED
(
	[UnaryTransformTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderVariable

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.Variable

	<References>
		<Reference>DeclarationContext</Reference>
		<Reference>QualifiedType</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Declaration</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderVariable](
		[VariableId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[DeclarationId] [uniqueidentifier] NULL,
		[OwningDeclarationContextId] [uniqueidentifier] NULL,
		[QualifiedTypeId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[Mangled] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderVariable] PRIMARY KEY CLUSTERED
(
	[VariableId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderVarTemplateSpecialization

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.VarTemplateSpecialization

	<References>
		<Reference>Declaration</Reference>
		<Reference>VarTemplate</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Variable</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderVarTemplateSpecialization](
		[VarTemplateSpecializationId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[VariableId] [uniqueidentifier] NULL,
		[OwningDeclarationId] [uniqueidentifier] NULL,
		[TemplateDeclVarTemplateId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
		[SpecializationKind] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderVarTemplateSpecialization] PRIMARY KEY CLUSTERED
(
	[VarTemplateSpecializationId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

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
/*
	Table: tblSDKHeaderVTableComponent

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.VTableComponent

	<References>
		<Reference>VTableLayout</Reference>
		<Reference>Declaration</Reference>
		<Reference>IntegerValue</Reference>
		<Reference>HeaderFile</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderVTableComponent](
		[VTableComponentId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[OwningVTableLayoutId] [uniqueidentifier] NULL,
		[DeclarationId] [uniqueidentifier] NULL,
		[OffsetIntegerValueId] [uniqueidentifier] NULL,
		[Kind] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderVTableComponent] PRIMARY KEY CLUSTERED
(
	[VTableComponentId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

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
/*
	Table: tblSDKHeaderTemplateParameterType

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TemplateParameterType

	<References>
		<Reference>IntegerValue</Reference>
		<Reference>IntegerValue</Reference>
		<Reference>TypeTemplateParameter</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>Type</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTemplateParameterType](
		[TemplateParameterTypeId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[DepthIntegerValueId] [uniqueidentifier] NULL,
		[IndexIntegerValueId] [uniqueidentifier] NULL,
		[ParameterTypeTemplateParameterId] [uniqueidentifier] NULL,
		[IsParameterPack] [bit] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderTemplateParameterType] PRIMARY KEY CLUSTERED
(
	[TemplateParameterTypeId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO
/*
	Table: tblSDKHeaderTypeAlias

	Generated automatically by DiaHeadersSqlTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09006a76b6b1ba0d, Author: u164225, Date: 10/9/2016 11:53:41 AM
	Maps to class VisualStudioProvider.PDB.Headers.TypeAlias

	<References>
		<Reference>DeclarationContext</Reference>
		<Reference>QualifiedType</Reference>
		<Reference>Template</Reference>
		<Reference>HeaderFile</Reference>
		<Reference>TypeDefNameDecl</Reference>
	</References>
*/


CREATE TABLE [dbo].[tblSDKHeaderTypeAlias](
		[TypeAliasId] [uniqueidentifier] NOT NULL,
		[HeaderFileId] [uniqueidentifier] NOT NULL,
		[TypeDefNameDeclId] [uniqueidentifier] NULL,
		[OwningDeclarationContextId] [uniqueidentifier] NULL,
		[QualifiedTypeId] [uniqueidentifier] NULL,
		[DescribedAliasTemplateId] [uniqueidentifier] NULL,
		[LocationIdentifier] [bigint] NOT NULL,
 CONSTRAINT [PK_tblSDKHeaderTypeAlias] PRIMARY KEY CLUSTERED
(
	[TypeAliasId] ASC
)

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

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
CREATE NONCLUSTERED INDEX [IX_tblSDKHeaderHeader_Name_Index] ON [dbo].[tblSDKHeaderHeader] ([Name] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_tblSDKHeaderHeader_FileName_Index] ON [dbo].[tblSDKHeaderHeader] ([FileName] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_tblSDKHeaderHTMLEndTagComment_TagName_Index] ON [dbo].[tblSDKHeaderHTMLEndTagComment] ([TagName] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_tblSDKHeaderHTMLStartTagComment_TagName_Index] ON [dbo].[tblSDKHeaderHTMLStartTagComment] ([TagName] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_tblSDKHeaderHTMLStartTagCommentAttribute_Name_Index] ON [dbo].[tblSDKHeaderHTMLStartTagCommentAttribute] ([Name] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_tblSDKHeaderMacroDefinition_Name_Index] ON [dbo].[tblSDKHeaderMacroDefinition] ([Name] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_tblSDKHeaderTranslationUnit_FileName_Index] ON [dbo].[tblSDKHeaderTranslationUnit] ([FileName] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_tblSDKHeaderDeclaration_Name_Index] ON [dbo].[tblSDKHeaderDeclaration] ([Name] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_tblSDKHeaderDeclarationContext_Name_Index] ON [dbo].[tblSDKHeaderDeclarationContext] ([Name] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_tblSDKHeaderMacro_Name_Index] ON [dbo].[tblSDKHeaderMacro] ([Name] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_tblSDKHeaderMacroExpansion_Name_Index] ON [dbo].[tblSDKHeaderMacroExpansion] ([Name] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_tblSDKHeaderNamespace_Name_Index] ON [dbo].[tblSDKHeaderNamespace] ([Name] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_tblSDKHeaderClass_ClassName_Index] ON [dbo].[tblSDKHeaderClass] ([ClassName] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_tblSDKHeaderFunction_FunctionName_Index] ON [dbo].[tblSDKHeaderFunction] ([FunctionName] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblSDKHeaderIntegerValue] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderIntegerValue_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVTableLayout] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVTableLayout_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBlockCommandCommentArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBlockCommandCommentArgument_tblSDKHeaderBlockCommandComment_OwningBlockCommandComment] FOREIGN KEY([OwningBlockCommandCommentId])
REFERENCES [dbo].[tblSDKHeaderBlockCommandComment] ([BlockCommandCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBlockCommandCommentArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBlockCommandCommentArgument_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBuiltinType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBuiltinType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBuiltinType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBuiltinType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBuiltinTypeExpression] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBuiltinTypeExpression_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBuiltinTypeExpression] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBuiltinTypeExpression_tblSDKHeaderExpression_Expression] FOREIGN KEY([ExpressionId])
REFERENCES [dbo].[tblSDKHeaderExpression] ([ExpressionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderCallExpr] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderCallExpr_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderCallExpr] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderCallExpr_tblSDKHeaderExpression_Expression] FOREIGN KEY([ExpressionId])
REFERENCES [dbo].[tblSDKHeaderExpression] ([ExpressionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassLayout] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassLayout_tblSDKHeaderVTableLayout_LayoutVTableLayout] FOREIGN KEY([LayoutVTableLayoutId])
REFERENCES [dbo].[tblSDKHeaderVTableLayout] ([VTableLayoutId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassLayout] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassLayout_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassTemplate_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassTemplate_tblSDKHeaderTemplate_Template] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[tblSDKHeaderTemplate] ([TemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassTemplatePartialSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassTemplatePartialSpecialization_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassTemplatePartialSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassTemplatePartialSpecialization_tblSDKHeaderClassTemplateSpecialization_ClassTemplateSpecialization] FOREIGN KEY([ClassTemplateSpecializationId])
REFERENCES [dbo].[tblSDKHeaderClassTemplateSpecialization] ([ClassTemplateSpecializationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderComment_tblSDKHeaderDeclaration_ParentDeclaration] FOREIGN KEY([ParentDeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderCXXConstructExpr] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderCXXConstructExpr_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderCXXConstructExpr] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderCXXConstructExpr_tblSDKHeaderExpression_Expression] FOREIGN KEY([ExpressionId])
REFERENCES [dbo].[tblSDKHeaderExpression] ([ExpressionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderExpression] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderExpression_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderExpression] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderExpression_tblSDKHeaderStatement_Statement] FOREIGN KEY([StatementId])
REFERENCES [dbo].[tblSDKHeaderStatement] ([StatementId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunctionTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunctionTemplate_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunctionTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunctionTemplate_tblSDKHeaderTemplate_Template] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[tblSDKHeaderTemplate] ([TemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHeader] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHeader_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHeader] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHeader_tblSDKHeaderNamespace_Namespace] FOREIGN KEY([NamespaceId])
REFERENCES [dbo].[tblSDKHeaderNamespace] ([NamespaceId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHTMLEndTagComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHTMLEndTagComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHTMLEndTagComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHTMLEndTagComment_tblSDKHeaderHTMLTagComment_HTMLTagComment] FOREIGN KEY([HTMLTagCommentId])
REFERENCES [dbo].[tblSDKHeaderHTMLTagComment] ([HTMLTagCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHTMLStartTagComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHTMLStartTagComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHTMLStartTagComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHTMLStartTagComment_tblSDKHeaderHTMLTagComment_HTMLTagComment] FOREIGN KEY([HTMLTagCommentId])
REFERENCES [dbo].[tblSDKHeaderHTMLTagComment] ([HTMLTagCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHTMLStartTagCommentAttribute] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHTMLStartTagCommentAttribute_tblSDKHeaderHTMLStartTagComment_OwningHTMLStartTagComment] FOREIGN KEY([OwningHTMLStartTagCommentId])
REFERENCES [dbo].[tblSDKHeaderHTMLStartTagComment] ([HTMLStartTagCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHTMLStartTagCommentAttribute] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHTMLStartTagCommentAttribute_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHTMLTagComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHTMLTagComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderHTMLTagComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderHTMLTagComment_tblSDKHeaderInlineContentComment_InlineContentComment] FOREIGN KEY([InlineContentCommentId])
REFERENCES [dbo].[tblSDKHeaderInlineContentComment] ([InlineContentCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInlineCommandCommentArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInlineCommandCommentArgument_tblSDKHeaderInlineCommandComment_OwningInlineCommandComment] FOREIGN KEY([OwningInlineCommandCommentId])
REFERENCES [dbo].[tblSDKHeaderInlineCommandComment] ([InlineCommandCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInlineCommandCommentArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInlineCommandCommentArgument_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMacroDefinition] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMacroDefinition_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMacroDefinition] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMacroDefinition_tblSDKHeaderPreprocessedEntity_PreprocessedEntity] FOREIGN KEY([PreprocessedEntityId])
REFERENCES [dbo].[tblSDKHeaderPreprocessedEntity] ([PreprocessedEntityId])
GO

ALTER TABLE [dbo].[tblSDKHeaderPackExpansionType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderPackExpansionType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderPackExpansionType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderPackExpansionType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParagraphComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParagraphComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParagraphComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParagraphComment_tblSDKHeaderBlockContentComment_BlockContentComment] FOREIGN KEY([BlockContentCommentId])
REFERENCES [dbo].[tblSDKHeaderBlockContentComment] ([BlockContentCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderPreprocessedEntity] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderPreprocessedEntity_tblSDKHeaderDeclaration_OwningDeclaration] FOREIGN KEY([OwningDeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderPreprocessedEntity] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderPreprocessedEntity_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderQualifiedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderQualifiedType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderQualifiedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderQualifiedType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderRawComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderRawComment_tblSDKHeaderFullComment_CommentBlockFullComment] FOREIGN KEY([CommentBlockFullCommentId])
REFERENCES [dbo].[tblSDKHeaderFullComment] ([FullCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderRawComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderRawComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderStatement] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderStatement_tblSDKHeaderDeclaration_DeclDeclaration] FOREIGN KEY([DeclDeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderStatement] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderStatement_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateTemplateParameter_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateTemplateParameter_tblSDKHeaderTemplate_Template] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[tblSDKHeaderTemplate] ([TemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTextComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTextComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTextComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTextComment_tblSDKHeaderInlineContentComment_InlineContentComment] FOREIGN KEY([InlineContentCommentId])
REFERENCES [dbo].[tblSDKHeaderInlineContentComment] ([InlineContentCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTParamCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTParamCommandComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTParamCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTParamCommandComment_tblSDKHeaderBlockCommandComment_BlockCommandComment] FOREIGN KEY([BlockCommandCommentId])
REFERENCES [dbo].[tblSDKHeaderBlockCommandComment] ([BlockCommandCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTranslationUnit] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTranslationUnit_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTranslationUnit] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTranslationUnit_tblSDKHeaderNamespace_Namespace] FOREIGN KEY([NamespaceId])
REFERENCES [dbo].[tblSDKHeaderNamespace] ([NamespaceId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeAliasTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeAliasTemplate_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeAliasTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeAliasTemplate_tblSDKHeaderTemplate_Template] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[tblSDKHeaderTemplate] ([TemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDefDecl] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDefDecl_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDefDecl] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDefDecl_tblSDKHeaderTypeDefNameDecl_TypeDefNameDecl] FOREIGN KEY([TypeDefNameDeclId])
REFERENCES [dbo].[tblSDKHeaderTypeDefNameDecl] ([TypeDefNameDeclId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplate_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplate_tblSDKHeaderTemplate_Template] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[tblSDKHeaderTemplate] ([TemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVerbatimBlockComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVerbatimBlockComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVerbatimBlockComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVerbatimBlockComment_tblSDKHeaderBlockCommandComment_BlockCommandComment] FOREIGN KEY([BlockCommandCommentId])
REFERENCES [dbo].[tblSDKHeaderBlockCommandComment] ([BlockCommandCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVerbatimLineComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVerbatimLineComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVerbatimLineComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVerbatimLineComment_tblSDKHeaderBlockCommandComment_BlockCommandComment] FOREIGN KEY([BlockCommandCommentId])
REFERENCES [dbo].[tblSDKHeaderBlockCommandComment] ([BlockCommandCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderAccessSpecifierDecl] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderAccessSpecifierDecl_tblSDKHeaderClass_OwningClass] FOREIGN KEY([OwningClassId])
REFERENCES [dbo].[tblSDKHeaderClass] ([ClassId])
GO

ALTER TABLE [dbo].[tblSDKHeaderAccessSpecifierDecl] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderAccessSpecifierDecl_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderAccessSpecifierDecl] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderAccessSpecifierDecl_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderArrayType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderArrayType_tblSDKHeaderQualifiedType_QualifiedType] FOREIGN KEY([QualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderArrayType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderArrayType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderArrayType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderArrayType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBaseClassSpecifier] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBaseClassSpecifier_tblSDKHeaderClass_OwningClass] FOREIGN KEY([OwningClassId])
REFERENCES [dbo].[tblSDKHeaderClass] ([ClassId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBaseClassSpecifier] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBaseClassSpecifier_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBaseClassSpecifier] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBaseClassSpecifier_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBlockCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBlockCommandComment_tblSDKHeaderParagraphComment_ParagraphComment] FOREIGN KEY([ParagraphCommentId])
REFERENCES [dbo].[tblSDKHeaderParagraphComment] ([ParagraphCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBlockCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBlockCommandComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBlockCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBlockCommandComment_tblSDKHeaderBlockContentComment_BlockContentComment] FOREIGN KEY([BlockContentCommentId])
REFERENCES [dbo].[tblSDKHeaderBlockContentComment] ([BlockContentCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBlockContentComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBlockContentComment_tblSDKHeaderComment_OwningComment] FOREIGN KEY([OwningCommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBlockContentComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBlockContentComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBlockContentComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBlockContentComment_tblSDKHeaderComment_Comment] FOREIGN KEY([CommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDeclaration] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDeclaration_tblSDKHeaderDeclaration_OwningDeclaration] FOREIGN KEY([OwningDeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDeclaration] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDeclaration_tblSDKHeaderDeclaration_CompleteDeclaration] FOREIGN KEY([CompleteDeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDeclaration] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDeclaration_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDeclarationContext] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDeclarationContext_tblSDKHeaderComment_Comment] FOREIGN KEY([CommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDeclarationContext] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDeclarationContext_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDeclarationContext] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDeclarationContext_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDependentNameType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDependentNameType_tblSDKHeaderQualifiedType_DesugaredQualifiedType] FOREIGN KEY([DesugaredQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDependentNameType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDependentNameType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDependentNameType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDependentNameType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDependentTemplateSpecializationType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDependentTemplateSpecializationType_tblSDKHeaderQualifiedType_DesugaredQualifiedType] FOREIGN KEY([DesugaredQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDependentTemplateSpecializationType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDependentTemplateSpecializationType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDependentTemplateSpecializationType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDependentTemplateSpecializationType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunctionTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunctionTemplateSpecialization_tblSDKHeaderFunctionTemplate_OwningFunctionTemplate] FOREIGN KEY([OwningFunctionTemplateId])
REFERENCES [dbo].[tblSDKHeaderFunctionTemplate] ([FunctionTemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunctionTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunctionTemplateSpecialization_tblSDKHeaderFunction_SpecializedFunction] FOREIGN KEY([SpecializedFunctionId])
REFERENCES [dbo].[tblSDKHeaderFunction] ([FunctionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunctionTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunctionTemplateSpecialization_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunctionType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunctionType_tblSDKHeaderQualifiedType_ReturnTypeQualifiedType] FOREIGN KEY([ReturnTypeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunctionType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunctionType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunctionType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunctionType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInlineCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInlineCommandComment_tblSDKHeaderIntegerValue_CommandIntegerValue] FOREIGN KEY([CommandIdIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInlineCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInlineCommandComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInlineCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInlineCommandComment_tblSDKHeaderInlineContentComment_InlineContentComment] FOREIGN KEY([InlineContentCommentId])
REFERENCES [dbo].[tblSDKHeaderInlineContentComment] ([InlineContentCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInlineContentComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInlineContentComment_tblSDKHeaderComment_OwningComment] FOREIGN KEY([OwningCommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInlineContentComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInlineContentComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInlineContentComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInlineContentComment_tblSDKHeaderComment_Comment] FOREIGN KEY([CommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMacro] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMacro_tblSDKHeaderNamespace_OwningNamespace] FOREIGN KEY([OwningNamespaceId])
REFERENCES [dbo].[tblSDKHeaderNamespace] ([NamespaceId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMacro] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMacro_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMacro] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMacro_tblSDKHeaderPreprocessedEntity_PreprocessedEntity] FOREIGN KEY([PreprocessedEntityId])
REFERENCES [dbo].[tblSDKHeaderPreprocessedEntity] ([PreprocessedEntityId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMacroExpansion] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMacroExpansion_tblSDKHeaderMacroDefinition_DefinitionMacroDefinition] FOREIGN KEY([DefinitionMacroDefinitionId])
REFERENCES [dbo].[tblSDKHeaderMacroDefinition] ([MacroDefinitionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMacroExpansion] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMacroExpansion_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMacroExpansion] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMacroExpansion_tblSDKHeaderPreprocessedEntity_PreprocessedEntity] FOREIGN KEY([PreprocessedEntityId])
REFERENCES [dbo].[tblSDKHeaderPreprocessedEntity] ([PreprocessedEntityId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMemberPointerType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMemberPointerType_tblSDKHeaderQualifiedType_PointeeQualifiedType] FOREIGN KEY([PointeeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMemberPointerType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMemberPointerType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMemberPointerType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMemberPointerType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderNamespace] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderNamespace_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderNamespace] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderNamespace_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderNamespace] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderNamespace_tblSDKHeaderDeclarationContext_DeclarationContext] FOREIGN KEY([DeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParamCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParamCommandComment_tblSDKHeaderIntegerValue_ParamIndexIntegerValue] FOREIGN KEY([ParamIndexIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParamCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParamCommandComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParamCommandComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParamCommandComment_tblSDKHeaderBlockCommandComment_BlockCommandComment] FOREIGN KEY([BlockCommandCommentId])
REFERENCES [dbo].[tblSDKHeaderBlockCommandComment] ([BlockCommandCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderPointerType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderPointerType_tblSDKHeaderQualifiedType_QualifiedPointeeQualifiedType] FOREIGN KEY([QualifiedPointeeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderPointerType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderPointerType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderPointerType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderPointerType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTagType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTagType_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTagType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTagType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTagType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTagType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameterSubstitutionType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameterSubstitutionType_tblSDKHeaderQualifiedType_ReplacementQualifiedType] FOREIGN KEY([ReplacementQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameterSubstitutionType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameterSubstitutionType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameterSubstitutionType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameterSubstitutionType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDefNameDecl] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDefNameDecl_tblSDKHeaderQualifiedType_QualifiedType] FOREIGN KEY([QualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDefNameDecl] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDefNameDecl_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDefNameDecl] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDefNameDecl_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypedefType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypedefType_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypedefType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypedefType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypedefType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypedefType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeTemplateParameter_tblSDKHeaderQualifiedType_DefaultArgumentQualifiedType] FOREIGN KEY([DefaultArgumentQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeTemplateParameter_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeTemplateParameter_tblSDKHeaderTemplateParameter_TemplateParameter] FOREIGN KEY([TemplateParameterId])
REFERENCES [dbo].[tblSDKHeaderTemplateParameter] ([TemplateParameterId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplatePartialSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplatePartialSpecialization_tblSDKHeaderVarTemplate_OwningVarTemplate] FOREIGN KEY([OwningVarTemplateId])
REFERENCES [dbo].[tblSDKHeaderVarTemplate] ([VarTemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplatePartialSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplatePartialSpecialization_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplatePartialSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplatePartialSpecialization_tblSDKHeaderVarTemplateSpecialization_VarTemplateSpecialization] FOREIGN KEY([VarTemplateSpecializationId])
REFERENCES [dbo].[tblSDKHeaderVarTemplateSpecialization] ([VarTemplateSpecializationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVerbatimBlockLineComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVerbatimBlockLineComment_tblSDKHeaderComment_OwningComment] FOREIGN KEY([OwningCommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVerbatimBlockLineComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVerbatimBlockLineComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVerbatimBlockLineComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVerbatimBlockLineComment_tblSDKHeaderComment_Comment] FOREIGN KEY([CommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderAttributedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderAttributedType_tblSDKHeaderQualifiedType_EquivalentQualifiedType] FOREIGN KEY([EquivalentQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderAttributedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderAttributedType_tblSDKHeaderQualifiedType_ModifiedQualifiedType] FOREIGN KEY([ModifiedQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderAttributedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderAttributedType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderAttributedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderAttributedType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBinaryOperator] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBinaryOperator_tblSDKHeaderExpression_LHSExpression] FOREIGN KEY([LHSExpressionId])
REFERENCES [dbo].[tblSDKHeaderExpression] ([ExpressionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBinaryOperator] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBinaryOperator_tblSDKHeaderExpression_RHSExpression] FOREIGN KEY([RHSExpressionId])
REFERENCES [dbo].[tblSDKHeaderExpression] ([ExpressionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBinaryOperator] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBinaryOperator_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderBinaryOperator] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderBinaryOperator_tblSDKHeaderExpression_Expression] FOREIGN KEY([ExpressionId])
REFERENCES [dbo].[tblSDKHeaderExpression] ([ExpressionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClass] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClass_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClass] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClass_tblSDKHeaderClassLayout_LayoutClassLayout] FOREIGN KEY([LayoutClassLayoutId])
REFERENCES [dbo].[tblSDKHeaderClassLayout] ([ClassLayoutId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClass] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClass_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClass] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClass_tblSDKHeaderDeclarationContext_DeclarationContext] FOREIGN KEY([DeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassTemplateSpecialization_tblSDKHeaderClassTemplate_OwningClassTemplate] FOREIGN KEY([OwningClassTemplateId])
REFERENCES [dbo].[tblSDKHeaderClassTemplate] ([ClassTemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassTemplateSpecialization_tblSDKHeaderClassTemplate_TemplatedDeclClassTemplate] FOREIGN KEY([TemplatedDeclClassTemplateId])
REFERENCES [dbo].[tblSDKHeaderClassTemplate] ([ClassTemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassTemplateSpecialization_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderClassTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderClassTemplateSpecialization_tblSDKHeaderClass_Class] FOREIGN KEY([ClassId])
REFERENCES [dbo].[tblSDKHeaderClass] ([ClassId])
GO

ALTER TABLE [dbo].[tblSDKHeaderEnumeration] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderEnumeration_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderEnumeration] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderEnumeration_tblSDKHeaderType_BuiltInType] FOREIGN KEY([BuiltInTypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderEnumeration] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderEnumeration_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderEnumeration] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderEnumeration_tblSDKHeaderDeclarationContext_DeclarationContext] FOREIGN KEY([DeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderEnumerationItem] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderEnumerationItem_tblSDKHeaderEnumeration_OwningEnumeration] FOREIGN KEY([OwningEnumerationId])
REFERENCES [dbo].[tblSDKHeaderEnumeration] ([EnumerationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderEnumerationItem] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderEnumerationItem_tblSDKHeaderIntegerValue_ValueIntegerValue] FOREIGN KEY([ValueIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderEnumerationItem] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderEnumerationItem_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderEnumerationItem] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderEnumerationItem_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFriend] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFriend_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFriend] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFriend_tblSDKHeaderDeclaration_FriendDeclaration] FOREIGN KEY([FriendDeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFriend] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFriend_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFriend] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFriend_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFullComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFullComment_tblSDKHeaderComment_OwningComment] FOREIGN KEY([OwningCommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFullComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFullComment_tblSDKHeaderRawComment_OwningRawComment] FOREIGN KEY([OwningRawCommentId])
REFERENCES [dbo].[tblSDKHeaderRawComment] ([RawCommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFullComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFullComment_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFullComment] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFullComment_tblSDKHeaderComment_Comment] FOREIGN KEY([CommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInjectedClassNameType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInjectedClassNameType_tblSDKHeaderClass_Class] FOREIGN KEY([ClassId])
REFERENCES [dbo].[tblSDKHeaderClass] ([ClassId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInjectedClassNameType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInjectedClassNameType_tblSDKHeaderQualifiedType_InjectedSpecializationTypeQualifiedType] FOREIGN KEY([InjectedSpecializationTypeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInjectedClassNameType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInjectedClassNameType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderInjectedClassNameType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderInjectedClassNameType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMethod] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMethod_tblSDKHeaderClass_OwningClass] FOREIGN KEY([OwningClassId])
REFERENCES [dbo].[tblSDKHeaderClass] ([ClassId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMethod] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMethod_tblSDKHeaderQualifiedType_ConversionTypeQualifiedType] FOREIGN KEY([ConversionTypeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMethod] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMethod_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderMethod] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderMethod_tblSDKHeaderFunction_Function] FOREIGN KEY([FunctionId])
REFERENCES [dbo].[tblSDKHeaderFunction] ([FunctionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderNonTypeTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderNonTypeTemplateParameter_tblSDKHeaderExpression_DefaultArgumentExpression] FOREIGN KEY([DefaultArgumentExpressionId])
REFERENCES [dbo].[tblSDKHeaderExpression] ([ExpressionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderNonTypeTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderNonTypeTemplateParameter_tblSDKHeaderIntegerValue_PositionIntegerValue] FOREIGN KEY([PositionIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderNonTypeTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderNonTypeTemplateParameter_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderNonTypeTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderNonTypeTemplateParameter_tblSDKHeaderTemplateParameter_TemplateParameter] FOREIGN KEY([TemplateParameterId])
REFERENCES [dbo].[tblSDKHeaderTemplateParameter] ([TemplateParameterId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplate_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplate_tblSDKHeaderDeclaration_TemplateDeclaration] FOREIGN KEY([TemplateDeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplate_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplate] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplate_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameter_tblSDKHeaderIntegerValue_DepthIntegerValue] FOREIGN KEY([DepthIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameter_tblSDKHeaderIntegerValue_IndexIntegerValue] FOREIGN KEY([IndexIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameter_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameter_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateSpecializationType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateSpecializationType_tblSDKHeaderTemplate_Template] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[tblSDKHeaderTemplate] ([TemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateSpecializationType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateSpecializationType_tblSDKHeaderQualifiedType_DesugardQualifiedType] FOREIGN KEY([DesugardQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateSpecializationType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateSpecializationType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateSpecializationType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateSpecializationType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDef] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDef_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDef] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDef_tblSDKHeaderQualifiedType_QualifiedType] FOREIGN KEY([QualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDef] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDef_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeDef] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeDef_tblSDKHeaderTypeDefNameDecl_TypeDefNameDecl] FOREIGN KEY([TypeDefNameDeclId])
REFERENCES [dbo].[tblSDKHeaderTypeDefNameDecl] ([TypeDefNameDeclId])
GO

ALTER TABLE [dbo].[tblSDKHeaderUnaryTransformType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderUnaryTransformType_tblSDKHeaderQualifiedType_DesugardedQualifiedType] FOREIGN KEY([DesugardedQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderUnaryTransformType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderUnaryTransformType_tblSDKHeaderQualifiedType_BaseTypeQualifiedType] FOREIGN KEY([BaseTypeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderUnaryTransformType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderUnaryTransformType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderUnaryTransformType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderUnaryTransformType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVariable] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVariable_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVariable] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVariable_tblSDKHeaderQualifiedType_QualifiedType] FOREIGN KEY([QualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVariable] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVariable_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVariable] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVariable_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplateSpecialization_tblSDKHeaderDeclaration_OwningDeclaration] FOREIGN KEY([OwningDeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplateSpecialization_tblSDKHeaderVarTemplate_TemplateDeclVarTemplate] FOREIGN KEY([TemplateDeclVarTemplateId])
REFERENCES [dbo].[tblSDKHeaderVarTemplate] ([VarTemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplateSpecialization_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVarTemplateSpecialization] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVarTemplateSpecialization_tblSDKHeaderVariable_Variable] FOREIGN KEY([VariableId])
REFERENCES [dbo].[tblSDKHeaderVariable] ([VariableId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVectorType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVectorType_tblSDKHeaderQualifiedType_ElementTypeQualifiedType] FOREIGN KEY([ElementTypeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVectorType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVectorType_tblSDKHeaderIntegerValue_NumElementsIntegerValue] FOREIGN KEY([NumElementsIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVectorType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVectorType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVectorType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVectorType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVTableComponent] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVTableComponent_tblSDKHeaderVTableLayout_OwningVTableLayout] FOREIGN KEY([OwningVTableLayoutId])
REFERENCES [dbo].[tblSDKHeaderVTableLayout] ([VTableLayoutId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVTableComponent] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVTableComponent_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVTableComponent] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVTableComponent_tblSDKHeaderIntegerValue_OffsetIntegerValue] FOREIGN KEY([OffsetIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderVTableComponent] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderVTableComponent_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDecayedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDecayedType_tblSDKHeaderQualifiedType_DecayedQualifiedType] FOREIGN KEY([DecayedQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDecayedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDecayedType_tblSDKHeaderQualifiedType_OriginalQualifiedType] FOREIGN KEY([OriginalQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDecayedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDecayedType_tblSDKHeaderQualifiedType_PointeeQualifiedType] FOREIGN KEY([PointeeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDecayedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDecayedType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderDecayedType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderDecayedType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderField] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderField_tblSDKHeaderClass_OwningClass] FOREIGN KEY([OwningClassId])
REFERENCES [dbo].[tblSDKHeaderClass] ([ClassId])
GO

ALTER TABLE [dbo].[tblSDKHeaderField] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderField_tblSDKHeaderIntegerValue_BitWidthIntegerValue] FOREIGN KEY([BitWidthIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderField] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderField_tblSDKHeaderQualifiedType_QualifiedType] FOREIGN KEY([QualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderField] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderField_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderField] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderField_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameterType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderIntegerValue_DepthIntegerValue] FOREIGN KEY([DepthIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameterType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderIntegerValue_IndexIntegerValue] FOREIGN KEY([IndexIntegerValueId])
REFERENCES [dbo].[tblSDKHeaderIntegerValue] ([IntegerValueId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameterType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderTypeTemplateParameter_ParameterTypeTemplateParameter] FOREIGN KEY([ParameterTypeTemplateParameterId])
REFERENCES [dbo].[tblSDKHeaderTypeTemplateParameter] ([TypeTemplateParameterId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameterType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateParameterType] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateParameterType_tblSDKHeaderType_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblSDKHeaderType] ([TypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeAlias] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeAlias_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeAlias] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeAlias_tblSDKHeaderQualifiedType_QualifiedType] FOREIGN KEY([QualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeAlias] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeAlias_tblSDKHeaderTemplate_DescribedAliasTemplate] FOREIGN KEY([DescribedAliasTemplateId])
REFERENCES [dbo].[tblSDKHeaderTemplate] ([TemplateId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeAlias] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeAlias_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTypeAlias] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTypeAlias_tblSDKHeaderTypeDefNameDecl_TypeDefNameDecl] FOREIGN KEY([TypeDefNameDeclId])
REFERENCES [dbo].[tblSDKHeaderTypeDefNameDecl] ([TypeDefNameDeclId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunction] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderDeclarationContext_OwningDeclarationContext] FOREIGN KEY([OwningDeclarationContextId])
REFERENCES [dbo].[tblSDKHeaderDeclarationContext] ([DeclarationContextId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunction] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderComment_Comment] FOREIGN KEY([CommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunction] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderFunction_InstantiatedFromFunction] FOREIGN KEY([InstantiatedFromFunctionId])
REFERENCES [dbo].[tblSDKHeaderFunction] ([FunctionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunction] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderQualifiedType_ReturnTypeQualifiedType] FOREIGN KEY([ReturnTypeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunction] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderFunction] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderFunction_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderFunctionType_OwningFunctionType] FOREIGN KEY([OwningFunctionTypeId])
REFERENCES [dbo].[tblSDKHeaderFunctionType] ([FunctionTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderFunction_OwningFunction] FOREIGN KEY([OwningFunctionId])
REFERENCES [dbo].[tblSDKHeaderFunction] ([FunctionId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderComment_Comment] FOREIGN KEY([CommentId])
REFERENCES [dbo].[tblSDKHeaderComment] ([CommentId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderQualifiedType_QualifiedType] FOREIGN KEY([QualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

ALTER TABLE [dbo].[tblSDKHeaderParameter] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderParameter_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderClassTemplateSpecialization_OwningClassTemplateSpecialization] FOREIGN KEY([OwningClassTemplateSpecializationId])
REFERENCES [dbo].[tblSDKHeaderClassTemplateSpecialization] ([ClassTemplateSpecializationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderDependentTemplateSpecializationType_OwningDependentTemplateSpecializationType] FOREIGN KEY([OwningDependentTemplateSpecializationTypeId])
REFERENCES [dbo].[tblSDKHeaderDependentTemplateSpecializationType] ([DependentTemplateSpecializationTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderVarTemplateSpecialization_OwningVarTemplateSpecialization] FOREIGN KEY([OwningVarTemplateSpecializationId])
REFERENCES [dbo].[tblSDKHeaderVarTemplateSpecialization] ([VarTemplateSpecializationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderTemplateSpecializationType_OwningTemplateSpecializationType] FOREIGN KEY([OwningTemplateSpecializationTypeId])
REFERENCES [dbo].[tblSDKHeaderTemplateSpecializationType] ([TemplateSpecializationTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderFunctionTemplateSpecialization_OwningFunctionTemplateSpecialization] FOREIGN KEY([OwningFunctionTemplateSpecializationId])
REFERENCES [dbo].[tblSDKHeaderFunctionTemplateSpecialization] ([FunctionTemplateSpecializationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderDeclaration_Declaration] FOREIGN KEY([DeclarationId])
REFERENCES [dbo].[tblSDKHeaderDeclaration] ([DeclarationId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderQualifiedType_TypeQualifiedType] FOREIGN KEY([TypeQualifiedTypeId])
REFERENCES [dbo].[tblSDKHeaderQualifiedType] ([QualifiedTypeId])
GO

ALTER TABLE [dbo].[tblSDKHeaderTemplateArgument] WITH CHECK ADD CONSTRAINT [FK_tblSDKHeaderTemplateArgument_tblSDKHeaderFile_HeaderFile] FOREIGN KEY([HeaderFileId])
REFERENCES [dbo].[tblSDKHeaderFile] ([HeaderFileId])
GO

