using System;
using System.Collections.Generic;
namespace Routemap
{
    internal class Program
    {
        //Sam Olan
        //Marc Touma
        //Mackenzie Neill
        static void Main(string[] args)
        {
            var map = new RouteMap();      // create a new RouteMap and pass the list to it
            var routeManager = new RouteManager();   // renamed from tools to follow C# naming conventions
            string userInput;       // Keeps track of user input

            InitializeAirports(map);
            InitializeRoutes(map, routeManager);

            // loop to perform route map operations based on user input
            do
            {
                DisplayMenu();
                userInput = Console.ReadLine()?.ToUpper(); // Convert input to uppercase

                ProcessUserInput(userInput, map);

            } while (userInput != "E");
        }

        private static void InitializeAirports(RouteMap map)
        {
            //preloaded map
            map.AddAirport(new AirportNode("Vancouver", "YVR"));
            map.AddAirport(new AirportNode("Edmonton", "YEG"));
            map.AddAirport(new AirportNode("Calgary", "YYC"));
            map.AddAirport(new AirportNode("Winnipeg", "YWG"));
            map.AddAirport(new AirportNode("Ottawa", "YOW"));
            map.AddAirport(new AirportNode("Toronto", "YYZ"));
            map.AddAirport(new AirportNode("Quebec", "YQB"));
            map.AddAirport(new AirportNode("Montreal", "YUL"));
            map.AddAirport(new AirportNode("Fredericton", "YFC"));
            map.AddAirport(new AirportNode("Moncton", "YQM"));
            map.AddAirport(new AirportNode("Halifax", "YHZ"));
            map.AddAirport(new AirportNode("Gander", "YQX"));
        }

        private static void InitializeRoutes(RouteMap map, RouteManager routeManager)
        {
            //preload routes
            routeManager.AddRoute("YVR", "YEG", map);
            routeManager.AddRoute("YYC", "YEG", map);
            routeManager.AddRoute("YYC", "YVR", map);
            routeManager.AddRoute("YYC", "YWG", map);
            routeManager.AddRoute("YVR", "YYZ", map);
            routeManager.AddRoute("YEG", "YWG", map);
            routeManager.AddRoute("YWG", "YYC", map);
            routeManager.AddRoute("YWG", "YOW", map);
            routeManager.AddRoute("YYZ", "YVR", map);
            routeManager.AddRoute("YYZ", "YOW", map);
            routeManager.AddRoute("YYZ", "YFC", map);
            routeManager.AddRoute("YYZ", "YHZ", map);
            routeManager.AddRoute("YYZ", "YYC", map);
            routeManager.AddRoute("YYZ", "YWG", map);
            routeManager.AddRoute("YOW", "YFC", map);
            routeManager.AddRoute("YOW", "YUL", map);
            routeManager.AddRoute("YOW", "YYZ", map);
            routeManager.AddRoute("YOW", "YEG", map);
            routeManager.AddRoute("YQB", "YOW", map);
            routeManager.AddRoute("YUL", "YOW", map);
            routeManager.AddRoute("YUL", "YQM", map);
            routeManager.AddRoute("YUL", "YQX", map);
            routeManager.AddRoute("YFC", "YQM", map);
            routeManager.AddRoute("YQM", "YFC", map);
            routeManager.AddRoute("YQM", "YQB", map);
            routeManager.AddRoute("YQM", "YHZ", map);
            routeManager.AddRoute("YHZ", "YQX", map);
            routeManager.AddRoute("YQX", "YUL", map);
            routeManager.AddRoute("YFC", "YYZ", map);
        }

        private static void DisplayMenu()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Command list:");
            Console.WriteLine("I = Insert an airport");
            Console.WriteLine("D = Remove an airport");
            Console.WriteLine("R = Create a route");
            Console.WriteLine("U = Remove a route");
            Console.WriteLine("S = find shortest path between airports");
            Console.WriteLine("V = view airport details");
            Console.WriteLine("L = list all airports");
            Console.WriteLine("E = exit program");
            Console.WriteLine();
            Console.Write("Enter a command >> ");
        }

        private static void ProcessUserInput(string input, RouteMap map)
        {
            switch (input)
            {
                case "I":
                    HandleAddAirport(map);
                    break;
                case "R":
                    HandleAddRoute(map);
                    break;
                case "U":
                    HandleRemoveRoute(map);
                    break;
                case "S":
                    HandleShortestPath(map);
                    break;
                case "V":
                    HandleViewAirport(map);
                    break;
                case "D":
                    HandleRemoveAirport(map);
                    break;
                case "L":
                    map.PrintAllAirports();
                    break;
                case "E":
                    Console.WriteLine("\nThank you for using the Route Map program. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid entry, please try again");
                    break;
            }
        }

