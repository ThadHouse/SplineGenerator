using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SplineGenerator
{
    public class Segment
    {
        public double Acceleration;
        public double Dt;
        public double Heading;
        public double Jerk;
        public double Position, Velocity;
        public double X, Y;

        public Segment()
        {
        }

        public Segment(double pos, double vel, double acc, double jerk,
            double heading, double dt, double x, double y)
        {
            Position = pos;
            Velocity = vel;
            Acceleration = acc;
            Jerk = jerk;
            Heading = heading;
            Dt = dt;
            X = x;
            Y = y;
        }

        /// <summary>
        /// Constructor with a deep copy of the segment
        /// </summary>
        /// <param name="toCopy"></param>
        public Segment(Segment toCopy)
        {
            Position = toCopy.Position;
            Velocity = toCopy.Velocity;
            Acceleration = toCopy.Acceleration;
            Jerk = toCopy.Jerk;
            Heading = toCopy.Heading;
            Dt = toCopy.Dt;
            X = toCopy.X;
            Y = toCopy.Y;
        }

        /// <summary>
        /// Returns segment as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "pos: " + Position + "; vel: " + Velocity + "; acc: " + Acceleration + "; jerk: "
                   + Jerk + "; heading: " + Heading;
        }

    }
}
