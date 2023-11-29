using Tonrich.Client.Services;
using Microsoft.AspNetCore.Components;

namespace Tonrich.Server.Components;

[StreamRendering(enabled: true)]
public partial class App
{
    [CascadingParameter] HttpContext HttpContext { get; set; } = default!;
}
