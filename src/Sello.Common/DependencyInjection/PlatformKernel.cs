using Ninject;

namespace Sello.Common.DependencyInjection
{
    public static class PlatformKernel
    {
        public static StandardKernel Instance { get; } = new StandardKernel();
    }
}