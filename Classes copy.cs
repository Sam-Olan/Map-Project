using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Routemap
{
    public class AirportNode
    {
        public string Name { get; }  // Made read-only since they shouldn't change after creation
        public string Code { get; }
        public List<AirportNode> Destinations { get; }

        public AirportNode(string name, string code)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException(nameof(code));

            Name = name;
            Code = code.ToUpper();  // Ensure codes are always uppercase
            Destinations = new List<AirportNode>();
        }

        public void AddDestination(AirportNode destAirport)
        {
            bool Found = false;
            foreach (AirportNode Airport in Destinations)
            { // checks each airport in the list
                if (Airport == destAirport)
                    Found = true;

            }

            if (Found == false)     // if the airport is not in the list, add it
                Destinations.Add(destAirport);
        }

        public void RemoveDestination(AirportNode destAirport)
        {
            bool Found = false;
            foreach (AirportNode Airport in Destinations)
            { // checks each airport in the list
                if (Airport == destAirport)
                    Found = true;
            }

            if (Found == true)      // if the airport is in the list, remove it
                Destinations.Remove(destAirport);
        }

        public override string ToString()       // converts all the airport's information into a simple string
        {
            string b = string.Empty;
            foreach (AirportNode node in Destinations)      // creates one string of all the destinations
            {
                b = b + " " + node.Name;

            }
            return string.Join(String.Empty, Name + " airport's code is " + Code + " and its destinations are: " + b);       // returns the joined strings
        }
    }

    public class RouteMap
    {
        private readonly List<AirportNode> _airports;

        public RouteMap()
        {
            _airports = new List<AirportNode>();
        } //RouteMap constructor

        public bool FindAirport(string name)
        {
            bool Found = false;
            foreach (AirportNode Airport in _airports)
            { // checks each airport in the list
                if (Airport.Name == name)
                    Found = true;
            }
            return Found;       // returns whether or not the airports name was found in the list
        } //Method to find airport by name.

        public AirportNode FindAirportbycode(string code)    
        {
            AirportNode returnNode = null;
            foreach (AirportNode Airport in _airports)
            { // checks each airport in the list
                if (Airport.Code == code)
                    returnNode = Airport;
            }
            return returnNode;      // returns the found node
        } 

        public bool AddAirport(AirportNode a)
        {
            bool added = false;
            if (FindAirportbycode(a.Code) == null && !FindAirport(a.Name))      // checks if the list contains an aiport with a matching code and name, if it doesnt:
            {
                if (!string.IsNullOrEmpty(a.Code) || !string.IsNullOrEmpty(a.Name))
                {
                    _airports.Add(a);       // adds it to the list
                    added = true;
                }
            }
            return added;       // returns a bool indicating whether or not the airport was added to the list

        }

        public bool RemoveAirport(AirportNode a)
        {
            //remove airport node from main list
            if (FindAirportbycode(a.Code) != null)      // checks if the list contains an aiport with a matching code, if it does
            {
                _airports.Remove(a);        // remove the aiport from the list

                //remove any flight route going to removed location
                bool removeLocation;
                foreach (AirportNode Airport in _airports)
                {
                    removeLocation = false;
                    foreach (AirportNode location in Airport.Destinations)
                    {
                        if (location == a)
                            removeLocation = true;
                    }
                    if (removeLocation)
                        Airport.Destinations.Remove(a);
                }
                return true;
            }
            else
                return false;
        } //Method to remove airport node. Node must exist. 

        public bool AddRoute(AirportNode origin, AirportNode dest)
        {
            bool routeAdded = false;
            bool connected = false;
            if (FindAirportbycode(origin.Code) != null && FindAirportbycode(dest.Code) != null)     // checks by code if dest and origin exist in the list
            {
                foreach (AirportNode node in origin.Destinations)       // itterates through the origins destination list to search for a node that matches dest
                {
                    if (node == dest)
                        connected = true;
                }
            }
            if (connected == false)     // if no matching node is found
            {
                origin.Destinations.Add(dest);      //  add it to the destinations origin list
                routeAdded = true;
            }
            return routeAdded;      // returns a bool indicating whether or not the destination was added to the origins destination list
        }

        public bool RemoveRoute(AirportNode origin, AirportNode dest)
        {
            bool connected = false;
            if (FindAirportbycode(origin.Code) != null && FindAirportbycode(dest.Code) != null)     // checks by code if dest and origin exist in the list
                foreach (AirportNode node in origin.Destinations)       // itterates through the origins destination list to search for a node that matches dest
                {
                    if (node == dest)       // if such a node exists
                        connected = true;
                }
            if (connected == true)
            {
                origin.Destinations.Remove(dest);       // remove the node from origins destination list
                return connected;       // return a bool indicating whether or not the destination was removed to the origins destination list
            }
            else        // If not found, return connected == false
            {
                return connected;
            }

        }



        // Method Shortest Path
        // Accepts the starting node and the end node.
        // Method uses a breadth first search to iterate through the list of nodes A in routemap.
        // Uses a queue to track nodes that are soon to be visited
        // Uses array list "previousNodes" to track the parent node of the last visited node at the appopriate index (same index as the vistied node in visted array list)
        //                      - this allows the program to keep paths between nodes from start to finish and rebuild the fastest route
        public ArrayList ShortestPath(AirportNode startNode, AirportNode endNode)
        {
            Queue<AirportNode> frontierQueue = new Queue<AirportNode>();   // fornteirQueue allows the program to iterate over destinations of current node (changing current to destination)
            ArrayList previousNodes = new ArrayList();               // previousNodes holds the previous node
            ArrayList finalPath = new ArrayList();                   // holds the final fastest path nodes in the proper order 
            ArrayList visited = new ArrayList();                     // holds all visited nodes 
            AirportNode currentNode = startNode;        // current node used to iterate through list of nodes and add to array lists

            for (int i = 0; i <= _airports.Count; i++)      // Initializes previousNodes to null values to allow for indexing later (= to size of node list)
            {
                previousNodes.Add(null);
            }


            frontierQueue.Enqueue(currentNode);     // Enqueue starting node and add to visited before the while loop
            visited.Add(startNode);                 // * Prime the while loop below *


            if (currentNode.Destinations.Contains(endNode) == true)     // If start node shares an edge with final node, print path and exit method
            {
                finalPath.Add(endNode);
                Console.Write("Shortest path found. From " + currentNode.Name + " to " + endNode.Name + " the path is: ");
                Console.Write(startNode.Name + " -> " + endNode.Name);
                Console.WriteLine("\nThe distance of the path is " + finalPath.Count);
            }

            else       // If the start node does not connect directly to end node 
            {
                while (frontierQueue.Count > 0)     // Loop while the frontier queue is not empty (all nodes have not been found)
                {

                    currentNode = frontierQueue.Dequeue();      // Removes last node from queue and sets = to current

                    foreach (AirportNode destination in currentNode.Destinations)       // Iterate through all destinations of current node
                    {

                        if (visited.Contains(destination) != true)      // If the node has not already been visited, add to visited and dequeue
                        {
                            frontierQueue.Enqueue(destination);
                            visited.Add(destination);
                        }


                        if (previousNodes.Contains(currentNode))        // If the current node is already in previousNodes:
                        {
                            if (previousNodes[visited.IndexOf(destination)] == null)        // Check to make sure program is not placing the same node at the same index
                            {
                                previousNodes[visited.IndexOf(destination)] = currentNode;      // Place previous node at index = current node index in visit
                            }
                        }

                        else       // If previousNodes does not contain the current nodes destination:
                        {
                            if (previousNodes[visited.IndexOf(destination)] == null)        // If currentNode is a new destination it should not be placed over a previous node 
                            {                                                               //         - Checks to make sure this is true
                                previousNodes[visited.IndexOf(destination)] = currentNode;      // Place previous node at index = current node index in visit
                            }
                        }
                    }

                }


                finalPath.Add(endNode);     // Add end node to the final path at the start

                object n = endNode;     // Creates an object n to hold node values to be placed into the final path. Initialize = to end node

                bool exit = false;      // Used to exit while loop when path is / isnt found

                bool found = false;     // Used to print out fastest path if it exists

                while (exit != true)        // Continue loop while starting node is not in final path. This would indicate the final path is not complete
                {                                                           //  - Working backwards from the last node to the beginning on fastest path
                    try         // Try and catch for if the path does not exist, the program prints appropriate message and continues
                    {
                        n = previousNodes[visited.IndexOf(n)];      // n = the parent node of n. this value is in previousNodes at the same index as n is in visited. 
                    }

                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("There is no path between nodes: " + startNode.Name + " and " + endNode.Name);
                        exit = true;
                    }
                    finalPath.Add(n);       // Add this node to the final path

                    if (finalPath.Contains(startNode))      // Check to make sure start node is not in final path (the path is not complete)
                    {
                        exit = true;        // If true, end the loop
                        found = true;       // If path exists, set found to true for check below
                    }
                }

                if (found == true)      // If the path exists, print out appropriate message
                {
                    finalPath.Reverse();        // The path is recorded from finish to start, so reverese it for start to finish path

                    //Print out the shortest path
                    Console.Write("Shortest path found. From " + startNode.Name + " to " + endNode.Name + " the path is: " + startNode.Name);
                    foreach (AirportNode value in finalPath)
                    {
                        currentNode = value;
                        if (value != startNode)
                        {
                            Console.Write(" -> " + currentNode.Name);
                        }

                    }
                    Console.WriteLine("\nThe distance of the path is " + (finalPath.Count - 1));        // Print the path distance
                                                                                                        // -1 to account for start node (starting at 0 distance)
                }

            }
            return previousNodes;
        }

        public void PrintAllAirports()
        {
            if (_airports.Count == 0)
            {
                Console.WriteLine("No airports in the system.");
                return;
            }

            Console.WriteLine("\nList of all airports:");
            Console.WriteLine("Code\tName");
            Console.WriteLine("----\t----");
            
            foreach (var airport in _airports)
            {
                Console.WriteLine($"{airport.Code}\t{airport.Name}");
            }
        }
    }
}