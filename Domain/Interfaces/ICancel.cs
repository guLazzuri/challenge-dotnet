namespace challenge.Domain.Interfaces
{
    internal interface ICancel
    {
        public Guid UserCancelID { get; set; }
        public bool IsCancel {  get; set; }
    }
}
