CREATE   PROCEDURE sp_UpdateLeaveStatus  
    @LeaveRequestId INT,  
    @Status NVARCHAR(50),  
    @ManagerComment NVARCHAR(MAX)  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    IF @Status NOT IN ('Approved', 'Rejected')  
        THROW 50005, 'Invalid leave status.', 1;  
  
    UPDATE tblLeaveRequests  
    SET  
        stStatus = @Status,  
        stManagerComment = @ManagerComment,  
        dtDecisionDate = GETDATE()  
    WHERE itLeaveRequestId = @LeaveRequestId  
      AND stStatus = 'Pending';  
  
    IF @@ROWCOUNT = 0  
        THROW 50006, 'Leave request not found or already processed.', 1;  
END;  