using AknakeresoWF.Design;

namespace AknakeresoWF
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            
            Application.Run(new Menu());
        }
    }
}