CREATE   PROCEDURE sp_ApplyLeave  
    @EmployeeId INT,  
    @StartDate DATE,  
    @EndDate DATE,  
    @Reason NVARCHAR(MAX),  
    @ManagerId INT  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    IF @EndDate < @StartDate  
        THROW 50001, 'End date cannot be earlier than start date.', 1;  
  
    IF DATEDIFF(DAY, @StartDate, @EndDate) + 1 > 30  
        THROW 50002, 'Maximum leave duration is 30 days.', 1;  
  
    IF NOT EXISTS (  
        SELECT 1  
        FROM tblEmployees  
        WHERE inEmployeeId = @EmployeeId  
          AND stRole = 'Employee'  
          AND inManagerId = @ManagerId  
    )  
        THROW 50003, 'Invalid employee or manager.', 1;  
  
    IF EXISTS (  
        SELECT 1  
        FROM tblLeaveRequests  
        WHERE itEmployeeId = @EmployeeId  
          AND stStatus IN ('Pending', 'Approved')  
          AND @StartDate <= dtEndDate  
          AND @EndDate >= dtStartDate  
    )  
        THROW 50004, 'Overlapping leave request already exists.', 1;  
  
    INSERT INTO tblLeaveRequests  
    (  
        itEmployeeId,  
        dtStartDate,  
        dtEndDate,  
        stReason,  
        stStatus,  
        dtAppliedDate,  
        inManagerId,  
        dtDecisionDate,  
        stManagerComment  
    )  
    VALUES  
    (  
        @EmployeeId,  
        @StartDate,  
        @EndDate,  
        @Reason,  
        'Pending',  
        GETDATE(),  
        @ManagerId,  
        NULL,  
        NULL  
    );  
END;  