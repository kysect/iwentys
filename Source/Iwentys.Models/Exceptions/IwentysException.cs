﻿using System;

namespace Iwentys.Models.Exceptions
{
    public class IwentysException : Exception
    {
        public IwentysException(string message) : base(message)
        {
        }

        public IwentysException() : base()
        {
        }

        public IwentysException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}