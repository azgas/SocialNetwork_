using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

using DomainUser = EPXOS.Domain.Objects.User;
using UserModel = EPXSocNet.TemplateModels.User.User;

namespace EPXSocNet.TemplateModels.User
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