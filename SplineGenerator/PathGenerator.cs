using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SplineGenerator
{

    class PathGenerator
    {
        public static void MakePath(WaypointSequence waypoints, TrajectoryGenerator.Config config, double wheelBaseWidth, string name)
        {
            
        }

        

        public static Trajectory GenerateFromPath(WaypointSequence path, TrajectoryGenerator.Config config)
        {
            if (path.GetNumWaypoints() < 2)
            {
                return null;
            }

            // Compute the total length of the path by creating splines for each pair
            // of waypoints.
            //Spline[] splines = new Spline[path.getNumWaypoints() - 1];
            List<Spline> splines = new List<Spline>();
            List<Double> splineLengths = new List<double>();
            //double[] spline_lengths = new double[splines.Length];
            double totalDistance = 0;
            for (int i = 0; i < path.GetNumWaypoints() - 1; ++i)
            {
                //splines[i] = new Spline();
                splines.Add(new Spline());
                if (!Spline.ReticulateSplines(path[i],
                        path[i+1], splines[i], Spline.Type.QuinticHermite))
                {
                    return null;
                }
                splineLengths.Add(splines[i].CalculateLength());
                //spline_lengths[i] = splines[i].calculateLength();
                totalDistance += splineLengths[i];
            }

            // Generate a smooth trajectory over the total distance.
            Trajectory traj = TrajectoryGenerator.Generate(config,
                    Strategy.SCurvesStrategy, 0.0, path[0].Theta,
                    totalDistance, 0.0, path[0].Theta);

            // Assign headings based on the splines.
            int curSpline = 0;
            double curSplineStartPos = 0;
            double lengthOfSplinesFinished = 0;
            for (int i = 0; i < traj.GetNumSegments(); ++i)
            {
                double curPos = traj.GetSegment(i).Pos;

                bool foundSpline = false;
                while (!foundSpline)
                {
                    double curPosRelative = curPos - curSplineStartPos;
                    if (curPosRelative <= splineLengths[curSpline])
                    {
                        double percentage = splines[curSpline].GetPercentageForDistance(
                                curPosRelative);
                        traj.GetSegment(i).Heading = splines[curSpline].AngleAt(percentage);
                        double[] coords = splines[curSpline].GetXandY(percentage);
                        traj.GetSegment(i).X = coords[0];
                        traj.GetSegment(i).Y = coords[1];
                        foundSpline = true;
                    }
                    else if (curSpline < splines.Count - 1)
                    {
                        lengthOfSplinesFinished += splineLengths[curSpline];
                        curSplineStartPos = lengthOfSplinesFinished;
                        ++curSpline;
                    }
                    else
                    {
                        traj.GetSegment(i).Heading = splines[splines.Count - 1].AngleAt(1.0);
                        double[] coords = splines[splines.Count - 1].GetXandY(1.0);
                        traj.GetSegment(i).X = coords[0];
                        traj.GetSegment(i).Y = coords[1];
                        foundSpline = true;
                    }
                }
            }

            return traj;
        }

        public static void MakeLeftAndRightTrajectories(Trajectory input,
                double wheelbaseWidth, out Trajectory leftTrajectory, out Trajectory rightTrajectory)
        {
            /*
            Trajectory[] output = new Trajectory[2];
            output[0] = input.Copy();
            output[1] = input.Copy();
             * */
            leftTrajectory = input.Copy();
            rightTrajectory = input.Copy();

            for (int i = 0; i < input.GetNumSegments(); i++)
            {
                Segment current = input.GetSegment(i);
                double cosAngle = Math.Cos(current.Heading);
                double sinAngle = Math.Sin(current.Heading);

                Segment sLeft = leftTrajectory.GetSegment(i);
                sLeft.X = current.X - wheelbaseWidth / 2 * sinAngle;
                sLeft.Y = current.Y + wheelbaseWidth / 2 * cosAngle;
                if (i > 0)
                {
                    // Get distance between current and last segment
                    double dist = Math.Sqrt((sLeft.X - leftTrajectory.GetSegment(i - 1).X)
                            * (sLeft.X - leftTrajectory.GetSegment(i - 1).X)
                            + (sLeft.Y - leftTrajectory.GetSegment(i - 1).Y)
                            * (sLeft.Y - leftTrajectory.GetSegment(i - 1).Y));
                    sLeft.Pos = leftTrajectory.GetSegment(i - 1).Pos + dist;
                    sLeft.Vel = dist / sLeft.Dt;
                    sLeft.Acc = (sLeft.Vel - leftTrajectory.GetSegment(i - 1).Vel) / sLeft.Dt;
                    sLeft.Jerk = (sLeft.Acc - leftTrajectory.GetSegment(i - 1).Acc) / sLeft.Dt;
                }

                Segment sRight = rightTrajectory.GetSegment(i);
                sRight.X = current.X + wheelbaseWidth / 2 * sinAngle;
                sRight.Y = current.Y - wheelbaseWidth / 2 * cosAngle;
                if (i > 0)
                {
                    // Get distance between current and last segment
                    double dist = Math.Sqrt((sRight.X - rightTrajectory.GetSegment(i - 1).X)
                            * (sRight.X - rightTrajectory.GetSegment(i - 1).X)
                            + (sRight.Y - rightTrajectory.GetSegment(i - 1).Y)
                            * (sRight.Y - rightTrajectory.GetSegment(i - 1).Y));
                    sRight.Pos = rightTrajectory.GetSegment(i - 1).Pos + dist;
                    sRight.Vel = dist / sRight.Dt;
                    sRight.Acc = (sRight.Vel - rightTrajectory.GetSegment(i - 1).Vel) / sRight.Dt;
                    sRight.Jerk = (sRight.Acc - rightTrajectory.GetSegment(i - 1).Acc) / sRight.Dt;
                }
            }
            //leftTrajectory = output[0];
            //rightTrajectory = output[1];
            //return new Trajectory.Pair(output[0], output[1]);
        }
    }
}
