namespace WSF.Modules
{
    internal interface IWSFModuleManager
    {
        void InitializeModules();

        void ShutdownModules();
    }
}