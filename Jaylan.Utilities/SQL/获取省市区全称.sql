
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jaylan
-- Create date: 2017-11-07
-- Description:	根据地区编码获取省市区名称
-- =============================================
CREATE FUNCTION [dbo].GetFullRegionName(@regionCode varchar(20))

RETURNS nvarchar(50)
AS
BEGIN
	DECLARE @tempRegionName nvarchar(50);
	DECLARE @regionFullName nvarchar(100);

	set @regionFullName = '';
	set @tempRegionName = (select [Name] from TpmRegion where Code = @regionCode)
	while @tempRegionName is not null
	begin
	    set @regionFullName = @tempRegionName + ' ' + @regionFullName

		set @regionCode = (SELECT ParentCode from TpmRegion where Code = @regionCode)
	    set @tempRegionName = (select [Name] from TpmRegion where Code = @regionCode)
	end

	RETURN @regionFullName

END
GO

