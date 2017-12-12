CREATE FUNCTION [dbo].[CalculateGradeFromEntryDate](@entryYear int, @schoolYear int, @schoolLevel int)
--EntryYear   入学年份
--SchoolYear  学年制
--SchoolLevel  小初高
RETURNS Nvarchar(50)

AS
BEGIN
    DECLARE @grade int;
    DECLARE @nowDate Datetime = getdate();
	DECLARE @gradeName nvarchar(50);
	
	SET @gradeName = N'小学 ';
    SET @grade = YEAR(@nowDate) - @entryYear;
	
    --9月份，新的学年
    IF(MONTH(@nowDate) >= 9)
    BEGIN
        SET @grade += 1;
    END
    --已毕业
    IF(@grade > @schoolYear or @grade < 0)
    BEGIN
        SET @grade = -1;
    END

    --初中，年级+6
    Else IF(@schoolLevel = 9)
	BEGIN
	    SET @grade +=6;
		SET @gradeName = N'初中 ';
	END
    --高中，年级+9
    Else IF(@schoolLevel = 12)
	BEGIN
	    SET @grade +=6;
		SET @gradeName = N'高中 ';
    END

	IF(@grade < 0)
    BEGIN
        SET @gradeName += CONVERT(nvarchar(10),  @entryYear + @schoolYear) + N'届';
    END
	ELSE
	BEGIN
       SET @gradeName = CONVERT(nvarchar(10), @grade) + N'年级';
	End

    RETURN @gradeName;

END