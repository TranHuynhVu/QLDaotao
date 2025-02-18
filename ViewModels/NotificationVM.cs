namespace QLDaoTao.ViewModels
{
    public class NotificationVM
    {
        public int Id { get; set; }
        public string Name { get; set; }         
        public string Title { get; set; }      
        public string NoiDung { get; set; }      
        public DateTime? NgayTao { get; set; }    
        public DateTime? NgayDoc { get; set; }   
        
        public int Status { get; set; }

        public int? CountStatus { get; set; }
    }

}
