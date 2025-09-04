using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfernalEclipseAPI.Common.Tools.Easings
{
    public class PolynomialEasing : EasingCurve
    {
        /// <summary>
        /// A polynomial easing curve of degree 2.
        /// </summary>
        public static readonly PolynomialEasing Quadratic = new(2f);

        /// <summary>
        /// A polynomial easing curve of degree 3.
        /// </summary>
        public static readonly PolynomialEasing Cubic = new(3f);

        /// <summary>
        /// A polynomial easing curve of degree 4.
        /// </summary>
        public static readonly PolynomialEasing Quartic = new(4f);

        /// <summary>
        /// A polynomial easing curve of degree 5.
        /// </summary>
        public static readonly PolynomialEasing Quintic = new(5f);

        /// <summary>
        /// A polynomial easing curve of degree 6.
        /// </summary>
        public static readonly PolynomialEasing Sextic = new(6f);

        public PolynomialEasing(float exponent)
        {
            InCurve = new(interpolant =>
            {
                return MathF.Pow(interpolant, exponent);
            });
            OutCurve = new(interpolant =>
            {
                return 1f - MathF.Pow(1f - interpolant, exponent);
            });
            InOutCurve = new(interpolant =>
            {
                if (interpolant < 0.5f)
                    return MathF.Pow(2f, exponent - 1f) * MathF.Pow(interpolant, exponent);
                return 1f - MathF.Pow(interpolant * -2f + 2f, exponent) * 0.5f;
            });
        }
    }
}
