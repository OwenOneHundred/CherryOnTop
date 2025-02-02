using UnityEngine;

/// <summary>
/// Attached to Cherries. Moves them along the track. Should only interact with the debuff manager on this Cherry,
/// and only to access a function to get a multiplier for movement speed, to allow for slow effects.
/// </summary>
public class CherryMovement : MonoBehaviour
{
    /// The track should be a (maybe disabled) linerenderer.
    /// The cherries should move from one point on the linerenderer to the next, which allows for easy
    /// track path adjustments. However, there should be a way to freeze cherries normal movement and have them
    /// "jump" to a new location. That's how they will move to a new layer, and it is important for level design.
    
    
}
