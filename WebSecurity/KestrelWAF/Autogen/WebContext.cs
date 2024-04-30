using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using Utils;
using Utils.ProcessHelpers;
using WebSecurity.Interfaces;
using WebSecurity.Models;

namespace WebSecurity.KestrelWAF;

public partial class WebContext
{
	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\REQUEST-901-INITIALIZATION.conf **************************************************

	// Rule: ************************* 901001 *************************

	public bool HasCrsSetupVersionCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
			var settings = repository.GetSettings();
			var version = (string?) settings["CrsSetupVersion"];
			var versionRule = rules.FirstOrDefault();

			if (version == null || versionRule == null)
			{
				return false;
			}

            return version == versionRule.Version;
		}
	}

	public CallbackResult CrsSetupVersionCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
            }
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901100 *************************

	public bool HasInboundAnomalyScoreThresholdCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["InboundAnomalyScoreThreshold"].IsTruthyWithCount();
		}
	}

	public CallbackResult InboundAnomalyScoreThresholdCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
                failed = false;
				skipAfter = result.SkipAfter;
            }
            else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901110 *************************

	public bool HasOutboundAnomalyScoreThresholdCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["OutboundAnomalyScoreThreshold"].IsTruthyWithCount();
		}
	}

	public CallbackResult OutboundAnomalyScoreThresholdCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901111 *************************

	public bool HasReportingLevelCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["ReportingLevel"].IsTruthyWithCount();
		}
	}

	public CallbackResult ReportingLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901115 *************************

	public bool HasEarlyBlockingCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["EarlyBlocking"].IsTruthyWithCount();
		}
	}

	public CallbackResult EarlyBlockingCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901120 *************************

	public bool HasBlockingParanoiaLevelCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["BlockingParanoiaLevel"].IsTruthyWithCount();
		}
	}

	public CallbackResult BlockingParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901125 *************************

	public bool HasDetectionParanoiaLevelCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["DetectionParanoiaLevel"].IsTruthyWithCount();
		}
	}

	public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901130 *************************

	public bool HasSamplingPercentageCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["SamplingPercentage"].IsTruthyWithCount();
		}
	}

	public CallbackResult SamplingPercentageCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901140 *************************

	public bool HasCriticalAnomalyScoreCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["CriticalAnomalyScore"].IsTruthyWithCount();
		}
	}

	public CallbackResult CriticalAnomalyScoreCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901141 *************************

	public bool HasErrorAnomalyScoreCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["ErrorAnomalyScore"].IsTruthyWithCount();
		}
	}

	public CallbackResult ErrorAnomalyScoreCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901142 *************************

	public bool HasWarningAnomalyScoreCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["WarningAnomalyScore"].IsTruthyWithCount();
		}
	}

	public CallbackResult WarningAnomalyScoreCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901143 *************************

	public bool HasNoticeAnomalyScoreCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["NoticeAnomalyScore"].IsTruthyWithCount();
		}
	}

	public CallbackResult NoticeAnomalyScoreCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901160 *************************

	public bool HasAllowedMethodsCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["AllowedMethods"].IsTruthyWithCount();
		}
	}

	public CallbackResult AllowedMethodsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901162 *************************

	public bool HasAllowedRequestContentTypeCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["AllowedRequestContentType"].IsTruthyWithCount();
		}
	}

	public CallbackResult AllowedRequestContentTypeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901168 *************************

	public bool HasAllowedRequestContentTypeCharsetCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["AllowedRequestContentTypeCharset"].IsTruthyWithCount();
		}
	}

	public CallbackResult AllowedRequestContentTypeCharsetCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901163 *************************

	public bool HasAllowedHttpVersionsCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["AllowedHttpVersions"].IsTruthyWithCount();
		}
	}

	public CallbackResult AllowedHttpVersionsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901164 *************************

	public bool HasRestrictedExtensionsCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["RestrictedExtensions"].IsTruthyWithCount();
		}
	}

	public CallbackResult RestrictedExtensionsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901165 *************************

	public bool HasRestrictedHeadersBasicCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["RestrictedHeadersBasic"].IsTruthyWithCount();
		}
	}

	public CallbackResult RestrictedHeadersBasicCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901171 *************************

	public bool HasRestrictedHeadersExtendedCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["RestrictedHeadersExtended"].IsTruthyWithCount();
		}
	}

	public CallbackResult RestrictedHeadersExtendedCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901167 *************************

	public bool HasEnforceBodyprocUrlencodedCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["EnforceBodyprocUrlencoded"].IsTruthyWithCount();
		}
	}

	public CallbackResult EnforceBodyprocUrlencodedCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901169 *************************

	public bool HasCrsValidateUtf8EncodingCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["CrsValidateUtf8Encoding"].IsTruthyWithCount();
		}
	}

	public CallbackResult CrsValidateUtf8EncodingCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901320 *************************

	public bool? EnableDefaultCollections()
	{
		var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
		var settings = repository.GetSettings();

		return (bool?) settings["EnableDefaultCollections"];
	}

	public CallbackResult EnableDefaultCollectionsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 100000 *************************

	public string? UserAgent
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).GetDefault("User-Agent", string.Empty);
		}
	}

	public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901340 *************************

	public string? ReqBodyProcessor
	{
		get
		{
			var bodyProcessor = this.HttpContext.GetRequestBodyProcessor();

			this.HttpContext.AddMatchedVarName(nameof(ReqBodyProcessor));
			this.HttpContext.AddMatchedVar(nameof(ReqBodyProcessor), bodyProcessor);

			return bodyProcessor;
		}
	}

	public CallbackResult ReqBodyProcessorCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901350 *************************

	public bool? EnforceBodyprocUrlencoded()
	{
        var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
		var settings = repository.GetSettings();

		return (bool?) settings["EnforceBodyprocUrlencoded"];
	}


	// Members declared elsewhere:

	// public CallbackResult EnforceBodyprocUrlencodedCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100001 *************************


	// Members declared elsewhere:

	// public string ReqBodyProcessor { get; }
	// public CallbackResult ReqBodyProcessorCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 901400 *************************

	public int? SamplingPercentage()
	{
		var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
		var settings = repository.GetSettings();

		return (int?) settings["SamplingPercentage"];
	}


	// Members declared elsewhere:

	// public CallbackResult SamplingPercentageCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 901410 *************************

	public string UniqueId
	{
		get
		{
			var transformations = this.Transformations;
			var value = (object) this.HttpContext.GetUniqueId();

			foreach (var transformation in transformations)
			{
				value = transformation(value);
			}

			return (string) value;
		}
	}

	public CallbackResult UniqueIdCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 901450 *************************

	public T? GetTransactionVariable<T>(string variable)
	{
        var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
        var settings = repository.GetSettings();

        return (T?)settings[variable];
    }

    public CallbackResult SamplingRnd100Callback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public int SamplingPercentage { get; }

	// Rule: ************************* 901500 *************************

	public int? GetBlockingParanoiaLevel()
	{
		var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
		var settings = repository.GetSettings();

		return (int?) settings["BlockingParanoiaLevel"];
	}


    // Members declared elsewhere:

    // public T? GetTransactionVariable<T>(string variable)
    // public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\REQUEST-905-COMMON-EXCEPTIONS.conf **************************************************

    // Rule: ************************* 905100 *************************

    [OwaspName("request_line")]
    public string RequestLine
	{
		get
		{
			var line = request.ToHttpString()!;

			this.HttpContext.AddMatchedVarName(nameof(RequestLine));
            this.HttpContext.AddMatchedVar(nameof(RequestLine), line);

            return line;
		}
	}

    [OwaspName("response_line")]
    public string ResponseLine
    {
        get
        {
            var line = response.ToHttpString()!;

            this.HttpContext.AddMatchedVarName(nameof(ResponseLine));
            this.HttpContext.AddMatchedVar(nameof(ResponseLine), line);

            return line;
        }
    }

    public CallbackResult RequestLineCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

    // Rule: ************************* 100002 *************************

    [OwaspName("remote_addr")]
    public string RemoteAddr
	{
		get
		{
			return this.HttpContext.GetRemoteAddress();
		}
	}

	public CallbackResult RemoteAddressCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 905110 *************************


	// Members declared elsewhere:

	// public string RemoteAddr { get; }
	// public CallbackResult RemoteAddressCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100003 *************************


	// Members declared elsewhere:

	// public string? UserAgent { get; }
	// public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100004 *************************


	// Members declared elsewhere:

	// public string RequestLine { get; }
	// public CallbackResult RequestLineCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\REQUEST-911-METHOD-ENFORCEMENT.conf **************************************************

	// Rule: ************************* 911011 *************************

	public int? DetectionParanoiaLevel()
	{
		var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
		var settings = repository.GetSettings();

		return (int?) settings["DetectionParanoiaLevel"];
	}


	// Members declared elsewhere:

	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 911012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 911100 *************************

	public ICollection<string>? GetAllowedMethods()
	{
		var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
		var settings = repository.GetSettings();

		return (ICollection<string>?) settings["AllowedMethods"];
	}

	public CallbackResult RequestMethodCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 911013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 911014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 911015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 911016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 911017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 911018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\REQUEST-913-SCANNER-DETECTION.conf **************************************************

	// Rule: ************************* 913011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 913012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 913100 *************************


	// Members declared elsewhere:

	// public string? UserAgent { get; }
	// public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 913013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 913014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 913015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 913016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 913017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 913018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\REQUEST-920-PROTOCOL-ENFORCEMENT.conf **************************************************

	// Rule: ************************* 920011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920100 *************************


	// Members declared elsewhere:

	// public string RequestLine { get; }
	// public CallbackResult RequestLineCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920120 *************************

	public IEnumerable<string> Files
	{
		get
		{
			var files = this.HttpContext.GetFiles();

			this.HttpContext.AddMatchedVars(files.ToDictionary(f => nameof(Files), f => (string?) f));

            return files;
		}
	}

	public CallbackResult FilesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	public IEnumerable<string> FilesNames
	{
		get
		{
			var fileNames = this.HttpContext.GetFilesNames();
			var lastFile = fileNames.LastOrDefault();

            this.HttpContext.AddMatchedVar(lastFile, lastFile);
            this.HttpContext.AddMatchedVarName(lastFile);
            this.HttpContext.AddMatchedVars(fileNames.ToDictionary(f => f, f => (string?)f));

            return fileNames;
		}
	}

	public CallbackResult FilesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 920160 *************************

	public CallbackResult ContentLengthCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public string? ContentLength { get; }

	// Rule: ************************* 920170 *************************

	public string RequestMethod
	{
		get
		{
			var method = request.Method;

			this.HttpContext.AddMatchedVar(nameof(RequestMethod), method);

			return method;
		}
	}


	// Members declared elsewhere:

	// public CallbackResult RequestMethodCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100005 *************************


	// Members declared elsewhere:

	// public string? ContentLength { get; }
	// public CallbackResult ContentLengthCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920171 *************************


	// Members declared elsewhere:

	// public string RequestMethod { get; }
	// public CallbackResult RequestMethodCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100006 *************************

	public bool HasRequestHeadersTransferEncoding
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).ContainsKey("Transfer-Encoding");
		}
	}

	public CallbackResult TransferEncodingCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 920180 *************************

	public string RequestProtocol
	{
		get
		{
			var protocol = request.Protocol;

			this.HttpContext.AddMatchedVar("RequestProtocol", protocol);

			return protocol;
		}
	}

	public CallbackResult RequestProtocolCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 100007 *************************


	// Members declared elsewhere:

	// public string RequestMethod { get; }
	// public CallbackResult RequestMethodCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100008 *************************

	public bool HasRequestHeadersContentLength
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).ContainsKey("Content-Length");
		}
	}


	// Members declared elsewhere:

	// public CallbackResult ContentLengthCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100009 *************************


	// Members declared elsewhere:

	// public bool HasRequestHeadersTransferEncoding { get; }
	// public CallbackResult TransferEncodingCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920181 *************************


	// Members declared elsewhere:

	// public bool HasRequestHeadersTransferEncoding { get; }
	// public CallbackResult TransferEncodingCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100010 *************************


	// Members declared elsewhere:

	// public bool HasRequestHeadersContentLength { get; }
	// public CallbackResult ContentLengthCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920190 *************************

	public string? Range
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).GetDefault("Range", string.Empty);
		}
	}

	public CallbackResult RangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	public string? RequestRange
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).GetDefault("Request-Range", string.Empty);
		}
	}

	public CallbackResult RequestRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 100011 *************************

	public string? GetLastCapturedGroup(int index)
	{
		return this.HttpContext.GetLastCapturedGroup(index);
	}

	public CallbackResult TransactionMatchCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 920210 *************************

	public string? Connection
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).GetDefault("Connection", string.Empty);
		}
	}

	public CallbackResult ConnectionCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

    // Rule: ************************* 920220 *************************

    [OwaspName("request_uri_raw")]
    public string RequestUriRaw
	{
		get
		{
			var uri = request.GetRawUri();

            this.HttpContext.AddMatchedVarName(nameof(RequestUriRaw));
            this.HttpContext.AddMatchedVar(nameof(RequestUriRaw), uri);

            return uri;
		}
	}

	public CallbackResult RequestUriRawCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 100012 *************************


	// Members declared elsewhere:

	// public string RequestUriRaw { get; }
	// public CallbackResult RequestUriRawCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100013 *************************

	public CallbackResult ValidateUrlEncodingCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


    // Members declared elsewhere:

    // public string GetLastCapturedGroup(int index)


    // Members declared elsewhere:

    // public string GetLastCapturedGroup(int index)
    // public CallbackResult ValidateUrlEncodingCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 920221 *************************

    [OwaspName("request_basename")]
    public string RequestBasename
	{
		get
		{
			var name = request.GetUriBaseName();

            this.HttpContext.AddMatchedVarName(nameof(RequestBasename));
            this.HttpContext.AddMatchedVar(nameof(RequestBasename), name);

			return name;
        }
    }

	public CallbackResult RequestBasenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 100014 *************************


	// Members declared elsewhere:

	// public string GetLastCapturedGroup(int index)
	// public CallbackResult ValidateUrlEncodingCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920250 *************************

	public bool? CrsValidateUtf8Encoding()
	{
		var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
		var settings = repository.GetSettings();

		return (bool?) settings["CrsValidateUtf8Encoding"];
	}


	// Members declared elsewhere:

	// public CallbackResult CrsValidateUtf8EncodingCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100015 *************************

	public string RequestFilename
	{
		get
		{
			var name = request.GetUriFileName();

			this.HttpContext.AddMatchedVar(nameof(RequestFilename), name);

			return name;
		}
	}

	public bool ValidateUtf8Encoding(string stringValue)
	{
		var validator = serviceProvider.GetService<ICrsValidator>();

		return validator.ValidateUtf8Encoding(stringValue);
	}

	public CallbackResult ValidateUtf8EncodingCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	public bool ValidateUtf8Encoding(Dictionary<string, string> dictionaryValue)
	{
		var validator = serviceProvider.GetService<ICrsValidator>();

		return validator.ValidateUtf8Encoding(dictionaryValue);
	}

	// Members declared elsewhere:

	// public CallbackResult ValidateUtf8EncodingCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	public IEnumerable<string> ArgsNames
	{
		get
		{
			var argNames = this.HttpContext.GetRequestArgsNames();

            this.HttpContext.AddMatchedVarNames(argNames);
            this.HttpContext.AddMatchedVarName(argNames.Last());

            return argNames;
		}
	}

	public bool ValidateUtf8Encoding(IEnumerable<string> enumerableValue)
	{
        var validator = serviceProvider.GetService<ICrsValidator>();

        return validator.ValidateUtf8Encoding(enumerableValue);
    }


    // Members declared elsewhere:

    // public CallbackResult ValidateUtf8EncodingCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 920260 *************************

    public string RequestUri
	{
		get
		{
			var uri = request.GetUri();

			this.HttpContext.AddMatchedVar(nameof(RequestUri), uri);

			return uri;
		}
	}

	public CallbackResult RequestUriCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	public string RequestBody
	{
		get
		{
			var body = request.GetBody();

            this.HttpContext.AddMatchedVarName(nameof(RequestBody));
            this.HttpContext.AddMatchedVar(nameof(RequestBody), body);

            return body;
		}
	}

	public CallbackResult RequestBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 920270 *************************

	public bool ValidateByteRange(string stringValue, int byteRangeStart, int byteRangeEnd)
	{
		var validator = serviceProvider.GetService<ICrsValidator>();

		return validator.ValidateByteRange(stringValue, byteRangeStart, byteRangeEnd);
	}

	public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public string RequestUri { get; }

	public Dictionary<string, string?> RequestHeaders
	{
		get
		{
			var headers = request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value)!;

			this.HttpContext.AddMatchedVars(headers);

			return headers;
		}
	}

	public bool ValidateByteRange(Dictionary<string, string> dictionaryValue, int byteRangeStart, int byteRangeEnd)
	{
		var validator = serviceProvider.GetService<ICrsValidator>();

		return validator.ValidateByteRange(dictionaryValue, byteRangeStart, byteRangeEnd);
	}


	// Members declared elsewhere:

	// public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public bool ValidateByteRange(Dictionary<string, string> dictionaryValue, int byteRangeStart, int byteRangeEnd)
	// public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	public bool ValidateByteRange(IEnumerable<string> enumerableValue, int byteRangeStart, int byteRangeEnd)
	{
		var validator = serviceProvider.GetService<ICrsValidator>();

		return validator.ValidateByteRange(enumerableValue, byteRangeStart, byteRangeEnd);
	}


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920280 *************************

	public bool HasRequestHeadersHost
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).ContainsKey("Host");
		}
	}

	public CallbackResult HostCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 920290 *************************

	public string? Host
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).GetDefault("Host", string.Empty);
		}
	}


	// Members declared elsewhere:

	// public CallbackResult HostCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920310 *************************

	public string? Accept
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).GetDefault("Accept", string.Empty);
		}
	}

	public CallbackResult AcceptCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 100016 *************************


	// Members declared elsewhere:

	// public string RequestMethod { get; }
	// public CallbackResult RequestMethodCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100017 *************************


	// Members declared elsewhere:

	// public string? UserAgent { get; }
	// public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920311 *************************


	// Members declared elsewhere:

	// public string? Accept { get; }
	// public CallbackResult AcceptCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100018 *************************


	// Members declared elsewhere:

	// public string RequestMethod { get; }
	// public CallbackResult RequestMethodCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100019 *************************

	public bool HasRequestHeadersUserAgent
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).ContainsKey("User-Agent");
		}
	}


	// Members declared elsewhere:

	// public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920330 *************************


	// Members declared elsewhere:

	// public string? UserAgent { get; }
	// public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920340 *************************


	// Members declared elsewhere:

	// public string? ContentLength { get; }
	// public CallbackResult ContentLengthCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100020 *************************

	public bool HasRequestHeadersContentType
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).ContainsKey("Content-Type");
		}
	}

	public CallbackResult ContentTypeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 920350 *************************


	// Members declared elsewhere:

	// public string? Host { get; }
	// public CallbackResult HostCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920380 *************************

	public bool HasMaxNumArgsCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
			var settings = repository.GetSettings();

			return settings["MaxNumArgs"].IsTruthyWithCount();
		}
	}

	public CallbackResult MaxNumArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Rule: ************************* 100023 *************************

	[OwaspName("args")]
    public Dictionary<string, string?> Args
    {
        get
        {
            var args = this.HttpContext.GetRequestArgs()!;

            this.HttpContext.AddMatchedVars(args);
            this.HttpContext.AddMatchedVar(args.Last());

			return args;
        }
    }

    public int ArgsLength
    {
        get
        {
			return this.HttpContext.GetRequestArgs().Sum(p => p.Key.Length + p.Value.Length);
        }
    }

    // Rule: ************************* 100021 *************************


    public int ArgsCount
	{
		get
		{
			return this.HttpContext.GetRequestArgs().Count;
		}
	}

	public int? GetMaxNumArgs()
	{
		var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
		var settings = repository.GetSettings();

		return (int?) settings["max_num_args"];
	}

	public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 920360 *************************

	public bool HasArgNameLengthCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["ArgNameLength"].IsTruthyWithCount();
		}
	}

	public CallbackResult ArgNameLengthCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 100022 *************************

	public int ArgsNamesCount
	{
		get
		{
			return this.HttpContext.GetRequestArgsNames().Count;
		}
	}

	public int? GetArgNameLength()
	{
		var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
		var settings = repository.GetSettings();

		return (int?) settings["arg_name_length"];
	}

	public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 920370 *************************

	public bool HasArgLengthCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["ArgLength"].IsTruthyWithCount();
		}
	}

	public CallbackResult ArgLengthCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public string GetArgLength { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920390 *************************

	public bool HasTotalArgLengthCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
			var settings = repository.GetSettings();

			return settings["TotalArgLength"].IsTruthyWithCount();
		}
	}

	public CallbackResult TotalArgLengthCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 100024 *************************

	public int ArgsCombinedSize
	{
		get
		{
			return this.HttpContext.GetRequestArgsCombinedSize();
		}
	}

	public int? GetTotalArgLength()
	{
		var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
		var settings = repository.GetSettings();

		return (int?) settings["TotalArgLength"];
	}

	public CallbackResult ArgsCombinedSizeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 920400 *************************

	public bool HasMaxFileSizeCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
			var settings = repository.GetSettings();

			return settings["MaxFileSize"].IsTruthyWithCount();
		}
	}

	public CallbackResult MaxFileSizeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 100025 *************************

	public string? ContentType
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).GetDefault("Content-Type", string.Empty);
		}
	}

	// Members declared elsewhere:

	// public CallbackResult ContentTypeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100026 *************************

	public long? GetMaxFileSize()
	{
		var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
		var settings = repository.GetSettings();

		return (long?) settings["MaxFileSize"];
	}


	// Members declared elsewhere:

	// public string ContentLength { get; }
	// public CallbackResult ContentLengthCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920410 *************************

	public bool HasCombinedFileSizesCount
	{
		get
		{
			var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
			var settings = repository.GetSettings();

			return settings["CombinedFileSizes"].IsTruthyWithCount();
		}
	}

	public CallbackResult CombinedFileSizesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	public CallbackResult FilesCombinedSizeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public int FilesCombinedSize { get; }

	// Rule: ************************* 920470 *************************


	// Members declared elsewhere:

	// public string? ContentType { get; }
	// public CallbackResult ContentTypeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920420 *************************


	// Members declared elsewhere:

	// public string? ContentType { get; }
	// public CallbackResult ContentTypeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100028 *************************

	public ICollection<string>? GetAllowedRequestContentTypes()
	{
		var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
		var settings = repository.GetSettings();

		return (ICollection<string>?) settings["AllowedRequestContentTypes"];
	}


	// Members declared elsewhere:

	// public T? GetTransactionVariable<T>(string variable)
	// public CallbackResult ContentTypeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920480 *************************


	// Members declared elsewhere:

	// public string? ContentType { get; }
	// public CallbackResult ContentTypeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100029 *************************

	public ICollection<string>? GetAllowedRequestContentTypeCharsets()
	{
		var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
		var settings = repository.GetSettings();

		return (ICollection<string>?) settings["AllowedRequestContentTypeCharsets"];
	}

	public CallbackResult ContentTypeCharsetCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public T? GetTransactionVariable<T>(string variable)

	// Rule: ************************* 920530 *************************


	// Members declared elsewhere:

	// public string? ContentType { get; }
	// public CallbackResult ContentTypeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920430 *************************

	public ICollection<string>? GetAllowedHttpVersions()
	{
		var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
		var settings = repository.GetSettings();

		return (ICollection<string>?) settings["AllowedHttpVersions"];
	}


	// Members declared elsewhere:

	// public CallbackResult RequestProtocolCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920440 *************************


	// Members declared elsewhere:

	// public string RequestBasename { get; }
	// public CallbackResult RequestBasenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100030 *************************

	public ICollection<string>? GetRestrictedExtensions()
	{
		var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
		var settings = repository.GetSettings();

		return (ICollection<string>?) settings["RestrictedExtensions"];
	}

	public CallbackResult ExtensionCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public T? GetTransactionVariable<T>(string variable)

	// Rule: ************************* 920500 *************************

	public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public string RequestFilename { get; }

	// Rule: ************************* 920450 *************************

	public IEnumerable<string> RequestHeadersNames
	{
		get
		{
			var names = request.Headers.Keys;

            this.HttpContext.AddMatchedVarNames(names.ToList());
            this.HttpContext.AddMatchedVarName(names.Last());

            return names;
		}
	}

	public CallbackResult RequestHeadersNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 100031 *************************

	public ICollection<string>? GetRestrictedHeadersBasics()
	{
		var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
		var settings = repository.GetSettings();

		return (ICollection<string>?) settings["RestrictedHeadersBasic"];
	}

	public CallbackResult HeaderName920450Callback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public T? GetTransactionVariable<T>(string variable)

	// Rule: ************************* 920520 *************************

	public string? AcceptEncoding
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).GetDefault("Accept-Encoding", string.Empty);
		}
	}

	public CallbackResult AcceptEncodingCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 920600 *************************


	// Members declared elsewhere:

	// public string? Accept { get; }
	// public CallbackResult AcceptCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920540 *************************


	// Members declared elsewhere:

	// public string ReqBodyProcessor { get; }
	// public CallbackResult ReqBodyProcessorCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100032 *************************


	// Members declared elsewhere:

	// public string RequestUri { get; }
	// public CallbackResult RequestUriCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920610 *************************


	// Members declared elsewhere:

	// public string RequestUriRaw { get; }
	// public CallbackResult RequestUriRawCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920620 *************************


	// Members declared elsewhere:

	// public bool HasRequestHeadersContentType { get; }
	// public CallbackResult ContentTypeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920200 *************************


	// Members declared elsewhere:

	// public string? Range { get; }
	// public CallbackResult RangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? RequestRange { get; }
	// public CallbackResult RequestRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100033 *************************


	// Members declared elsewhere:

	// public string RequestBasename { get; }
	// public CallbackResult RequestBasenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920201 *************************


	// Members declared elsewhere:

	// public string RequestBasename { get; }
	// public CallbackResult RequestBasenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100034 *************************


	// Members declared elsewhere:

	// public string? Range { get; }
	// public CallbackResult RangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? RequestRange { get; }
	// public CallbackResult RequestRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920230 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920271 *************************

	public bool ValidateByteRange(string stringValue, string byteRangeList)
	{
		var validator = serviceProvider.GetService<ICrsValidator>();

		return validator.ValidateByteRange(stringValue, byteRangeList);
	}


	// Members declared elsewhere:

	// public string RequestUri { get; }
	// public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	public bool ValidateByteRange(Dictionary<string, string> dictionaryValue, string byteRangeList)
	{
		var validator = serviceProvider.GetService<ICrsValidator>();

		return validator.ValidateByteRange(dictionaryValue, byteRangeList);
	}


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public bool ValidateByteRange(Dictionary<string, string> dictionaryValue, string byteRangeList)
	// public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	public bool ValidateByteRange(IEnumerable<string> enumerableValue, string byteRangeList)
	{
		var validator = serviceProvider.GetService<ICrsValidator>();

		return validator.ValidateByteRange(enumerableValue, byteRangeList);
	}


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 920320 *************************


    // Members declared elsewhere:

    // public bool HasRequestHeadersUserAgent { get; }
    // public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 920121 *************************


    // Members declared elsewhere:

    // public IEnumerable<string> FilesNames { get; }
    // public CallbackResult FilesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> Files { get; }
    // public CallbackResult FilesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 920341 *************************


    // Members declared elsewhere:

    // public string? ContentLength { get; }
    // public CallbackResult ContentLengthCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 100035 *************************


    // Members declared elsewhere:

    // public bool HasRequestHeadersContentType { get; }
    // public CallbackResult ContentTypeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 920451 *************************


    // Members declared elsewhere:

    // public IEnumerable<string> RequestHeadersNames { get; }
    // public CallbackResult RequestHeadersNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 100036 *************************

    public Dictionary<string, string?> GetRequestCookiesForRegexPattern(string pattern)
	{
		return this.RequestCookies.Where(p => p.Key.RegexIsMatch(pattern) || p.Value.RegexIsMatch(pattern)).ToDictionary();
	}

    public ICollection<string>? GetRestrictedHeadersExtended()
	{
		var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
		var settings = repository.GetSettings();

		return (ICollection<string>?) settings["RestrictedHeadersExtended"];
	}

	public CallbackResult HeaderName920451Callback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public T? GetTransactionVariable<T>(string variable)

	// Rule: ************************* 920240 *************************


	// Members declared elsewhere:

	// public string? ContentType { get; }
	// public CallbackResult ContentTypeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100037 *************************


	// Members declared elsewhere:

	// public string RequestBody { get; }
	// public CallbackResult RequestBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100038 *************************

	public bool ValidateUrlEncoding(string stringValue)
	{
        var validator = serviceProvider.GetService<ICrsValidator>();

        return validator.ValidateUrlEncoding(stringValue);
    }


    // Members declared elsewhere:

    // public string RequestBody { get; }
    // public CallbackResult ValidateUrlEncodingCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 920015 *************************


    // Members declared elsewhere:

    // public int DetectionParanoiaLevel { get; }
    // public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 920016 *************************


    // Members declared elsewhere:

    // public int DetectionParanoiaLevel { get; }
    // public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 920272 *************************


    // Members declared elsewhere:

    // public string RequestUri { get; }
    // public bool ValidateByteRange(string stringValue, string byteRangeList)
    // public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestHeaders { get; }
    // public bool ValidateByteRange(Dictionary<string, string> dictionaryValue, string byteRangeList)
    // public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public bool ValidateByteRange(Dictionary<string, string> dictionaryValue, string byteRangeList)
    // public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public bool ValidateByteRange(IEnumerable<string> enumerableValue, string byteRangeList)
    // public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestBody { get; }
    // public bool ValidateByteRange(string stringValue, string byteRangeList)
    // public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 920300 *************************

    public bool HasRequestHeadersAccept
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).ContainsKey("Accept");
		}
	}


	// Members declared elsewhere:

	// public CallbackResult AcceptCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100039 *************************


	// Members declared elsewhere:

	// public string RequestMethod { get; }
	// public CallbackResult RequestMethodCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100040 *************************


	// Members declared elsewhere:

	// public string? UserAgent { get; }
	// public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920490 *************************

	public bool HasRequestHeadersXupdevcappostcharset
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).ContainsKey("x-up-devcap-post-charset");
		}
	}

	public CallbackResult XupdevcappostcharsetCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 100041 *************************


	// Members declared elsewhere:

	// public string? UserAgent { get; }
	// public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920510 *************************

	public bool HasRequestHeadersCacheControl
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).ContainsKey("Cache-Control");
		}
	}

	public CallbackResult CacheControlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 100042 *************************

	public string? CacheControl
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).GetDefault("Cache-Control", string.Empty);
		}
	}


	// Members declared elsewhere:

	// public CallbackResult CacheControlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920521 *************************


	// Members declared elsewhere:

	// public string? AcceptEncoding { get; }
	// public CallbackResult AcceptEncodingCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920202 *************************


	// Members declared elsewhere:

	// public string RequestBasename { get; }
	// public CallbackResult RequestBasenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100043 *************************


	// Members declared elsewhere:

	// public string? Range { get; }
	// public CallbackResult RangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? RequestRange { get; }
	// public CallbackResult RequestRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920273 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public bool ValidateByteRange(Dictionary<string, string> dictionaryValue, string byteRangeList)
	// public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public bool ValidateByteRange(IEnumerable<string> enumerableValue, string byteRangeList)
	// public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestBody { get; }
	// public bool ValidateByteRange(string stringValue, string byteRangeList)
	// public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920274 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public bool ValidateByteRange(Dictionary<string, string> dictionaryValue, string byteRangeList)
	// public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? UserAgent { get; }
	// public bool ValidateByteRange(string stringValue, string byteRangeList)
	// public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	public string? Referer
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).GetDefault("Referer", string.Empty);
		}
	}


	// Members declared elsewhere:

	// public bool ValidateByteRange(string stringValue, string byteRangeList)
	// public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	public string? Cookie
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).GetDefault("Cookie", string.Empty);
		}
	}


	// Members declared elsewhere:

	// public bool ValidateByteRange(string stringValue, string byteRangeList)
	// public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	public string? SecFetchUser
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).GetDefault("Sec-Fetch-User", string.Empty);
		}
	}


	// Members declared elsewhere:

	// public bool ValidateByteRange(string stringValue, string byteRangeList)
	// public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	public string? SecCHUA
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).GetDefault("Sec-CH-UA", string.Empty);
		}
	}


	// Members declared elsewhere:

	// public bool ValidateByteRange(string stringValue, string byteRangeList)
	// public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	public string? SecCHUAMobile
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).GetDefault("Sec-CH-UA-Mobile", string.Empty);
		}
	}


	// Members declared elsewhere:

	// public bool ValidateByteRange(string stringValue, string byteRangeList)
	// public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 920275 *************************

	public CallbackResult SecFetchUserCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public string? SecFetchUser { get; }

	public CallbackResult SecCHUAMobileCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public string? SecCHUAMobile { get; }

	// Rule: ************************* 920460 *************************


	// Members declared elsewhere:

	// public string RequestUri { get; }
	// public CallbackResult RequestUriCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\REQUEST-921-PROTOCOL-ATTACK.conf **************************************************

	// Rule: ************************* 921011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921110 *************************


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestBody { get; }
	// public CallbackResult RequestBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	public string GetXmlForWildcardPattern(string wildcardPattern)
	{
		var xml = HttpContext.GetXml();
		var repository = serviceProvider.GetService<ICrsRepository<IGlobal, IGlobal>>();
		var settings = repository.GetSettings();

		// todo = add matched vars

		return DebugUtils.BreakReturnNull();
	}

	public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 921120 *************************

	public Dictionary<string, string?>? RequestCookies
	{
		get
		{
			var cookies = request.Cookies.ToDictionary(c => c.Key, c => (string?) c.Value);

            this.HttpContext.AddMatchedVars(cookies);

            return cookies;
		}
	}

	public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	public IEnumerable<string> RequestCookiesNames
	{
		get
		{
			var names = request.Cookies.Keys;

            this.HttpContext.AddMatchedVars(names.ToDictionary(n => nameof(RequestCookiesNames), n => (string?) n));
            this.HttpContext.AddMatchedVarNames(names.ToList());

            return names;
		}
	}

	public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921130 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921140 *************************


	// Members declared elsewhere:

	// public IEnumerable<string> RequestHeadersNames { get; }
	// public CallbackResult RequestHeadersNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921150 *************************


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921160 *************************

	public IEnumerable<string> ArgsGetNames
	{
		get
		{
			var names = this.HttpContext.GetRequestArgsGetNames();

            this.HttpContext.AddMatchedVars(names.ToDictionary(n => nameof(ArgsGetNames), n => (string?)n));
            this.HttpContext.AddMatchedVarNames(names.ToList());

            return names;
		}
	}

	public CallbackResult ArgsGetNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	public Dictionary<string, string?> ArgsGet
	{
		get
		{
			var args = this.HttpContext.GetRequestArgsGet();

            this.HttpContext.AddMatchedVars(args);

            return args;
		}
	}

	public CallbackResult ArgsGetCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 921190 *************************


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921200 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921421 *************************


	// Members declared elsewhere:

	// public string? ContentType { get; }
	// public CallbackResult ContentTypeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921240 *************************


	// Members declared elsewhere:

	// public string RequestUri { get; }
	// public CallbackResult RequestUriCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921151 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> ArgsGet { get; }
	// public CallbackResult ArgsGetCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921422 *************************


	// Members declared elsewhere:

	// public string? ContentType { get; }
	// public CallbackResult ContentTypeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921230 *************************

	public bool HasRequestHeadersRange
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).ContainsKey("Range");
		}
	}


	// Members declared elsewhere:

	// public CallbackResult RangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921170 *************************


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921180 *************************

	public CallbackResult ParamCounterCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 100044 *************************

	public IEnumerable<string> MatchedVarsNames
	{
		get
		{
			return this.HttpContext.GetLastMatchedVarsNames();
		}
	}

	public CallbackResult MatchedVarsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 921210 *************************


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 921220 *************************


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\REQUEST-922-MULTIPART-ATTACK.conf **************************************************

	// Rule: ************************* 922100 *************************

	public bool HasMultipartPartHeaders_Charset
	{
		get
		{
			return this.HttpContext.GetMultiPartHeaders() != null;
		}
	}

	public CallbackResult _CharsetCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 100045 *************************

	public CallbackResult _922100CharsetCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public T? GetTransactionVariable<T>(string variable)
	// public ICollection<string> GetAllowedRequestContentTypeCharsets { get; }

	// Rule: ************************* 922110 *************************

	public Dictionary<string, string> MultipartPartHeaders
	{
		get
		{
			return this.HttpContext.GetMultiPartHeaders();
		}
	}

	public CallbackResult MultipartPartHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 100046 *************************

	public CallbackResult _1Callback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public string GetLastCapturedGroup(int index)

	// Rule: ************************* 922120 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> MultipartPartHeaders { get; }
	// public CallbackResult MultipartPartHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\REQUEST-930-APPLICATION-ATTACK-LFI.conf **************************************************

	// Rule: ************************* 930011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 930012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 930100 *************************


	// Members declared elsewhere:

	// public string RequestUriRaw { get; }
	// public CallbackResult RequestUriRawCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public string? Referer { get; }


	// Members declared elsewhere:

	// public IEnumerable<string> Files { get; }
	// public CallbackResult FilesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 930110 *************************


	// Members declared elsewhere:

	// public string RequestUri { get; }
	// public CallbackResult RequestUriCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? Referer { get; }
	// public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> Files { get; }
	// public CallbackResult FilesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 930120 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 930130 *************************


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 930013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 930014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 930121 *************************


	// Members declared elsewhere:

	// public string? Referer { get; }
	// public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? UserAgent { get; }
	// public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 930015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 930016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 930017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 930018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\REQUEST-931-APPLICATION-ATTACK-RFI.conf **************************************************

	// Rule: ************************* 931011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 931012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 931100 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 931110 *************************

	public string QueryString
	{
		get
		{
			var queryString = request.QueryString.ToString();

			this.HttpContext.AddMatchedVar(nameof(QueryString), queryString);

			return queryString;
		}
	}

	public CallbackResult QueryStringCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public string RequestBody { get; }
	// public CallbackResult RequestBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 931120 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 931013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 931014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 931130 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100047 *************************

	public CallbackResult RfiParameterCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 931131 *************************


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100048 *************************


	// Members declared elsewhere:

	// public CallbackResult RfiParameterCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 931015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 931016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 931017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 931018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\REQUEST-932-APPLICATION-ATTACK-RCE.conf **************************************************

	// Rule: ************************* 932011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932230 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932235 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932120 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932125 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932130 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932140 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932250 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932260 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932330 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932160 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932170 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestLine { get; }
	// public CallbackResult RequestLineCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932171 *************************


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> FilesNames { get; }
	// public CallbackResult FilesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932175 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932180 *************************


	// Members declared elsewhere:

	// public IEnumerable<string> Files { get; }
	// public CallbackResult FilesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? XFilename { get; }
	// public CallbackResult XFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932370 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932380 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932231 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932131 *************************


	// Members declared elsewhere:

	// public string? UserAgent { get; }
	// public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? Referer { get; }
	// public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932200 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100049 *************************

	public KeyValuePair<string, string?> MatchedVar
	{
		get
		{
			return this.HttpContext.GetLastMatchedVar();
		}
	}

	public CallbackResult MatchedVarCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 100050 *************************


	// Members declared elsewhere:

	// public KeyValuePair<string, string> MatchedVar { get; }
	// public CallbackResult MatchedVarCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932205 *************************


	// Members declared elsewhere:

	// public string? Referer { get; }
	// public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100051 *************************

	public CallbackResult _0Callback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public string GetLastCapturedGroup(int index)

	// Rule: ************************* 100052 *************************


	// Members declared elsewhere:

	// public string GetLastCapturedGroup(int index)
	// public CallbackResult _1Callback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100053 *************************


	// Members declared elsewhere:

	// public string GetLastCapturedGroup(int index)
	// public CallbackResult _1Callback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932206 *************************


	// Members declared elsewhere:

	// public string? Referer { get; }
	// public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100054 *************************


	// Members declared elsewhere:

	// public KeyValuePair<string, string> MatchedVar { get; }
	// public CallbackResult MatchedVarCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100055 *************************


	// Members declared elsewhere:

	// public KeyValuePair<string, string> MatchedVar { get; }
	// public CallbackResult MatchedVarCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932220 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932240 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100056 *************************


	// Members declared elsewhere:

	// public KeyValuePair<string, string> MatchedVar { get; }
	// public CallbackResult MatchedVarCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932210 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932300 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932310 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932320 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932236 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932239 *************************


	// Members declared elsewhere:

	// public string? UserAgent { get; }
	// public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? Referer { get; }
	// public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932161 *************************


	// Members declared elsewhere:

	// public string? UserAgent { get; }
	// public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? Referer { get; }
	// public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932232 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932237 *************************


	// Members declared elsewhere:

	// public string? UserAgent { get; }
	// public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? Referer { get; }
	// public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932238 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? Referer { get; }
	// public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? UserAgent { get; }
	// public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932190 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932301 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932311 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932321 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932331 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 932018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\REQUEST-933-APPLICATION-ATTACK-PHP.conf **************************************************

	// Rule: ************************* 933011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933100 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933110 *************************


	// Members declared elsewhere:

	// public IEnumerable<string> Files { get; }
	// public CallbackResult FilesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? XFilename { get; }
	// public CallbackResult XFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? XFilename { get; }
	// public CallbackResult XFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? XFilename { get; }
	// public CallbackResult XFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? XFileName { get; }
	// public CallbackResult XFileNameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933120 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100057 *************************

	public Dictionary<string, string?> MatchedVars
	{
		get
		{
			return this.HttpContext.GetLastMatchedVars();
		}
	}

	public CallbackResult MatchedVarsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 933130 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933140 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933200 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933150 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933160 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933170 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933180 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933210 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933151 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100058 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> MatchedVars { get; }
	// public CallbackResult MatchedVarsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933131 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933161 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933111 *************************


	// Members declared elsewhere:

	// public IEnumerable<string> Files { get; }
	// public CallbackResult FilesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? XFilename { get; }
	// public CallbackResult XFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? XFilename { get; }
	// public CallbackResult XFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? XFilename { get; }
	// public CallbackResult XFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? XFileName { get; }
	// public CallbackResult XFileNameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933190 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933211 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 933018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\REQUEST-934-APPLICATION-ATTACK-GENERIC.conf **************************************************

	// Rule: ************************* 934011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 934012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 934100 *************************


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 934110 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 934130 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 934150 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 934160 *************************


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 934170 *************************


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 934013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 934014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 934101 *************************


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 934120 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 934140 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 934015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 934016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 934017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 934018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\REQUEST-941-APPLICATION-ATTACK-XSS.conf **************************************************

	// Rule: ************************* 941011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 941012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 941010 *************************


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public bool ValidateByteRange(string stringValue, string byteRangeList)
	// public CallbackResult ValidateByteRangeCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 941100 *************************

	public bool DetectXSS(Dictionary<string, string> dictionaryValue)
	{
        var validator = serviceProvider.GetService<ICrsValidator>();

        return validator.DetectXSS(dictionaryValue);
    }

    public CallbackResult DetectXSSCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }


	// Members declared elsewhere:

	// public CallbackResult UtmCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	public bool DetectXSS(IEnumerable<string> enumerableValue)
	{
        var validator = serviceProvider.GetService<ICrsValidator>();

        return validator.DetectXSS(enumerableValue);
    }


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult DetectXSSCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    public bool DetectXSS(string stringValue)
	{
        var validator = serviceProvider.GetService<ICrsValidator>();

        return validator.DetectXSS(stringValue);
    }


    // Members declared elsewhere:

    // public string? UserAgent { get; }
    // public CallbackResult DetectXSSCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public bool DetectXSS(IEnumerable<string> enumerableValue)
    // public CallbackResult DetectXSSCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public bool DetectXSS(Dictionary<string, string> dictionaryValue)
    // public CallbackResult DetectXSSCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public bool DetectXSS(string stringValue)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941110 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? UserAgent { get; }
    // public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? Referer { get; }
    // public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941130 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? UserAgent { get; }
    // public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941140 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? UserAgent { get; }
    // public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? Referer { get; }
    // public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941160 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? UserAgent { get; }
    // public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? Referer { get; }
    // public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941170 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? UserAgent { get; }
    // public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? Referer { get; }
    // public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941180 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult UtmCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941190 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941200 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941210 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941220 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941230 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941240 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941250 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941260 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941270 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941280 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941290 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941300 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941310 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 100059 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941350 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941360 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941370 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941390 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941400 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941013 *************************


    // Members declared elsewhere:

    // public int DetectionParanoiaLevel { get; }
    // public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941014 *************************


    // Members declared elsewhere:

    // public int DetectionParanoiaLevel { get; }
    // public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941101 *************************


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public bool DetectXSS(string stringValue)
    // public CallbackResult DetectXSSCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? Referer { get; }
    // public bool DetectXSS(string stringValue)
    // public CallbackResult DetectXSSCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941120 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? UserAgent { get; }
    // public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? Referer { get; }
    // public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941150 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? UserAgent { get; }
    // public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941181 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult UtmCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941320 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941330 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941340 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941380 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941015 *************************


    // Members declared elsewhere:

    // public int DetectionParanoiaLevel { get; }
    // public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941016 *************************


    // Members declared elsewhere:

    // public int DetectionParanoiaLevel { get; }
    // public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941017 *************************


    // Members declared elsewhere:

    // public int DetectionParanoiaLevel { get; }
    // public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 941018 *************************


    // Members declared elsewhere:

    // public int DetectionParanoiaLevel { get; }
    // public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\REQUEST-942-APPLICATION-ATTACK-SQLI.conf **************************************************

    // Rule: ************************* 942011 *************************


    // Members declared elsewhere:

    // public int DetectionParanoiaLevel { get; }
    // public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942012 *************************


    // Members declared elsewhere:

    // public int DetectionParanoiaLevel { get; }
    // public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942100 *************************

    public bool DetectSQLI(Dictionary<string, string> dictionaryValue)
	{
        var validator = serviceProvider.GetService<ICrsValidator>();

		return validator.DetectSQLI(dictionaryValue);
    }

    public CallbackResult DetectSQLICallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }


	// Members declared elsewhere:

	// public CallbackResult UtmCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	public bool DetectSQLI(IEnumerable<string> enumerableValue)
	{
        var validator = serviceProvider.GetService<ICrsValidator>();

        return validator.DetectSQLI(enumerableValue);
    }


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult DetectSQLICallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    public bool DetectSQLI(string stringValue)
	{
        var validator = serviceProvider.GetService<ICrsValidator>();

        return validator.DetectSQLI(stringValue);
    }


    // Members declared elsewhere:

    // public string? UserAgent { get; }
    // public CallbackResult DetectSQLICallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? Referer { get; }
    // public bool DetectSQLI(string stringValue)
    // public CallbackResult DetectSQLICallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public bool DetectSQLI(IEnumerable<string> enumerableValue)
    // public CallbackResult DetectSQLICallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public bool DetectSQLI(Dictionary<string, string> dictionaryValue)
    // public CallbackResult DetectSQLICallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public bool DetectSQLI(string stringValue)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942140 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942151 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942160 *************************


    // Members declared elsewhere:

    // public string RequestBasename { get; }
    // public CallbackResult RequestBasenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942170 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942190 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942220 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942230 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942240 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942250 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942270 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942280 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942290 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942320 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942350 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942360 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942500 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942540 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942560 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942550 *************************


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942013 *************************


    // Members declared elsewhere:

    // public int DetectionParanoiaLevel { get; }
    // public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942014 *************************


    // Members declared elsewhere:

    // public int DetectionParanoiaLevel { get; }
    // public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942120 *************************


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string RequestFilename { get; }
    // public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942130 *************************


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 100060 *************************


    // Members declared elsewhere:

    // public string GetLastCapturedGroup(int index)
    // public CallbackResult TransactionMatchCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942131 *************************


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 100061 *************************


    // Members declared elsewhere:

    // public string GetLastCapturedGroup(int index)
    // public CallbackResult TransactionMatchCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942150 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942180 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942200 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? UserAgent { get; }
    // public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? Referer { get; }
    // public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942210 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942260 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942300 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942310 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942330 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942340 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942361 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942362 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942370 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? Referer { get; }
    // public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string? UserAgent { get; }
    // public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942380 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942390 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942400 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942410 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942470 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942480 *************************


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestCookies { get; }
    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> RequestCookiesNames { get; }
    // public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> RequestHeaders { get; }
    // public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942430 *************************


    // Members declared elsewhere:

    // public IEnumerable<string> ArgsNames { get; }
    // public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public Dictionary<string, string> Args { get; }
    // public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


    // Members declared elsewhere:

    // public string GetXmlForWildcardPattern(string xml)
    // public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

    // Rule: ************************* 942441 *************************

    public string? Fbclid
	{
		get
		{
			return this.HttpContext.GetRequestArgsGet().GetDefault("fbclid", string.Empty);
		}
	}

	public CallbackResult FbclidCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 942442 *************************

	public string? Gclid
	{
		get
		{
			return this.HttpContext.GetRequestArgsGet().GetDefault("gclid", string.Empty);
		}
	}

	public CallbackResult GclidCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 942440 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100062 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> MatchedVars { get; }
	// public CallbackResult MatchedVarsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942450 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942510 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942520 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942521 *************************


	// Members declared elsewhere:

	// public string? UserAgent { get; }
	// public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? Referer { get; }
	// public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100063 *************************


	// Members declared elsewhere:

	// public string GetLastCapturedGroup(int index)
	// public CallbackResult _1Callback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942522 *************************


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942101 *************************


	// Members declared elsewhere:

	// public string RequestBasename { get; }
	// public bool DetectSQLI(string stringValue)
	// public CallbackResult DetectSQLICallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public bool DetectSQLI(string stringValue)
	// public CallbackResult DetectSQLICallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942152 *************************


	// Members declared elsewhere:

	// public string? Referer { get; }
	// public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? UserAgent { get; }
	// public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942321 *************************


	// Members declared elsewhere:

	// public string? Referer { get; }
	// public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? UserAgent { get; }
	// public CallbackResult UserAgentCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942251 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942490 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942420 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942431 *************************


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942460 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942511 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942530 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942421 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 942432 *************************


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\REQUEST-943-APPLICATION-ATTACK-SESSION-FIXATION.conf **************************************************

	// Rule: ************************* 943011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 943012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 943100 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 943110 *************************


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100064 *************************


	// Members declared elsewhere:

	// public string? Referer { get; }
	// public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100065 *************************

	public string? GetHost(int index)
	{
		return this.HttpContext.GetLastCapturedGroup(index);
	}


	// Members declared elsewhere:

	// public CallbackResult TransactionMatchCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 943120 *************************


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100066 *************************

	public bool HasRequestHeadersReferer
	{
		get
		{
			return request.Headers.ToDictionary(h => h.Key, h => (string?) h.Value).ContainsKey("Referer");
		}
	}


	// Members declared elsewhere:

	// public CallbackResult RefererCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 943013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 943014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 943015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 943016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 943017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 943018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\REQUEST-944-APPLICATION-ATTACK-JAVA.conf **************************************************

	// Rule: ************************* 944011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944100 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestBody { get; }
	// public CallbackResult RequestBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944110 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestBody { get; }
	// public CallbackResult RequestBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100067 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestBody { get; }
	// public CallbackResult RequestBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944120 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestBody { get; }
	// public CallbackResult RequestBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100068 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> MatchedVars { get; }
	// public CallbackResult MatchedVarsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944130 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestBody { get; }
	// public CallbackResult RequestBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestFilename { get; }
	// public CallbackResult RequestFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944140 *************************


	// Members declared elsewhere:

	// public IEnumerable<string> Files { get; }
	// public CallbackResult FilesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? XFilename { get; }
	// public CallbackResult XFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? XFilename { get; }
	// public CallbackResult XFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? XFilename { get; }
	// public CallbackResult XFilenameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string? XFileName { get; }
	// public CallbackResult XFileNameCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944150 *************************


	// Members declared elsewhere:

	// public string RequestLine { get; }
	// public CallbackResult RequestLineCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944151 *************************


	// Members declared elsewhere:

	// public string RequestLine { get; }
	// public CallbackResult RequestLineCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944200 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestBody { get; }
	// public CallbackResult RequestBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944210 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestBody { get; }
	// public CallbackResult RequestBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944240 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestBody { get; }
	// public CallbackResult RequestBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944250 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestBody { get; }
	// public CallbackResult RequestBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944260 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestBody { get; }
	// public CallbackResult RequestBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944300 *************************


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string RequestBody { get; }
	// public CallbackResult RequestBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 944152 *************************


	// Members declared elsewhere:

	// public string RequestLine { get; }
	// public CallbackResult RequestLineCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> Args { get; }
	// public CallbackResult ArgsCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> ArgsNames { get; }
	// public CallbackResult ArgsNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestCookies { get; }
	// public CallbackResult RequestCookiesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public IEnumerable<string> RequestCookiesNames { get; }
	// public CallbackResult RequestCookiesNamesCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public Dictionary<string, string> RequestHeaders { get; }
	// public CallbackResult RequestHeadersCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)


	// Members declared elsewhere:

	// public string GetXmlForWildcardPattern(string xml)
	// public CallbackResult XmlCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\REQUEST-949-BLOCKING-EVALUATION.conf **************************************************

	// Rule: ************************* 949052 *************************

	public int? BlockingParanoiaLevel()
	{
		var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
		var settings = repository.GetSettings();

		return (int?) settings["BlockingParanoiaLevel"];
	}


	// Members declared elsewhere:

	// public CallbackResult BlockingParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949152 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949053 *************************


	// Members declared elsewhere:

	// public int BlockingParanoiaLevel { get; }
	// public CallbackResult BlockingParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949153 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949054 *************************


	// Members declared elsewhere:

	// public int BlockingParanoiaLevel { get; }
	// public CallbackResult BlockingParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949154 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949055 *************************


	// Members declared elsewhere:

	// public int BlockingParanoiaLevel { get; }
	// public CallbackResult BlockingParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949155 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949060 *************************


	// Members declared elsewhere:

	// public int BlockingParanoiaLevel { get; }
	// public CallbackResult BlockingParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949160 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949061 *************************


	// Members declared elsewhere:

	// public int BlockingParanoiaLevel { get; }
	// public CallbackResult BlockingParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949161 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949062 *************************


	// Members declared elsewhere:

	// public int BlockingParanoiaLevel { get; }
	// public CallbackResult BlockingParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949162 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949063 *************************


	// Members declared elsewhere:

	// public int BlockingParanoiaLevel { get; }
	// public CallbackResult BlockingParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949163 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949111 *************************

	public int? GetInboundAnomalyScoreThreshold()
	{
		var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
		var settings = repository.GetSettings();

		return (int?) settings["InboundAnomalyScoreThreshold"];
	}

	public CallbackResult BlockingInboundAnomalyScoreCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public T? GetTransactionVariable<T>(string variable)

	// Rule: ************************* 100069 *************************

	public bool? EarlyBlocking()
	{
		var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
		var settings = repository.GetSettings();

		return (bool?) settings["EarlyBlocking"];
	}


	// Members declared elsewhere:

	// public CallbackResult EarlyBlockingCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100070 *************************


	// Members declared elsewhere:

	// public T? GetTransactionVariable<T>(string variable)
	// public int GetInboundAnomalyScoreThreshold { get; }
	// public CallbackResult BlockingInboundAnomalyScoreCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 949018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\RESPONSE-950-DATA-LEAKAGES.conf **************************************************

	// Rule: ************************* 950011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 950012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 950130 *************************

	public string ResponseBody
	{
		get
		{
            var body = response.GetBody();

            this.HttpContext.AddMatchedVarName(nameof(ResponseBody));
            this.HttpContext.AddMatchedVar(nameof(ResponseBody), body);

            return body;
		}
	}

	public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 950140 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 950013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 950014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 950100 *************************

	public string ResponseStatus
	{
		get
		{
			var status = response.StatusCode.ToString();

            this.HttpContext.AddMatchedVar(nameof(ResponseStatus), status);

            return status;
		}
	}

	public CallbackResult ResponseStatusCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 950015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 950016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 950017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 950018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\RESPONSE-951-DATA-LEAKAGES-SQL.conf **************************************************

	// Rule: ************************* 951011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951100 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951110 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951120 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951130 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951140 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951150 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951160 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951170 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951180 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951190 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951200 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951210 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951220 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951230 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951240 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951250 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951260 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 951018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\RESPONSE-952-DATA-LEAKAGES-JAVA.conf **************************************************

	// Rule: ************************* 952011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 952012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 952100 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 952110 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 952013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 952014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 952015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 952016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 952017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 952018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\RESPONSE-953-DATA-LEAKAGES-PHP.conf **************************************************

	// Rule: ************************* 953011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 953012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 953100 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 953110 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 953120 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 953013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 953014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 953101 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 953015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 953016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 953017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 953018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\RESPONSE-954-DATA-LEAKAGES-IIS.conf **************************************************

	// Rule: ************************* 954011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 954012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 954100 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 954110 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 954120 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 954130 *************************


	// Members declared elsewhere:

	// public int ResponseStatus { get; }
	// public CallbackResult ResponseStatusCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100071 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 954013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 954014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 954015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 954016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 954017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 954018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\RESPONSE-955-WEB-SHELLS.conf **************************************************

	// Rule: ************************* 955011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955100 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955110 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955120 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955130 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955140 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955150 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955160 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955170 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955180 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955190 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955200 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955210 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955220 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955230 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955240 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955250 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955260 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955270 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955280 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955290 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955300 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955310 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955320 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955330 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955340 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955350 *************************


	// Members declared elsewhere:

	// public string ResponseBody { get; }
	// public CallbackResult ResponseBodyCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 955018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\RESPONSE-959-BLOCKING-EVALUATION.conf **************************************************

	// Rule: ************************* 959052 *************************


	// Members declared elsewhere:

	// public int BlockingParanoiaLevel { get; }
	// public CallbackResult BlockingParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959152 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959053 *************************


	// Members declared elsewhere:

	// public int BlockingParanoiaLevel { get; }
	// public CallbackResult BlockingParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959153 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959054 *************************


	// Members declared elsewhere:

	// public int BlockingParanoiaLevel { get; }
	// public CallbackResult BlockingParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959154 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959055 *************************


	// Members declared elsewhere:

	// public int BlockingParanoiaLevel { get; }
	// public CallbackResult BlockingParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959155 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959060 *************************


	// Members declared elsewhere:

	// public int BlockingParanoiaLevel { get; }
	// public CallbackResult BlockingParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959160 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959061 *************************


	// Members declared elsewhere:

	// public int BlockingParanoiaLevel { get; }
	// public CallbackResult BlockingParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959161 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959062 *************************


	// Members declared elsewhere:

	// public int BlockingParanoiaLevel { get; }
	// public CallbackResult BlockingParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959162 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959063 *************************


	// Members declared elsewhere:

	// public int BlockingParanoiaLevel { get; }
	// public CallbackResult BlockingParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959163 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959101 *************************

	public int? GetOutboundAnomalyScoreThreshold()
	{
		var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
		var settings = repository.GetSettings();

		return (int?) settings["OutboundAnomalyScoreThreshold"];
	}

	public CallbackResult BlockingOutboundAnomalyScoreCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public T? GetTransactionVariable<T>(string variable)

	// Rule: ************************* 100072 *************************


	// Members declared elsewhere:

	// public bool EarlyBlocking { get; }
	// public CallbackResult EarlyBlockingCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 100073 *************************


	// Members declared elsewhere:

	// public T? GetTransactionVariable<T>(string variable)
	// public int GetOutboundAnomalyScoreThreshold { get; }
	// public CallbackResult BlockingOutboundAnomalyScoreCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 959018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// File: ************************************************** C:\Users\kenln\Downloads\coreruleset\rules\RESPONSE-980-CORRELATION.conf **************************************************

	// Rule: ************************* 980041 *************************

	public int? ReportingLevel()
	{
		var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
		var settings = repository.GetSettings();

		return (int?) settings["ReportingLevel"];
	}


	// Members declared elsewhere:

	// public CallbackResult ReportingLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 980042 *************************


	// Members declared elsewhere:

	// public int ReportingLevel { get; }
	// public CallbackResult ReportingLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 980043 *************************

	public string? DetectionAnomalyScore()
	{
		var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
		var settings = repository.GetSettings();

		return (string?) settings["DetectionAnomalyScore"];
	}

	public CallbackResult DetectionAnomalyScoreCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 980044 *************************


	// Members declared elsewhere:

	// public T? GetTransactionVariable<T>(string variable)
	// public int GetInboundAnomalyScoreThreshold { get; }
	// public CallbackResult BlockingInboundAnomalyScoreCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 980045 *************************


	// Members declared elsewhere:

	// public T? GetTransactionVariable<T>(string variable)
	// public int GetOutboundAnomalyScoreThreshold { get; }
	// public CallbackResult BlockingOutboundAnomalyScoreCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 980046 *************************


	// Members declared elsewhere:

	// public int ReportingLevel { get; }
	// public CallbackResult ReportingLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 980047 *************************

	public CallbackResult DetectionInboundAnomalyScoreCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public T? GetTransactionVariable<T>(string variable)
	// public int GetInboundAnomalyScoreThreshold { get; }

	// Rule: ************************* 980048 *************************

	public CallbackResult DetectionOutboundAnomalyScoreCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}


	// Members declared elsewhere:

	// public T? GetTransactionVariable<T>(string variable)
	// public int GetOutboundAnomalyScoreThreshold { get; }

	// Rule: ************************* 980049 *************************


	// Members declared elsewhere:

	// public int ReportingLevel { get; }
	// public CallbackResult ReportingLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 980050 *************************

	public int? BlockingAnomalyScore()
	{
		var repository = serviceProvider.GetService<ICrsRepository<ITransaction, ITransaction>>();
		var settings = repository.GetSettings();

		return (int?) settings["BlockingAnomalyScore"];
	}

	public CallbackResult BlockingAnomalyScoreCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)
	{
		string? skipAfter = null;
		var captures = this.GroupCaptures;
		var rule = this.CurrentRule;

		if (!disabled)
		{
			var result = transActionsProvider.HandleTransformationsActions(rule, this, captures, this.ContextMatches, TransformationsActions, failed, httpContext);
			var statusCode = result.HttpStatusCode;

			if (statusCode.IsSuccess())
			{
				failed = false;
				skipAfter = result.SkipAfter;
			}
			else if (failed)
			{
				response.StatusCode = (int) statusCode;
			}
		}

		return new CallbackResult(failed && !disabled, skipAfter);
	}

	// Rule: ************************* 980051 *************************


	// Members declared elsewhere:

	// public int ReportingLevel { get; }
	// public CallbackResult ReportingLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 980011 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 980012 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 980013 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 980014 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 980015 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 980016 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 980017 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

	// Rule: ************************* 980018 *************************


	// Members declared elsewhere:

	// public int DetectionParanoiaLevel { get; }
	// public CallbackResult DetectionParanoiaLevelCallback(HttpContext httpContext, IServiceProvider serviceProvider, bool failed, bool disabled, JToken TransformationsActions)

}
