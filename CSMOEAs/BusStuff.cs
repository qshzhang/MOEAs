using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;


namespace BusRouteCalculator
{   
    class Test
    {
        static void Testpoint()
        {
            string testGraph = "";
            string testTimetable = "";
            StreamReader streamReader = new StreamReader("exeterTransit_33-1-_-y10-1_graph.json");
            testGraph = streamReader.ReadToEnd();
            streamReader = new StreamReader("exeterTransit_33-1-_-y10-1_timetable.json");
            testTimetable = streamReader.ReadToEnd();
            Graph graph = JsonSerializer.Deserialize<Graph>(testGraph);
            TimeTable timeTable = JsonSerializer.Deserialize<TimeTable>(testTimetable);
            BusRoute busRoute = new BusRoute(timeTable, graph);
            Console.WriteLine("Total route distance = " + busRoute.CalculateRouteDistance().ToString());

            float buses = 1f;
            float chargingStations = 1f;
            float chargeLevel = 0.4f;

            BusNetwork busNetwork = new BusNetwork();
            busNetwork.BusRoutes.Add(new BusRoute(busRoute));
            float[] results = busNetwork.SolveNetwork(new float[] { buses, chargingStations, chargeLevel });
            Console.WriteLine("With {0} buses, {1} charging stations, and a charge level of {2}: \n\rCost: " + results[0].ToString() + " Tardiness: " + results[1].ToString(), buses, chargingStations, chargeLevel);
            busNetwork.BusRoutes.Add(new BusRoute(busRoute));
            buses = 2f;
            results = busNetwork.SolveNetwork(new float[] { buses, chargingStations, chargeLevel });
            Console.WriteLine("With {0} buses, {1} charging stations, and a charge level of {2}: \n\rCost: " + results[0].ToString() + " Tardiness: " + results[1].ToString(), buses, chargingStations, chargeLevel);
            busNetwork.BusRoutes.Add(new BusRoute(busRoute));
            buses = 3f;
            results = busNetwork.SolveNetwork(new float[] { buses, chargingStations, chargeLevel });
            Console.WriteLine("With {0} buses, {1} charging stations, and a charge level of {2}: \n\rCost: " + results[0].ToString() + " Tardiness: " + results[1].ToString(), buses, chargingStations, chargeLevel);
            busNetwork.BusRoutes.Add(new BusRoute(busRoute));
            chargingStations = 2f;
            results = busNetwork.SolveNetwork(new float[] { buses, chargingStations, chargeLevel });
            Console.WriteLine("With {0} buses, {1} charging stations, and a charge level of {2}: \n\rCost: " + results[0].ToString() + " Tardiness: " + results[1].ToString(), buses, chargingStations, chargeLevel);
            busNetwork.BusRoutes.Add(new BusRoute(busRoute));
            chargeLevel = 0.5f;
            results = busNetwork.SolveNetwork(new float[] { buses, chargingStations, chargeLevel });
            Console.WriteLine("With {0} buses, {1} charging stations, and a charge level of {2}: \n\rCost: " + results[0].ToString() + " Tardiness: " + results[1].ToString(), buses, chargingStations, chargeLevel);
            busNetwork.BusRoutes.Add(new BusRoute(busRoute));
            buses = 4f;
            results = busNetwork.SolveNetwork(new float[] { buses, chargingStations, chargeLevel });
            Console.WriteLine("With {0} buses, {1} charging stations, and a charge level of {2}: \n\rCost: " + results[0].ToString() + " Tardiness: " + results[1].ToString(), buses, chargingStations, chargeLevel);
            busNetwork.BusRoutes.Add(new BusRoute(busRoute));
            chargeLevel = 0.6f;
            results = busNetwork.SolveNetwork(new float[] { buses, chargingStations, chargeLevel });
            Console.WriteLine("With {0} buses, {1} charging stations, and a charge level of {2}: \n\rCost: " + results[0].ToString() + " Tardiness: " + results[1].ToString(), buses, chargingStations, chargeLevel);
            busNetwork.BusRoutes.Add(new BusRoute(busRoute));
            buses = 8f;
            results = busNetwork.SolveNetwork(new float[] { buses, chargingStations, chargeLevel });
            Console.WriteLine("With {0} buses, {1} charging stations, and a charge level of {2}: \n\rCost: " + results[0].ToString() + " Tardiness: " + results[1].ToString(), buses, chargingStations, chargeLevel);
            busNetwork.BusRoutes.Add(new BusRoute(busRoute));
            buses = 4f;
            chargingStations = 4f;
            results = busNetwork.SolveNetwork(new float[] { buses, chargingStations, chargeLevel });
            Console.WriteLine("With {0} buses, {1} charging stations, and a charge level of {2}: \n\rCost: " + results[0].ToString() + " Tardiness: " + results[1].ToString(), buses, chargingStations, chargeLevel);
            Console.ReadLine();
        }
    }

