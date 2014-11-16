using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SplineGenerator
{
    public class ChezyMath
    {
        const double sq2p1 = 2.414213562373095048802e0;
        const double sq2m1 = .414213562373095048802e0;
        const double p4 = .161536412982230228262e2;
        const double p3 = .26842548195503973794141e3;
        const double p2 = .11530293515404850115428136e4;
        const double p1 = .178040631643319697105464587e4;
        const double p0 = .89678597403663861959987488e3;
        const double q4 = .5895697050844462222791e2;
        const double q3 = .536265374031215315104235e3;
        const double q2 = .16667838148816337184521798e4;
        const double q1 = .207933497444540981287275926e4;
        const double q0 = .89678597403663861962481162e3;
        const double PIO2 = 1.5707963267948966135E0;
        const double nan = (0.0 / 0.0);
        // reduce

        private static double Mxatan(double arg)
        {
            double argsq = arg * arg;
            double value = ((((p4 * argsq + p3) * argsq + p2) * argsq + p1) * argsq + p0);
            value = value / (((((argsq + q4) * argsq + q3) * argsq + q2) * argsq + q1) * argsq + q0);
            return value * arg;
        }

        // reduce
        private static double Msatan(double arg)
        {
            if (arg < sq2m1)
            {
                return Mxatan(arg);
            }
            if (arg > sq2p1)
            {
                return PIO2 - Mxatan(1 / arg);
            }
            return PIO2 / 2 + Mxatan((arg - 1) / (arg + 1));
        }

        // implementation of atan
        public static double Atan(double arg)
        {
            if (arg > 0)
            {
                return Msatan(arg);
            }
            return -Msatan(-arg);
        }

        // implementation of atan2
        public static double Atan2(double arg1, double arg2)
        {
            if (arg1 + arg2 == arg1)
            {
                if (arg1 >= 0)
                {
                    return PIO2;
                }
                return -PIO2;
            }
            arg1 = Atan(arg1 / arg2);
            if (arg2 < 0)
            {
                if (arg1 <= 0)
                {
                    return arg1 + Math.PI;
                }
                return arg1 - Math.PI;
            }
            return arg1;

        }

        // implementation of asin
        public static double Asin(double arg)
        {
            int sign = 0;
            if (arg < 0)
            {
                arg = -arg;
                sign++;
            }
            if (arg > 1)
            {
                return nan;
            }
            double temp = Math.Sqrt(1 - arg * arg);
            if (arg > 0.7)
            {
                temp = PIO2 - Atan(temp / arg);
            }
            else
            {
                temp = Atan(arg / temp);
            }
            if (sign > 0)
            {
                temp = -temp;
            }
            return temp;
        }

        // implementation of acos
        public static double Acos(double arg)
        {
            if (arg > 1 || arg < -1)
            {
                return nan;
            }
            return PIO2 - Asin(arg);
        }

        /**
         * Get the difference in angle between two angles.
         *
         * @param from The first angle
         * @param to The second angle
         * @return The change in angle from the first argument necessary to line up
         * with the second. Always between -Pi and Pi
         */
        public static double GetDifferenceInAngleRadians(double from, double to)
        {
            return BoundAngleNegPiToPiRadians(to - from);
        }

        /**
         * Get the difference in angle between two angles.
         *
         * @param from The first angle
         * @param to The second angle
         * @return The change in angle from the first argument necessary to line up
         * with the second. Always between -180 and 180
         */
        public static double GetDifferenceInAngleDegrees(double from, double to)
        {
            return BoundAngleNeg180To180Degrees(to - from);
        }

        public static double BoundAngle0To360Degrees(double angle)
        {
            // Naive algorithm
            while (angle >= 360.0)
            {
                angle -= 360.0;
            }
            while (angle < 0.0)
            {
                angle += 360.0;
            }
            return angle;
        }

        public static double BoundAngleNeg180To180Degrees(double angle)
        {
            // Naive algorithm
            while (angle >= 180.0)
            {
                angle -= 360.0;
            }
            while (angle < -180.0)
            {
                angle += 360.0;
            }
            return angle;
        }

        public static double BoundAngle0To2PiRadians(double angle)
        {
            // Naive algorithm
            while (angle >= 2.0 * Math.PI)
            {
                angle -= 2.0 * Math.PI;
            }
            while (angle < 0.0)
            {
                angle += 2.0 * Math.PI;
            }
            return angle;
        }

        public static double BoundAngleNegPiToPiRadians(double angle)
        {
            // Naive algorithm
            while (angle >= Math.PI)
            {
                angle -= 2.0 * Math.PI;
            }
            while (angle < -Math.PI)
            {
                angle += 2.0 * Math.PI;
            }
            return angle;
        }
    }
}

