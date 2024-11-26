using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ComboData", menuName = "Data/Combo", order = 1)]
public class ComboData : ScriptableObject
{
    [SerializeField] private int _attacksAnimationSpeed = 2;
    [SerializeField] ComboCommandNode _comboRoot;

    public int Speed => _attacksAnimationSpeed;
    public ComboCommandNode ComboList => _comboRoot;

    public ComboCommandNode GetComboByIndex(int index)
    {
        return SearchNodeInChildrens(index, _comboRoot.Children);
    }

    private ComboCommandNode SearchNodeInChildrens(int indexToFind, List<ComboCommandNode> childrenNodes)
    {
        if(indexToFind == _comboRoot.Index)
        {
            return _comboRoot;
        }

        foreach (ComboCommandNode node in childrenNodes)
        {
            if (node.Index == indexToFind)
            {
                return node;
            }
            if (node.Index > indexToFind)
            {
                return SearchNodeInChildrens(indexToFind, node.Children);
            }
        }
        Debug.Log("#5 something really wrong happened with index: " + indexToFind);
        return new ComboCommandNode();
    }
}

[Serializable]
public struct ComboCommandNode
{
    public string Command;
    public int ActivationStartFrame;
    [FormerlySerializedAs("ActivationEndTime")] public int ActivationEndFrame;
    public HitData HitData;
    public int Index;
    public List<ComboCommandNode> Children;
}
