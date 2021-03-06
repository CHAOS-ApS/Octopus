USE [Octopus]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 12/07/2009 14:22:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[ID] [int] NOT NULL,
	[CreatedDate] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Destination]    Script Date: 12/07/2009 14:22:47 ******/
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
/****** Object:  Table [dbo].[Status]    Script Date: 12/07/2009 14:22:47 ******/
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
/****** Object:  StoredProcedure [dbo].[AssemblyInfo_Get]    Script Date: 12/07/2009 14:22:43 ******/
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
/****** Object:  Table [dbo].[Agent]    Script Date: 12/07/2009 14:22:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Agent](
	[ID] [int] IDENTITY(1000,1) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Agent] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Job]    Script Date: 12/07/2009 14:22:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Job](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ClientID] [int] NOT NULL,
	[StatusID] [int] NOT NULL,
	[JobXML] [varchar](max) NOT NULL,
	[CreatedDate] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Assembly]    Script Date: 12/07/2009 14:22:47 ******/
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
	[CreatedDate] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_Assembly] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Plugin]    Script Date: 12/07/2009 14:22:47 ******/
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
	[CreatedDate] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_Plugin] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ExecutionSlot]    Script Date: 12/07/2009 14:22:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExecutionSlot](
	[ID] [int] IDENTITY(1000,1) NOT NULL,
	[AgentID] [int] NOT NULL,
	[MaxSlots] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_ExecutionSlot] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExecutionSlot_Plugin_Join]    Script Date: 12/07/2009 14:22:47 ******/
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
/****** Object:  StoredProcedure [dbo].[Test_Clean]    Script Date: 12/07/2009 14:22:43 ******/
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
	WHERE	StatusID IN (999,1000,100,0)

	DELETE 
	FROM	Client
	WHERE	ID IN (1)
	
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
/****** Object:  StoredProcedure [dbo].[Test_Insert]    Script Date: 12/07/2009 14:22:43 ******/
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

	INSERT 
	INTO	Client (ID)
    VALUES	(1)

	INSERT 
	INTO	Destination (ID,WriteURL,ReadURL)
    VALUES (1,@APP_Path,@APP_Path)

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

