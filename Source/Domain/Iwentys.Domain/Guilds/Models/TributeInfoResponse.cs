﻿using System;
using System.Linq.Expressions;
using Iwentys.Domain.AccountManagement.Dto;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Study.Models;

namespace Iwentys.Domain.Guilds.Models
{
    public class TributeInfoResponse
    {
        public StudentProjectInfoResponse Project { get; set; }

        public int GuildId { get; set; }

        public TributeState State { get; set; }
        public int? DifficultLevel { get; set; }
        public int? Mark { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public DateTime LastUpdateTimeUtc { get; private set; }

        public IwentysUserInfoDto Mentor { get; set; }

        public static Expression<Func<Tribute, TributeInfoResponse>> FromEntity =>
            project =>
                new TributeInfoResponse
                {
                    Project = new StudentProjectInfoResponse
                    {
                        Id = project.Project.Id,
                        Url = project.Project.FullUrl,
                        Name = project.Project.FullUrl,
                        Description = project.Project.Description
                    },

                    GuildId = project.GuildId,
                    State = project.State,
                    DifficultLevel = project.DifficultLevel,
                    Mark = project.Mark,
                    CreationTimeUtc = project.CreationTimeUtc,
                    LastUpdateTimeUtc = project.LastUpdateTimeUtc,
                    Mentor = project.Mentor == null ? null : new IwentysUserInfoDto(project.Mentor)
                };

        public static TributeInfoResponse Wrap(Tribute project)
        {
            return new TributeInfoResponse
            {
                Project = StudentProjectInfoResponse.Wrap(project.Project),
                GuildId = project.GuildId,
                State = project.State,
                DifficultLevel = project.DifficultLevel,
                Mark = project.Mark,
                CreationTimeUtc = project.CreationTimeUtc,
                LastUpdateTimeUtc = project.LastUpdateTimeUtc,
                Mentor = project.Mentor is null ? null : new IwentysUserInfoDto(project.Mentor)
            };
        }
    }
}