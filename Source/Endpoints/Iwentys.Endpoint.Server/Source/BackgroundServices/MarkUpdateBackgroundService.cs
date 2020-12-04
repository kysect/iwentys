﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Features.StudentFeature.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Iwentys.Endpoint.Server.Source.BackgroundServices
{
    public class MarkUpdateBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger _logger;

        public MarkUpdateBackgroundService(ILoggerFactory loggerFactory, IServiceProvider sp)
        {
            _sp = sp;
            _logger = loggerFactory.CreateLogger("MarkUpdateBackgroundService");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("MarkUpdateBackgroundService start");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using IServiceScope scope = _sp.CreateScope();
                    _logger.LogInformation("Execute MarkUpdateBackgroundService update");

                    var accessor = scope.ServiceProvider.GetRequiredService<DatabaseAccessor>();
                    var googleTableUpdateService = new MarkGoogleTableUpdateService(accessor.Student, accessor.SubjectActivity, _logger, ApplicationOptions.GoogleServiceToken);

                    foreach (GroupSubjectEntity g in accessor.GroupSubject.Read().ToList())
                    {
                        try
                        {
                            googleTableUpdateService.UpdateSubjectActivityForGroup(g);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, "Fail to perform MarkUpdateBackgroundService update");
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Fail to perform MarkUpdateBackgroundService update");
                }

                await Task.Delay(ApplicationOptions.DaemonUpdateInterval, stoppingToken).ConfigureAwait(false);
            }
        }
    }
}