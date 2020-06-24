﻿using System;

namespace Iwentys.Core.Tools
{
    public static class Monads
    {
        public static TResult Maybe<TValue, TResult>(this TValue value, Func<TValue, TResult> just) where TValue : class
        {
            return value == null ? default : just(value);
        }

        public static TResult To<TValue, TResult>(this TValue value, Func<TValue, TResult> convertor) where TValue : class
        {
            return convertor(value);
        }
    }
}