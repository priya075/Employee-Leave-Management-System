CREATE   PROCEDURE sp_GetTeamLeaveRequests  
    @ManagerId INT  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    SELECT *  
    FROM tblLeaveRequests  
    WHERE inManagerId = @ManagerId  
    ORDER BY dtAppliedDate DESC;  
END;  