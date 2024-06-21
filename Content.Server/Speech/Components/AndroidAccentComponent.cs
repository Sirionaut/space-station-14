using Content.Server.Speech.EntitySystems;

namespace Content.Server.Speech.Components;

/// <summary>
///     Android accent that replaces contractions, e.g. can't -> can not.
/// </summary>
[RegisterComponent]
[Access(typeof(AndroidAccentSystem))]
public sealed partial class AndroidAccentComponent : Component
{
}
