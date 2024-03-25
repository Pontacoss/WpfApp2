namespace WpfApp2.Domain.ValueObjects
{
    public sealed class ModelName : ValueObject<ModelName>
    {
        public string Value { get; }

        public ModelName(string value)
        {
            Value = value;
        }

        protected override bool EqualsCore(ModelName other)
        {
            return this.Value == other.Value;
        }
    }
}
