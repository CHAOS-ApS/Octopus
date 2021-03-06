USE [master]
GO
/****** Object:  Database [Octopus]    Script Date: 09/09/2010 13:52:02 ******/
CREATE DATABASE [Octopus] ON  PRIMARY 
( NAME = N'Octopus', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\Octopus.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Octopus_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\Octopus_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Octopus] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Octopus].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Octopus] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [Octopus] SET ANSI_NULLS OFF
GO
ALTER DATABASE [Octopus] SET ANSI_PADDING OFF
GO
ALTER DATABASE [Octopus] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [Octopus] SET ARITHABORT OFF
GO
ALTER DATABASE [Octopus] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [Octopus] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [Octopus] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [Octopus] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [Octopus] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [Octopus] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [Octopus] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [Octopus] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [Octopus] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [Octopus] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [Octopus] SET  DISABLE_BROKER
GO
ALTER DATABASE [Octopus] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [Octopus] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [Octopus] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [Octopus] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [Octopus] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [Octopus] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [Octopus] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [Octopus] SET  READ_WRITE
GO
ALTER DATABASE [Octopus] SET RECOVERY FULL
GO
ALTER DATABASE [Octopus] SET  MULTI_USER
GO
ALTER DATABASE [Octopus] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [Octopus] SET DB_CHAINING OFF
GO
USE [Octopus]
GO
/****** Object:  ForeignKey [FK_Assembly_Destination]    Script Date: 09/09/2010 13:52:07 ******/
ALTER TABLE [dbo].[Assembly] DROP CONSTRAINT [FK_Assembly_Destination]
GO
/****** Object:  ForeignKey [FK_ExecutionSlot_Agent]    Script Date: 09/09/2010 13:52:09 ******/
ALTER TABLE [dbo].[ExecutionSlot] DROP CONSTRAINT [FK_ExecutionSlot_Agent]
GO
/****** Object:  ForeignKey [FK_Job_Status]    Script Date: 09/09/2010 13:52:10 ******/
ALTER TABLE [dbo].[Job] DROP CONSTRAINT [FK_Job_Status]
GO
/****** Object:  ForeignKey [FK_WatchFolder_Destination]    Script Date: 09/09/2010 13:52:10 ******/
ALTER TABLE [dbo].[WatchFolder] DROP CONSTRAINT [FK_WatchFolder_Destination]
GO
/****** Object:  ForeignKey [FK_Plugin_Assembly]    Script Date: 09/09/2010 13:52:10 ******/
ALTER TABLE [dbo].[Plugin] DROP CONSTRAINT [FK_Plugin_Assembly]
GO
/****** Object:  ForeignKey [FK_ExecutionSlot_Plugin_Join_ExecutionSlot]    Script Date: 09/09/2010 13:52:12 ******/
ALTER TABLE [dbo].[ExecutionSlot_Plugin_Join] DROP CONSTRAINT [FK_ExecutionSlot_Plugin_Join_ExecutionSlot]
GO
/****** Object:  ForeignKey [FK_ExecutionSlot_Plugin_Join_Plugin]    Script Date: 09/09/2010 13:52:12 ******/
ALTER TABLE [dbo].[ExecutionSlot_Plugin_Join] DROP CONSTRAINT [FK_ExecutionSlot_Plugin_Join_Plugin]
GO
/****** Object:  StoredProcedure [dbo].[Test_CleanAndInsertDummyData]    Script Date: 09/09/2010 13:52:12 ******/
DROP PROCEDURE [dbo].[Test_CleanAndInsertDummyData]
GO
/****** Object:  StoredProcedure [dbo].[Test_InsertDemoData]    Script Date: 09/09/2010 13:52:12 ******/
DROP PROCEDURE [dbo].[Test_InsertDemoData]
GO
/****** Object:  StoredProcedure [dbo].[Test_Insert]    Script Date: 09/09/2010 13:52:12 ******/
DROP PROCEDURE [dbo].[Test_Insert]
GO
/****** Object:  StoredProcedure [dbo].[Test_Clean]    Script Date: 09/09/2010 13:52:12 ******/
DROP PROCEDURE [dbo].[Test_Clean]
GO
/****** Object:  StoredProcedure [dbo].[PluginInfo_GetBy]    Script Date: 09/09/2010 13:52:12 ******/
DROP PROCEDURE [dbo].[PluginInfo_GetBy]
GO
/****** Object:  View [dbo].[PluginInfo]    Script Date: 09/09/2010 13:52:12 ******/
DROP VIEW [dbo].[PluginInfo]
GO
/****** Object:  StoredProcedure [dbo].[AssemblyInfo_Get]    Script Date: 09/09/2010 13:52:12 ******/
DROP PROCEDURE [dbo].[AssemblyInfo_Get]
GO
/****** Object:  Table [dbo].[ExecutionSlot_Plugin_Join]    Script Date: 09/09/2010 13:52:12 ******/
ALTER TABLE [dbo].[ExecutionSlot_Plugin_Join] DROP CONSTRAINT [FK_ExecutionSlot_Plugin_Join_ExecutionSlot]
GO
ALTER TABLE [dbo].[ExecutionSlot_Plugin_Join] DROP CONSTRAINT [FK_ExecutionSlot_Plugin_Join_Plugin]
GO
DROP TABLE [dbo].[ExecutionSlot_Plugin_Join]
GO
/****** Object:  View [dbo].[AssemblyInfo]    Script Date: 09/09/2010 13:52:12 ******/
DROP VIEW [dbo].[AssemblyInfo]
GO
/****** Object:  StoredProcedure [dbo].[Job_GetBy]    Script Date: 09/09/2010 13:52:12 ******/
DROP PROCEDURE [dbo].[Job_GetBy]
GO
/****** Object:  StoredProcedure [dbo].[Job_GetUnfinishedJobs]    Script Date: 09/09/2010 13:52:12 ******/
DROP PROCEDURE [dbo].[Job_GetUnfinishedJobs]
GO
/****** Object:  StoredProcedure [dbo].[Job_Insert]    Script Date: 09/09/2010 13:52:12 ******/
DROP PROCEDURE [dbo].[Job_Insert]
GO
/****** Object:  StoredProcedure [dbo].[Job_Update]    Script Date: 09/09/2010 13:52:12 ******/
DROP PROCEDURE [dbo].[Job_Update]
GO
/****** Object:  View [dbo].[JobInfo]    Script Date: 09/09/2010 13:52:12 ******/
DROP VIEW [dbo].[JobInfo]
GO
/****** Object:  Table [dbo].[Plugin]    Script Date: 09/09/2010 13:52:10 ******/
ALTER TABLE [dbo].[Plugin] DROP CONSTRAINT [FK_Plugin_Assembly]
GO
ALTER TABLE [dbo].[Plugin] DROP CONSTRAINT [DF_Plugin_CreatedDate]
GO
DROP TABLE [dbo].[Plugin]
GO
/****** Object:  StoredProcedure [dbo].[ExecutionSlot_GetBy]    Script Date: 09/09/2010 13:52:10 ******/
DROP PROCEDURE [dbo].[ExecutionSlot_GetBy]
GO
/****** Object:  StoredProcedure [dbo].[WatchFolder_Get]    Script Date: 09/09/2010 13:52:10 ******/
DROP PROCEDURE [dbo].[WatchFolder_Get]
GO
/****** Object:  Table [dbo].[WatchFolder]    Script Date: 09/09/2010 13:52:10 ******/
ALTER TABLE [dbo].[WatchFolder] DROP CONSTRAINT [FK_WatchFolder_Destination]
GO
ALTER TABLE [dbo].[WatchFolder] DROP CONSTRAINT [DF_WatchFolder_DateCreated]
GO
DROP TABLE [dbo].[WatchFolder]
GO
/****** Object:  Table [dbo].[Job]    Script Date: 09/09/2010 13:52:10 ******/
ALTER TABLE [dbo].[Job] DROP CONSTRAINT [FK_Job_Status]
GO
ALTER TABLE [dbo].[Job] DROP CONSTRAINT [DF_Job_StatusID]
GO
ALTER TABLE [dbo].[Job] DROP CONSTRAINT [DF_Job_CreatedDate]
GO
ALTER TABLE [dbo].[Job] DROP CONSTRAINT [DF_Job_LastUpdated]
GO
DROP TABLE [dbo].[Job]
GO
/****** Object:  Table [dbo].[ExecutionSlot]    Script Date: 09/09/2010 13:52:09 ******/
ALTER TABLE [dbo].[ExecutionSlot] DROP CONSTRAINT [FK_ExecutionSlot_Agent]
GO
ALTER TABLE [dbo].[ExecutionSlot] DROP CONSTRAINT [DF_ExecutionSlot_MaxSlots]
GO
ALTER TABLE [dbo].[ExecutionSlot] DROP CONSTRAINT [DF_ExecutionSlot_DateCreated]
GO
DROP TABLE [dbo].[ExecutionSlot]
GO
/****** Object:  StoredProcedure [dbo].[Agent_GetBy]    Script Date: 09/09/2010 13:52:09 ******/
DROP PROCEDURE [dbo].[Agent_GetBy]
GO
/****** Object:  Table [dbo].[Assembly]    Script Date: 09/09/2010 13:52:07 ******/
ALTER TABLE [dbo].[Assembly] DROP CONSTRAINT [FK_Assembly_Destination]
GO
ALTER TABLE [dbo].[Assembly] DROP CONSTRAINT [DF_Assembly_CreatedDate]
GO
DROP TABLE [dbo].[Assembly]
GO
/****** Object:  Table [dbo].[Destination]    Script Date: 09/09/2010 13:52:06 ******/
DROP TABLE [dbo].[Destination]
GO
/****** Object:  Table [dbo].[Agent]    Script Date: 09/09/2010 13:52:06 ******/
ALTER TABLE [dbo].[Agent] DROP CONSTRAINT [DF_Agent_DateCreated]
GO
DROP TABLE [dbo].[Agent]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 09/09/2010 13:52:06 ******/
DROP TABLE [dbo].[Status]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 09/09/2010 13:52:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Status](
	[ID] [int] NOT NULL,
	[Type] [varchar](64) NOT NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Agent]    Script Date: 09/09/2010 13:52:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Agent](
	[ID] [int] IDENTITY(1000,1) NOT NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_Agent_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_Agent] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Destination]    Script Date: 09/09/2010 13:52:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Destination](
	[ID] [int] NOT NULL,
	[WriteURL] [varchar](1024) NOT NULL,
	[ReadURL] [varchar](1024) NULL,
 CONSTRAINT [PK_Destination] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Assembly]    Script Date: 09/09/2010 13:52:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Assembly](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DestinationID] [int] NOT NULL,
	[Version] [varchar](64) NOT NULL,
	[Name] [varchar](512) NOT NULL,
	[Filename] [varchar](1024) NOT NULL,
	[CreatedDate] [smalldatetime] NOT NULL CONSTRAINT [DF_Assembly_CreatedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_Assembly] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[Agent_GetBy]    Script Date: 09/09/2010 13:52:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2009.11.25
