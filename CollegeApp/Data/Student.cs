namespace CollegeApp.Data
{
    public class Student
    {
        // [Key] // Primary Key  // Added configuration file
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Identity seed or auto genarated column
        public int Id { get; set; } // recognize as a primary key column
        public string StdName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime DOB { get; set; }
    }
}