    // We need to check the SOC of the bus at every stop (discrete) rather than constantly checking it (continuous), adding a fail condition
    // if the bus will not have enough charge to complete the trip back to the charging point.


    class BusNetwork
    {
        // This should contain all the routes, all the constants, all the buses, and the calculations for everything
        // Constants:
        public const int COST_OF_A_BUS = 290500;                // Cost is in GBP, average of mean and mean excluding min and max and rounded to nearest 100.
        public const int COST_OF_A_CHARGING_STATION = 73300;    // Cost in GBP, calculated as above (though with less samples).
        public const float BUS_BATTERY_CAPACITY = 250f;         // Total charge in kWh.
        public const float FAST_CHARGER_OUTPUT = 250f;          // Power in kW, rounded to the nearest 10.
        public const float CHARGING_EFFICIENCY = 0.9f;          // Charging efficiency is a percentage that accounts for loss.
        public const float OFF_PEAK_CHARGING_COST = 0.05f;      // Charging cost is measured in GBP per kWh (rounded to the nearest 0.01).
        public const float PEAK_CHARGING_COST = 0.11f;          // As above.
        public const float AVERAGE_CHARGING_COST = 0.09f;       // As above.
        public const float BATTERY_DISCHARGE_RATE = 1.34f;      // Discharge rate in kWh per km.
        public const float MINIMUM_SAFE_CHARGE = 0.25f;         // The minimum charge percentage the battery can go down to
        public const float MAXIMUM_FAST_CHARGE = 0.9f;          // The highest amount to charge the bus battery to
        public const float TERMINAL_CLOSENESS = 1.0f;           // When combining terminal locations, how close should they be (in km)
        public const int BUS_CHARGING_PENALTY = 60;             // A time penalty to apply when the bus needs to charge but isn't at a charging point.
        //public const float BUS_RANGE = 200f;                    // Average range of an electric bus in km (for a full charge down to zero).

        //  Calculations for chargimg:
        //
        //  Number of minutes to charge a battery = (60*Capacity to charge)/(Charger output*Efficiency)
        //  
        //  Capacity to charge = difference from current battery capacity to objective capacity
        //  e.g. from 20% to 80% of a 250 kWh battery is 50 to 200 = 150

        public bool Debug = false;

        public List<BusRoute> BusRoutes;
        public List<Bus> Buses;

        public BusNetwork()
        {
            BusRoutes = new List<BusRoute>();
            Buses = new List<Bus>();
        }

        private void ChargeBus(ref Bus bus)
        {
            bus.CurrentCharge += (FAST_CHARGER_OUTPUT * CHARGING_EFFICIENCY) / 60f;
        }

        private int ChargeBus(Bus bus, float _chargeLevel)
        {
            int minutes = 0;
            // Update the charge within this method, and return the number of minutes:
            while (bus.CurrentCharge < bus.MaximumCapacity * _chargeLevel)
            {
                minutes++;
                bus.CurrentCharge += (FAST_CHARGER_OUTPUT * CHARGING_EFFICIENCY) / 60f;
            }
            return minutes;
        }

