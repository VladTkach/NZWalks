﻿using AutoMapper;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;

namespace NZWalks.Mappings;

public class AutoMapperProfiles: Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Region, RegionDto>().ReverseMap();
        CreateMap<AddRegionRequestDto, Region>().ReverseMap();
        CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();

        CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
        CreateMap<WalkDto, Walk>().ReverseMap();
        CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();

        CreateMap<Difficulty, DifficultyDto>().ReverseMap();
    }
}