using Il2Cpp;
using Il2CppPantheonPersist;
using PantheonMetrics.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PantheonMetrics.Data;

public static class MetricsGroup
{
  private static EntityGroupObject _group = new EntityGroupObject();
  private static ActiveGroup? _currentGroup = null;

  public static EntityGroupObject RefreshGroupMembers()
  {
    _group = new EntityGroupObject();
    if (MetricsPlayer.PlayerGameObject == null)
      return _group;

    _group = new EntityGroupObject();
    Il2Cpp.Group.Logic group = MetricsPlayer.PlayerGameObject.Group;
    if (group == null) 
      return _group;

    _currentGroup = group.Current;

    if (_currentGroup == null)
      return _group;

    _group.GroupLeaderNetworkId = _currentGroup.Leader.NetworkId.Value.ToString();

    foreach (GroupMember? item in _currentGroup.members)
    {
      var relation = EntityRelationEnum.GroupMember;

      if (item.EntityNetworkId.Value.ToString() == MetricsPlayer.PlayerNetworkId)
        relation = EntityRelationEnum.LocalPlayer;
      _group.AddGroupMember(new EntityObject(item.EntityNetworkId.Value.ToString(), item.Name, item.Class.ToString(), relation));
    }   
    return _group;
  }

  public static EntityObject? GetGroupMember(string name) => _group.GroupMembers.Values.FirstOrDefault(g => g.DisplayName == name);

  public static bool InGroup() => _group.Any();

  public static EntityGroupObject GetGroup() => _group;

  public static string GetDisplayString()
  {
    if (InGroup()) 
      return "Not in a group";


    return _group.ToString();
  }

  public static string GetDisplayStringDebug()
  {
    if (InGroup())
      return "Not in a group";


    return _group.ToStringDebug();
  }


}
