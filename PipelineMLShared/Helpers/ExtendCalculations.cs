﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    /// <summary>
    /// Collection of extension methods to assist in calculations
    /// </summary>
    public static class ExtendCalculations
    {

        public static double RootMeanSquareDeviation(this double[] x)
        {
            double sum = 0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += (x[i] * x[i]);
            }
            return Math.Sqrt(sum / x.Length);
        }

        public static double MeanSquareError(this double[] x)
        {
            double sum = 0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += (x[i] * x[i]);
            }
            return sum / x.Length;
        }


        public static DateTime NextBusinessDay(this DateTime currDate)
        {
            var newDate = currDate.AddDays(1);
            if (newDate.DayOfWeek == DayOfWeek.Saturday) { newDate = newDate.AddDays(2); }
            if (newDate.DayOfWeek == DayOfWeek.Sunday) { newDate = newDate.AddDays(1); }
            return newDate;
        }


        public static double StandardDeviation(this IEnumerable<decimal> values)
        {
            decimal avg = values.Average();
            return Math.Sqrt(values.Average(v => Math.Pow((double)(v - avg), 2)));
        }


    }
}
