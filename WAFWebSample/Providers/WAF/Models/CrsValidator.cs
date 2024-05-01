using WAFWebSample.Data;
using WebSecurity.Interfaces;
using WebSecurity.Models;

namespace WAFWebSample.WebApi.Providers.WAF.Models
{
    public class CrsValidator : ICrsValidator
    {
        private readonly ILogger<CrsValidator> logger;
        private readonly IConfiguration configuration;
        private readonly IHostEnvironment environment;
        private readonly IServiceProvider serviceProvider;
        private ICrsRepository<IGlobal, IGlobal> globalRepository;
        private IHttpContextAccessor httpContextAccessor;

        public CrsValidator(IConfiguration configuration, IHostEnvironment environment, IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor, ICrsRepository<IGlobal, IGlobal> globalRepository, ILogger<CrsValidator> logger) 
        {
            this.logger = logger;
            this.configuration = configuration;
            this.environment = environment;
            this.serviceProvider = serviceProvider;
            this.globalRepository = globalRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public bool DetectSQLI(string stringValue)
        {
            throw new NotImplementedException();
        }

        public bool DetectSQLI(Dictionary<string, string> dictionaryValue)
        {
            throw new NotImplementedException();
        }

        public bool DetectSQLI(IEnumerable<string> enumerableValue)
        {
            throw new NotImplementedException();
        }

        public bool DetectXSS(Dictionary<string, string> dictionaryValue)
        {
            throw new NotImplementedException();
        }

        public bool DetectXSS(IEnumerable<string> enumerableValue)
        {
            throw new NotImplementedException();
        }

        public bool DetectXSS(string stringValue)
        {
            throw new NotImplementedException();
        }

        public bool ValidateByteRange(string stringValue, string byteRangeList)
        {
            throw new NotImplementedException();
        }

        public bool ValidateByteRange(IEnumerable<string> enumerableValue, int byteRangeStart, int byteRangeEnd)
        {
            throw new NotImplementedException();
        }

        public bool ValidateByteRange(Dictionary<string, string> dictionaryValue, string byteRangeList)
        {
            throw new NotImplementedException();
        }

        public bool ValidateByteRange(Dictionary<string, string> dictionaryValue, int byteRangeStart, int byteRangeEnd)
        {
            throw new NotImplementedException();
        }

        public bool ValidateByteRange(IEnumerable<string> enumerableValue, string byteRangeList)
        {
            throw new NotImplementedException();
        }

        public bool ValidateByteRange(string stringValue, int byteRangeStart, int byteRangeEnd)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUrlEncoding(string stringValue)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUtf8Encoding(string stringValue)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUtf8Encoding(Dictionary<string, string> dictionaryValue)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUtf8Encoding(IEnumerable<string> enumerableValue)
        {
            throw new NotImplementedException();
        }
    }
}
