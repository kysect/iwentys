﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Students.Entities
{
    public class StudentEntity
    {
        public StudentEntity()
        {
        }

        public StudentEntity(int id, string firstName, string middleName, string secondName) : this()
        {
            Id = id;
            FirstName = firstName;
            MiddleName = middleName;
            SecondName = secondName;
            Role = UserType.Common;
            CreationTime = DateTime.UtcNow;
            LastOnlineTime = DateTime.UtcNow;
            GuildLeftTime = DateTime.MinValue;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SecondName { get; set; }
        public UserType Role { get; set; }
        public StudentType Type { get; set; }
        public int? GroupId { get; set; }
        public string GithubUsername { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastOnlineTime { get; set; }
        public int BarsPoints { get; set; }
        public string AvatarUrl { get; set; }

        public DateTime GuildLeftTime { get; set; }

        public static StudentEntity CreateFromIsu(int id, string firstName, string secondName)
        {
            return CreateFromIsu(id, firstName, null, secondName);
        }

        public static StudentEntity CreateFromIsu(int id, string firstName, string middleName, string secondName, int? groupId = null)
        {
            return new StudentEntity(id, firstName, middleName, secondName)
            {
                GroupId = groupId
            };
        }
    }
}