        /// <summary>
        /// Given any chromosome representing buses, chargers, and a charging plan, how much will the bus route network cost and how well
        /// will it keep to the timetable.
        /// </summary>
        /// <param name="_chromosome">This contains the number of buses [0], the number of charging stations [1], and the percentage charge
        /// to maintain [2].</param>
        /// <returns>The total network cost (in GBP) [0] and the total timekeeping penalty (in minutes) [1].</returns>
        public float[] SolveNetwork(float[] _chromosome)
        {
            // Cost to replicate current timetable

            // The chromosome should contain the number of buses (from 1 to many), the number of charging stations (from 1 to many),
            // and what battery percentage to maintain charge to (cannot drop below 20% on any trip, cannot go above 90% max).
            if (_chromosome.Length != 3)
            {
                throw new Exception("Incorrect chromosome length");
            }
            float[] results = new float[2];
            results[0] = 0f;
            results[1] = 0f;
            for(int i = 0; i < (int)_chromosome[0]; i++)
            {
                Buses.Add(new Bus());
                Buses[i].BusID = i.ToString();
                Buses[i].CurrentCharge = BUS_BATTERY_CAPACITY;
                Buses[i].MaximumCapacity = BUS_BATTERY_CAPACITY;
                results[0] += COST_OF_A_BUS;
            }
            for(int j = 0; j < (int)_chromosome[1]; j++)
            {
                results[0] += COST_OF_A_CHARGING_STATION;
            }
            // We will start off writing this for a single route.
            // Each bus can only be in one place at any time, and cannot move from one terminal stop to the other without taking time
            // and using charge.
            Dictionary<string, BusTrip> allTrips = new Dictionary<string, BusTrip>();

            foreach(BusRoute b in BusRoutes)
            {
                // We check which graph nodes are terminal nodes and then allocate charging resources to them
                // Assume bus follows trip
                // Bus cannot go below xx% charge on a trip
                // At each terminal check if bus has energy to reach next charging station. If not, charge it until it does. If it does 
                // but the charge level is below the threshold, charge it to the threshold. Else don't charge it.
                // If you have a new bus use it, else wait for bus to recharge and penalise time
                // We set the initial terminal of the first trip to have a charging station:
                //b.FindNode(b.busTrips[0].busStops[0].StopID).HasChargingStation = true;

                // Put all BusStops into a list and sort them
                foreach (BusTrip trip in b.busTrips)
                {
                    allTrips.Add(trip.TripID, trip);
                }
                Time currentTime = new Time(b.busTrips[0].StartTime.ToString());
                foreach (Bus bus in Buses)
                {
                    bus.BusReadyTime = new Time(currentTime.ToString());
                }

                #region Charging Station placement
                // We find all other terminal stops before deciding where to
                // place any remaining charging stations:
                Dictionary<string, int> terminalStopIDs = new Dictionary<string, int>();
                Dictionary<string, string> duplicateTerminals = new Dictionary<string, string>();
                foreach (BusTrip trip in b.busTrips)
                {
                    if(!terminalStopIDs.ContainsKey(trip.StartID))
                    {
                        // We assume that the terminal is new:
                        bool isNewTerminal = true;
                        // Then we check to make sure it isn't near another terminal:
                        foreach (KeyValuePair<string, int> stopID in terminalStopIDs)
                        {
                            // Check if the terminal stop is close to any of the others:
                            GraphNode temp = b.FindNode(stopID.Key);
                            GraphNode node = b.FindNode(trip.StartID);
                            if (Vector2.DistanceBetweenPoints(temp.Position.y, temp.Position.x, node.Position.y, node.Position.x) < TERMINAL_CLOSENESS)
                            {
                                isNewTerminal = false;
                                terminalStopIDs[stopID.Key]++;
                                if (!duplicateTerminals.ContainsKey(trip.StartID))
                                {
                                    duplicateTerminals.Add(trip.StartID, stopID.Key);
                                }
                                break;
                            }
                        }
                        if (isNewTerminal)
                        {
                            terminalStopIDs.Add(trip.StartID, 1);
                        }
                    }
                    else
                    {
                        terminalStopIDs[trip.StartID]++;
                    }
                    if (!terminalStopIDs.ContainsKey(trip.EndID))
                    {
                        bool isNewTerminal = true;
                        foreach (KeyValuePair<string, int> stopID in terminalStopIDs)
                        {
                            GraphNode temp = b.FindNode(stopID.Key);
                            GraphNode node = b.FindNode(trip.EndID);
                            if (Vector2.DistanceBetweenPoints(temp.Position.y, temp.Position.x, node.Position.y, node.Position.x) < TERMINAL_CLOSENESS)
                            {
                                isNewTerminal = false;
                                terminalStopIDs[stopID.Key]++;
                                if (!duplicateTerminals.ContainsKey(trip.StartID))
                                {
                                    duplicateTerminals.Add(trip.StartID, stopID.Key);
                                }
                                break;
                            }
                        }
                        if (isNewTerminal)
                        {
                            terminalStopIDs.Add(trip.EndID, 1);
                        }
                    }
                    else
                    {
                        terminalStopIDs[trip.EndID]++;
                    }
                }
                for (int i = 0; i < (int)_chromosome[1]; i++)
                {
                    int temp = 0;
                    string target = "";
                    foreach (KeyValuePair<string, int> terminalStopID in terminalStopIDs)
                    {
                        if (terminalStopID.Value > temp && !b.FindNode(terminalStopID.Key).HasChargingStation)
                        {
                            temp = terminalStopID.Value;
                            target = terminalStopID.Key;
                        }
                    }
                    b.FindNode(target).HasChargingStation = true;
                    foreach (KeyValuePair<string, string> terminal in duplicateTerminals)
                    {
                        if (terminal.Value == target)
                        {
                            b.FindNode(terminal.Key).HasChargingStation = true;
                        }
                    }
                }
                #endregion

                // For the first stop of each trip we need to assign a bus to it from our pool of buses. We can set
                // buses to StopID "Far Terminal" to represent the non-depot end of the route (?).
                // For each stop we need to update the current state of charge of the bus on that trip, unless the
                // stop is a charging station in which case we check if we need to charge the bus and increment the
                // cost and charge accordingly.

                //here is what I need to fix! The charging patterns need more work... if there is charging station at point don't penalise an hour

                while (true)
                {
                    foreach(BusTrip trip in b.busTrips)
                    {
                        float batteryDrain = trip.TripDistance * BATTERY_DISCHARGE_RATE;
                        if (trip.StartTime == currentTime)
                        {
                            #region We assign a bus
                            if (!trip.HasBus)
                            {
                                // If we get here then there is no bus yet assigned to this trip:
                                for(int i = 0; i < Buses.Count; i++)
                                {
                                    if (Buses[i].BusReadyTime == currentTime && Buses[i].IsCharging)
                                    {
                                        Buses[i].IsCharging = false;
                                    }
                                    // We look for unassigned buses at the depot that are now ready:
                                    if (!Buses[i].Assigned && Buses[i].StopID == "Depot" && Buses[i].BusReadyTime <= currentTime && !Buses[i].IsCharging)
                                    {
                                        if (Buses[i].CurrentCharge - batteryDrain < Buses[i].MaximumCapacity * _chromosome[2])
                                        {
                                            // The bus needs more charging so cannot be assigned
                                            int chargeTime = 0;
                                            Buses[i].IsCharging = true;
                                            chargeTime += ChargeBus(Buses[i], MAXIMUM_FAST_CHARGE);
                                            results[0] += chargeTime * PEAK_CHARGING_COST;
                                            if (!b.FindNode(trip.StartID).HasChargingStation)
                                            {
                                                chargeTime += BUS_CHARGING_PENALTY;
                                            }
                                            trip.BusAssigned.BusReadyTime = new Time(currentTime.ToString()) + chargeTime;
                                        }
                                        else
                                        {
                                            // Here we find an unassigned bus and assign it:
                                            Buses[i].Assigned = true;
                                            Buses[i].StopID = trip.StartID;
                                            Buses[i].TripID = trip.TripID;
                                            Buses[i].BusReadyTime = new Time(trip.EndTime.ToString());
                                            trip.HasBus = true;
                                            trip.BusAssigned = Buses[i];
                                            break;
                                        }
                                    }
                                }
                            }
                            if(!trip.HasBus)
                            {
                                // If we couldn't find a bus to assign then we delay the trip and check the next one:
                                trip.DelayTrip();
                                results[1]++;
                                continue;
                            }
                            #endregion
                            // After assigning a bus to the trip we then check the SoC of the bus. The bus needs to
                            // have enough charge to complete the trip without going below the minimum threshold and
                            // should start above or at the threshold given by the chromosome.
                            if (b.FindNode(trip.StartID).HasChargingStation)
                            {
                                #region If we can/it needs it, we charge the bus at the start
                                // Here we have access to a charging station, so we make sure that we start with a charge level above our threshold, 
                                // and have to end with a charge level above the safe minimum:
                                int chargeTime = 0;
                                trip.BusAssigned.IsCharging = true;
                                while (trip.BusAssigned.CurrentCharge - batteryDrain <= trip.BusAssigned.MaximumCapacity * _chromosome[2])
                                {
                                    ChargeBus(ref trip.BusAssigned);
                                    chargeTime++;
                                }
                                trip.BusAssigned.IsCharging = false;
                                if (chargeTime != 0)
                                {
                                    results[0] += chargeTime * PEAK_CHARGING_COST;
                                    int waitingTime = (trip.busStops[0].DepartureTime - trip.busStops[0].ArrivalTime).Minutes;
                                    if (chargeTime > waitingTime)
                                    {
                                        results[1] += chargeTime - waitingTime;
                                        for (int i = 0; i < chargeTime - waitingTime; i++)
                                        {
                                            trip.DelayTrip(true);
                                        }
                                    }
                                    //Console.WriteLine("Charged at start: " + trip.TripID);
                                }
                                if (Debug)
                                {
                                    Console.WriteLine("Station! " + trip.TripID + " " + trip.BusAssigned.CurrentCharge + " " + trip.BusAssigned.BusID + " " + currentTime.ToString());
                                }
                                #endregion
                            }
                            else if (b.FindNode(trip.EndID).HasChargingStation)
                            {
                                #region If we can't but it needs it, we send it to the depot to charge
                                if (trip.BusAssigned.CurrentCharge - batteryDrain <= trip.BusAssigned.MaximumCapacity * _chromosome[2])
                                {
                                    // We will violate the constraints of minimum charge! Therefore we will have to delay/penalise the trip and unassign the bus:
                                    trip.DelayTrip();
                                    results[1]++;
                                    trip.BusAssigned.StopID = "Depot";
                                    trip.BusAssigned.Assigned = false;
                                    trip.HasBus = false;
                                    int chargeTime = 0;
                                    trip.BusAssigned.IsCharging = true;
                                    chargeTime += ChargeBus(trip.BusAssigned, MAXIMUM_FAST_CHARGE);
                                    results[0] += chargeTime * PEAK_CHARGING_COST;
                                    chargeTime += BUS_CHARGING_PENALTY;
                                    trip.BusAssigned.BusReadyTime = new Time(currentTime.ToString()) + chargeTime;
                                    //Console.WriteLine("Sent to depot to charge: " + trip.TripID);
                                }
                                else
                                {

                                }
                                if (Debug)
                                {
                                    Console.WriteLine("No station! " + trip.TripID + " " + trip.BusAssigned.CurrentCharge + " " + trip.BusAssigned.BusID + " " + currentTime.ToString());
                                }
                                #endregion
                            }
                            else
                            {
                                // If we get here then there are no charging stations on the trip!
                                Console.WriteLine("No charging station at either end of this trip!");
                            }
                        }
                        else if (trip.EndTime == currentTime)
                        {
                            // Here we have reached the end of a trip and we can unassign/and or charge the bus
                            if (Debug)
                            {
                                Console.Write(trip.TripID + " " + trip.BusAssigned.BusID + " " + trip.BusAssigned.CurrentCharge + " - ");
                            }
                            trip.BusAssigned.CurrentCharge -= batteryDrain;
                            if (Debug)
                            {
                                Console.WriteLine(batteryDrain + " = " + trip.BusAssigned.CurrentCharge);
                            }
                            if(trip.BusAssigned.CurrentCharge < trip.BusAssigned.MaximumCapacity * MINIMUM_SAFE_CHARGE)
                            {
                                //Console.WriteLine(trip.BusAssigned.CurrentCharge + " " + batteryDrain);
                                Console.WriteLine("Trip ID:" + trip.TripID + " violated minimum safe charge! " + trip.BusAssigned.BusID);
                            }
                            if (trip.BusAssigned.CurrentCharge < 0)
                            {
                                Console.WriteLine("Violated laws of physics!");
                            }
                            trip.BusAssigned.StopID = "Depot";
                            trip.BusAssigned.Assigned = false;
                            // Penalty of an hour if can't charge at end?
                            int chargeTime = 0;
                            if (b.FindNode(trip.EndID).HasChargingStation)
                            {
                                // If we have a charging station then we charge it if needed:
                                while(trip.BusAssigned.CurrentCharge < trip.BusAssigned.MaximumCapacity * _chromosome[2])
                                {
                                    ChargeBus(ref trip.BusAssigned);
                                    chargeTime++;
                                }
                                results[0] += chargeTime * PEAK_CHARGING_COST;
                            }
                            else
                            {
                                // If we don't have a charging station we still check if the bus needs charging;
                                // if it does then we will penalise the bus by an hour plus the time to charge.
                                if (trip.BusAssigned.CurrentCharge < trip.BusAssigned.MaximumCapacity * _chromosome[2])
                                {
                                    trip.BusAssigned.IsCharging = true;
                                    chargeTime += ChargeBus(trip.BusAssigned, MAXIMUM_FAST_CHARGE);
                                    results[0] += chargeTime * PEAK_CHARGING_COST;
                                    chargeTime += BUS_CHARGING_PENALTY;
                                }
                            }
                            trip.BusAssigned.BusReadyTime = new Time(currentTime.ToString()) + chargeTime;
                        }
                    }

                    // Finally we update and check our escape condition:
                    currentTime++;
                    if (currentTime == (b.busTrips[0].StartTime))
                    {
                        break;
                    }
                }
                // Reset the bus stops/graph nodes
                //and here!
                // Reset the stops
                b.ResetGraphNodes();
            } // End of foreach bus route loop
            // Here we need to reset everything:

            Buses.Clear();
            BusRoutes.Clear();

            return results;
        }
    }

