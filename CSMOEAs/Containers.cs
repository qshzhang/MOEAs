using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusRouteCalculator
{
    // Container classes for the json objects. These three cover the graph:
    class Stop
    {
        // Json object holders have to match EXACTLY the titles and variables of the objects
        public string id { get; set; }          // ID of the stop
        public string caption { get; set; }     // Name of the stop in human readable terms
        public float[] lnglat { get; set; }     // Location in degrees
    }

    class Edge
    {
        public string id { get; set; }          // A composite of the ids of the Stops this Edge connects
        public string anode { get; set; }       // The ID of a Stop this edge connects
        public string bnode { get; set; }
        public float distance { get; set; }     // Distance is in km
        public bool directed { get; set; }      // True or false, in our case always false
    }

    class Graph
    {
        public Stop[] stops { get; set; }
        public Edge[] edges { get; set; }
    }

    // These three cover the timetable:
    class StopTime
    {
        public string tripId { get; set; }      // The trip that this time belongs to, one of several per route
        public string arrivalTime { get; set; } // hh:mm:ss in 24-hr format
        public string departureTime { get; set; }
        public int arrivalSeconds { get; set; } // Total seconds elapsed since start of day
        public int departureSeconds { get; set; }
        public string stopId { get; set; }      // Corresponds to the id of a Stop in the graph
        public int stopSequence { get; set; }   // These need sorting by stop sequence as they are not in order!
    }

    class Trip
    {
        public string routeId { get; set; }     // This is the ID for the bus route (between two destinations)
        public string tripId { get; set; }      // This is the ID for a single trip along the route
        public int directionId { get; set; }    // The direction travelled along the route (up or down)
        public StopTime[] stopTimes { get; set; }
    }

    class TimeTable
    {
        public Trip[] trips { get; set; }
    }
}
