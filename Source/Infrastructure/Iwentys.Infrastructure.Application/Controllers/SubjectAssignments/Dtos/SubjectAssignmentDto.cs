﻿using System;
using System.Collections.Generic;
using Iwentys.Domain.AccountManagement.Dto;

namespace Iwentys.Infrastructure.Application.Controllers.SubjectAssignments.Dtos
{
    public class SubjectAssignmentDto
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public IwentysUserInfoDto Author { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public DateTime LastUpdateTimeUtc { get; set; }
        public DateTime? DeadlineTimeUtc { get; set; }
        public int Position { get; set; }
        public bool AvailableForStudent { get; set; }

        public List<SubjectAssignmentSubmitDto> Submits { get; set; }
    }
}