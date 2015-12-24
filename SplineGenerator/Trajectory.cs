using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace SplineGenerator
{
    //Made Trajectory IEnumerable so a foreach loop could cycle thru it
    public class Trajectory : IEnumerable<Segment>
    {
        public string Serialize()
        {
            string serialized = JsonConvert.SerializeObject(this, Formatting.Indented);
            return serialized;
        }


        //Stored list of segments
        private readonly List<Segment> _segments;

        /// <summary>
        /// Gets the number of segments in the Trajectory
        /// </summary>
        public int Count
        {
            get { return _segments.Count; }
        }


        /// <summary>
        /// Default constructor initialized new List
        /// </summary>
        public Trajectory()
        {
            _segments = new List<Segment>();
        }

        /// <summary>
        /// Copy Constructor (Shallow Copy)
        /// </summary>
        /// <param name="segments">Trajectory to copy from</param>
        public Trajectory(IEnumerable<Segment> segments)
        {
            _segments = new List<Segment>(segments);
        }

        /// <summary>
        /// Add a segment to the trajectory
        /// </summary>
        /// <param name="s">Segment to add (Does not Copy)</param>
        public void AddSegment(Segment s)
        {
            _segments.Add(s);
        }

        /// <summary>
        /// Indexer to grab individual segments
        /// </summary>
        /// <param name="i">Index</param>
        /// <returns>Segment at Index</returns>
        public Segment this[int i]
        {
            get { return _segments[i]; }
        }

        //These next 2 are required for the class to be IEnumerable
        public IEnumerator<Segment> GetEnumerator()
        {
            return _segments.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Scale a trajectory
        /// </summary>
        /// <param name="scalingFactor">Factor to scale by</param>
        public void Scale(double scalingFactor)
        {
            for (int i = 0; i < Count; ++i)
            {
                _segments[i].Position *= scalingFactor;
                _segments[i].Velocity *= scalingFactor;
                _segments[i].Acceleration *= scalingFactor;
                _segments[i].Jerk *= scalingFactor;
            }
        }

        /// <summary>
        /// Invert X (Forward and Backward on the field)
        /// </summary>
        public void InvertX()
        {
            foreach (var segment in _segments)
            {
                segment.X = -segment.X;
                segment.Velocity = -segment.Velocity;
                segment.Acceleration = -segment.Acceleration;
                segment.Jerk = -segment.Jerk;
                segment.Position = -segment.Position;
            }
        }

        /// <summary>
        /// Bound angles to be between -Pi and Pi
        /// </summary>
        /// <param name="mode">Switches outputs between degrees and radians</param>
        public void CorrectAngles(AngleMode mode)
        {
            foreach (var segment in _segments)
            {
                segment.Heading = ChezyMath.BoundAngleNegPiToPiRadians(segment.Heading);
                if (mode == AngleMode.Degrees)
                    segment.Heading = segment.Heading * (180.0 / Math.PI); //RadiansToDegree(segment.Heading);
            }
        }

        /// <summary>
        /// Append a Trajectory
        /// </summary>
        /// <param name="toAppend"></param>
        public void Append(Trajectory toAppend)
        {
            foreach (Segment segment in toAppend)
            {
                _segments.Add(segment);
            }
        }

        /// <summary>
        /// Gets a deep copy of the trajectory
        /// </summary>
        /// <returns></returns>
        public Trajectory Copy()
        {
            return new Trajectory(CopySegments(_segments));
        }
        private IEnumerable<Segment> CopySegments(IEnumerable<Segment> toCopy)
        {
            List<Segment> copied = toCopy.Select(s => new Segment(s)).ToList();
            return copied;
        }

        /// <summary>
        /// Outputs the path to a string
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            String str = "Segment\tPos\tVel\tAcc\tJerk\tHeading\n";
            for (int i = 0; i < Count; ++i)
            {
                Segment segment = _segments[i];
                str += i + "\t";
                str += segment.Position + "\t";
                str += segment.Velocity + "\t";
                str += segment.Acceleration + "\t";
                str += segment.Jerk + "\t";
                str += segment.Heading + "\t";
                str += "\n";
            }

            return str;
        }

        /// <summary>
        /// Outputs the path to a Euclidean string
        /// </summary>
        /// <returns></returns>
        public String ToStringEuclidean()
        {
            String str = "Segment\tx\ty\tHeading\n";
            for (int i = 0; i < Count; ++i)
            {
                Segment segment = _segments[i];
                str += i + "\t";
                str += segment.X + "\t";
                str += segment.Y + "\t";
                str += segment.Heading + "\t";
                str += "\n";
            }

            return str;
        }
    }
}
