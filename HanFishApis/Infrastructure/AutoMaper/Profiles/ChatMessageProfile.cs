using AutoMapper;
using Entities.Message;
using Models.Message;
using Utils.Enums;

namespace HanFishApis.Infrastructure.AutoMaper.Profiles
{
    public class ChatMessageProfile : Profile
    {
        public ChatMessageProfile()
        {
            CreateMap<ChatMessage, ChatMessageModel>()
                .ForMember(m => m.Type, opt => opt.MapFrom(c => (int)c.Type));
            CreateMap<ChatMessage, ChatMessageDetailModel>()
                .ForMember(m => m.Type, opt => opt.MapFrom(c => (int)c.Type))
                .ForMember(m => m.TypeName, opt => opt.MapFrom(c => c.Type.ToString()));
            CreateMap<AddChatMessageModel, ChatMessage>()
                .ForMember(c => c.Type, opt => opt.MapFrom(m => (ChatMessageTypes)m.type)); ;
        }
    }
}
