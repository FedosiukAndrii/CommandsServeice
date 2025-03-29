namespace CommandsServeice.Interfaces
{
    public interface IEventParser
    {
        IEvent Parse(string json);
    }
}