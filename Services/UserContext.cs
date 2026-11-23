using ELMS.Common.Constants;

namespace ELMS.Services
{
    public class UserContext
    {
        private readonly IHttpContextAccessor _http;

        public UserContext(IHttpContextAccessor http) 
        {
            _http = http;
        }

        public int miEmployeeId =>
            _http.HttpContext?.Session.GetInt32(SessionKeys.lsEmployeeId) ?? 0;

        public string msRole =>
            _http.HttpContext?.Session.GetString(SessionKeys.lsRole) ?? string.Empty;

        public bool mbManager =>
            _http.HttpContext?.Session.GetString(SessionKeys.lsManager) == "true";
    }
}