        private static void HandleAddAirport(RouteMap map)
        {
            Console.WriteLine("What is the airports name?");
            string name = Console.ReadLine();
            Console.WriteLine("what is the airports Code?");
            string code = Console.ReadLine()?.ToUpper();

            var airport = new AirportNode(name, code);
            if (map.AddAirport(airport))
                Console.WriteLine($"{airport.Name} was successfully added to the map");
            else
                Console.WriteLine($"{airport.Name} was unable to be added");
        }

        private static void HandleAddRoute(RouteMap map)
        {
            Console.WriteLine("What is the origin airports Code?");
            string originCode = Console.ReadLine();
            Console.WriteLine("What is the dest airport code?");
            string destCode = Console.ReadLine();

            var originAirport = map.FindAirportbycode(originCode);
            var destAirport = map.FindAirportbycode(destCode);

            if (originAirport != null && destAirport != null)
            {
                if (map.AddRoute(originAirport, destAirport))
                    Console.WriteLine("Route added successfully");
                else
                    Console.WriteLine("Failed to add route");
            }
            else
                Console.WriteLine("airport not found");
        }

        private static void HandleRemoveRoute(RouteMap map)
        {
            Console.WriteLine("What is the origin airports Code?");
            string originCode = Console.ReadLine();
            Console.WriteLine("What is the dest airport code?");
            string destCode = Console.ReadLine();

            var originAirport = map.FindAirportbycode(originCode);
            var destAirport = map.FindAirportbycode(destCode);

            if (originAirport != null && destAirport != null)
            {
                if (map.RemoveRoute(originAirport, destAirport))
                    Console.WriteLine("Route removed successfully");
                else
                    Console.WriteLine("Failed to remove route");
            }
            else
                Console.WriteLine("airport not found");
        }

        private static void HandleShortestPath(RouteMap map)
        {
            map.PrintAllAirports();

            Console.WriteLine("\nWhat is the origin airports Code?");
            string originCode = Console.ReadLine()?.ToUpper();
            var originAirport = map.FindAirportbycode(originCode);

            if (originAirport == null)
            {
                Console.WriteLine("Origin airport not found");
                return;
            }

            Console.WriteLine($"\nDestinations from {originAirport.Name}:");
            foreach (var destination in originAirport.Destinations)
            {
                Console.WriteLine($"{destination.Code}\t{destination.Name}");
            }

            Console.WriteLine("\nWhat is the dest airport code?");
            string destCode = Console.ReadLine()?.ToUpper();
            var destAirport = map.FindAirportbycode(destCode);

            if (destAirport == null)
            {
                Console.WriteLine("Destination airport not found");
                return;
            }

            if (originAirport == destAirport)
            {
                Console.WriteLine($"\nYou are already at {originAirport.Name}");
            }
            else
            {
                map.ShortestPath(originAirport, destAirport);
            }
        }

        private static void HandleViewAirport(RouteMap map)
        {
            map.PrintAllAirports();
            
            Console.WriteLine("\nWhat is the code of the airport you want to view?");
            string code = Console.ReadLine()?.ToUpper();

            var airport = map.FindAirportbycode(code);

            if (airport != null)
                Console.WriteLine(airport.ToString());
            else
                Console.WriteLine("airport not found");
        }

        private static void HandleRemoveAirport(RouteMap map)
        {
            Console.WriteLine("What is the airport's code?");
            string code = Console.ReadLine();
            var oldAirport = map.FindAirportbycode(code);

            if (oldAirport != null)
            {
                if (map.RemoveAirport(oldAirport))
                    Console.WriteLine("Airport removed");
                else
                    Console.WriteLine("Failed to remove airport");
            }
            else
                Console.WriteLine("airport not found");
        }
    }

    // Renamed from tools to follow C# naming conventions
    public class RouteManager
    {
        public void AddRoute(string start, string end, RouteMap map)
        {
            if (string.IsNullOrWhiteSpace(start) || string.IsNullOrWhiteSpace(end) || map == null)
                return;

            var startLocation = map.FindAirportbycode(start.ToUpper());
            var endLocation = map.FindAirportbycode(end.ToUpper());

            if (startLocation == null || endLocation == null || !map.AddRoute(startLocation, endLocation))
            {
                Console.WriteLine($"Error: {start} to {end}");
            }
        }
    }
}