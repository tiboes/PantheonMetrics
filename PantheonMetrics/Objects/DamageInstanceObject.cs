using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantheonMetrics.Objects;

public record DamageInstanceObject(DateTime Time, string AttackerName, string DefenderName, int Damage, float MitigatedDamage, string DamageAbility, string DamageType, string Result);