-- Description:	This SP returns Agents
-- =============================================
CREATE PROCEDURE [dbo].[Agent_GetBy] 
	@ID				int		 = NULL,
	@DateCreated	datetime = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	*
	FROM	Agent
	WHERE	( @ID IS NULL OR Agent.ID = @ID ) AND
			( @DateCreated IS NULL OR Agent.DateCreated = @DateCreated )
END
GO
/****** Object:  Table [dbo].[ExecutionSlot]    Script Date: 09/09/2010 13:52:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExecutionSlot](
	[ID] [int] IDENTITY(1000,1) NOT NULL,
	[AgentID] [int] NOT NULL,
	[MaxSlots] [int] NOT NULL CONSTRAINT [DF_ExecutionSlot_MaxSlots]  DEFAULT ((0)),
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_ExecutionSlot_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_ExecutionSlot] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Job]    Script Date: 09/09/2010 13:52:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Job](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[StatusID] [int] NOT NULL CONSTRAINT [DF_Job_StatusID]  DEFAULT ((0)),
	[JobXML] xml NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_Job_CreatedDate]  DEFAULT (getdate()),
	[LastUpdated] [datetime] NOT NULL CONSTRAINT [DF_Job_LastUpdated]  DEFAULT (getdate()),
 CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_StatusID_A] ON [dbo].[Job] 
(
	[StatusID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WatchFolder]    Script Date: 09/09/2010 13:52:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WatchFolder](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DestinationID] [int] NOT NULL,
	[Filter] [nvarchar](max) NULL,
	[WorkflowXML] [xml] NOT NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_WatchFolder_DateCreated]  DEFAULT (getdate()),
	[IsEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_WatchFolder] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_IsEnabled_A] ON [dbo].[WatchFolder] 
(
	[IsEnabled] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[WatchFolder_Get]    Script Date: 09/09/2010 13:52:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2010.05.10
-- Description:	This SP return watchfolders
-- =============================================
CREATE PROCEDURE [dbo].[WatchFolder_Get]
	@ID			INT	= NULL,
	@IsEnabled	BIT = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	*
	  FROM	WatchFolder
	 WHERE	( @ID        IS NULL OR WatchFolder.ID        = @ID ) AND
			( @IsEnabled IS NULL OR WatchFolder.IsEnabled = @IsEnabled ) 
	
END
GO
/****** Object:  StoredProcedure [dbo].[ExecutionSlot_GetBy]    Script Date: 09/09/2010 13:52:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen	
-- Create date: 2009.11.25
-- Description:	This SP returns ExecutionSlots
-- =============================================
CREATE PROCEDURE [dbo].[ExecutionSlot_GetBy]
	@ID				int			= NULL,
	@AgentID	    int			= NULL,
	@MaxSlots		int			= NULL,
	@DateCreated	datetime	= NULL
AS
BEGIN

	SET NOCOUNT ON;

    SELECT	*
	FROM	ExecutionSlot
	WHERE	(@ID		  IS NULL OR ExecutionSlot.ID		   = @ID) AND
			(@AgentID	  IS NULL OR ExecutionSlot.AgentID	   = @AgentID) AND
			(@MaxSlots    IS NULL OR ExecutionSlot.MaxSlots    = @MaxSlots) AND
			(@DateCreated IS NULL OR ExecutionSlot.DateCreated = @DateCreated)

END
GO
/****** Object:  Table [dbo].[Plugin]    Script Date: 09/09/2010 13:52:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Plugin](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AssemblyID] [int] NOT NULL,
	[Name] [varchar](64) NOT NULL,
	[Description] [varchar](max) NULL,
	[Classname] [varchar](512) NOT NULL,
	[CreatedDate] [smalldatetime] NOT NULL CONSTRAINT [DF_Plugin_CreatedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_Plugin] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[JobInfo]    Script Date: 09/09/2010 13:52:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[JobInfo]
AS
SELECT     dbo.Job.ID, dbo.Job.ClientID, dbo.Status.Type AS Status, dbo.Job.JobXML, dbo.Job.CreatedDate, dbo.Job.LastUpdated
FROM         dbo.Job INNER JOIN
                      dbo.Status ON dbo.Job.StatusID = dbo.Status.ID
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[5] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Job"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 170
               Right = 198
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Status"
            Begin Extent = 
               Top = 6
               Left = 236
               Bottom = 95
               Right = 396
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1605
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'JobInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'JobInfo'
GO


GO
/****** Object:  View [dbo].[AssemblyInfo]    Script Date: 09/09/2010 13:52:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[AssemblyInfo]
AS
SELECT     dbo.[Assembly].ID, dbo.[Assembly].Version, dbo.[Assembly].Name, dbo.[Assembly].Filename, dbo.Destination.WriteURL + dbo.[Assembly].Filename AS WriteURL, 
                      dbo.Destination.ReadURL + dbo.[Assembly].Filename AS ReadURL, dbo.[Assembly].CreatedDate
FROM         dbo.[Assembly] INNER JOIN
                      dbo.Destination ON dbo.[Assembly].DestinationID = dbo.Destination.ID
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Assembly"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 198
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Destination"
            Begin Extent = 
               Top = 38
               Left = 354
               Bottom = 142
               Right = 514
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'AssemblyInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'AssemblyInfo'
GO
/****** Object:  Table [dbo].[ExecutionSlot_Plugin_Join]    Script Date: 09/09/2010 13:52:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExecutionSlot_Plugin_Join](
	[ExecutionSlotID] [int] NOT NULL,
	[PluginID] [int] NOT NULL,
 CONSTRAINT [PK_ExecutionSlot_Plugin_Join] PRIMARY KEY CLUSTERED 
(
	[ExecutionSlotID] ASC,
	[PluginID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[AssemblyInfo_Get]    Script Date: 09/09/2010 13:52:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2009.09.08
-- Description:	This SP return assemblies based on the search criteria supplied
-- =============================================
CREATE PROCEDURE [dbo].[AssemblyInfo_Get]
	@ID				int				= NULL,
    @Version		varchar(64)		= NULL,
    @Name			varchar(512)	= NULL,
    @Filename		varchar(1024)	= NULL,
	@WriteURL		varchar(1024)	= NULL,
	@ReadURL		varchar(1024)	= NULL,
    @CreatedDate	smalldatetime	= NULL,
	@IsAnd			bit				= 1
AS
BEGIN

	SET NOCOUNT ON;

	IF( @IsAnd = 1 )
	BEGIN
		SELECT	*
		FROM	AssemblyInfo
		WHERE	( @ID		   IS NULL OR ID = @ID ) AND
				( @Version     IS NULL OR Version = @Version ) AND
				( @Name        IS NULL OR [Name] = @Name ) AND
				( @Filename    IS NULL OR [Filename] = @Filename ) AND
				( @WriteURL    IS NULL OR WriteURL = @WriteURL ) AND
				( @ReadURL     IS NULL OR ReadURL = @ReadURL ) AND
				( @CreatedDate IS NULL OR CreatedDate = @CreatedDate )
	END
	ELSE
	BEGIN
		SELECT	*
		FROM	AssemblyInfo
		WHERE	( @ID		   IS NULL OR ID = @ID ) OR
				( @Version     IS NULL OR Version = @Version ) OR
				( @Name        IS NULL OR [Name] = @Name ) OR
				( @Filename    IS NULL OR [Filename] = @Filename ) OR
				( @WriteURL    IS NULL OR WriteURL = @WriteURL ) OR
				( @ReadURL     IS NULL OR ReadURL = @ReadURL ) OR
				( @CreatedDate IS NULL OR CreatedDate = @CreatedDate )
	END
END
GO
/****** Object:  View [dbo].[PluginInfo]    Script Date: 09/09/2010 13:52:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[PluginInfo]
AS
SELECT     dbo.Plugin.ID, dbo.Plugin.Name, dbo.[Assembly].Version, dbo.Plugin.Description, dbo.Plugin.Classname, dbo.[Assembly].Name AS [Assembly], 
                      dbo.[Assembly].Filename, dbo.Destination.WriteURL + dbo.[Assembly].Filename AS WriteURL, dbo.Destination.ReadURL + dbo.[Assembly].Filename AS ReadURL, 
                      dbo.Plugin.CreatedDate
FROM         dbo.Destination INNER JOIN
                      dbo.[Assembly] ON dbo.Destination.ID = dbo.[Assembly].DestinationID INNER JOIN
                      dbo.Plugin ON dbo.[Assembly].ID = dbo.Plugin.AssemblyID
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[21] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Destination"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 145
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Assembly"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 125
               Right = 387
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Plugin"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 187
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 360
         Width = 900
         Width = 1920
         Width = 2655
         Width = 1260
         Width = 3255
         Width = 3255
         Width = 1740
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PluginInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PluginInfo'
GO
/****** Object:  StoredProcedure [dbo].[PluginInfo_GetBy]    Script Date: 09/09/2010 13:52:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2009.05.11
-- Description:	This SP returns a list of PluginInfo based on the search specifications
-- =============================================
CREATE PROCEDURE [dbo].[PluginInfo_GetBy]
	@PluginID			int			  = NULL,
	@ExecutionSlotID	int		      = NULL,
	@Name				varchar(64)	  = NULL,
	@Version			varchar(64)   = NULL,
	@Description		varchar(MAX)  = NULL,
	@Assemblyname		varchar(512)  = NULL,
	@Classname			varchar(512)  = NULL,
	@WriteURL			varchar(2048) = NULL,
	@ReadURL			varchar(2048) = NULL,
	@CreatedDate		smalldatetime = NULL
AS
BEGIN

	SET NOCOUNT ON;

	IF( @ExecutionSlotID IS NULL )
	BEGIN
		SELECT	PluginInfo.*
		FROM	PluginInfo
		WHERE	( @PluginID IS NULL OR PluginInfo.ID = @PluginID ) AND
				( @Name IS NULL OR PluginInfo.Name = @Name ) AND
				( @Version IS NULL OR PluginInfo.Version = @Version ) AND
				( @Description IS NULL OR PluginInfo.Description = @Description ) AND
				( @Assemblyname IS NULL OR PluginInfo.Assembly = @Assemblyname ) AND
				( @Classname IS NULL OR PluginInfo.Classname = @Classname ) AND
				( @WriteURL IS NULL OR PluginInfo.WriteURL = @WriteURL ) AND
				( @ReadURL IS NULL OR PluginInfo.ReadURL = @ReadURL ) AND
				( @CreatedDate IS NULL OR PluginInfo.CreatedDate = @CreatedDate )
	END
	ELSE
	BEGIN
		SELECT	PluginInfo.*
		FROM	PluginInfo INNER JOIN
					ExecutionSlot_Plugin_Join ON PluginInfo.ID = ExecutionSlot_Plugin_Join.PluginID
		WHERE	( @PluginID IS NULL OR PluginInfo.ID = @PluginID ) AND
				( ExecutionSlot_Plugin_Join.ExecutionSlotID = @ExecutionSlotID ) AND
				( @Name IS NULL OR PluginInfo.Name = @Name ) AND
				( @Version IS NULL OR PluginInfo.Version = @Version ) AND
				( @Description IS NULL OR PluginInfo.Description = @Description ) AND
				( @Assemblyname IS NULL OR PluginInfo.Assembly = @Assemblyname ) AND
				( @Classname IS NULL OR PluginInfo.Classname = @Classname ) AND
				( @WriteURL IS NULL OR PluginInfo.WriteURL = @WriteURL ) AND
				( @ReadURL IS NULL OR PluginInfo.ReadURL = @ReadURL ) AND
				( @CreatedDate IS NULL OR PluginInfo.CreatedDate = @CreatedDate )
	END
END
GO
/****** Object:  StoredProcedure [dbo].[Test_Clean]    Script Date: 09/09/2010 13:52:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2009.05.07
-- Description:	This SP removed all database objects
-- =============================================
CREATE PROCEDURE [dbo].[Test_Clean]

AS
BEGIN
	
	DELETE	
	FROM	Job

	DELETE
	  FROM WatchFolder
	 WHERE	DestinationID = 1

	DELETE	
	FROM	[Status]
	WHERE   ID IN (0,100,250,500,750,999,1000,1001,2000,2001,3000,4000,5000,5001,6000,7000)
	
	DELETE
	FROM	ExecutionSlot_Plugin_Join

	DELETE
	FROM	ExecutionSlot
	
	DELETE
	FROM	Agent

	DELETE
	FROM	Plugin

	DELETE
	FROM	[Assembly]

	DELETE
	FROM	Destination
	WHERE	ID IN (1)

END
GO
/****** Object:  StoredProcedure [dbo].[Test_Insert]    Script Date: 09/09/2010 13:52:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2009.05.07
-- Description:	This SP Inserts dummy data
-- =============================================
CREATE PROCEDURE [dbo].[Test_Insert]
	@APP_Path	varchar(1024)
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO dbo.Status (ID,[Type]) VALUES( 0, 'New' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 100, 'Done' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 250, 'Pending' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 500, 'Job Loaded' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 750, 'Job Added to Queue' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 999, 'Test New' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 1000, 'Test Done' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 1001, 'Execute Started' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 2000, 'Execute Complete' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 2001, 'Commit Started' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 3000, 'Execute Failed' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 4000, 'Commit Complete' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 5000, 'Commit Failed' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 6000, 'Rollback Complete' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 7000, 'Rollback Failed' )

	INSERT 
	INTO	Destination (ID,WriteURL,ReadURL)
    VALUES (1,@APP_Path,@APP_Path)

	INSERT
	INTO	Agent 
			([DateCreated])
	VALUES	(getdate())
	
	INSERT 
	INTO	ExecutionSlot
           ([AgentID]
           ,[MaxSlots]
           ,[DateCreated])
    VALUES
           (@@IDENTITY
           ,4
           ,getdate())
           
    DECLARE @ExecutionSlotID int
    SET @ExecutionSlotID = @@IDENTITY

	INSERT
	INTO	[Assembly]
           ([DestinationID]
           ,[Version]
           ,[Name]
           ,[Filename])
    VALUES
           (1
		   ,'1.0.0.0'
           ,'Geckon.Octopus.Plugins.TestPlugin'
           ,'Geckon.Octopus.Plugins.TestPlugin.dll')

	DECLARE @AssemblyID	int
	SET @AssemblyID = @@IDENTITY

	INSERT 
	INTO	Plugin
           ([AssemblyID]
           ,[Name]
           ,[Description]
           ,[Classname])
    VALUES	(@AssemblyID
            ,'Test plugin'
			,'Some lengthy description'
			,'TestPlugin')
			
	INSERT 
	INTO	ExecutionSlot_Plugin_Join
           ([ExecutionSlotID]
           ,[PluginID])
    VALUES
           (@ExecutionSlotID
           ,@@IDENTITY)

	INSERT 
	INTO	Plugin
           ([AssemblyID]
           ,[Name]
           ,[Description]
           ,[Classname])
    VALUES	(@AssemblyID
            ,'Test plugin 2'
			,'Some lengthy description'
			,'TestPlugin2')
			
	INSERT 
	INTO	ExecutionSlot_Plugin_Join
           ([ExecutionSlotID]
           ,[PluginID])
    VALUES
           (@ExecutionSlotID
           ,@@IDENTITY)

	INSERT 
	INTO	Plugin
           ([AssemblyID]
           ,[Name]
           ,[Description]
           ,[Classname])
    VALUES	(@AssemblyID
            ,'FFmpegTestPlugin 1'
			,'Some lengthy description'
			,'FFmpegTestPlugin')

	INSERT 
	INTO	ExecutionSlot_Plugin_Join
           ([ExecutionSlotID]
           ,[PluginID])
    VALUES
           (@ExecutionSlotID
           ,@@IDENTITY)

	INSERT
	INTO	[Assembly]
           ([DestinationID]
           ,[Version]
           ,[Name]
           ,[Filename])
    VALUES
           (1
		   ,'1.0.0.0'
           ,'Geckon.Octopus.Plugins.Transcoding.Image'
           ,'Geckon.Octopus.Plugins.Transcoding.Image.dll')

	SET @AssemblyID = @@IDENTITY

	INSERT 
	INTO	Plugin
           ([AssemblyID]
           ,[Name]
           ,[Description]
           ,[Classname])
    VALUES	(@AssemblyID
            ,'ImageResizePlugin 1'
			,'Some lengthy description'
			,'ImageResizePlugin')
			
	INSERT 
	INTO	ExecutionSlot_Plugin_Join
           ([ExecutionSlotID]
           ,[PluginID])
    VALUES
           (@ExecutionSlotID
           ,@@IDENTITY)

	INSERT
	INTO	[Assembly]
           ([DestinationID]
           ,[Version]
           ,[Name]
           ,[Filename])
    VALUES
           (1
		   ,'1.0.0.0'
           ,'Geckon.Octopus.Plugins.Transcoding.FFmpeg'
           ,'Geckon.Octopus.Plugins.Transcoding.FFmpeg.dll')

	SET @AssemblyID = @@IDENTITY

	INSERT 
	INTO	Plugin
           ([AssemblyID]
           ,[Name]
           ,[Description]
           ,[Classname])
    VALUES	(@AssemblyID
            ,'FFmpegCreateStillPlugin 1'
			,'Some lengthy description'
			,'FFmpegCreateStillPlugin')
			
	INSERT 
	INTO	ExecutionSlot_Plugin_Join
           ([ExecutionSlotID]
           ,[PluginID])
    VALUES
           (@ExecutionSlotID
           ,@@IDENTITY)

	INSERT
	INTO	[Assembly]
           ([DestinationID]
           ,[Version]
           ,[Name]
           ,[Filename])
    VALUES
           (1
		   ,'1.0.0.0'
           ,'Geckon.Octopus.Plugins.FilePlugins'
           ,'Geckon.Octopus.Plugins.FilePlugins.dll')

	SET @AssemblyID = @@IDENTITY

	INSERT 
	INTO	Plugin
           ([AssemblyID]
           ,[Name]
           ,[Description]
           ,[Classname])
    VALUES	(@AssemblyID
            ,'FileCopyPlugin 1'
			,'Some lengthy description'
			,'FileCopyPlugin')
			
	INSERT 
	INTO	ExecutionSlot_Plugin_Join
           ([ExecutionSlotID]
           ,[PluginID])
    VALUES
           (@ExecutionSlotID
           ,@@IDENTITY)

	INSERT 
	INTO	Plugin
           ([AssemblyID]
           ,[Name]
           ,[Description]
           ,[Classname])
    VALUES	(@AssemblyID
            ,'FileDeletePlugin 1'
			,'Some lengthy description'
			,'FileDeletePlugin')

	INSERT 
	INTO	ExecutionSlot_Plugin_Join
           ([ExecutionSlotID]
           ,[PluginID])
    VALUES
           (@ExecutionSlotID
           ,@@IDENTITY)

--	DECLARE @PluginID int
--	SET	@PluginID = @@IDENTITY

	INSERT
	INTO	Job ([StatusID]
			    ,[JobXML])	
	VALUES	    (999
			    ,'<Job ClientID="1">
  <Step>
    <Plugin Class="Geckon.Octopus.Plugins.TestPlugin.TestPlugin" Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Plugin>
    <Plugin Class="Geckon.Octopus.Plugins.TestPlugin.TestPlugin" Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Plugin>
    <Plugin Class="Geckon.Octopus.Plugins.TestPlugin.TestPlugin" Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Plugin>
    <Flow>
      <Step>
        <Plugin Class="Geckon.Octopus.Plugins.TestPlugin.TestPlugin" Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Plugin>
        <Plugin Class="Geckon.Octopus.Plugins.TestPlugin.TestPlugin" Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Plugin>
      </Step>
      <Step>
        <Plugin Class="Geckon.Octopus.Plugins.TestPlugin.TestPlugin"  Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Plugin>
      </Step>
      <Step>
        <Plugin Class="Geckon.Octopus.Plugins.TestPlugin.TestPlugin"  Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Plugin>
        <Plugin Class="Geckon.Octopus.Plugins.TestPlugin.TestPlugin"  Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Plugin>
        <Plugin Class="Geckon.Octopus.Plugins.TestPlugin.TestPlugin"  Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Plugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Plugin Class="Geckon.Octopus.Plugins.TestPlugin.TestPlugin"  Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Plugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Plugin Class="Geckon.Octopus.Plugins.TestPlugin.TestPlugin"  Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Plugin>
  </Step>
</Job>')

END

INSERT INTO WatchFolder
           ([DestinationID]
           ,[WorkflowXML]
           ,[IsEnabled])
     VALUES
           (1
           ,'<Job ClientID="1">
			  <Step>
				<Plugin Class="Geckon.Octopus.Plugins.FilePlugins.FileDeletePlugin" Version="1.0.0.0">
				<FilePath>{{FILE_PATH}}</FilePath>
				</Plugin>
			  </Step>
			</Job>'
           ,1)
GO
/****** Object:  StoredProcedure [dbo].[Test_InsertDemoData]    Script Date: 09/09/2010 13:52:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--=============================================
 --Author:		Jesper Fyhr Knudsen
 --Create date: 2009.11.08
 --Description:	This SP Inserts demo Data
 --=============================================
CREATE PROCEDURE [dbo].[Test_InsertDemoData]
	@APP_Path	varchar(1024)
AS
BEGIN
	SET NOCOUNT ON;

	EXEC Test_Clean

	SET NOCOUNT ON;

	INSERT INTO dbo.Status (ID,[Type]) VALUES( 0, 'New' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 100, 'Done' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 250, 'Pending' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 500, 'Job Loaded' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 750, 'Job Added to Queue' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 999, 'Test New' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 1000, 'Test Done' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 1001, 'Execute Started' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 2000, 'Execute Complete' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 2001, 'Commit Started' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 3000, 'Execute Failed' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 4000, 'Commit Complete' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 5000, 'Commit Failed' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 6000, 'Rollback Complete' )
	INSERT INTO dbo.Status (ID,[Type]) VALUES( 7000, 'Rollback Failed' )

	INSERT 
	INTO	Destination (ID,WriteURL,ReadURL)
    VALUES (1,@APP_Path,@APP_Path)

	INSERT
	INTO	Agent 
			([DateCreated])
	VALUES	(getdate())
	
	INSERT 
	INTO	ExecutionSlot
           ([AgentID]
           ,[MaxSlots]
           ,[DateCreated])
    VALUES
           (@@IDENTITY
           ,4
           ,getdate())
           
    DECLARE @ExecutionSlotID int
    SET @ExecutionSlotID = @@IDENTITY
	DECLARE @AssemblyID	int

	INSERT
	INTO	[Assembly]
           ([DestinationID]
           ,[Version]
           ,[Name]
           ,[Filename])
    VALUES
           (1
		   ,'1.0.0.0'
           ,'Geckon.Octopus.Plugins.TestPlugin'
           ,'Geckon.Octopus.Plugins.TestPlugin.dll')

	SET @AssemblyID = @@IDENTITY

	INSERT 
	INTO	Plugin
           ([AssemblyID]
           ,[Name]
           ,[Description]
           ,[Classname])
    VALUES	(@AssemblyID
            ,'Test plugin'
			,'Some lengthy description'
			,'TestPlugin')
			
	INSERT 
	INTO	ExecutionSlot_Plugin_Join
           ([ExecutionSlotID]
           ,[PluginID])
    VALUES
           (@ExecutionSlotID
           ,@@IDENTITY)

	INSERT 
	INTO	Plugin
           ([AssemblyID]
           ,[Name]
           ,[Description]
           ,[Classname])
    VALUES	(@AssemblyID
            ,'Test plugin 2'
			,'Some lengthy description'
			,'TestPlugin2')
			
	INSERT 
	INTO	ExecutionSlot_Plugin_Join
           ([ExecutionSlotID]
           ,[PluginID])
    VALUES
           (@ExecutionSlotID
           ,@@IDENTITY)

	INSERT 
	INTO	Plugin
           ([AssemblyID]
           ,[Name]
           ,[Description]
           ,[Classname])
    VALUES	(@AssemblyID
            ,'FFmpegTestPlugin 1'
			,'Some lengthy description'
			,'FFmpegTestPlugin')

	INSERT 
	INTO	ExecutionSlot_Plugin_Join
           ([ExecutionSlotID]
           ,[PluginID])
    VALUES
           (@ExecutionSlotID
           ,@@IDENTITY)

	INSERT
	INTO	[Assembly]
           ([DestinationID]
           ,[Version]
           ,[Name]
           ,[Filename])
    VALUES
           (1
		   ,'1.0.0.0'
           ,'Geckon.Octopus.Plugins.Transcoding.Image'
           ,'Geckon.Octopus.Plugins.Transcoding.Image.dll')
           
	SET @AssemblyID = @@IDENTITY

	INSERT 
	INTO	Plugin
           ([AssemblyID]
           ,[Name]
           ,[Description]
           ,[Classname])
    VALUES	(@AssemblyID
            ,'ImageResizePlugin 1'
			,'Some lengthy description'
			,'ImageResizePlugin')
			
	INSERT 
	INTO	ExecutionSlot_Plugin_Join
           ([ExecutionSlotID]
           ,[PluginID])
    VALUES
           (@ExecutionSlotID
           ,@@IDENTITY)

	INSERT
	INTO	[Assembly]
           ([DestinationID]
           ,[Version]
           ,[Name]
           ,[Filename])
    VALUES
           (1
		   ,'1.0.0.0'
           ,'Geckon.Octopus.Plugins.Transcoding.FFmpeg'
           ,'Geckon.Octopus.Plugins.Transcoding.FFmpeg.dll')

	SET @AssemblyID = @@IDENTITY

	INSERT 
	INTO	Plugin
           ([AssemblyID]
           ,[Name]
           ,[Description]
           ,[Classname])
    VALUES	(@AssemblyID
            ,'FFmpegCreateStillPlugin 1'
			,'Some lengthy description'
			,'FFmpegCreateStillPlugin')
			
	INSERT 
	INTO	ExecutionSlot_Plugin_Join
           ([ExecutionSlotID]
           ,[PluginID])
    VALUES
           (@ExecutionSlotID
           ,@@IDENTITY)
           
           	INSERT 
	INTO	Plugin
           ([AssemblyID]
           ,[Name]
           ,[Description]
           ,[Classname])
    VALUES	(@AssemblyID
            ,'TranscodeTwoPassh264Plugin 1'
			,'Some lengthy description'
			,'TranscodeTwoPassh264Plugin')
			
	INSERT 
	INTO	ExecutionSlot_Plugin_Join
           ([ExecutionSlotID]
           ,[PluginID])
    VALUES
           (@ExecutionSlotID
           ,@@IDENTITY)

	INSERT
	INTO	[Assembly]
           ([DestinationID]
           ,[Version]
           ,[Name]
           ,[Filename])
    VALUES
           (1
		   ,'1.0.0.0'
           ,'Geckon.Octopus.Plugins.FilePlugins'
           ,'Geckon.Octopus.Plugins.FilePlugins.dll')

	SET @AssemblyID = @@IDENTITY

	INSERT 
	INTO	Plugin
           ([AssemblyID]
           ,[Name]
           ,[Description]
           ,[Classname])
    VALUES	(@AssemblyID
            ,'FileCopyPlugin 1'
			,'Some lengthy description'
			,'FileCopyPlugin')
			
	INSERT 
	INTO	ExecutionSlot_Plugin_Join
           ([ExecutionSlotID]
           ,[PluginID])
    VALUES
           (@ExecutionSlotID
           ,@@IDENTITY)

	INSERT 
	INTO	Plugin
           ([AssemblyID]
           ,[Name]
           ,[Description]
           ,[Classname])
    VALUES	(@AssemblyID
            ,'FileDeletePlugin 1'
			,'Some lengthy description'
			,'FileDeletePlugin')

	INSERT 
	INTO	ExecutionSlot_Plugin_Join
           ([ExecutionSlotID]
           ,[PluginID])
    VALUES
           (@ExecutionSlotID
           ,@@IDENTITY)

END
GO
/****** Object:  StoredProcedure [dbo].[Test_CleanAndInsertDummyData]    Script Date: 09/09/2010 13:52:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2009.05.07
-- Description:	This SP removed all database objects and inserts dummy data
-- =============================================
CREATE PROCEDURE [dbo].[Test_CleanAndInsertDummyData]
	@APP_Path	varchar(1024)
AS
BEGIN
	SET NOCOUNT ON;

	EXEC Test_Clean
	EXEC Test_Insert @APP_Path

END
GO
/****** Object:  ForeignKey [FK_Assembly_Destination]    Script Date: 09/09/2010 13:52:07 ******/
ALTER TABLE [dbo].[Assembly]  WITH CHECK ADD  CONSTRAINT [FK_Assembly_Destination] FOREIGN KEY([DestinationID])
REFERENCES [dbo].[Destination] ([ID])
GO
ALTER TABLE [dbo].[Assembly] CHECK CONSTRAINT [FK_Assembly_Destination]
GO
/****** Object:  ForeignKey [FK_ExecutionSlot_Agent]    Script Date: 09/09/2010 13:52:09 ******/
ALTER TABLE [dbo].[ExecutionSlot]  WITH CHECK ADD  CONSTRAINT [FK_ExecutionSlot_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([ID])
GO
ALTER TABLE [dbo].[ExecutionSlot] CHECK CONSTRAINT [FK_ExecutionSlot_Agent]
GO
/****** Object:  ForeignKey [FK_Job_Status]    Script Date: 09/09/2010 13:52:10 ******/
ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_Job_Status] FOREIGN KEY([StatusID])
REFERENCES [dbo].[Status] ([ID])
GO
ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK_Job_Status]
GO
/****** Object:  ForeignKey [FK_WatchFolder_Destination]    Script Date: 09/09/2010 13:52:10 ******/
ALTER TABLE [dbo].[WatchFolder]  WITH CHECK ADD  CONSTRAINT [FK_WatchFolder_Destination] FOREIGN KEY([DestinationID])
REFERENCES [dbo].[Destination] ([ID])
GO
ALTER TABLE [dbo].[WatchFolder] CHECK CONSTRAINT [FK_WatchFolder_Destination]
GO
/****** Object:  ForeignKey [FK_Plugin_Assembly]    Script Date: 09/09/2010 13:52:10 ******/
ALTER TABLE [dbo].[Plugin]  WITH CHECK ADD  CONSTRAINT [FK_Plugin_Assembly] FOREIGN KEY([AssemblyID])
REFERENCES [dbo].[Assembly] ([ID])
GO
ALTER TABLE [dbo].[Plugin] CHECK CONSTRAINT [FK_Plugin_Assembly]
GO
/****** Object:  ForeignKey [FK_ExecutionSlot_Plugin_Join_ExecutionSlot]    Script Date: 09/09/2010 13:52:12 ******/
ALTER TABLE [dbo].[ExecutionSlot_Plugin_Join]  WITH CHECK ADD  CONSTRAINT [FK_ExecutionSlot_Plugin_Join_ExecutionSlot] FOREIGN KEY([ExecutionSlotID])
REFERENCES [dbo].[ExecutionSlot] ([ID])
GO
ALTER TABLE [dbo].[ExecutionSlot_Plugin_Join] CHECK CONSTRAINT [FK_ExecutionSlot_Plugin_Join_ExecutionSlot]
GO
/****** Object:  ForeignKey [FK_ExecutionSlot_Plugin_Join_Plugin]    Script Date: 09/09/2010 13:52:12 ******/
ALTER TABLE [dbo].[ExecutionSlot_Plugin_Join]  WITH CHECK ADD  CONSTRAINT [FK_ExecutionSlot_Plugin_Join_Plugin] FOREIGN KEY([PluginID])
REFERENCES [dbo].[Plugin] ([ID])
GO
ALTER TABLE [dbo].[ExecutionSlot_Plugin_Join] CHECK CONSTRAINT [FK_ExecutionSlot_Plugin_Join_Plugin]
GO

