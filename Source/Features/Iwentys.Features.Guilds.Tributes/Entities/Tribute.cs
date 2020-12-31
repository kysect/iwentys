﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Common.Exceptions;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Tributes.Enums;
using Iwentys.Features.Guilds.Tributes.Models;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Tributes.Entities
{
    public class Tribute
    {
        public Tribute()
        {
        }

        public Tribute(Guild guild, GithubProject project) : this()
        {
            GuildId = guild.Id;
            ProjectId = project.Id;
            State = TributeState.Active;
            CreationTimeUtc = DateTime.UtcNow;
        }

        [Key]
        public long ProjectId { get; init; }

        [ForeignKey("ProjectId")]
        public virtual GithubProject Project { get; init; }

        public int GuildId { get; init; }
        public virtual Guild Guild { get; init; }

        public int? MentorId { get; private set; }
        public virtual Student Mentor { get; private set; }
        
        public TributeState State { get; private set; }
        public int? DifficultLevel { get; private set; }
        public int? Mark { get; private set; }
        public string Comment { get; private set; }
        public DateTime CreationTimeUtc { get; init; }

        public static Tribute Create(Guild guild, Student student, GithubProject project, List<Tribute> allTributes)
        {
            if (student.GithubUsername != project.Owner)
                throw InnerLogicException.TributeExceptions.TributeCanBeSendFromStudentAccount(student.Id, project.Owner);

            if (allTributes.Any(t => t.ProjectId == project.Id))
                throw InnerLogicException.TributeExceptions.ProjectAlreadyUsed(project.Id);

            //TODO: fix NRE t.Project.StudentId
            //if (allTributes.Any(t => t.State == TributeState.Active && t.Project.StudentId == student.Id))
            //    throw InnerLogicException.Tribute.UserAlreadyHaveTribute(student.Id);
            
            var tribute = new Tribute(guild, project);
            return tribute;
        }

        public void SetCanceled()
        {
            if (State != TributeState.Active)
                throw InnerLogicException.TributeExceptions.IsNotActive(ProjectId);

            State = TributeState.Canceled;
        }

        public void SetCompleted(int mentorId, TributeCompleteRequest tributeCompleteRequest)
        {
            if (State != TributeState.Active)
                throw InnerLogicException.TributeExceptions.IsNotActive(this.ProjectId);

            MentorId = mentorId;
            DifficultLevel = tributeCompleteRequest.DifficultLevel;
            Mark = tributeCompleteRequest.Mark;
            Comment = tributeCompleteRequest.Comment;
            State = TributeState.Completed;
        }

        public static Expression<Func<Tribute, bool>> IsActive => tribute => tribute.State == TributeState.Active;
        public static Expression<Func<Tribute, bool>> BelongTo(Student student) => tribute => tribute.Project.StudentId == student.Id;
    }
}