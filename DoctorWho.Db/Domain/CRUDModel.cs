namespace DoctorWho.Db.Domain
{
 

    public class CRUDModel
    {
        public void Add()
        {
            using var context = new DoctorWhoCoreDbContext();
            context.Add(this);
            context.SaveChanges();
        }

        public void Update()
        {
            using var context = new DoctorWhoCoreDbContext();
            context.Update(this);
            context.SaveChanges();
        }

        public void Delete()
        {
            using var context = new DoctorWhoCoreDbContext();
            context.Remove(this);
            context.SaveChanges();
        }
    }
}