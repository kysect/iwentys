﻿using System;
using Iwentys.Models.Types;

namespace Iwentys.Models.Entities
{
    public class Quest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime CreationTime { get; set; }
        public QuestState State { get; set; }


        public int AuthorId { get; set; }
        public Student Author { get; set; }

        public static Quest New(string title, string description, int price, Student author)
        {
            return new Quest
            {
                Title = title,
                Description = description,
                Price = price,
                CreationTime = DateTime.UtcNow,
                State = QuestState.Active,
                AuthorId = author.Id
            };
        }
    }
}