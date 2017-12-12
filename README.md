# Dfsu-VelocityMaxFlow
Provides velocity components from dfsu result files, at time of maximum speed/depth

This tool reads a dynamic dfsu result file from a MIKE 21 FM or MIKE FLOOD simulation, and returns a new dfsu file with a single time step, containing U-velocity and V-velocity components as well as current speed and direction, at the time of the peak of the flood. Peak is identified either as maximum current speed or maximum water depth time.

Such a result file can be used to map velocity vectors thanks to U-velocity and V-velocity components being saved for the same time step, corresponding to the peak flow.

The code requires references to DHI.Generic.MikeZero.DFS.dll and DHI.Generic.MikeZero.EUM.dll, from MIKE SDK.
