using System.Text.RegularExpressions;

namespace NyapFetch
{
  public static class Info
  {
    private static string? s_osInfo;
    public static string GetOS()
    {
      s_osInfo = Environment.OSVersion.ToString();
      return s_osInfo;
    }

    private static string? s_cpuInfo;
    public static string GetCPU()
    {
      var file = File.OpenRead("/proc/cpuinfo");
      var stream = new StreamReader(file);

      while (!stream.EndOfStream)
      {
        string? line = stream.ReadLine();
        if (line != null && line.Contains("model name"))
        {
          s_cpuInfo = line;
          break;
        }
      }
      
      if (s_cpuInfo == null)
      {
        s_cpuInfo = "Undefined";
      }
      else
      {
        s_cpuInfo = Regex.Replace(s_cpuInfo, "(model name	\\: )", String.Empty);
      }

      return s_cpuInfo;
    }

    private static string? s_ramInfo;
    public static string GetRAM()
    {
      var file = File.OpenRead("/proc/meminfo");
      var stream = new StreamReader(file);

      string? ramtotal = null;
      string? ramfree = null;

      while (!stream.EndOfStream)
      {
        string? line = stream.ReadLine();
        
        if (line != null && line.Contains("MemTotal"))
        {
           ramtotal = line;
        }
        if (line != null && line.Contains("MemFree"))
        {
          ramfree = line;
        }
      }
      
      ramtotal = Regex.Replace(ramtotal, "(MemTotal\\:)", String.Empty).Trim();
      ramfree = Regex.Replace(ramfree, "(MemFree\\:)", String.Empty).Trim();

      s_ramInfo = ramfree + " / " + ramtotal;

      if (s_ramInfo == null)
      {
        s_ramInfo = "Undefined";
      }

      return s_ramInfo;
    }
  }
}
