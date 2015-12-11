using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SplineGenerator
{
    public enum AngleMode { Degrees, Radians }
    
    public class Path
    {
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        //Store the center trajectory, and the left and right trajectories
        public Trajectory _mainTrajectory;
        public Trajectory _left;
        public Trajectory _right;

        private readonly TrajectoryGenerator.Config _config;
        private readonly WaypointSequence _sequence;

        /// <summary>
        /// Gets or sets the name of the Path
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the WheelBaseWidth (Feet)
        /// </summary>
        protected double WheelBaseWidth { get; set; }

        /// <summary>
        /// Gets the Main (Center) Trajectory
        /// </summary>
        public Trajectory MainTrajectory 
        {
            get { return _mainTrajectory; }
        }

        /// <summary>
        /// Gets the Left Side Trajectory
        /// </summary>
        public Trajectory Left
        {
            get { return _left; }
        }

        /// <summary>
        /// Gets the Right Side Trajectory
        /// </summary>
        public Trajectory Right
        {
            get { return _right; }
        }

        //Constructors
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

        /// <summary>
        /// Generates the path to follow
        /// </summary>
        /// <param name="mode">What angle mode to return</param>
        /// <returns>True if successful</returns>
        public bool GeneratePath(AngleMode mode = AngleMode.Degrees)
        {
            _mainTrajectory = PathGenerator.GenerateFromPath(_sequence, _config);
            if (_mainTrajectory == null)
                return false;
            PathGenerator.MakeLeftAndRightTrajectories(_mainTrajectory, WheelBaseWidth, out _left, out _right);

            _left.CorrectAngles(mode);
            _right.CorrectAngles(mode);

            return true;

        }

        /// <summary>
        /// Add a waypoint (does not copy)
        /// </summary>
        /// <param name="w">Waypoint to add</param>
        public void AddWaypoint(Waypoint w)
        {
            _sequence.AddWaypoint(w);
        }

        /// <summary>
        /// Add a waypoint with specified values
        /// </summary>
        /// <param name="x">Waypoint X Location</param>
        /// <param name="y">Waypoint Y Location</param>
        /// <param name="theta">Waypoint Theta</param>
        public void AddWaypoint(double x, double y, double theta)
        {
            _sequence.AddWaypoint(new Waypoint(x, y, theta));
        }

        /// <summary>
        /// Set the config
        /// </summary>
        /// <param name="maxVel"></param>
        /// <param name="maxAcc"></param>
        /// <param name="maxJerk"></param>
        public void SetConfig(double maxVel = 10.0, double maxAcc = 10.0, double maxJerk = 50.0)
        {
            _config.MaxVel = maxVel;
            _config.MaxAcc = maxAcc;
            _config.MaxJerk = maxJerk;
        }

        /// <summary>
        /// Set the dt of the path
        /// </summary>
        /// <param name="dt">dt in seconds</param>
        public void SetConfigdDt(double dt)
        {
            _config.Dt = dt;
        }

        /// <summary>
        /// Gets the heading at the end of the path
        /// </summary>
        /// <returns></returns>
        public double GetEndHeading()
        {
            int numSegments = _left.Count;
            Segment lastSegment = _left[(numSegments - 1)];
            return lastSegment.Heading;
        }
    }
}
