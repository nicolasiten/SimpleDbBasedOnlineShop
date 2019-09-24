USE [master]
GO
/****** Object:  Database [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a]    Script Date: 23.09.2019 12:03:17 ******/
CREATE DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a]
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET ARITHABORT OFF 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET  ENABLE_BROKER 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET  MULTI_USER 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET DB_CHAINING OFF 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET QUERY_STORE = OFF
GO
USE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 23.09.2019 12:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 23.09.2019 12:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Address] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[Country] [nvarchar](max) NULL,
	[Date] [datetime2](7) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Zip] [nvarchar](max) NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderLine]    Script Date: 23.09.2019 12:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderLine](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_OrderLine] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 23.09.2019 12:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Details] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Price] [float] NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190127080942_InitialMigration', N'2.2.6-servicing-10079')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190128010341_MakeOrderLineForeignKeysCascaseDelete', N'2.2.6-servicing-10079')
GO
SET IDENTITY_INSERT [dbo].[Order] ON 
GO
INSERT [dbo].[Order] ([Id], [Address], [City], [Country], [Date], [Name], [Zip]) VALUES (1, N'Address', N'City', N'Country', CAST(N'2019-09-17T21:06:00.0000000' AS DateTime2), N'John Doe', N'Zip')
GO
SET IDENTITY_INSERT [dbo].[Order] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderLine] ON 
GO
INSERT [dbo].[OrderLine] ([Id], [OrderId], [ProductId], [Quantity]) VALUES (1, 1, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[OrderLine] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON 
GO
INSERT [dbo].[Product] ([Id], [Description], [Details], [Name], [Price], [Quantity]) VALUES (1, N'(2nd Generation) - Black', NULL, N'Echo Dot', 92.5, 10)
GO
INSERT [dbo].[Product] ([Id], [Description], [Details], [Name], [Price], [Quantity]) VALUES (2, N'Tangle-Free Micro USB Cable', NULL, N'Anker 3ft / 0.9m Nylon Braided', 9.99, 20)
GO
INSERT [dbo].[Product] ([Id], [Description], [Details], [Name], [Price], [Quantity]) VALUES (3, N'Riptidz, In-Ear', NULL, N'JVC HAFX8R Headphone', 69.99, 30)
GO
INSERT [dbo].[Product] ([Id], [Description], [Details], [Name], [Price], [Quantity]) VALUES (4, N'Cordless Phone', NULL, N'VTech CS6114 DECT 6.0', 32.5, 40)
GO
INSERT [dbo].[Product] ([Id], [Description], [Details], [Name], [Price], [Quantity]) VALUES (5, N'Cell Phone', NULL, N'NOKIA OEM BL-5J', 895, 50)
GO
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
/****** Object:  Index [IX_OrderLine_ProductId]    Script Date: 23.09.2019 12:03:18 ******/
CREATE NONCLUSTERED INDEX [IX_OrderLine_ProductId] ON [dbo].[OrderLine]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderLineEntity_OrderEntityId]    Script Date: 23.09.2019 12:03:18 ******/
CREATE NONCLUSTERED INDEX [IX_OrderLineEntity_OrderEntityId] ON [dbo].[OrderLine]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OrderLine]  WITH CHECK ADD  CONSTRAINT [FK__OrderLine__Produ__52593CB8] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderLine] CHECK CONSTRAINT [FK__OrderLine__Produ__52593CB8]
GO
ALTER TABLE [dbo].[OrderLine]  WITH CHECK ADD  CONSTRAINT [FK_OrderLineEntity_OrderEntity_OrderEntityId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderLine] CHECK CONSTRAINT [FK_OrderLineEntity_OrderEntity_OrderEntityId]
GO
USE [master]
GO
ALTER DATABASE [P3Referential-Test-2f561d3b-493f-46fd-83c9-6e2643e7bd0a] SET  READ_WRITE 
GO
