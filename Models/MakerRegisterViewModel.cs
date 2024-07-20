namespace Gifts_Store_First_project.Models
{
    public class MakerRegisterViewModel
    {
        public GiftUser User { get; set; }
        public IEnumerable<GiftCategory> Categories { get; set; }
    }
}
