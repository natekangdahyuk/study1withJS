using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class AbstactReferenceContainer
{

    public void Add(IReferenceDataByKey add_data)
    {
        if (true == _ReferenceContainer_By_Key.ContainsKey(add_data.GetKey().ToString()))
            return;

        _ReferenceContainer_By_Key.Add(add_data.GetKey().ToString(), add_data);
    }

    public void AddByGroup(IReferenceDataByGroup add_data)
    {
        List<IReferenceDataByGroup> current_group_list = null;

        if (false == _ReferenceContainer_By_Group.TryGetValue(add_data.GetGroupKey().ToString(), out current_group_list))
        {
            current_group_list = new List<IReferenceDataByGroup>();

            _ReferenceContainer_By_Group.Add(add_data.GetGroupKey().ToString(), current_group_list);
        }


        current_group_list.Add(add_data);
    }


    public IReferenceDataByKey Find(object key)
    {
        IReferenceDataByKey find_data = null;

        if (true == _ReferenceContainer_By_Key.TryGetValue(key.ToString(), out find_data))
            return find_data;


        return null;
    }

    public List<IReferenceDataByGroup> FindByGroup(object key)
    {
        List<IReferenceDataByGroup> find_data = null;

        if (true == _ReferenceContainer_By_Group.TryGetValue(key.ToString(), out find_data))
            return find_data;

        return null;
    }


    public void Release()
    {
        _ReferenceContainer_By_Key.Clear();
        _ReferenceContainer_By_Group.Clear();
    }




    protected Dictionary<string, IReferenceDataByKey> _ReferenceContainer_By_Key = new Dictionary<string, IReferenceDataByKey>();

    protected Dictionary<string, List<IReferenceDataByGroup>> _ReferenceContainer_By_Group = new Dictionary<string, List<IReferenceDataByGroup>>();

}
