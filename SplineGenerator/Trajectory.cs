using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SplineGenerator
{
    public class Trajectory : IEnumerable<Segment>
    {
        private bool _invertedY = false;
        private readonly List<Segment> _segments;//; = new List<Segment>();

        public Trajectory(int length)
        {
            _segments = new List<Segment>();
            for (int i = 0; i < length; ++i)
            {
                _segments.Add(new Segment());
            }
        }

        public Segment this[int i]
        {
            get { return _segments[i]; }
        }

        public IEnumerator<Segment> GetEnumerator()
        {
            return _segments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Trajectory(IEnumerable<Segment> segments)
        {
            _segments = new List<Segment>(segments);
        }

        public void SetInvertedY(bool inverted)
        {
            _invertedY = inverted;
        }

        public int GetNumSegments()
        {
            return _segments.Count;
        }

        public Segment GetSegment(int index)
        {
            if (index < GetNumSegments())
            {
                if (!_invertedY)
                {
                    return _segments[index];
                }
                else
                {
                    Segment segment = new Segment(_segments[index]);
                    segment.Y *= -1.0;
                    segment.Heading *= -1.0;
                    return segment;
                }
            }
            else
            {
                return new Segment();
            }
        }

        public void SetSegment(int index, Segment segment)
        {
            if (index < GetNumSegments())
            {
                _segments[index] = segment;
            }
        }

        public void Scale(double scalingFactor)
        {
            for (int i = 0; i < GetNumSegments(); ++i)
            {
                _segments[i].Pos *= scalingFactor;
                _segments[i].Vel *= scalingFactor;
                _segments[i].Acc *= scalingFactor;
                _segments[i].Jerk *= scalingFactor;
            }
        }

        public void Append(Trajectory toAppend)
        {
            for (int i = 0; i < toAppend.GetNumSegments(); ++i)
            {
                _segments.Add(toAppend.GetSegment(i));
            }
        }

        public Trajectory Copy()
        {
            return new Trajectory(CopySegments(_segments));
        }

        private List<Segment> CopySegments(IEnumerable<Segment> toCopy)
        {
            List<Segment> copied = toCopy.Select(s => new Segment(s)).ToList();
            return copied;
        }

        public override String ToString()
        {
            String str = "Segment\tPos\tVel\tAcc\tJerk\tHeading\n";
            for (int i = 0; i < GetNumSegments(); ++i)
            {
                Segment segment = GetSegment(i);
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

        public String ToStringProfile()
        {
            return ToString();
        }

        public String ToStringEuclidean()
        {
            String str = "Segment\tx\ty\tHeading\n";
            for (int i = 0; i < GetNumSegments(); ++i)
            {
                Segment segment = GetSegment(i);
                str += i + "\t";
                str += segment.X + "\t";
                str += segment.Y + "\t";
                str += segment.Heading + "\t";
                str += "\n";
            }

            return str;
        }
    }

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

        public override String ToString()
        {
            return "pos: " + Pos + "; vel: " + Vel + "; acc: " + Acc + "; jerk: "
                   + Jerk + "; heading: " + Heading;
        }

    }
}
