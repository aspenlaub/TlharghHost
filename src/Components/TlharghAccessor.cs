using System.Net;
using Aspenlaub.Net.GitHub.CSharp.Dvin.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TlharghHost.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.TlharghHost.Components;

public class TlharghAccessor(IDvinRepository dvinRepository, ISimpleLogger simpleLogger, ILogConfiguration logConfiguration) : ITlharghAccessor {
    public async Task<IErrorsAndInfos> EnsureTlharghAppIsRunningAsync() {
        return await Task.FromResult(new ErrorsAndInfos());
    }

    public async Task<HttpStatusCode> PingAsync() {
        return await Task.FromResult(HttpStatusCode.NotFound);
    }
}
