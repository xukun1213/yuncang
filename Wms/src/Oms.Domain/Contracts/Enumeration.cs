namespace Huayu.Oms.Domain.Contracts
{
    public abstract class Enumeration : IComparable
    {
        public string Name { get; private set; }
        public int Id { get; private set; }

        protected Enumeration(int id, string name) => (Id, Name) = (id, name);

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Static |
                System.Reflection.BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();

        public override bool Equals(object? obj)
        {
            if (obj is not Enumeration other) return false;


            var typeMatchs = GetType().Equals(obj.GetType());
            var valuesMatchs = Id.Equals(other.Id);

            return typeMatchs && valuesMatchs;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Id - secondValue.Id);
            return absoluteDifference;
        }

        public static T FromValue<T>(int value) where T : Enumeration
            => Parse<T, int>(value, "value", item => item.Id == value);

        public static T FromDisPlayName<T>(string displayName) where T : Enumeration
            => Parse<T, string>(displayName, "display name", item => item.Name == displayName);

        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);

        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
                throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

            return matchingItem;
        }
    }
}