-- =============================================
-- 2010.09.10 (RMJ)
-- =============================================
CREATE PROCEDURE [dbo].[JobTemplate_Get]
	@ID	int = NULL,
	@Name nvarchar(MAX) = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SELECT *
	FROM [JobTemplate]
	WHERE
		(@ID IS NULL OR [ID] = @ID)
	OR
		(@Name IS NULL OR [Name] = @Name) 

END

GO

USE [Octopus]
GO

CREATE TABLE [dbo].[JobTemplate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[JobTemplateXml] [xml] NOT NULL,
 CONSTRAINT [PK_JobTemplate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- =============================================
-- 2010.09.10 (RMJ)
-- =============================================
CREATE PROCEDURE [dbo].[JobTemplate_Create]
	@Name nvarchar(MAX) = NULL,
	@TemplateXml xml
AS
BEGIN

	INSERT INTO [JobTemplate] (
		Name, 
		JobTemplateXml) 
	OUTPUT 
		[inserted].*
	VALUES (
		@Name,
		@TemplateXml
		)

END

GO


alter table jobtemplate drop column jobtemplatexml

GO

alter table jobtemplate add  JobTemplateUri nvarchar(MAX)

GO

-- 2010.09.10 (RMJ)
-- 2010.09.17 (RMJ)
-- =============================================
ALTER PROCEDURE [dbo].[JobTemplate_Create]
	@Name nvarchar(MAX) = NULL,
	@TemplateUri nvarchar(MAX)
AS
BEGIN

	INSERT INTO [JobTemplate] (
		Name,
		JobTemplateUri
		)
	OUTPUT 
		[inserted].*
	VALUES (
		@Name,
		@TemplateUri
		)

END

GO



-- =============================================
-- 2010.09.10 (RMJ)
-- =============================================
ALTER PROCEDURE [dbo].[JobTemplate_Get]
	@ID	int = NULL,
	@Name nvarchar(MAX) = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SELECT *
	FROM [JobTemplate]
	WHERE
		(@ID IS NULL OR [ID] = @ID)
	AND
		(@Name IS NULL OR [Name] = @Name) 

END

GO

CREATE VIEW [dbo].[JobInfo]
AS
SELECT     ID, StatusID, JobXML, CreatedDate, LastUpdated, JobXML.value('Job[1]/@TotalProgress', 'nvarchar(MAX)') AS TotalProgress, 
                      JobXML.value('Job[1]/@OperationProgress', 'nvarchar(MAX)') AS OperationProgress
FROM         dbo.Job

GO


-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2009.05.11
-- Description:	This SP returns Jobs based on the search criteria supplied.
-- 2010.10.13 RMJ
-- =============================================
CREATE PROCEDURE [dbo].[Job_GetBy]
	@ID				int           = NULL,
	@StatusID		int           = NULL,
	@JobXML			nvarchar(MAX)  = NULL,
	@CreatedDate	smalldatetime = NULL,
	@IsAND			bit			  = 1
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @Sql nvarchar(4000)
	
	SET @Sql = '
		SELECT	*
		FROM	Job
		WHERE   (1=1)
		'
	
	IF (@ID IS NOT NULL)
		SET @Sql = @Sql + ' AND [ID] = ' + CAST(@ID as nvarchar(MAX))
		
	IF (@StatusID IS NOT NULL)
		SET @Sql = @Sql + ' AND [StatusID] = ' + CAST(@StatusID as nvarchar(MAX))
		
	IF (@JobXML IS NOT NULL)
		SET @Sql = @Sql + ' AND CAST([JobXML] AS nvarchar(MAX)) = ''' + @JobXML + ''''
		
	IF (@CreatedDate IS NOT NULL)
		SET @Sql = @Sql + ' AND [CreatedDate] = ''' + CAST(@CreatedDate as nvarchar(MAX)) + ''''
		
	IF (@IsAND = 0)
		SET @Sql = REPLACE(@Sql, ' AND ', ' OR ')
		
	PRINT @Sql
	EXEC sp_executesql @Sql
		
	-- For allowing LINQ to understand return type.
	SELECT * INTO #temp FROM JobInfo WHERE (1=2)
END

GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2009.07.14
-- Description:	This SP returns a list of non finished jobs
-- 2010.10.13 RMJ
-- =============================================
CREATE PROCEDURE [dbo].[Job_GetUnfinishedJobs]

AS
BEGIN

	SET NOCOUNT ON;
	
	SELECT	[ID]
			,[StatusID]
			,[JobXML]
			,[CreatedDate]
			,[LastUpdated]
	FROM	Job
	WHERE	StatusID <> 4000 AND
			StatusID <> 5000 AND
			StatusID <> 6000 AND
			StatusID <> 7000

END

GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 30.06.2009
-- Description:	This SP Inserts a Job into the database and returns the @@IDENTITY
-- 2010.10.13 RMJ
-- =============================================
CREATE PROCEDURE [dbo].[Job_Insert]
	@StatusID	int = 0,
	@JobXML		varchar(MAX)
AS
BEGIN

	INSERT
	INTO Job (
		[StatusID],
		[JobXML]
		)	
	VALUES (
		ISNULL(@StatusID,0),
		CAST (@JobXML AS xml)
		)
	SELECT	*
	FROM	Job
	WHERE	ID = @@IDENTITY

END

GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2009.07.29
-- Description:	This SP updates a Job in the database, by it's ID.
-- 2010.10.13 RMJ
-- =============================================
CREATE PROCEDURE [dbo].[Job_Update]
	@ID				INT,
	@StatusID		INT          = NULL,
	@JobXML			xml = NULL,
	@CreatedDate	DATETIME     = NULL
AS
BEGIN

	UPDATE Job
	   SET StatusID    = ISNULL(@StatusID,StatusID),
		   JobXML      = ISNULL(@JobXML,JobXML),
		   CreatedDate = ISNULL(@CreatedDate,CreatedDate),
		   LastUpdated = getdate()
	 WHERE ID = @ID

	SELECT * FROM Job WHERE ID = @ID

END




