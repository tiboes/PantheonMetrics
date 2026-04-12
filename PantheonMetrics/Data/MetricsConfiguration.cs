using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantheonMetrics.Data;

public static class MetricsConfiguration
{
  public static bool BreathWarningEnabled { get; set; } = true;
  public static bool ExperienceMetricEnabled { get; set; } = true;
  public static bool CombatTrackingEnabled { get; set; } = true;

}
