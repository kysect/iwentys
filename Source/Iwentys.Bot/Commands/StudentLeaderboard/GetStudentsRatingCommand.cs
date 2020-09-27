﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using Iwentys.ClientBot.Tools;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Study;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Core;

namespace Iwentys.ClientBot.Commands.StudentLeaderboard
{
    public class GetStudentsRatingCommand : IBotCommand
    {
        private readonly IwentysApiProvider _iwentysApi;

        public GetStudentsRatingCommand(IwentysApiProvider iwentysApi)
        {
            _iwentysApi = iwentysApi;
        }

        public Result CanExecute(CommandArgumentContainer args)
        {
            if (args.Arguments.Count != Args.Length)
                return Result.Fail("Wrong argument count");

            if (!int.TryParse(args.Arguments[0], out _))
                return Result.Fail("Argument must be int value (courseId)");

            return Result.Ok();
        }

        public async Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            ICollection<StudyLeaderboardRow> studyLeaderboardRows = await _iwentysApi.Client.ApiStudyleaderboardAsync(null, int.Parse(args.Arguments[0]), null, null).ConfigureAwait(false);

            return ResultFormatter.FormatToResult(studyLeaderboardRows.Take(20));
        }

        public string CommandName { get; } = nameof(GetStudentsRatingCommand);
        public string Description { get; } = nameof(GetStudentsRatingCommand);
        public string[] Args { get; } = {"CourseId"};
    }
}