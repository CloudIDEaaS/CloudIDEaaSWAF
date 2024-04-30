using Accord.Statistics.Kernels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static alglib;

namespace Utils
{
    public class Spline2d
    {
        public double[] XValues { get; }
        public double[] YValues { get; }
        private barycentricinterpolant interpolant;

        public Spline2d(double[] xValues, double[] yValues, barycentricinterpolant barycentricinterpolant)
        {
            XValues = xValues;
            YValues = yValues;
            interpolant = barycentricinterpolant;
        }

        public double this[double x]
        {
            get
            {
                return barycentriccalc(interpolant, x);
            }
        }
    }

    public static class SplineExtensions
    {
        public static Spline2d CreateSpline<TSource>(this IEnumerable<TSource> input, Func<TSource, double> xSelector, Func<TSource, double> ySelector)
        {
            var barycentricinterpolant = new barycentricinterpolant();
            var xValues = input.Select(i => xSelector(i)).ToArray();
            var yValues = input.Select(i => ySelector(i)).ToArray();

            polynomialbuild(xValues, yValues, xValues.Length, out barycentricinterpolant);

            return new Spline2d(xValues, yValues, barycentricinterpolant);
        }
    }
}
