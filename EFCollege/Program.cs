using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;

namespace EFCollege
{
    [Table("Teacher")]
    public class Teacher
    {
        [Key]
        public int Id;
        public string teacherName { get; set; }
        public virtual IEnumerable<Course> CourseList { get; set; }
    }

    [Table("Student")]
    public class Student
    {
      
        [Key]
        int Id;
        string studentName { get; set; }
        //int courseId { get; set; }
        //int gradeId { get; set; }
        public virtual IEnumerable<Course> CourseList { get; set; }
        public virtual IEnumerable<Grade> GradesList { get; set; }
    }

    [Table("Course")]
    public class Course
    {
        [Key]
        int id;
        string courseName { get; set; }
        //int teacherId;
        //int studentId;
        public Teacher TeacherRecord { get; set; }
        public virtual IEnumerable<Student> studentRecord { get; set; }
    }

    [Table("Grade")]
    public class Grade
    {
        [Key]
        int Id;
        float score { get; set; }
        float maxScore { get; set; }
        //int studentId;
        //int courseId;
        public Student studentId { get; set; }
        public Course courseId { get; set; }
    }
    public class contextClass : DbContext
    {
            public DbSet<Teacher> TeacherSet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>
            (entity =>
            {
                //entity.ToTable("Applicant_Educations");
                //entity.Property(_ => _.StartDate.HasColumnName("Start_Date").HasColumnType("date");
                //For single column primary key
                entity.HasKey(e => e.Id);
                //MultiColumn Composite key
                //entity.HasKey(e => new { e.Id, e.Major });
                entity.HasMany(e => e.CourseList)
    .WithOne(p => p.TeacherRecord);
               // entity.Property(e => e.TimeStamp).IsRowVersion();
                //Alternatively in Poco class use //[NotMapped]
            });
            base.OnModelCreating(modelBuilder);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
                   }


    }
    public class Program<T> where T : class
    {
        private contextClass _context;
        public Program()
        {
            _context = new contextClass();
        }
        public void Add(params T[] items)
        {
            foreach (T item in items)
            {
                _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            _context.SaveChanges();
        }
        public IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> dbQuery = _context.Set<T>();
            foreach (Expression<Func<T, object>> property in navigationProperties)
            {

                dbQuery = dbQuery.Include<T, object>(property);
            }
            return dbQuery.ToList();

        }

        public IList<T> GetList(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> dbQuery = _context.Set<T>();
            foreach (
                Expression<Func<T, object>> property in navigationProperties)
            {
                dbQuery = dbQuery.Include<T, object>(property);

            }
            return dbQuery.Where(where).ToList<T>();
        }

        public T GetSingle(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> dbQuery = _context.Set<T>();
            foreach (
                Expression<Func<T, object>> property in navigationProperties)
            {
                dbQuery = dbQuery.Include<T, object>(property);
            }
            return dbQuery.Where(where).FirstOrDefault();
        }

        public void Remove(params T[] items)
        {
            foreach (T item in items)
            {
                _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }
            _context.SaveChanges();
        }

        public void Update(params T[] items)
        {
            foreach (T item in items)
            {
                _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            _context.SaveChanges();
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

