using System;
using System.Text;
using System.IO;

namespace SplineGenerator
{
    class Program
    {
        /// <summary>
        /// Main entry point for the program
        /// </summary>
        /// <param name="args">Command Line Argument</param>
        static void Main(string[] args)
        {
            //Converts inches to feet
            const double wheelBaseWidth = 26.5/12;

            { // Tester
                Path path = new Path("Testing", wheelBaseWidth);
                path.SetConfig(10.0, 8.0, 50.0);
                path.AddWaypoint(0, 0, 0);
                path.AddWaypoint(7, 0, 0);
                path.AddWaypoint(14, 1, Math.PI/12);

                path.GeneratePath();

                var mPath = path.MainTrajectory;
                var l = path.Left;
                var r = path.Right;



                /*
                StringBuilder content = new StringBuilder();
                content.AppendLine(path.Name);
                content.AppendLine(l.GetNumSegments().ToString());
                content.AppendLine(Serialize(l));
                content.AppendLine(Serialize(r));
                 * */

                /*
                if (!Directory.Exists("Outputs"))
                    Directory.CreateDirectory("Outputs");
                if (File.Exists("Outputs\\" + path.Name + "Output.txt"))
                {
                    File.Delete("Outputs\\" + path.Name + "Output.txt");
                }
                File.WriteAllText("Outputs\\" + path.Name + "Output.txt", content.ToString());
                
                //File.WriteAllLines("Outputs\\" + path.Name + "Output.txt", content);

                int x = 1;
                 * */
            }
            { // Origin to Goal
                Path path = new Path("0,0 to 9.5,4.1", wheelBaseWidth);
                path.SetConfig(6.0, 8.0, 50.0);
                path.AddWaypoint(0, 0, 0);
                path.AddWaypoint(5, 3, Math.PI/6);
                path.AddWaypoint(9.5, 4.1, 0);
                path.AddWaypoint(13, 3, -Math.PI / 6);
                path.AddWaypoint(19, 0, 0);

                path.GeneratePath();
                
                string output = SerializePathSimple(path);

                if (!Directory.Exists("Outputs"))
                    Directory.CreateDirectory("Outputs");
                if (File.Exists("Outputs\\" + path.Name + "Output.txt"))
                {
                    File.Delete("Outputs\\" + path.Name + "Output.txt");
                }
                File.WriteAllText("Outputs\\" + path.Name + "Output.txt", output);
                 
            }
            { //Goal to 22 ft back
                Path path = new Path("0,0 to -22,0", wheelBaseWidth);
                path.SetConfig(6.0, 8.0, 50.0);
                path.AddWaypoint(0, 0, 0);
                //path.AddWaypoint(5, 3, Math.PI / 6);
                path.AddWaypoint(22, 0, 0);

                path.GeneratePath();

                path.Left.InvertX();
                path.Right.InvertX();
                
                string output = SerializePathSimple(path);

                if (!Directory.Exists("Outputs"))
                    Directory.CreateDirectory("Outputs");
                if (File.Exists("Outputs\\" + path.Name + "Output.txt"))
                {
                    File.Delete("Outputs\\" + path.Name + "Output.txt");
                }
                File.WriteAllText("Outputs\\" + path.Name + "Output.txt", output);
                 
            }
            { // 22 ft back to origin
                Path path = new Path("0,0 to 12,-4.1", wheelBaseWidth);
                path.SetConfig(6.0, 8.0, 50.0);
                path.AddWaypoint(0, 0, 0);
                path.AddWaypoint(5, -2.5, Math.PI / -12);
                path.AddWaypoint(11, -4.1, 0);
                path.AddWaypoint(12, -4.1, 0);

                path.GeneratePath();

               
                string output = SerializePathSimple(path);

                if (!Directory.Exists("Outputs"))
                    Directory.CreateDirectory("Outputs");
                if (File.Exists("Outputs\\" + path.Name + "Output.txt"))
                {
                    File.Delete("Outputs\\" + path.Name + "Output.txt");
                }
                File.WriteAllText("Outputs\\" + path.Name + "Output.txt", output);
                 
            }
            { //22 ft back to goal, grabbing tube 2.
                Path path = new Path("0,0 to 22,0 Grab Tube 2", wheelBaseWidth);
                path.SetConfig(6.0, 8.0, 50.0);
                path.AddWaypoint(0, 0, 0);
                path.AddWaypoint(5, -2.5, Math.PI / -12);
                path.AddWaypoint(11, -4.1, 0);
                path.AddWaypoint(12, -4.1, 0);
                path.AddWaypoint(17, -4.1+3, Math.PI / 6);
                path.AddWaypoint(9.5+12, 0, 0);

                path.GeneratePath();
                
                string output = SerializePathSimple(path);

                if (!Directory.Exists("Outputs"))
                    Directory.CreateDirectory("Outputs");
                if (File.Exists("Outputs\\" + path.Name + "Output.txt"))
                {
                    File.Delete("Outputs\\" + path.Name + "Output.txt");
                }
                File.WriteAllText("Outputs\\" + path.Name + "Output.txt", output);
                
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

        public static string SerializePathSimple(Path path)
        {
            StringBuilder content = new StringBuilder();
            var left = path.Left;
            var right = path.Right;
            for (int i = 0; i < path.Left.Count; i++)
            {
                content.AppendLine(left[i].Pos.ToString("F3") + ", " + left[i].Vel.ToString("F3") + ", " +
                                   left[i].Acc.ToString("F3")
                                   + ", " + left[i].Heading.ToString("F3") + ", " + right[i].Pos.ToString("F3") + ", " + right[i].Vel.ToString("F3") +
                                   ", " + right[i].Acc.ToString("F3") + ", "
                                   + right[i].Dt);
            }

            return content.ToString();
        }
    }
}
