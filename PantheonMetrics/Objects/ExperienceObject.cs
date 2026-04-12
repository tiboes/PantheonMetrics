using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantheonMetrics.Objects;

public  class ExperienceObject
{
  private long _ticks;
  private DateTime _time;
  public int Experience { get; set; }

  public EntityObject ExperienceSource { get; set; }
  public DateTime EarnedTime => _time;
  public long EarnedTimeTicks => _ticks;

  public ExperienceObject(int experience, EntityObject experienceSource)
  {
    ExperienceSource = experienceSource;
    Experience = experience;
    _ticks = DateTime.UtcNow.Ticks;
    _time = new DateTime(_ticks);
  }

  public override string ToString() => $"{Experience}-{_time:yyyy-MM-dd HH:mm:ss} from {ExperienceSource}";
  public string ToStringDebug() => $"{Experience}-{_time:yyyy-MM-dd HH:mm:ss:fff}|{_ticks}, from {ExperienceSource.ToStringDebug()}";


}
