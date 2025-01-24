using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace HtmlAgilityPack
{
	public class HtmlWeb
	{
		public delegate void PostResponseHandler(HttpWebRequest request, HttpWebResponse response);

		public delegate void PreHandleDocumentHandler(HtmlDocument document);

		public delegate bool PreRequestHandler(HttpWebRequest request);

		private bool _autoDetectEncoding = true;

		private bool _cacheOnly;

		private string _cachePath;

		private bool _fromCache;

		private int _requestDuration;

		private Uri _responseUri;

		private HttpStatusCode _statusCode = HttpStatusCode.OK;

		private int _streamBufferSize = 1024;

		private bool _useCookies;

		private bool _usingCache;

		private bool _usingCacheAndLoad;

		private bool _usingCacheIfExists;

		private string _userAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:x.x.x) Gecko/20041107 Firefox/x.x";

		private int _timeout = 100000;

		private int? _maxAutoRedirects;

		public PostResponseHandler PostResponse;

		public PreHandleDocumentHandler PreHandleDocument;

		public PreRequestHandler PreRequest;

		internal static ConcurrentDictionary<string, HttpClient> SharedHttpClient = new ConcurrentDictionary<string, HttpClient>();

		private Encoding _encoding;

		public DecompressionMethods AutomaticDecompression { get; set; }

		public int? MaxAutoRedirects
		{
			get
			{
				return _maxAutoRedirects;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException();
				}
				_maxAutoRedirects = value;
			}
		}

		public int Timeout
		{
			get
			{
				return _timeout;
			}
			set
			{
				if (value <= 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException();
				}
				_timeout = value;
			}
		}

		public bool AutoDetectEncoding
		{
			get
			{
				return _autoDetectEncoding;
			}
			set
			{
				_autoDetectEncoding = value;
			}
		}

		public Encoding OverrideEncoding
		{
			get
			{
				return _encoding;
			}
			set
			{
				_encoding = value;
			}
		}

		public bool CacheOnly
		{
			get
			{
				return _cacheOnly;
			}
			set
			{
				if (value && !UsingCache)
				{
					throw new HtmlWebException("Cache is not enabled. Set UsingCache to true first.");
				}
				_cacheOnly = value;
			}
		}

		public bool UsingCacheIfExists
		{
			get
			{
				return _usingCacheIfExists;
			}
			set
			{
				_usingCacheIfExists = value;
			}
		}

		public string CachePath
		{
			get
			{
				return _cachePath;
			}
			set
			{
				_cachePath = value;
			}
		}

		public bool FromCache => _fromCache;

		public int RequestDuration => _requestDuration;

		public Uri ResponseUri => _responseUri;

		public HttpStatusCode StatusCode => _statusCode;

		public int StreamBufferSize
		{
			get
			{
				return _streamBufferSize;
			}
			set
			{
				if (_streamBufferSize <= 0)
				{
					throw new ArgumentException("Size must be greater than zero.");
				}
				_streamBufferSize = value;
			}
		}

		public bool UseCookies
		{
			get
			{
				return _useCookies;
			}
			set
			{
				_useCookies = value;
			}
		}

		public bool CaptureRedirect { get; set; }

		public string UserAgent
		{
			get
			{
				return _userAgent;
			}
			set
			{
				_userAgent = value;
			}
		}

		public bool UsingCache
		{
			get
			{
				if (_cachePath != null)
				{
					return _usingCache;
				}
				return false;
			}
			set
			{
				if (value && _cachePath == null)
				{
					throw new HtmlWebException("You need to define a CachePath first.");
				}
				_usingCache = value;
			}
		}

		internal static HttpClient GetSharedHttpClient(string userAgent)
		{
			return SharedHttpClient.GetOrAdd(userAgent, (string x) => new HttpClient
			{
				DefaultRequestHeaders = { { "User-Agent", userAgent } }
			});
		}

		public static string GetContentTypeForExtension(string extension, string def)
		{
			if (string.IsNullOrEmpty(extension))
			{
				return def;
			}
			string value = "";
			if (!extension.StartsWith("."))
			{
				extension = "." + extension;
			}
			if (!MimeTypeMap.Mappings.TryGetValue(extension, out value))
			{
				value = def;
			}
			return value;
		}

		public static string GetExtensionForContentType(string contentType, string def)
		{
			if (string.IsNullOrEmpty(contentType))
			{
				return def;
			}
			if (contentType.StartsWith("."))
			{
				throw new ArgumentException("Requested mime type is not valid: " + contentType);
			}
			string value = "";
			if (!MimeTypeMap.Mappings.TryGetValue(contentType, out value))
			{
				value = def;
			}
			return value;
		}

		public object CreateInstance(string url, Type type)
		{
			return CreateInstance(url, null, null, type);
		}

		public void Get(string url, string path)
		{
			Get(url, path, "GET");
		}

		public void Get(string url, string path, WebProxy proxy, NetworkCredential credentials)
		{
			Get(url, path, proxy, credentials, "GET");
		}

		public void Get(string url, string path, string method)
		{
			Uri uri = new Uri(url);
			if (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
			{
				Get(uri, method, path, null, null, null);
				return;
			}
			throw new HtmlWebException("Unsupported uri scheme: '" + uri.Scheme + "'.");
		}

		public void Get(string url, string path, WebProxy proxy, NetworkCredential credentials, string method)
		{
			Uri uri = new Uri(url);
			if (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
			{
				Get(uri, method, path, null, proxy, credentials);
				return;
			}
			throw new HtmlWebException("Unsupported uri scheme: '" + uri.Scheme + "'.");
		}

		public string GetCachePath(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (!UsingCache)
			{
				throw new HtmlWebException("Cache is not enabled. Set UsingCache to true first.");
			}
			if (uri.AbsolutePath == "/")
			{
				return Path.Combine(_cachePath, ".htm");
			}
			string text = uri.AbsolutePath;
			string text2 = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
			for (int i = 0; i < text2.Length; i++)
			{
				text = text.Replace(text2[i].ToString(), "");
			}
			if (uri.AbsolutePath[uri.AbsolutePath.Length - 1] == Path.AltDirectorySeparatorChar)
			{
				return Path.Combine(_cachePath, (uri.Host + text.TrimEnd(new char[1] { Path.AltDirectorySeparatorChar })).Replace('/', '\\') + ".htm");
			}
			return Path.Combine(_cachePath, uri.Host + text.Replace('/', '\\'));
		}

		public HtmlDocument Load(string url)
		{
			return Load(url, "GET");
		}

		public HtmlDocument Load(Uri uri)
		{
			return Load(uri, "GET");
		}

		public HtmlDocument Load(string url, string proxyHost, int proxyPort, string userId, string password)
		{
			WebProxy webProxy = new WebProxy(proxyHost, proxyPort);
			webProxy.BypassProxyOnLocal = true;
			NetworkCredential networkCredential = null;
			if (userId != null && password != null)
			{
				networkCredential = new NetworkCredential(userId, password);
				new CredentialCache
				{
					{ webProxy.Address, "Basic", networkCredential },
					{ webProxy.Address, "Digest", networkCredential }
				};
			}
			return Load(url, "GET", webProxy, networkCredential);
		}

		public HtmlDocument Load(Uri uri, string proxyHost, int proxyPort, string userId, string password)
		{
			WebProxy webProxy = new WebProxy(proxyHost, proxyPort);
			webProxy.BypassProxyOnLocal = true;
			NetworkCredential networkCredential = null;
			if (userId != null && password != null)
			{
				networkCredential = new NetworkCredential(userId, password);
				new CredentialCache
				{
					{ webProxy.Address, "Basic", networkCredential },
					{ webProxy.Address, "Digest", networkCredential }
				};
			}
			return Load(uri, "GET", webProxy, networkCredential);
		}

		public HtmlDocument Load(string url, string method)
		{
			Uri uri = new Uri(url);
			return Load(uri, method);
		}

		public HtmlDocument Load(Uri uri, string method)
		{
			if (UsingCache)
			{
				_usingCacheAndLoad = true;
			}
			HtmlDocument htmlDocument;
			if (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
			{
				htmlDocument = LoadUrl(uri, method, null, null);
			}
			else
			{
				if (!(uri.Scheme == Uri.UriSchemeFile))
				{
					throw new HtmlWebException("Unsupported uri scheme: '" + uri.Scheme + "'.");
				}
				htmlDocument = new HtmlDocument();
				htmlDocument.OptionAutoCloseOnEnd = false;
				htmlDocument.OptionAutoCloseOnEnd = true;
				if (OverrideEncoding != null)
				{
					htmlDocument.Load(uri.OriginalString, OverrideEncoding);
				}
				else
				{
					htmlDocument.DetectEncodingAndLoad(uri.OriginalString, _autoDetectEncoding);
				}
			}
			if (PreHandleDocument != null)
			{
				PreHandleDocument(htmlDocument);
			}
			return htmlDocument;
		}

		public HtmlDocument Load(string url, string method, WebProxy proxy, NetworkCredential credentials)
		{
			Uri uri = new Uri(url);
			return Load(uri, method, proxy, credentials);
		}

		public HtmlDocument Load(Uri uri, string method, WebProxy proxy, NetworkCredential credentials)
		{
			if (UsingCache)
			{
				_usingCacheAndLoad = true;
			}
			HtmlDocument htmlDocument;
			if (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
			{
				htmlDocument = LoadUrl(uri, method, proxy, credentials);
			}
			else
			{
				if (!(uri.Scheme == Uri.UriSchemeFile))
				{
					throw new HtmlWebException("Unsupported uri scheme: '" + uri.Scheme + "'.");
				}
				htmlDocument = new HtmlDocument();
				htmlDocument.OptionAutoCloseOnEnd = false;
				htmlDocument.OptionAutoCloseOnEnd = true;
				htmlDocument.DetectEncodingAndLoad(uri.OriginalString, _autoDetectEncoding);
			}
			if (PreHandleDocument != null)
			{
				PreHandleDocument(htmlDocument);
			}
			return htmlDocument;
		}

		public void LoadHtmlAsXml(string htmlUrl, XmlTextWriter writer)
		{
			Load(htmlUrl).Save(writer);
		}

		private static void FilePreparePath(string target)
		{
			if (File.Exists(target))
			{
				FileAttributes attributes = File.GetAttributes(target);
				File.SetAttributes(target, attributes & ~FileAttributes.ReadOnly);
				return;
			}
			string directoryName = Path.GetDirectoryName(target);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
		}

		private static DateTime RemoveMilliseconds(DateTime t)
		{
			return new DateTime(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second, 0);
		}

		private static DateTime RemoveMilliseconds(DateTimeOffset? offset)
		{
			DateTimeOffset dateTimeOffset = offset ?? DateTimeOffset.Now;
			return new DateTime(dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, dateTimeOffset.Hour, dateTimeOffset.Minute, dateTimeOffset.Second, 0);
		}

		private static long SaveStream(Stream stream, string path, DateTime touchDate, int streamBufferSize)
		{
			FilePreparePath(path);
			long num = 0L;
			using (FileStream output = new FileStream(path, FileMode.Create, FileAccess.Write))
			{
				using (BinaryReader binaryReader = new BinaryReader(stream))
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(output))
					{
						byte[] array;
						do
						{
							array = binaryReader.ReadBytes(streamBufferSize);
							num += array.Length;
							if (array.Length != 0)
							{
								binaryWriter.Write(array);
							}
						}
						while (array.Length != 0);
						binaryWriter.Flush();
					}
				}
			}
			File.SetLastWriteTime(path, touchDate);
			return num;
		}

		private HttpStatusCode Get(Uri uri, string method, string path, HtmlDocument doc, IWebProxy proxy, ICredentials creds)
		{
			string text = null;
			bool flag = false;
			HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
			if (MaxAutoRedirects.HasValue)
			{
				httpWebRequest.MaximumAutomaticRedirections = MaxAutoRedirects.Value;
			}
			httpWebRequest.Timeout = Timeout;
			httpWebRequest.Method = method;
			httpWebRequest.UserAgent = UserAgent;
			httpWebRequest.AutomaticDecompression = AutomaticDecompression;
			if (CaptureRedirect)
			{
				httpWebRequest.AllowAutoRedirect = false;
			}
			if (proxy != null)
			{
				if (creds != null)
				{
					proxy.Credentials = creds;
					httpWebRequest.Credentials = creds;
				}
				else
				{
					proxy.Credentials = CredentialCache.DefaultCredentials;
					httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
				}
				httpWebRequest.Proxy = proxy;
			}
			_fromCache = false;
			_requestDuration = 0;
			int tickCount = Environment.TickCount;
			if (UsingCache)
			{
				text = GetCachePath(httpWebRequest.RequestUri);
				if (File.Exists(text))
				{
					httpWebRequest.IfModifiedSince = File.GetLastAccessTime(text);
					flag = true;
				}
			}
			if (_cacheOnly || _usingCacheIfExists)
			{
				if (File.Exists(text))
				{
					if (path != null)
					{
						IOLibrary.CopyAlways(text, path);
						if (text != null)
						{
							File.SetLastWriteTime(path, File.GetLastWriteTime(text));
						}
					}
					_fromCache = true;
					return HttpStatusCode.NotModified;
				}
				if (_cacheOnly)
				{
					throw new HtmlWebException("File was not found at cache path: '" + text + "'");
				}
			}
			if (_useCookies)
			{
				httpWebRequest.CookieContainer = new CookieContainer();
			}
			if (PreRequest != null && !PreRequest(httpWebRequest))
			{
				return HttpStatusCode.ResetContent;
			}
			HttpWebResponse httpWebResponse;
			try
			{
				httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
			}
			catch (WebException ex)
			{
				_requestDuration = Environment.TickCount - tickCount;
				httpWebResponse = (HttpWebResponse)ex.Response;
				if (httpWebResponse == null)
				{
					if (flag)
					{
						if (path != null)
						{
							IOLibrary.CopyAlways(text, path);
							File.SetLastWriteTime(path, File.GetLastWriteTime(text));
						}
						return HttpStatusCode.NotModified;
					}
					throw;
				}
			}
			catch (Exception)
			{
				_requestDuration = Environment.TickCount - tickCount;
				throw;
			}
			if (PostResponse != null)
			{
				PostResponse(httpWebRequest, httpWebResponse);
			}
			_requestDuration = Environment.TickCount - tickCount;
			_responseUri = httpWebResponse.ResponseUri;
			HttpStatusCode statusCode = httpWebResponse.StatusCode;
			bool flag2 = IsHtmlContent(httpWebResponse.ContentType);
			bool flag3 = string.IsNullOrEmpty(httpWebResponse.ContentType);
			string text2 = "";
			if (!string.IsNullOrEmpty(httpWebResponse.CharacterSet))
			{
				text2 = httpWebResponse.CharacterSet.Replace("\"", "");
			}
			Encoding encoding = null;
			try
			{
				encoding = ((!string.IsNullOrEmpty(flag2 ? text2 : httpWebResponse.ContentEncoding)) ? Encoding.GetEncoding(flag2 ? text2 : httpWebResponse.ContentEncoding) : null);
			}
			catch (Exception)
			{
			}
			if (OverrideEncoding != null)
			{
				encoding = OverrideEncoding;
			}
			if (CaptureRedirect && httpWebResponse.StatusCode == HttpStatusCode.Found)
			{
				string text3 = httpWebResponse.Headers["Location"];
				if (!Uri.TryCreate(text3, UriKind.Absolute, out var result))
				{
					result = new Uri(uri, text3);
				}
				return Get(result, "GET", path, doc, proxy, creds);
			}
			if (httpWebResponse.StatusCode == HttpStatusCode.NotModified)
			{
				if (UsingCache)
				{
					_fromCache = true;
					if (path != null)
					{
						IOLibrary.CopyAlways(text, path);
						File.SetLastWriteTime(path, File.GetLastWriteTime(text));
					}
					return httpWebResponse.StatusCode;
				}
				throw new HtmlWebException("Server has send a NotModifed code, without cache enabled.");
			}
			Stream responseStream = httpWebResponse.GetResponseStream();
			if (responseStream != null)
			{
				if (UsingCache)
				{
					SaveStream(responseStream, text, RemoveMilliseconds(httpWebResponse.LastModified), _streamBufferSize);
					SaveCacheHeaders(httpWebRequest.RequestUri, httpWebResponse);
					if (path != null)
					{
						IOLibrary.CopyAlways(text, path);
						File.SetLastWriteTime(path, File.GetLastWriteTime(text));
					}
					if (_usingCacheAndLoad)
					{
						doc.Load(text);
					}
				}
				else
				{
					if (doc != null && flag2)
					{
						if (encoding == null)
						{
							doc.Load(responseStream, detectEncodingFromByteOrderMarks: true);
						}
						else
						{
							doc.Load(responseStream, encoding);
						}
					}
					if (doc != null && flag3)
					{
						try
						{
							if (encoding == null)
							{
								doc.Load(responseStream, detectEncodingFromByteOrderMarks: true);
							}
							else
							{
								doc.Load(responseStream, encoding);
							}
						}
						catch
						{
						}
					}
				}
				httpWebResponse.Close();
			}
			return statusCode;
		}

		private string GetCacheHeader(Uri requestUri, string name, string def)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(GetCacheHeadersPath(requestUri));
			XmlNode xmlNode = xmlDocument.SelectSingleNode("//h[translate(@n, 'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')='" + name.ToUpperInvariant() + "']");
			if (xmlNode == null)
			{
				return def;
			}
			return xmlNode.Attributes[name].Value;
		}

		private string GetCacheHeadersPath(Uri uri)
		{
			return GetCachePath(uri) + ".h.xml";
		}

		private bool IsCacheHtmlContent(string path)
		{
			string contentTypeForExtension = GetContentTypeForExtension(Path.GetExtension(path), null);
			return IsHtmlContent(contentTypeForExtension);
		}

		private bool IsHtmlContent(string contentType)
		{
			return contentType.ToLowerInvariant().StartsWith("text/html");
		}

		private bool IsGZipEncoding(string contentEncoding)
		{
			return contentEncoding.ToLowerInvariant().StartsWith("gzip");
		}

		private HtmlDocument LoadUrl(Uri uri, string method, WebProxy proxy, NetworkCredential creds)
		{
			HtmlDocument htmlDocument = new HtmlDocument();
			htmlDocument.OptionAutoCloseOnEnd = false;
			htmlDocument.OptionFixNestedTags = true;
			_statusCode = Get(uri, method, null, htmlDocument, proxy, creds);
			if (_statusCode == HttpStatusCode.NotModified)
			{
				htmlDocument.DetectEncodingAndLoad(GetCachePath(uri));
			}
			return htmlDocument;
		}

		private void SaveCacheHeaders(Uri requestUri, HttpWebResponse resp)
		{
			string cacheHeadersPath = GetCacheHeadersPath(requestUri);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml("<c></c>");
			XmlNode firstChild = xmlDocument.FirstChild;
			foreach (string header in resp.Headers)
			{
				XmlNode xmlNode = xmlDocument.CreateElement("h");
				XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("n");
				xmlAttribute.Value = header;
				xmlNode.Attributes.Append(xmlAttribute);
				xmlAttribute = xmlDocument.CreateAttribute("v");
				xmlAttribute.Value = resp.Headers[header];
				xmlNode.Attributes.Append(xmlAttribute);
				firstChild.AppendChild(xmlNode);
			}
			xmlDocument.Save(cacheHeadersPath);
		}

		public Task<HtmlDocument> LoadFromWebAsync(string url)
		{
			return LoadFromWebAsync(new Uri(url), null, null);
		}

		public Task<HtmlDocument> LoadFromWebAsync(string url, CancellationToken cancellationToken)
		{
			return LoadFromWebAsync(new Uri(url), null, null, cancellationToken);
		}

		public Task<HtmlDocument> LoadFromWebAsync(string url, Encoding encoding)
		{
			return LoadFromWebAsync(new Uri(url), encoding, null, CancellationToken.None);
		}

		public Task<HtmlDocument> LoadFromWebAsync(string url, Encoding encoding, CancellationToken cancellationToken)
		{
			return LoadFromWebAsync(new Uri(url), encoding, null, cancellationToken);
		}

		public Task<HtmlDocument> LoadFromWebAsync(string url, Encoding encoding, string userName, string password)
		{
			return LoadFromWebAsync(new Uri(url), encoding, new NetworkCredential(userName, password), CancellationToken.None);
		}

		public Task<HtmlDocument> LoadFromWebAsync(string url, Encoding encoding, string userName, string password, CancellationToken cancellationToken)
		{
			return LoadFromWebAsync(new Uri(url), encoding, new NetworkCredential(userName, password), cancellationToken);
		}

		public Task<HtmlDocument> LoadFromWebAsync(string url, Encoding encoding, string userName, string password, string domain)
		{
			return LoadFromWebAsync(new Uri(url), encoding, new NetworkCredential(userName, password, domain), CancellationToken.None);
		}

		public Task<HtmlDocument> LoadFromWebAsync(string url, Encoding encoding, string userName, string password, string domain, CancellationToken cancellationToken)
		{
			return LoadFromWebAsync(new Uri(url), encoding, new NetworkCredential(userName, password, domain), cancellationToken);
		}

		public Task<HtmlDocument> LoadFromWebAsync(string url, string userName, string password, string domain)
		{
			return LoadFromWebAsync(new Uri(url), null, new NetworkCredential(userName, password, domain), CancellationToken.None);
		}

		public Task<HtmlDocument> LoadFromWebAsync(string url, string userName, string password, string domain, CancellationToken cancellationToken)
		{
			return LoadFromWebAsync(new Uri(url), null, new NetworkCredential(userName, password, domain), cancellationToken);
		}

		public Task<HtmlDocument> LoadFromWebAsync(string url, string userName, string password)
		{
			return LoadFromWebAsync(new Uri(url), null, new NetworkCredential(userName, password), CancellationToken.None);
		}

		public Task<HtmlDocument> LoadFromWebAsync(string url, string userName, string password, CancellationToken cancellationToken)
		{
			return LoadFromWebAsync(new Uri(url), null, new NetworkCredential(userName, password), cancellationToken);
		}

		public Task<HtmlDocument> LoadFromWebAsync(string url, NetworkCredential credentials)
		{
			return LoadFromWebAsync(new Uri(url), null, credentials, CancellationToken.None);
		}

		public Task<HtmlDocument> LoadFromWebAsync(string url, NetworkCredential credentials, CancellationToken cancellationToken)
		{
			return LoadFromWebAsync(new Uri(url), null, credentials, cancellationToken);
		}

		public Task<HtmlDocument> LoadFromWebAsync(Uri uri, Encoding encoding, NetworkCredential credentials)
		{
			return LoadFromWebAsync(uri, encoding, credentials, CancellationToken.None);
		}

		public async Task<HtmlDocument> LoadFromWebAsync(Uri uri, Encoding encoding, NetworkCredential credentials, CancellationToken cancellationToken)
		{
			HtmlDocument doc = new HtmlDocument();
			HttpClientHandler httpClientHandler = new HttpClientHandler();
			if (credentials == null)
			{
				httpClientHandler.UseDefaultCredentials = true;
			}
			else
			{
				httpClientHandler.Credentials = credentials;
			}
			if (CaptureRedirect)
			{
				httpClientHandler.AllowAutoRedirect = false;
			}
			HttpClient httpClient;
			if (credentials != null || CaptureRedirect)
			{
				httpClient = new HttpClient(httpClientHandler);
				httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
			}
			else
			{
				httpClient = GetSharedHttpClient(UserAgent);
			}
			HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			_statusCode = httpResponseMessage.StatusCode;
			if (httpResponseMessage.Headers != null && httpResponseMessage.Headers.Location != null)
			{
				if (httpResponseMessage.Headers.Location.IsAbsoluteUri)
				{
					_responseUri = new Uri(httpResponseMessage.Headers.Location.AbsoluteUri);
				}
				else
				{
					_responseUri = new Uri(uri.GetLeftPart(UriPartial.Authority) + httpResponseMessage.Headers.Location);
				}
			}
			string text = string.Empty;
			if (encoding != null)
			{
				using (StreamReader streamReader = new StreamReader(await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false), encoding))
				{
					text = streamReader.ReadToEnd();
				}
			}
			else
			{
				text = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			if (PreHandleDocument != null)
			{
				PreHandleDocument(doc);
			}
			if (text != null)
			{
				doc.LoadHtml(text);
			}
			return doc;
		}

		public object CreateInstance(string htmlUrl, string xsltUrl, XsltArgumentList xsltArgs, Type type)
		{
			return CreateInstance(htmlUrl, xsltUrl, xsltArgs, type, null);
		}

		public object CreateInstance(string htmlUrl, string xsltUrl, XsltArgumentList xsltArgs, Type type, string xmlPath)
		{
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			if (xsltUrl == null)
			{
				LoadHtmlAsXml(htmlUrl, xmlTextWriter);
			}
			else if (xmlPath == null)
			{
				LoadHtmlAsXml(htmlUrl, xsltUrl, xsltArgs, xmlTextWriter);
			}
			else
			{
				LoadHtmlAsXml(htmlUrl, xsltUrl, xsltArgs, xmlTextWriter, xmlPath);
			}
			xmlTextWriter.Flush();
			XmlTextReader xmlReader = new XmlTextReader(new StringReader(stringWriter.ToString()));
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			try
			{
				return xmlSerializer.Deserialize(xmlReader);
			}
			catch (InvalidOperationException ex)
			{
				throw new Exception(ex?.ToString() + ", --- xml:" + stringWriter);
			}
		}

		public void LoadHtmlAsXml(string htmlUrl, string xsltUrl, XsltArgumentList xsltArgs, XmlTextWriter writer)
		{
			LoadHtmlAsXml(htmlUrl, xsltUrl, xsltArgs, writer, null);
		}

		public void LoadHtmlAsXml(string htmlUrl, string xsltUrl, XsltArgumentList xsltArgs, XmlTextWriter writer, string xmlPath)
		{
			if (htmlUrl == null)
			{
				throw new ArgumentNullException("htmlUrl");
			}
			HtmlDocument htmlDocument = Load(htmlUrl);
			if (xmlPath != null)
			{
				XmlTextWriter xmlTextWriter = new XmlTextWriter(xmlPath, htmlDocument.Encoding);
				htmlDocument.Save(xmlTextWriter);
				xmlTextWriter.Close();
			}
			if (xsltArgs == null)
			{
				xsltArgs = new XsltArgumentList();
			}
			xsltArgs.AddParam("url", "", htmlUrl);
			xsltArgs.AddParam("requestDuration", "", RequestDuration);
			xsltArgs.AddParam("fromCache", "", FromCache);
			XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
			xslCompiledTransform.Load(xsltUrl);
			xslCompiledTransform.Transform(htmlDocument, xsltArgs, writer);
		}
	}
}
