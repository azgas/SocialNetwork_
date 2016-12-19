using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

using DomainUser = EPXOS.Domain.Objects.User;
using UserModel = EPX_Template1.TemplateModels.User.User;

namespace EPX_Template1.TemplateModels.User
{
    public class UserMappings
    {
        public static void SetUpMappings()
        {
            Mapper.CreateMap<DomainUser, UserModel>();

            Mapper.CreateMap<UserModel, DomainUser>()
                .ForMember(dest => dest.UserName, opt => opt.UseValue(null));
        }
    }
}