using System;
using AutoMapper;
using TodoApiMediatR.Demo.Api.Infrastructure.Data;

namespace TodoApiMediatR.Demo.Api.Features.Todos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoItem, ListAll.Result>()
                .ForMember(destination => destination.Id, options => options.MapFrom(source => source.Id))
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Name))
                .ForMember(destination => destination.IsComplete, options => options.MapFrom(source => source.IsComplete))
                ;

            CreateMap<TodoItem, GetItemById.Result>()
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Name))
                .ForMember(destination => destination.IsComplete, options => options.MapFrom(source => source.IsComplete))
                ;

            CreateMap<CreateItem.Command, TodoItem>()
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Name))
                ;
            CreateMap<TodoItem, CreateItem.Result>()
                .ForMember(destination => destination.Id, options => options.MapFrom(source => source.Id))
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Name))
                .ForMember(destination => destination.IsComplete, options => options.MapFrom(source => source.IsComplete))
                ;

            CreateMap<TodoItem, DeleteItem.Result>()
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Name))
                .ForMember(destination => destination.IsComplete, options => options.MapFrom(source => source.IsComplete))
                ;

            CreateMap<UpdateItem.Command, TodoItem>()
                .ForMember(destination => destination.Id, options => options.MapFrom(source => source.Id))
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.ItemToUpdate.Name))
                .ForMember(destination => destination.IsComplete, options => options.MapFrom(source => source.ItemToUpdate.IsComplete))
                ;
            CreateMap<TodoItem, UpdateItem.Result>()
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Name))
                .ForMember(destination => destination.IsComplete, options => options.MapFrom(source => source.IsComplete))
                ;
        }
    }
}
