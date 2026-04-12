using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantheonMetrics.Data;

public static class MetricsBookKeeper
{
  public static int Counter {  get; set; } = 0;

  public static int AccumulatedDamage { get; set; } = 0;

  public static float AccumulatedMitigatedDamage { get; set; } = 0;
}
