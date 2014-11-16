using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SplineGenerator
{
    class WriteFile
    {
        public string SerializePathComplete(Path path)
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine(path.Name);
            content.AppendLine(path.Left.GetNumSegments().ToString());
            content.AppendLine(SerializeTrajectoryComplete(path.Left));
            content.AppendLine(SerializeTrajectoryComplete(path.Right));
        }

        string SerializeTrajectoryComplete(Trajectory trajectory)
        {
            StringBuilder content = new StringBuilder();

            foreach (Segment s in trajectory)
            {
                content.AppendLine(s.Pos.ToString("F3") + " " + s.Vel.ToString("F3") + " " + s.Acc.ToString("F3") + " " + s.Jerk.ToString("F3") + " " + s.Heading.ToString("F3") + " " + s.Dt.ToString("F3") + " " + s.X.ToString("F3") + " " + s.Y.ToString("F3"));
            }
            return content.ToString();
        }

        public string SerializePathSimple(Path path)
        {
            StringBuilder content = new StringBuilder();
            var left = path.Left;
            var right = path.Right;
            for (int i = 0; i < path.Left.GetNumSegments(); i++)
            {
                content.AppendLine(left[i].Pos.ToString("F3") + ", " + left[i].Vel.ToString("F3") + ", " +
                                   left[i].Acc.ToString("F3")
                                   + ", " + left[i].Heading.ToString("F3") + ", " + right[i].Pos + ", " + right[i].Vel +
                                   ", " + right[i].Acc + ", "
                                   + right[i].Dt);
            }

            return content.ToString();
        }
    }
}
