using System;
using System.Text;
using System.IO;
using Newtonsoft.Json;

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
            {
                /*
                HolonomicPath hPath = new HolonomicPath("HoloPath");
                hPath.SetConfig(10.0, 15.0, 70.0);
                hPath.AddWaypoint(0);
                hPath.AddWaypoint(10);

                hPath.GeneratePath(Math.PI/2, 0);

                StringBuilder content = new StringBuilder();
                var left = hPath.HoloTrajectory;
                var right = left;
                for (int i = 0; i < hPath.HoloTrajectory.Count; i++)
                {
                    content.AppendLine(left[i].Dt.ToString("F3") + ", " + left[i].MagnitudeVelocity.ToString("F3") + ", " +
                                       left[i].MagnitudeAcceleration.ToString("F3")
                                       + ", " + left[i].MagnitueJerk.ToString("F3") + ", " + right[i].Position.ToString("F3") + ", " + right[i].Dt.ToString("F3") +
                                       ", " + right[i].Dt.ToString("F3") + ", "
                                       + right[i].Dt);
                }
                string output = content.ToString();
                if (!Directory.Exists("Outputs"))
                    Directory.CreateDirectory("Outputs");
                if (File.Exists("Outputs\\" + hPath.Name + "Output.txt"))
                {
                    File.Delete("Outputs\\" + hPath.Name + "Output.txt");
                }
                File.WriteAllText("Outputs\\" + hPath.Name + "Output.txt", output);
                */
            }


            { // Tester
                Path path = new Path("Testing", wheelBaseWidth);
                path.SetConfig(10.0, 8.0, 50.0);
                path.AddWaypoint(0, 0, 0);
                path.AddWaypoint(7, 0, 0);
                //path.AddWaypoint(14, 1, Math.PI/12);

                path.GeneratePath();

                string serialized = path.Left.Serialize();

                var mPath = path.MainTrajectory;
                var l = path.Left;
                var r = path.Right;

                var deserialize = JsonConvert.DeserializeObject<Trajectory>(serialized);
                ;

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

                File.WriteAllText("File.txt", JsonConvert.SerializeObject(path, Formatting.Indented));

                var unserialized = JsonConvert.DeserializeObject<Path>(File.ReadAllText("File.txt"));
               

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
    }
}
