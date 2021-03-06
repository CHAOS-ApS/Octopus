ALTER VIEW JobInfo AS
	SELECT     ID, StatusID, JobXML, CreatedDate, LastUpdated, JobXML.value('Job[1]/@TotalProgress', 'nvarchar(MAX)') AS TotalProgress, 
						  JobXML.value('Job[1]/@OperationProgress', 'nvarchar(MAX)') AS OperationProgress
	FROM         dbo.Job
GO


-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2009.05.11
-- Description:	This SP returns Jobs based on the search criteria supplied.
-- 2010.10.13 RMJ
-- 2010.11.19 RMJ - added created date desc sorting.
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
		
	SET @Sql = @Sql + ' ORDER BY [CreatedDate] DESC'
		
	PRINT @Sql
	EXEC sp_executesql @Sql
		
	-- For allowing LINQ to understand return type.
	SELECT * INTO #temp FROM JobInfo WHERE (1=2)
END
GO


