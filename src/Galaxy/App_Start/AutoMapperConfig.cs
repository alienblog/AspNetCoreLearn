using AutoMapper;
using Galaxy.Infrastructure.Mappings;

namespace Galaxy
{
    public class AutoMapperConfig
    {
	    public static void Configure()
	    {
		    Mapper.Initialize(x =>
		    {
			    x.AddProfile<DomainToViewModelMappingProfile>();
		    });
	    }
    }
}
