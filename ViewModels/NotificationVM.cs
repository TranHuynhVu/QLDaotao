namespace QLDaoTao.ViewModels
{
    public class NotificationVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }         
        public string? Title { get; set; }      
        public string? Description { get; set; }      
        public DateTime? CreatedAt { get; set; }    
        public DateTime? ReadAt { get; set; }
        public string? Receiver { get; set; }   
        public int Status { get; set; }
        public int? CountStatus { get; set; }
    }

}
