﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Study.SubjectAssignments.Models;

namespace Iwentys.Endpoint.Client.Pages.Study.Subjects.Managements
{
    public partial class SubjectAssignmentSubmitJounalPage
    {
        private List<SubjectAssignmentSubmitDto> _subjectAssignmentSubmits;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _subjectAssignmentSubmits = await ClientHolder.SubjectAssignment.SearchSubjectAssignmentSubmits(SubjectId);
        }

        private void NavigateToSubmitPage(object row)
        {
            if (row is not SubjectAssignmentSubmitDto submit)
                throw new IwentysExecutionException("Something goes wrong.");

            NavigationManager.NavigateTo($"/subject/{SubjectId}/management/assignments/submits/{submit.Id}");
        }
    }
}