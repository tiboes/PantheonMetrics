using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantheonMetrics.Objects;

public class EntityObject
{
  public EntityObject() { }

  public EntityObject(string networkId, string displayName, string entityClass, EntityRelationEnum relation, string owner)
  {
    Class = entityClass;
    NetworkId = networkId;
    DisplayName = displayName;
    Relation = relation;
    OwnerName = owner;
  }

  public string NetworkId { get; set; } = string.Empty;
  public string DisplayName { get; set; } = string.Empty;
  public string Class { get; set; } = string.Empty;
  public EntityRelationEnum Relation { get; set; } = EntityRelationEnum.None;

  public string OwnerName { get; set;  } = string.Empty;

  public bool IsPlayer() => Relation == EntityRelationEnum.LocalPlayer;


  public string ToStringDebug()
  {
    return $"{DisplayName}({Class}|{NetworkId}|{Relation})";
  }
  public override string ToString()
  {
    return $"{DisplayName}({Class})";
  }
}
