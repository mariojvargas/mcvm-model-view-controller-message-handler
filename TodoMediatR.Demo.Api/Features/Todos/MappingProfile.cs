using System;
using AutoMapper;
using TodoApiMediatR.Demo.Api.Infrastructure.Data;

namespace TodoApiMediatR.Demo.Api.Features.Todos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoItem, TodoListItemDto>()
                .ForMember(destination => destination.Id, options => options.MapFrom(source => source.Id))
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Name))
                .ForMember(destination => destination.IsComplete, options => options.MapFrom(source => source.IsComplete))
                ;

            CreateMap<TodoItem, TodoItemDto>()
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Name))
                .ForMember(destination => destination.IsComplete, options => options.MapFrom(source => source.IsComplete))
                ;

            CreateMap<CreateItem.Command, TodoItem>()
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Name))
                ;
            CreateMap<TodoItem, CreatedTodoItemDto>()
                .ForMember(destination => destination.Id, options => options.MapFrom(source => source.Id))
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Name))
                .ForMember(destination => destination.IsComplete, options => options.MapFrom(source => source.IsComplete))
                ;

            CreateMap<TodoItem, DeletedTodoItemDto>()
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Name))
                .ForMember(destination => destination.IsComplete, options => options.MapFrom(source => source.IsComplete))
                ;

            CreateMap<UpdateItem.Command, TodoItem>()
                .ForMember(destination => destination.Id, options => options.MapFrom(source => source.Id))
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Dto.Name))
                .ForMember(destination => destination.IsComplete, options => options.MapFrom(source => source.Dto.IsComplete))
                ;
            CreateMap<TodoItem, ConfirmedUpdatedTodoItemDto>()
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Name))
                .ForMember(destination => destination.IsComplete, options => options.MapFrom(source => source.IsComplete))
                ;
        }
    }
}
