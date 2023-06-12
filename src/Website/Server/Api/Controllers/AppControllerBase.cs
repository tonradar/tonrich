namespace Tonrich.Server.Api.Controllers;

public partial class AppControllerBase : ControllerBase
{
    [AutoInject] protected AppSettings AppSettings = default!;

    [AutoInject] protected IMapper Mapper = default!;

    [AutoInject] protected IStringLocalizer<AppStrings> Localizer = default!;

}
