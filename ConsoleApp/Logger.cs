using log4net;
using log4net.Config;
using System;

namespace ConsoleApp
{
  public class Logger
  {
    private readonly ILog _Log = LogManager.GetLogger("LOGGER");

    public Logger()
    {
      _Log = LogManager.GetLogger("LOGGER");

      XmlConfigurator.Configure();
    }

    public void Info(object message)
    {
      _Log.Info(message);
    }

    public void Info(object message, Exception ex)
    {
      _Log.Info(message, ex);
    }

    public void Error(object message)
    {
      _Log.Error(message);
    }

    public void Error(object message, Exception ex)
    {
      _Log.Error(message, ex);
    }
  }
}
