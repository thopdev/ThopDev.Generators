using System.Collections.Generic;

namespace ThopDev.Generator.Blazor.Routes.Models.Routing
{

    public class Route
    {
        private readonly List<string> _segments = new List<string>();

        public void Add(string segment)
        {
            _segments.Add(segment);
        }

        public override string ToString()
        {
            return "/" + string.Join("/", _segments);
        }
    }
}