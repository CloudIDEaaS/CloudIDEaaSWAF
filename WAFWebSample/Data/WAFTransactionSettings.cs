using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSecurity.Models;

namespace WAFWebSample.Data
{
    public class WAFTransactionSettings
    {
        [Key]
        public Guid WAFTransactionSettingsId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? ConnectionId { get; set; }
        public int? DetectionParanoiaLevel { get; set; }
        public int? _922100Charset { get; set; }
        public int? ArgLength { get; set; }
        public int? ArgNameLength { get; set; }
        public int? BlockingAnomalyScore { get; set; }
        public int? BlockingInboundAnomalyScore { get; set; }
        public int? BlockingOutboundAnomalyScore { get; set; }
        public int? CombinedFileSizes { get; set; }
        public string? ContentType { get; set; }
        public string? ContentTypeCharset { get; set; }
        public int? CriticalAnomalyScore { get; set; }
        public int? DetectionAnomalyScore { get; set; }
        public int? DetectionInboundAnomalyScore { get; set; }
        public int? DetectionOutboundAnomalyScore { get; set; }
        public int? ErrorAnomalyScore { get; set; }
        public string? Extension { get; set; }
        public int? NoticeAnomalyScore { get; set; }
        public int? SamplingRnd100 { get; set; }
        public int? WarningAnomalyScore { get; set; }
        public int? InboundAnomalyScoreThreshold { get; set; }
        public int? OutboundAnomalyScoreThreshold { get; set; }
        public int? BlockingParanoiaLevel { get; set; }
        public bool? EarlyBlocking { get; set; }
        public int? ReportingLevel { get; set; }
        public int? SamplingPercentage { get; set; }
        public string? AllowedMethods { get; set; }
        public string? AllowedRequestContentType { get; set; }
        public string? AllowedRequestContentTypeCharset { get; set; }
        public string? AllowedHttpVersions { get; set; }
        public string? RestrictedExtensions { get; set; }
        public string? RestrictedHeadersBasic { get; set; }
        public string? RestrictedHeadersExtended { get; set; }
        public bool? EnforceBodyprocUrlencoded { get; set; }
        public bool? CrsValidateUtf8Encoding { get; set; }
        public int? InboundAnomalyScorePl1 { get; set; }
        public int? InboundAnomalyScorePl2 { get; set; }
        public int? InboundAnomalyScorePl3 { get; set; }
        public int? InboundAnomalyScorePl4 { get; set; }
        public int? SqlInjectionScore { get; set; }
        public int? XssScore { get; set; }
        public int? RfiScore { get; set; }
        public int? LfiScore { get; set; }
        public int? RceScore { get; set; }
        public int? PhpInjectionScore { get; set; }
        public int? HttpViolationScore { get; set; }
        public int? SessionFixationScore { get; set; }
        public int? OutboundAnomalyScorePl1 { get; set; }
        public int? OutboundAnomalyScorePl2 { get; set; }
        public int? OutboundAnomalyScorePl3 { get; set; }
        public int? OutboundAnomalyScorePl4 { get; set; }
        public int? AnomalyScore { get; set; }
        public string? _932260MatchedVarName { get; set; }
        public string? _932200MatchedVarName { get; set; }
        public string? _932205MatchedVarName { get; set; }
        public string? _932206MatchedVarName { get; set; }
        public string? _932240MatchedVarName { get; set; }
        public string? _933120Tx0 { get; set; }
        public string? _933151Tx0 { get; set; }
        public string? _942130MatchedVarName { get; set; }
        public string? _942131MatchedVarName { get; set; }
        public string? _942521MatchedVarName { get; set; }
        public string? _943110MatchedVarName { get; set; }
        public string? _943120MatchedVarName { get; set; }
    }
}
