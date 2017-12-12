--授予 用户存储过程或者函数权限

--用户名
DECLARE @userName VARCHAR(20)
--存储过程或者函数
DECLARE @type VARCHAR(10)
SET @userName = '';
SET @type = 'PROCEDURE'

DECLARE @sqlText VARCHAR(1000);
DECLARE @UserId    INT;

SELECT @UserId = uid FROM sys.sysusers WHERE name = @userName;

    IF @type = 'PROCEDURE'
	BEGIN
	    CREATE TABLE #ProcedureName( SqlText  VARCHAR(max));
		INSERT  INTO #ProcedureName
		    SELECT  'GRANT EXECUTE ON ' + p.name + ' TO ' + @userName + ';' FROM sys.procedures p
            WHERE   NOT EXISTS (SELECT 1 FROM   sys.database_permissions r
                                WHERE  r.major_id = p.object_id AND r.grantee_principal_id = @UserId AND r.permission_name IS NOT  NULL)

	    SELECT * FROM #ProcedureName;
            --SELECT  'GRANT EXECUTE ON ' + NAME + ' TO ' +@userName +';'
            --FROM    sys.procedures;
            --SELECT 'GRANT EXECUTE ON ' + [name] + ' TO ' +@userName +';'
            -- FROM sys.all_objects
            --WHERE [type]='P' OR [type]='X' OR [type]='PC'
            
        DECLARE cr_procedure CURSOR FOR SELECT * FROM #ProcedureName;

        OPEN cr_procedure;
        FETCH NEXT FROM cr_procedure  INTO @sqlText;
        WHILE @@FETCH_STATUS = 0
        BEGIN        
            EXECUTE(@sqlText);            
            FETCH NEXT FROM cr_procedure INTO @sqlText;
        END            
        CLOSE cr_procedure;
        DEALLOCATE cr_procedure;
        
        END
    ELSE
        IF @type='FUNCTION'
		BEGIN
		    CREATE TABLE #FunctionSet( functionName VARCHAR(1000));

            INSERT  INTO #FunctionSet 
			    SELECT  'GRANT EXEC ON ' + name + ' TO ' + @userName + ';' FROM    sys.all_objects s 
				WHERE NOT EXISTS (SELECT 1 FROM sys.database_permissions p
                                  WHERE  p.major_id = s.object_id AND  p.grantee_principal_id = @UserId)
                      AND schema_id = SCHEMA_ID('dbo')
                      AND( s.[type] = 'FN' OR s.[type] = 'AF' OR s.[type] = 'FS' OR s.[type] = 'FT');
 
		    SELECT * FROM #FunctionSet;
                    --SELECT 'GRANT EXEC ON ' + name + ' TO ' + @userName +';' FROM sys.all_objects
                    -- WHERE schema_id =schema_id('dbo')
                    --     AND ([type]='FN' OR [type] ='AF' OR [type]='FS' OR [type]='FT' );
                
            INSERT  INTO #FunctionSet
            SELECT  'GRANT SELECT ON ' + name + ' TO ' + @userName + ';'
            FROM    sys.all_objects s
            WHERE   NOT EXISTS (SELECT 1 FROM   sys.database_permissions p
                                WHERE  p.major_id = s.object_id AND  p.grantee_principal_id = @UserId)
                    AND schema_id = SCHEMA_ID('dbo')
                    AND( s.[type] = 'TF' OR s.[type] = 'IF');

			SELECT * FROM #FunctionSet;
			--SELECT 'GRANT SELECT ON ' + name + ' TO ' + @userName +';' FROM sys.all_objects
			-- WHERE schema_id =schema_id('dbo')
			--AND ([type]='TF' OR  [type]='IF') ;         
                         
                    
			DECLARE cr_Function CURSOR FOR

			SELECT functionName FROM #FunctionSet;
			OPEN cr_Function;
			FETCH NEXT FROM cr_Function INTO @sqlText;
			WHILE @@FETCH_STATUS = 0
			BEGIN    
			    PRINT(@sqlText);
			    EXEC(@sqlText);
			    FETCH NEXT FROM cr_Function INTO @sqlText;
			END
			CLOSE cr_Function;
			DEALLOCATE cr_Function;

           END
GO