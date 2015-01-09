using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SplineGenerator
{
    //Holonomic path to generate X and Y paths. Maybe will do polar as well. Does not currently support rotation.
    class HolonomicPath
    {
        private Trajectory _mainTrajectory;
        private HolonomicTrajectory _holonomicTrajectory;

        private readonly TrajectoryGenerator.Config _config;
        private readonly WaypointSequence _sequence;

        /// <summary>
        /// Gets or sets the name of the Path
        /// </summary>
        public String Name { get; set; }

        /*
        /// <summary>
        /// Gets the Main (Center) Trajectory
        /// </summary>
        public Trajectory MainTrajectory
        {
            get { return _mainTrajectory; }
        }
        */

        public HolonomicTrajectory HoloTrajectory
        {
            get { return _holonomicTrajectory; }
        }
        
        public HolonomicPath(String name)
        {
            Name = name;
            _config = new TrajectoryGenerator.Config() {Dt = 0.01};
            _sequence = new WaypointSequence();
        }

        public HolonomicPath()
        {
            _config = new TrajectoryGenerator.Config() {Dt = 0.01};
            _sequence = new WaypointSequence();
        }

        public bool GeneratePath(double polarHeading, double rotation)
        {
            _mainTrajectory = PathGenerator.GenerateFromPath(_sequence, _config, true);//This only needs to generate X's and Y's, so this can be much different, and maybe much faster.
            if (_mainTrajectory == null)
                return false;

            _holonomicTrajectory = _mainTrajectory.CopyToHolonomicTrajectory();
            HoloTrajectory.InjectPolarHeading(polarHeading);






            return true;
        }
        /*
        /// <summary>
        /// Add a waypoint (does not copy)
        /// </summary>
        /// <param name="w">Waypoint to add</param>
        public void AddWaypoint(Waypoint w)
        {
            _sequence.AddWaypoint(w);
        }
         * */

        /// <summary>
        /// Add a waypoint with specified values
        /// </summary>
        /// <param name="x">Waypoint X Location</param>
        public void AddWaypoint(double x)
        {
            _sequence.AddWaypoint(new Waypoint(x, 0, 0));
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
    }

    public class HolonomicSegment
    {
        public double MagnitudeVelocity { get; set; }

        public double MagnitudeAcceleration { get; set; }

        public double MagnitueJerk { get; set; }

        public double RotationVelocity { get; set; }

        public double RotationAcceleration { get; set; }

        public double RotationJerk { get; set; }

        public double Dt { get; set; }

        public double Position { get; set; }

        public double PolarHeading { get; set; }

        public double RotationHeading { get; set; }

        public HolonomicSegment()
        {
            
        }

        public HolonomicSegment(HolonomicSegment toCopy)
        {
            MagnitudeVelocity = toCopy.MagnitudeVelocity;
            MagnitudeAcceleration = toCopy.MagnitudeAcceleration;
            MagnitueJerk = toCopy.MagnitueJerk;
            RotationVelocity = toCopy.RotationVelocity;
            RotationAcceleration = toCopy.RotationAcceleration;
            RotationJerk = toCopy.RotationJerk;
            Dt = toCopy.Dt;
            Position = toCopy.Position;
            PolarHeading = toCopy.PolarHeading;
            RotationHeading = toCopy.RotationHeading;
        }
    }
}
