## Emby Bark Notifier Plugin

### 功能描述
这是一个Emby媒体服务器的Bark通知插件，可以将Emby的媒体库更新、系统事件等通过Bark Api推送通知到App。
解决Emby Webhooks标题问题和不能自定义参数的问题。

### 安装步骤
1. 下载插件文件，建议重命名为`Emby.Notifications.Bark.dll`
2. 将插件放入Emby的插件目录
3. 重启Emby服务器
4. 在`通知`中点击`添加通知`，下拉列表中选择`Bark`
5. 在插件配置页面设置Bark服务器地址和设备Key

### 配置
- **Bark服务器URL**：你的Bark服务器地址，通常是 `https://api.day.app/`
- **设备Key**：你的Bark设备唯一标识符

### 支持的通知类型
- 媒体库更新
- 新媒体入库
- 系统状态变更
- 其它未测试

### 环境要求
- Emby Server 4.7.0 及以上
- .NET 6.0 运行时

### 开发
- 语言：C#
- 框架：.NET 6.0
- 依赖：MediaBrowser.Server.Core
