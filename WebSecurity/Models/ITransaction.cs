using Castle.Components.DictionaryAdapter;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity.Models;

[OwaspName("Tx")]
public interface ITransaction : IStorageBase
{
    string? ConnectionId { get; set; }
    [OwaspName("inbound_anomaly_score_threshold")]
    int? InboundAnomalyScoreThreshold { get; set; }
    [OwaspName("922100_CHARSET")]
    int? _922100Charset { get; set; }
    [OwaspName("outbound_anomaly_score_threshold")]
    int? OutboundAnomalyScoreThreshold { get; set; }
    [OwaspName("reporting_level")]
    int? ReportingLevel { get; set; }
    [OwaspName("early_blocking")]
    bool? EarlyBlocking { get; set; }
    [OwaspName("blocking_paranoia_level")]
    int? BlockingParanoiaLevel { get; set; }
    [OwaspName("detection_paranoia_level")]
    int? DetectionParanoiaLevel { get; set; }
    [OwaspName("sampling_percentage")]
    int? SamplingPercentage { get; set; }
    [OwaspName("ARG_LENGTH")]
    int? ArgLength { get; set; }
    [OwaspName("ARG_NAME_LENGTH")]
    int? ArgNameLength { get; set; }
    [OwaspName("BLOCKING_ANOMALY_SCORE")]
    int? BlockingAnomalyScore { get; set; }
    [OwaspName("BLOCKING_INBOUND_ANOMALY_SCORE")]
    int? BlockingInboundAnomalyScore { get; set; }
    [OwaspName("BLOCKING_OUTBOUND_ANOMALY_SCORE")]
    int? BlockingOutboundAnomalyScore { get; set; }
    [OwaspName("COMBINED_FILE_SIZES")]
    int? CombinedFileSizes { get; set; }
    [OwaspName("content_type")]
    string? ContentType { get; set; }
    [OwaspName("content_type_charset")]
    string? ContentTypeCharset { get; set; }
    [OwaspName("critical_anomaly_score")]
    int? CriticalAnomalyScore { get; set; }
    [OwaspName("DETECTION_ANOMALY_SCORE")]
    int? DetectionAnomalyScore { get; set; }
    [OwaspName("DETECTION_INBOUND_ANOMALY_SCORE")]
    int? DetectionInboundAnomalyScore { get; set; }
    [OwaspName("DETECTION_OUTBOUND_ANOMALY_SCORE")]
    int? DetectionOutboundAnomalyScore { get; set; }
    [OwaspName("error_anomaly_score")]
    int? ErrorAnomalyScore { get; set; }
    [OwaspName("EXTENSION")]
    string? Extension { get; set; }
    [OwaspName("notice_anomaly_score")]
    int? NoticeAnomalyScore { get; set; }
    [OwaspName("sampling_rnd100")]
    int? SamplingRnd100 { get; set; }
    [OwaspName("warning_anomaly_score")]
    int? WarningAnomalyScore { get; set; }
    [OwaspName("allowed_methods")]
    ICollection<string>? AllowedMethods { get; set; }
    [OwaspName("allowed_request_content_type")]
    ICollection<string>? AllowedRequestContentType { get; set; }
    [OwaspName("allowed_request_content_type_charset")]
    ICollection<string>? AllowedRequestContentTypeCharset { get; set; }
    [OwaspName("allowed_http_versions")]
    ICollection<string>? AllowedHttpVersions { get; set; }
    [OwaspName("restricted_extensions")]
    ICollection<string>? RestrictedExtensions { get; set; }
    [OwaspName("restricted_headers_basic")]
    ICollection<string>? RestrictedHeadersBasic { get; set; }
    [OwaspName("restricted_headers_extended")]
    ICollection<string>? RestrictedHeadersExtended { get; set; }
    [OwaspName("enforce_bodyproc_urlencoded")]
    bool? EnforceBodyprocUrlencoded { get; set; }
    [OwaspName("crs_validate_utf8_encoding")]
    bool? CrsValidateUtf8Encoding { get; set; }
    [OwaspName("inbound_anomaly_score_pl1")]
    int? InboundAnomalyScorePl1 { get; set; }
    [OwaspName("inbound_anomaly_score_pl2")]
    int? InboundAnomalyScorePl2 { get; set; }
    [OwaspName("inbound_anomaly_score_pl3")]
    int? InboundAnomalyScorePl3 { get; set; }
    [OwaspName("inbound_anomaly_score_pl4")]
    int? InboundAnomalyScorePl4 { get; set; }
    [OwaspName("sql_injection_score")]
    int? SqlInjectionScore { get; set; }
    [OwaspName("xss_score")]
    int? XssScore { get; set; }
    [OwaspName("rfi_score")]
    int? RfiScore { get; set; }
    [OwaspName("lfi_score")]
    int? LfiScore { get; set; }
    [OwaspName("rce_score")]
    int? RceScore { get; set; }
    [OwaspName("php_injection_score")]
    int? PhpInjectionScore { get; set; }
    [OwaspName("http_violation_score")]
    int? HttpViolationScore { get; set; }
    [OwaspName("session_fixation_score")]
    int? SessionFixationScore { get; set; }
    [OwaspName("outbound_anomaly_score_pl1")]
    int? OutboundAnomalyScorePl1 { get; set; }
    [OwaspName("outbound_anomaly_score_pl2")]
    int? OutboundAnomalyScorePl2 { get; set; }
    [OwaspName("outbound_anomaly_score_pl3")]
    int? OutboundAnomalyScorePl3 { get; set; }
    [OwaspName("outbound_anomaly_score_pl4")]
    int? OutboundAnomalyScorePl4 { get; set; }
    [OwaspName("anomaly_score")]
    int? AnomalyScore { get; set; }
    [OwaspName("932260_matched_var_name")]
    string? _932260MatchedVarName { get; set; }
    [OwaspName("932200_matched_var_name")]
    string? _932200MatchedVarName { get; set; }
    [OwaspName("932205_matched_var_name")]
    string? _932205MatchedVarName { get; set; }
    [OwaspName("932206_matched_var_name")]
    string? _932206MatchedVarName { get; set; }
    [OwaspName("932240_matched_var_name")]
    string? _932240MatchedVarName { get; set; }
    [OwaspName("933120_tx_0")]
    string? _933120Tx0 { get; set; }
    [OwaspName("933151_tx_0")]
    string? _933151Tx0 { get; set; }
    [OwaspName("942130_matched_var_name")]
    string? _942130MatchedVarName { get; set; }
    [OwaspName("942131_matched_var_name")]
    string? _942131MatchedVarName { get; set; }
    [OwaspName("942521_matched_var_name")]
    string? _942521MatchedVarName { get; set; }
    [OwaspName("943110_matched_var_name")]
    string? _943110MatchedVarName { get; set; }
    [OwaspName("943120_matched_var_name")]
    string? _943120MatchedVarName { get; set; }
}
