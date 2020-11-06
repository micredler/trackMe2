using System;
using System.Collections.Generic;

public class FramedVehicleJourneyRef
{
    public string DataFrameRef { get; set; }
    public string DatedVehicleJourneyRef { get; set; }
}

public class VehicleLocation
{
    public string Longitude { get; set; }
    public string Latitude { get; set; }
}

public class MonitoredCall
{
    public string StopPointRef { get; set; }
    public string Order { get; set; }
    public DateTime ExpectedArrivalTime { get; set; }
    public string DistanceFromStop { get; set; }
    public DateTime? AimedArrivalTime { get; set; }
}

public class MonitoredVehicleJourney
{
    public string LineRef { get; set; }
    public string DirectionRef { get; set; }
    public FramedVehicleJourneyRef FramedVehicleJourneyRef { get; set; }
    public string PublishedLineName { get; set; }
    public string OperatorRef { get; set; }
    public string DestinationRef { get; set; }
    public DateTime OriginAimedDepartureTime { get; set; }
    public VehicleLocation VehicleLocation { get; set; }
    public string Bearing { get; set; }
    public string Velocity { get; set; }
    public string VehicleRef { get; set; }
    public MonitoredCall MonitoredCall { get; set; }
}

public class MonitoredStopVisit
{
    public DateTime RecordedAtTime { get; set; }
    public string ItemIdentifier { get; set; }
    public string MonitoringRef { get; set; }
    public MonitoredVehicleJourney MonitoredVehicleJourney { get; set; }
}

public class StopMonitoringDelivery
{
    public string Version { get; set; }
public DateTime ResponseTimestamp { get; set; }
public string Status { get; set; }
public IList<MonitoredStopVisit> MonitoredStopVisit { get; set; }
    }

    public class ServiceDelivery
{
    public DateTime ResponseTimestamp { get; set; }
    public string ProducerRef { get; set; }
    public string ResponseMessageIdentifier { get; set; }
    public string RequestMessageRef { get; set; }
    public string Status { get; set; }
    public IList<StopMonitoringDelivery> StopMonitoringDelivery { get; set; }
}

public class Siri
{
    public ServiceDelivery ServiceDelivery { get; set; }
}

public class ApiResponse
{
    public Siri Siri { get; set; }
}