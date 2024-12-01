namespace BlazorNet9RenderModes.Client.Services
{
    public interface ICounterStateService
    {
        public int CounterManual { get; }

        public int GetCounterInit(string id);

        public void IncrementManual();
        public void IncrementInit(string id);

        string GetAllInit();
    }
}
