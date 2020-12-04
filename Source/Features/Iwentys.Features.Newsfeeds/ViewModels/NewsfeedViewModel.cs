﻿using System;
using Iwentys.Features.Newsfeeds.Entities;
using Iwentys.Features.StudentFeature.ViewModels;

namespace Iwentys.Features.Newsfeeds.ViewModels
{
    public class NewsfeedViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public string SourceLink { get; set; }

        public StudentPartialProfileDto Author { get; set; }

        public static NewsfeedViewModel Wrap(NewsfeedEntity entity)
        {
            return new NewsfeedViewModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                CreationTimeUtc = entity.CreationTimeUtc,
                SourceLink = entity.SourceLink,
                Author = new StudentPartialProfileDto(entity.Author)
            };
        }
    }
}