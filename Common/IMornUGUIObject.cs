using UnityEngine;

namespace MornLib
{
    public interface IMornUGUIObject
    {
        Transform Transform { get; }
        GameObject GameObject { get; }
    }
}