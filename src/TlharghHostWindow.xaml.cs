using Aspenlaub.Net.GitHub.CSharp.TlharghHost.Components;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.TlharghHost;
/// <summary>
/// Interaction logic for TlharghHostWindow.xaml
/// </summary>
public partial class TlharghHostWindow {
    public TlharghHostWindow() {
        InitializeComponent();
        IContainer _ = new ContainerBuilder().UseTlharghHostDvinAndPegh("TlharghHost").Build();
    }
}