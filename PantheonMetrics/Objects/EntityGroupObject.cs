using Il2CppPantheonPersist;
using PantheonMetrics.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantheonMetrics.Objects;

public class EntityGroupObject
{
  public string GroupLeaderNetworkId { get; set; } = string.Empty;
  public Dictionary<string, EntityObject> GroupMembers { get; set; } = new Dictionary<string, EntityObject>();

  public EntityObject GetGroupMember(string networkId)
  {
    return GroupMembers[networkId];
  }

  public void AddGroupMember(EntityObject entity)
  {
    GroupMembers.Remove(entity.NetworkId);
    GroupMembers.Add(entity.NetworkId, entity);
  }

  public bool Any() => GroupMembers.Any();

  public IEnumerable<EntityObject> GetGroupMembers() 
  { 
    return GroupMembers.Values;
  }

  public IEnumerable<string> GetGroupMemberNetworkIds()
  {
    return GroupMembers.Keys;
  }

  public EntityObject GetGroupLeader()
  {
    return GroupMembers[GroupLeaderNetworkId];
  }

  public override string ToString()
  {
    if (!Any()) 
      return "No Group";

    return $"*{GetGroupLeader()}* - {String.Join("|",GetGroupMembers().Where(x=>x.NetworkId != GroupLeaderNetworkId))}";
  }
  public string ToStringDebug()
  {
    if (!Any())
      return "No Group";

    return $"*{GetGroupLeader().ToStringDebug}* - {String.Join("|", GetGroupMembers().Where(x => x.NetworkId != GroupLeaderNetworkId).Select(y=> y.ToStringDebug()))}";
  }

}
