using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace eu.bayly.EADBasicNet.OurAirports {
  [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
  public class DataContext : DbContext {
    public DbSet<Airport> Airports { get; set; }

    public DbSet<Country> Countries { get; set; }

    public DbSet<Frequency> Frequencies { get; set; }

    public DbSet<NavAid> NavAids { get; set; }

    public DbSet<Region> Regions { get; set; }

    public DbSet<Runway> Runways { get; set; }

    public DataContext() : base() {
      Configuration.LazyLoadingEnabled = false;
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);

      Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataContext>());
      //var sqliteConnectionInitializer = new SqliteDropCreateDatabaseWhenModelChanges<TestContext>(modelBuilder);
      //Database.SetInitializer(sqliteConnectionInitializer);
      //Database.SetInitializer(new DropCreateDatabaseAlways<TestContext>());
    }

    protected DbSet<T> GetDbSet<T>() where T: class, new() {
      // Find the property whose DbSet matches the type passed
      DbSet<T> set = null;
      foreach (var prop in this.GetType().GetProperties()) {
        if (prop.PropertyType == typeof(DbSet<T>)) {
          return (DbSet<T>)prop.GetValue(this);
        }
      }

      throw new ArgumentException("Type not found.", "T");
    }

    public void Import<T>() where T : class, new() {
      var req = (HttpWebRequest)HttpWebRequest.Create(typeof(T).GetCustomAttribute<UriAttribute>().Uri);
      using (var resp = (HttpWebResponse)req.GetResponse()) {
        using (var stream = resp.GetResponseStream()) {
          Import<T>(stream);
        }
      }
    }

    public void Import<T>(FileInfo file) where T : class, new() {
      using (var fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read)) {
        Import<T>(fs);
      }
    }

    public void Import<T>(Stream stream) where T : class, new() {
      // Properties, and the associated column mappings
      var props = typeof(T).GetProperties();
      var cols = new ColumnAttribute[props.Length];
      for (int i = 0; i < props.Length; i++) {
        cols[i] = props[i].GetCustomAttribute<ColumnAttribute>();
      }

      var enumRegEx = new Regex(@"[-_]", RegexOptions.Compiled);

      var list = new List<T>();

      foreach (var l in Csv.CsvReader.ReadFromStream(stream)) {
        T item = new T();
        for (int i = 0; i < props.Length; i++) {
          if (cols[i] == null)
            continue;

          string text = l[cols[i].Name];

          // ChangeType doesn't like Nullable types
          Type t = props[i].PropertyType;
          t = Nullable.GetUnderlyingType(t) ?? t;

          object val = null;
          if (t == typeof(bool)) {
            val = (text == "yes") || (text == "1");

          } else if (text == "") {
            var required = props[i].GetCustomAttribute<RequiredAttribute>();
            if ((required != null) && required.AllowEmptyStrings)
              val = text;

          } else if (t.BaseType == typeof(Enum)) {
            val = Enum.Parse(t, enumRegEx.Replace(text, ""), true);

          } else {
            val = Convert.ChangeType(text, t);

          }

          props[i].SetValue(item, val);
        }
        list.Add(item);
      }

      DbSet<T> set = GetDbSet<T>();
      set.RemoveRange(set.ToArray());
      set.AddRange(list);
      this.SaveChanges();
      //foreach (var o in list) {
      //  set.Add(o);
      //  SaveChanges();
      //}
    }
  }
}
