using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SplineGenerator
{
    public class Path
    {
        private Trajectory _mainTrajectory;
        private Trajectory _left;
        private Trajectory _right;
        private readonly TrajectoryGenerator.Config _config;
        private readonly WaypointSequence _sequence;


        public String Name { get; set; }
        protected double WheelBaseWidth { get; set; }

        public Trajectory MainTrajectory 
        {
            get { return _mainTrajectory; }
        }

        public Trajectory Left
        {
            get { return _left; }
        }

        public Trajectory Right
        {
            get { return _right; }
        }


        public Path(String name, double wheelBaseWidth)
        {
            Name = name;
            WheelBaseWidth = wheelBaseWidth;
            _config = new TrajectoryGenerator.Config { Dt = 0.01 };
            _sequence = new WaypointSequence();
        }

        public Path()
        {
            _config = new TrajectoryGenerator.Config();
            _sequence = new WaypointSequence();
        }


        public bool GeneratePath()
        {
            _mainTrajectory = PathGenerator.GenerateFromPath(_sequence, _config);
            if (_mainTrajectory == null)
                return false;
            PathGenerator.MakeLeftAndRightTrajectories(_mainTrajectory, WheelBaseWidth, out _left, out _right);
            return true;

        }

        public void AddWaypoint(Waypoint w)
        {
            _sequence.AddWaypoint(w);
        }

        public void AddWaypoint(double x, double y, double theta)
        {
            _sequence.AddWaypoint(new Waypoint(x, y, theta));
        }

        public void SetConfig(double maxVel = 10.0, double maxAcc = 10.0, double maxJerk = 50.0)
        {
            _config.MaxVel = maxVel;
            _config.MaxAcc = maxAcc;
            _config.MaxJerk = maxJerk;
        }

        public void SetConfigDt(double dt)
        {
            _config.Dt = dt;
        }

        public double GetEndHeading()
        {
            int numSegments = _left.GetNumSegments();
            Segment lastSegment = _left[(numSegments - 1)];
            return lastSegment.Heading;
        }
    }
}
