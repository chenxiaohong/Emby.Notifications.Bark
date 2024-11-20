using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Drawing;
using MediaBrowser.Model.Plugins;

namespace Emby.Notifications.Bark;

public class Plugin : BasePlugin, IHasWebPages, IHasThumbImage, IHasTranslations
{
    private const string EditorJsName = "barknotificationeditorjs";

    public static string StaticName = "Bark";

    private readonly Guid _id = new("f2a5e216-a803-4b4d-a670-9a0f24a5a50d");

    public string NotificationSetupModuleUrl => GetPluginPageUrl(EditorJsName);

    public override string Description => "Sends notifications via Bark Service.";

    public override Guid Id => _id;

    /// <summary>
    ///     Gets the name of the plugin
    /// </summary>
    /// <value>The name.</value>
    public override string Name => StaticName + " Notifications";

    public Stream GetThumbImage()
    {
        var type = GetType();
        var resourceStream = type.Assembly.GetManifestResourceStream(type.Namespace + ".thumb.png");
        return resourceStream ?? throw new FileNotFoundException("Thumb image resource not found");
    }

    public ImageFormat ThumbImageFormat => ImageFormat.Png;

    public TranslationInfo[] GetTranslations()
    {
        var basePath = GetType().Namespace + ".strings.";

        return GetType()
            .Assembly
            .GetManifestResourceNames()
            .Where(i => i.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
            .Select(i => new TranslationInfo
            {
                Locale = Path.GetFileNameWithoutExtension(i.Substring(basePath.Length)),
                EmbeddedResourcePath = i
            }).ToArray();
    }

    public IEnumerable<PluginPageInfo> GetPages()
    {
        return new[]
        {
            new PluginPageInfo
            {
                Name = EditorJsName,
                EmbeddedResourcePath = GetType().Namespace + ".Configuration.entryeditor.js"
            },
            new PluginPageInfo
            {
                Name = "barkeditortemplate",
                EmbeddedResourcePath = GetType().Namespace + ".Configuration.entryeditor.template.html",
                IsMainConfigPage = false
            }
        };
    }
}