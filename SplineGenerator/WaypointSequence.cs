using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SplineGenerator
{
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