    /// <summary>
    /// The Bus Route class contains all the details for a single route of a bus network.
    /// </summary>
    class BusRoute
    {
        public List<GraphNode> graphNodes;
        public List<GraphEdge> graphEdges;
        public List<BusTrip> busTrips;
        public string RouteID;

        // Number of allocated buses
        // Current time?

        public BusRoute()
        {
            graphEdges = new List<GraphEdge>();
            graphNodes = new List<GraphNode>();
            busTrips = new List<BusTrip>();
            RouteID = "No Route ID";
        }

        public BusRoute(TimeTable _timeTable, Graph _graph)
        {
            graphEdges = new List<GraphEdge>();
            graphNodes = new List<GraphNode>();
            busTrips = new List<BusTrip>();
            ConvertStops(_graph.stops);
            ConvertEdges(_graph.edges);
            ConvertTrips(_timeTable.trips);
            // We grab the route ID from the first trip in our list:
            RouteID = _timeTable.trips[0].routeId;
        }

        public BusRoute(BusRoute _busRoute)
        {
            graphEdges = new List<GraphEdge>();
            foreach(GraphEdge edge in _busRoute.graphEdges)
            {
                graphEdges.Add(new GraphEdge(edge));
            }
            graphNodes = new List<GraphNode>();
            foreach(GraphNode node in _busRoute.graphNodes)
            {
                graphNodes.Add(new GraphNode(node));
            }
            busTrips = new List<BusTrip>();
            foreach(BusTrip trip in _busRoute.busTrips)
            {
                busTrips.Add(new BusTrip(trip));
            }
            // We grab the route ID from the first trip in our list:
            RouteID = busTrips[0].RouteID;
        }

