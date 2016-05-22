using System.Collections.Generic;

namespace HomeOS.Hub.Tools.EnvironmentMonitor
{
    public class ValidationsResponse
    {
        public List<ValidationResponse> Validations;

        public ValidationsResponse()
        {
            Validations = new List<ValidationResponse>();
        }
    }
}