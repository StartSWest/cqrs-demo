using Vueling.Otd.Domain.CurrencyPair.ValueObjects;

namespace Vueling.Otd.Domain.CurrencyPair.Models
{
    public class CurrencyPairGraph
    {
        // Stores all possible currency conversions from a currency standpoint.
        private readonly Dictionary<string, Dictionary<string, decimal>> _currencyOptions;

        /// <summary>
        /// Creates the currency graph from a currency pair list.
        /// </summary>
        /// <param name="currencyPairList">
        /// Flat list of currency conversion pair.
        /// </param>
        public CurrencyPairGraph(CurrencyPairList currencyPairList)
        {
            if (currencyPairList == null)
                throw new ArgumentNullException(nameof(currencyPairList));

            // Stores currency 'from' and all possible currencies 'to' and its rates.
            _currencyOptions = new Dictionary<string, Dictionary<string, decimal>>();
            foreach (var pair in currencyPairList.CurrencyPairs)
            {
                if (_currencyOptions.ContainsKey(pair.From))
                {
                    // Adds more currency conversion options for an already existing currency 'from'.
                    _currencyOptions[pair.From].Add(pair.To, pair.Rate);
                }
                else
                {
                    // Adds a new currency 'from' with its first currency conversion option.
                    _currencyOptions.Add(pair.From, new Dictionary<string, decimal> { [pair.To] = pair.Rate });
                }
            }
        }

        /// <summary>
        /// Tries to get the conversion rate from a source currency to a target currency even if there
        /// is not a direct conversion. Calculates the best possible conversion using alternate currencies
        /// conversions.
        /// </summary>
        /// <param name="from">Source currency</param>
        /// <param name="to">Target currency</param>
        /// <returns>
        /// The conversion rate from a source to a target currency or -1 for none.
        /// </returns>
        public decimal GetRate(string? from, string? to)
        {
            return new RoundedRate(GetRawRate(from, to)).HalfToEven();
        }

        private decimal GetRawRate(string? from, string? to)
        {
            if (from == null || to == null)
                return -1;

            if (!_currencyOptions.ContainsKey(from))
            {
                return -1;
            }

            var distance = new Dictionary<string, decimal> { [from] = 1 };
            var pairQueue = new SortedSet<string>(new[] { from }, new DistanceComparer(distance));
            var current = new HashSet<string>();

            decimal GetDistance(string key)
            {
                return distance.ContainsKey(key) ? distance[key] : decimal.MaxValue;
            }

            do
            {
                var c = pairQueue.First();
                pairQueue.Remove(c);

                if (c == to)
                {
                    return distance[c];
                }

                current.Remove(c);

                foreach (var pair in _currencyOptions[c])
                {
                    if (pair.Key == from)
                    {
                        continue;
                    }

                    var currencyTo = pair.Key;
                    var rate = pair.Value;

                    var totalRate = new RoundedRate(GetDistance(c) * rate).HalfToEven();
                    if (GetDistance(currencyTo) > totalRate)
                    {
                        if (current.Contains(currencyTo))
                        {
                            pairQueue.Remove(currencyTo);
                        }

                        distance[currencyTo] = totalRate;
                        pairQueue.Add(currencyTo);
                    }
                }
            }
            while (pairQueue.Count > 0);

            return -1;
        }

        private class DistanceComparer : IComparer<string>
        {
            private readonly IDictionary<string, decimal> _distance;

            public DistanceComparer(IDictionary<string, decimal> distance)
            {
                _distance = distance;
            }

            public int Compare(string? a, string? b)
            {
                if (a == null && b == null)
                    return 0;

                if (a == null)
                    return -1;

                if (b == null)
                    return 1;

                var aDistance = _distance.ContainsKey(a) ? _distance[a] : decimal.MaxValue;
                var bDistance = _distance.ContainsKey(b) ? _distance[b] : decimal.MaxValue;

                int comparer = aDistance.CompareTo(bDistance);

                if (comparer == 0)
                {
                    return a.CompareTo(b);
                }

                return comparer;
            }
        }
    }
}