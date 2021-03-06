﻿using System.Collections.Generic;

namespace Iwentys.Infrastructure.Application.Controllers.SubjectAssignments.Dtos
{
    public class SubjectAssignmentJournalItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<SubjectAssignmentDto> Assignments { get; set; }
    }
}