        public GraphNode FindNode(string _ID)
        {
            foreach (GraphNode g in graphNodes)
            {
                if (g.ID == _ID)
                {
                    return g;
                }
            }
            return null;
        }

        public void ResetGraphNodes()
        {
            foreach (GraphNode g in graphNodes)
            {
                g.HasChargingStation = false;
                g.IsTerminalStop = false;
            }
        }

        /// <summary>
        /// Fill the node list by converting the stop container objects. 
        /// </summary>
        /// <param name="_stops">The array of stop objects to be converted</param>
        private void ConvertStops(Stop[] _stops)
        {
            foreach(Stop s in _stops)
            {
                graphNodes.Add(new GraphNode(s));
            }
        }

        /// <summary>
        /// Fill the edge list by converting the edge container objects. This needs to be called 
        /// after the stop objects have been converted.
        /// </summary>
        /// <param name="_edges">The array of edge objects to be converted</param>
        private void ConvertEdges(Edge[] _edges)
        {
            for (int i = 0; i < _edges.Length; i++ )
            {
                graphEdges.Add(new GraphEdge(_edges[i]));
                graphEdges[i].NodeA = graphNodes.Find(
                delegate(GraphNode node)
                {
                    return node.ID == _edges[i].anode;
                }
                );
                graphEdges[i].NodeB = graphNodes.Find(
                delegate(GraphNode node)
                {
                    return node.ID == _edges[i].bnode;
                }
                );
            }
        }

