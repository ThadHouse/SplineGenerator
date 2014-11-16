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
        public TrajectoryGenerator.Config Config;
        private WaypointSequence sequence;
        public String Name { get; set; }
        protected double WheelBaseWidth { get; set; }

        public Trajectory MainTrajectory {
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


        public bool GeneratePath()
        {
            _mainTrajectory = PathGenerator.GenerateFromPath(sequence, Config);
            if (_mainTrajectory == null)
                return false;
            PathGenerator.MakeLeftAndRightTrajectories(_mainTrajectory, WheelBaseWidth, out _left, out _right);
            return true;

        }

        public Path(String name, double wheelBaseWidth)
        {
            Name = name;
            WheelBaseWidth = wheelBaseWidth;
            Config = new TrajectoryGenerator.Config {Dt = 0.01};
            sequence = new WaypointSequence();
        }

        public Path()
        {
            Config = new TrajectoryGenerator.Config();
            sequence = new WaypointSequence();
        }

        public void AddWaypoint(Waypoint w)
        {
            sequence.AddWaypoint(w);
        }

        public void AddWaypoint(double x, double y, double theta)
        {
            sequence.AddWaypoint(new Waypoint(x, y, theta));
        }

        public void SetConfig(double maxVel = 10.0, double maxAcc = 10.0, double maxJerk = 50.0)
        {
            Config.MaxVel = maxVel;
            Config.MaxAcc = maxAcc;
            Config.MaxJerk = maxJerk;
        }

        public void SetConfigDt(double dt)
        {
            Config.Dt = dt;
        }

        public double GetEndHeading()
        {
            int numSegments = _left.GetNumSegments();
            Segment lastSegment = _left.GetSegment(numSegments - 1);
            return lastSegment.Heading;
        }
    }
}
