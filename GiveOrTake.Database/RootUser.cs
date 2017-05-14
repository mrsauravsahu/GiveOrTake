namespace GiveOrTake.Database
{
    public partial class RootUser
    {
        public int Id { get; set; }
        public string Password { get; set; }

        public virtual User IdNavigation { get; set; }
    }
}
