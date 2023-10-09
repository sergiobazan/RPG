global using AutoMapper;
using System;

namespace RPG.Services.CharacterService
{
  public class CharacterService : ICharacterService
  {
  private readonly IMapper _mapper;
  private readonly DataContext _context;
  public CharacterService(IMapper mapper, DataContext context)
  {
    _mapper = mapper;
    _context = context;
  }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
      var character = _mapper.Map<Character>(newCharacter);
      _context.Characters.Add(character);
      await _context.SaveChangesAsync();
      serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
      return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
      try
      {
        var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
        if (character is null)
          throw new Exception($"Character with Id '{id}' not found");
        _context.Characters.Remove(character);

        await _context.SaveChangesAsync();

        serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
      }
      catch (Exception ex)
      {
        serviceResponse.Success = false;
        serviceResponse.Message = ex.Message;
      }
      return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter(int userId)
    {
      var dbCharacter = await _context.Characters.Where(c => c.User!.Id == userId).ToListAsync();
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
      {
        Data = dbCharacter.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList()
      };
      return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
      var serviceResponse = new ServiceResponse<GetCharacterDto>();
      var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
      serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
      return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
      var serviceResponse = new ServiceResponse<GetCharacterDto>();
      try
      {
        var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

        if (character is null)
          throw new Exception($"Character with Id '{updatedCharacter.Id}' not found");

        character.Name = updatedCharacter.Name;
        character.HitPoints = updatedCharacter.HitPoints;
        character.Defense = updatedCharacter.Defense;
        character.Class = updatedCharacter.Class;
        character.Strength = updatedCharacter.Strength;
        character.Intelligence = updatedCharacter.Intelligence;

        await _context.SaveChangesAsync();

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