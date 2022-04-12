﻿using CommunityToolkit.Common;

using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfAppListBindingTest
{
    public sealed class TaskResultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Task task)
            {
                return task.GetResultOrDefault();
            }

            return null;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            throw new NotImplementedException();
        }
    }
}
