namespace DoctorWho.Db.Domain
{
 

    public class CRUDModel
    {
        public DoctorWhoCoreDbContext Context { get; set; }

        public CRUDModel(DoctorWhoCoreDbContext context)
        {
            if (context != null)
                Context = context;
            else
                Context = new DoctorWhoCoreDbContext();
        }

        public void Add()
        {
            Context.Add(this);
            Context.SaveChanges();
        }

        public void Update()
        {
            Context.Update(this);
            Context.SaveChanges();
        }

        public void Delete()
        {
            Context.Remove(this);
            Context.SaveChanges();
        }
    }
}