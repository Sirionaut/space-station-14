using Content.Server.Speech.EntitySystems;

namespace Content.Server.Speech.Components;

/// <summary>
/// Android accent replaces contractions. cant -> cannot
/// </summary>
[RegisterComponent]
[Access(typeof(FrenchAccentSystem))]
public sealed partial class AndroidAccentComponent : Component
{
}
