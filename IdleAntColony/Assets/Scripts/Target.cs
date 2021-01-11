using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private readonly List<Transform> _remainingPieces = new List<Transform>();

    private void Awake()
    {
        var root = transform.GetChild(0);
        foreach (var child in root.GetComponentsInChildren<Transform>())
        {
            if (child == root) continue;
            _remainingPieces.Add(child);
        }
    }

    public bool GetNextPiece(Vector3 origin, out Transform closestPiece)
    {
        closestPiece = null;
        if (_remainingPieces.Count == 0) return false;

        var minDistance = float.MaxValue;
        foreach (var piece in _remainingPieces)
        {
            float distance = Vector3.Distance(origin, piece.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPiece = piece;
            }
        }

        if (closestPiece)
        {
            _remainingPieces.Remove(closestPiece);
            return true;
        }

        return false;
    }
}