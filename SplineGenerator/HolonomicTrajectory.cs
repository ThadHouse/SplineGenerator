using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SplineGenerator
{
    public class HolonomicTrajectory : IEnumerable<HolonomicSegment>
    {
        //Stored list of segments
        private readonly List<HolonomicSegment> _segments;

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
        public HolonomicTrajectory()
        {
            _segments = new List<HolonomicSegment>();
        }
        
        /// <summary>
        /// Copy Constructor (Shallow Copy)
        /// </summary>
        /// <param name="segments">Trajectory to copy from</param>
        public HolonomicTrajectory(IEnumerable<HolonomicSegment> segments)
        {
            _segments = new List<HolonomicSegment>(segments);
        }

        /// <summary>
        /// Add a segment to the trajectory
        /// </summary>
        /// <param name="s">Segment to add (Does not Copy)</param>
        public void AddSegment(HolonomicSegment s)
        {
            _segments.Add(s);
        }

        /// <summary>
        /// Indexer to grab individual segments
        /// </summary>
        /// <param name="i">Index</param>
        /// <returns>Segment at Index</returns>
        public HolonomicSegment this[int i]
        {
            get { return _segments[i]; }
        }

        //These next 2 are required for the class to be IEnumerable
        public IEnumerator<HolonomicSegment> GetEnumerator()
        {
            return _segments.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        
        //Returns the number of Segments
        /*
        public int GetNumSegments()
        {
            return _segments.Count;
        }
         * */
        /*
        /// <summary>
        /// Scale a trajectory
        /// </summary>
        /// <param name="scalingFactor">Factor to scale by</param>
        public void Scale(double scalingFactor)
        {
            for (int i = 0; i < Count; ++i)
            {
                _segments[i].Pos *= scalingFactor;
                _segments[i].Vel *= scalingFactor;
                _segments[i].Acc *= scalingFactor;
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
                segment.Vel = -segment.Vel;
                segment.Acc = -segment.Acc;
                segment.Jerk = -segment.Jerk;
                segment.Pos = -segment.Pos;
            }
        }
         * 

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
                    segment.Heading = segment.Heading*(180.0/Math.PI); //RadiansToDegree(segment.Heading);
            }
        }
        */
        /// <summary>
        /// Append a Trajectory
        /// </summary>
        /// <param name="toAppend"></param>
        public void Append(HolonomicTrajectory toAppend)
        {
            foreach (HolonomicSegment t in toAppend)
            {
                _segments.Add(t);
            }
        }

        /// <summary>
        /// Gets a deep copy of the trajectory
        /// </summary>
        /// <returns></returns>
        public HolonomicTrajectory Copy()
        {
            return new HolonomicTrajectory(CopySegments(_segments));
        }

        
        private IEnumerable<HolonomicSegment> CopySegments(IEnumerable<HolonomicSegment> toCopy)
        {
            List<HolonomicSegment> copied = toCopy.Select(s => new HolonomicSegment(s)).ToList();
            return copied;
        }

        public void InjectPolarHeading(double heading)
        {
            foreach (var holonomicSegment in _segments)
            {
                holonomicSegment.PolarHeading = heading;
            }
        }
        /*
        /// <summary>
        /// Outputs the path to a string
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            String str = "Segment\tPos\tVel\tAcc\tJerk\tHeading\n";
            for (int i = 0; i < Count; ++i)
            {
                HolonomicSegment segment = _segments[i];
                str += i + "\t";
                str += segment.Pos + "\t";
                str += segment.Vel + "\t";
                str += segment.Acc + "\t";
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
                HolonomicSegment segment = _segments[i];
                str += i + "\t";
                str += segment.X + "\t";
                str += segment.Y + "\t";
                str += segment.Heading + "\t";
                str += "\n";
            }

            return str;
        }
         * */
    }
}
