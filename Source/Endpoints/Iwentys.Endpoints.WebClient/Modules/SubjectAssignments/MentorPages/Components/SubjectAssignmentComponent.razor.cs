﻿namespace Iwentys.Endpoints.WebClient.Modules.SubjectAssignments.MentorPages.Components
{
    public partial class SubjectAssignmentComponent
    {
        private string LinkToSubjectAssignmentCreate(int subjectId) => $"/subject/assignment-management/{subjectId}/create";
        private string LinkToSubjectAssignmentUpdate(int subjectId) => $"/subject/assignment-management/{subjectId}/update";
        private string LinkToSubjectAssignmentSubmitJournal(int subjectId) => $"/subject/assignment-management/{subjectId}/submits";
    }
}