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


