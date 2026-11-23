CREATE   PROCEDURE sp_GetEmployeeLeaveHistory  
    @EmployeeId INT  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    SELECT *  
    FROM tblLeaveRequests  
    WHERE itEmployeeId = @EmployeeId  
    ORDER BY dtAppliedDate DESC;  
END;  