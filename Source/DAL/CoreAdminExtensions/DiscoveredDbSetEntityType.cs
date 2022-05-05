namespace DAL.CoreAdminExtensions
{
    public class DiscoveredDbSetEntityType
    {
        public Type DbContextType { get; set; }
        public string Name { get; set; }
        public Type DbSetType { get; set; }
        public Type UnderlyingType { get; set; }
    }
}
