using AutoMapper;
using PostgreProject.Dtos.AboutDtos;
using PostgreProject.Entities;

namespace PostgreProject.Mapping
{
	public class GeneralMapping : Profile
	{
		public GeneralMapping()
		{
			CreateMap<About, ResultAboutDto>();
			CreateMap<About, RemoveAboutDto>();

			CreateMap<CreateAboutDto, About>();
			CreateMap<UpdateAboutDto, About>();

		}
	}
}
