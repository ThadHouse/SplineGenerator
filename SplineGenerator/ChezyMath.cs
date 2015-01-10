using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SplineGenerator
{
    public class ChezyMath
    {
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

