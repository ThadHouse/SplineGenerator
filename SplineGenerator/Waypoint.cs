using System;

public class Waypoint
{
    /// <summary>
    /// Create a waypoint with specified values
    /// </summary>
    /// <param name="x">Waypoint X Location</param>
    /// <param name="y">Waypoint Y Location</param>
    /// <param name="theta">Waypoint Theta</param>
    public Waypoint(double x, double y, double theta)
    {
        X = x;
        Y = y;
        Theta = theta;
    }

    /// <summary>
    /// Copy Constructor (Deep Copy)
    /// </summary>
    /// <param name="toCopy">Waypoint to copy</param>
    public Waypoint(Waypoint toCopy)
    {
        X = toCopy.X;
        Y = toCopy.Y;
        Theta = toCopy.Theta;
    }
    public double X { get; set; }
    public double Y { get; set; }
    public double Theta { get; set; }
}
