#if SQLITE
using SQLite.CodeFirst;
#endif
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
  /// <summary>
  /// The database context used for accessing the OurAirports data.
  /// </summary>
#if MYSQL
  [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
#endif
  public class DataContext : DbContext {
    /// <summary>
    /// Gets or sets the Airports data.
    /// </summary>
    public DbSet<Airport> Airports { get; set; }

    /// <summary>
    /// Gets or sets the Countries data.
    /// </summary>
    public DbSet<Country> Countries { get; set; }

    /// <summary>
    /// Gets or sets the Frequencies data.
    /// </summary>
    public DbSet<Frequency> Frequencies { get; set; }

    /// <summary>
    /// Gets or sets the NavAids data.
    /// </summary>
    public DbSet<NavAid> NavAids { get; set; }

    /// <summary>
    /// Gets or sets the Regions data.
    /// </summary>
    public DbSet<Region> Regions { get; set; }

    /// <summary>
    /// Gets or sets the Runways data.
    /// </summary>
    public DbSet<Runway> Runways { get; set; }

    /// <summary>
    /// Creates an instance of the DataContext class.
    /// </summary>
    public DataContext()
      : base() {
      Configuration.LazyLoadingEnabled = false;
    }

    /// <summary>
    /// This method is called when the model for a derived context has been initialized.
    /// </summary>
    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
#if SQLITE
      Database.SetInitializer(new SqliteDropCreateDatabaseWhenModelChanges<DataContext>(modelBuilder));
#elif (MYSQL || MSSQL)
      Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataContext>());
      // Make sure booleans are stored as bits
      modelBuilder.Properties()
            .Where(x => x.PropertyType == typeof(bool))
            .Configure(x => x.HasColumnType("bit"));
#endif

      base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Gets the DbSet for the specified type.
    /// </summary>
    protected DbSet<T> GetDbSet<T>() where T : class, new() {
      // Find the property whose DbSet matches the type passed
      foreach (var prop in this.GetType().GetProperties()) {
        if (prop.PropertyType == typeof(DbSet<T>)) {
          return (DbSet<T>)prop.GetValue(this);
        }
      }

      throw new ArgumentException("Type not found.", "T");
    }

    /// <summary>
    /// Clears the database and imports all the data.
    /// </summary>
    public void ImportAll(DirectoryInfo cacheDir = null) {
      Frequencies.RemoveRange(Frequencies.ToArray());
      Runways.RemoveRange(Runways.ToArray());
      NavAids.RemoveRange(NavAids.ToArray());
      Airports.RemoveRange(Airports.ToArray());
      Regions.RemoveRange(Regions.ToArray());
      Countries.RemoveRange(Countries.ToArray());
      SaveChanges();

      Import<Country>(cacheDir);
      Import<Region>(cacheDir);
      Import<Airport>(cacheDir);
      Import<Frequency>(cacheDir);
      Import<Runway>(cacheDir);
      Import<NavAid>(cacheDir);
      SaveChanges();
    }

    /// <summary>
    /// Imports data from OurAirports into the specified type.
    /// </summary>
    public void Import<T>(DirectoryInfo cacheDir = null) where T : class, new() {
      var uri = typeof(T).GetCustomAttribute<UriAttribute>().Uri;

      FileInfo localFile = null;
      DateTime? lastModified = null;
      if (cacheDir != null) {
        localFile = new FileInfo(Path.Combine(cacheDir.FullName, uri.Segments.Last()));
        if (localFile.Exists) {
          lastModified = localFile.LastWriteTimeUtc;
        }
      }

      var req = (HttpWebRequest)HttpWebRequest.Create(uri);
      if (lastModified != null) {
        req.IfModifiedSince = lastModified.Value;
      }

      HttpWebResponse resp = null;
      try {
        resp = (HttpWebResponse)req.GetResponse();
        using (var stream = resp.GetResponseStream()) {
          if (localFile == null) {
            Import<T>(stream);
            return;

          } else {
            using (var fs = localFile.OpenWrite()) {
              stream.CopyTo(fs);
            }

            if (resp.LastModified != DateTime.MinValue) {
              localFile.LastWriteTimeUtc = resp.LastModified.ToUniversalTime();
            }

            localFile.Refresh();

          }
        }

      } catch (WebException ex) {
        resp = (HttpWebResponse)(ex.Response);
        if (resp.StatusCode != HttpStatusCode.NotModified) {
          throw;
        }

      } finally {
        if (resp != null)
          resp.Dispose();

      }

      Import<T>(localFile);
    }

    /// <summary>
    /// Imports data from a local file into the specified type.
    /// </summary>
    public void Import<T>(FileInfo file) where T : class, new() {
      using (var fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read)) {
        Import<T>(fs);
      }
    }

    /// <summary>
    /// Imports data from OurAirports into the specified type.
    /// </summary>
    public void Import(string type, DirectoryInfo cacheDir = null) {
      switch (type) {
        case "Airport":
          Import<Airport>(cacheDir);
          break;
        case "Country":
          Import<Country>(cacheDir);
          break;
        case "Frequency":
          Import<Frequency>(cacheDir);
          break;
        case "NavAid":
          Import<NavAid>(cacheDir);
          break;
        case "Region":
          Import<Region>(cacheDir);
          break;
        case "Runway":
          Import<Runway>(cacheDir);
          break;
        default:
          throw new ArgumentException("'" + type + "' is not valid value.", "type");
      }
    }

    /// <summary>
    /// Imports data into the specified type.
    /// </summary>
    protected void Import<T>(Stream stream) where T : class, new() {
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
    }
  }
}
