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
        private Trajectory _xTrajectory;
        private Trajectory _yTrajectory;

        private readonly TrajectoryGenerator.Config _config;
        private readonly WaypointSequence _sequence;

        /// <summary>
        /// Gets or sets the name of the Path
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Gets the Main (Center) Trajectory
        /// </summary>
        public Trajectory MainTrajectory
        {
            get { return _mainTrajectory; }
        }

        public Trajectory XTrajectory
        {
            get { return _xTrajectory; }
        }

        public Trajectory YTrajectory
        {
            get { return _yTrajectory; }
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

        public bool GeneratePath()
        {
            _mainTrajectory = PathGenerator.GenerateFromPath(_sequence, _config);//This only needs to generate X's and Y's, so this can be much different, and maybe much faster.
            if (_mainTrajectory == null)
                return false;



            return true;
        }
    }
}
