namespace Reports.Persistence
{
    public static class DbInitializer
    {
        public static void Initialize(ReportsDbContext context) =>
            context.Database.EnsureCreated();
    }
}