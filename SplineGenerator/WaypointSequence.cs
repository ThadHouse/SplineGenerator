using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SplineGenerator
{
    public class Waypoint
    {
        public Waypoint(double x, double y, double theta)
        {
            X = x;
            Y = y;
            Theta = theta;
        }

        public Waypoint(Waypoint toCopy)
        {
            X = toCopy.X;
            Y = toCopy.Y;
            Theta = toCopy.Theta;
        }
        public double X { get; set; }
        public double Y { get; set; }
        public double Theta { get; set; }
    }

    class WaypointSequence : IEnumerable<Waypoint>
    {
        private readonly List<Waypoint> _waypoints;

        public WaypointSequence()
        {
            _waypoints = new List<Waypoint>();
        }

        public void AddWaypoint(Waypoint w)
        {
            _waypoints.Add(w);
        }

        public Waypoint this[int i]
        {
            get { return _waypoints[i]; }
        }

        public int GetNumWaypoints()
        {
            return _waypoints.Count;
        }

        public IEnumerator<Waypoint> GetEnumerator()
        {
            return _waypoints.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public WaypointSequence InvertY()
        {
            WaypointSequence inverted = new WaypointSequence();
            for (int i = 0; i < GetNumWaypoints(); ++i)
            {
                inverted._waypoints.Add(_waypoints[i]);
                inverted._waypoints[i].Y *= -1;
                inverted._waypoints[i].Theta = ChezyMath.BoundAngle0To2PiRadians(
                    2 * Math.PI - inverted._waypoints[i].Theta);
            }

            return inverted;
        }
    }
}
