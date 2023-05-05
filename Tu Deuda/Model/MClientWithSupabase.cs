namespace Tu_Deuda.Model
{
    public class MClientWithSupabase
    {
        public MClient MClient { get; set; }
        public MClientSupabase MClientSupabase { get; set; }

        public MClientWithSupabase(MClient client, MClientSupabase clientSupabase)
        {
            MClient = client;
            MClientSupabase = clientSupabase;
        }
    }
}