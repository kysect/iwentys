﻿using System;
using System.Linq.Expressions;
using Iwentys.Domain.Enums;

namespace Iwentys.Domain
{
    public class CompanyWorker
    {
        public int CompanyId { get; init; }
        public virtual Company Company { get; init; }

        public int WorkerId { get; init; }
        public virtual IwentysUser Worker { get; init; }

        public CompanyWorkerType Type { get; private set; }
        public int? ApprovedById { get; private set; }
        public virtual IwentysUser ApprovedBy { get; init; }

        public static Expression<Func<CompanyWorker, bool>> IsRequested => worker => worker.Type == CompanyWorkerType.Requested;

        public static CompanyWorker NewRequest(Company company, IwentysUser worker)
        {
            return new CompanyWorker
            {
                Company = company,
                Worker = worker,
                Type = CompanyWorkerType.Requested
            };
        }

        public void Approve(SystemAdminUser systemAdminUser)
        {
            ApprovedById = systemAdminUser.User.Id;
            Type = CompanyWorkerType.Accepted;
        }
    }
}