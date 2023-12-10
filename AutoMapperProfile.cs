using RPG.Dtos.Weapon;

namespace RPG
{
    public class AutoMapperProfile : Profile
    {
       public AutoMapperProfile()
       {
        CreateMap<Character, GetCharacterDto>(); 
        CreateMap<AddCharacterDto, Character>(); 
        CreateMap<Weapon, GetWeaponDto>();
       } 
    }
}