using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AeFinder.Cli;

[DependsOn(
    typeof(AeFinderCliCoreModule),
    typeof(AbpAutofacModule)
)]
public class AeFinderCliModule: AbpModule
{

}