global using AutoMapper;
using System;

namespace RPG.Services.CharacterService
{
  public class CharacterService : ICharacterService
  {
  private static readonly List<Character> characters = new(){
    new Character(),
    new Character { Id = 1, Name = "Sam" }
  };
  private readonly IMapper _mapper;

  public CharacterService(IMapper mapper)
  {
    _mapper = mapper;
  }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
      var character = _mapper.Map<Character>(newCharacter);
      character.Id = characters.Max(c => c.Id) + 1;
      characters.Add(character);
      serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
      return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
      try
      {
        var character = characters.First(c => c.Id == id);
        characters.Remove(character);
        serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
      }
      catch (Exception ex)
      {
        serviceResponse.Success = false;
        serviceResponse.Message = ex.Message;
      }
      return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter()
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
      {
        Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList()
      };
      return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
      var serviceResponse = new ServiceResponse<GetCharacterDto>();
      var character = characters.FirstOrDefault(c => c.Id == id);
      serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
      return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
      var serviceResponse = new ServiceResponse<GetCharacterDto>();
      try
      {
        var character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);

        if (character is null)
          throw new Exception($"Character with Id '{updatedCharacter.Id}' not found");

        character.Name = updatedCharacter.Name;
        character.HitPoints = updatedCharacter.HitPoints;
        character.Defense = updatedCharacter.Defense;
        character.Class = updatedCharacter.Class;
        character.Strength = updatedCharacter.Strength;
        character.Intelligence = updatedCharacter.Intelligence;

        serviceResponse.Data =_mapper.Map<GetCharacterDto>(character);
      }
      catch (Exception ex)
      {
        serviceResponse.Success = false;
        serviceResponse.Message = ex.Message;
      }
      return serviceResponse;
    }
  }
}