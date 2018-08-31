namespace Common.UnitTestsBase
{
    public static class ExtensionMethods
    {
        public static T AssertNotNull<T>(this T component)
            where T : class
        {
            if (component == null)
            {
                throw new UnitTestsException($"Failed to get {typeof(T).Name} component."); 
            }

            return component;
        }
    }
}