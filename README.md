# Boids
This program it's a try to test the boids algorithm in my accademy's engine. The boids try to avoid each other and to group at the same time following the same direction. The rules applied in the Boids algorithm are as follows: 
- separation: steer to avoid crowding local flockmates
- alignment: steer towards the average heading of local flockmates
- cohesion: steer to move towards the average position (center of mass) of local flockmates.
In this case I also add a colour merge, infact a boid spwan with a random colour and when a boid meet another their colour became a mix between. 
You can also influence the movement by clicking middle and left mouse button (left to attract the boids in the cursor position, middle to reject the boids from the cursor position). 
Right click to spawn a boid
