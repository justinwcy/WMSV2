namespace InboundService.Constants
{
    public enum IncomingStatus
    {
        OrderExpected = 0,
        OrderArrived = 1,
        OrderUnloading = 2,
        OrderReceived = 3,
        UnderInspection = 4,
        Accepted = 5,
        Rejected = -1,
        OrderOnHold = 6,
        PutAwayInProgress = 7,
        PutAwayCompleted = 8
    }
}
