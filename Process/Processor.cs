using CouponUnblockQualifier.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CouponUnblockQualifier.Process
{
    public class Processor
    {
        private IConfiguration _configuration { get; set; }
        private readonly ILogger<Processor> _logger;
        private readonly IDBHelper _IDBHelper;
        public Processor(IConfiguration configuration, ILogger<Processor> logger, IDBHelper IDBHelper)
        {
            _IDBHelper = IDBHelper;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Process() 
        {
            
            _logger.LogInformation("Service Started.");
            var isStopProcess = _configuration["IsStopProcess"];            
            _logger.LogInformation("IsStopProcess {0} ", isStopProcess);
        }        
    }
}
