
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

