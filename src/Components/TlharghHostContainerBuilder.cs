using Aspenlaub.Net.GitHub.CSharp.Dvin.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.TlharghHost.Components;

public static class TlharghHostContainerBuilder {
    public static ContainerBuilder UseTlharghHostDvinAndPegh(this ContainerBuilder builder, string applicationName) {
        builder.UseDvinAndPegh(applicationName, new DummyCsArgumentPrompter());
        return builder;
    }
}
