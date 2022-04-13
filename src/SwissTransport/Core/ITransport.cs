namespace SwissTransport.Core
{
    using System;
    using SwissTransport.Models;

    public interface ITransport
    {
        Stations GetStations(string query);

        StationBoardRoot GetStationBoard(string station, string id);

        Connections GetConnections(string fromStation, string toStation);

        Connections GetConnections(string fromStation, string toStation, DateTime date);
    }
}