using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SplineGenerator
{
    public class Segment
    {
        public double Acc;
        public double Dt;
        public double Heading;
        public double Jerk;
        public double Pos, Vel;
        public double X, Y;

        public Segment()
        {
        }

        public Segment(double pos, double vel, double acc, double jerk,
            double heading, double dt, double x, double y)
        {
            Pos = pos;
            Vel = vel;
            Acc = acc;
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
            Pos = toCopy.Pos;
            Vel = toCopy.Vel;
            Acc = toCopy.Acc;
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
        public override String ToString()
        {
            return "pos: " + Pos + "; vel: " + Vel + "; acc: " + Acc + "; jerk: "
                   + Jerk + "; heading: " + Heading;
        }

    }
}
