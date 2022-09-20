namespace Wms.Admin.BlazorServerApp
{
    public class LogEvent
    {
        public const int Event1 = 1000;
        public const int Event2 = 1001;
        public static EventId EventInfo = new(1002, "event——info");
    }
}
