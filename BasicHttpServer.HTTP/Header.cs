using System;

namespace BasicHttpServer.HTTP
{
    public class Header
    {
        public Header(string headerLine)
        {
            var headerParts = headerLine.Split(new[] { ": " }, 2, StringSplitOptions.None);
            Name = headerParts[0];
            Value = headerParts[1];
        }

        public Header(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }
    }
}