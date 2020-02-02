namespace Common.UnitTestsBase
{
    public static class ExtensionMethods
    {
        public static TComponent AssertNotNull<TComponent>(this TComponent component) 
            where TComponent : class
        {
            if (component == null)
            {
                throw new FailedToGetComponentException(typeof(TComponent).Name);
            }

            return component;
        }
    }
}