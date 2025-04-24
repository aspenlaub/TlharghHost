using Aspenlaub.Net.GitHub.CSharp.Dvin.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using System.Net;
using System.Windows.Threading;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.TlharghHost.Components;
using Aspenlaub.Net.GitHub.CSharp.TlharghHost.Interfaces;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.TlharghHost;

public partial class TlharghHostWindow : IDisposable {
    private ITlharghAccessor TlharghAccessor { get; }
    private DispatcherTimer? _DispatcherTimer;
    private SynchronizationContext? UiSynchronizationContext { get; }
    private DateTime _UiThreadLastActiveAt, _StatusLastConfirmedAt;

    public TlharghHostWindow() {
        InitializeComponent();
        IContainer container = new ContainerBuilder().UseTlharghHostDvinAndPegh(Constants.TlharghHostAppId).Build();
        TlharghAccessor = new TlharghAccessor(
            container.Resolve<IDvinRepository>(),
            container.Resolve<ISimpleLogger>(),
            container.Resolve<ILogConfiguration>()
        );
        UiSynchronizationContext = SynchronizationContext.Current;
        UpdateUiThreadLastActiveAt();
    }

    public void Dispose() {
        _DispatcherTimer?.Stop();
    }

    private void OnCloseButtonClickAsync(object sender, RoutedEventArgs e) {
        Environment.Exit(0);
    }

    private async Task ConnectAndMakeTlharghRegistrationAsync() {
        IErrorsAndInfos tlharghErrorsAndInfos = await TlharghAccessor.EnsureTlharghAppIsRunningAsync();
        if (tlharghErrorsAndInfos.AnyErrors()) {
            MessageBox.Show(string.Join("\r\n", tlharghErrorsAndInfos.Errors), Properties.Resources.CouldNotConnectToTlhargh, MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }
    }

    private async void OnTlharghHostWindowLoadedAsync(object sender, RoutedEventArgs e) {
        await ConnectAndMakeTlharghRegistrationAsync();
        CreateAndStartTimer();
    }

    private void CreateAndStartTimer() {
        _DispatcherTimer = new DispatcherTimer();
        _DispatcherTimer.Tick += TlharghHostWindow_TickAsync;
        _DispatcherTimer.Interval = TimeSpan.FromSeconds(7);
        _DispatcherTimer.Start();
    }

    // ReSharper disable once AsyncVoidMethod
    private async void TlharghHostWindow_TickAsync(object? sender, EventArgs e) {
        UiSynchronizationContext!.Send(_ => UpdateUiThreadLastActiveAt(), null);
        if (_StatusLastConfirmedAt == _UiThreadLastActiveAt) { return; }

        HttpStatusCode statusCode = await TlharghAccessor.PingAsync();
        if (statusCode == HttpStatusCode.NoContent) {
            _StatusLastConfirmedAt = _UiThreadLastActiveAt;
            UiSynchronizationContext.Post(_ => ShowLastCommunicatedTimeStamp(), null);
            return;
        }

        UiSynchronizationContext.Post(_ => CommunicateCouldNotPingTlharghThenStop(statusCode), null);
    }

    private void UpdateUiThreadLastActiveAt() {
        _UiThreadLastActiveAt = DateTime.Now;
    }

    private void CommunicateCouldNotPingTlharghThenStop(HttpStatusCode statusCode) {
        string s = string.Format(Properties.Resources.CouldNotPingTlhargh, statusCode.ToString());
        MonitorBox.Text = MonitorBox.Text + (MonitorBox.Text.Length == 0 ? "" : "\r\n") + s;
    }

    private void ShowLastCommunicatedTimeStamp() {
        StatusConfirmedAt.Text = _StatusLastConfirmedAt.Year > 2000 ? _StatusLastConfirmedAt.ToLongTimeString() : "";
    }

}