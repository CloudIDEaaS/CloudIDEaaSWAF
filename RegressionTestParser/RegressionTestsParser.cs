using Microsoft.AspNetCore.Http;
using Moq;
using OWASPCoreRulesetParser.Emitter;
using Utils;
using YamlDotNet.Serialization;

namespace OWASPCoreRulesetParser
{
    public class RegressionTestsParser
    {
        public Dictionary<string, TestInputExpectedResult> Parse(string testsPath, bool writeToConsole = true)
        {
            var directory = new DirectoryInfo(testsPath);
            var testFiles = directory.GetFiles("*.yaml", SearchOption.AllDirectories).ToList();
            var testInputExpectedResultDictionary = new Dictionary<string, TestInputExpectedResult>();
            var fileIndex = 0;

            if (writeToConsole)
            {
                Console.WriteLine("Found {0} test files.", testFiles.Count);
            }

            foreach (var testFile in testFiles)
            {
                var testFileName = testFile.FullName;

                using (var stream = File.OpenRead(testFileName))
                using (var reader = new StreamReader(stream))
                {
                    var deserializerBuilder = new DeserializerBuilder();
                    Dictionary<object, object> yamlObject;
                    List<object> testsYamlObject;
                    IDeserializer deserializer;

                    deserializer = deserializerBuilder.Build();

                    if (writeToConsole)
                    {
                        Console.WriteLine("Processing {0} of {1} test files.", fileIndex + 1, testFiles.Count);
                    }

                    yamlObject = (Dictionary<object, object>) deserializer.Deserialize(reader);
                    testsYamlObject = (List<object>) yamlObject["tests"];

                    foreach (var testObject in testsYamlObject)
                    {
                        var testDictionary = (Dictionary<object, object>)testObject;
                        var title = (string) testDictionary["test_title"];
                        var stages = (List<object>) testDictionary["stages"];
                        var inputExpectedResult = new TestInputExpectedResult(title, testFileName);

                        // inputs

                        foreach (Dictionary<object, object> stage in stages)
                        {
                            var innerStage = (Dictionary<object, object>) stage["stage"];
                            var input = (Dictionary<object, object>) innerStage["input"];
                            var output = (Dictionary<object, object>) innerStage["output"];
                            var context = new DefaultHttpContext();
                            var expectedResult = new ExpectedResult();
                            var connection = context.Connection;
                            var request = context.Request;
                            var response = context.Response;
                            UriBuilder? uriBuilder = null;
                            UriBuilder? uriConnectBuilder = null;
                            string? destAddress = null;
                            string? port = null;
                            string? method = null;
                            string? data = null;
                            string? uri = null;
                            string? version = null;
                            string? protocol = null;
                            string? stopMagic = null;
                            string? encodedRequest = null;
                            string? noLogContains = null;
                            string? logContains = null;
                            object? statusText = null;
                            int? status = null;
                            string? expectError = null;
                            Dictionary<object, object>? headers = null;

                            foreach (var pair in input)
                            {
                                var value = pair.Value;

                                switch (pair.Key)
                                {
                                    case "dest_addr":
                                        {
                                            destAddress = (string) value;
                                        }
                                        break;
                                    case "port":
                                        {
                                            port = (string)value;
                                        }
                                        break;
                                    case "headers":
                                        {
                                            headers = (Dictionary<object, object>)value;
                                        }
                                        break;
                                    case "method":
                                        {
                                            method = (string)value;
                                        }
                                        break;
                                    case "data":
                                        {
                                            data = (string)value;
                                        }
                                        break;
                                    case "uri":
                                        {
                                            uri = (string)value;
                                        }
                                        break;
                                    case "version":
                                        {
                                            version = (string)value;
                                        }
                                        break;
                                    case "protocol":
                                        {
                                            protocol = (string)value;
                                        }
                                        break;
                                    case "stop_magic":
                                        {
                                            stopMagic = (string)value; 
                                        }
                                        break;
                                    case "encoded_request":
                                        {
                                            encodedRequest = (string)value;
                                        }
                                        break;
                                    default:
                                        DebugUtils.Break();
                                        break;
                                }
                            }

                            if (destAddress != null)
                            {
                                uriBuilder = new UriBuilder(destAddress);
                            }

                            if (port != null)
                            {
                                uriBuilder.Port = int.Parse(port);
                            }
                            
                            if (uri != null)
                            {
                                switch (method)
                                {
                                    case "CONNECT":
                                        uriConnectBuilder = new UriBuilder(uri);
                                        break;
                                    default:
                                        uriBuilder.Path = uri;
                                        break;
                                }
                            }
                            
                            if (version != null)
                            {
                                request.Protocol = version;
                            }
                            
                            if (protocol != null)
                            {
                                uriBuilder.Scheme = protocol;
                            }
                            
                            if (stopMagic != null)
                            {
                                inputExpectedResult.StopMagic = bool.Parse(stopMagic);
                            }
                            
                            if (encodedRequest != null)
                            {
                                inputExpectedResult.IsEncodedRequest = true;
                                inputExpectedResult.EncodedRequest = encodedRequest;
                            }

                            if (method != null)
                            {
                                request.Method = method;
                            }

                            if (data != null)
                            {
                                switch (method)
                                {
                                    case "POST":
                                        request.Body = data.ToStream();
                                        break;
                                    case "GET":
                                        uriBuilder.Query = "?" + data;
                                        break;
                                    case "HEAD":
                                        uriBuilder.Query = "?" + data;
                                        break;
                                    default:
                                        request.Body = data.ToStream();
                                        break;
                                }
                            }

                            if (headers != null)
                            {
                                foreach (var header in headers)
                                {
                                    var key = (string)header.Key;
                                    var value = (string)header.Value;   

                                    request.Headers.Append(key, value);
                                }
                            }

                            if (uriBuilder != null && uriConnectBuilder != null)
                            {
                                request.Scheme = uriBuilder.Scheme;
                                request.Host = new HostString(uriBuilder.Host);
                                request.Path = uriBuilder.Path;
                                request.PathBase = uriBuilder.Path.LeftUpToIndexOf('/');
                            }
                            else
                            {
                                if (uriBuilder != null)
                                {
                                    request.Scheme = uriBuilder.Scheme;
                                    request.Host = new HostString(uriBuilder.Host);

                                    // these will never happen because ASP.NET will never let it, but test anyway

                                    if (title.IsOneOf("920270-1", "920270-2", "920270-3", "920270-9", "920271-3", "930100-3", "930120-1", "930120-2", "930120-3", "933170-5"))
                                    {
                                        var pathString = new PathString(uriBuilder.Path);

                                        request.Path = pathString;
                                    }
                                    else
                                    {
                                        request.Path = uriBuilder.Path.PrependIfMissing("/");
                                    }
                                }

                                if (uriConnectBuilder != null)
                                {

                                }
                            }

                            inputExpectedResult.HttpContext = context;
                            inputExpectedResult.ExpectedResult = expectedResult;

                            //context.Abort();
                            //context.CallPrivateMethod("DebuggerToString");

                            // expected results

                            foreach (var pair in output)
                            {
                                var value = pair.Value;

                                switch (pair.Key)
                                {
                                    case "no_log_contains":
                                        {
                                            noLogContains = (string)value;
                                        }
                                        break;
                                    case "log_contains":
                                        {
                                            logContains = (string)value;
                                        }
                                        break;
                                    case "status":
                                        {
                                            switch (statusText)
                                            {
                                                case List<object> statusList:

                                                    if (statusList.Count == 1)
                                                    {
                                                        status = int.Parse((string) statusList[0]);
                                                    }
                                                    else
                                                    {
                                                        DebugUtils.Break();
                                                    }

                                                    break;
                                                case string statusString:
                                                    status = int.Parse(statusString);
                                                    break;
                                            }
                                        }
                                        break;
                                    case "expect_error":
                                        {
                                            expectError = (string)value;
                                        }
                                        break;
                                    default:
                                        DebugUtils.Break();
                                        break;
                                }
                            }

                            if (noLogContains != null)
                            {
                                expectedResult.NoLogContains = noLogContains;
                            }
                            
                            if (logContains != null)
                            {
                                expectedResult.LogContains = logContains;
                            }
                            
                            if (status != null)
                            {
                                expectedResult.Status = status;
                            }
                            
                            if (expectError != null)
                            {
                                expectedResult.Error = expectError;
                            }
                        }

                        testInputExpectedResultDictionary.Add(title, inputExpectedResult);
                    }
                }

                fileIndex++;
            }

            var requests = testInputExpectedResultDictionary.Values.Select(r => r.HttpContext).ToList();

            return testInputExpectedResultDictionary;
        }
    }
}