        /// <summary>
        /// Fill the list of bus trips by converting the container objects.
        /// </summary>
        /// <param name="_trips">The array of trips to be converted</param>
        private void ConvertTrips(Trip[] _trips)
        {
            foreach(Trip t in _trips)
            {
                busTrips.Add(new BusTrip(t));
            }

            GraphEdge temp = new GraphEdge();

            foreach(BusTrip b in busTrips)
            {
                b.GenerateGraphEdges(graphNodes);
                foreach (GraphEdge g in b.GraphEdges)
                {
                    b.TripDistance += g.Length;
                }
                b.AddStopDistances();
            }
        }

        /// <summary>
        /// A simple method that adds all trip distances
        /// </summary>
        /// <returns>The total distance of the route in km</returns>
        public float CalculateRouteDistance()
        {
            float distance = 0;
            foreach (BusTrip b in busTrips)
            {
                distance += b.TripDistance;
            }
            return distance;
        }
    }

    class Bus
    {
        // State of charge, current location/time, route/trip assigned to, cost
        public string StopID;
        public float MaximumCapacity;
        public float CurrentCharge;
        public string TripID;
        public string BusID;
        public bool Assigned = false;
        public Time BusReadyTime;
        public bool IsCharging = false;

        public Bus()
        {
            StopID = "Depot";
            TripID = "Unassigned";
            BusReadyTime = new Time("00:00:00");
        }
    }

    /// <summary>
    /// A Bus Trip is a single journey on a Bus Route
    /// </summary>
    class BusTrip
    {
        public string TripID;
        public string RouteID;
        public int Direction;
        public List<BusStop> busStops;
        public List<GraphEdge> GraphEdges;
        public float TripDistance;
        public Time StartTime;
        public Time EndTime;
        public string StartID;
        public string EndID;
        public bool HasBus = false;
        public Bus BusAssigned;

