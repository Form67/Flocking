Jared Okun and Sam Gould
1. The three features are equally weighted, but the magnitudes of acceleration are biased towards avoiding collisions.

2. Both cone check and collision avoidance looked at the closest objects but collision avoidance also specifically tried
to check for potential collisions as well. Pathfinding was weighted the same as evasion for collision avoidance, and slightly
below evasion for cone check. The seperation algorithm was the same used as in part 1 but only checked within the group.

3. I used two rays in order to make implementing arbitration an easier task for solving the corner problem.
It did make traversing the tunnel more difficult but it allowed avoiding potential collisions on the sides.
The rays are displayed using the debug show raycast feature.
