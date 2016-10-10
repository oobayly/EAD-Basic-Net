using System;
using System.ComponentModel;
using System.Globalization;

namespace eu.bayly.EADBasicNet.OurAirports {
  public class EnumNameAttribute : Attribute {
    public string Name { get; set; }

    public EnumNameAttribute(string name) {
      this.Name = name;
    }
  }

  public class UriAttribute : Attribute {
    public Uri Uri { get; set; }

    public UriAttribute(string uri) : this(new Uri(uri)) {
    }

    public UriAttribute(Uri uri) {
      this.Uri = uri;
    }
  }

  public class NullIntConverter : TypeConverter {
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
      if ((value is string) && (value != null)) {
        try {

        } catch {
          return null;
        }
      }

      return base.ConvertFrom(context, culture, value);
    }
  }
}
