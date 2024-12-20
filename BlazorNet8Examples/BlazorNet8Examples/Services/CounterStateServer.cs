﻿using System.Text;

using BlazorNet8RenderModes.Common;

namespace BlazorNet8RenderModes.Services
{
    public class CounterStateServer : ICounterStateService
    {
        private readonly Dictionary<string, int> _counterInit = new();

        public CounterStateServer()
        {
            CounterManual = 0;
        }

        public int CounterManual { get; private set; }

        public int GetCounterInit(string id)
        {
            return _counterInit.GetValueOrDefault(id, 0); // Return 0 if the id doesn't exist in the dictionary
        }

        public string GetAllInit()
        {
            StringBuilder sb = new();
            foreach (var item in _counterInit)
            {
                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }

                if (item.Value > 1)
                {
                    sb.Append($"{item.Key}={item.Value}");
                }
                else
                {
                    sb.Append($"{item.Key}");
                }
            }
            return sb.ToString();
        }

        public void IncrementManual()
        {
            CounterManual++;
        }

        public void IncrementInit(string id)
        {
            if (!_counterInit.TryAdd(id, 1))
            {
                _counterInit[id]++;
            }
        }
    }
}
