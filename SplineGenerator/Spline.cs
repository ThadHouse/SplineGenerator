using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SplineGenerator
{
    class Spline
    {
        public enum Type { CubicHermite, QuinticHermite }

        double _a;  // ax^5
        double _b;  // + bx^4
        double _c;  // + cx^3
        double _d;  // + dx^2
        double _e;  // + ex

        double _yOffset;
        double _xOffset;
        double _knotDistance;
        double _thetaOffset;
        double _arcLength;

        public Spline()
        {
            _arcLength = -1;
        }

        private static bool AlmostEqual(double x, double y)
        {
            return Math.Abs(x - y) < 1E-6;
        }

        public static bool ReticulateSplines(Waypoint start,
          Waypoint goal, Spline result, Type type)
        {
            return ReticulateSplines(start.X, start.Y, start.Theta, goal.X, goal.Y,
                    goal.Theta, result, type);
        }


        public static bool ReticulateSplines(double x0, double y0, double theta0,
            double x1, double y1, double theta1, Spline result, Type type)
        {
            Console.WriteLine("Reticulating splines...");

            // Transform x to the origin
            result._xOffset = x0;
            result._yOffset = y0;f0
            double x1Hat = Math.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0));
            if (x1Hat == 0)
            {
                return false;
            }
            result._knotDistance = x1Hat;
            result._thetaOffset = Math.Atan2(y1 - y0, x1 - x0);
            double theta0Hat = ChezyMath.GetDifferenceInAngleRadians(
                    result._thetaOffset, theta0);
            double theta1Hat = ChezyMath.GetDifferenceInAngleRadians(
                    result._thetaOffset, theta1);
            // We cannot handle vertical slopes in our rotated, translated basis.
            // This would mean the user wants to end up 90 degrees off of the straight
            // line between p0 and p1.
            if (AlmostEqual(Math.Abs(theta0Hat), Math.PI / 2)
                    || AlmostEqual(Math.Abs(theta1Hat), Math.PI / 2))
            {
                return false;
            }
            // We also cannot handle the case that the end angle is facing towards the
            // start angle (total turn > 90 degrees).
            if (Math.Abs(ChezyMath.GetDifferenceInAngleRadians(theta0Hat,
                    theta1Hat))
                    >= Math.PI / 2)
            {
                return false;
            }
            // Turn angles into derivatives (slopes)
            double yp0Hat = Math.Tan(theta0Hat);
            double yp1Hat = Math.Tan(theta1Hat);

            if (type == Type.CubicHermite)
            {
                // Calculate the cubic spline coefficients
                result._a = 0;
                result._b = 0;
                result._c = (yp1Hat + yp0Hat) / (x1Hat * x1Hat);
                result._d = -(2 * yp0Hat + yp1Hat) / x1Hat;
                result._e = yp0Hat;
            }
            else if (type == Type.QuinticHermite)
            {
                result._a = -(3 * (yp0Hat + yp1Hat)) / (x1Hat * x1Hat * x1Hat * x1Hat);
                result._b = (8 * yp0Hat + 7 * yp1Hat) / (x1Hat * x1Hat * x1Hat);
                result._c = -(6 * yp0Hat + 4 * yp1Hat) / (x1Hat * x1Hat);
                result._d = 0;
                result._e = yp0Hat;
            }

            return true;
        }
        public double CalculateLength()
        {
            if (_arcLength >= 0)
            {
                return _arcLength;
            }

            const int kNumSamples = 100000;
            double arcLength = 0;
            double t, dydt;
            double integrand, lastIntegrand
                    = Math.Sqrt(1 + DerivativeAt(0) * DerivativeAt(0)) / kNumSamples;
            for (int i = 1; i <= kNumSamples; ++i)
            {
                t = ((double)i) / kNumSamples;
                dydt = DerivativeAt(t);
                integrand = Math.Sqrt(1 + dydt * dydt) / kNumSamples;
                arcLength += (integrand + lastIntegrand) / 2;
                lastIntegrand = integrand;
            }
            _arcLength = _knotDistance * arcLength;
            return _arcLength;
        }

        public double GetPercentageForDistance(double distance)
        {
            const int kNumSamples = 100000;
            double arcLength = 0;
            double t = 0;
            double lastArcLength = 0;
            double dydt;
            double integrand, lastIntegrand
                    = Math.Sqrt(1 + DerivativeAt(0) * DerivativeAt(0)) / kNumSamples;
            distance /= _knotDistance;
            for (int i = 1; i <= kNumSamples; ++i)
            {
                t = ((double)i) / kNumSamples;
                dydt = DerivativeAt(t);
                integrand = Math.Sqrt(1 + dydt * dydt) / kNumSamples;
                arcLength += (integrand + lastIntegrand) / 2;
                if (arcLength > distance)
                {
                    break;
                }
                lastIntegrand = integrand;
                lastArcLength = arcLength;
            }

            // Interpolate between samples.
            double interpolated = t;
            if (arcLength != lastArcLength)
            {
                interpolated += ((distance - lastArcLength)
                        / (arcLength - lastArcLength) - 1) / kNumSamples;
            }
            return interpolated;
        }

        public double[] GetXandY(double percentage)
        {
            double[] result = new double[2];

            percentage = Math.Max(Math.Min(percentage, 1), 0);
            double xHat = percentage * _knotDistance;
            double yHat = (_a * xHat + _b) * xHat * xHat * xHat * xHat
                    + _c * xHat * xHat * xHat + _d * xHat * xHat + _e * xHat;

            double cosTheta = Math.Cos(_thetaOffset);
            double sinTheta = Math.Sin(_thetaOffset);

            result[0] = xHat * cosTheta - yHat * sinTheta + _xOffset;
            result[1] = xHat * sinTheta + yHat * cosTheta + _yOffset;
            return result;
        }

        public double ValueAt(double percentage)
        {
            percentage = Math.Max(Math.Min(percentage, 1), 0);
            double xHat = percentage * _knotDistance;
            double yHat = (_a * xHat + _b) * xHat * xHat * xHat * xHat
                    + _c * xHat * xHat * xHat + _d * xHat * xHat + _e * xHat;

            double cosTheta = Math.Cos(_thetaOffset);
            double sinTheta = Math.Sin(_thetaOffset);

            double value = xHat * sinTheta + yHat * cosTheta + _yOffset;
            return value;
        }

        private double DerivativeAt(double percentage)
        {
            percentage = Math.Max(Math.Min(percentage, 1), 0);

            double xHat = percentage * _knotDistance;
            double ypHat = (5 * _a * xHat + 4 * _b) * xHat * xHat * xHat + 3 * _c * xHat * xHat
                    + 2 * _d * xHat + _e;

            return ypHat;
        }

        private double SecondDerivativeAt(double percentage)
        {
            percentage = Math.Max(Math.Min(percentage, 1), 0);

            double xHat = percentage * _knotDistance;
            double yppHat = (20 * _a * xHat + 12 * _b) * xHat * xHat + 6 * _c * xHat + 2 * _d;

            return yppHat;
        }

        public double AngleAt(double percentage)
        {
            double angle = ChezyMath.BoundAngle0To2PiRadians(
                    Math.Atan(DerivativeAt(percentage)) + _thetaOffset);
            return angle;
        }

        public double AngleChangeAt(double percentage)
        {
            return ChezyMath.BoundAngleNegPiToPiRadians(
                    Math.Atan(SecondDerivativeAt(percentage)));
        }

        public override String ToString()
        {
            return "a=" + _a + "; b=" + _b + "; c=" + _c + "; d=" + _d + "; e=" + _e;
        }
    }

    
}
