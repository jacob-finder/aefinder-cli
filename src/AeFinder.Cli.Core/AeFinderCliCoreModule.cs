using Volo.Abp.Domain;
using Volo.Abp.Http;
using Volo.Abp.IdentityModel;
using Volo.Abp.Json;
using Volo.Abp.Minify;
using Volo.Abp.Modularity;

namespace AeFinder.Cli;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpJsonModule),
    typeof(AbpIdentityModelModule),
    typeof(AbpMinifyModule),
    typeof(AbpHttpModule)
)]
public class AeFinderCliCoreModule: AbpModule
{

}