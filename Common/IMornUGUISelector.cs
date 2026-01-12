using UnityEngine;

namespace MornLib
{
    public interface IMornUGUISelector
    {
        Vector2Int ValueRange { get; }
        int Value { get; }
    }
}