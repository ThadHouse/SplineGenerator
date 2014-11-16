using System;
using System.Text;
using System.IO;

namespace SplineGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            const double wheelBaseWidth = 26.5/12;
            {
                Path path = new Path("Testing", wheelBaseWidth);
                path.SetConfig(10.0, 8.0, 50.0);
                path.AddWaypoint(0, 0, 0);
                path.AddWaypoint(7, 0, 0);
                path.AddWaypoint(14, 1, Math.PI/12);

                path.GeneratePath();

                var mPath = path.MainTrajectory;
                var l = path.Left;
                var r = path.Right;




                StringBuilder content = new StringBuilder();
                content.AppendLine(path.Name);
                content.AppendLine(l.GetNumSegments().ToString());
                content.AppendLine(Serialize(l));
                content.AppendLine(Serialize(r));


                if (!Directory.Exists("Outputs"))
                    Directory.CreateDirectory("Outputs");
                if (File.Exists("Outputs\\" + path.Name + "Output.txt"))
                {
                    File.Delete("Outputs\\" + path.Name + "Output.txt");
                }
                File.WriteAllText("Outputs\\" + path.Name + "Output.txt", content.ToString());
                
                //File.WriteAllLines("Outputs\\" + path.Name + "Output.txt", content);

                int x = 1;
            }
        }

        static string Serialize(Trajectory trajectory)
        {
            StringBuilder content = new StringBuilder();

            foreach (Segment s in trajectory)
            {
                content.AppendLine(s.Pos.ToString("F3") + " " + s.Vel.ToString("F3") + " " + s.Acc.ToString("F3") + " " + s.Jerk.ToString("F3") + " " + s.Heading.ToString("F3") + " " + s.Dt.ToString("F3") + " " + s.X.ToString("F3") + " " + s.Y.ToString("F3"));
            }

            /*
            String content = "";
            for (int i = 0; i < trajectory.GetNumSegments(); ++i)
            {
                Segment segment = trajectory[i];
                content += String.Format(
                        "%.3f %.3f %.3f %.3f %.3f %.3f %.3f %.3f\n",
                        segment.pos, segment.vel, segment.acc, segment.jerk,
                        segment.heading, segment.dt, segment.x, segment.y);
            }
             * */
            return content.ToString();
        }
    }
}
