﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

using DomainApplication = EPXOS.Domain.Objects.Application;
using ApplicationModel = SocNetEPX.TemplateModels.Application.Application;

namespace SocNetEPX.TemplateModels.Application
{
    public class ApplicationMappings
    {
        public static void SetUpMappings()
        {
            Mapper.CreateMap<DomainApplication, ApplicationModel>()
                .ForMember(src => src.Name, opt => opt.MapFrom(src => src.ApplicationName))
                .ForMember(src => src.Url, opt => opt.MapFrom(src => src.ApplicationUri));
        }
    }
}