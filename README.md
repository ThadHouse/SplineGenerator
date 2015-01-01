SplineGenerator
===============

I always wanted to get a better understanding of how 254's Spline Generator worked, so I did that the best way I know how. I converted it to C#. I wanted to share it with the community in case they want to try and work with it, but in another language.

A few things were upgraded:
  Uses IEnumerable for all the lists, so they can be queried, and used with foreach loops.
  Changed some of the Typedefs in Java to Enums to be easier to use
  Added a simple serializer so the array could easily be read from a LabVIEW ReadFromSpreadsheet VI
  
It still needs some work on the serializing, and file writing, but thats not super important. 
