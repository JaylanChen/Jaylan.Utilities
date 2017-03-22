--逻辑查询处理阶段简介

--FROM：对FROM子句中的前两个表执行笛卡尔积（Cartesian product)(交叉联接），生成虚拟表VT1
--ON：对VT1应用ON筛选器。只有那些使<join_condition>为真的行才被插入VT2。
--OUTER(JOIN)：如 果指定了OUTER JOIN（相对于CROSS JOIN 或(INNER JOIN),保留表（preserved table：左外部联接把左表标记为保留表，右外部联接把右表标记为保留表，完全外部联接把两个表都标记为保留表）中未找到匹配的行将作为外部行添加到 VT2,生成VT3.如果FROM子句包含两个以上的表，则对上一个联接生成的结果表和下一个表重复执行步骤1到步骤3，直到处理完所有的表为止。
--WHERE：对VT3应用WHERE筛选器。只有使<where_condition>为true的行才被插入VT4.
--GROUP BY：按GROUP BY子句中的列列表对VT4中的行分组，生成VT5.
--CUBE|ROLLUP：把超组(Suppergroups)插入VT5,生成VT6.
--HAVING：对VT6应用HAVING筛选器。只有使<having_condition>为true的组才会被插入VT7.
--SELECT：处理SELECT列表，产生VT8.
--DISTINCT：将重复的行从VT8中移除，产生VT9.
--ORDER BY：将VT9中的行按ORDER BY 子句中的列列表排序，生成游标（VC10).
--TOP：从VC10的开始处选择指定数量或比例的行，生成表VT11,并返回调用者。
--注：步骤10，这一步是第一步也是唯一一步可以使用SELECT列表中的列别名的步骤。


select top 100 *  from dbo.STUDENT

SELECT COUNT(1) FROM STUDENT
declare @i int 
set @i=1

--insert into dbo.STUDENT values(REPLICATE('Tom'),ceiling (RAND()*5),)
while @i<20000
begin
Insert into STUDENT values(Replicate('Tom',ceiling(Rand()*5)),floor(Rand()*100), floor(Rand()*2),Replicate('Hollywood',ceiling(Rand()*10)),DATEADD(mi,ceiling(Rand()*10),GETDATE()),ceiling(rand()*5))
set @i=@i+1
end

insert into STUDENT select NAME,AGE,SEX,[ADDRESS],ADDTIME,CLASSID from STUDENT WHERE ID<100000

declare @TIME DATETIME 
SET @TIME=GETDATE()
 SELECT TOP 10 * FROM STUDENT WHERE id NOT IN(SELECT TOP 500000 addtime FROM STUDENT ORDER BY id)ORDER BY id
SELECT DATEDIFF(MS,@TIME,GETDATE()) AS TIMESPAN 

declare @TIME DATETIME 
SET @TIME=GETDATE()
 SELECT TOP 10 * FROM STUDENT WHERE addtime > 
 (SELECT MAX(addtime)FROM (SELECT TOP 500000 id FROM STUDENT ORDER BY id)AS T )ORDER BY id
SELECT DATEDIFF(MS,@TIME,GETDATE()) AS TIMESPAN 

declare @TIME DATETIME 
SET @TIME=GETDATE()
select  * from (select *,Row_number() over(order by id asc) as RankId
 from STUDENT) as temp where temp.RankId between 500000  and 500010
 SELECT DATEDIFF(MS,@TIME,GETDATE()) AS TIMESPAN 

declare @TIME DATETIME SET @TIME=GETDATE() with Table_CTE as 
                
(select ceiling((Row_number() over(order by ID ASC))/10) as page_num,* from dbo.STUDENT) 
select * From Table_CTE where page_num=50000;
 
SELECT DATEDIFF(MS,@TIME,GETDATE()) AS TIMESPAN

CREATE CLUSTERED INDEX index_compex on student(id,addtime)

drop index index_addtime on student

delete student where id<100000

select *  from student where addtime ='2014-02-15 16:16:36.287'




--查询表字段的默认值
SELECT SO.NAME AS "Table Name", SC.NAME AS "Column Name", SM.TEXT AS "Default Value"
FROM dbo.sysobjects SO INNER JOIN dbo.syscolumns SC ON SO.id = SC.id
LEFT JOIN dbo.syscomments SM ON SC.cdefault = SM.id
WHERE SO.xtype = 'U'
ORDER BY SO.[name], SC.colid

--查询所有默认值的字段
SELECT SO.NAME AS "Table Name", SC.NAME AS "Column Name", SM.TEXT AS "Default Value"
FROM dbo.sysobjects SO INNER JOIN dbo.syscolumns SC ON SO.id = SC.id
INNER JOIN dbo.syscomments SM ON SC.cdefault = SM.id
WHERE SO.xtype = 'U'
ORDER BY SO.[name], SC.colid

--添加默认值约束
alter table DeptTree add constraint DF_DeptTree_URL DEFAULT (('ss')) for URL

--添加共用的默认值约束
--create DEFAULT NowTime AS getdate()
go
--把共用的默认值添加到 表的某个字段上
EXEC sp_bindefault 'NowTime', 'UserInfo.[CreateTime]'