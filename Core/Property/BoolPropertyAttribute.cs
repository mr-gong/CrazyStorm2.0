﻿/*
 * The MIT License (MIT)
 * Copyright (c) StarX 2016 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrazyStorm.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    sealed class BoolPropertyAttribute : PropertyAttribute
    {
        public override bool IsLegal(string newValue, out object value)
        {
            value = null;
            bool typeValue;
            bool result = bool.TryParse(newValue, out typeValue);
            if (result)
            {
                value = typeValue;
                return true;
            }
            return false;
        }
    }
}
