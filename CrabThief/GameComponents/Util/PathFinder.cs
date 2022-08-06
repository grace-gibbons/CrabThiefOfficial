using System.Collections.Generic;

using CrabThief.GameComponents.Map;

//Find the shortest path on the world map
namespace CrabThief.GameComponents.Util {
    class PathFinder {

        /// <summary>
        /// Use a breadth first search to find the shortest path between two background tiles
        /// </summary>
        /// <param name="start"> Tile to start search at </param>
        /// <param name="end"> Tile to find/end at </param>
        /// <returns></returns>
        public LinkedList<BackgroundTile> shortestPath(BackgroundTile start, BackgroundTile end) {
            //Visited
            LinkedList<BackgroundTile> queue = new LinkedList<BackgroundTile>();
            //Predecessors to find shortest path
            Dictionary<BackgroundTile, BackgroundTile> predecessors = new Dictionary<BackgroundTile, BackgroundTile>();

            queue.AddLast(start);
            predecessors.Add(start, start); 

            //Do bfs
            while(queue.Count != 0) {
                BackgroundTile current = queue.First.Value;
                queue.RemoveFirst();
                if (current.GetCoordinates() == end.GetCoordinates()) {
                    break;
                }
                //Loop over neighbours of current node
                foreach(BackgroundTile neighbour in current.GetConnected()) {
                    //Add neighbours to the map if not visited
                    if(!predecessors.ContainsKey(neighbour)) {
                        predecessors.Add(neighbour, current);
                        queue.AddLast(neighbour);
                    }
                }
            }

            //Find the shortest path through the world map 
            LinkedList<BackgroundTile> shortestPath = new LinkedList<BackgroundTile>(); 
            if(predecessors.ContainsKey(end)) {
                BackgroundTile current = end; 
                while(!current.Equals(start)) {
                    shortestPath.AddFirst(current);
                    current = predecessors[current];
                }
                shortestPath.AddFirst(start);
            }
            return shortestPath;
        }
    }
}
