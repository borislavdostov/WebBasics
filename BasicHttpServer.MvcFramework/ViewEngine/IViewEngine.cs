﻿namespace BasicHttpServer.MvcFramework.ViewEngine
{
    public interface IViewEngine
    {
        string GetHtml(string templateCode, object viewModel);
    }
}
