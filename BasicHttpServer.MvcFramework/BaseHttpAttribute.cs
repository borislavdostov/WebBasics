using System;
using BasicHttpServer.HTTP;

namespace BasicHttpServer.MvcFramework
{
    public abstract class BaseHttpAttribute : Attribute
    {
        public string Url { get; set; }
        public abstract HttpMethod Method { get; }
    }
}
