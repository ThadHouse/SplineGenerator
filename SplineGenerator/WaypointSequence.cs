using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SplineGenerator
{
    public class Waypoint
    {
        /// <summary>
        /// Create a waypoint with specified values
        /// </summary>
        /// <param name="x">Waypoint X Location</param>
        /// <param name="y">Waypoint Y Location</param>
        /// <param name="theta">Waypoint Theta</param>
        public Waypoint(double x, double y, double theta)
        {
            X = x;
            Y = y;
            Theta = theta;
        }

        /// <summary>
        /// Copy Constructor (Deep Copy)
        /// </summary>
        /// <param name="toCopy">Waypoint to copy</param>
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

    //Made IEnumerable so it could be accessed with foreach loops
    class WaypointSequence : IEnumerable<Waypoint>
    {
        private readonly List<Waypoint> _waypoints;

        public int Count
        {
            get { return _waypoints.Count; }
        }

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

        public IEnumerator<Waypoint> GetEnumerator()
        {
            return _waypoints.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
