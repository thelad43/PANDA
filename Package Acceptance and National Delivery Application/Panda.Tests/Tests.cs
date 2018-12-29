namespace Panda.Tests
{
    using Common.Mapping;
    using Panda.Services;

    public class Tests
    {
        private static bool testsInitialized = false;

        public static void Initialize()
        {
            if (!testsInitialized)
            {
                AutoMapperProfile.RegisterMappings(typeof(IService).Assembly);
                testsInitialized = true;
            }
        }
    }
}