        public BusTrip()
        {
            RouteID = "No Route ID";
            TripID = "No Trip ID";
            Direction = 0;
            busStops = new List<BusStop>();
            TripDistance = 0;
            StartTime = new Time(0, 0, 0);
            EndTime = new Time(0, 0, 0);
            StartID = "No Start Selected";
            EndID = "No End Selected";
            BusAssigned = new Bus();
        }

        public BusTrip(Trip _trip)
        {
            RouteID = _trip.routeId;
            TripID = _trip.tripId;
            Direction = _trip.directionId;
            busStops = new List<BusStop>();
            ConvertStopTimes(_trip.stopTimes);
            GraphEdges = new List<GraphEdge>();
            TripDistance = 0;
            BusAssigned = new Bus();
        }

        public BusTrip(BusTrip _busTrip)
        {
            RouteID = _busTrip.RouteID;
            TripID = _busTrip.TripID;
            Direction = _busTrip.Direction;
            busStops = new List<BusStop>();
            foreach(BusStop busStop in _busTrip.busStops)
            {
                busStops.Add(new BusStop(busStop));
            }
            GraphEdges = new List<GraphEdge>();
            TripDistance = 0;
            BusAssigned = new Bus();
            StartID = _busTrip.StartID;
            StartTime = new Time(busStops[0].ArrivalTime.ToString());
            EndID = _busTrip.EndID;
            EndTime = new Time(busStops[busStops.Count - 1].ArrivalTime.ToString());
        }

        public void DelayTrip()
        {
            foreach(BusStop busStop in busStops)
            {
                busStop.ArrivalTime++;
                busStop.DepartureTime++;
            }
            StartTime++;
            EndTime++; 
        }

        public void DelayTrip(bool _skipStart)
        {
            for (int i = 0; i < busStops.Count; i++)
            {
                busStops[i].ArrivalTime++;
                busStops[i].DepartureTime++;
            }
            StartTime = busStops[0].DepartureTime;
            EndTime++;
        }

        /// <summary>
        /// This method generates the graph edges to connect the nodes for a given route.
        /// </summary>
        /// <param name="_graphNodes">The full list of graph nodes (though likely that only some will be needed)</param>
        public void GenerateGraphEdges(List<GraphNode> _graphNodes)
        {
            for (int i = 0; i < busStops.Count - 1; i++)
            {
                GraphEdges.Add(new GraphEdge(_graphNodes.Find(
                delegate (GraphNode node)
                {
                    return node.ID == busStops[i].StopID;
                }
                ),
                _graphNodes.Find(
                delegate (GraphNode node)
                {
                    return node.ID == busStops[i + 1].StopID;
                }
                )
                )
                );
                if(i == 0)
                {
                    // This is a terminal stop
                    _graphNodes.Find(
                    delegate (GraphNode node)
                    {
                        return node.ID == busStops[i].StopID;
                    }
                    ).IsTerminalStop = true;
                }
                if (i == busStops.Count - 2)
                {
                    // This is the other terminal stop
                    _graphNodes.Find(
                    delegate (GraphNode node)
                    {
                        return node.ID == busStops[i + 1].StopID;
                    }
                    ).IsTerminalStop = true;
                }
            }
        }

        /// <summary>
        /// Here we convert the containers to usable objects and sort them.
        /// </summary>
        /// <param name="_stopTimes">The array of stop times to be converted</param>
        private void ConvertStopTimes(StopTime[] _stopTimes)
        {
            List<StopTime> stopTimeList = _stopTimes.ToList();
            StopTimeComparer stopTimeComparer = new StopTimeComparer();
            stopTimeList.Sort(stopTimeComparer);
            foreach(StopTime s in stopTimeList)
            {
                busStops.Add(new BusStop(s));
            }
            //BusStopComparer busStopComparer = new BusStopComparer();
            //busStops.Sort(busStopComparer);
            StartID = busStops[0].StopID;
            StartTime = new Time(busStops[0].ArrivalTime.ToString());
            EndID = busStops[busStops.Count - 1].StopID;
            EndTime = new Time(busStops[busStops.Count - 1].ArrivalTime.ToString());
        }

        public void AddStopDistances()
        {
            float distance = 0;
            busStops[0].TripDistanceRemaining = TripDistance;
            for (int i = 1; i < busStops.Count; i++)
            {
                distance += GraphEdges[i - 1].Length;
                busStops[i].TripDistanceRemaining = TripDistance - distance;
            }
        }
    }

