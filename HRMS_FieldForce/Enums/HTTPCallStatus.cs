namespace HRMS_FieldForce.Enums
{
    public enum HTTPCallStatus
    {
        Success = 0,
        NotAuthenticated = 1,
        InvalidRequest = 2,
        Duplicate = 7,
        Error = 8,
        Overburden = 6,
        UserNotActive = 100,
        UserDisabled = 101,
        RoleDisabled = 102,
        DuplicateCondition = 200, 
        ErrorRequest = 201,      
        MandatoryFieldsRequired = 202,
        ValueError = 203
            
    }
}
