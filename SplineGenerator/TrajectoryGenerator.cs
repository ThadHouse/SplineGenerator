using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SplineGenerator
{
    public enum Strategy
    {
        StepStrategy,
        TrapezoidalStrategy,
        SCurvesStrategy,
        AutomaticStrategy
    }

    public enum IntegrationMethod
    {
        RectangularIntegration,
        TrapezoidalIntegration
    }

    public class TrajectoryGenerator
    {
        public class Config
        {

            public double Dt;
            public double MaxVel;
            public double MaxAcc;
            public double MaxJerk;
        }

        public static Trajectory Generate(
            Config config,
            Strategy strategy,
            double startVel,
            double startHeading,
            double goalPos,
            double goalVel,
            double goalHeading)
        {
            // Choose an automatic strategy.
            if (strategy == Strategy.AutomaticStrategy)
            {
                strategy = ChooseStrategy(startVel, goalVel, config.MaxVel);
            }

            Trajectory traj;
            if (strategy == Strategy.StepStrategy)
            {
                double impulse = (goalPos / config.MaxVel) / config.Dt;

                // Round down, meaning we may undershoot by less than max_vel*dt.
                // This is due to discretization and avoids a strange final
                // velocity.
                int time = (int)(Math.Floor(impulse));
                traj = SecondOrderFilter(1, 1, config.Dt, config.MaxVel,
                    config.MaxVel, impulse, time, IntegrationMethod.TrapezoidalIntegration);

            }
            else if (strategy == Strategy.TrapezoidalStrategy)
            {
                // How fast can we go given maximum acceleration and deceleration?
                double startDiscount = .5 * startVel * startVel / config.MaxAcc;
                double endDiscount = .5 * goalVel * goalVel / config.MaxAcc;

                double adjustedMaxVel = Math.Min(config.MaxVel,
                    Math.Sqrt(config.MaxAcc * goalPos - startDiscount
                              - endDiscount));
                double tRampup = (adjustedMaxVel - startVel) / config.MaxAcc;
                double xRampup = startVel * tRampup + .5 * config.MaxAcc
                                  * tRampup * tRampup;
                double tRampdown = (adjustedMaxVel - goalVel) / config.MaxAcc;
                double xRampdown = adjustedMaxVel * tRampdown - .5
                                    * config.MaxAcc * tRampdown * tRampdown;
                double xCruise = goalPos - xRampdown - xRampup;

                // The +.5 is to round to nearest
                int time = (int)((tRampup + tRampdown + xCruise
                                   / adjustedMaxVel) / config.Dt + .5);

                // Compute the length of the linear filters and impulse.
                int f1Length = (int)Math.Ceiling((adjustedMaxVel
                                                    / config.MaxAcc) / config.Dt);
                double impulse = (goalPos / adjustedMaxVel) / config.Dt
                                 - startVel / config.MaxAcc / config.Dt
                                 + startDiscount + endDiscount;
                traj = SecondOrderFilter(f1Length, 1, config.Dt, startVel,
                    adjustedMaxVel, impulse, time, IntegrationMethod.TrapezoidalIntegration);

            }
            else if (strategy == Strategy.SCurvesStrategy)
            {
                // How fast can we go given maximum acceleration and deceleration?
                double adjustedMaxVel = Math.Min(config.MaxVel,
                    (-config.MaxAcc * config.MaxAcc + Math.Sqrt(config.MaxAcc
                                                                * config.MaxAcc * config.MaxAcc * config.MaxAcc
                                                                + 4 * config.MaxJerk * config.MaxJerk * config.MaxAcc
                                                                * goalPos)) / (2 * config.MaxJerk));

                // Compute the length of the linear filters and impulse.
                int f1Length = (int)Math.Ceiling((adjustedMaxVel
                                                    / config.MaxAcc) / config.Dt);
                int f2Length = (int)Math.Ceiling((config.MaxAcc
                                                    / config.MaxJerk) / config.Dt);
                double impulse = (goalPos / adjustedMaxVel) / config.Dt;
                int time = (int)(Math.Ceiling(f1Length + f2Length + impulse));
                traj = SecondOrderFilter(f1Length, f2Length, config.Dt, 0,
                    adjustedMaxVel, impulse, time, IntegrationMethod.TrapezoidalIntegration);

            }
            else
            {
                return null;
            }
            // Now assign headings by interpolating along the path.
            // Don't do any wrapping because we don't know units.
            double totalHeadingChange = goalHeading - startHeading;
            for (int i = 0; i < traj.Count; ++i)
            {
                traj[i].Heading = startHeading + totalHeadingChange
                                            * (traj[i].Pos)
                                            / traj[traj.Count - 1].Pos;
            }

            return traj;
        }
        private static Trajectory SecondOrderFilter(
          int f1Length,
          int f2Length,
          double dt,
          double startVel,
          double maxVel,
          double totalImpulse,
          int length,
          IntegrationMethod integration)
        {
            if (length <= 0)
            {
                return null;
            }
            Trajectory traj = new Trajectory();

            Segment last = new Segment { Pos = 0, Vel = startVel, Acc = 0, Jerk = 0, Dt = dt };
            // First segment is easy

            // f2 is the average of the last f2_length samples from f1, so while we
            // can recursively compute f2's sum, we need to keep a buffer for f1.
            double[] f1 = new double[length];
            f1[0] = (startVel / maxVel) * f1Length;
            for (int i = 0; i < length; ++i)
            {
                Segment segment = new Segment();
                // Apply input
                double input = Math.Min(totalImpulse, 1);
                if (input < 1)
                {
                    // The impulse is over, so decelerate
                    input -= 1;
                    totalImpulse = 0;
                }
                else
                {
                    totalImpulse -= input;
                }

                // Filter through F1
                double f1Last = i > 0 ? f1[i - 1] : f1[0];
                f1[i] = Math.Max(0.0, Math.Min(f1Length, f1Last + input));

                double f2 = 0;
                // Filter through F2
                for (int j = 0; j < f2Length; ++j)
                {
                    if (i - j < 0)
                    {
                        break;
                    }

                    f2 += f1[i - j];
                }
                f2 = f2 / f1Length;

                // Velocity is the normalized sum of f2 * the max velocity
                segment.Vel = f2 / f2Length * maxVel;

                if (integration == IntegrationMethod.RectangularIntegration)
                {
                    segment.Pos = segment.Vel * dt + last.Pos;
                }
                else if (integration == IntegrationMethod.TrapezoidalIntegration)
                {
                    segment.Pos = (last.Vel
                            + segment.Vel) / 2.0 * dt + last.Pos;
                }
                segment.X = segment.Pos;
                segment.Y = 0;

                // Acceleration and jerk are the differences in velocity and
                // acceleration, respectively.
                segment.Acc = (segment.Vel - last.Vel) / dt;
                segment.Jerk = (segment.Acc - last.Acc) / dt;
                segment.Dt = dt;

                last = segment;
                traj.AddSegment(segment);
            }

            return traj;
        }
        public static Strategy ChooseStrategy(double startVel, double goalVel,
          double maxVel)
        {
            Strategy strategy;
            if (startVel == goalVel && startVel == maxVel)
            {
                strategy = Strategy.StepStrategy;
            }
            else if (startVel == goalVel && startVel == 0)
            {
                strategy = Strategy.SCurvesStrategy;
            }
            else
            {
                strategy = Strategy.TrapezoidalStrategy;
            }
            return strategy;
        }
    }

}
