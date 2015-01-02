using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SplineGenerator
{
    class PathGenerator
    {
        public static Trajectory GenerateFromPath(WaypointSequence waypoints, TrajectoryGenerator.Config config)
        {
            if (waypoints.Count < 2)
            {
                return null;
            }

            // Compute the total length of the path by creating splines for each pair
            // of waypoints.
            List<Spline> splines = new List<Spline>();
            List<Double> splineLengths = new List<double>();
            double totalDistance = 0;
            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                splines.Add(new Spline());
                if (!Spline.ReticulateSplines(waypoints[i],
                        waypoints[i+1], splines[i], Spline.Type.QuinticHermite))
                {
                    return null;
                }
                splineLengths.Add(splines[i].CalculateLength());
                totalDistance += splineLengths[i];
            }

            // Generate a smooth trajectory over the total distance.
            Trajectory traj = TrajectoryGenerator.Generate(config,
                    Strategy.SCurvesStrategy, 0.0, waypoints[0].Theta,
                    totalDistance, 0.0, waypoints[0].Theta);

            // Assign headings based on the splines.
            int curSpline = 0;
            double curSplineStartPos = 0;
            double lengthOfSplinesFinished = 0;
            for (int i = 0; i < traj.Count; i++)
            {
                double curPos = traj[i].Pos;

                bool foundSpline = false;
                while (!foundSpline)
                {
                    double curPosRelative = curPos - curSplineStartPos;
                    if (curPosRelative <= splineLengths[curSpline])
                    {
                        double percentage = splines[curSpline].GetPercentageForDistance(
                                curPosRelative);
                        traj[i].Heading = splines[curSpline].AngleAt(percentage);
                        double[] coords = splines[curSpline].GetXandY(percentage);
                        traj[i].X = coords[0];
                        traj[i].Y = coords[1];
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
                        traj[i].Heading = splines[splines.Count - 1].AngleAt(1.0);
                        double[] coords = splines[splines.Count - 1].GetXandY(1.0);
                        traj[i].X = coords[0];
                        traj[i].Y = coords[1];
                        foundSpline = true;
                    }
                }
            }

            return traj;
        }

        public static void MakeLeftAndRightTrajectories(Trajectory input,
                double wheelbaseWidth, out Trajectory leftTrajectory, out Trajectory rightTrajectory)
        {
            leftTrajectory = input.Copy();
            rightTrajectory = input.Copy();

            for (int i = 0; i < input.Count; i++)
            {
                Segment current = input[i];
                double cosAngle = Math.Cos(current.Heading);
                double sinAngle = Math.Sin(current.Heading);

                Segment sLeft = leftTrajectory[i];
                sLeft.X = current.X - wheelbaseWidth / 2 * sinAngle;
                sLeft.Y = current.Y + wheelbaseWidth / 2 * cosAngle;
                if (i > 0)
                {
                    // Get distance between current and last segment
                    double dist = Math.Sqrt((sLeft.X - leftTrajectory[i-1].X)
                            * (sLeft.X - leftTrajectory[i-1].X)
                            + (sLeft.Y - leftTrajectory[i-1].Y)
                            * (sLeft.Y - leftTrajectory[i-1].Y));
                    sLeft.Pos = leftTrajectory[i-1].Pos + dist;
                    sLeft.Vel = dist / sLeft.Dt;
                    sLeft.Acc = (sLeft.Vel - leftTrajectory[i-1].Vel) / sLeft.Dt;
                    sLeft.Jerk = (sLeft.Acc - leftTrajectory[i-1].Acc) / sLeft.Dt;
                }

                Segment sRight = rightTrajectory[i];
                sRight.X = current.X + wheelbaseWidth / 2 * sinAngle;
                sRight.Y = current.Y - wheelbaseWidth / 2 * cosAngle;
                if (i > 0)
                {
                    // Get distance between current and last segment
                    double dist = Math.Sqrt((sRight.X - rightTrajectory[i-1].X)
                            * (sRight.X - rightTrajectory[i-1].X)
                            + (sRight.Y - rightTrajectory[i-1].Y)
                            * (sRight.Y - rightTrajectory[i-1].Y));
                    sRight.Pos = rightTrajectory[i-1].Pos + dist;
                    sRight.Vel = dist / sRight.Dt;
                    sRight.Acc = (sRight.Vel - rightTrajectory[i-1].Vel) / sRight.Dt;
                    sRight.Jerk = (sRight.Acc - rightTrajectory[i-1].Acc) / sRight.Dt;
                }
            }
        }
    }
}
