using System.Text.RegularExpressions;
using Content.Server.Speech.Components;
using Robust.Shared.Random;

namespace Content.Server.Speech.EntitySystems;

/// <summary>
///     Accent that replaces english contractions in messages.
/// </summary>
public sealed class AndroidAccentSystem : EntitySystem
{
    private static readonly Dictionary<string, string> SpecialPhrases = new Dictionary<string, string>
    {
        // Special cases that are done before the regular replacements
        { @"\bwon't\b\s+(they|you|he|she|it)", "will $1 not" },
        { @"\bwouldn't it be\b", "would it not be" },
        { @"\bcouldn't it be\b", "could it not be" },
        { @"\bshouldn't it be\b", "should it not be" },
        { @"\bwouldn't you\b", "would you not" },
        { @"\bcouldn't you\b", "could you not" },
        { @"\bshouldn't you\b", "should you not" },
        { @"\bhasn't it been\b", "has it not been"},
        { @"\bhasn't (he|she|it)\b", "has $1 not" },
        { @"\bhadn't (he|she|it)\b", "had $1 not" },
        { @"\bisn't it\b", "is it not" },
        { @"\baren't I\b", "am I not" },
    };
    private static readonly Dictionary<string, string> Contractions = new Dictionary<string, string>
    {
        // Contraction patterns
        { @"\b(would|should|could|is|has|does|did|do|had|were|are|was|have|must)n't\b", "$1 not" },
        { @"\b(he|she|it|that|there|what)'s\b", "$1 is" },
        { @"\b(you|we|they)'re\b", "$1 are" },
        { @"\b(i|you|we|they|there|who|what)'ve\b", "$1 have" },
        { @"\b(i|you|we|they|there|who|what)'d\b", "$1 would" },
        { @"\b(i|you|we|they|there|who|what)'ll\b", "$1 will" },

        // General contractions
        { @"\bI'm\b", "I am" },
        { @"\bcan't\b", "can not" },
        { @"\bwon't\b", "will not" },
        { @"\bhow's\b", "how is" },
        { @"\bhow've\b", "how have" },
        { @"\bhow'd\b", "how would" },
        { @"\bhow'll\b", "how will" },
        { @"\bshan't\b", "shall not" },
        { @"\bhadn't\b", "had not" },
    };
    private static readonly List<(Regex regex, string replacement)> SpecialPhrasePatterns = new List<(Regex, string)>();
    private static readonly List<(Regex regex, string replacement)> ContractionPatterns = new List<(Regex, string)>();
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AndroidAccentComponent, AccentGetEvent>(OnAccentGet);
        // Compile regex patterns
        foreach (var (pattern, replacement) in SpecialPhrases)
        {
            SpecialPhrasePatterns.Add((new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase), replacement));
        }

        foreach (var (pattern, replacement) in Contractions)
        {
            ContractionPatterns.Add((new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase), replacement));
        }
    }

    private void OnAccentGet(EntityUid uid, AndroidAccentComponent component, AccentGetEvent args)
    {
        args.Message = ReplaceContractions(args.Message);
    }

    /// <summary>
    ///     Replaces contractions in a message.
    /// </summary>
    /// <param name="message">Original message with contractions.</param>
    /// <returns>Message with contractions replaced.</returns>
    public string ReplaceContractions(string message)
    {
        // Apply special phrases first
        foreach (var (regex, replacement) in SpecialPhrasePatterns)
        {
            message = regex.Replace(message, replacement);
        }
        // Apply general contractions
        foreach (var (regex, replacement) in ContractionPatterns)
        {
            message = regex.Replace(message, replacement);
        }
        return message;
    }
}