    /// <summary>
    /// A Bus Stop object represents a single stop on a Bus Trip
    /// </summary>
    class BusStop
    {
        public string TripID;
        public string StopID;
        public int StopSequence;
        public Time ArrivalTime;
        public int ArrivalSeconds;
        public Time DepartureTime;
        public int DepartureSeconds;
        public float TripDistanceRemaining { get; set; }
        public Time InitialArrivalTime;
        public Time InitialDepartureTime;

        public BusStop()
        {
            TripID = "No Trip ID";
            StopID = "No Stop ID";
            StopSequence = 0;
            ArrivalTime = new Time("00:00:00");
            ArrivalSeconds = 0;
            DepartureTime = new Time("00:00:00");
            DepartureSeconds = 0;
            InitialArrivalTime = new Time("00:00:00"); ;
            InitialDepartureTime = new Time("00:00:00"); ;
        }

        public BusStop(StopTime _stopTime)
        {
            TripID = _stopTime.tripId;
            StopID = _stopTime.stopId;
            StopSequence = _stopTime.stopSequence;
            ArrivalTime = new Time(_stopTime.arrivalTime);
            ArrivalSeconds = _stopTime.arrivalSeconds;
            DepartureTime = new Time(_stopTime.departureTime);
            DepartureSeconds = _stopTime.departureSeconds;
            InitialArrivalTime = new Time(_stopTime.arrivalTime);
            InitialDepartureTime = new Time(_stopTime.departureTime);
        }

        public BusStop(BusStop _busStop)
        {
            TripID = _busStop.TripID;
            StopID = _busStop.StopID;
            StopSequence = _busStop.StopSequence;
            ArrivalTime = new Time(_busStop.ArrivalTime.ToString());
            ArrivalSeconds = _busStop.ArrivalSeconds;
            DepartureTime = new Time(_busStop.DepartureTime.ToString());
            DepartureSeconds = _busStop.DepartureSeconds;
            InitialArrivalTime = new Time(_busStop.InitialArrivalTime.ToString());
            InitialDepartureTime = new Time(_busStop.InitialDepartureTime.ToString());
        }

        public void Reset()
        {
            ArrivalTime = InitialArrivalTime;
            InitialArrivalTime = new Time(ArrivalTime.ToString());
            DepartureTime = InitialDepartureTime;
            InitialDepartureTime = new Time(DepartureTime.ToString());
        }
    }

    /// <summary>
    /// A Graph Node is the physical location of a Bus Stop
    /// </summary>
    class GraphNode
    {
        public string ID;
        public string Name;
        public Vector2 Position;
        public bool HasChargingStation = false;
        public bool IsTerminalStop = false;

        public GraphNode()
        {
            ID = "No ID";
            Name = "No Name";
            Position = new Vector2(0f, 0f);
        }

        public GraphNode(Stop _stop)
        {
            ID = _stop.id;
            Name = _stop.caption;
            Position = new Vector2(_stop.lnglat[0], _stop.lnglat[1]);
        }

        public GraphNode(GraphNode _graphNode)
        {
            ID = _graphNode.ID;
            Name = _graphNode.Name;
            Position = new Vector2(_graphNode.Position.x, _graphNode.Position.y);
        }
    }

    /// <summary>
    /// A Graph Edge connects two Graph Nodes and represents a single leg of a Bus Trip
    /// </summary>
    class GraphEdge
    {
        public string ID;
        public float Length;
        public GraphNode NodeA;
        public GraphNode NodeB;
        public bool Directional = false;

        public GraphEdge()
        {
            ID = "No ID";
            Length = 0;
            NodeA = new GraphNode();
            NodeB = new GraphNode();
        }

        public GraphEdge(Edge _edge)
        {
            ID = _edge.id;
            Length = _edge.distance;
            Directional = _edge.directed;
            NodeA = new GraphNode();
            NodeB = new GraphNode();
        }

        public GraphEdge(GraphNode _nodeA, GraphNode _nodeB)
        {
            ID = _nodeA.ID + "-" + _nodeB.ID;
            NodeA = _nodeA;
            NodeB = _nodeB;
            Length = Vector2.DistanceBetweenPoints(_nodeA.Position.y, _nodeA.Position.x, _nodeB.Position.y, _nodeB.Position.x);
        }

        public GraphEdge(GraphEdge _graphEdge)
        {
            ID = _graphEdge.ID;
            Length = _graphEdge.Length;
            Directional = _graphEdge.Directional;
            NodeA = new GraphNode(_graphEdge.NodeA);
            NodeB = new GraphNode(_graphEdge.NodeB);
        }
    }
}
