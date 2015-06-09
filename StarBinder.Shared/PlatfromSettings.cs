namespace StarBinder
{
    public class PlatfromSettings
    {
        public bool IsStoreApp 
        { 
            get 
            {
#if NETFX_CORE
                return true;
#else
                return false;
#endif                
            } 
        }

    }
}
