Put the 'levels' folder in your www folder (and run wamp)
The robotarm server can call to that website (like: localhost/levels/bas/tower)
You can now load levels from the "server" with the python library:

robot_arm.load_level("levelname", "user")
robot_arm.load_level("tower", "bas")