USE [FDHS]
GO
/****** Object:  StoredProcedure [dbo].[Page]    Script Date: 2017/10/16 10:48:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Create date: <2012-9-12>
-- Description:    <高效分页存储过程，适用于Sql2005>
-- Notes:        <排序字段强烈建议建索引>-- sosoft.cnblogs.com
-- =============================================
ALTER Procedure [dbo].[Page] 
 @TableName varchar(50),        --表名
 @Fields varchar(1000) = '*',    --字段名(全部字段为*)
 @OrderField varchar(1000),        --排序字段(必须!支持多字段)
 @sqlWhere varchar(1000) = Null,--条件语句(不用加where)
 @pageSize int,                    --每页多少条记录
 @pageIndex int = 1 ,            --指定当前为第几页
 @TotalPage int output            --返回总页数
as
begin

    Begin Tran --开始事务

    Declare @sql nvarchar(4000);
    Declare @totalRecord int;    

    --计算总记录数
         
    if (@SqlWhere='' or @sqlWhere=NULL)
        set @sql = 'select @totalRecord = count(*) from ' + @TableName
    else
        set @sql = 'select @totalRecord = count(*) from ' + @TableName + ' where ' + @sqlWhere

    EXEC sp_executesql @sql,N'@totalRecord int OUTPUT',@totalRecord OUTPUT--计算总记录数       
    
    --计算总数量
    select @TotalPage=@totalRecord--CEILING((@totalRecord+0.0)/@PageSize)+1

    if (@SqlWhere='' or @sqlWhere=NULL)
        set @sql = 'Select * FROM (select ROW_NUMBER() Over(order by ' + @OrderField + ') as 序号,' + @Fields + ' from ' + @TableName 
    else
        set @sql = 'Select * FROM (select ROW_NUMBER() Over(order by ' + @OrderField + ') as 序号,' + @Fields + ' from ' + @TableName + ' where ' + @SqlWhere    
        
    
    --处理页数超出范围情况
    if @PageIndex<=0 
        Set @pageIndex = 1
    
    if @pageIndex>@TotalPage
        Set @pageIndex = @TotalPage

     --处理开始点和结束点
    Declare @StartRecord int
    Declare @EndRecord int
    
    set @StartRecord = (@pageIndex-1)*@PageSize + 1
    set @EndRecord = @StartRecord + @pageSize - 1

    --继续合成sql语句
    set @Sql = @Sql + ') as ' + @TableName + ' where 序号 between ' + Convert(varchar(50),@StartRecord) + ' and ' +  Convert(varchar(50),@EndRecord)
    --print @Sql
    Exec(@Sql)

--    print @totalRecord 
    If @@Error <> 0
      Begin
        RollBack Tran
        Return -1
      End
     Else
      Begin
        Commit Tran
        --print @totalRecord 
        Return @totalRecord ---返回记录总数
        
        
      End   
    
end