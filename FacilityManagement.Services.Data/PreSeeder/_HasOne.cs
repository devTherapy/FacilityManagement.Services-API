
using System;
using System.Collections.Generic;
using System.Threading;

namespace FacilityManagement.Services.Data.PreSeeder
{
    public partial class Seeder
    {
        private class _HasOne<T, T1>  where T : class where T1 : class
        {
            public List<T> FromOne { get; set; }
            public List<T1> ToOne { get; set; }
            public DataContext AppDbContext { get; }

            public _HasOne(string fromOne, string toOne, DataContext appDbContext)
            {
                AppDbContext = appDbContext;
                FromOne = TransformJsonToPoco<T>(appDbContext,fromOne);
                ToOne = TransformJsonToPoco<T1>(appDbContext,toOne);
            }

            public void SetFK(Action<dynamic, dynamic> setRelation)
            {
                var models = new List<T>();
                for (int i = 0; i < FromOne.Count; i++)
                {
                    var from = FromOne[i];
                    var toOne = ToOne[i];
                    setRelation(from, toOne);
                    models.Add(from);
                }
                AppDbContext.Set<T>().UpdateRange(models);
                try
                {
                    var res = AppDbContext.SaveChangesAsync(new CancellationToken()).Result;
                    Console.WriteLine($"HasOne:{res} rows created .");
                }
                catch (Exception)
                {
                    Console.WriteLine($"Can`t create Records for {typeof(T)} and {typeof(T1)}, Consider droping existing database.");
                }
            }
        }
    }
}