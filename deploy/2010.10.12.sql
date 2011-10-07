ALTER TABLE Job ALTER COLUMN JobXML xml

GO

CREATE VIEW [dbo].[JobInfo]
AS
SELECT     ID, StatusID, JobXML, CreatedDate, LastUpdated, JobXML.value('Job[1]/@TotalProgress', 'nvarchar(MAX)') AS TotalProgress, 
                      JobXML.value('Job[1]/@OperationProgress', 'nvarchar(MAX)') AS OperationProgress
FROM         dbo.Job

GO