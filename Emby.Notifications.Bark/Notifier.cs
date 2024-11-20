using MediaBrowser.Common.Net;
using MediaBrowser.Controller;
using MediaBrowser.Model.Logging;
using HttpRequestOptions = MediaBrowser.Common.Net.HttpRequestOptions;

namespace Emby.Notifications.Bark;

public class Notifier : IUserNotifier
{
    private const int RequestTimeoutMs = 10000; // 10 seconds timeout
    private readonly IServerApplicationHost _appHost;
    private readonly IHttpClient _httpClient;
    private readonly ILogger _logger;

    public Notifier(ILogger logger, IServerApplicationHost applicationHost, IHttpClient httpClient)
    {
        _logger = logger;
        _appHost = applicationHost;
        _httpClient = httpClient;
    }

    private Plugin Plugin => _appHost.Plugins.OfType<Plugin>().First();

    public string Name => Plugin.StaticName;

    public string Key => "barknotifications";

    public string SetupModuleUrl => Plugin.NotificationSetupModuleUrl;

    public async Task SendNotification(InternalNotificationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var options = request.Configuration.Options;
            var barkServerUrl = GetBarkServerUrlFromConfig();
            options.TryGetValue("Params", out var extrasParams);
            if (string.IsNullOrEmpty(barkServerUrl))
                if (!options.TryGetValue("ServerUrl", out barkServerUrl))
                {
                    _logger.Error("Bark notification failed: ServerUrl is required");
                    return;
                }

            if (!options.TryGetValue("DeviceKey", out var deviceKey))
            {
                _logger.Error("Bark notification failed: DeviceKey is required");
                return;
            }

            if (!Uri.TryCreate(barkServerUrl, UriKind.Absolute, out var baseUri))
            {
                _logger.Error($"Bark notification failed: Invalid server URL: {barkServerUrl}");
                return;
            }

            var title = Uri.EscapeDataString(request.Title ?? "Notification");
            var description = Uri.EscapeDataString(request.Description ?? string.Empty);
            var query = string.Empty;
            if (!string.IsNullOrEmpty(extrasParams)) query = extrasParams.TrimStart('?');
            var barkUrl = $"{barkServerUrl.TrimEnd('/')}/{deviceKey}/{title}/{description}";
            if (!string.IsNullOrEmpty(query)) barkUrl += $"?{query}";

            _logger.Info($"Sending Bark notification to: {barkUrl}");

            var httpRequest = new HttpRequestOptions
            {
                Url = barkUrl,
                TimeoutMs = RequestTimeoutMs,
                CancellationToken = cancellationToken
            };

            using var response = await _httpClient.Post(httpRequest).ConfigureAwait(false);
            _logger.Info($"Bark notification sent successfully: {request.Title}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Failed to send Bark notification: {ex.Message}");
            throw;
        }
    }

    private string GetBarkServerUrlFromConfig()
    {
        try
        {
            var serverUrl = Environment.GetEnvironmentVariable("BARK_SERVER_URL");
            if (!string.IsNullOrEmpty(serverUrl))
            {
                _logger.Info($"Using Bark server URL from environment: {serverUrl}");
                return serverUrl;
            }

            return string.Empty;
        }
        catch (Exception ex)
        {
            _logger.Error($"Failed to get Bark server URL from config: {ex.Message}");
            return string.Empty;
        }
    }
}