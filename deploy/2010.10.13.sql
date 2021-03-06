
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2009.05.11
-- Description:	This SP returns Jobs based on the search criteria supplied.
-- 2010.10.13 RMJ
-- =============================================
ALTER PROCEDURE [dbo].[Job_GetBy]
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
		

END

GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2009.07.14
-- Description:	This SP returns a list of non finished jobs
-- 2010.10.13 RMJ
-- =============================================
ALTER PROCEDURE [dbo].[Job_GetUnfinishedJobs]

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

ALTER TABLE Job ALTER COLUMN JobXml xml

GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 30.06.2009
-- Description:	This SP Inserts a Job into the database and returns the @@IDENTITY
-- 2010.10.13 RMJ
-- =============================================
ALTER PROCEDURE [dbo].[Job_Insert]
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
ALTER PROCEDURE [dbo].[Job_Update]
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