--	DECLARE @PluginID int
--	SET	@PluginID = @@IDENTITY

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (1
			    ,999
			    ,'<Job ClientID="1">
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
    <Flow>
      <Step>
        <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
          <ExecuteDuration>100</ExecuteDuration>
          <CommitDuration>100</CommitDuration>
          <RollbackDuration>100</RollbackDuration>
        </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
      </Step>
    </Flow>
  </Step>
  <Step>
    <Geckon.Octopus.Plugins.TestPlugin.TestPlugin Version="1.0.0.0">
      <ExecuteDuration>100</ExecuteDuration>
      <CommitDuration>100</CommitDuration>
      <RollbackDuration>100</RollbackDuration>
    </Geckon.Octopus.Plugins.TestPlugin.TestPlugin>
  </Step>
</Job>')


END
GO
/****** Object:  View [dbo].[PluginInfo]    Script Date: 12/07/2009 14:22:49 ******/
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
/****** Object:  View [dbo].[AssemblyInfo]    Script Date: 12/07/2009 14:22:49 ******/
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
/****** Object:  StoredProcedure [dbo].[Job_Insert]    Script Date: 12/07/2009 14:22:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 30.06.2009
-- Description:	This SP Inserts a Job into the database and returns the @@IDENTITY
-- =============================================
CREATE PROCEDURE [dbo].[Job_Insert]
	@ClientID	int,
	@StatusID	int = 0,
	@JobXML		varchar(MAX)
AS
BEGIN

	INSERT
	INTO	Job ([ClientID]
			    ,[StatusID]
			    ,[JobXML])	
	VALUES	    (@ClientID
			    ,@StatusID
			    ,@JobXML)

	SELECT	*
	FROM	Job
	WHERE	Job.ID = @@IDENTITY

END
GO
/****** Object:  StoredProcedure [dbo].[Job_GetUnfinishedJobs]    Script Date: 12/07/2009 14:22:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2009.07.14
-- Description:	This SP returns a list of non finished jobs
-- =============================================
CREATE PROCEDURE [dbo].[Job_GetUnfinishedJobs]

AS
BEGIN

	SET NOCOUNT ON;

	SELECT	*
	FROM	Job
	WHERE	Job.StatusID NOT IN ( 100 )

END
GO
/****** Object:  StoredProcedure [dbo].[Job_Update]    Script Date: 12/07/2009 14:22:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2009.07.29
-- Description:	This SP updates a Job in the database, by it's ID.
-- =============================================
CREATE PROCEDURE [dbo].[Job_Update]
	@ID				INT,
	@ClientID		INT          = NULL,
	@StatusID		INT          = NULL,
	@JobXML			VARCHAR(MAX) = NULL,
	@CreatedDate	DATETIME     = NULL
AS
BEGIN

	UPDATE Job
	   SET ClientID    = ISNULL(@ClientID,ClientID),
		   StatusID    = ISNULL(@StatusID,StatusID),
		   JobXML      = ISNULL(@JobXML,JobXML),
		   CreatedDate = ISNULL(@CreatedDate,CreatedDate)
	 WHERE ID = @ID

	--RETURN SELECT @@ROWCOUNT
END
GO
/****** Object:  StoredProcedure [dbo].[Job_GetBy]    Script Date: 12/07/2009 14:22:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2009.05.11
-- Description:	This SP returns Jobs based on the search criteria supplied.
-- =============================================
CREATE PROCEDURE [dbo].[Job_GetBy]
	@ID				int           = NULL,
	@ClientID		int           = NULL,
	@StatusID		int           = NULL,
	@JobXML			varchar(MAX)  = NULL,
	@CreatedDate	smalldatetime = NULL,
	@IsAND			bit			  = 1
AS
BEGIN

	SET NOCOUNT ON;

	IF( @IsAND = 1 )
	BEGIN
		SELECT	*
		FROM	Job
		WHERE	( @ID		   IS NULL OR Job.ID = @ID ) AND
				( @ClientID    IS NULL OR Job.ClientID = @ClientID ) AND
				( @StatusID    IS NULL OR Job.StatusID = @StatusID ) AND
				( @JobXML      IS NULL OR Job.JobXML = @JobXML ) AND
				( @CreatedDate IS NULL OR Job.CreatedDate = @CreatedDate )
	END
	ELSE
	BEGIN
		SELECT	*
		FROM	Job
		WHERE	( @ID		   IS NULL OR Job.ID = @ID ) OR
				( @ClientID    IS NULL OR Job.ClientID = @ClientID ) OR
				( @StatusID    IS NULL OR Job.StatusID = @StatusID ) OR
				( @JobXML      IS NULL OR Job.JobXML = @JobXML ) OR
				( @CreatedDate IS NULL OR Job.CreatedDate = @CreatedDate )
	END

END
GO
/****** Object:  StoredProcedure [dbo].[Agent_GetBy]    Script Date: 12/07/2009 14:22:43 ******/
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
/****** Object:  StoredProcedure [dbo].[ExecutionSlot_GetBy]    Script Date: 12/07/2009 14:22:43 ******/
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
	@MaxSlots		int			= NULL,
	@DateCreated	datetime	= NULL
AS
BEGIN

	SET NOCOUNT ON;

    SELECT	*
	FROM	ExecutionSlot
	WHERE	(@ID		  IS NULL OR ExecutionSlot.ID		   = @ID) AND
			(@MaxSlots    IS NULL OR ExecutionSlot.MaxSlots    = @MaxSlots) AND
			(@DateCreated IS NULL OR ExecutionSlot.DateCreated = @DateCreated)

END
GO
/****** Object:  StoredProcedure [dbo].[Test_InsertDemoData]    Script Date: 12/07/2009 14:22:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2009.11.08
-- Description:	This SP Inserts demo Data
-- =============================================
CREATE PROCEDURE [dbo].[Test_InsertDemoData]
	@APP_Path	varchar(1024)
AS
BEGIN
	SET NOCOUNT ON;

	EXEC Test_Clean

	INSERT 
	INTO	Client (ID)
    VALUES	(1)

	INSERT 
	INTO	Destination (ID,WriteURL,ReadURL)
    VALUES (1,@APP_Path,@APP_Path)

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
	INTO	Plugin
           ([AssemblyID]
           ,[Name]
           ,[Description]
           ,[Classname])
    VALUES	(@AssemblyID
            ,'FileDeletePlugin 1'
			,'Some lengthy description'
			,'FileDeletePlugin')

END
GO
/****** Object:  StoredProcedure [dbo].[Test_CleanAndInsertDummyData]    Script Date: 12/07/2009 14:22:43 ******/
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
/****** Object:  StoredProcedure [dbo].[PluginInfo_GetBy]    Script Date: 12/07/2009 14:22:43 ******/
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
/****** Object:  Default [DF_Agent_DateCreated]    Script Date: 12/07/2009 14:22:47 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_Assembly_CreatedDate]    Script Date: 12/07/2009 14:22:47 ******/
ALTER TABLE [dbo].[Assembly] ADD  CONSTRAINT [DF_Assembly_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
/****** Object:  Default [DF_Client_ID]    Script Date: 12/07/2009 14:22:47 ******/
ALTER TABLE [dbo].[Client] ADD  CONSTRAINT [DF_Client_ID]  DEFAULT ((0)) FOR [ID]
GO
/****** Object:  Default [DF_Client_CreatedDate]    Script Date: 12/07/2009 14:22:47 ******/
ALTER TABLE [dbo].[Client] ADD  CONSTRAINT [DF_Client_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
/****** Object:  Default [DF_ExecutionSlot_MaxSlots]    Script Date: 12/07/2009 14:22:47 ******/
ALTER TABLE [dbo].[ExecutionSlot] ADD  CONSTRAINT [DF_ExecutionSlot_MaxSlots]  DEFAULT ((0)) FOR [MaxSlots]
GO
/****** Object:  Default [DF_ExecutionSlot_DateCreated]    Script Date: 12/07/2009 14:22:47 ******/
ALTER TABLE [dbo].[ExecutionSlot] ADD  CONSTRAINT [DF_ExecutionSlot_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_Job_StatusID]    Script Date: 12/07/2009 14:22:47 ******/
ALTER TABLE [dbo].[Job] ADD  CONSTRAINT [DF_Job_StatusID]  DEFAULT ((0)) FOR [StatusID]
GO
/****** Object:  Default [DF_Job_CreatedDate]    Script Date: 12/07/2009 14:22:47 ******/
ALTER TABLE [dbo].[Job] ADD  CONSTRAINT [DF_Job_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
/****** Object:  Default [DF_Plugin_CreatedDate]    Script Date: 12/07/2009 14:22:47 ******/
ALTER TABLE [dbo].[Plugin] ADD  CONSTRAINT [DF_Plugin_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
/****** Object:  ForeignKey [FK_Assembly_Destination]    Script Date: 12/07/2009 14:22:47 ******/
ALTER TABLE [dbo].[Assembly]  WITH CHECK ADD  CONSTRAINT [FK_Assembly_Destination] FOREIGN KEY([DestinationID])
REFERENCES [dbo].[Destination] ([ID])
GO
ALTER TABLE [dbo].[Assembly] CHECK CONSTRAINT [FK_Assembly_Destination]
GO
/****** Object:  ForeignKey [FK_ExecutionSlot_Agent]    Script Date: 12/07/2009 14:22:47 ******/
ALTER TABLE [dbo].[ExecutionSlot]  WITH CHECK ADD  CONSTRAINT [FK_ExecutionSlot_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([ID])
GO
ALTER TABLE [dbo].[ExecutionSlot] CHECK CONSTRAINT [FK_ExecutionSlot_Agent]
GO
/****** Object:  ForeignKey [FK_ExecutionSlot_Plugin_Join_ExecutionSlot]    Script Date: 12/07/2009 14:22:47 ******/
ALTER TABLE [dbo].[ExecutionSlot_Plugin_Join]  WITH CHECK ADD  CONSTRAINT [FK_ExecutionSlot_Plugin_Join_ExecutionSlot] FOREIGN KEY([ExecutionSlotID])
REFERENCES [dbo].[ExecutionSlot] ([ID])
GO
ALTER TABLE [dbo].[ExecutionSlot_Plugin_Join] CHECK CONSTRAINT [FK_ExecutionSlot_Plugin_Join_ExecutionSlot]
GO
/****** Object:  ForeignKey [FK_ExecutionSlot_Plugin_Join_Plugin]    Script Date: 12/07/2009 14:22:47 ******/
ALTER TABLE [dbo].[ExecutionSlot_Plugin_Join]  WITH CHECK ADD  CONSTRAINT [FK_ExecutionSlot_Plugin_Join_Plugin] FOREIGN KEY([PluginID])
REFERENCES [dbo].[Plugin] ([ID])
GO
ALTER TABLE [dbo].[ExecutionSlot_Plugin_Join] CHECK CONSTRAINT [FK_ExecutionSlot_Plugin_Join_Plugin]
GO
/****** Object:  ForeignKey [FK_Job_Client]    Script Date: 12/07/2009 14:22:47 ******/
ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_Job_Client] FOREIGN KEY([ClientID])
REFERENCES [dbo].[Client] ([ID])
GO
ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK_Job_Client]
GO
/****** Object:  ForeignKey [FK_Job_Status]    Script Date: 12/07/2009 14:22:47 ******/
ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_Job_Status] FOREIGN KEY([StatusID])
REFERENCES [dbo].[Status] ([ID])
GO
ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK_Job_Status]
GO
/****** Object:  ForeignKey [FK_Plugin_Assembly]    Script Date: 12/07/2009 14:22:47 ******/
ALTER TABLE [dbo].[Plugin]  WITH CHECK ADD  CONSTRAINT [FK_Plugin_Assembly] FOREIGN KEY([AssemblyID])
REFERENCES [dbo].[Assembly] ([ID])
GO
ALTER TABLE [dbo].[Plugin] CHECK CONSTRAINT [FK_Plugin_Assembly]
GO
