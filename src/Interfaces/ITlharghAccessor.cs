using System.Net;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.TlharghHost.Interfaces;

public interface ITlharghAccessor {
    Task<IErrorsAndInfos> EnsureTlharghAppIsRunningAsync();
    Task<HttpStatusCode> PingAsync();
}
