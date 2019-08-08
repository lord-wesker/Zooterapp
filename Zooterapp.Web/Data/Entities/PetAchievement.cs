namespace Zooterapp.Web.Data.Entities
{
    public class PetAchievement
    {
        public int Id { get; set; }

        public int PetId { get; set; }
        public Pet Pet { get; set; }

        public int AchievementId { get; set; }

        public Achievement Achievement { get; set; }
    }
}
