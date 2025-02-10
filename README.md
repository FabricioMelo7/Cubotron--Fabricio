Work in progress...
This project's focus is to allow me to implement new ideas such as this one, while incorporating clean and maintainable code.
The assets used are all default from Unity 2022, such as cube, cylinder, camera, and lighting.

FIXED - There is a small issue with the stuttering of the cubes when moving. This is likely due to some objects using physics being updated inside the FixedUpdate() method, and other non-physics objects using the Update() method (which updates more frequently than the former).
Should be an easy fix in a future iteration.

https://github.com/user-attachments/assets/bfa3f902-5af7-447b-bc9d-14e7578849f9
