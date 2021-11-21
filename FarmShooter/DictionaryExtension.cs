namespace FarmShooter
{
    public static class DictionaryExtension
    {
        public static T GetValueOrSpecificDefault<TKey, T>(this Dictionary<TKey, T> dict, TKey key, T SD)
            => dict.TryGetValue(key, out var value) ? value : SD;
    }
}
